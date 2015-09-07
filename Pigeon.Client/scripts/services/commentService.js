define(['app', 'constants', 'requestService'], function (app) {
    app.factory('commentService', function (constants, requestService) {
        var service = {};
        var serviceUrl = constants.baseServiceUrl + 'api/pigeons';

        service.createComment = function (pigeonId, commentData) {
            var url = serviceUrl + '/' + pigeonId + '/comments';
            var headers = requestService.getHeaders();
            return requestService.postRequest(url, headers, commentData);
        };

        service.loadPigeonComments = function (pigeonId) {
            var url = serviceUrl + '/' + pigeonId + '/comments';
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        }

        service.editComment = function (pigeonId, commentId, commentData) {
            var url = serviceUrl + '/' + pigeonId + '/comments/' + commentId;
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, commentData);
        };

        service.deleteComment = function (pigeonId, commentId) {
            var url = serviceUrl + '/' + pigeonId + '/comments/' + commentId;
            var headers = requestService.getHeaders();
            return requestService.deleteRequest(url, headers);
        };

        return service;
    });
});
