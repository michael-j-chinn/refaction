using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace refactor_me.Models
{
	public interface ILoggerService
	{
		void Log(LogLevel level, string message, Exception exception = null, Dictionary<string, object> customProps = null);
	}

	public class LoggerService : ILoggerService
	{
		private const string _ROOT_LOGGER_NAME = "root";
		private const string _DEFAULT_LOGGER_NAME = "default";
		private const string _DEFAULT_APPENDER_FOLDER = ""; // Save in the root Log folder.
		private string _directory;
		private string _filename;
		private ILog _rootLogger;

		public LoggerService(string directory = "", string filename = "")
		{
			// Set where logs will get saved.
			_directory = string.IsNullOrWhiteSpace(directory) ? AppDomain.CurrentDomain.BaseDirectory : directory;
			_directory = !_directory.EndsWith("\\") ? _directory + "\\" : _directory;
			_filename = string.IsNullOrWhiteSpace(filename) ? "log.txt" : filename;

			// Create the root logger
			var hierarchy = ((Hierarchy)LogManager.GetRepository());
			hierarchy.Threshold = Level.Debug;

			var root = hierarchy.Root;

			// Initialize root logger with the default appender.
			var rollingFileAppender = CreateRollingFileAppender(_DEFAULT_LOGGER_NAME, _DEFAULT_APPENDER_FOLDER);
			root.AddAppender(rollingFileAppender);

			// Create the logger interface object
			_rootLogger = CreateNewLogger(hierarchy, rollingFileAppender, _ROOT_LOGGER_NAME);
		}

		public void Log(LogLevel level, string message, Exception exception = null, Dictionary<string, object> customProps = null)
		{
			// Create the JSON record to be logged
			var logJson = GetJsonLogRecord(level, message, customProps, exception);

			// Write the JSON record to disk
			WriteLog(level, logJson);
		}
		private void WriteLog(LogLevel level, string message)
		{
			switch (level)
			{
				case LogLevel.DEBUG:
					_rootLogger.Debug(message);
					break;
				case LogLevel.INFO:
					_rootLogger.Info(message);
					break;
				case LogLevel.WARN:
					_rootLogger.Warn(message);
					break;
				case LogLevel.ERROR:
					_rootLogger.Error(message);
					break;
				case LogLevel.FATAL:
					_rootLogger.Fatal(message);
					break;
				default:
					break;
			}
		}
		private string GetJsonLogRecord(LogLevel level, string message, Dictionary<string, object> customProps = null, Exception exception = null)
		{
			Dictionary<string, object> record = new Dictionary<string, object>();

			// Set default properties
			record.Add("timestamp", DateTime.UtcNow);
			record.Add("level", level.ToString());
			record.Add("threadId", Thread.CurrentThread.ManagedThreadId);
			record.Add("message", message);
			record.Add("machineName", Environment.MachineName);

			// Add any custom properties from user.
			if (customProps != null)
			{
				foreach (var kvp in customProps)
				{
					if (!record.ContainsKey(kvp.Key))
						record.Add(kvp.Key, kvp.Value);
				}
			}

			// Add exception specific properties.
			if (exception != null)
			{
				record.Add("exStackTrace", exception.StackTrace);
				record.Add("exMessage", exception.Message);
				record.Add("exInnerMessage", exception.InnerException != null ? exception.InnerException.Message : string.Empty);
			}

			return JsonConvert.SerializeObject(record);
		}
		private RollingFileAppender CreateRollingFileAppender(string name, string folder)
		{
			RollingFileAppender appender = new RollingFileAppender();

			appender.Name = name;
			appender.File = _directory + (!string.IsNullOrWhiteSpace(folder) ? folder + "\\" : "") + _filename;
			appender.AppendToFile = true;
			appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
			appender.MaxSizeRollBackups = 20;
			appender.MaximumFileSize = "5000KB";
			appender.StaticLogFileName = true;
			appender.Layout = new log4net.Layout.PatternLayout("%message%newline");
			appender.Threshold = Level.All;
			appender.LockingModel = new FileAppender.MinimalLock();
			appender.ActivateOptions();

			return appender;
		}
		private ILog CreateNewLogger(Hierarchy hierarchy, IAppender appender, string name)
		{
			var logger = hierarchy.LoggerFactory.CreateLogger(hierarchy, name);

			logger.Hierarchy = hierarchy;
			logger.AddAppender(appender);
			logger.Repository.Configured = true;
			logger.Level = Level.Debug;

			return new LogImpl(logger);
		}
	}

	public enum LogLevel
	{
		DEBUG,
		INFO,
		WARN,
		ERROR,
		FATAL
	}
}