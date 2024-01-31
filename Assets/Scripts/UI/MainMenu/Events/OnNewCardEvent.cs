using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class OnNewCardEvent : GameEvent
    {
        public const string ON_NEW_CARD = "onNewCard";

        public int CurrentCardIndex { get; }
        public int AllCardsCount { get; }
        
        public OnNewCardEvent(int currentCardIndex, int allCardsCount) : base(ON_NEW_CARD)
        {
            CurrentCardIndex = currentCardIndex;
            AllCardsCount = allCardsCount;
        }
    }
}