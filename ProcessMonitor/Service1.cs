using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProcessMonitor
{
    public partial class Service1 : ServiceBase
    {
        private Process _process;
        private const string _path = @"D:\outlook.log";
        private readonly Timer _timer = new Timer(1000);

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CheckRights();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            EventLog.WriteEntry("clock started", EventLogEntryType.Information);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PerformTick();
        }

        private void CheckRights()
        {
            string dir = Path.GetDirectoryName(_path);
            if (string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(dir);
            }

            try
            {
                File.AppendAllText(_path, string.Empty);
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException(dir + " not writable", e);
            }

            
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
        }

        private static Process GetProcess()
        {
            Process[] processes = Process.GetProcessesByName("outlook");

            return processes.FirstOrDefault();
        }

        private void PerformTick()
        {
            if (_process != null)
            {
                CheckRunning(_process);
                return;
            };
            _process = GetProcess();
            if (_process == null)
            {
                return;
            }
            Log(StartStop.Start);
        }

        private void CheckRunning(Process process)
        {
            if (process.HasExited)
            {
                _process = null;
                Log(StartStop.Stop);
            }
        }

        private static void Log(StartStop startStop)
        {
            string lines = string.Format("{0}:\t{1}{2}", startStop, DateTime.Now.ToString("O"), Environment.NewLine);
            File.AppendAllText(_path, lines);
        }
    }
}
