define(['app', 'pigeonService', 'notifyService'],
    function (app) {
        app.controller('PigeonController', function ($scope, pigeonService) {
            $scope.isCreatePigeonExpanded = false;
            $scope.pigeonData = {};
            $scope.pigeonsData = [];
            $scope.isCommentsExpanded = false;

            $scope.expandComments = function (pigeonId) {
                $scope.expandCommentsPigeonId = pigeonId;
                $scope.isCommentsExpanded = !$scope.isCommentsExpanded;
            }

            $scope.expandCreatePigeon = function () {
                $scope.isCreatePigeonExpanded = !$scope.isCreatePigeonExpanded;
            }

            $scope.createPigeon = function () {
                var pigeonData = $scope.pigeonData;
                pigeonService.createPigeon(pigeonData).then(
                        function (serverData) {
                            $scope.pigeonsData.unshift(serverData);
                        },
                        function (serverError) {
                            console.error(serverError);
                        }
                    );
            }

            $scope.loadOwnPigeons = function () {
                pigeonService.loadOwnPigeons().then(
                    function (serverResponse) {
                        serverResponse.forEach(function (pigeon) {
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
                        });
                        console.log(serverResponse);
                        $scope.pigeonsData = serverResponse;
                    },
                    function (serverError) {
                        console.error(serverError);
                    });
            }
        });
    }
);

