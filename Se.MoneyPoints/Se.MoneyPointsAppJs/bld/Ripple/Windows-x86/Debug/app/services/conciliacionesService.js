
angular.module('moneyPointsApp').service('conciliacionesService', function ($http, $rootScope) {


    //Create new conciliacion
    this.post = function (conciliacion) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/conciliaciones",
            data: conciliacion
        });
        return request;
    }

    //Get conciliaciones
    this.get = function (conciliacionId) {
        return $http.get($rootScope.baseAddress + "/api/conciliaciones/" + conciliacionId);
    }

    this.GetConciliacionesByCliente = function () {
        return $http.get($rootScope.baseAddress + "/api/Conciliaciones/ConciliacionesByCliente/" + clienteId);
    }

    //Get All conciliaciones
    this.getConciliaciones = function () {
        return $http.get($rootScope.baseAddress + "/api/conciliaciones");
    }

    //Update conciliaciones
    this.put = function (conciliacionId, conciliacion) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/conciliaciones/" + conciliacionId,
            data: conciliacion
        });
        return request;
    }

    //Delete conciliaciones
    this.delete = function (conciliacionId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/conciliaciones/" + conciliacionId
        });
        return request;
    }
});