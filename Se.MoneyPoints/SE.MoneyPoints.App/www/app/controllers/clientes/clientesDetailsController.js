(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('clientesDetailsController', ['$rootScope',
    '$scope', '$routeParams', 'clientesService', 'beneficiariosService', 'equivalenciasService', 'afiliadosClientesService', 'authorizationService',
    clientesDetailsController]);

    function clientesDetailsController($rootScope, $scope, $routeParams, clientesService, beneficiariosService,
        equivalenciasService, afiliadosClientesService, authorizationService) {
        
        var clienteId = $routeParams.ClienteId

        $scope.canDelete = false;
        $scope.canCancel = false;

        var user = authorizationService.getCurrentUser();

        if (user && user.rolId == 1) {
            $scope.canDelete = true;
            $scope.canCancel = true;
        }

        if (clienteId == 0) {
            $scope.IsEditing = false;
        } else {
            var promiseGetCliente = clientesService.get(clienteId);
            //divDetailCliente

            promiseGetCliente.then(function (pl) {
                var res = pl.data;
                $scope.cliente = res;
                $scope.IsEditing = true;

                //Datasource de equivalencias del cliente

            },
             function (errorPl) {
                 handleError(errorPl);
             });
        }


        //fin

        //Guardar Cliente
        $scope.guardarCliente = function () {
            kendo.ui.progress($("#divDetailCliente"), true);
            if ($scope.IsEditing) {
                var promiseGuardarCliente = clientesService.put($scope.cliente.ClienteId, $scope.cliente);
                promiseGuardarCliente.then(function (pl) {
                    new PNotify({
                        text: 'Se Actualizo Correctamente el Cliente',
                        type: 'info',
                        delay: 3000
                    });
                    kendo.ui.progress($("#divDetailCliente"), false);
                },
                function (errorPl) {
                    handleError(errorPl);
                    kendo.ui.progress($("#divDetailCliente"), false);
                });
            } else {
                $scope.cliente.Tercero.Usuarios = [];
                $scope.cliente.Tercero.Usuarios[0] = {
                    Login: $scope.security.Login,
                    Password: Base64.encode($scope.security.newClave)
                };

                var promiseGuardarCliente = clientesService.post($scope.cliente);
                promiseGuardarCliente.then(function (pl) {
                    new PNotify({
                        text: 'Se insertó Correctamente el Cliente',
                        type: 'info',
                        delay: 3000
                    });
                    kendo.ui.progress($("#divDetailCliente"), false);
                },
                function (errorPl) {
                    handleError(errorPl);
                    kendo.ui.progress($("#divDetailCliente"), false);
                });
            }

            //fin
        };

        //Eliminar cliente
        $scope.deleteCliente = function () {
            kendo.ui.progress($("#divDetailCliente"), true);
            var promiseDeleteCliente = clientesService.delete($scope.cliente.ClienteId);
            promiseDeleteCliente.then(function (pl) {
                new PNotify({
                    text: 'Se Elimino Correctamente el Cliente',
                    type: 'info',
                    delay: 3000
                });
                //$scope.goPath('/clientes');
                kendo.ui.progress($("#divDetailCliente"), false);
            },
            function (errorPl) {
                handleError(errorPl);
                console.log('Error eliminando cliente', errorPl);
                kendo.ui.progress($("#divDetailCliente"), false);
            });
        };
    };

})();