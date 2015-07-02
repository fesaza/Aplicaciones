angular.module("moneyPointsApp").controller('cargarPuntosController',
    ['$rootScope', '$routeParams', '$scope', 'beneficiariosClienteService', 'beneficiariosPuntosService', cargarPuntosController]);

function cargarPuntosController($rootScope, $routeParams, $scope, beneficiariosClienteService, beneficiariosPuntosService) {

    var id = $routeParams.beneficiariosClienteId;

    beneficiariosClienteService.get(id).then(function (pl) {
        var res = pl.data;
        $scope.benefCliente = res;
    }, function (errorPl) {
        new PNotify({
            text: 'Error consultando información',
            type: 'danger',
            delay: 3000
        });
    });

    $scope.cargarPuntos = function () {
        $scope.benefPuntos.BeneficiariosClienteId = id;
        $scope.benefPuntos.Fecha = new Date();
        $scope.benefPuntos.Valor = 0;

        beneficiariosPuntosService.post($scope.benefPuntos).then(function (pl) {
            new PNotify({
                text: 'Se cargaron los puntos correctamente',
                type: 'info',
                delay: 3000
            });
        }, function (errorPl) {
            handleError(errorPl);
            console.log(errorPl);
        });
    };
}