var app;
(function (app) {
    "use strict";
    var routes = (function () {
        function routes() {
        }
        routes.init = function ($stateProvider, $urlRouterProvider, EnabledApplications) {
            var isDriveEnabled = EnabledApplications.toLowerCase().indexOf("drive") > -1;
            var isVacationEnabled = EnabledApplications.toLowerCase().indexOf("vacation") > -1;
            $stateProvider.
                state('default', {
                url: '/',
                templateUrl: '/App/app.html',
                controller: [
                    '$scope', '$state', function ($scope, $state) {
                        if (isDriveEnabled && isVacationEnabled) {
                            $scope.handledState = true;
                            return;
                        }
                        if (isDriveEnabled)
                            $state.go('drive');
                        if (isVacationEnabled)
                            $state.go('vacation');
                    }
                ]
            });
            if (isDriveEnabled) {
                $stateProvider.state('drive', {
                    url: '/drive',
                    templateUrl: '/App/Drive/app.drive.html',
                    controller: [
                        '$scope', '$state', function ($scope, $state) {
                            $state.go('drive.driving');
                        }
                    ]
                });
            }
            if (isVacationEnabled) {
                $stateProvider.state('vacation', {
                    url: '/vacation',
                    templateUrl: '/App/Vacation/app.vacation.html',
                    controller: [
                        '$scope', '$state', function ($scope, $state) {
                            $state.go('vacation.approvereports');
                        }
                    ]
                });
            }
            $urlRouterProvider.otherwise("/");
            $urlRouterProvider.rule(function ($injector, $location) {
                var path = $location.path();
                var hasTrailingSlash = path[path.length - 1] === "/";
                if (hasTrailingSlash) {
                    var newPath = path.substr(0, path.length - 1);
                    return newPath;
                }
                return path;
            });
        };
        routes.$inject = ["$stateProvider", "$urlRouterProvider", "EnabledApplications"];
        return routes;
    })();
    app.routes = routes;
    angular.module("app").config(routes.init);
})(app || (app = {}));
