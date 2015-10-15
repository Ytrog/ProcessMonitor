using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessMonitorLogViewer
{
    class LogEntry
    {
        public LogEntry(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) throw new ArgumentNullException("line");
            ParseLine(line);
        }

        private void ParseLine(string line)
        {
            Regex regex = new Regex(@"(Start:|Stop:)\t(?<date>\d{4}-(\d{2}-?){2}T(\d{2}:?){3}\.\d+\+\d{2}:\d{2})");
            IsStart = line.StartsWith("Start");
            if (!regex.IsMatch(line))
            {
                Debugger.Break();
            }

            var match = regex.Match(line);
            var date = match.Groups["date"];
            DateTime = System.DateTime.Parse(date.Value);

        }

        public bool IsStart { get; protected set; }
        public DateTime DateTime { get; protected set; }
    }
}
