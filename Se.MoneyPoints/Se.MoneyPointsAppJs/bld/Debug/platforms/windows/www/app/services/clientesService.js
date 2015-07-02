angular.module('moneyPointsApp').service('clientesService', function ($http, $rootScope) {


    //Create new record
    this.post = function (Cliente) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/Clientes",
            data: Cliente
        });
        return request;
    }

    //Get Single Records
    this.get = function (ClienteId) {
        return $http.get($rootScope.baseAddress + "/api/Clientes/" + ClienteId);
    }

    //Get All Employees
    this.getClientes = function () {
        return $http.get($rootScope.baseAddress + "/api/Clientes");
    }


    //Update the Record
    this.put = function (ClienteId, Cliente) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/Clientes/" + ClienteId,
            data: Cliente
        });
        return request;
    }
    //Delete the Record
    this.delete = function (ClienteId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/Clientes/" + ClienteId
        });
        return request;
    }
});