define(['app', 'validationService', 'accountService', 'navigationService', 'notifyService'],
    function (app) {
        app.controller('RegisterController', function ($scope, $rootScope, validationService, accountService,
                                                     navigationService, notifyService) {
            $scope.title = 'Pigeon - Register';
            $scope.registerData = {};
            $scope.isLoggedIn = accountService.isLoggedIn();

            $scope.register = function () {
                var registerData = $scope.registerData;
                if (validationService.validateRegisterForm(registerData.username, registerData.email,
                        registerData.password, registerData.confirmPassword, registerData.name)) {
                    registerData = validationService.escapeHtmlSpecialChars(registerData);
                    accountService.register(registerData).then(
                        function (serverResponse) {
                            $rootScope.$broadcast('myDataUpdate');
                            accountService.setCredentials(serverResponse);
                            navigationService.loadHome();
                            notifyService.showInfo("Register successful.");
                        },
                        function (serverError) {
                            notifyService.showError("Unsuccessful Registration!", serverError);
                        }
                    );
                }
            };
        });
    }
);