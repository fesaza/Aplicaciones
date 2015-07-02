(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('detallesCuentaBeneficiarioController', ['$rootScope', '$scope', '$routeParams', detallesCuentaBeneficiarioController]);

    function detallesCuentaBeneficiarioController($rootScope, $scope, $routeParams) {

        var beneficiariosClienteId = $routeParams.beneficiariosClienteId

        $scope.historialPtsBeneficiarioOptions = {

            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/BeneficiariosPuntos?$filter= BeneficiariosClienteId eq " + beneficiariosClienteId, dataType: "json",
                        beforeSend: $rootScope.beforeSendRequest
                    },
                    parameterMap: function (options, type) {
                        var paramMap = kendo.data.transports.odata.parameterMap(options);
                        delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                        return paramMap;
                    }
                },
                pageSize: 50,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                requestStart: function (e) {
                    kendo.ui.progress($("#gridHistorialPtsBeneficiarios"), true);
                },
                requestEnd: function (e) {
                    kendo.ui.progress($("#gridHistorialPtsBeneficiarios"), false);
                },
            },
            sortable: true,
            pageable: true,
            filterable: true,
            scrollable: {
                virtual: true
            },
            schema: {
                data: function (data) {
                    return data;
                },
                total: function (data) {
                    return data.lenght;
                },
                model: {
                    fields: {
                        "Fecha": { type: "date" },
                        "Puntos": { type: "number" },
                        "Valor": { type: "number" },
                    }
                }
            },
            template: kendo.template($("#tmpdetallesCtaBeneficiario").html()),
        };

        //Consultar historial de redenciones
        //BeneficiariosClientesCompras
        $scope.historialTxBeneficiarioOptions = {

            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/BeneficiariosClientesCompras?$filter= BeneficiariosClienteId eq " + beneficiariosClienteId, dataType: "json",
                        beforeSend: $rootScope.beforeSendRequest
                    },
                    parameterMap: function (options, type) {
                        var paramMap = kendo.data.transports.odata.parameterMap(options);
                        delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                        return paramMap;
                    }
                },
                pageSize: 50,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                requestStart: function (e) {
                    kendo.ui.progress($("#gridHistorialTxBeneficiarios"), true);
                },
                requestEnd: function (e) {
                    kendo.ui.progress($("#gridHistorialTxBeneficiarios"), false);
                },
            },
            filterable: {
                field: "Compra.Afiliado.Cliente.Tercero.Nombre",
                operator: "contains"
            },
            template: kendo.template($("#tmpHistorialTxBeneficiario").html()),
        };
    };


})();