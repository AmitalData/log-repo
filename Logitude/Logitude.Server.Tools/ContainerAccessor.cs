using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public class ContainerAccessor:IContainerAccessor
    {

        #region Members

        private static IUnityContainer _container;

        #endregion

        #region Properties

        /// <summary>
        /// The Unity container for the current application
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();
                }
                return _container;
            }
        }

        #endregion

        #region IContainerAccessor Members

        /// <summary>
        /// Returns the Unity container of the application 
        /// </summary>
        IUnityContainer IContainerAccessor.Container
        {
            get { return Container; }
        }

        #endregion

        
           
        public static void InitContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
            }

            // Register the relevant types for the 
            // container here through classes or configuration

            switch (LogitudeSettings.StorageServiceMode)
            {
                case "azure":
                    _container.RegisterType<IBlobService, AzureBlobService>("StorageService", new InjectionFactory(c => new AzureBlobService()));
                    break;
                case "db":
                    _container.RegisterType<IBlobService, DatabaseBlobService>("StorageService", new InjectionFactory(c => new DatabaseBlobService()));
                    break;
                case "fs":
                    _container.RegisterType<IBlobService, FileSystemBlobService>("StorageService", new InjectionFactory(c => new FileSystemBlobService()));
                    break;
                default:
                    _container.RegisterType<IBlobService, AzureBlobService>("StorageService", new InjectionFactory(c => new AzureBlobService()));
                    break;


            }

            switch (LogitudeSettings.QueueServiceMode)
            {
                case "azure":
                    _container.RegisterType<IQueueService, AzureQueueService>("QueueService", new InjectionFactory(c => new AzureQueueService()));
                    break;
                case "db":
                    _container.RegisterType<IQueueService, DbQueueService>("QueueService", new InjectionFactory(c => new DbQueueService()));
                    break;
                default:
                    _container.RegisterType<IQueueService, AzureQueueService>("QueueService", new InjectionFactory(c => new AzureQueueService()));
                    break;


            }



        }

        public static void RegisterTypeFactory<TFrom, TTo>(string key, TTo _factoryObject) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(key, new InjectionFactory(c => _factoryObject));
        }

        public static void CleanUp()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }

    }

    public static class Extension
    {
        public static string NameOf(this object o)
        {
            var o1 = o.GetType().UnderlyingSystemType.Name;
            return o.GetType().Name;
        }
        public static I ResolveSafe<I>(this IUnityContainer unityContainer)
        {
            if ((unityContainer as UnityContainer).IsRegistered<I>())
            {
                return ContainerAccessor.Container.Resolve<I>();
            }
            return default(I);
        }
    }
}
