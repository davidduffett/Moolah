using System.Collections.Specialized;
using System.Text;
using NLog;

namespace Moolah
{
    public static class LoggerExtensions
    {
        public static void Log(this Logger logger, string message, object details = null)
        {
            logger.Info(() =>
            {
                var sb = new StringBuilder();
                sb.Append(message);
                if (details is NameValueCollection)
                {
                    foreach (var key in ((NameValueCollection)details).AllKeys)
                        sb.AppendFormat(" {0}: '{1}'", key, ((NameValueCollection)details)[key]);
                }
                else if (details != null)
                {
                    foreach (var property in details.GetType().GetProperties())
                        sb.AppendFormat(" {0}: '{1}'", property.Name, property.GetValue(details, null));
                }
                return sb.ToString();
            });
        }
    }
}