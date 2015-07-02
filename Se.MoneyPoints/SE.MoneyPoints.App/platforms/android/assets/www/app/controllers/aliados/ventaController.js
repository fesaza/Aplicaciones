(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('ventaController', ['$scope', '$rootScope', '$', 'ventaService', 'tercerosService', ventaController]);

    function ventaController($scope, $rootScope, $, ventaService, tercerosService) {
        $scope.text = "";

        //Se debe cambiar valor para que no este quemado (Usuario logueado
        var clienteId = 3;
        $scope.onlyNumber = /^\d+$/;
        $scope.venta = null;

        var updateGreetingMessage = function (text) {
            $scope.text = text;
        }

        var generarQRCompleted = function (text) {
            $scope.text = text;
        }

        $rootScope.$on("finalizarCompraCompleted", function (e, message) {
            finalizarCompraCallBack(message);
        });

        var finalizarCompraCallBack = function (message) {
            new PNotify({ text: message, type: 'info', delay: 3000 });
        };

        ventaService.initialize();

        $scope.greetAll = function () {
            ventaService.sendRequest();
        }

        $scope.generarQR = function () {

            kendo.ui.progress($("#frmVenta"), true);

            //Limpiar div
            $('#divQR').empty();
            $scope.terceroBeneficiario = null;

            //Validar cedula
            var cedula = $scope.venta.Identificacion
            
            var promiseGetbeneficiario = tercerosService.getByIdentificacion(cedula);

            promiseGetbeneficiario.then(function (pl) {
                var res = pl.data;

                if (res.length == 0) {
                    new PNotify({ text: 'La identificación no existe.', type: 'danger', delay: 3000 });
                } else {
                    var tercero = res[0];
                    if (tercero.Beneficiarios.length == 0) {
                        new PNotify({ text: 'La identificación no esta registrada como beneficiario.', type: 'danger', delay: 3000 });
                        return;
                    }

                    $scope.terceroBeneficiario = tercero;

                    var textQR = ventaService.getConnectionId() + "_" + tercero.Beneficiarios[0].BeneficiarioId + "_" + $scope.venta.Valor + "_" + clienteId;
                    $scope.text = textQR;

                    //generar qr
                    $('#divQR').qrcode({
                        "render": 'div',
                        "width": 100,
                        "height": 100,
                        "color": "#FFFFFF",
                        "mode": 4,
                        "text": textQR
                    });
                }

                kendo.ui.progress($("#frmVenta"), false);
            },
             function (errorPl) {
                 handleError(errorPl);
             });


        }

        $rootScope.$on("acceptGreet", function (e, message) {
            $scope.$apply(function () {
                updateGreetingMessage(message)
            });
        });

        $rootScope.$on("generarQRCompleted", function (e, message) {
            $scope.$apply(function () {
                generarQRCompleted(message)
            });
        });

        var confirmarCompraCallBack = function (message) {
            new PNotify({ text: message + " Esperando PIN.", type: 'info', delay: 3000 });
        };

        $rootScope.$on("confirmarCompra", function (e, message) {
            $scope.$apply(function () {
                confirmarCompraCallBack(message)
            });
        });
    };

})();