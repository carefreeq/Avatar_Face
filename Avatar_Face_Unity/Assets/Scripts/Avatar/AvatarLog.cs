using UnityEngine;
using System.IO;
using System.Diagnostics;

namespace Avatar
{
    /// <summary>
    /// AvatarLog类
    /// </summary>
    public sealed class AvatarDebug
    {
        /// <summary>
        /// AvtarLog回调委托
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="stack">栈跟踪</param>
        /// <param name="type">日志类型</param>
        public delegate void LogCallBack(string log, string stack, LogType type);
        /// <summary>
        /// AvatarLog的回调事件
        /// </summary>
        public static event LogCallBack LogEvent;
        /// <summary>
        /// AvtarLog开关
        /// </summary>
        public static bool Enable { get { return enable; } set { enable = value; } }
        private static bool enable = false;
        private static string Time { get { return "[" + System.DateTime.Now + "]:"; } }
        /// <summary>
        /// AvtarLog文件目录
        /// </summary>
        public static string FolderPath { get { return Application.persistentDataPath + "/AvatarDatas"; } }
        /// <summary>
        /// AvtarLog文件名
        /// </summary>
        public static string FileName { get { return "AvatarLog.txt"; } }
        internal static void Log(string log, LogType type = LogType.Log)
        {
            if (Enable)
            {
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);
                StreamWriter text = File.AppendText(FolderPath + "/" + FileName);
                string content = "[" + type + "]" + Time + log + "\n";
                string stackInfo = "";
                StackTrace stack = new StackTrace(true);
                StackFrame[] sfs = stack.GetFrames();
                for (int i = 0; i < stack.FrameCount; i++)
                {
                    StackFrame sf = sfs[i];
                    stackInfo += sf.GetMethod() + "(at " + sf.GetFileName() + ":" + sf.GetFileLineNumber() + ")\n";
                }
                text.WriteLine(content + stackInfo);
                text.Close();
                text.Dispose();
                switch (type)
                {
                    case LogType.Log:
                        break;
                    case LogType.Warning:
                        //Debug.LogWarning(content);
                        break;
                    case LogType.Error:
                        //throw new System.Exception(content);
                        break;
                }
                if (LogEvent != null)
                    LogEvent(log, stackInfo, type);
            }
        }
        /// <summary>
        /// 删除Log文件
        /// </summary>
        public static void Delete()
        {
            File.Delete(FolderPath + "/" + FileName);
        }
    }
}