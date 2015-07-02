
(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('conciliacionesController', ['$scope', '$rootScope', '$routeParams', 'conciliacionesService', 'authorizationService', conciliacionesController]);

    function conciliacionesController($scope, $rootScope, $routeParams, conciliacionesService, authorizationService) {


        //ConciliacionAfiliadosClientes
        $scope.conciliacionAliadosGridOptions = {
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/Conciliaciones/ConciliacionesByCliente/" + authorizationService.getId(),
                        dataType: "json",
                        beforeSend: $rootScope.beforeSendRequest
                    }
                },
                requestStart: function (e) {
                    kendo.ui.progress($("#divConciliacionesEnvia"), true);
                },
                requestEnd: function (e) {
                    kendo.ui.progress($("#divConciliacionesEnvia"), false);
                },
                group: "Estado",
                pageSize: 5,
                serverPaging: true,
                serverSorting: true
            },
            sortable: true,
            pageable: true,
            filterable: {
                field: "AfiliadosCliente.Afiliado.Cliente.Tercero.Nombre",
                operator: "contains"
            },
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
                        "AfiliadosCliente.Cliente.Tercero.Nombre": { type: "string" },
                        "Fecha": { type: "date" },
                        "Valor": { type: "number" }
                    }
                }
            },
            headerTemplate: "${value}",
            template: kendo.template($("#tmpConciliacionxAliado").html()),
        };


        $scope.conciliacionClientesGridOptions = {
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/Conciliaciones/ConciliacionesByAliado/" + authorizationService.getId(),
                        dataType: "json",
                        beforeSend: $rootScope.beforeSendRequest
                    }
                },
                group: "Estado",
                pageSize: 5,
                serverPaging: true,
                serverSorting: true
            },
            sortable: true,
            pageable: true,
            filterable: {
                field: "AfiliadosCliente.Cliente.Tercero.Nombre",
                operator: "contains"
            },
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
                        "AfiliadosCliente.Cliente.Tercero.Nombre": { type: "string" },
                        "Fecha": { type: "date" },
                        "Valor": { type: "number" }
                    }
                }
            },
            headerTemplate: "${value}",
            template: kendo.template($("#tmpConciliacionxCliente").html()),
        };
    };

})();