using Microsoft.AspNetCore.Http;

namespace Zwaj.api.helper
{
    public static class Extesions
    {
        public static void AddApplicationError(this HttpResponse response,string massage){
        response.Headers.Add("Application-Error",massage);
        response.Headers.Add("Access-Control-expose-Headers","Application-Error");
        response.Headers.Add("Access-Control-Allow-Origin","*");

        }

    }
}