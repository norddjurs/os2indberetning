﻿using System.Collections.Generic;
using System.Linq;
using Core.DomainModel;
using Core.DomainServices;

namespace FileGenerator
{
    public class ReportGenerator
    {
        private readonly IGenericRepository<DriveReport> _reportRepo;
        private readonly IReportFileWriter _fileWriter;
        
        public ReportGenerator(IGenericRepository<DriveReport> reportRepo, IReportFileWriter fileWriter)
        {
            _reportRepo = reportRepo;
            _fileWriter = fileWriter;
        }

        public void WriteRecordsToFileAndAlterReportStatus()
        {
            var usersToReimburse = GetUsersAndReportsToReimburse();
            var records = RecordListBuilder(usersToReimburse);

            _fileWriter.WriteRecordsToFile(records);

            foreach (var reports in usersToReimburse.Values)
            {
                foreach (var report in reports)
                {
                    report.Status = ReportStatus.Invoiced;
                }
            }
            _reportRepo.Save();
        }

        private Dictionary<string, List<DriveReport>> GetUsersAndReportsToReimburse()
        {
            var reportsToReimburse = GetDriveReportsToReimburse();
            var reportsPerUser = new Dictionary<string, List<DriveReport>>();

            foreach (var report in reportsToReimburse)
            {
                var cpr = report.Person.CprNumber;
                if (!reportsPerUser.ContainsKey(cpr))
                {
                    reportsPerUser.Add(cpr, new List<DriveReport>());
                }
                reportsPerUser[cpr].Add(report);                
            }

            return reportsPerUser;
        }

        private IEnumerable<DriveReport> GetDriveReportsToReimburse()
        {
            return _reportRepo.AsQueryable().Where(r => r.Status == ReportStatus.Accepted).ToList();
        }

        private static List<FileRecord> RecordListBuilder(Dictionary<string, List<DriveReport>> usersToReimburse)
        {
            return (from pair in usersToReimburse from report in pair.Value select new FileRecord(report, pair.Key)).ToList();
        }
    }
}
