'use strict';

angular.module('moneyPointsApp')

.factory('authenticationService',
    ['Base64', '$http', '$cookieStore', '$rootScope', '$timeout',
    function (Base64, $http, $cookieStore, $rootScope, $timeout) {
        var service = {};

        service.changePassword = function (opts, callback) {
            $http.post($rootScope.baseAddress + '/api/authentication/ChangePassword', JSON.stringify(opts),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                .success(function (response) {
                    callback(response);
                }).error(function (data, status, headers, config) {
                    data.success = false;
                    callback(data);
                });
        }

        service.changePIN = function (opts, callback) {
            $http.post($rootScope.baseAddress + '/api/authentication/ChangePIN', JSON.stringify(opts),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                .success(function (response) {
                    callback(response);
                }).error(function (data, status, headers, config) {
                    data.success = false;
                    callback(data);
                });
        }

        service.login = function (username, password, callback) {
            
            var pwd = Base64.encode(password);

            var usuario = {
                Login: username,
                Password: pwd
            };

            $http.post($rootScope.baseAddress + '/api/authentication/Authenticate', JSON.stringify(usuario),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                .success(function (response) {
                    response.success = true;
                    callback(response);
                }).error(function (data, status, headers, config) {
                    data.success = false;
                    callback(data);
                });
        };

        service.setCredentials = function (response) {
            var username = response.Login;
            var password = response.Password;

            var authdata = Base64.encode(username + ':' + Base64.decode(password));

            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata,
                    rolId: response.RolId,
                    usuarioId: response.UsuarioId
                }
            };

            $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata; // jshint ignore:line
            $cookieStore.put('globals', $rootScope.globals);
        };



        service.clearCredentials = function () {
            $rootScope.globals = {};
            $cookieStore.remove('globals');
            $http.defaults.headers.common.Authorization = 'Basic ';
        };

        return service;
    }])

.factory('Base64', function () {
    /* jshint ignore:start */

    var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';

    return {
        encode: function (input) {
            var output = "";
            var chr1, chr2, chr3 = "";
            var enc1, enc2, enc3, enc4 = "";
            var i = 0;

            do {
                chr1 = input.charCodeAt(i++);
                chr2 = input.charCodeAt(i++);
                chr3 = input.charCodeAt(i++);

                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;

                if (isNaN(chr2)) {
                    enc3 = enc4 = 64;
                } else if (isNaN(chr3)) {
                    enc4 = 64;
                }

                output = output +
                    keyStr.charAt(enc1) +
                    keyStr.charAt(enc2) +
                    keyStr.charAt(enc3) +
                    keyStr.charAt(enc4);
                chr1 = chr2 = chr3 = "";
                enc1 = enc2 = enc3 = enc4 = "";
            } while (i < input.length);

            return output;
        },

        decode: function (input) {
            var output = "";
            var chr1, chr2, chr3 = "";
            var enc1, enc2, enc3, enc4 = "";
            var i = 0;

            // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
            var base64test = /[^A-Za-z0-9\+\/\=]/g;
            if (base64test.exec(input)) {
                window.alert("There were invalid base64 characters in the input text.\n" +
                    "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                    "Expect errors in decoding.");
            }
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

            do {
                enc1 = keyStr.indexOf(input.charAt(i++));
                enc2 = keyStr.indexOf(input.charAt(i++));
                enc3 = keyStr.indexOf(input.charAt(i++));
                enc4 = keyStr.indexOf(input.charAt(i++));

                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;

                output = output + String.fromCharCode(chr1);

                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }

                chr1 = chr2 = chr3 = "";
                enc1 = enc2 = enc3 = enc4 = "";

            } while (i < input.length);

            return output;
        }
    };

    /* jshint ignore:end */
});