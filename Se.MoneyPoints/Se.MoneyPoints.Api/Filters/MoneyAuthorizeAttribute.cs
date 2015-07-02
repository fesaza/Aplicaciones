using Se.MoneyPoints.Model.Bussiness.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Se.MoneyPoints.Api.Filters
{

    public class MoneyAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public static List<Usuario> AllowedUsers = new List<Usuario>();

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Si cuenta con el atributo AllowAnonymous no valida las credenciales
            var skipAuth = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
               || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (skipAuth) return;

            //Case that user is authenticated using forms authentication
            //so no need to check header for basic authentication.
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var credArray = GetCredentials(authHeader);
                    var userName = credArray[0];
                    var password = credArray[1];

                    //encriptar clave
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
                    password = System.Convert.ToBase64String(plainTextBytes);

                    if (IsResourceOwner(userName, actionContext))
                    {
                        //You can use Websecurity or asp.net memebrship provider to login, for
                        //for he sake of keeping example simple, we used out own login functionality
                        if (ValidateUser(userName, password))
                        {
                            var currentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                            Thread.CurrentPrincipal = currentPrincipal;
                            return;
                        }
                    }
                }
                else
                {
                    throw new Exception("basic comparacion mala");
                }
            }
            else
            {
                throw new Exception("Algo anda mal con Headers Authorization");
            }

            HandleUnauthorizedRequest(actionContext);
        }

        private bool ValidateUser(string userName, string password)
        {
            if (AllowedUsers.FirstOrDefault(u => u.Login == userName && u.Password == password) != null)
            {
                return true;
            }
            else
            {
                using (var entities = new MoneyPoints_dlloEntities())
                {
                    var user = entities.Usuarios.FirstOrDefault(u => u.Login == userName && u.Password == password);

                    return !(user == null);
                }
            }
        }

        private string[] GetCredentials(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {

            //Base 64 encoded string
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));

            var credArray = cred.Split(':');

            return credArray;
        }

        private bool IsResourceOwner(string userName, System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //var routeData = actionContext.Request.GetRouteData();
            //var resourceUserName = routeData.Values["userName"] as string;

            //if (resourceUserName == userName)
            //{
            return true;
            //}
            //return false;
        }

        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            actionContext.Response.Headers.Add("WWW-Authenticate",
            "Basic Scheme='money' location='/login'");
        }
    }

}