define(['app', 'HeaderController', 'accountService', 'pigeonService', 'commentService', 'ngPictureSelect'],
    function (app) {
        app.controller('UserController', function ($scope, $routeParams, accountService, pigeonService, commentService) {
            $scope.isLoggedIn = accountService.isLoggedIn();
            $scope.myData = accountService.loadMyData();
            $scope.userData = {};
            $scope.pigeonData = {};
            $scope.pigeonsData = [];
            $scope.commentData = {};
            $scope.editCommentData = {};
            $scope.userData.username = $routeParams.username;
            $scope.isCreatePigeonExpanded = false;
            $scope.isCommentsExpanded = false;
            $scope.isEditCommentExpanded = false;

            $scope.expandCreatePigeon = function () {
                $scope.isCreatePigeonExpanded = !$scope.isCreatePigeonExpanded;
            }

            $scope.expandComments = function (pigeonId) {
                $scope.expandCommentsPigeonId = pigeonId;
                $scope.isCommentsExpanded = !$scope.isCommentsExpanded;
            }

            $scope.expandEditComment = function (comment) {
                $scope.expandEditCommentsCommentId = comment.id;
                $scope.editCommentData.content = comment.content;
                $scope.isEditCommentExpanded = !$scope.isEditCommentExpanded;
            }

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
                    function (serverData) {
                        serverData.forEach(function (pigeon) {
                            parsePigeonDate(pigeon);
                        });
                        $scope.pigeonsData = serverData;
                    },
                    function (serverError) {
                        console.error(serverError);
                    });
            }

            $scope.createComment = function (pigeon) {
                commentService.createComment(pigeon.id, $scope.commentData).then(
                    function (serverData) {
                        pigeon.comments.push(serverData);
                        $scope.commentData = {};
                    },
                    function (serverError) {
                        console.error(serverError);
                    }
                );
            }

            $scope.loadPigeonComments = function (pigeon) {
                commentService.loadPigeonComments(pigeon.id).then(
                    function (serverData) {
                        pigeon.comments = serverData;
                    },
                    function (serverError) {
                        console.error(serverError);
                    }
                );
            }
            
            $scope.editComment = function (pigeon, comment) {
                commentService.editComment(pigeon.id, comment.id, $scope.editCommentData).then(
                    function (serverData) {
                        comment.content = serverData.content;
                        $scope.isEditCommentExpanded = false;
                    },
                    function (serverError) {
                        console.error(serverError);
                    }
                );
            }

            $scope.deleteComment = function (pigeon, comment) {
                commentService.deleteComment(pigeon.id, comment.id).then(
                    function (serverData) {
                        pigeon.comments.pop(comment);
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