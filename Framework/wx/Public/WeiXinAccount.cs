using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WeChat.DataAccess.WeiXin
{
    public delegate int RefreshAccessToken(WeiXinAccount currentAccount);
    public  class WeiXinAccount : IDisposable
    {
        public event RefreshAccessToken OnAccessTokenExpired;
        public string AccessToken { get; set; }
        public string AccountID { get; set; }
        public string Secret { get; set; }

        public int Duration = 1;

        public int Counter = 0;

        public bool IsAutoMoniter = true;

        private Thread MoniterThread = null;

        public bool IsAvaiable { get; set; }

        public WeiXinAccount(bool autoMonitor)
        {
            IsAutoMoniter = autoMonitor;
        }

        protected void StartMornitor()
        {
            Log.ILog4_Debug.Debug("进入监控执行刷新Token");
            if (IsAutoMoniter)
            {
                Log.ILog4_Debug.Debug("开始监控执行刷新Token");
                MoniterThread = new Thread(new ThreadStart(this.TimerMonitor_Elapsed));
                MoniterThread.Start();
            }
            Log.ILog4_Debug.Debug("结束监控执行刷新Token");
        }

        public void TimerMonitor_Elapsed()
        {
            Thread.Sleep(1000*10);
            while (true)
            {
                if (this.Counter >= this.Duration)
                {
                    if (OnAccessTokenExpired != null)
                    {
                        this.Duration = OnAccessTokenExpired(this);
                    }
                    this.Counter = 0;
                }

                Counter = Counter + 1;
                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            if (MoniterThread != null)
            {
                MoniterThread.Abort();
            }
        }
    }
}
