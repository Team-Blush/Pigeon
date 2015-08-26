define(['app', 'noty'], function (app, noty) {
    app.factory('notifyService', function () {
        return {
            showInfo: function (msg) {
                noty({
                    text: msg,
                    type: 'success',
                    layout: 'bottomCenter',
                    timeout: 1000
                }
                );
            },
            showError: function (msg, serverError) {
                // Collect errors to display from the server response
                var errors = [];
                if (serverError && serverError.message) {
                    errors.push(serverError.message);
                }
                if (serverError && serverError.error_description) {
                    errors.push(serverError.error_description);
                }
                if (serverError && serverError.modelState) {
                    var modelStateErrors = serverError.modelState;
                    for (var propertyName in modelStateErrors) {
                        var errorMessages = modelStateErrors[propertyName];
                        var trimmedName = propertyName.substr(propertyName.indexOf('.') + 1);
                        for (var i = 0; i < errorMessages.length; i++) {
                            var currentError = errorMessages[i];
                            errors.push(trimmedName + ' - ' + currentError);
                        }
                    }
                }
                if (errors.length > 0) {
                    msg = msg + "<br>" + errors.join("<br>");
                }
                noty({
                    text: msg,
                    type: 'error',
                    layout: 'bottomCenter',
                    timeout: 5000
                }
                );
            }
        }
    }
    );
});