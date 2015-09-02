define(['app', 'HeaderController', 'PigeonController', 'accountService', 'validationService', 'navigationService', 'notifyService', 'ngPictureSelect'],
    function (app) {
        app.controller('ProfileController', function ($scope, $rootScope, accountService, validationService, navigationService, notifyService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.changePasswordData = {};
            $scope.title = $scope.myData.username + ' - Profile';

            $scope.editProfile = function () {
                var myData = $scope.myData;
                if (validationService.validateEditProfileForm(myData.firstName, myData.lastName, myData.email, myData.age)) {
                    myData = validationService.escapeHtmlSpecialChars(myData);
                    accountService.editProfile(myData).then(
                        function (serverResponse) {
                            $rootScope.$broadcast('myDataUpdate');
                            navigationService.loadHome();
                            notifyService.showInfo(serverResponse.message);
                        },
                        function (serverError) {
                            notifyService.showError("Unsuccessful edit profile", serverError);
                        }
                    );
                }
            };

            $scope.changePassword = function () {
                var changePasswordData = $scope.changePasswordData;
                if (validationService.validateChangePasswordForm(changePasswordData.newPassword, changePasswordData.confirmPassword)) {
                    changePasswordData = validationService.escapeHtmlSpecialChars(changePasswordData);
                    accountService.changePassword(changePasswordData).then(
                        function (serverResponse) {
                            notifyService.showInfo(serverResponse.message);
                            navigationService.loadHome();
                        },
                        function (serverError) {
                            notifyService.showError("Unsuccessful change password!", serverError);
                        }
                    );
                }
            };

        });
    }
);