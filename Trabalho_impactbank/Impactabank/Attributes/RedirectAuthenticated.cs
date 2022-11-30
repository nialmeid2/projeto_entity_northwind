using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Impactabank.Attributes
{
    public class RedirectAuthenticated : ActionFilterAttribute
    {
        private string url;
        public RedirectAuthenticated(string url)
        {
            this.url = url;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated) 
            {
                base.OnActionExecuting(context);
                return;
            }
            
            context.HttpContext.Response.Redirect(url); // redirecionar para a url desejada
        
        }
    }
}
