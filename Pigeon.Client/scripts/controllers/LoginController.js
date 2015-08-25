define(['app', 'constants', 'validationService', 'accountService', 'navigationService', 'notifyService'],
    function (app) {
        app.controller('LoginController', function ($scope, constants, validationService, accountService,
                                                    navigationService, notifyService) {
            $scope.title = 'Pigeon - Login';
            $scope.loginData = {};
            $scope.isLoggedIn = accountService.isLoggedIn();

            $scope.login = function () {
                var loginData = $scope.loginData;
                if (validationService.validateLoginForm(loginData.username, loginData.password)) {
                    loginData = validationService.escapeHtmlSpecialChars(loginData);
                    loginData["grant_type"] = constants.grantType;
                    accountService.login(loginData).then(
                        function (serverResponse) {
                            accountService.setCredentials(serverResponse);
                            navigationService.loadHome();
                            notifyService.showInfo("Log In successful.");
                        },
                        function (serverError) {
                            notifyService.showError("Unsuccessful Log In!", serverError);
                        }
                    );
                }
            };
        });
    }
);