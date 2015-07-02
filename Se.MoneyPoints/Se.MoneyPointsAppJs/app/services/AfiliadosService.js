angular.module('moneyPointsApp').service('afiliadosService', function ($http, $rootScope) {

    //Create new afiliado
    this.post = function (afiliado) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/afiliados",
            data: afiliado
        });
        return request;
    }

    //Get afiliado
    this.get = function (afiliadoId) {
        return $http.get($rootScope.baseAddress + "/api/afiliados/" + afiliadoId);
    }

    //Get todos los afiliados
    this.getAfiliados = function () {
        return $http.get($rootScope.baseAddress + "/api/afiliados");
    }

    //Update afiliados
    this.put = function (afiliadoId, afiliado) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/afiliados/" + afiliadoId,
            data: afiliado
        });
        return request;
    }
    //Delete afiliados
    this.delete = function (afiliadoId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/afiliados/" + afiliadoId
        });
        return request;
    }
});