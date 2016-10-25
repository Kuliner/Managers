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
        private ContentControl _content;
        private ContentControl _popUpControl;
        private List<ViewModelConnection> _views = new List<ViewModelConnection>();
        private ContentControl _topBarControl;

        #endregion Fields

        #region Constructors

        public ViewManager(IoCManager iocManager)
        {
            _iocManager = iocManager;
        }

        #endregion Constructors

        #region Methods

        public void Init(ContentControl content, ContentControl popUpControl = null, ContentControl topBarControl = null)
        {
            _content = content;
            _popUpControl = popUpControl;
            _topBarControl = topBarControl;
        }

        public void OpenView<VM>(ContentControl control = null, Parameter parameter = null) where VM : ViewModelBaseWrapper
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (control != null)
                    OpenViewBlocking(typeof(VM), control, parameter);
                else
                    OpenViewBlocking(typeof(VM), null, parameter);

            }), (System.Windows.Threading.DispatcherPriority)10, null);
        }

        public void OpenViewBlocking(Type VM, ContentControl control = null, Parameter parameter = null)
        {
            if (control != null)
            {
                if (control.DataContext as ViewModelBaseWrapper != null)
                {
                    (control.DataContext as ViewModelBaseWrapper).NavigatedFrom();
                }
            }

            // Get item for passed viewModel type
            ViewModelConnection item = _views.FirstOrDefault(x => x.ViewModelType == VM);
            if (item == null)
                throw new Exception("View not registered.");

            ContentControl view = GetView(item);

            ViewModelBaseWrapper viewModel = view.DataContext as ViewModelBaseWrapper;
            if (viewModel == null)
                viewModel = GetViewModel(item);

            if (control == null)
            {
                view.DataContext = viewModel;
                _content.Content = view;
            }
            else
            {
                view.DataContext = viewModel;
                control.Content = view;
            }

            viewModel.Parameters = parameter;
            viewModel.NavigatedTo();

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

        protected ViewModelBaseWrapper GetViewModel(ViewModelConnection item)
        {
            Type type = item.ViewModelType;
            ViewModelBaseWrapper viewModel = (ViewModelBaseWrapper)_iocManager.Resolve(item.ViewModelType);

            return viewModel;
        }
        #endregion Methods
    }
}