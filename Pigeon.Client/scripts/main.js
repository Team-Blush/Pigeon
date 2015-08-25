requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // libs
        angular: 'libs/angular',
        angularRouter: 'libs/angular-route',
        angularAMD: 'libs/angularAMD.min',
        bootstrap: 'libs/bootstrap',
        jquery: 'libs/jquery-1.9.1',
        noty: 'libs/noty/jquery.noty',

        // controllers
        HomeController: 'controllers/HomeController'

    },

    shim: {
        angularRouter: ['angular'],
        angularAMD: ['angular'],
        bootstrap: ['jquery'],
        noty: ['jquery']
    }
});

requirejs(['app', 'jquery', 'bootstrap']);