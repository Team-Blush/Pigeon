define(['app', 'constants', 'validationService', 'accountService', 'navigationService', 'notifyService'],
    function (app) {
        app.controller('LoginController', function ($scope, $rootScope, constants, validationService, accountService,
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
                            $rootScope.$broadcast('myDataUpdate');
                            accountService.setCredentials(serverResponse);
                            navigationService.loadHome();
                            notifyService.showInfo("Login successful.");
                        },
                        function (serverError) {
                            notifyService.showError("Unsuccessful Login!", serverError);
                        }
                    );
                }
            };
        });
    }
);