﻿
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Core.DomainModel;
using Core.DomainModel.Example;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OS2Indberetning
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.CreateKernel());

            config.MapHttpAttributeRoutes();

            config.AddODataQueryFilter();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MapODataServiceRoute(
                routeName: "odata",
                routePrefix: "odata",
                model: GetModel()
                );

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            
        }

        public static Microsoft.OData.Edm.IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<TestReport>("TestReports");
            //var test = builder.EntityType<TestReport>();
            //test.Ignore(report => report.DateTimeTest);

            builder.EntitySet<Address>("Addresses");

            builder.EntitySet<DriveReport>("DriveReports");

            builder.EntitySet<DriveReportPoint>("DriveReportPoints");

            builder.EntitySet<Employment>("Employments");
            var eType = builder.EntityType<Employment>();
            eType.HasKey(e => e.Id);

            builder.EntitySet<FileGenerationSchedule>("FileGenerationSchedules");
            
            var lType = builder.EntityType<LicensePlate>();
            lType.Ignore(l => l.Person);
            builder.EntitySet<LicensePlate>("LicensePlates");

            builder.EntitySet<MailNotificationSchedule>("MailNotificationSchedules");

            builder.EntitySet<MobileToken>("MobileToken");

            builder.EntitySet<OrgUnit>("OrgUnits");

            builder.EntitySet<Person>("Person");
            var pType = builder.EntityType<Person>();
            pType.HasKey(p => p.Id);
            pType.Ignore(p => p.LicensePlates);            

            builder.EntitySet<PersonalAddress>("PersonalAddresses");

            builder.EntitySet<PersonalRoute>("PersonalRoutes");

            builder.EntitySet<Point>("Points");

            builder.EntitySet<Rate>("Rates");

            builder.EntitySet<Report>("Reports");

            builder.EntitySet<Substitute>("Substitutes");

            builder.EntitySet<BankAccount>("BankAccounts");

            return builder.GetEdmModel();
        }
    }
}
