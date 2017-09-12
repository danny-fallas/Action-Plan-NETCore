using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Braintree.NETCore.API.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // Do something you need
            context.Response.Cookies.Append("token", "test_token_12345");

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }

    // Expose middleware through IApplicationBuilder
    public static class BraintreeMiddlewareExtensions
    {
        public static IApplicationBuilder UseBraintreeToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
