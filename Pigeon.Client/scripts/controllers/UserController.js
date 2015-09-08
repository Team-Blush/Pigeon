define(['app', 'constants', 'HeaderController', 'PigeonController', 'accountService', 'notifyService', 'ngPictureSelect'],
    function (app) {
        app.controller('UserController', function ($scope, $routeParams, constants, accountService, notifyService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.userData = {};
            $scope.userData.username = $routeParams.username;

            accountService.loadUserFullData($scope.userData.username).then(
                function (serverData) {
                    serverData.profilePhotoData = serverData.profilePhotoData ? serverData.profilePhotoData : constants.profilePhotoData;
                    serverData.coverPhotoData = serverData.coverPhotoData ? serverData.coverPhotoData : constants.coverPhotoData;
                    $scope.userData = serverData;
                },
                function (serverError) {
                    console.error(serverError);
                }
            );

            $scope.follow = function () {
                accountService.follow($scope.userData.username).then(
                    function (serverData) {
                        $scope.userData.isFollowed = true;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
            }

            $scope.unfollow = function () {
                accountService.unfollow($scope.userData.username).then(
                    function (serverData) {
                        $scope.userData.isFollowed = false;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
            }
        });
    }
);