namespace Utils.Constants
{
    public class GameConstants
    {
        //configs
        public const string LANGUAGE_CONFIG = "Configs/words";
        public const string FULL_LANGUAGE_CONFIG_PATH = "/Resources/Configs/words.xml";

        public const string PATH_TO_OTHERS_IMAGES = "Assets/Resources/Images/Others/";
        public const string PATH_TO_OTHERS_IMAGES_WITHOUT_ASSETS = "Images/Others/";
        
        //sounds
        //https://
        public const string SOUND_SOURCE = "Prefabs/Sound/AudioSource";
        public const string URL_GOOGLE_TRANSLATE_PC = "translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";
        //public const string URL_GOOGLE_TRANSLATE_ANDROID = "https://www.translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&q=";
        public const string URL_GOOGLE_TRANSLATE_ANDROID = "translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";

        public const string AUDIO_CONTAINER = "AudioContainer";

        //prefabs
        public const string MAIN_MENU = "Prefabs/Dialogs/MainMenuDialog";
        public const string CARD = "Prefabs/pfCard";
        
        //game
        public const string CARD_CONTAINER = "CardContainer";
        public const string STUDYING_TEXT = "Изучение";
        public const string REPEAT_TEXT = "Повторение";
        public const string SETTING_TEXT = "Добавление";
        public const int COUNT_STARTED_CARDS = 3;
        public const int COUNT_VIEWING_CARDS = 3;
        public const int OFFSET_CARD_Y = 300;
        
        
        //notice
        public const string EXPLORE_PANEL_NOTICE_TEXT = 
        "  На этой панели вы можете выбрать слова для обучения, либо добавить новые." + "\n" +
        "   Для того чтобы добавить новые слова, нажмите кнопку добавить." + "\n" +
        "   Для изучения слов необходимо выбрать доступные темы и нажать кнопку начать.";

        public const string REPEAT_PANEL_NOTICE_TEXT = 
        " Эта панель создана для повторения изученных слов." + "\n" + 
        " Содержатся 3 кнопки, каждая из которых отвечает за 1 колоду." + "\n" + 
        " 1 колода - для ежедневных повторений" + "\n" + 
        " 2 колода - для повторений через день" + "\n" + 
        " 3 колода - для повторений каждые 2 дня";
    }
}