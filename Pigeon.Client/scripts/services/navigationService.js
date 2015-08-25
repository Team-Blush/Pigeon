define(['app'], function (app) {
    app.factory('navigationService', function($location, $route) {
        var service = {};

        service.loadHome = function() {
            $location.path('/');

        };

        service.reload = function() {
            $route.reload();
        };

        return service;
    });
});
