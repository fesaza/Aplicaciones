angular.module('moneyPointsApp').service('afiliadosClientesService', function ($http, $rootScope) {


    //Create new record
    this.post = function (AfiliadosCliente) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/AfiliadosClientes",
            data: AfiliadosCliente
        });
        return request;
    }

    //Get Single Records
    this.get = function (AfilidoClienteId) {
        return $http.get($rootScope.baseAddress + "/api/AfiliadosClientes/" + AfilidoClienteId);
    }

    this.GetAfiliadosClientesByClientes = function (clienteId) {
        return $http.get($rootScope.baseAddress + "api/AfiliadosClientes/AfiliadosClientesByCliente/" + clienteId);
    }

    //Get All Employees
    this.GetAfiliadosCliente = function () {
        return $http.get($rootScope.baseAddress + "/api/AfiliadosClientes");
    }

    //Update the Record
    this.put = function (AfilidoClienteId, AfilidoClienteId) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/AfiliadosClientes/" + AfilidoClienteId,
            data: AfilidoClienteId
        });
        return request;
    }

    //Delete the Record
    this.delete = function (AfilidoClienteId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/AfiliadosClientes/" + AfilidoClienteId
        });
        return request;
    }
});