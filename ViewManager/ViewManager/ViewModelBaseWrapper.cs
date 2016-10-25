using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace ViewManagement
{
    public class Parameter : Dictionary<string, object>
    {
        #region Constructors

        public Parameter()
        {
        }

        public Parameter(string name, object value)
        {
            this[name] = value;
        }

        public Parameter(Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                this[parameter.Key] = parameter.Value;
            }
        }

        #endregion Constructors

        #region Methods

        public object GetParameter(string parameterName)
        {
            return this[parameterName];
        }

        public T GetParameter<T>(string parameterName)
        {
            var parameter = this[parameterName];
            if (parameter == null)
                return default(T);
            else
                return (T)parameter;
        }

        public void SetParameter(string parameterName, object parameterValue)
        {
            this[parameterName] = parameterValue;
        }

        #endregion Methods
    }

    public class ViewModelBaseWrapper : ViewModelBase
    {
        public Parameter Parameters;

        #region Methods

        public virtual void NavigatedFrom()
        {
        }

        public virtual void NavigatedTo()
        {
        }

        #endregion Methods
    }
}