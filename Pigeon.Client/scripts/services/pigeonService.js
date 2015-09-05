define(['app', 'constants', 'requestService'], function (app) {
    app.factory('pigeonService', function (constants, requestService) {
        var service = {};
        var serviceUrl = constants.baseServiceUrl + 'api/pigeons';

        service.createPigeon = function (pigeonData) {
            var url = serviceUrl;
            var headers = requestService.getHeaders();
            return requestService.postRequest(url, headers, pigeonData);
        };

        service.loadUserPigeons = function(username) {
            var url = serviceUrl + '/' + username + '/all';
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        }

        return service;
    });
});
