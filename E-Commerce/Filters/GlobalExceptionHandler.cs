using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

public class GlobalExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(HttpActionExecutedContext context)
    {

        if (context.Exception is UnauthorizedAccessException)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Unauthorized access. Please provide valid credentials."),
                ReasonPhrase = "Unauthorized"
            };
        }
        else if (context.Exception is ArgumentException)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(context.Exception.Message),
                ReasonPhrase = "Bad Request"
            };
        }
        else
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unexpected error occurred. Please try again later."),
                ReasonPhrase = "Internal Server Error"
            };
        }
    }
}
