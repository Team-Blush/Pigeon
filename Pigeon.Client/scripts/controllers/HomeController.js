define(['app', 'HeaderController', 'PigeonController', 'accountService', 'ngPictureSelect'],
    function (app) {
        app.controller('HomeController', function ($scope, $rootScope, accountService) {
            $scope.title = 'Welcome to Pigeon';
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.isCommentsExpanded = false;
            $scope.myData = accountService.loadMyData();

            $rootScope.$on('myDataUpdate', function () {
                $rootScope.myDataUpdate = true;
            });

            if ($rootScope.myDataUpdate) {
                accountService.getMe().then(
                    function (data) {
                        accountService.saveMyData(data);
                        $rootScope.myDataUpdate = false;
                        $scope.myData = accountService.loadMyData();
                        $scope.title = $scope.myData.userName + ' - ' + $scope.title;
                    },
                    function (error) {
                        console.error(error);
                    });
            } else {
                $scope.myData = accountService.loadMyData();
                if ($scope.isLoggedIn) {
                    $scope.title = $scope.myData.userName + ' - ' + $scope.title;
                }
            }
        });
    }
);