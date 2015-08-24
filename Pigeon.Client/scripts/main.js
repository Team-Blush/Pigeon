requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // libs
        angular: 'libs/angular',
        angularRouter: 'libs/angular-route',
        angularAMD: 'libs/angularAMD.min',
        jquery: 'libs/jquery-1.9.1.intellisense',
        noty: 'libs/noty/jquery.noty',

        // controllers
        HomeController: 'controllers/HomeController'

    },

    shim: {
        noty: ['jquery'],
        angular: ['jquery'],
        angularAMD: ['angular'],
        angularRouter: ['angular']
    }
});

requirejs(['app']);