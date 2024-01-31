using System.Xml;
using Core.IoC.AttributeInject;
using Core.XmlReader.Service;
using Descriptors.Model;
using Descriptors.Service;
using SaveWords.Model;
using SaveWords.Repositroy;
using UnityEngine;
using Utils.Constants;

namespace SaveWords.Service
{
    public class SaveWordsService : MonoBehaviour
    {
        private XmlBuilder _xmlBuilder = new();

        [Dependence] 
        private DescriptorService _descriptorService;
        [Dependence] 
        private SaveNewWordRepository _saveNewWordRepository;
        
        public void SaveWord(string englishWord, string russianWord, string iconPath)
        {
            XmlDocument xmlDocument = _xmlBuilder.CreateXmlDocument(GameConstants.LANGUAGE_CONFIG);
            XmlElement xmlElementMain = xmlDocument.CreateElement("word");
            
            XmlElement xmlElementEnglishWord = xmlDocument.CreateElement("englishWord");
            XmlElement xmlElementRussianWord = xmlDocument.CreateElement("russianWord");
            XmlElement xmlElementIconPath = xmlDocument.CreateElement("image");
            XmlElement xmlElementWordType = xmlDocument.CreateElement("wordType");
            XmlElement xmlElementNeedShowText = xmlDocument.CreateElement("needShowText");
            
            xmlElementEnglishWord.InnerText = englishWord;
            xmlElementRussianWord.InnerText = russianWord;
            xmlElementIconPath.InnerText = iconPath;
            xmlElementWordType.InnerText = "other";
            xmlElementNeedShowText.InnerText = "true";

            xmlElementMain.AppendChild(xmlElementEnglishWord);
            xmlElementMain.AppendChild(xmlElementRussianWord);
            xmlElementMain.AppendChild(xmlElementIconPath);
            xmlElementMain.AppendChild(xmlElementWordType);
            xmlElementMain.AppendChild(xmlElementNeedShowText);

            xmlDocument.DocumentElement!.AppendChild(xmlElementMain);
            if (SystemInfo.deviceType == DeviceType.Desktop) {
               string pcPath = UnityEngine.Application.dataPath + GameConstants.FULL_LANGUAGE_CONFIG_PATH;
               Debug.Log("Path" + $"{pcPath}");
               xmlDocument.Save(pcPath);
            } else {
                NewWordsModel newWordsModel = _saveNewWordRepository.Get();
                NewWords newWords = new NewWords
                {
                    EnglishWord = englishWord,
                    RussianWord = russianWord,
                    ImagePath = iconPath
                };
                newWordsModel.AddNewWord(newWords);
                _saveNewWordRepository.Set(newWordsModel);
            }
            
            _descriptorService.LoadDescriptors(typeof(LanguageDescriptor), GameConstants.LANGUAGE_CONFIG);;
        }
    }
}