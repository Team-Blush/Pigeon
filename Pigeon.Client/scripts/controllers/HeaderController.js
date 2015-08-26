define(['app', 'accountService', 'navigationService', 'notifyService'],
    function (app) {
        app.controller('HeaderController', function ($scope, accountService,
                                                        navigationService, notifyService) {
            $scope.isLoggedIn = accountService.isLoggedIn();

            $scope.logout = function () {
                accountService.logout().then(
                    function (serverResponse) {
                        notifyService.showInfo("Logout Successful.");
                        accountService.clearCredentials();
                        navigationService.loadHome();
                        navigationService.reload();
                    },
                    function (serverError) {
                        notifyService.showError("Unsuccessful Logout!", serverError);
                    }
                );
            };
        });
    }
);
