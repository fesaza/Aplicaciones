using Se.MoneyPoints.Api.Filters;
using Se.MoneyPoints.Model.Bussiness.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace Se.MoneyPoints.Api.Controllers
{
    public class AuthenticationController : ApiController
    {
        [Route("api/Authentication/ChangePassword")]
        public IHttpActionResult ChangePassword(ChangePassword cambioClave)
        {
            if (string.IsNullOrEmpty(cambioClave.Password)) throw new Exception("Ingrese la clave actual");

            if (cambioClave.Password == cambioClave.NewPassword) throw new Exception("La clave nueva no debe ser igual a la anterior");

            if (cambioClave.NewPassword != cambioClave.NewPasswordConfirm) throw new Exception("La clave nueva no coincide");

            var bytesPwd = Convert.FromBase64String(cambioClave.NewPassword);
            var Pwd = Encoding.UTF8.GetString(bytesPwd);

            if (Pwd.Length < 4) throw new Exception("la nueva clave es muy corta");

            using (var entities = new MoneyPoints_dlloEntities())
            {
                var user = entities.Usuarios.FirstOrDefault(u => u.UsuarioId == cambioClave.UsuarioId && u.Password == cambioClave.Password);

                if (user == null) throw new Exception("Clave actual incorrecta");

                user.Password = cambioClave.NewPassword;
                entities.Entry(user).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }

            return Ok();
        }

        [Route("api/Authentication/ChangePIN")]
        public IHttpActionResult ChangePIN(ChangePassword cambioClave)
        {
            if (cambioClave.PIN == cambioClave.NewPIN) throw new Exception("El nuevo PIN no debe ser igual al anterior");

            if (cambioClave.NewPIN != cambioClave.ConfirmPIN) throw new Exception("El nuevo PIN no coincide");

            try
            {
                var bytesPIN = System.Convert.FromBase64String(cambioClave.NewPIN);
                var PIN = Encoding.UTF8.GetString(bytesPIN);

                if (PIN.Length != 4) throw new Exception("El PIN debe ser de cuatro digitos");
            }
            catch (Exception)
            {
                throw new Exception("No se pudo decodificar la clave, debe estar en base 64");
            }

            using (var entities = new MoneyPoints_dlloEntities())
            {
                var user = entities.Usuarios.FirstOrDefault(u => u.UsuarioId == cambioClave.UsuarioId);

                if (user == null) throw new Exception("Usuario no existe");

                var benef = user.Tercero.Beneficiarios.ToList()[0];

                if (benef.Pin != cambioClave.PIN) throw new Exception("PIN incorrecto");

                benef.Pin = cambioClave.NewPIN;

                entities.Entry(benef).State = System.Data.Entity.EntityState.Modified;

                entities.SaveChanges();
            }

            return Ok();
        }

        // POST api/Authentication/Authenticate
        [AllowAnonymous]
        [ResponseType(typeof(Usuario))]
        [Route("api/Authentication/Authenticate")]
        public IHttpActionResult Authenticate(Usuario usuario)
        {
            //MoneyAuthorizeAttribute.AllowedUsers.Add(usuario);

            try
            {
                using (var entities = new MoneyPoints_dlloEntities())
                {
                    var user = entities.Usuarios.FirstOrDefault(u => u.Login == usuario.Login && u.Password == usuario.Password);

                    if (user == null)
                    {
                        return BadRequest("Usuario y/o clave incorrectas.");
                    }

                    usuario.UsuarioId = user.UsuarioId;
                    usuario.RolId = user.RolId;
                    usuario.TerceroId = user.TerceroId;

                    MoneyAuthorizeAttribute.AllowedUsers.Add(user);

                    return Ok(usuario);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //logout

    }

    public class UserMoney
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }

    public class ChangePassword
    {
        public int UsuarioId { get; set; }

        public string PIN { get; set; }

        public string NewPIN { get; set; }

        public string ConfirmPIN { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirm { get; set; }
    }

}
