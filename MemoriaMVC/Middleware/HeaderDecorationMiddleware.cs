namespace MemoriaMVC.Middleware
{
    public class HeaderDecorationMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderDecorationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
                }
                else
                {
                    // Handle the case when the cookie is not present
                    // For example, you can log a message or perform any other necessary logic
                    Console.WriteLine("JWT token cookie is not present");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await _next(context);
        }


    }
}

