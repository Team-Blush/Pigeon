define(['app', 'notifyService'], function (app) {
    app.factory('validationService', function (notifyService) {
        var service = {};

        service.validatePictureType = function (picture) {
            if (picture.type !== 'image/jpeg') {
                notifyService.showError("The picture format must be .jpg!");
                return false;
            }
            return true;
        };

        service.validatePictureSize = function (picture, maxSize) {
            if (picture.size > maxSize) {
                notifyService.showError('The picture size cannot be more than ' + (maxSize / 1024) + 'KB');
                return false;
            }
            return true;
        };

        service.validateMessage = function (postDataContent) {
            if (postDataContent.length < 2) {
                notifyService.showError("The message content must be at least 2 symbols long.");
                return false;
            }
            return true;
        };

        function validateUsername(username) {
            var usernamePattern = /^[a-zA-Z0-9]{4,32}$/;
            if (!usernamePattern.test(username)) {
                notifyService.showError("The username can only contain letters or digits from 4 to 32 symbols.");
                return false;
            }
            return true;
        }

        function validateEmailAddress(email) {
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            if (!emailPattern.test(email)) {
                notifyService.showError("Incorrect email address.");
                return false;
            }
            return true;
        }

        function validatePassword(password) {
            if (password.length < 6 || password.length > 32) {
                notifyService.showError("The password must be from 6 to 32 characters long.");
                return false;
            }
            return true;
        }

        function validateConfirmPasswordMatch(password, confirmPassword) {
            if (password !== confirmPassword) {
                notifyService.showError("The password and confirmation password do not match.");
                return false;
            }
            return true;
        }

        function escapeHtmlSpecialChars(text) {
            var map = {
                '<': '&lt;',
                '>': '&gt;'
            };

            return text.replace(/[<>]/g, function (m) {
                return map[m];
            });
        }

        service.escapeHtmlSpecialChars = function (input, isString) {
            if (isString) {
                return escapeHtmlSpecialChars(input);
            } else {
                var outputObject = {};
                for (var key in input) {
                    if (input.hasOwnProperty(key)) {
                        if (key !== 'gender' && key !== 'profileImageData' && key !== 'coverImageData') {
                            outputObject[key] = escapeHtmlSpecialChars(input[key]);
                        } else {
                            outputObject[key] = input[key];
                        }
                    }
                }
                return outputObject;
            }
        };

        service.validateLoginForm = function (username, password) {
            return (validateUsername(username) && validatePassword(password));
        };

        service.validateRegisterForm = function (username, email, password, confirmPassword, name) {
            return (validateUsername(username) && validateEmailAddress(email) && validatePassword(password) &&
                validateConfirmPasswordMatch(password, confirmPassword));
        };

        service.validateChangePasswordForm = function (newPassword, confirmPassword) {
            return (validatePassword(newPassword) && validateConfirmPasswordMatch(newPassword, confirmPassword));
        };

        service.validateEditProfileForm = function (name, email) {
            return (validateName(name) && validateEmailAddress(email));
        };
        
        return service;
    });
});
