﻿module app.vacation {
    "use strict";

    import Person = core.models.Person;

    class MyApprovedVacationReportsController extends BaseMyVacationReportsController {

        static $inject: string[] = [
            "$scope",
            "$modal",
            "$rootScope",
            "VacationReport",
            "$timeout",
            "Person",
            "moment",
            "$state"
        ];

        constructor(
            protected $scope,
            protected $modal: angular.ui.bootstrap.IModalService,
            protected $rootScope,
            protected VacationReport,
            protected $timeout: ng.ITimeoutService,
            protected Person: Person,
            protected moment: moment.MomentStatic,
            protected $state: ng.ui.IStateService) {

            super($scope, $modal, $rootScope, VacationReport, $timeout, Person, moment, $state);

            this.vacationReportsOptions = {
                autoBind: false,
                dataSource: {
                    type: "odata-v4",
                    transport: {
                        read: {
                            url: this.getVacationReportsUrl(),
                            dataType: "json",
                            cache: false
                        }
                    },
                    pageSize: 20,
                    serverPaging: true,
                    serverAggregates: false,
                    serverSorting: true,
                    serverFiltering: true,
                    sort: { field: "StartTimestamp", dir: "desc" }
                },
                pageable: {
                    messages: {
                        display: "{0} - {1} af {2} indberetninger",
                        //{0} is the index of the first record on the page, {1} - index of the last record on the page, {2} is the total amount of records
                        empty: "Ingen indberetninger at vise",
                        page: "Side",
                        of: "af {0}", //{0} is total amount of pages
                        itemsPerPage: "indberetninger pr. side",
                        first: "Gå til første side",
                        previous: "Gå til forrige side",
                        next: "Gå til næste side",
                        last: "Gå til sidste side",
                        refresh: "Genopfrisk"
                    },
                    pageSizes: [5, 10, 20, 30, 40, 50, 100, 150, 200]
                },
                dataBound: function () {
                    this.expandRow(this.tbody.find("tr.k-master-row").first());
                },
                columns: [
                    {
                        title: "Feriestart",
                        template: data => {
                            const m = this.moment.utc(data.StartTimestamp, 'X');
                            return m.format("L");
                        }
                    },
                    {
                        title: "Ferieafslutning",
                        template: data => {
                            const m = this.moment.utc(data.EndTimestamp, 'X');
                            return m.format("L");
                        }
                    },
                    {
                        template: data => {
                            if (data.Comment != "") {
                                return `<button kendo-tooltip k-position="'right'" k-content="'${data.Comment}'" class="transparent-background pull-right no-border">
                                        <i class="fa fa-comment-o"></i></button>`;
                            }
                            return "<i>Ingen kommantar angivet</i>";
                        },
                        title: "Kommentar"
                    },
                    {
                        title: "Ferietype",
                        template: data => {
                            return data.VacationType === "Regular" ?
                                "Almindelig ferie" : "6. ferieuge";
                        }
                    },
                    {
                        field: "CreatedDateTimestamp",
                        template: data => {
                            var m = this.moment.utc(data.CreatedDateTimestamp, 'X');
                            return m.format("L");
                        },
                        title: "Indberettet"
                    },
                    {
                        field: "ClosedDateTimestamp",
                        title: "Godkendelsesdato",
                        template: data => {
                            var m = this.moment.utc(data.ClosedDateTimestamp, 'X');
                            return m.format("L");
                        }
                    },
                    {
                        field: "ProcessedDateTimestamp",
                        title: "Afsendt til løn",
                        template: data => {
                            if (data.ProcessedDateTimestamp != 0 && data.ProcessedDateTimestamp != null && data.ProcessedDateTimestamp != undefined) {
                                const m = this.moment.utc(data.ProcessedDateTimestamp, 'X');
                                return m.format("L");
                            }
                            return "";
                        }
                    },
                    {
                        field: "ApprovedBy.FullName",
                        title: "Godkendt af"
                    }
                ],
                scrollable: false
            };
        }

        getVacationReportsUrl() {
            return `/odata/VacationReports?status=Accepted&$expand=ResponsibleLeader,ApprovedBy &$filter=PersonId eq ${this.personId}`;
        }
    }

    angular.module("app.vacation").controller("MyApprovedVacationReportsController", MyApprovedVacationReportsController);
}
