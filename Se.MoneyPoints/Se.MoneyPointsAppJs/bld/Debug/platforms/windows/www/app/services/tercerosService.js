angular.module('moneyPointsApp').service('tercerosService', function ($http, $rootScope) {


    //Create new tercero
    this.post = function (tercero) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/terceros",
            data: tercero
        });
        return request;
    }

    //Get tercero
    this.get = function (terceroId) {
        return $http.get($rootScope.baseAddress + "/api/terceros/" + terceroId);
    }

    this.getByIdentificacion = function (identificacion) {
        return $http.get($rootScope.baseAddress + "/api/terceros?$filter=Identificacion eq '" + identificacion + "'");
    }

    //Get todos los terceros
    this.getterceros = function () {
        return $http.get($rootScope.baseAddress + "/api/terceros");
    }


    //Update terceros
    this.put = function (terceroId, tercero) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/terceros/" + terceroId,
            data: tercero
        });
        return request;
    }
    //Delete terceros
    this.delete = function (terceroId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/terceros/" + terceroId
        });
        return request;
    }
});