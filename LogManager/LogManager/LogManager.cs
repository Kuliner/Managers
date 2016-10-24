using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

public class LogManager
{
    #region Fields

    private ILog _log = null;
    private static LogManager _instance;

    #endregion Fields

    #region Properties

    public static LogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LogManager();
            }
            return _instance;
        }
    }

    public void Init(ILog log)
    {
        _log = log;
    }

    #endregion Properties

    #region Methods

    public void LogError(string message)
    {
        if (_log == null)
            throw new Exception("Logger not initialized!");

        _log.Error(message);
    }

    public void LogInfo(string message)
    {
        if (_log == null)
            throw new Exception("Logger not initialized!");

        _log.Info(message);
    }

    public void LogWarning(string message)
    {
        if (_log == null)
            throw new Exception("Logger not initialized!");

        _log.Warn(message);
    }

    #endregion Methods
}
