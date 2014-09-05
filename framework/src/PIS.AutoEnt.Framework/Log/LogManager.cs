using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;
using PIS.AutoEnt.Config;

namespace PIS.AutoEnt
{
    public static class LogManager
    {
        public static void Configure()
        {
            XmlNode node = ConfigManager.ThirdPartyConfig.GetConfig(SysConsts.Log4netSectionName);

            if (node != null)
            {
                log4net.Config.XmlConfigurator.Configure(node.FirstChild as XmlElement);
            }
        }

        public static ILog GetLogger(string name)
        {
            return log4net.LogManager.GetLogger(name);
        }

        public static void Log(string message)
        {
            Log(message, SysConsts.DefaultLoggerName, LogLevel.DEBUG);
        }

        public static void Log(string message, string loggerName)
        {
            Log(message, loggerName, LogLevel.DEBUG);
        }

        public static void Log(string message, Exception ex)
        {
            Log(message, ex, SysConsts.DefaultLoggerName, LogLevel.DEBUG);
        }

        public static void Log(string message, string loggerName, LogLevel level)
        {
            ILog logger = LogManager.GetLogger(loggerName);

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

        public static void Log(string message, Exception ex, string loggerName, LogLevel level)
        {
            ILog logger = LogManager.GetLogger(loggerName);

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
