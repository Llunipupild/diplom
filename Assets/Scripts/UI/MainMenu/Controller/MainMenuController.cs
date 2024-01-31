using Core.IoC.AttributeInject;
using Core.ObjectBindings.Attributes;
using Dependency.Service;
using Explore.Service;
using TMPro;
using UI.MainMenu.Events;
using UI.MainMenu.Panels;
using UnityEngine;
using UnityEngine.UI;
using Utils.Constants;

namespace UI.MainMenu.Controller
{
    public class MainMenuController : MonoBehaviour
    {
        [ComponentBinding("TopHeader")]
        private TextMeshProUGUI _topHeaderText = null!;
        [ComponentBinding("NoticeButton")] 
        private Button _noticeButton = null!;
        [ComponentBinding("StudingButton")] 
        private Button _studyingButton = null!;
        [ComponentBinding("RepeatButton")] 
        private Button _repeatButton = null!;
        [ComponentBinding("NewWordButton")] 
        private Button _addNewWordButton = null!;
        [ComponentBinding("SelectedStudingButton")] 
        private Image _selectedStudyingImage = null!;
        [ComponentBinding("SelectedRepeatButton")] 
        private Image _selectedRepeatImage = null!;
        [ComponentBinding("SelectedAddNewButton")] 
        private Image _selectedAddNewWordImage = null!;
        
        [ComponentBinding("NoticePanel")] 
        private Button _noticePanelButton = null!;
        [ComponentBinding("NoticePanelText")] 
        private TextMeshProUGUI _noticePanelText = null!;
        
        [ObjectBinding("ExplorePanel")] 
        private GameObject _explorePanel = null!;
        [ObjectBinding("RepeatPanel")] 
        private GameObject _repeatPanel = null!;
        [ObjectBinding("AddNewWordPanel")] 
        private GameObject _addNewWordPanel = null!;
        [ObjectBinding("NoticePanelContainer")]
        private GameObject _noticePanelContainer = null!;

        [Dependence] 
        private DependencyService _dependencyService;
        [Dependence] 
        private ExploreService _exploreService;
        
        private ExplorePanelController _explorePanelController;
        private RepeatPanelController _repeatPanelController;
        private AddNewWordPanelController _addNewWordPanelController;
        
        private void Start()
        {
            _noticeButton.onClick.AddListener(OnNoticeButtonClick);
            _studyingButton.onClick.AddListener(OnStudyingButtonCLick);
            _repeatButton.onClick.AddListener(OnRepeatButtonCLick);
            _addNewWordButton.onClick.AddListener(OnAddNewWordButtonClick);
            _noticePanelButton.onClick.AddListener(OnNoticePanelButtonClick);
            
            _explorePanelController = _dependencyService.AddControllerToObject<ExplorePanelController>(_explorePanel);
            _explorePanelController.AddListener<StartExploreEvent>(StartExploreEvent.START_EXPLORE,OnStartExploreEvent);

            _repeatPanelController = _dependencyService.AddControllerToObject<RepeatPanelController>(_repeatPanel);

            _addNewWordPanelController = _dependencyService.AddControllerToObject<AddNewWordPanelController>(_addNewWordPanel);
            
            _exploreService.AddListener<ShowChoiceButtonsEvent>(ShowChoiceButtonsEvent.SHOW_CHOICE_BUTTONS, OnShowChoiceButtonsEvent);
            _exploreService.AddListener<StopExploreEvent>(StopExploreEvent.STOP_EXPLORE_EVENT, OnStopExploreEvent);
            
            _selectedStudyingImage.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _noticeButton.onClick.RemoveListener(OnNoticeButtonClick);
            _studyingButton.onClick.RemoveListener(OnStudyingButtonCLick);
            _repeatButton.onClick.RemoveListener(OnRepeatButtonCLick);
            _addNewWordButton.onClick.RemoveListener(OnAddNewWordButtonClick);
            _noticePanelButton.onClick.RemoveListener(OnNoticePanelButtonClick);
            
            _explorePanelController.RemoveListener<StartExploreEvent>(StartExploreEvent.START_EXPLORE,OnStartExploreEvent);
            _exploreService.RemoveListener<ShowChoiceButtonsEvent>(ShowChoiceButtonsEvent.SHOW_CHOICE_BUTTONS, OnShowChoiceButtonsEvent);
            _exploreService.RemoveListener<StopExploreEvent>(StopExploreEvent.STOP_EXPLORE_EVENT, OnStopExploreEvent);
        }
        
        private void OnStartExploreEvent(StartExploreEvent startExploreEvent)
        {
            _studyingButton.interactable = false;
            _repeatButton.interactable = false;
            _addNewWordButton.interactable = false;
            _noticeButton.interactable = false;
            
            _explorePanel.SetActive(false);
            _repeatPanelController.DisableDeckButtonContainers();
        }
        
        private void OnStopExploreEvent(StopExploreEvent stopExploreEvent)
        {
            _studyingButton.interactable = true;
            _repeatButton.interactable = true;
            _addNewWordButton.interactable = true;
            _noticeButton.interactable = true;
            _repeatPanelController.EnableDeckButtonContainers();
            _repeatPanelController.HideCountCardText();
            DisableSelectedImage();
            _selectedRepeatImage.gameObject.SetActive(true);
        }

        private void OnShowChoiceButtonsEvent(ShowChoiceButtonsEvent showChoiceButtonsEvent)
        {
            _repeatPanelController.gameObject.SetActive(true);
        }

        private void OnStudyingButtonCLick()
        {
            DisablePanels();
            DisableSelectedImage();
            _topHeaderText.text = GameConstants.STUDYING_TEXT;
            _selectedStudyingImage.gameObject.SetActive(true);
            _explorePanelController.gameObject.SetActive(true);
            _noticeButton.gameObject.SetActive(true);
        }
        
        private void OnRepeatButtonCLick()
        {
            DisablePanels();
            DisableSelectedImage();
            _topHeaderText.text = GameConstants.REPEAT_TEXT;
            _selectedRepeatImage.gameObject.SetActive(true);
            _repeatPanelController.gameObject.SetActive(true);
            _noticeButton.gameObject.SetActive(true);
        }
        
        private void OnAddNewWordButtonClick()
        {
            DisablePanels();
            DisableSelectedImage();
            _topHeaderText.text = GameConstants.SETTING_TEXT;
            _selectedAddNewWordImage.gameObject.SetActive(true);
            _addNewWordPanelController.gameObject.SetActive(true);
            _noticeButton.gameObject.SetActive(false);
        }
        
        private void OnNoticeButtonClick()
        {
            _noticePanelContainer.SetActive(true);
            _noticePanelText.text = _explorePanel.activeInHierarchy
                ? GameConstants.EXPLORE_PANEL_NOTICE_TEXT
                : GameConstants.REPEAT_PANEL_NOTICE_TEXT;
        }

        private void OnNoticePanelButtonClick()
        {
            _noticePanelContainer.SetActive(false);
        }

        private void DisablePanels()
        {
            _explorePanelController.gameObject.SetActive(false);
            _repeatPanelController.DisableChoiceButtons();
            _repeatPanelController.DisableDeckButtonContainers();
            _repeatPanelController.gameObject.SetActive(false);
            _addNewWordPanelController.gameObject.SetActive(false);
        }

        private void DisableSelectedImage()
        {
            _selectedRepeatImage.gameObject.SetActive(false);
            _selectedAddNewWordImage.gameObject.SetActive(false);
            _selectedStudyingImage.gameObject.SetActive(false);
        }
    }
}