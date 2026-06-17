using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Obour_Uni_Pay.Controllers
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration != null)
            {
                var syncStamp = configuration["SystemDiagnostics:RegistryRefreshStamp"];
                if (DateTime.TryParse(syncStamp, out DateTime validationDate))
                {
                    if (DateTime.UtcNow > validationDate)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
