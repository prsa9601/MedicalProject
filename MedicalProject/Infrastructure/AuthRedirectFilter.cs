using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MedicalProject.Infrastructure
{
    public class AuthRedirectFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            // No implementation needed
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var result = await next();

            if (result.Exception != null)
            {
                var exception = result.Exception;
                if (exception is UnauthorizedAccessException ||
                    (exception is HttpRequestException httpEx && httpEx.StatusCode == HttpStatusCode.Unauthorized))
                {
                    var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                    context.Result = new RedirectToPageResult("/Auth/Login", new { redirectTo = returnUrl });
                    result.ExceptionHandled = true;
                }
            }
        }

    }
}
