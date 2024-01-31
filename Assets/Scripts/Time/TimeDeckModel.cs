using System;

namespace Time
{
    public class TimeDeckModel
    {
        public TimeDeckModel()
        {
            DateTimeDeck1 = new DateTime();
            DateTimeDeck2 = new DateTime();
            DateTimeDeck3 = new DateTime();
        }
        
        public DateTime DateTimeDeck1 { get; set; }
        public DateTime DateTimeDeck2 { get; set; }
        public DateTime DateTimeDeck3 { get; set; }
    }
}