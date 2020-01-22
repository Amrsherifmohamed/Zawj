using System; 
using System.Collections.Generic;
using Zwaj.api.Models;

namespace Zwaj.api.Dtos
{
    public class UserForDetailsDto
    {
         public int Id { get; set; }    
        public string Username { get; set; }
       
        public string  Gender { get; set; }
        public int  age { get; set; }
        public string KnownAs { get; set; }
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string photourl { get; set; }
        public ICollection<PhotoForDetailsDto> Photos { get; set; }
       
    }
}