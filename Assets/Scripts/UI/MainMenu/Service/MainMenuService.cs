using Core.IoC.AttributeInject;
using Dependency.Service;
using UI.MainMenu.Controller;
using UnityEngine;
using Utils.Constants;

namespace UI.MainMenu
{
    public class MainMenuService : MonoBehaviour
    {
        [Dependence] 
        private DependencyService _dependencyService;

        private Transform _canvas;

        private void Start()
        {
            Debug.Log("UIService start");
            _canvas = gameObject.GetComponentInChildren<Canvas>().transform;
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            MainMenuController mainMenuController = _dependencyService.CreateObjectWithController<MainMenuController>(GameConstants.MAIN_MENU, _canvas);
            mainMenuController.transform.SetAsFirstSibling();
        }

        public Transform Canvas => _canvas;
        public float ScaleFactor => _canvas.GetComponent<Canvas>().scaleFactor;
    }
}