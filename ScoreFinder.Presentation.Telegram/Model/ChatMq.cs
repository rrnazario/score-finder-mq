using System;

namespace ScoreFinder.Presentation.Telegram.Model
{
    public class ChatMq
    {
        public long ChatId { get; set; }
        public string QueueName { get; set; }
        public DateTime LastQuery { get; set; }
    }
}
