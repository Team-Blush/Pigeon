define(['app', 'constants', 'requestService'], function (app) {
    app.factory('accountService', function ($rootScope, constants, requestService) {
        var service = {};
        var serviceUrlUsers = constants.baseServiceUrl + 'api/users/';
        var serviceUrlProfile = constants.baseServiceUrl + 'api/profile/';

        service.isLoggedIn = function () {
            return !!(sessionStorage['accessToken'] && sessionStorage['accessToken'].length > constants.accessTokenMinLength);
        };

        service.register = function (registerData) {
            var url = serviceUrlUsers + 'register';
            var headers = null;
            return requestService.postRequest(url, headers, registerData);
        };

        service.login = function (loginData) {
            var url = serviceUrlUsers + 'login';
            var headers = null;
            return requestService.postRequest(url, headers, loginData);
        };
        
        service.logout = function () {
            var url = serviceUrlUsers + 'logout';
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
            var url = serviceUrlProfile;
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        };

        service.saveMyData = function (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    switch (property) {
                        case 'profilePhotoData':
                            sessionStorage[property] = data[property] || constants.profilePhotoData;
                            break;
                        case 'coverPhotoData':
                            sessionStorage[property] = data[property] || constants.coverPhotoData;
                            break;
                        case 'null':
                            sessionStorage[property] = '';
                            break;
                        default:
                            sessionStorage[property] = data[property];
                            break;
                    }
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
            var url = serviceUrlProfile + 'edit';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, editProfileData);
        };

        service.changePassword = function (changePasswordData) {
            var url = serviceUrlProfile + 'changePassword';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers, changePasswordData);
        };

        service.loadUserFullData = function (username) {
            var url = serviceUrlUsers + username;
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        };

        service.search = function (searchTerm) {
            var url = serviceUrlUsers + 'search?searchTerm=' + searchTerm;
            var headers = requestService.getHeaders();
            return requestService.getRequest(url, headers);
        };

        service.follow = function (username) {
            var url = serviceUrlUsers + username + '/follow';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers);
        };

        service.unfollow = function (username) {
            var url = serviceUrlUsers + username + '/unfollow';
            var headers = requestService.getHeaders();
            return requestService.putRequest(url, headers);
        };

        return service;
    });
});
