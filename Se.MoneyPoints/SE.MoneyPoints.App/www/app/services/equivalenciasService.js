angular.module('moneyPointsApp').service('equivalenciasService', function ($http, $rootScope) {


    //Create new equivalencia
    this.post = function (equivalencia) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/equivalencias",
            data: equivalencia
        });
        return request;
    }

    //Get equivalencias
    this.get = function (EquivalenciaId) {
        return $http.get($rootScope.baseAddress + "/api/equivalencias/" + equivalenciaId);
    }

    this.GetEquivalenciasByCliente = function(clienteId) {
        return $http.get($rootScope.baseAddress + "/api/Equivalencias/EquivalenciasByCliente/" + clienteId);
    }

    //Get All equivalencias
    this.getEquivalencias = function () {
        return $http.get($rootScope.baseAddress + "/api/equivalencias");
    }

    //Update equivalencias
    this.put = function (EquivalenciaId, equivalencia) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/equivalencias/" + equivalenciaId,
            data: equivalencia
        });
        return request;
    }

    //Delete equivalencias
    this.delete = function (equivalenciaId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/equivalencias/" + equivalenciaId
        });
        return request;
    }
});