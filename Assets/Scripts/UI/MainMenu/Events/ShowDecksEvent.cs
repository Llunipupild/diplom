using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class ShowDecksEvent : GameEvent
    {
        public const string SHOW_DECKS_EVENT = "showDecksEvent";
        
        public ShowDecksEvent() : base(SHOW_DECKS_EVENT)
        {
        }
    }
}