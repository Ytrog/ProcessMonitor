using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessMonitorLogViewer
{
    public partial class Form1 : Form
    {
        private const string _path = @"D:\outlook.log";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(@"D:\outlook.log"))
            {
                MessageBox.Show("no log found");
            }

            var lines = GetItems(_path).ToArray();

            var start = lines.Where(l => l.IsStart).ToArray();
            var stop = lines.Where(l => !l.IsStart).ToArray();

            EntryPair[] pairs = start.Take(stop.Length)
                .Zip(stop, (startEntry, stopEntry) => new EntryPair(startEntry, stopEntry)).ToArray();

            var grouped = pairs.GroupBy(p => p.Start.Date, pair => pair.Duration);

            var entryPairs = grouped.GroupBy(spans => spans.Key,
                spans => spans.Aggregate((span, timeSpan) => span + timeSpan));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var ep in entryPairs)
            {
                stringBuilder.AppendFormat("{0}\t{1}", ep.Key.ToString("dd-MM-yyyy"), ep.Single().ToString(@"hh\:mm\:ss"));
                stringBuilder.AppendLine();
            }
            textBox1.Text = stringBuilder.ToString();
        }

        private IEnumerable<LogEntry> GetItems(string dOutlookLog)
        {
            var lines = File.ReadAllLines(dOutlookLog);

            foreach (var l in lines)
            {
                yield return new LogEntry(l);
            }
        }
    }
}
