using System;

namespace FixMyCrypto {
    public enum LogLevel {
        Debug = 5,
        Info = 4,
        Warning = 3,
        Error = 2,
        None = 0,
    }

    class Log {
        private static void LogStdout(string str = null) {
            Console.WriteLine(str);
        }
        private static void LogErr(string str = null) {
            Console.Error.WriteLine(str);
        }
        public static void All(string str = null) {
            LogStdout(str);
        }
        public static void Debug(string str = null) {
            if ((LogLevel)Settings.logLevel >= LogLevel.Debug)
                LogStdout(str);
        }

        public static void Info(string str = null) {
            if ((LogLevel)Settings.logLevel >= LogLevel.Info)
                LogStdout(str);
        }

        public static void Warning(string str = null) {
            if ((LogLevel)Settings.logLevel >= LogLevel.Warning)
                LogErr(str);
       }
        public static void Error(string str = null) {
            if ((LogLevel)Settings.logLevel >= LogLevel.Error)
                LogErr(str);
       }
    }
}