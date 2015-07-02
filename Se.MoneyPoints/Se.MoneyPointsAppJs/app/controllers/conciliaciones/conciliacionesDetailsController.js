
(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('conciliacionesDetailsController', ['$rootScope', '$scope',
        '$routeParams', 'conciliacionesService', 'authorizationService', conciliacionesDetailsController]);

    function conciliacionesDetailsController($rootScope, $scope, $routeParams, conciliacionesService, authorizationService) {

        var conciliacionId = $routeParams.conciliacionId;
        $scope.conciliacion = {};

        if (!conciliacionId || conciliacionId == 0) {
            $scope.isEditing = false;
        } else {
            $scope.isEditing = true;

            //Obtener la conciliacion
            conciliacionesService.get(conciliacionId).then(function (pl) {
                var res = pl.data;
                $scope.conciliacion = res;
            }, function (errorPl) {
                handleError(errorPl);
            });
        }

        //Guardar Conciliación
        $scope.guardarConciliacion = function () {
            if (!$scope.isEditing) {
                var promiseGuardarConciliacion = conciliacionesService.post($scope.conciliacion);
                promiseGuardarConciliacion.then(function (pl) {
                    new PNotify({
                        text: 'Se insertó Correctamente',
                        type: 'info',
                        delay: 3000
                    });
                    $scope.goPath('/conciliaciones');
                },
                function (errorPl) {
                    handleError(errorPl);
                });
            } else {
                conciliacionesService.put(conciliacionId, $scope.conciliacion).then(function (pl) {
                    new PNotify({
                        text: 'Se modificó Correctamente',
                        type: 'info',
                        delay: 3000
                    });
                    $scope.goPath('/conciliaciones');
                },
                function (errorPl) {
                    handleError(errorPl);
                });
            }
        };
        //fin scope

        //Eliminar Conciliación
        $scope.deleteConciliacion = function () {
            var promiseDeleteConciliacion = conciliacionesService.delete($scope.conciliacion.conciliacionId);
            promiseDeleteConciliacion.then(function (pl) {
                new PNotify({
                    text: 'Se elimino Correctamente',
                    type: 'info',
                    delay: 3000
                });
                $scope.goPath('/conciliaciones');
            },
            function (errorPl) {
                handleError(errorPl);
            });
        };
        //fin scope

        //select
        $scope.SeleccionarOptions = {
            dataSource: {
                transport: {
                    read: {
                        dataType: "json",
                        url: $rootScope.baseAddress + "/api/AfiliadosClientes/AfiliadosClientesByCliente/" + authorizationService.getId(),
                        beforeSend: $rootScope.beforeSendRequest
                    }
                },
                serverFiltering: true
            },
            dataTextField: "Afiliado.Cliente.Tercero.Nombre",
            dataValueField: "AfiliadosClienteId",
            change: function () {

            }
        };
        //fin
    };
})();