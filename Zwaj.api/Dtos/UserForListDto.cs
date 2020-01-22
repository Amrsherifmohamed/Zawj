using System;
namespace Zwaj.api.Dtos
{
    public class UserForListDto
    {
         public int Id { get; set; }
        public string Username { get; set; }
        public string  Gender { get; set; }
        public int  age { get; set; }
        public string KnownAs { get; set; }
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
       public string photourl { get; set; }
        
    }
}