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

        service.loadFavouritePigeons = function () {
            var url = serviceUrl + '/favourites';
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        }

        service.loadNewsPigeons = function () {
            var url = serviceUrl + '/news';
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        }

        service.editPigeon = function (pigeonId, pigeonData) {
            var url = serviceUrl + '/' + pigeonId + '/edit';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, pigeonData);
        };

        service.deletePigeon = function (pigeonId) {
            var url = serviceUrl + '/' + pigeonId;
            var headers = requestService.getHeaders();
            return requestService.deleteRequest(url, headers);
        };

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
