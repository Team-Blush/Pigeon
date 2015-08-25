define(['angularAMD', 'angularRouter'], function (angularAMD) {

    var app = angular.module('app', ['ngRoute']);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/', angularAMD.route({
                templateUrl: 'templates/home-view.html',
                controller: 'HomeController',
                controllerUrl: 'controllers/HomeController'
            }))
            .when('/login', angularAMD.route({
                templateUrl: 'templates/login-view.html',
                controller: 'LoginController',
                controllerUrl: 'controllers/LoginController'
            }))
            .when('/register', angularAMD.route({
                templateUrl: 'templates/register-view.html',
                controller: 'RegisterController',
                controllerUrl: 'controllers/RegisterController'
            }))
            .otherwise({ redirectTo: "/" });
    });

    return angularAMD.bootstrap(app);
});