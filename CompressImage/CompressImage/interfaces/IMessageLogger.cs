using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressImage.interfaces
{
    public interface IMessageLogger
    {
        void ConfigureLogger(string name);
        void Log(string message);
        void Debug(string message);
        void StackTrace(string message, Exception exception);
        void Error(string message, string exception);
        bool EnableDebug { get; set; }
    }
}
