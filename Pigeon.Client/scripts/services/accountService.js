define(['app', 'constants', 'requestService'], function (app) {
    app.factory('accountService', function ($rootScope, constants, requestService) {
        var service = {};
        var serviceUrl = constants.baseServiceUrl;

        service.isLoggedIn = function () {
            return !!(sessionStorage['accessToken'] && sessionStorage['accessToken'].length > constants.accessTokenMinLength);
        };

        service.register = function (registerData) {
            var url = serviceUrl + 'api/users/register';
            var headers = null;
            return requestService.postRequest(url, headers, registerData);
        };

        service.login = function (loginData) {
            var url = serviceUrl + 'api/users/login';
            var headers = null;
            return requestService.postRequest(url, headers, loginData);
        };
        
        service.logout = function () {
            var url = serviceUrl + 'api/users/logout';
            var headers = requestService.getHeaders();
            return requestService.postRequest(url, headers);
        };

        service.setCredentials = function (serverData) {
            sessionStorage['accessToken'] = serverData.access_token;
        };

        service.clearCredentials = function() {
            for (var key in sessionStorage) {
                if (sessionStorage.hasOwnProperty(key)) {
                    delete sessionStorage[key];
                }
            }
        };

        service.getMe = function () {
            var url = serviceUrl + "api/profile";
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        };

        service.saveMyData = function (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    sessionStorage[property] = data[property];
                }
            }
        };

        service.loadMyData = function () {
            var data = {};
            for (var property in sessionStorage) {
                if (sessionStorage.hasOwnProperty(property)) {
                    data[property] = sessionStorage[property];
                }
            }
            return data;
        };

        service.editProfile = function (editProfileData) {
            var url = serviceUrl + 'api/profile/edit';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, editProfileData);
        };

        service.changePassword = function (changePasswordData) {
            var url = serviceUrl + 'api/profile/changePassword';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, changePasswordData);
        };

        service.loadUserFullData = function (username) {
            var url = serviceUrl + 'api/users/' + username;
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        };

        return service;
    });
});
