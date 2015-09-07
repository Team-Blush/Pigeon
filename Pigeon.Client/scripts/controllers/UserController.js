define(['app', 'constants', 'HeaderController', 'accountService', 'pigeonService', 'commentService', 'notifyService', 'ngPictureSelect'],
    function (app) {
        app.controller('UserController', function ($scope, $routeParams, constants, accountService, pigeonService, commentService, notifyService) {
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
                    serverData.profilePhotoData = serverData.profilePhotoData ? serverData.profilePhotoData : constants.profilePhotoData;
                    serverData.coverPhotoData = serverData.coverPhotoData ? serverData.coverPhotoData : constants.coverPhotoData;
                    $scope.userData = serverData;
                },
                function (serverError) {
                    console.error(serverError);
                }
            );

            $scope.follow = function() {
                accountService.follow($scope.userData.username).then(
                    function (serverData) {
                        $scope.userData.isFollowed = true;
                        notifyService.showInfo(serverData.message);
                    },
                    function(serverError) {
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
                        console.log(serverData);
                        serverData.forEach(function (pigeon) {
                            parsePigeonDate(pigeon);
                        });
                        $scope.pigeonsData = serverData;
                    },
                    function (serverError) {
                        console.error(serverError);
                    });
            }

            $scope.favourite = function (pigeon) {
                pigeonService.favourite(pigeon.id).then(
                    function (serverData) {
                        pigeon.favouritedCount++;
                        pigeon.favourited = true;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
            }

            $scope.unfavourite = function (pigeon) {
                pigeonService.unfavourite(pigeon.id).then(
                    function (serverData) {
                        pigeon.favouritedCount--;
                        pigeon.favourited = false;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
            }

            $scope.voteUp = function (pigeon) {
                var vote = {}
                vote.value = 1;
                pigeonService.vote(pigeon.id, vote).then(
                    function (serverData) {
                        if (pigeon.voted === 0) {
                            pigeon.upVotesCount++;
                        } else {
                            pigeon.upVotesCount++;
                            pigeon.downVotesCount--;
                        }
                        pigeon.voted = 1;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
            }

            $scope.voteDown = function (pigeon) {
                var vote = {}
                vote.value = -1;
                pigeonService.vote(pigeon.id, vote).then(
                    function (serverData) {
                        if (pigeon.voted === 0) {
                            pigeon.downVotesCount++;
                        } else {
                            pigeon.downVotesCount++;
                            pigeon.upVotesCount--;
                        }
                        pigeon.voted = -1;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        console.log(serverError);
                    }
                );
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
                        serverData.forEach(function (comment) {
                            comment.author.profilePhotoData = comment.author.profilePhotoData ? comment.author.profilePhotoData : constants.profilePhotoData;
                        });
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