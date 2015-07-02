(function () {
    'use strict';


    angular.module("moneyPointsApp").controller('beneficiariosController', ['$scope', '$rootScope', 'beneficiariosService', beneficiariosController]);

    function beneficiariosController($scope, $rootScope, beneficiariosService) {
        $scope.beneficiariosGridOptions = {
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $rootScope.baseAddress + "/api/beneficiarios", dataType: "json",
                        beforeSend: $rootScope.beforeSendRequest
                    },
                    parameterMap: function (options, type) {
                        var paramMap = kendo.data.transports.odata.parameterMap(options);
                        delete paramMap.$inlinecount; // <-- quitar parametro inlinecount
                        return paramMap;
                    }
                },
                requestStart: function (e) {
                    kendo.ui.progress($("#gridBeneficiarios"), true);
                },
                requestEnd: function (e) {
                    kendo.ui.progress($("#gridBeneficiarios"), false);
                },
                pageSize: 5,
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
                        "Tercero.Apellidos": { type: "string" },
                        "Tercero.Telefono": { type: "string" },
                    }
                }
            },
            template: kendo.template($("#rowBeneficiariosTemplate2").html()),
        };
    };
})();