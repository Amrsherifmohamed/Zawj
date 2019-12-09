using System.ComponentModel.DataAnnotations;

namespace Zwaj.api.Dtos
{
    public class UserForRegiterDto
    {
        [Required]
        public string Username { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="This Password Is Small and weak")]
        public string Password { get; set; }
    }
}