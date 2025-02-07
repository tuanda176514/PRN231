
namespace JollyCustomerClient.MiddleWare
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //int? userId = context.Session.GetInt32("userId");

            ////check if not logged in and route is not / signin => redirect to signin
            //if (userId == null && !context.Request.Path.Equals("/auth/login"))
            //{
            //    // No active session, redirect to login page
            //    context.Response.Redirect("/auth/login");
            //    return;
            //}else if (userId != null && context.Request.Path.Equals("/auth/login"))
            //{
            //    context.Response.Redirect("/");
            //    return;
            //}
            await _next(context);
        }
    }
}
