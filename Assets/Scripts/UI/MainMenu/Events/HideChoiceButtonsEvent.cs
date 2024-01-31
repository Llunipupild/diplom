using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class HideChoiceButtonsEvent : GameEvent
    {
        public const string HIDE_CHOICE_BUTTONS = "hideChoiceButtons";
        
        public HideChoiceButtonsEvent() : base(HIDE_CHOICE_BUTTONS)
        {
            
        }
    }
}