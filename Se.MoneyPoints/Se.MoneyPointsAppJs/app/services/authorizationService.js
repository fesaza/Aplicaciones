'use strict';

angular.module('moneyPointsApp')

.factory('authorizationService',
    ['$http', '$rootScope',
    function ($http, $rootScope) {
        var service = {};

        service.getAsegurables = function (callback) {

            var currentUser = $rootScope.globals.currentUser;

            return $http.get($rootScope.baseAddress + "/api/AsegurablesRoles/GetAsegurablesRolesByRole/" + currentUser.rolId)
                .success(function (response) {
                    callback(response);
                })
                .error(function (data, status, headers, config) {
                    callback(data);
                });
        };

        service.getId = function () {
            return $rootScope.globals.currentUser.id;
        }

        service.getCurrentUser = function () {
            return $rootScope.globals.currentUser;
        };

        service.setId = function (tercero) {
            var id = 0;

            if (tercero.Beneficiarios.length > 0) {
                id = tercero.Beneficiarios[0].BeneficiarioId;
            } else {
                if (tercero.Clientes.length > 0) {
                    id = tercero.Clientes[0].ClienteId;
                } else {
                    id = tercero.TerceroId;
                }
            }

            $rootScope.globals.currentUser.id = id;
        }

        return service;
    }]);