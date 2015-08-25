define(['app'], function (app) {
    app.factory('requestService', function($http, $q) {
        var service = {};

        function makeRequest(method, url, headers, data) {
            var deferred = $q.defer();

            $http({
                    method: method,
                    url: url,
                    headers: headers,
                    data: data
                })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }

        service.getRequest = function(url, headers, data) {
            return makeRequest('GET', url, headers, data);
        };

        service.postRequest = function(url, headers, data) {
            return makeRequest('POST', url, headers, data);
        };

        service.putRequest = function(url, headers, data) {
            return makeRequest('PUT', url, headers, data);
        };

        service.deleteRequest = function(url, headers, data) {
            return makeRequest('DELETE', url, headers, data);
        };

        service.getHeaders = function() {
            return {
                Authorization: 'Bearer ' + sessionStorage['accessToken']
            };
        };

        return service;
    });
});
