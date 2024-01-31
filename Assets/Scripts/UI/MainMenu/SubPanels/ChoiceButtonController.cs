using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.SubPanels
{
    public class ChoiceButtonController : MonoBehaviour
    {
        private Button _choiceButton;
        private Image _image;
        private TextMeshProUGUI _buttonText;
        private bool _inited;
        
        public Action<string> onButtonClickEvent;

        private void Start()
        {
            _choiceButton = GetComponent<Button>();
            _image = GetComponent<Image>();
            _choiceButton.onClick.AddListener(OnChoiceButtonClick);
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            _inited = true;
        }

        private void OnEnable()
        {
            if (!_inited) {
                return;
            }
            
            _image.color = Color.white;
        }

        private void OnDestroy()
        {
            _choiceButton.onClick.RemoveListener(OnChoiceButtonClick);
        }

        public void LockButton()
        {
            _choiceButton.interactable = false;
        }
        
        public void UnLockButton()
        {
            _choiceButton.interactable = true;
        }

        public void SetTrueColor()
        {
            _image.color = Color.green;
        }

        public void SetWrongAnswer()
        {
            _image.color = Color.red;
        }

        private void OnChoiceButtonClick()
        {
            onButtonClickEvent?.Invoke(ButtonText);
        }

        public string ButtonText
        {
            get => _buttonText.text;
            set => _buttonText.text = value;
        }
    }
}