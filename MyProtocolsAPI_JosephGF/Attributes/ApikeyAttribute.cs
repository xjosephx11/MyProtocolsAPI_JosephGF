using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyProtocolsAPI_JosephGF.Attributes
{
    //esta clase ayuda a limitar la forma en que se puede consumir un recurso de controlador
    //(un end point.) basicamente vamos a crear una decoracion personalizada que
    //inyecta cierta funcionabilidad ya sea a todo un controller o a un end point particular

    [AttributeUsage(validOn: AttributeTargets.All)]
    public sealed class ApikeyAttribute : Attribute, IAsyncActionFilter
    {
        //especificamos cual es el clave:valor dentro de appsettings que queremos usar como apikey
        private readonly string _apiKey = "Progra6Apikey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //aca validamos que en el body (en tipo json) del request vaya la info de la apikey
            //si no va la info, presentamos un mensaje de error indicado que falta la apikey
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKey, out var ApiSalida))//si no viene este valor dentro del header
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Llamada no contiene informacion de seguridad..."
                };
                return;
                //si no hay info de seguridad sale de la funcion y muestra este mensaje
            }
            //si viene info de seguridad falta validad que sea la correcta
            //para esto lo primero es extraer el valor de progra6apikey dentro de appisettings.json
            //para poder comparar contro lo que viene en el request 
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var ApikeyValue = appSettings.GetValue<string>(_apiKey);

            //queda comparar que las apikey sean iguales
            if (!ApikeyValue.Equals(ApiSalida))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Apikey Invalida..."
                };
               return;
            }
            await next();
        }









    }
}
