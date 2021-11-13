using System.Web;
using System.Configuration;
using PSWebApi.Models;

namespace PSWebApi.Utils
{
    public static class FileSystemManager
    {
        public static readonly string WebAppPath = HttpRuntime.AppDomainAppPath;
        //Применяется только при не надлежащем использовании connectionString (для указания пути размещения файла, например) в Web.config
        public static string GetPathByConnectionStringName(string connectionStringName)
        {
            return WebAppPath + ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
        public static string GetLogPath()
        {
            return WebAppPath + Constants.RELATIVE_LOG_PATH;
        }
    }
}