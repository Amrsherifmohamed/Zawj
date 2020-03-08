using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Zwaj.api.Models
{
    public class User :IdentityUser<int>
    {
        
        public string  Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
       public ICollection<Photo> Photos { get; set; }
       public ICollection<Like> Likers { get; set; }
       public ICollection<Like> Likees { get; set; }
       public ICollection<Message> MassageSent { get; set; }
       public ICollection<Message> MassageRecived  { get; set; }
       public ICollection<UserRole> UserRoles { get; set; }
    }
}