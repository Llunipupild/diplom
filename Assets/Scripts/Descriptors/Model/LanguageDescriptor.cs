using Core.XmlReader.Config;
using Descriptors.Enumer;
using Descriptors.Interface;

namespace Descriptors.Model
{
    public class LanguageDescriptor : IDescriptor
    {
        public string EnglishWord { get; set; }
        public string RussianWord { get; set; }
        public string Image { get; set; }
        public bool NeedShowText { get; set; }
        public WordType WordType { get; set; }

        public void SetData(Configuration config)
        {
            EnglishWord = config.GetString("englishWord");
            RussianWord = config.GetString("russianWord");
            Image = config.GetString("image");
            NeedShowText = config.GetBool("needShowText");
            WordType = WordTypeExtensions.ValueOf(config.GetString("wordType"));
        }

        public void SetData(string englishWord, string russianWord, string iconPath)
        {
            EnglishWord = englishWord;
            RussianWord = russianWord;
            Image = iconPath;
            NeedShowText = true;
            WordType = WordType.OTHER;
        }
    }
}