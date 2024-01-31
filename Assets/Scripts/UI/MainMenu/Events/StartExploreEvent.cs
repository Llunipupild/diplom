using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class StartExploreEvent : GameEvent
    {
        public const string START_EXPLORE = "startExplore";
        
        public StartExploreEvent() : base(START_EXPLORE)
        {
            
        }
    }
}