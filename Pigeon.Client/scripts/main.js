requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // libs
        angular: 'libs/angular',
        angularRouter: 'libs/angular-route',
        angularAMD: 'libs/angularAMD.min',
        bootstrap: 'libs/bootstrap',
        jquery: 'libs/jquery-1.9.1',
        noty: 'libs/jquery.noty.packaged',

        // controllers
        HeaderController: 'controllers/HeaderController',
        HomeController: 'controllers/HomeController',
        RegisterController: 'controllers/RegisterController',
        LoginController: 'controllers/LoginController',
        UserController: 'controllers/UserController',
        ProfileController: 'controllers/ProfileController',

        // services
        accountService: 'services/accountService',
        pigeonService: 'services/pigeonService',
        commentService: 'services/commentService',
        
        fileReaderService: 'services/fileReaderService',
        navigationService: 'services/navigationService',
        notifyService: 'services/notifyService',
        requestService: 'services/requestService',
        validationService: 'services/validationService',

        // directives
        ngPictureSelect: 'directives/ngPictureSelect',

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