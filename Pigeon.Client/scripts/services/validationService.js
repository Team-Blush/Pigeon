define(['app', 'notifyService'], function (app) {
    app.factory('validationService', function (notifyService) {
        var service = {};

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
                        if (key !== 'profilePhotoData' && key !== 'coverPhotoData') {
                            outputObject[key] = escapeHtmlSpecialChars(input[key]);
                        } else {
                            outputObject[key] = input[key];
                        }
                    }
                }
                return outputObject;
            }
        };

        function validateUsername(username) {
            var usernamePattern = /^[a-zA-Z0-9]{4,32}$/;
            if (!username || !usernamePattern.test(username)) {
                notifyService.showError("The username can only contain letters or digits from 4 to 32 symbols");
                return false;
            }
            return true;
        }

        function validateEmailAddress(email) {
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            if (!emailPattern.test(email)) {
                notifyService.showError("Incorrect email address");
                return false;
            }
            return true;
        }

        function validatePassword(password) {
            if (!password || password.length < 6 || password.length > 100) {
                notifyService.showError("The password must be from 6 to 100 characters long");
                return false;
            }
            return true;
        }

        function validateConfirmPasswordMatch(password, confirmPassword) {
            if (password !== confirmPassword) {
                notifyService.showError("The password and confirmation password do not match");
                return false;
            }
            return true;
        }

        function validateName(name, isFirst) {
            if (name.length < 2 || name.length > 20) {
                var namePart;
                if (isFirst) {
                    namePart = "First";
                } else {
                    namePart = "Last";
                }
                notifyService.showError(namePart + " name must be from 2 to 20 characters long");
                return false;
            }
            return true;
        }

        function validateAge(age) {
            if (age < 0 || age > 100) {
                notifyService.showError("Your age must be from 0 to 100 years");
                return false;
            }
            return true;
        };

        function validatePigeonTitle(title) {
            if (!title || title.length < 5 || title.length > 50) {
                notifyService.showError("Pigeon title must be from 5 to 50 symbols");
                return false;
            }
            return true;
        };

        function validatePigeonContent(content) {
            if (content && content.length > 150) {
                notifyService.showError("Pigeon message must be under 150 symbols");
                return false;
            }
            return true;
        };

        function validateCommentContent(content) {
            if (content == null || content.length < 5 || content.length > 50) {
                notifyService.showError("Comment must be from 5 to 100 symbols");
                return false;
            }
            return true;
        };

        service.validateLoginForm = function (loginData) {
            return (validateUsername(loginData.username) && validatePassword(loginData.password));
        };

        service.validateRegisterForm = function (registerData) {
            return (validateUsername(registerData.username) && validateEmailAddress(registerData.email) &&
                validatePassword(registerData.password) && validateConfirmPasswordMatch(registerData.password, registerData.confirmPassword));
        };

        service.validateChangePasswordForm = function (changePasswordData) {
            return (validatePassword(changePasswordData.newPassword) &&
                validateConfirmPasswordMatch(changePasswordData.newPassword, changePasswordData.confirmPassword));
        };

        service.validateEditProfileForm = function (myData) {
            return (validateName(myData.firstName, true) && validateName(myData.lastName, false) &&
                validateEmailAddress(myData.email) && validateAge(myData.age));
        };

        service.validateComment = function(comment) {
            return validateCommentContent(comment.content);
        };

        service.validatePigeon = function (pigeon) {
            return (validatePigeonTitle(pigeon.title) && validatePigeonContent(pigeon.content));
        };

        return service;
    });
});
