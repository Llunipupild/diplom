using Audio.Service;
using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using TMPro;
using UI.Cards.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards.Controller
{
    public class CardController : MonoBehaviour
    {
        [ComponentBinding("WordText")]
        private TextMeshProUGUI _wordTextField = null!;
        [ComponentBinding("Image")]
        private RawImage _image = null!;
        [ComponentBinding("SoundButton")] 
        private Button _soundButton = null!;

        [Dependence]
        private AudioService _audioService = null!;

        private DragAndDropComponent _dragAndDropComponent;
        private string _word;
        private bool _alreadyVoiced;
        private Vector3 _startedPosition;

        private void Start()
        {
            _soundButton.onClick.AddListener(OnSoundButtonClick);
            _dragAndDropComponent = gameObject.GetComponent<DragAndDropComponent>();
            _startedPosition = transform.position;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _soundButton.onClick.RemoveListener(OnSoundButtonClick);
        }

        public void Refresh(string word, string image, float scaleFactor, bool needShowText = true)
        {
            _word = word;
            _wordTextField.text = needShowText ? word : string.Empty;
            Texture2D texture2D = Resources.Load<Texture2D>(image);
            if (texture2D == null) {
                var rawData = System.IO.File.ReadAllBytes(image);
                Texture2D tex = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
                tex.LoadImage(rawData);
                texture2D = tex;
            }
            _image.texture = texture2D;
            gameObject.GetComponent<DragAndDropComponent>().ScaleFactor = scaleFactor;
            transform.position = _startedPosition;
        }

        public void LockSoundButton()
        {
            _soundButton.interactable = false;
        }
        
        public void UnLockSoundButton()
        {
            _soundButton.interactable = true;
        }

        public void DisableDragAndDrop()
        {
            _dragAndDropComponent.enabled = false;
        }

        public void EnableDragAndDrop()
        {
            _dragAndDropComponent.enabled = true;
        }

        private async void OnSoundButtonClick()
        {
            if (_alreadyVoiced) {
                return;
            }

            _alreadyVoiced = true;
            await _audioService.SoundTheWord(_word);
            _alreadyVoiced = false;
        }

        public bool Free => !gameObject.activeInHierarchy;
    }
}