using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;
using PIS.AutoEnt.Config;

namespace PIS.AutoEnt
{
    public enum LogLevel
    {
        OFF = 0,
        FATAL = 1,
        ERROR = 2,
        WARN = 4,
        INFO = 8,
        DEBUG = 16,
        ALL = 32
    }

    public static class LogManager
    {
        public static void Configure()
        {
            XmlNode node = ConfigManager.LogConfig.GetConfig(ConfigSections.Log4net);

            if (node != null)
            {
                log4net.Config.XmlConfigurator.Configure(node.FirstChild as XmlElement);
            }
        }

        public static ILog GetLogger(string name, string loggerName = GlobalConsts.DefaultLoggerName)
        {
            return log4net.LogManager.GetLogger(name);
        }

        public static void Log(string message, string loggerName = GlobalConsts.DefaultLoggerName)
        {
            Log(message, LogLevel.DEBUG, loggerName);
        }

        public static void Log(string message, Exception ex, string loggerName = GlobalConsts.DefaultLoggerName)
        {
            Log(message, ex, LogLevel.DEBUG, loggerName);
        }

        public static void Log(string message, LogLevel level, string loggerName = GlobalConsts.DefaultLoggerName)
        {
            ILog logger = LogManager.GetLogger(loggerName ?? GlobalConsts.DefaultLoggerName);

            switch (level)
            {
                case LogLevel.DEBUG:
                    logger.Debug(message);
                    break;
                case LogLevel.WARN:
                    logger.Info(message);
                    break;
                case LogLevel.INFO:
                    logger.Info(message);
                    break;
                case LogLevel.ERROR:
                    logger.Error(message);
                    break;
                case LogLevel.FATAL:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }

        public static void Log(string message, Exception ex, LogLevel level, string loggerName = GlobalConsts.DefaultLoggerName)
        {
            ILog logger = LogManager.GetLogger(loggerName ?? GlobalConsts.DefaultLoggerName);

            switch (level)
            {
                case LogLevel.DEBUG:
                    logger.Debug(message, ex);
                    break;
                case LogLevel.WARN:
                    logger.Info(message, ex);
                    break;
                case LogLevel.INFO:
                    logger.Info(message, ex);
                    break;
                case LogLevel.ERROR:
                    logger.Error(message, ex);
                    break;
                case LogLevel.FATAL:
                    logger.Fatal(message, ex);
                    break;
                default:
                    logger.Debug(message, ex);
                    break;
            }
        }
    }
}
