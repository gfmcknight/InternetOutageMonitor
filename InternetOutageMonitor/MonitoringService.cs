using System;
using System.IO;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Timers;

namespace InternetOutageMonitor
{
    public partial class MonitoringService : ServiceBase
    {
        private static readonly int PingTimeoutTime = 4000;
        private static readonly string NewlineChar = Environment.NewLine;

        private bool lastConnectionState;
        private int outages;

        private string file;

        public MonitoringService(string[] args)
        {
            InitializeComponent();
            outages = 0;
        }

        public void OnTimer(object sender, ElapsedEventArgs e)
        {
            bool connectionState = checkConnection();
            if (connectionState != lastConnectionState)
            {
                if (connectionState)
                {
                    File.AppendAllText(file, String.Format("Connection to the internet restored at {0}." + NewlineChar, getTimeString()));
                }
                else
                {
                    File.AppendAllText(file, String.Format("Connection to the internet lost at {0}." + NewlineChar, getTimeString()));
                    outages++;
                }
            }
            lastConnectionState = connectionState;
        }

        protected override void OnStart(string[] args)
        {
            file = Path.Combine(Environment.CurrentDirectory, "OutageLog.txt");

            File.AppendAllText(file, String.Format("InternetOutageMonitor started at {0}." + NewlineChar, getTimeString()));

            if (checkConnection())
            {
                File.AppendAllText(file, "Connected to the internet at start." + NewlineChar);
                lastConnectionState = true;
            }
            else
            {
                File.AppendAllText(file, "Disconnected from the internet at start." + NewlineChar);
                lastConnectionState = false;
                outages++;
            }

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            File.AppendAllText(file, String.Format("InternetOutageMonitor stopped at {0}." + NewlineChar, getTimeString()));
            File.AppendAllText(file, String.Format("{0} outages since start." + NewlineChar, outages));
        }

        protected override void OnContinue()
        {
        }

        private bool checkConnection()
        {
            var myping = new System.Net.NetworkInformation.Ping();

            PingReply result1;
            PingReply result2;
            PingReply result3;

            byte[] rawIP = { 8, 8, 8, 8 };
            var googleDNSAddress = new System.Net.IPAddress(rawIP);
            try
            {
                result1 = myping.Send("www.google.com", PingTimeoutTime);
                result2 = myping.Send("www.facebook.com", PingTimeoutTime);
                result3 = myping.Send(googleDNSAddress, PingTimeoutTime);
            }
            catch
            {
                return false;
            }

            return result1.Status == System.Net.NetworkInformation.IPStatus.Success ||
                   result2.Status == System.Net.NetworkInformation.IPStatus.Success ||
                   result3.Status == System.Net.NetworkInformation.IPStatus.Success;
        }

        private string getTimeString()
        {
            DateTime time = System.DateTime.Now;
            return String.Format("{0}, {1}", time.ToLongDateString(), time.ToLongTimeString());
        }
    }
}
