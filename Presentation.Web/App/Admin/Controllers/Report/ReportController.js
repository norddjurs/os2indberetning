﻿angular.module('application').controller('ReportController', [
    "$scope", "$rootScope", "$window", "$state", "Person","Autocomplete", "OrgUnit",
    function ($scope, $rootScope, $window, $state, Person, Autocomplete, OrgUnit) {

        $scope.container = {};
        $scope.persons = Autocomplete.allUsers();
        $scope.orgUnits = Autocomplete.orgUnits();

        $scope.autoCompleteOptions = {
            filter: "contains"
        };

        $scope.createReportClick = function () {

            var url = $state.href('document');

            $scope.$broadcast('createReportClicked');
            
            if ($scope.container.employeeFilter != undefined && $scope.container.reportFromDateString != undefined && $scope.container.reportToDateString != undefined) {
            
                $window.open(url + '?Employee=' + $scope.container.employeeFilter + '&manr=' + $scope.container.MANrFilter + '&from= ' + $scope.container.reportFromDateString + '&to=' + $scope.container.reportToDateString + "&orgUnit=" + $scope.container.orgUnitFilter, '_blank');
            } else {

                alert('Du mangler at udfylde et felt med en *');
            }
            
        }


    }
]);
