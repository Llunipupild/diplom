using System;
using System.Collections.Generic;
using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using Decks.Controller;
using Decks.Enum;
using Decks.Model;
using Decks.Repository;
using Dependency.Service;
using Explore.Service;
using Time;
using TMPro;
using UI.MainMenu.Events;
using UI.MainMenu.SubPanels;
using UnityEngine;
using UnityEngine.UI;
using Utils.Timer;
using Random = System.Random;

namespace UI.MainMenu.Panels
{
    public class RepeatPanelController : MonoBehaviour
    {
        [ObjectBinding("ChoiceButtonsContainer")]
        private GameObject _choiceButtonsContainer;
        [ObjectBinding("Deck1ImageContainer")]
        private GameObject _deck1ImageContainer;
        [ObjectBinding("Deck2ImageContainer")]
        private GameObject _deck2ImageContainer;
        [ObjectBinding("Deck3ImageContainer")]
        private GameObject _deck3ImageContainer;
        
        [ObjectBinding("Deck1ButtonContainer")]
        private GameObject _deck1ButtonContainer;
        [ObjectBinding("Deck2ButtonContainer")]
        private GameObject _deck2ButtonContainer;
        [ObjectBinding("Deck3ButtonContainer")]
        private GameObject _deck3ButtonContainer;

        [ComponentBinding("CountCardsText")] 
        private TextMeshProUGUI _countCardsText;
        [ComponentBinding("CompleteButton")] 
        private Button _completeButton;
        [ComponentBinding("Deck1Button")]
        private Button _deck1Button;
        [ComponentBinding("Deck2Button")]
        private Button _deck2Button;
        [ComponentBinding("Deck3Button")]
        private Button _deck3Button;

        private DeckController _deck1Controller;
        private DeckController _deck2Controller;
        private DeckController _deck3Controller;

        [Dependence] 
        private DependencyService _dependencyService;
        [Dependence] 
        private ExploreService _exploreService;
        [Dependence] 
        private DecksRepository _decksRepository;
        [Dependence]
        private TimeDeckRepository _timeDeckRepository;

        private string _trueAnswer;
        private string _trueAnswerEnglish;
        private bool _inited;

        private void Start()
        {
            _exploreService.AddListener<ShowChoiceButtonsEvent>(ShowChoiceButtonsEvent.SHOW_CHOICE_BUTTONS, OnShowChoiceButtonsEvent);
            _exploreService.AddListener<HideChoiceButtonsEvent>(HideChoiceButtonsEvent.HIDE_CHOICE_BUTTONS, OnHideShowChoiceButtonsEvent);
            _exploreService.AddListener<ShowDecksEvent>(ShowDecksEvent.SHOW_DECKS_EVENT, OnShowDecksEvent);
            _exploreService.AddListener<HideDecksEvent>(HideDecksEvent.HIDE_DECKS_EVENT, OnHideDecksEvent);
            _exploreService.AddListener<OnNewCardEvent>(Events.OnNewCardEvent.ON_NEW_CARD, OnNewCardEvent);
            _exploreService.AddListener<UnlockDecksAfterExplore>(UnlockDecksAfterExplore.UNLOCK_DECKS, OnUnlockDecks);
            
            _deck1Button.onClick.AddListener(OnDeck1ButtonCLick);
            _deck2Button.onClick.AddListener(OnDeck2ButtonCLick);
            _deck3Button.onClick.AddListener(OnDeck3ButtonCLick);
            _completeButton.onClick.AddListener(OnCompleteButtonClick);

            _deck1ImageContainer.GetComponent<Deck>().onSelectedDeck += OnSelectDeck;
            _deck2ImageContainer.GetComponent<Deck>().onSelectedDeck += OnSelectDeck;
            _deck3ImageContainer.GetComponent<Deck>().onSelectedDeck += OnSelectDeck;

            _deck1Controller = _dependencyService.AddControllerToObject<DeckController>(_deck1ButtonContainer);
            _deck1Controller.Init(DeckNumber.DECK1);
            _deck2Controller = _dependencyService.AddControllerToObject<DeckController>(_deck2ButtonContainer);
            _deck2Controller.Init(DeckNumber.DECK2);
            _deck3Controller = _dependencyService.AddControllerToObject<DeckController>(_deck3ButtonContainer);
            _deck3Controller.Init(DeckNumber.DECK3);
            
            _inited = true;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!_inited) {
                return;
            }
            EnableDeckButtonContainers();
        }

        private void OnDisable()
        {
            DisableDeckButtonContainers();
        }

        private void OnDestroy()
        {
            _exploreService.RemoveListener<ShowChoiceButtonsEvent>(ShowChoiceButtonsEvent.SHOW_CHOICE_BUTTONS, OnShowChoiceButtonsEvent);
            _exploreService.RemoveListener<HideChoiceButtonsEvent>(HideChoiceButtonsEvent.HIDE_CHOICE_BUTTONS, OnHideShowChoiceButtonsEvent);
            _exploreService.RemoveListener<ShowDecksEvent>(ShowDecksEvent.SHOW_DECKS_EVENT, OnShowDecksEvent);
            _exploreService.RemoveListener<HideDecksEvent>(HideDecksEvent.HIDE_DECKS_EVENT, OnHideDecksEvent);
            _exploreService.RemoveListener<OnNewCardEvent>(Events.OnNewCardEvent.ON_NEW_CARD, OnNewCardEvent);
            _exploreService.RemoveListener<UnlockDecksAfterExplore>(UnlockDecksAfterExplore.UNLOCK_DECKS, OnUnlockDecks);
            _deck1ImageContainer.GetComponent<Deck>().onSelectedDeck -= OnSelectDeck;
            _deck2ImageContainer.GetComponent<Deck>().onSelectedDeck -= OnSelectDeck;
            _deck3ImageContainer.GetComponent<Deck>().onSelectedDeck -= OnSelectDeck;
            _completeButton.onClick.RemoveListener(OnCompleteButtonClick);
            _deck1Button.onClick.RemoveListener(OnDeck1ButtonCLick);
            _deck2Button.onClick.RemoveListener(OnDeck2ButtonCLick);
            _deck3Button.onClick.RemoveListener(OnDeck3ButtonCLick);
        }

        public void DisableChoiceButtons()
        {
            _choiceButtonsContainer.SetActive(false);
        }

        public void HideCountCardText()
        {
            _countCardsText.gameObject.SetActive(false);
        }

        private async void OnDeck1ButtonCLick()
        {
            DecksModel decksModel = _decksRepository.Require();
            if (decksModel.Decks1.Count <= 0) {
                return;
            }
            DisableDeckButtonContainers();
            await _exploreService.StartExploreAsync(decksModel.Decks1);
            
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            timeDeckModel.DateTimeDeck1 = DateTime.Now;
            _timeDeckRepository.Set(timeDeckModel);
            
            _deck1Controller.EnableLockImage();
        }

        private void OnUnlockDecks(UnlockDecksAfterExplore unlockDecksAfterExplore)
        {
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            timeDeckModel.DateTimeDeck1 = new DateTime();
            timeDeckModel.DateTimeDeck2 = new DateTime();
            timeDeckModel.DateTimeDeck3 = new DateTime();
            
            _deck1Controller.DisableLockImage();
            _deck2Controller.DisableLockImage();
            _deck3Controller.DisableLockImage();
        }
        
        private async void OnDeck2ButtonCLick()
        {
            DecksModel decksModel = _decksRepository.Require();
            if (decksModel.Decks2.Count <= 0) {
                return;
            }
            DisableDeckButtonContainers();
            await _exploreService.StartExploreAsync(decksModel.Decks2);
            
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            timeDeckModel.DateTimeDeck2 = DateTime.Now;
            _timeDeckRepository.Set(timeDeckModel);
            
            _deck2Controller.EnableLockImage();
        }
        
        private async void OnDeck3ButtonCLick()
        {
            DecksModel decksModel = _decksRepository.Require();
            if (decksModel.Decks3.Count <= 0) {
                return;
            }
            DisableDeckButtonContainers();
            await _exploreService.StartExploreAsync(decksModel.Decks3);
            
            TimeDeckModel timeDeckModel = _timeDeckRepository.Get();
            timeDeckModel.DateTimeDeck3 = DateTime.Now;
            _timeDeckRepository.Set(timeDeckModel);
            
            _deck3Controller.EnableLockImage();
        }
        
        private void OnNewCardEvent(OnNewCardEvent onNewCardEvent)
        {
            _countCardsText.gameObject.SetActive(true);
            _countCardsText.text = $"{onNewCardEvent.CurrentCardIndex} из {onNewCardEvent.AllCardsCount}";
        }

        private void OnCompleteButtonClick()
        {
            _exploreService.Canceled = true;
            _exploreService.ChoiceIsMade = true;
            _completeButton.interactable = false;
        }
        
        private void OnSelectDeck(DeckNumber deckNumber)
        {
            _exploreService.ChoiceIsMade = true;
            DecksModel decksModel = _decksRepository.Require();
            switch (deckNumber)
            {
                case DeckNumber.DECK1:
                    decksModel.AddWordInDecks1(_trueAnswerEnglish);
                    break;
                case DeckNumber.DECK2:
                    decksModel.AddWordInDecks2(_trueAnswerEnglish);
                    break;
                case DeckNumber.DECK3:
                    decksModel.AddWordInDecks3(_trueAnswerEnglish);
                    break;
            }
            
            _decksRepository.Set(decksModel);
        }

        private void OnShowChoiceButtonsEvent(ShowChoiceButtonsEvent showChoiceButtonsEvent)
        {
            _trueAnswer = string.Empty;
            _trueAnswerEnglish = string.Empty;
            _choiceButtonsContainer.SetActive(true);
            Random random = new Random();
            int childCount = _choiceButtonsContainer.transform.childCount;
            int randomIndex = random.Next(0, childCount-1);
            List<string> usingWords = new List<string>();
            for (int i = 0; i < childCount; i++)
            {
                Transform child = _choiceButtonsContainer.transform.GetChild(i);
                ChoiceButtonController choiceButtonController = child.GetComponent<ChoiceButtonController>();
                choiceButtonController.onButtonClickEvent += OnChoiceButtonClick;
                choiceButtonController.UnLockButton();
                if (randomIndex == i) {
                    _trueAnswer = showChoiceButtonsEvent.TrueAnswer;
                    _trueAnswerEnglish = showChoiceButtonsEvent.TrueAnswerEnglish;
                    choiceButtonController.ButtonText = showChoiceButtonsEvent.TrueAnswer;
                    continue;
                }

                string findWord = showChoiceButtonsEvent.Words.Find(w => !usingWords.Contains(w));
                choiceButtonController.ButtonText = findWord;
                usingWords.Add(findWord);
            }
            
            _completeButton.gameObject.SetActive(true);
            _completeButton.interactable = true;
        }

        private void OnHideShowChoiceButtonsEvent(HideChoiceButtonsEvent hideChoiceButtonsEvent)
        {
            _choiceButtonsContainer.SetActive(false);
            _completeButton.gameObject.SetActive(false);
        }

        private void OnShowDecksEvent(ShowDecksEvent showDecksEvent)
        {
            _countCardsText.gameObject.SetActive(false);
            EnableDeckImageContainers();
        }

        private void OnHideDecksEvent(HideDecksEvent hideDecksEvent)
        {
            DisableDeckImageContainers();   
        }

        private void OnChoiceButtonClick(string answer)
        {
            for (int i = 0; i < _choiceButtonsContainer.transform.childCount; i++)
            {
                Transform child = _choiceButtonsContainer.transform.GetChild(i);
                ChoiceButtonController choiceButtonController = child.GetComponent<ChoiceButtonController>();
                choiceButtonController.LockButton();
                if (choiceButtonController.ButtonText == _trueAnswer) {
                    choiceButtonController.SetTrueColor();
                    continue;
                }
                if (choiceButtonController.ButtonText == answer) {
                    choiceButtonController.SetWrongAnswer();
                }
            }
            _exploreService.ChoiceIsMade = true;
        }

        public void DisableDeckButtonContainers()
        {
            _deck1ButtonContainer.SetActive(false);
            _deck2ButtonContainer.SetActive(false);
            _deck3ButtonContainer.SetActive(false);
        }
        
        public void EnableDeckButtonContainers()
        {
            _deck1ButtonContainer.SetActive(true);
            _deck2ButtonContainer.SetActive(true);
            _deck3ButtonContainer.SetActive(true);
        }
        
        private void DisableDeckImageContainers()
        {
            _deck1ImageContainer.SetActive(false);
            _deck2ImageContainer.SetActive(false);
            _deck3ImageContainer.SetActive(false);
        }
        
        private void EnableDeckImageContainers()
        {
            _deck1ImageContainer.SetActive(true);
            _deck2ImageContainer.SetActive(true);
            _deck3ImageContainer.SetActive(true);
        }
    }
}