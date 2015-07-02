
(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('recibirConciliacionesController', ['$rootScope', '$scope',
        '$routeParams', 'conciliacionesService', 'authorizationService', recibirConciliacionesController]);

    function recibirConciliacionesController($rootScope, $scope, $routeParams, conciliacionesService, authorizationService) {

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

                $scope.estados = [];

                //Estados conciliacion
                if (res.Estado == "Enviada") {
                    $scope.estados = ["Rechazada", "Aceptada", "Enviada"];
                }

                if (res.Estado == "Rechazada") {
                    $scope.estados = ["Aceptada", "Rechazada"];
                }

                if (res.Estado == "Aceptada") {
                    $scope.estados = ["Aceptada"];
                }

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
    };
})();