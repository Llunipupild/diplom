using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Core.IoC.AttributeInject;
using Core.XmlReader.Config;
using Core.XmlReader.Service;
using Descriptors.Enumer;
using Descriptors.Interface;
using Descriptors.Model;
using SaveWords.Model;
using SaveWords.Repositroy;
using UnityEngine;
using Utils.Constants;

namespace Descriptors.Service
{
    public class DescriptorService : MonoBehaviour
    {
        private static XmlBuilder _xmlBuilder = new();
        
        private Dictionary<Type, List<IDescriptor>> _descriptors = new();

        [Dependence] 
        private SaveNewWordRepository _saveNewWordRepository;

        private void Start()
        {
            Debug.Log("DescriptorService start");
            LoadDescriptors(typeof(LanguageDescriptor), GameConstants.LANGUAGE_CONFIG);
            Debug.Log("Descriptors loaded");
        }
        
        public List<T> GetAllDescriptors<T>() where T : class
        {
            Type type = typeof(T);
            if (!_descriptors.ContainsKey(type)) {
                throw new NullReferenceException($"Descriptors with type not found. Type={type.Name}");
            }

            return _descriptors[type].Cast<T>().ToList() ?? throw new InvalidCastException($"Dont cast descriptors. Type={type}");
        }

        public List<LanguageDescriptor> GetDescriptorByWords(List<string> words)
        {
            List<LanguageDescriptor> result = new List<LanguageDescriptor>();
            List<LanguageDescriptor> languageDescriptors = GetAllDescriptors<LanguageDescriptor>();
            foreach (LanguageDescriptor languageDescriptor in languageDescriptors)
            {
                if (!words.Contains(languageDescriptor.EnglishWord)) {
                    continue;
                }
                
                result.Add(languageDescriptor);
            }

            return result;
        }

        public List<LanguageDescriptor> GetDescriptorsWithWordType(List<WordType> categories)
        {
            List<LanguageDescriptor> result = new List<LanguageDescriptor>();
            List<LanguageDescriptor> languageDescriptors = GetAllDescriptors<LanguageDescriptor>();
            foreach (LanguageDescriptor languageDescriptor in languageDescriptors)
            {
                if (!categories.Contains(languageDescriptor.WordType)) {
                    continue;
                }
                
                result.Add(languageDescriptor);
            }

            return result;
        }

        public void LoadDescriptors(Type type, string configPath)
        {
            _descriptors = new(); 
            List<IDescriptor> descriptors = new();
            XmlDocument config = _xmlBuilder.CreateXmlDocument(configPath);
            List<Configuration> configurations = _xmlBuilder.LoadConfiguration(config);

            foreach (Configuration item in configurations) {
                IDescriptor descriptor = (IDescriptor) Activator.CreateInstance(type);
                descriptor.SetData(item);
                descriptors.Add(descriptor);
            }

            NewWordsModel newWordsModel = _saveNewWordRepository.Get();
            foreach (NewWords newWords in newWordsModel.NewWordsList)
            {
                IDescriptor descriptor = (IDescriptor) Activator.CreateInstance(type);
                descriptor.SetData(newWords.EnglishWord, newWords.RussianWord, newWords.ImagePath);
                descriptors.Add(descriptor);
            }

            _descriptors.Add(type, descriptors);
        }
    }
}