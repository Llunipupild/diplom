using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class StopExploreEvent : GameEvent
    {
        public const string STOP_EXPLORE_EVENT = "stopExploreEvent";
        
        public StopExploreEvent() : base(STOP_EXPLORE_EVENT)
        {
        }
    }
}