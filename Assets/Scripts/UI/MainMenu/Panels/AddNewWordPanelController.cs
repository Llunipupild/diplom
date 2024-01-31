using System;
using System.Collections.Generic;
using System.IO;
using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using Cysharp.Threading.Tasks;
using Descriptors.Model;
using Descriptors.Service;
using SaveWords.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Constants;
using Random = UnityEngine.Random;

namespace UI.MainMenu.Panels
{
    public class AddNewWordPanelController : MonoBehaviour
    {
        [ComponentBinding("OpenBrowserButton")] 
        private Button _loadImageButton;
        [ComponentBinding("SaveWordButton")] 
        private Button _saveWordButton;
        [ComponentBinding("LoadedImage")] 
        private RawImage _loadedImage;
        [ComponentBinding("EnglishWordInputField")] 
        private TMP_InputField _englishWordInputField;
        [ComponentBinding("RussianWordInputField")] 
        private TMP_InputField _russianWordInputField;

        [ObjectBinding("TextPanel")] 
        private GameObject _textPanel;
        [ComponentBinding("ErrorText")] 
        private TextMeshProUGUI _textField;
        
        [Dependence] 
        private SaveWordsService _saveWordsService;
        [Dependence] 
        private DescriptorService _descriptorService;

        private string _imagePath;
        
        private void Start()
        {
            _loadImageButton.onClick.AddListener(OnLoadImageButtonCLick);
            _saveWordButton.onClick.AddListener(OnSaveButtonClick);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _loadImageButton.onClick.RemoveListener(OnLoadImageButtonCLick);
            _saveWordButton.onClick.RemoveListener(OnSaveButtonClick);
        }

        private void OnLoadImageButtonCLick()
        {
            NativeGallery.GetImageFromGallery(OnGetImageFromGallery, "Выберите изображение");
        }
        
        private void OnGetImageFromGallery(string path)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(path);
            int range = Random.Range(0, Int32.MaxValue);
            string pathTexture = UnityEngine.Application.persistentDataPath + "/" + range + ".png";
            File.Copy( path, pathTexture);
            _imagePath = pathTexture;
            _loadedImage.texture = texture;
        }
        
        private void OnSaveButtonClick()
        {
            if (!CanSave()) {
                return;
            }
            
            _saveWordsService.SaveWord(_englishWordInputField.text, _russianWordInputField.text, _imagePath);
            _textPanel.gameObject.SetActive(true);
            _textField.text = "Слово сохранено";
            HideTextPanel().Forget();
            
            _englishWordInputField.text = string.Empty;
            _russianWordInputField.text = string.Empty;
            _imagePath = string.Empty;
            _loadedImage.texture = null;
        }

        private bool CanSave()
        {
            if (string.IsNullOrEmpty(_englishWordInputField.text)) {
                _textPanel.gameObject.SetActive(true);
                _textField.text = "Вы не написали английское слово";
                HideTextPanel().Forget();
                return false;
            }
            if (string.IsNullOrEmpty(_russianWordInputField.text)) {
                _textPanel.gameObject.SetActive(true);
                _textField.text = "Вы не написали перевод слова";
                HideTextPanel().Forget();
                return false;
            }
            if (string.IsNullOrEmpty(_imagePath)) {
                _textPanel.gameObject.SetActive(true);
                _textField.text = "Вы не выбрали картинку";
                HideTextPanel().Forget();
                return false;
            }
            List<LanguageDescriptor> languageDescriptors = _descriptorService.GetAllDescriptors<LanguageDescriptor>();
            foreach (LanguageDescriptor languageDescriptor in languageDescriptors)
            {
                if (languageDescriptor.EnglishWord == _englishWordInputField.text) {
                    _textPanel.gameObject.SetActive(true);
                    _textField.text = "Данное слово уже существует";
                    HideTextPanel().Forget();
                    return false;
                }
            }

            return true;
        }

        private async UniTaskVoid HideTextPanel()
        {
            await UniTask.Delay(700);
            _textPanel.gameObject.SetActive(false);
        }
    }
}