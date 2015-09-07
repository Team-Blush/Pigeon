define(['app', 'constants', 'HeaderController', 'accountService', 'ngPictureSelect'],
    function (app) {
        app.controller('HomeController', function ($scope, $rootScope, constants, accountService) {
            $scope.title = 'Welcome to Pigeon';
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.isCommentsExpanded = false;
            $scope.myData = accountService.loadMyData();
            $scope.isSearchExpanded = false;
            $scope.searchData = {};
            $scope.people = [];

            $scope.expandSearch = function() {
                $scope.isSearchExpanded = !$scope.isSearchExpanded;
            }

            $scope.search = function () {
                var searchTerm = $scope.searchData.searchTerm;
                accountService.search(searchTerm).then(
                    function (serverData) {
                        serverData.forEach(function (person) {
                            person.profilePhotoData = person.profilePhotoData ? person.profilePhotoData : constants.profilePhotoData;
                            person.firstName = person.firstName ? person.firstName : '';
                            person.lastName = person.lastName ? person.lastName : '';
                        });

                        $scope.people = serverData;
                    },
                    function(serverError) {
                        console.error(serverError);
                    }
                );
            }

            $rootScope.$on('myDataUpdate', function () {
                $rootScope.myDataUpdate = true;
            });

            if ($rootScope.myDataUpdate) {
                accountService.getMe().then(
                    function (data) {
                        accountService.saveMyData(data);
                        $rootScope.myDataUpdate = false;
                        $scope.myData = accountService.loadMyData();
                        $scope.title = $scope.myData.username + ' - ' + $scope.title;
                    },
                    function (error) {
                        console.error(error);
                    });
            } else {
                $scope.myData = accountService.loadMyData();
                if ($scope.isLoggedIn) {
                    $scope.title = $scope.myData.username + ' - ' + $scope.title;
                }
            }
        });
    }
);