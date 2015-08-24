define(['angularAMD', 'angularRouter'], function (angularAMD) {

    var app = angular.module('app', ['ngRoute']);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/', angularAMD.route({
                templateUrl: 'templates/home-view.html',
                controller: 'HomeController',
                controllerUrl: 'controllers/HomeController'
            }))
            .otherwise({ redirectTo: "/" });
    });

    return angularAMD.bootstrap(app);
});