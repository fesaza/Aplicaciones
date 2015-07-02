angular.module('moneyPointsApp').service('beneficiariosService', function ($http, $rootScope) {


    //Create new beneficiario
    this.post = function (beneficiario) {
        var request = $http({
            method: "post",
            url: $rootScope.baseAddress + "/api/beneficiarios" ,
            data: beneficiario
        });
        return request;
    }

    //Get beneficiario
    this.get = function (beneficiarioId) {
        return $http.get($rootScope.baseAddress + "/api/beneficiarios/" + beneficiarioId);
    }

    //Get todos los beneficiarios
    this.getbeneficiarios = function () {
        return $http.get($rootScope.baseAddress + "/api/beneficiarios");
    }


    //Update beneficiarios
    this.put = function (beneficiarioId, beneficiario) {
        var request = $http({
            method: "put",
            url: $rootScope.baseAddress + "/api/beneficiarios/" + beneficiarioId,
            data: beneficiario
        });
        return request;
    }
    //Delete beneficiarios
    this.delete = function (beneficiarioId) {
        var request = $http({
            method: "delete",
            url: $rootScope.baseAddress + "/api/beneficiarios/" + beneficiarioId
        });
        return request;
    }
});