using Audio.Service;
using Core.IoC.Container;
using Decks.Model;
using Decks.Repository;
using Dependency.Service;
using Descriptors.Service;
using Explore.Service;
using SaveWords.Model;
using SaveWords.Repositroy;
using SaveWords.Service;
using Time;
using UI.MainMenu;
using UnityEngine;

namespace Application
{
    public class App : MonoBehaviour
    {
        public static IocContainer IoCContainer { get; private set; } = null!;
        private void Awake()
        {
            Debug.Log("App awake");
            IoCContainer = gameObject.AddComponent<IocContainer>();
            
            IoCContainer.RegisterSingletonClass<DecksRepository>();
            IoCContainer.RegisterSingletonClass<TimeDeckRepository>();
            IoCContainer.RegisterSingletonClass<SaveNewWordRepository>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<DescriptorService>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<DependencyService>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<MainMenuService>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<AudioService>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<ExploreService>();
            IoCContainer.RegisterSingletonMonoBehaviourClass<SaveWordsService>();

            DecksRepository decksRepository = IoCContainer.RequireInstance<DecksRepository>();
            TimeDeckRepository timeDeckRepository = IoCContainer.RequireInstance<TimeDeckRepository>();
            SaveNewWordRepository saveNewWordRepository = IoCContainer.RequireInstance<SaveNewWordRepository>();
            
            if (decksRepository.Get() == null) {
                decksRepository.Set(new DecksModel());
            }

            if (timeDeckRepository.Get() == null) {
                timeDeckRepository.Set(new TimeDeckModel());
            }
            
            if (saveNewWordRepository.Get() == null) {
                saveNewWordRepository.Set(new NewWordsModel());
            }
        }
    }
}