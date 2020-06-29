using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Presentation.Telegram.Model
{
    public class ChatMq
    {
        public long ChatId { get; set; }
        public string QueueName { get; set; }
        public DateTime LastQuery { get; set; }
    }
}
