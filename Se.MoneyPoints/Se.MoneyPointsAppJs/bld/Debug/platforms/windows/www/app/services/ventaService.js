var app = angular.module('moneyPointsApp');
app.value('$', $);
app.service('ventaService', function ($, $rootScope) {
    var money;
    var initialize = function () {
        //Getting the connection object
        $.connection.hub.url = $rootScope.baseAddress + "/signalr";
        money = $.connection.money;

        money.client.confirmarCompra = function (message) {
            $rootScope.$emit("confirmarCompra", message);
        };

        money.client.finalizarCompraCompleted = function (message) {
            $rootScope.$emit("finalizarCompraCompleted", message);
        };

        //Starting connection
        $.connection.hub.start({ jsonp: true }).done(function () {
            $rootScope.$emit("startCompleted");
        });
    };

    var getConnectionId = function () {
        return $.connection.hub.id;
    };

    var sendRequest = function () {
        money.server.greetAll();
    };

    var generarQR = function (cedula, valor) {
        money.server.generarQR(cedula, valor);
    };

    var sincronizarCompra = function (data) {
        money.server.sincronizarCompra(data);
    }

    var finalizarCompra = function (venta) {
        money.server.finalizarCompra(venta);
    };

    return {
        initialize: initialize,
        sendRequest: sendRequest,
        generarQR: generarQR,
        getConnectionId: getConnectionId,
        sincronizarCompra: sincronizarCompra,
        finalizarCompra: finalizarCompra
    };
});


//angular.module("moneyPointsApp").service('ventaService', function ($, $rootScope) {
//var proxy = null;
//var connection = null;

//var initialize = function () {
//    //Getting the connection object
//    connection = $.hubConnection();

//    //Creating proxy
//    this.proxy = connection.createHubProxy('moneyPointsHub');

//    //Starting connection
//    connection.start();

//    //Publishing an event when server pushes a greeting message
//    this.proxy.on('acceptGreet', function (message) {
//        $rootScope.$emit("acceptGreet", message);
//    });
//};

//var sendRequest = function () {
//    //Invoking greetAll method defined in hub
//    this.proxy.invoke('greetAll');
//};

//return {
//    initialize: initialize,
//    sendRequest: sendRequest
//};
//});