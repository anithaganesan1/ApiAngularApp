namespace ApiAngularApp
{
    public static class MyMiddlewareExtensionsBase
    {
        public static IApplicationBuilder UseCustMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustMiddleware>();
        }
    }
}