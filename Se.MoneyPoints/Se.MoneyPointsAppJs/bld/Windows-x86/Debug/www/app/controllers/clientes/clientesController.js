(function () {
    'use strict';


    angular.module("moneyPointsApp").controller('clientesController', ['$http', '$scope', '$rootScope', 'clientesService', clientesController]);

    function clientesController($http, $scope, $rootScope, clientesService) {

        $scope.clientesGridOptions = {
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/clientes",
                        dataType: "json",
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
            },
            sortable: true,
            pageable: true,
            filterable: {
                field: "Tercero.Nombre",
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
                    return 6;
                },
                model: {
                    fields: {
                        "Tercero.Nombre": { type: "string" },
                        "Tercero.Identificacion": { type: "number" },
                        "Tercero.Telefono": { type: "string" },
                    }
                }
            },
            columns: [{
                field: "Tercero.Nombre",
                title: "Razón Social"
            },
            {
                field: "Tercero.Identificacion",
                title: "Nit"
            },
            {
                field: "Tercero.Telefono",
                title: "Teléfono"
            }],
            template: kendo.template($("#tmpClientes").html()),
        };


    };


})();