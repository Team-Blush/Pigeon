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

        service.favourite = function (pigeonId) {
            var url = serviceUrl + '/' + pigeonId + '/favourite';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers);
        };

        service.unfavourite = function (pigeonId) {
            var url = serviceUrl + '/' + pigeonId + '/unfavourite';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers);
        };

        service.vote = function (pigeonId, vote) {
            var url = serviceUrl + '/' + pigeonId + '/vote';
            var headers = requestService.getHeaders();
            return requestService.postRequest(url, headers, vote);
        };

        return service;
    });
});
