using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcBib.Filters
{
    public class CustomHandleErrorAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //file halen uit config
            WriteLog(Path.Combine(HttpContext.Current.Server.MapPath("~/Logs"), "log.txt"), filterContext.Exception.ToString());
        }

        private void WriteLog(string logFile, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            sb.AppendLine("==================================================");

            System.IO.File.AppendAllText(logFile, sb.ToString());
        }
    }
}