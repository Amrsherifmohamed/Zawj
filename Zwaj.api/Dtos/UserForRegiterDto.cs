using System;
using System.ComponentModel.DataAnnotations;

namespace Zwaj.api.Dtos
{
    public class UserForRegiterDto
    {
        [Required]
        public string Username { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="This Password Is Small and weak")]
        [Required]
        public string Password { get; set; }
        [Required]
        public string  Gender { get; set; }
        [Required]
        public string  KnownAs { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        [Required] 
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public UserForRegiterDto()
        {
            created=DateTime.Now;
            lastActive=DateTime.Now;
        }
    }
}