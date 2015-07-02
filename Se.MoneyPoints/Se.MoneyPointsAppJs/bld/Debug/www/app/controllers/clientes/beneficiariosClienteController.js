"use strict";

angular.module("moneyPointsApp").controller('beneficiariosClienteController',
    ['$rootScope', '$scope', '$routeParams', 'authorizationService', 'beneficiariosClienteService', beneficiariosClienteController]);


function beneficiariosClienteController($rootScope, $scope, $routeParams, authorizationService, beneficiariosClienteService) {
    var clienteId = authorizationService.getId();

    //obtener la lista de beneficiarios por cliente

    $scope.benefClienteOpts = {
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: $rootScope.baseAddress + "/api/BeneficiariosClientes/BeneficiariosClientesByCliente/" + clienteId,
                    dataType: "json",
                    beforeSend: $rootScope.beforeSendRequest
                },
                parameterMap: function (options, type) {
                    var paramMap = kendo.data.transports.odata.parameterMap(options);
                    delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                    return paramMap;
                }
            }, requestStart: function (e) {
                kendo.ui.progress($("#divBenefCliente"), true);
            },
            requestEnd: function (e) {
                kendo.ui.progress($("#divBenefCliente"), false);
            },
            pageSize: 5,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
        },
        filterable: {
            field: "Beneficiario.Tercero.Nombre",
            operator: "contains"
        },
        template: kendo.template($("#tmpBenefCliente").html()),
    };

    $scope.beneficiariosSelectOpts = {
        dataSource: {
            transport: {
                read: {
                    dataType: "json",
                    url: $rootScope.baseAddress + "/api/Beneficiarios/GetBeneficiariosCandidatos/" + authorizationService.getId(),
                    beforeSend: $rootScope.beforeSendRequest
                }
            },
            serverFiltering: true,
            filterable: true
        },
        filterable:true,
        dataTextField: "Tercero.Nombre",
        dataValueField: "BeneficiarioId"
    };

    $scope.agregarBeneficiario = function () {
        var benefCliente = {
            ClienteId: authorizationService.getId(),
            BeneficiarioId: $scope.benef,
            FechaIngreso: new Date(),
            FechaBaja: null
        };
        beneficiariosClienteService.post(benefCliente).then(
            function (pl) {
                new PNotify({
                    text: 'Se agregó correctamente el beneficiario',
                    type: 'info',
                    delay: 3000
                });
                $scope.goPath('/beneficiariosCliente');
                $scope.benefs.dataSource.read();
                $scope.cmbBeneficiario.dataSource.read();
            },
            function (errorPl) {
                handleError(errorPl);
                console.log('Error registrando beneficiario', errorPl);
            });
    };


}