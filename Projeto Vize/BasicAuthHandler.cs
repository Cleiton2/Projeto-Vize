using System.Text;

namespace Projeto_Vize
{
    public class BasicAuthHandler(RequestDelegate next)
    {
        private readonly RequestDelegate next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Usuário não autorizado");
                return;
            }

            string header = context.Request.Headers.Authorization.ToString();
            string encodedCreds = header[6..];
            string creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));
            string[] uidpwd = creds.Split(':');
            string idusuario = uidpwd[0];
            string senha = uidpwd[1];

            if(idusuario != "Admin" || senha != "admin2024")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Usuário não autorizado");
                return;
            }

            await next(context);
        }
    }
}