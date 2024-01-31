using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class UnlockDecksAfterExplore : GameEvent
    {
        public const string UNLOCK_DECKS = "unlockDecks";
        
        public UnlockDecksAfterExplore() : base(UNLOCK_DECKS)
        {
        }
    }
}