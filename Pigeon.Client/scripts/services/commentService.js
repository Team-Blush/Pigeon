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

        return service;
    });
});
