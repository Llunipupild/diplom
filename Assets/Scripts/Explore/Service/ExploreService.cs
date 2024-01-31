using System.Collections.Generic;
using System.Linq;
using Core.Events.System;
using Core.IoC.AttributeInject;
using Cysharp.Threading.Tasks;
using Decks.Model;
using Decks.Repository;
using Dependency.Service;
using Descriptors.Enumer;
using Descriptors.Model;
using Descriptors.Service;
using UI.Cards.Controller;
using UI.MainMenu.Events;
using UnityEngine;
using Utils.Constants;
using Random = System.Random;

namespace Explore.Service
{
    public class ExploreService : EventsComponent
    {
        [Dependence] 
        private DescriptorService _descriptorService;
        [Dependence] 
        private DependencyService _dependencyService;
        [Dependence] 
        private DecksRepository _decksRepository;

        private List<CardController> _cardControllers = new();
        private Transform _cardContainer;
        private float _scaleFactor;
        private Random _random = new();
        
        public bool Canceled { get; set; }
        public bool ChoiceIsMade { get; set; }

        private void Start()
        {
            _cardContainer = GameObject.Find(GameConstants.CARD_CONTAINER).transform;
            _scaleFactor = gameObject.GetComponentInChildren<Canvas>().scaleFactor;
            CreateCardControllers();
        }

        public async UniTask StartExploreAsync(List<string> words)
        {
            List<LanguageDescriptor> descriptors = _descriptorService.GetDescriptorByWords(words);
            if (descriptors.Count <= 0) {
                //todo мб какой-нибудь вывод
                Invoke(new StopExploreEvent());
                return;
            }

            await StartExploreAsync(descriptors);
        }

        public async UniTaskVoid StartExploreAsync(List<WordType> allowedCategories)
        {
            List<LanguageDescriptor> allDescriptors = _descriptorService.GetDescriptorsWithWordType(allowedCategories);
            List<LanguageDescriptor> descriptors = ExcludeExistCards(allDescriptors);

            if (descriptors.Count == 0) {
                //todo мб какой-нибудь вывод
                Invoke(new StopExploreEvent());
                return;
            }
            
            await StartExploreAsync(descriptors);
            DecksModel decksModel = _decksRepository.Get();
            foreach (WordType allowedCategory in allowedCategories)
            {
                List<LanguageDescriptor> descriptorsWithWordType = _descriptorService.GetDescriptorsWithWordType(new List<WordType>() {allowedCategory});
                bool existOnDecks = true;
                foreach (LanguageDescriptor languageDescriptor in descriptorsWithWordType)
                {
                    if (decksModel.ExistOnDecks(languageDescriptor.EnglishWord)) {
                        continue;
                    }
                    
                    existOnDecks = false;
                    break;
                }
                
                if (!existOnDecks) {
                    continue;
                }
                        
                Invoke(new UnlockDecksAfterExplore());
                return;
            }
        }
        
        public async UniTask StartExploreAsync(List<LanguageDescriptor> descriptors)
        {
            Canceled = false;
            descriptors = descriptors.OrderBy(v => _random.Next()).ToList();
            int descriptorIndex = 0;
            
            while (!Canceled)
            {
                if (descriptorIndex >= descriptors.Count) {
                    Canceled = true;
                    break;
                }
                
                LanguageDescriptor descriptor = descriptors[descriptorIndex];
                CardController freeCardController = GetFreeCardController();
                PrepareCardController(freeCardController, descriptor);
                ShowChoiceButtons(descriptor, descriptors);
                Invoke(new OnNewCardEvent(descriptorIndex+1, descriptors.Count));
                await UniTask.WaitWhile(() => !ChoiceIsMade);
                await UniTask.Delay(500);
                ChoiceIsMade = false;
                HideChoiceButtons();
                if (Canceled) {
                    freeCardController.gameObject.SetActive(false);
                    break;
                }
                freeCardController.EnableDragAndDrop();
                ShowDecks();
                await UniTask.WaitWhile(() => !ChoiceIsMade);
                ChoiceIsMade = false;
                HideDecks();
                freeCardController.gameObject.SetActive(false);
                descriptorIndex++;
            }
            
            Invoke(new StopExploreEvent());
        }
        
        private void CreateCardControllers()
        {
            for (int i = 0; i < GameConstants.COUNT_STARTED_CARDS; i++)
            {
                CardController cardController = _dependencyService.CreateObjectWithController<CardController>(GameConstants.CARD, _cardContainer);
                _cardControllers.Add(cardController);
            }
        }

        private void PrepareCardController(CardController cardController, LanguageDescriptor languageDescriptor)
        {
            cardController.gameObject.SetActive(true);
            cardController.Refresh(languageDescriptor.EnglishWord, languageDescriptor.Image, _scaleFactor, languageDescriptor.NeedShowText);
            cardController.DisableDragAndDrop();
        }

        private void ShowChoiceButtons(LanguageDescriptor currentCardDescriptor, List<LanguageDescriptor> otherDescriptors)
        {
            if (otherDescriptors.Count <= 3) {
                otherDescriptors = _descriptorService.GetAllDescriptors<LanguageDescriptor>();
            }
            List<string> otherWords = new List<string>();
            Random random = new Random();
            bool ready = false;
            while (!ready)
            {
                int randomIndex = random.Next(1, otherDescriptors.Count);
                LanguageDescriptor randomDescriptor = otherDescriptors[randomIndex];
                if (otherWords.Contains(randomDescriptor.RussianWord) || randomDescriptor.RussianWord == currentCardDescriptor.RussianWord) {
                    continue;
                }
                
                otherWords.Add(randomDescriptor.RussianWord);
                if (otherWords.Count < 3) {
                    continue;
                }
                
                ready = true;
            }

            Invoke(new ShowChoiceButtonsEvent(otherWords,currentCardDescriptor.RussianWord, currentCardDescriptor.EnglishWord));
        }

        private void HideChoiceButtons()
        {
            Invoke(new HideChoiceButtonsEvent());
        }
        
        private void ShowDecks()
        {
            Invoke(new ShowDecksEvent());
        }

        private void HideDecks()
        {
            Invoke(new HideDecksEvent());
        }
        
        private List<LanguageDescriptor> ExcludeExistCards(List<LanguageDescriptor> descriptors)
        {
            List<LanguageDescriptor> result = new List<LanguageDescriptor>();
            DecksModel decksModel = _decksRepository.Require();
            foreach (LanguageDescriptor languageDescriptor in descriptors)
            {
                if (decksModel.ExistOnDecks(languageDescriptor.EnglishWord)) {
                    continue;
                }
                
                result.Add(languageDescriptor);
            }
            
            return result;
        }
        
        private CardController GetFreeCardController()
        {
            return _cardControllers.First(cc => cc.Free);
        }
    }
}