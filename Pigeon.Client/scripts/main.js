requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // libs
        angular: 'libs/angular',
        angularRouter: 'libs/angular-route',
        angularAMD: 'libs/angularAMD.min',
        bootstrap: 'libs/bootstrap',
        jquery: 'libs/jquery-1.9.1',
        noty: 'libs/jquery.noty',

        // controllers
        HeaderController: 'controllers/HeaderController',
        HomeController: 'controllers/HomeController',
        RegisterController: 'controllers/RegisterController',
        LoginController: 'controllers/LoginController',

        // services
        accountService: 'services/accountService',
        
        navigationService: 'services/navigationService',
        notifyService: 'services/notifyService',
        requestService: 'services/requestService',
        validationService: 'services/validationService',

        // constants
        constants: 'constants'
    },

    shim: {
        angularRouter: ['angular'],
        angularAMD: ['angular'],
        bootstrap: ['jquery'],
        noty: ['jquery']
    }
});

requirejs(['app', 'jquery', 'bootstrap']);