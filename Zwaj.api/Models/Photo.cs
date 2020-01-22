using System;
namespace Zwaj.api.Models
{
    public class Photo
    {
       public int Id { get; set; } 
       public string Url { get; set; }
       public string Description { get; set; }
       public DateTime DateAdded { get; set; }
       public bool Ismain { get; set; } //main photo
       public User User { get; set; }
       public int UserId { get; set; }
    }
}