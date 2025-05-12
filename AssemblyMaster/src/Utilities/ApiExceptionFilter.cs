using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AssemblyMaster.Utilities
{
    public class ApiExceptionFilter : IExceptionFilter, IFilterMetadata
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new { message = context.Exception.Message })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
