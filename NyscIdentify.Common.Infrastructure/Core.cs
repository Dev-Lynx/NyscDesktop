using NLog;
using NLog.Config;
using NLog.Targets;
using NyscIdentify.Common.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NyscIdentify.Common.Infrastructure
{
    public static class Core
    {
        #region Properties
        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        static bool Initialized { get; set; }
        static bool IsMainInstance { get; set; }
        static Mutex Instance { get; set; }
        static Thread ListenServer { get; set; }

        #region Shortcuts
        public static Application App => Application.Current;
        public static Dispatcher Dispatcher => Application.Current.Dispatcher;
        public static ResourceDictionary Resources => Application.Current.Resources;
        public static Assembly CoreAssembly => Assembly.GetExecutingAssembly();
        #endregion

        #region Statics
        public const string PRODUCT_NAME = "NYSC Identify";
        public const string PRODUCT_VERSION = "v1.0";
        public const string SOURCE_COMPANY = "Dev-Lynx";
        public const string AUTHOR = "Prince Owen";

        #region Prism

        #region Regions
        public const string MAIN_REGION = "Main Region";
        public const string ACCOUNT_REGION = "Account Region";
        public const string IDENTITY_TIMELINE_REGION = "Identity Timeline Region";
        public const string CONTENT_REGION = "Content Region";
        #endregion

        #region Views
        public const string MENU_VIEW = "Menu View";
        public const string HOME_VIEW = "Home View";
        public const string MAIN_VIEW = "Main View";
        public const string IDENTITY_VIEW = "Identity View";
        public const string SINGLE_IDENTITY_VIEW = "Single Identity View";
        #endregion

        #endregion

        #region Directories
        public readonly static string SYSTEM_DATA_DIR = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public readonly static string BASE_DIR = Directory.GetCurrentDirectory();
        public readonly static string WORK_DIR = Path.Combine(SYSTEM_DATA_DIR, PRODUCT_NAME);
        public readonly static string TEMP_DIR = Path.Combine(WORK_DIR, "Temp");
        public readonly static string DATABASE_DIR = Path.Combine(WORK_DIR, "Data");
        public readonly static string RESOURCE_DIR = Path.Combine(DATABASE_DIR, "Resources");
        public readonly static string LOG_DIR = Path.Combine(WORK_DIR, "Logs");
        #endregion

        #region Paths
        public readonly static string CONFIGURATION_PATH = Path.Combine(DATABASE_DIR, "config.xml");
        public readonly static string DATABASE_PATH = Path.Combine(DATABASE_DIR, "Data.sqlite");
        public static readonly string ERROR_LOG_PATH = Path.Combine(LOG_DIR, ERROR_LOG_NAME + ".log");
        #endregion

        #region Names
        public readonly static string SERVER_NAME = $"{PRODUCT_NAME} Server";
        public const string CONSOLE_LOG_NAME = "console-debugger";
        public const string LOG_LAYOUT = "${longdate}|${uppercase:${level}}| ${message}";
        public const string ERROR_LOG_NAME = "Errors";
        #endregion

        #region Misc
        public readonly static string API_BASE = "http://localhost:5000/api/";
        public readonly static string API_GET_IDENTITIES = API_BASE + "identity";
        public readonly static string API_GET_SINGLE_IDENTITY = API_BASE + "identity/single";
        public readonly static string API_APPROVE_IDENTITY = API_BASE + "identity/approve";
        public readonly static string API_DECLINE_IDENTITY = API_BASE + "identity/decline";
        public readonly static string API_GET_ACTIVITIES = API_BASE + "identity/activities";
        public readonly static string API_REVOKE_IDENTITY_EVALUATION = API_BASE + "identity/revokeEvaluation";
        public readonly static string API_UPDATE_ACCOUNT = API_BASE + "account/update";
        public readonly static string API_CHANGE_PASSWORD = API_BASE + "account/changePassword";
        public readonly static string ORIGIN = $"{PRODUCT_NAME} {PRODUCT_VERSION} Desktop Client";
        #endregion

        #endregion

        #endregion

        #region Methods
        public static void Initialize()
        {
            if (Initialized) return;
            Initialized = true;

            AnalyzeInstance();
            ClearDirectory(Core.TEMP_DIR);
            CreateDirectories(Core.DATABASE_DIR, Core.LOG_DIR, Core.TEMP_DIR, Core.RESOURCE_DIR);
            ConfigureLogger();

#if DEBUG
            // Register and Initialize the Console Debugger
            Trace.Listeners.Add(new ConsoleTraceListener(true));
            Debug.Listeners.Add(new ConsoleTraceListener(true));
            ConsoleManager.Show();

            Log.Info("Welcome to the {0} Debugger", PRODUCT_NAME);
#endif
        }


        static void ConfigureLogger()
        {
            var config = new LoggingConfiguration();

#if DEBUG
            var debugConsole = new ColoredConsoleTarget()
            {
                Name = Core.CONSOLE_LOG_NAME,
                Layout = Core.LOG_LAYOUT,
                Header = $"{PRODUCT_NAME} Debugger"
            };

            var debugRule = new LoggingRule("*", LogLevel.Debug, debugConsole);
            config.LoggingRules.Add(debugRule);
#endif

            var errorFileTarget = new FileTarget()
            {
                Name = Core.ERROR_LOG_NAME,
                FileName = Core.ERROR_LOG_PATH,
                Layout = Core.LOG_LAYOUT
            };

            config.AddTarget(errorFileTarget);

            var errorRule = new LoggingRule("*", LogLevel.Error, errorFileTarget);
            config.LoggingRules.Add(errorRule);

            LogManager.Configuration = config;

            LogManager.ReconfigExistingLoggers();
        }

        #region Helpers
        /// <summary>
        /// Easy and safe way to create multiple directories. 
        /// </summary>
        /// <param name="directories">The set of directories to create</param>
        public static void CreateDirectories(params string[] directories)
        {
            if (directories == null || directories.Length <= 0) return;

            foreach (var directory in directories)
                try
                {
                    if (Directory.Exists(directory)) continue;

                    Directory.CreateDirectory(directory);
                    Log.Info("A new directory has been created ({0})", directory);
                }
                catch (Exception e)
                {
                    Log.Error("Error while creating directory {0} - {1}", directory, e);
                }
        }

        public static void ClearDirectory(string directory, bool removeDirectory = false)
        {
            if (string.IsNullOrWhiteSpace(directory)) return;

            foreach (var d in Directory.EnumerateDirectories(directory))
                ClearDirectory(d, true);

            foreach (var file in Directory.EnumerateFiles(directory, "*"))
                try { File.Delete(file); }
                catch (Exception e) { Log.Error("Failed to delete {0}\n", file, e); }

            if (removeDirectory)
                try { Directory.Delete(directory); }
                catch (Exception ex) { Log.Error("An error occured while attempting to remove a directory ({0})\n{1}", directory, ex); }
        }

        public static string CreateTemporaryPath(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            string ext = Path.GetExtension(fileName);
            string name = Guid.NewGuid().ToString() + ext;
            while (File.Exists(Path.Combine(Core.TEMP_DIR, name)))
                name = Guid.NewGuid().ToString() + ext;


            return Path.Combine(Core.TEMP_DIR, name);
        }
        #endregion

        #region Singleton
        static async void AnalyzeInstance()
        {
            try
            {
                Instance = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out bool newInstance);
                IsMainInstance = newInstance;

                if (IsMainInstance)
                {
                    ListenServer = new Thread(Listen);
                    ListenServer.IsBackground = true;
                    ListenServer.Start();
                }
                else
                {
                    Instance.Dispose();

                    var args = new string[0];
                    await SendMessage(args);
                    Application.Current.Shutdown();
                }
            }
            catch { }
        }

        static async void Listen()
        {
            while (true)
            {
                try
                {
                    using (var server = new NamedPipeServerStream(Core.SERVER_NAME))
                    using (var reader = new StreamReader(server))
                    {
                        await server.WaitForConnectionAsync();

                        var args = new List<string>();

                        while (!reader.EndOfStream)
                        {
                            string buffer = string.Empty;
                            char c = '\0';
                            bool allowSpace = false;

                            while (!reader.EndOfStream)
                            {
                                c = (char)reader.Read();

                                if (allowSpace && c == '"') allowSpace = false;
                                else if (c == '"') allowSpace = true;

                                if (c == '"') continue;
                                if (c == ' ' && !allowSpace) break;

                                buffer += c;
                            }

                            args.Add(buffer);
                        }

                        //Application.Current.Dispatcher.Invoke(() => Core.ParseArguments(args.ToArray()));
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.Error(ex, "An unexpected error occured in the server");
                }
            }
        }

        static async Task<bool> SendMessage(params string[] messages)
        {
            if (messages == null) return false;

            try
            {
                using (var mutex = Mutex.OpenExisting(Assembly.GetExecutingAssembly().GetName().Name))
                using (var client = new NamedPipeClientStream(Core.SERVER_NAME))
                using (var writer = new StreamWriter(client))
                {
                    while (!client.IsConnected)
                        await client.ConnectAsync();

                    for (int i = 0; i < messages.Length; i++)
                    {
                        writer.Write("\"{0}\"", messages[i]);

                        if (i < messages.Length - 1) writer.Write(" ");
                    }
                }
            }
            catch (Exception e)
            {
                Core.Log.Error(e);
                return false;
            }
            return true;
        }
        #endregion

        #endregion
    }
}
