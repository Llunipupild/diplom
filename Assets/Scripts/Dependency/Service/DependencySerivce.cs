using Application;
using UnityEngine;

namespace Dependency.Service
{
    public class DependencyService : MonoBehaviour
    {
        public T AddControllerToObject<T>(GameObject obj) where T : MonoBehaviour
        {
            return BuildObject<T>(obj);
        }

        public T CreateObjectWithController<T>(string pathToObject, Transform parent) where T : MonoBehaviour
        {
            GameObject result = Instantiate(Resources.Load<GameObject>(pathToObject), parent);
            return BuildObject<T>(result);
        }

        private T BuildObject<T>(GameObject obj) where T : MonoBehaviour
        {
            return App.IoCContainer.AttachControllerToGameObject<T>(obj);
        }
    }
}