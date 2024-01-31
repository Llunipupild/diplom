using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class HideDecksEvent : GameEvent
    {
        public const string HIDE_DECKS_EVENT = "hideDecksEvent";
        
        public HideDecksEvent() : base(HIDE_DECKS_EVENT)
        {
        }
    }
}