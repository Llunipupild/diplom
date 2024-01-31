using System;
using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using Decks.Enum;
using Time;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Timer
{
    public class DeckController : MonoBehaviour
    {
        [ObjectBinding("LockImage")]
        private GameObject _lockImage;
        [ComponentBinding("TimeToUnlockText")] 
        private TextMeshProUGUI _timeToUnlockText;

        
        private Button _deckButton;
        
        [Dependence] 
        private TimeDeckRepository _timeDeckRepository;

        private DeckNumber _deck;

        public void Init(DeckNumber deck)
        {
            _deckButton = gameObject.GetComponentInChildren<Button>();
            _deck = deck;
            CheckEnableImageStatus();
        }
        
        private void Update()
        {
            if (!_lockImage.activeInHierarchy) {
                return;
            }
            
            UpdateTime();
        }
        
        public void EnableLockImage()
        {
            _lockImage.SetActive(true);
            _timeToUnlockText.gameObject.SetActive(true);
            _deckButton.interactable = false;
        }
        
        public void DisableLockImage()
        {
            _lockImage.SetActive(false);
            _timeToUnlockText.gameObject.SetActive(false);
            _deckButton.interactable = true;
        }

        private void UpdateTime()
        {
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            DateTime dateTime = new DateTime();

            switch (_deck)
            {
                case DeckNumber.DECK1:
                    dateTime = timeDeckModel.DateTimeDeck1;
                    dateTime = dateTime.AddDays(1);
                    break;
                case DeckNumber.DECK2:
                    dateTime = timeDeckModel.DateTimeDeck2;
                    dateTime = dateTime.AddDays(2);
                    break;
                case DeckNumber.DECK3:
                    dateTime = timeDeckModel.DateTimeDeck3;
                    dateTime = dateTime.AddDays(3);
                    break;
            }
            
            //timeSpan.TotalDays * 24

            TimeSpan timeSpan = dateTime.Subtract(DateTime.Now);
            string timeHours = (int)timeSpan.TotalHours <= 9 ? $"0{(int)timeSpan.TotalHours}" : ((int)timeSpan.TotalHours).ToString();
            string timeMinutes = timeSpan.Minutes <= 9 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes.ToString();
            string timeSeconds = timeSpan.Seconds <= 9 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds.ToString();
            _timeToUnlockText.text = timeHours + "." + timeMinutes + "." + timeSeconds;
            
            if (timeSpan.Ticks > 0) {
                return;
            }
            
            DisableLockImage();
        }
        
        private void CheckEnableImageStatus()
        {
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            DateTime dateTime = new DateTime();

            switch (_deck)
            {
                case DeckNumber.DECK1:
                    dateTime = timeDeckModel.DateTimeDeck1;
                    dateTime = dateTime.AddDays(1);
                    break;
                case DeckNumber.DECK2:
                    dateTime = timeDeckModel.DateTimeDeck2;
                    dateTime = dateTime.AddDays(2);
                    break;
                case DeckNumber.DECK3:
                    dateTime = timeDeckModel.DateTimeDeck3;
                    dateTime = dateTime.AddDays(3);
                    break;
            }

            TimeSpan timeSpan = dateTime.Subtract(DateTime.Now);
            
            if (timeSpan.Ticks > 0) {
                EnableLockImage();
            }
        }
    }
}