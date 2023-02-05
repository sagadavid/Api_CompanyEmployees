using Contracts;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
//using ILogger = NLog.ILogger;//notice
//NLog needs to have information about where to put log files on the file system,
//what the name of these files will be, and what is the minimum level of logging that we want.
//We are going to define all these constants in a text file in the main project and name it nlog.config .

namespace LoggerService
{
    internal class LoggerManager : ILoggerManager//notice using contracts
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();
       
        public LoggerManager()
        {

        }
        //As you can see, our methods are just wrappers around NLog’s methods.
        //Both ILogger and LogManager are part of the NLog namespace.
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
