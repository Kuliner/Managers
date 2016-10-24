using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace ViewManagement
{
    public class IoCManager
    {
        #region Members

        private UnityContainer _container;

        #endregion

        #region Ctors

        public IoCManager(UnityContainer unityContainer)
        {
            _container = unityContainer;
        }

        #endregion

        #region Public Methods

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public void RegisterType<T>(bool singleton = false)
        {
            if (singleton)
                _container.RegisterType<T>(new ContainerControlledLifetimeManager());
            else
                _container.RegisterType<T>();
        }

        public void RegisterInstance<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        #endregion
    }
}
