﻿module app.core.models {
    "use strict";

    export enum ReportStatus {
        Pending,
        Accepted,
        Rejected,
        Invoiced
    }

    export class Report {
        Id: number;
        Status: ReportStatus;
        CreatedDateTimestamp: number;
        EditedDateTimestamp: number;
        Comment: string;
        ClosedDateTimestamp: number;
        ProcessedDateTimestamp: number;
        ApprovedBy: Person;
        PersonId: number;
        Person: Person;
        EmploymentId: number;
        Employment: Employment;
        ResponsibleLeaderId: number;
        ResponsibleLeader: Person;
        ActualLeaderId: number;

    }
}