using System;

namespace SaveWords.Model
{
    [Serializable]
    public class NewWords
    {
        public string EnglishWord { get; set; }
        public string RussianWord { get; set; }
        public string ImagePath { get; set; }
    }
}