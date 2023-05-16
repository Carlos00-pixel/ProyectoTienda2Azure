using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoTienda2.Filters
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute,
        IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            string controller =
                context.RouteData.Values["controller"].ToString();
            string action =
                context.RouteData.Values["action"].ToString();
            string idusuario = "";
            if (context.RouteData.Values.ContainsKey("id"))
            {
                idusuario = context.RouteData.Values["id"].ToString();
            }
            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();
            var TempData = provider.LoadTempData(context.HttpContext);
            TempData["controller"] = controller;
            TempData["action"] = action;
            TempData["id"] = idusuario;

            provider.SaveTempData(context.HttpContext, TempData);

            if (user.Identity.IsAuthenticated == false)
            {
                RouteValueDictionary routeLogin =
                    new RouteValueDictionary(new
                    {
                        controller = "Managed",
                        action = "LoginArtista"
                    });
                context.Result = new RedirectToRouteResult(routeLogin);
            }

        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary route =
                new RouteValueDictionary(new
                {
                    controller = controller
                ,
                    action = action
                });
            return new RedirectToRouteResult(route);
        }
    }
}
