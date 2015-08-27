define(['app', 'pigeonService', 'notifyService'],
    function (app) {
        app.controller('PigeonController', function ($scope, pigeonService) {
            $scope.pigeonData = {};

            $scope.createPigeon = function () {
                var pigeonData = $scope.pigeonData;
                pigeonService.createPigeon(pigeonData).then(
                        function (serverData) {
                            console.log(serverData);
                        },
                        function (serverError) {
                            console.error(serverError);
                        }
                    );
            }
        });
    }
);

