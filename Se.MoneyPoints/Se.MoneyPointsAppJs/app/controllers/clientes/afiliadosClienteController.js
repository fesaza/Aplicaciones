"use strict";

angular.module("moneyPointsApp").controller('afiliadosClienteController',
    ['$rootScope', '$scope', '$routeParams', 'authorizationService', 'afiliadosClientesService', afiliadosClienteController]);


function afiliadosClienteController($rootScope, $scope, $routeParams, authorizationService, afiliadosClientesService) {
    var clienteId = authorizationService.getId();

    //obtener la lista de beneficiarios por cliente

    $scope.aliadosClienteOpts = {
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: $rootScope.baseAddress + "/api/AfiliadosClientes/AfiliadosClientesByCliente/" + clienteId,
                    dataType: "json",
                    beforeSend: $rootScope.beforeSendRequest
                },
                parameterMap: function (options, type) {
                    var paramMap = kendo.data.transports.odata.parameterMap(options);
                    delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                    return paramMap;
                }
            }, requestStart: function (e) {
                kendo.ui.progress($("#divAfiliadosCliente"), true);
            },
            requestEnd: function (e) {
                kendo.ui.progress($("#divAfiliadosCliente"), false);
            },
            pageSize: 5,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
        },
        filterable: {
            field: "Afiliado.Cliente.Tercero.Nombre",
            operator: "contains"
        },
        template: kendo.template($("#tmpAfiliadosCliente").html()),
    };

    $scope.aliadosSelectOpts = {
        dataSource: {
            transport: {
                read: {
                    dataType: "json",
                    url: $rootScope.baseAddress + "/api/Afiliados/GetAfiliadosCandidatos/" + authorizationService.getId(),
                    beforeSend: $rootScope.beforeSendRequest
                }
            },
            serverFiltering: true,
            filterable: true
        },
        dataTextField: "Cliente.Tercero.Nombre",
        dataValueField: "AfiliadoId"
    };

    $scope.agregarAliado = function () {
        var aliadoCliente = {
            ClienteId: authorizationService.getId(),
            AfiliadoId: $scope.aliado,
            FechaIngreso: new Date(),
            FechaBaja: null
        };
        afiliadosClientesService.post(aliadoCliente).then(
            function (pl) {
                new PNotify({
                    text: 'Se agregó correctamente el aliado',
                    type: 'info',
                    delay: 3000
                });
                $scope.aliados.dataSource.read();
                $scope.cmbAliado.dataSource.read();
            },
            function (errorPl) {
                console.log('Error registrando aliado', errorPl);
            });
    };


}