using System;
using System.Collections.Generic;
using Core.Events.System;
using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using Decks.Model;
using Decks.Repository;
using Descriptors.Enumer;
using Descriptors.Model;
using Descriptors.Service;
using Explore.Service;
using UI.MainMenu.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Panels
{
    public class ExplorePanelController : EventsComponent
    {
        [ObjectBinding("CategoriesContainer")]
        private GameObject _categoriesContainer;

        [ComponentBinding("StartExploreButton")] 
        private Button _startExploreButton;
        [ComponentBinding("SelectAllButton")] 
        private Button _selectAllButton;

        [Dependence] 
        private ExploreService _exploreService = null!;
        [Dependence] 
        private DecksRepository _decksRepository = null!;
        [Dependence]
        private DescriptorService _descriptorService = null!;

        private List<Toggle> _categories = new();
        private bool _inited;

        private void Start()
        {
            _startExploreButton.onClick.AddListener(OnStartExploreButtonCLick);
            _selectAllButton.onClick.AddListener(OnSelectAllClick);
            _exploreService.AddListener<StopExploreEvent>(StopExploreEvent.STOP_EXPLORE_EVENT, OnStopExploreEvent);
            SetToggles();
            LockEmptyToggles();
            _inited = true;
        }

        private void OnDestroy()
        {
            _startExploreButton.onClick.RemoveListener(OnStartExploreButtonCLick);
            _selectAllButton.onClick.RemoveListener(OnSelectAllClick);
            _exploreService.RemoveListener<StopExploreEvent>(StopExploreEvent.STOP_EXPLORE_EVENT, OnStopExploreEvent);
        }

        private void OnEnable()
        {
            if (!_inited) {
                return;
            }
            
            LockEmptyToggles();
        }

        private void OnStopExploreEvent(StopExploreEvent stopExploreEvent)
        {
            LockEmptyToggles();
        }

        public void LockButtons()
        {
            _startExploreButton.interactable = false;
            _selectAllButton.interactable = false;
            _categories.ForEach(t => t.interactable = false);
        }

        public void UnlockButtons()
        {
            _startExploreButton.interactable = true;
            _selectAllButton.interactable = true;
            _categories.ForEach(t => t.interactable = true);
        }
        
        private void OnSelectAllClick()
        {
            foreach (Toggle category in _categories)
            {
                if (!category.interactable) {
                    continue;
                }
                
                category.isOn = !category.isOn;
            }
        }

        private void OnStartExploreButtonCLick()
        {
            List<WordType> allowedCategories = new List<WordType>();
            foreach (Toggle category in _categories)
            {
                if (!category.isOn) {
                    continue;
                }

                WordTypeBehaviour wordTypeBehaviour = category.GetComponent<WordTypeBehaviour>();
                allowedCategories.Add(wordTypeBehaviour.WordType);
            }

            if (allowedCategories.Count <= 0) {
                return;
            }
            
            _exploreService.StartExploreAsync(allowedCategories).Forget();
            ReturnSelection();
            Invoke(new StartExploreEvent());
        }

        private void LockEmptyToggles()
        {
            DecksModel decksModel = _decksRepository.Require();
            foreach (Toggle category in _categories)
            {
                WordTypeBehaviour wordTypeBehaviour = category.GetComponent<WordTypeBehaviour>();
                List<LanguageDescriptor> descriptors = _descriptorService.GetDescriptorsWithWordType(new List<WordType>(){wordTypeBehaviour.WordType});
                bool hasNotExploreWords = false;
                foreach (LanguageDescriptor languageDescriptor in descriptors) {
                    if (decksModel.ExistOnDecks(languageDescriptor.EnglishWord)) {
                        continue;
                    }

                    hasNotExploreWords = true;
                    break;
                }

                if (hasNotExploreWords) {
                    category.interactable = true;
                    continue;
                }

                
                category.interactable = false;
            }
        }
        
        private void ReturnSelection()
        {
            foreach (Toggle category in _categories)
            {
                if (!category.isOn) {
                    continue;
                }

                category.isOn = !category.isOn;
            }
        }
        
        private void SetToggles()
        {
            for (int i = 0; i < _categoriesContainer.transform.childCount; i++)
            {
                Transform child = _categoriesContainer.transform.GetChild(i);
                _categories.Add(child.gameObject.GetComponent<Toggle>());
            }
        }
    }
}