define(['app', 'accountService'],
    function (app) {
        app.controller('HomeController', function ($scope, accountService) {
            $scope.title = 'Welcome to Pigeon';
            $scope.isLoggedIn = accountService.isLoggedIn();
        });
    }
);