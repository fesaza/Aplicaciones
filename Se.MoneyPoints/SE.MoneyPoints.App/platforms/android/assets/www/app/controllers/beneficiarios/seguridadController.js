(function () {
    'use strict';

    angular.module("moneyPointsApp").controller('seguridadController',
        ['$scope', '$rootScope', '$routeParams', 'authorizationService', 'authenticationService', 'Base64', compraController]);

    function compraController($scope, $rootScope, $routeParams, authorizationService, authenticationService, Base64) {
        //var beneficiarioId = $routeParams.beneficiarioId
        var user = authorizationService.getCurrentUser();

        $scope.canCambiarPIN = true;

        if (user && user.rolId != 4) {
            $scope.canCambiarPIN = false;
        }

        $scope.cambiarClave = function () {
            var sec = $scope.security;

            var opts = {
                UsuarioId: user.usuarioId,
                Password: Base64.encode(sec.clave),
                newPassword: Base64.encode(sec.newClave),
                newPasswordConfirm: Base64.encode(sec.newClaveConfirm)
            };

            authenticationService.changePassword(opts, changePasswordCompleted);
        };



        function changePasswordCompleted(response) {
          
            if (response.success == false) {
                new PNotify({ text: "Error: " + response.Message, type: "danger", delay: 3000 });
            } else {               
                var sec = $scope.security;
                sec.clave = null;
                sec.newClave = null;
                sec.newClaveConfirm = null;
                sec.PIN = null;
                sec.newPIN = null;
                sec.newPINConfirm = null;
                new PNotify({ text: "La información se modificó satisfactoriamente", type: "info", delay: 3000 });
            }
        }

        $scope.cambiarPIN = function () {
         
            var sec = $scope.security;

            var opts = {
                UsuarioId: user.usuarioId,
                PIN: Base64.encode(sec.PIN),
                NewPIN: Base64.encode(sec.newPIN),
                ConfirmPIN: Base64.encode(sec.newPINConfirm)
            };

            authenticationService.changePIN(opts, changePasswordCompleted);
        };
    };
})();