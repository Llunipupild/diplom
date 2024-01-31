using System.Collections.Generic;
using Core.Events.Model;

namespace UI.MainMenu.Events
{
    public class ShowChoiceButtonsEvent : GameEvent
    {
        public const string SHOW_CHOICE_BUTTONS = "showChoiceButtons";
        public List<string> Words { get; }
        public string TrueAnswer { get; }
        public string TrueAnswerEnglish { get; }
        
        public ShowChoiceButtonsEvent(List<string> words, string trueAnswer, string trueAnswerEnglish) : base(SHOW_CHOICE_BUTTONS)
        {
            Words = words;
            TrueAnswer = trueAnswer;
            TrueAnswerEnglish = trueAnswerEnglish;
        }
    }
}