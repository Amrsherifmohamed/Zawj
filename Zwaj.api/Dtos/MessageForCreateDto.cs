using System;

namespace Zwaj.api.Dtos
{
    public class MessageForCreateDto
    {
         public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime MessageSent { get; set; }
        public MessageForCreateDto()
        {
            MessageSent=DateTime.Now;
        }
    }
}