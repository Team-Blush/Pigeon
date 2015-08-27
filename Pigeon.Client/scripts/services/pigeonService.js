define(['app', 'constants', 'requestService'], function (app) {
    app.factory('pigeonService', function (constants, requestService) {
        var service = {};
        var serviceUrl = constants.baseServiceUrl + 'api/pigeons';

        service.createPigeon = function (postData) {
            var url = serviceUrl;
            var headers = requestService.getHeaders();
            return requestService.postRequest(url, headers, postData);
        };

        return service;
    });
});
