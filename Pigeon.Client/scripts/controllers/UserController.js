define(['app', 'HeaderController', 'PigeonController', 'accountService', 'ngPictureSelect'],
    function (app) {
        app.controller('UserController', function ($scope, $routeParams, accountService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.userData = {};
            $scope.userData.username = $routeParams.username;

            accountService.loadUserFullData($scope.userData.username).then(
                function (serverData) {
                    $scope.userData = serverData;
                },
                function (serverError) {
                    console.error(serverError);

                }
            );
            
        });
    }
);