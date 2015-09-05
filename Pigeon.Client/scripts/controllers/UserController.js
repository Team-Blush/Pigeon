define(['app', 'HeaderController', 'PigeonController', 'accountService', 'pigeonService', 'ngPictureSelect'],
    function (app) {
        app.controller('UserController', function ($scope, $routeParams, accountService, pigeonService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.userData = {};
            $scope.pigeonData = {};
            $scope.pigeonsData = [];
            $scope.userData.username = $routeParams.username;

            accountService.loadUserFullData($scope.userData.username).then(
                function (serverData) {
                    $scope.userData = serverData;
                },
                function (serverError) {
                    console.error(serverError);
                }
            );

            function parsePigeonDate(pigeon) {
                var date = new Date(pigeon.createdOn);
                var months = {
                    1: 'January',
                    2: 'February',
                    3: 'March',
                    4: 'April',
                    5: 'May',
                    6: 'June',
                    7: 'July',
                    8: 'August',
                    9: 'September',
                    10: 'October',
                    11: 'November',
                    12: 'December'
                }
                var day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                var month = months[date.getMonth() + 1];
                var year = date.getFullYear();

                pigeon.createdOn = day + ' ' + month + ' ' + year;
            }

            $scope.createPigeon = function () {
                var pigeonData = $scope.pigeonData;
                pigeonService.createPigeon(pigeonData).then(
                    function (serverData) {
                        parsePigeonDate(serverData);
                        $scope.pigeonsData.unshift(serverData);
                        $scope.isCreatePigeonExpanded = false;
                    },
                    function (serverError) {
                        console.error(serverError);
                    });
            }

            $scope.loadUserPigeons = function () {
                pigeonService.loadUserPigeons($scope.userData.username).then(
                    function (serverResponse) {
                        serverResponse.forEach(function (pigeon) {
                            parsePigeonDate(pigeon);
                        });
                        $scope.pigeonsData = serverResponse;
                    },
                    function (serverError) {
                        console.error(serverError);
                    });
            }
        });
    }
);