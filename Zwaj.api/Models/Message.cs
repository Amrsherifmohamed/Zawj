using System;

namespace Zwaj.api.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public User Sender { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DataRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDelated { get; set; }
        public bool RecipientDeleted { get; set; }

    }
}