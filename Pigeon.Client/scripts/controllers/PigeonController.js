define(['app', 'constants', 'pigeonService', 'commentService', 'validationService', 'notifyService', 'ngPictureSelect'],
    function (app) {
        app.controller('PigeonController', function ($scope, constants, pigeonService, commentService, validationService, notifyService) {
            $scope.pigeonData = {};
            $scope.pigeonsData = [];
            $scope.editPigeonData = {};
            $scope.commentData = {};
            $scope.editCommentData = {};
            $scope.isCreatePigeonExpanded = false;
            $scope.isEditPigeonExpanded = false;
            $scope.isCreateCommentExpanded = false;
            $scope.isEditCommentExpanded = false;
            $scope.isDeletePigeonExpanded = false;

            $scope.expandCreatePigeon = function () {
                $scope.isCreatePigeonExpanded = !$scope.isCreatePigeonExpanded;
            }

            $scope.expandEditPigeon = function (pigeonId) {
                $scope.expandEditPigeonsPigeonId = pigeonId;
                $scope.isEditPigeonExpanded = !$scope.isEditPigeonExpanded;
            }

            $scope.expandDeletePigeon = function (pigeonId) {
                $scope.expandDeletePigeonsPigeonId = pigeonId;
                $scope.isDeletePigeonExpanded = !$scope.isDeletePigeonExpanded;
            }

            $scope.hideDeletePigeon = function() {
                $scope.isDeletePigeonExpanded = false;
            }

            $scope.expandCreateComment = function (pigeonId) {
                $scope.expandCommentsPigeonId = pigeonId;
                $scope.isCreateCommentExpanded = !$scope.isCreateCommentExpanded;
            }

            $scope.expandEditComment = function (comment) {
                $scope.expandEditCommentsCommentId = comment.id;
                $scope.editCommentData.content = comment.content;
                $scope.isEditCommentExpanded = !$scope.isEditCommentExpanded;
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
                if (validationService.validatePigeon(pigeonData)) {
                    pigeonService.createPigeon(pigeonData).then(
                        function (serverData) {
                            parsePigeonDate(serverData);
                            $scope.pigeonsData.unshift(serverData);
                            $scope.isCreatePigeonExpanded = false;
                            $scope.pigeonData = {};
                        },
                        function (serverError) {
                            console.error(serverError);
                        }
                    );
                }
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

            $scope.loadFavouritePigeons = function () {
                pigeonService.loadFavouritePigeons().then(
                    function (serverData) {
                        serverData.forEach(function (pigeon) {
                            parsePigeonDate(pigeon);
                        });
                        $scope.pigeonsData = serverData;
                    },
                    function (serverError) {
                        console.error(serverError);
                    }
                );
            }

            $scope.loadNewsPigeons = function () {
                pigeonService.loadNewsPigeons().then(
                    function (serverData) {
                        console.log(serverData);
                        serverData.forEach(function (pigeon) {
                            parsePigeonDate(pigeon);
                        });
                        $scope.pigeonsData = serverData;
                    },
                    function (serverError) {
                        console.error(serverError);
                    }
                );
            }

            $scope.editPigeon = function (pigeon) {
                if (validationService.validatePigeon(pigeon)) {
                    var pigeonData = {
                        title: pigeon.title,
                        content: pigeon.content,
                        photoData: pigeon.photoData
                    }
                    pigeonService.editPigeon(pigeon.id, pigeonData).then(
                        function (serverData) {
                            pigeon.content = serverData.content;
                            $scope.isEditPigeonExpanded = false;
                            notifyService.showInfo("Successfully updated pigeon");
                        },
                        function (serverError) {
                            notifyService.showError(serverError);
                        }
                    );
                }
            }

            $scope.deletePigeon = function (pigeon) {
                $scope.isDeletePigeonExpanded = false;
                pigeonService.deletePigeon(pigeon.id).then(
                    function (serverData) {
                        var index = $scope.pigeonsData.indexOf(pigeon);
                        if (index > -1) {
                            $scope.pigeonsData.splice(index, 1);
                        }
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        notifyService.showError(serverError);
                    }
                );
            }

            $scope.favourite = function (pigeon) {
                pigeonService.favourite(pigeon.id).then(
                    function (serverData) {
                        pigeon.favouritedCount++;
                        pigeon.favourited = true;
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        notifyService.showError(serverError);
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
                        notifyService.showError(serverError);
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
                        notifyService.showError(serverError);
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
                        notifyService.showError(serverError);
                    }
                );
            }

            $scope.createComment = function (pigeon) {
                var commentData = $scope.commentData;
                if (validationService.validateComment(commentData)) {
                    commentService.createComment(pigeon.id, $scope.commentData).then(
                    function (serverData) {
                        serverData.author.profilePhotoData = serverData.author.profilePhotoData ? serverData.author.profilePhotoData : constants.profilePhotoData;
                        pigeon.comments.push(serverData);
                        $scope.commentData = {};
                        notifyService.showInfo("Successfully add comment");
                    },
                    function (serverError) {
                        notifyService.showError(serverError);
                    });
                }
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
                var commentData = $scope.editCommentData;
                if (validationService.validateComment(commentData)) {
                    commentService.editComment(pigeon.id, comment.id, commentData).then(
                    function (serverData) {
                        comment.content = serverData.content;
                        $scope.isEditCommentExpanded = false;
                        notifyService.showInfo("Successfully edit comment");
                    },
                    function (serverError) {
                        notifyService.showError(serverError);
                    });
                }
            }

            $scope.deleteComment = function (pigeon, comment) {
                commentService.deleteComment(pigeon.id, comment.id).then(
                    function (serverData) {
                        var index = pigeon.comments.indexOf(comment);
                        if (index > -1) {
                            pigeon.comments.splice(index, 1);
                        }
                        notifyService.showInfo(serverData.message);
                    },
                    function (serverError) {
                        notifyService.showError(serverError);
                    }
                );
            }
        });
    }
);