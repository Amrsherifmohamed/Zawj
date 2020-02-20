using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        public static void AddPagination(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems,int totalPages)
       {
           var paginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
           var camelCaseFormatter= new JsonSerializerSettings();
           camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
           response.Headers.Add("Pagination",JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
           response.Headers.Add("Access-Control-Expose-Headers","Pagination");

       }
    //      public static void AddPagination(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems,int totalPages)
    //    {
    //        var paginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
    //     //  var camelCaseFormatter= new JsonSerializerSettings();
    //     //  camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //        response.Headers.Add("Pagination",JsonConvert.SerializeObject(paginationHeader/*,camelCaseFormatter*/));
    //        response.Headers.Add("Access-Control-Expose-Headers","Pagination");

    //    }
        public static int calculateage(this DateTime datetime){
            var age =DateTime.Today.Year-datetime.Year;
            if(datetime.AddYears(age)>DateTime.Today) age--;
            return age;
        }

    }
}