using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace ViewManagement
{
    public class ViewManager
    {
        #region Fields

        protected IoCManager _iocManager;
        private static ViewManager _instance;
        private ContentControl _content;
        private ContentControl _popUpControl;
        private List<ViewModelConnection> _views = new List<ViewModelConnection>();

        #endregion Fields

        #region Constructors

        public ViewManager(IoCManager iocManager)
        {
            _iocManager = iocManager;
        }

        #endregion Constructors

        #region Methods

        public void Init(ContentControl content, ContentControl popUpControl = null)
        {
            _content = content;
            _popUpControl = popUpControl;
        }

        public void OpenView<VM>() where VM : ViewModelBase
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                OpenViewBlocking(typeof(VM));
            }), (System.Windows.Threading.DispatcherPriority)10, null);
        }

        public void OpenViewBlocking(Type VM)
        {
            // Get item for passed viewModel type
            ViewModelConnection item = _views.FirstOrDefault(x => x.ViewModelType == VM);
            if (item == null)
                throw new Exception("View not registered.");

            ContentControl view = GetView(item);

            ViewModelBase viewModel = view.DataContext as ViewModelBase;
            if (viewModel == null)
                viewModel = GetViewModel(item);

            view.DataContext = viewModel;
            _content.Content = view;
        }

        public void RegisterViewModel<VM, V>()
            where VM : ViewModelBase
            where V : ContentControl
        {
            var item = new ViewModelConnection();
            item.Set<VM, V>();
            _views.Add(item);

            _iocManager.RegisterType<VM>();
        }

        protected ContentControl GetView(ViewModelConnection item) => Activator.CreateInstance(item.ViewType) as ContentControl;

        protected ViewModelBase GetViewModel(ViewModelConnection item)
        {
            Type type = item.ViewModelType;
            ViewModelBase viewModel = (ViewModelBase)_iocManager.Resolve(item.ViewModelType);

            return viewModel;
        }
        #endregion Methods
    }
}