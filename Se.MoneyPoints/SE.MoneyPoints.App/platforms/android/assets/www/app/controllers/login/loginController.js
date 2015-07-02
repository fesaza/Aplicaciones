'use strict';

angular.module('moneyPointsApp')

.controller('loginController',
    ['$scope', '$rootScope', '$location', 'authenticationService', 'authorizationService', 'Base64', '$http', 'tercerosService',
    function ($scope, $rootScope, $location, authenticationService, authorizationService, Base64, $http, tercerosService) {

     
        $scope.login = function () {
            
            kendo.ui.progress($("#form"), true);
            $scope.dataLoading = true;
            $scope.error = "";
            authenticationService.login($scope.username, $scope.password, function (response) {
                if (response.success) {
                    authenticationService.setCredentials(response);
                    //ConsultarId
                    tercerosService.get(response.TerceroId).then(function (pl) {
                        var res = pl.data;
                        authorizationService.setId(res);

                        //Consultar asegurables
                        authorizationService.getAsegurables(function (response) {
                            for (var i = 0; i < response.length; i++) {
                                response[i].Asegurable.Ruta = response[i].Asegurable.Ruta.replace("{id}", authorizationService.getId());
                            }
                            $rootScope.$emit('refreshMenu', response);
                            $scope.dataLoading = false;
                            kendo.ui.progress($("#form"), false);
                        });
                        
                        if (response.RolId == 2) {//si el usuario logeado es cliente
                            $location.path('/vender');
                        } else if (response.RolId == 4) {//si el usuario logeado es Beneficiario
                            $location.path('/beneficiariosDetails/' + authorizationService.getId());
                        } else {//si el usuario logeado es admin
                            $location.path('/home');
                        }
                    });
                } else {
                    $scope.error = response.Message;
                    $scope.dataLoading = false;
                    kendo.ui.progress($("#form"), false);
                }
            });
        };
    }]);