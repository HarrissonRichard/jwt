using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Twwet.Models;

namespace Tweet.Services

{
    public class AuthMiddlewareHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new AuthorizationMiddlewareResultHandler();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new ErrorResponseModel(ResponseCode.UnAuthorized, "UnAuthorized token, please log in to acces"));
                return;

                //authentic
                //true when token is invalid
                //and false when token is authentic
            }

            if (authorizeResult.Forbidden)
            {
                //true when token valid but user does not have permission to access the resources asked

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(new ErrorResponseModel(ResponseCode.Forbidden, "You Dont Have rights acces this resource"));
                return;
            }

            await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);


        }
    }
}