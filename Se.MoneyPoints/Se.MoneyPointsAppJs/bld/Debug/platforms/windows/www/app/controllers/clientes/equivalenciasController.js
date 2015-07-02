angular.module("moneyPointsApp").controller('equivalenciasController',
    ['$scope', '$rootScope', 'authorizationService', 'equivalenciasService', equivalenciasController]);

function equivalenciasController($scope, $rootScope, authorizationService, equivalenciasService) {
    var clienteId = authorizationService.getId();

    equivalenciasService.GetEquivalenciasByCliente(clienteId).then(function (pl) {
        var data = pl.data;

        $scope.equivalencia = data[data.length - 1]; //tomamos el ultimo registro ingresado

    }, function (errorPl) {
        handleError(errorPl);

    });

    $scope.guardarEquivalencia = function () {
        $scope.equivalencia.ClienteId = clienteId;
        equivalenciasService.post($scope.equivalencia).then(function (pl) {
            new PNotify({ text: "Se ingresó correctamente la nueva equivalencia", type: "info", delay: 3000 });
        }, function (errorPl) {
            handleError(errorPl);
        });
    };
}