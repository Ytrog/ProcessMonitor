using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitorLogViewer
{
    class EntryPair
    {
        public EntryPair(LogEntry startEntry, LogEntry stopEntry)
        {
            if (startEntry == null)
            {
                throw new ArgumentNullException("startEntry");
            }

            if (stopEntry == null)
            {
                throw new ArgumentNullException("stopEntry");
            }

            Start = startEntry.DateTime;
            Stop = stopEntry.DateTime;

            Duration = Stop - Start;
        }
        public DateTime Start { get; private set; }
        public DateTime Stop { get; private set; }
        public TimeSpan Duration { get; private set; }
    }
}
