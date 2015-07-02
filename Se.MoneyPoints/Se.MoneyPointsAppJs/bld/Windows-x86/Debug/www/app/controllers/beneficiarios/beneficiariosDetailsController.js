(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('beneficiariosDetailsController', ['$rootScope', '$scope', '$routeParams', 'beneficiariosService', 'authorizationService', 'Base64', beneficiariosDetailsController]);

    function beneficiariosDetailsController($rootScope, $scope, $routeParams, beneficiariosService, authorizationService, Base64) {
                

        var beneficiarioId = $routeParams.beneficiarioId
        $scope.onlyNumber = /^\d+$/;
        $scope.canDelete = false;
        $scope.canCancel = false;

        var user = authorizationService.getCurrentUser();
        if (user && user.rolId == 1) {
            $scope.canDelete = true;
            $scope.canCancel = true;
        }

        if (beneficiarioId == 0) {
            $scope.IsEditing = false;
            $scope.showEstados = false;
        } else {
            $scope.showEstados = true;
            var promiseGetbeneficiario = beneficiariosService.get(beneficiarioId);

            promiseGetbeneficiario.then(function (pl) {
                var res = pl.data;
                $scope.beneficiario = res;
                $scope.IsEditing = true;

            },
             function (errorPl) {
                 handleError(errorPl);
                 console.log('Error cargando beneficiario', errorPl);
             });

            //Consultar estados de cuentas
            $scope.cuentasBeneficiarioOptions = {
                dataSource: {
                    type: "json",
                    transport: {
                        read: {
                            url: $rootScope.baseAddress + "/api/BeneficiariosClientes?$filter=BeneficiarioId eq " + beneficiarioId,
                            dataType: "json",
                            beforeSend: $rootScope.beforeSendRequest
                        },

                        parameterMap: function (options, type) {
                            var paramMap = kendo.data.transports.odata.parameterMap(options);
                            delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                            return paramMap;
                        }
                    },
                    requestStart: function (e) {
                        kendo.ui.progress($("#gridCuentasBeneficiario"), true);
                    },
                    requestEnd: function (e) {
                        kendo.ui.progress($("#gridCuentasBeneficiario"), false);
                    },
                    pageSize: 50,
                    serverPaging: true,
                    serverSorting: true
                },
                sortable: true,
                pageable: true,
                schema: {
                    model: {
                        fields: {
                            Saldo: { type: "number" },
                            Puntos: { type: "number" },
                            "Cliente.Tercero.Nombre": { type: "string" }
                        }
                    }
                },
                template: kendo.template($("#tmpCuentasBeneficiario").html()),
            };
        }


        //Guardar beneficiario
        $scope.guardarbeneficiario = function () {
            if ($scope.IsEditing) {
                var promiseGuadarbeneficiario = beneficiariosService.put($scope.beneficiario.BeneficiarioId, $scope.beneficiario);
                promiseGuadarbeneficiario.then(function (pl) {
                    //$scope.mensaje = "Actualizado satisfactoriamente";
                    new PNotify({
                        text: 'Se Actualizo Correctamente',
                        type: 'info',
                        delay: 3000
                    });
                    //$scope.goPath('/beneficiarios');
                }, function (errorPl) {
                    handleError(errorPl);
                });
            } else {
                $scope.beneficiario.Tercero.Usuarios = [];
                $scope.beneficiario.Pin = Base64.encode($scope.security.newPIN)
                $scope.beneficiario.Tercero.Usuarios[0] = {
                    Login: $scope.security.Login,
                    Password: Base64.encode($scope.security.newClave)
                };

                var promiseGuadarbeneficiario = beneficiariosService.post($scope.beneficiario);
                promiseGuadarbeneficiario.then(function (pl) {
                    new PNotify({
                        text: 'Se insertó Correctamente',
                        type: 'info',
                        delay: 3000
                    });
                    if ($scope.beneficiario.Tercero.TerceroId == undefined || $scope.beneficiario.Tercero.TerceroId == null || $scope.beneficiario.Tercero.TerceroId == 0) {
                        $scope.goPath('/login');
                    }else{
                        $scope.goPath('/beneficiarios');
                    }                    
                },
                function (errorPl) {
                    handleError(errorPl);
                });
            }
        };

        //Eliminar beneficiario
        $scope.deletebeneficiario = function () {
            var promiseDeletebeneficiario = beneficiariosService.delete($scope.beneficiario.beneficiarioId);
            promiseDeletebeneficiario.then(function (pl) {
                //$scope.mensaje = "Eliminado satisfactoriamente.";
                new PNotify({
                    text: 'Se Elimino Correctamente',
                    type: 'danger',
                    delay: 3000
                });
                $scope.goPath('/beneficiarios');
            },
            function (errorPl) {
                handleError(errorPl);
            });
        };

        
    };

})();