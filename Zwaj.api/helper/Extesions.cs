using Microsoft.AspNetCore.Http;
using System;

namespace Zwaj.api.helper
{
    public static class Extesions
    {
        public static void AddApplicationError(this HttpResponse response,string massage){
        response.Headers.Add("Application-Error",massage);
        response.Headers.Add("Access-Control-expose-Headers","Application-Error");
        response.Headers.Add("Access-Control-Allow-Origin","*");

        }
        public static int calculateage(this DateTime datetime){
            var age =DateTime.Today.Year-datetime.Year;
            if(datetime.AddYears(age)>DateTime.Today) age--;
            return age;
        }

    }
}