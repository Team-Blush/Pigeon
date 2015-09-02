define(['app', 'HeaderController', 'PigeonController', 'accountService', 'ngPictureSelect'],
    function (app) {
        app.controller('ProfileController', function ($scope, accountService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.title = $scope.myData.username + ' - Profile';


            
        });
    }
);