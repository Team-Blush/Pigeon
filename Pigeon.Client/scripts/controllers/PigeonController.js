define(['app', 'pigeonService', 'notifyService'],
    function (app) {
        app.controller('PigeonController', function ($scope, pigeonService) {
            $scope.isCreatePigeonExpanded = false;
            $scope.isCommentsExpanded = false;

            $scope.expandComments = function (pigeonId) {
                $scope.expandCommentsPigeonId = pigeonId;
                $scope.isCommentsExpanded = !$scope.isCommentsExpanded;
            }

            $scope.expandCreatePigeon = function () {
                $scope.isCreatePigeonExpanded = !$scope.isCreatePigeonExpanded;
            }
        });
    }
);