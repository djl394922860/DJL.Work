using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace DJL.Work.Tool
{
    public static class LogHelper
    {
        private static readonly log4net.ILog _loggerError = log4net.LogManager.GetLogger("LogError");

        private static Queue<string> logQueue;
        public static void LogWriterMsg(string msg)
        {
            logQueue.Enqueue(msg);
        }

        static LogHelper()
        {
            logQueue = new Queue<string>();
            ThreadPool.QueueUserWorkItem(u =>
            {
                while (true)
                {
                    if (logQueue.Count >= 1)
                    {
                        string str = logQueue.Dequeue();
                        //写入到日志文件 lock 加锁 并发
                        lock (logQueue)
                        {
                            //记录logs.log文件同时写入数据库备份 log4net sql server
                            _loggerError.Error(str);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }
    }
}
