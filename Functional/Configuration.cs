using System;
using System.Configuration;

namespace Functional
{
    public static class ConfigurationManagerWrapper
    {
        public static Func<string, string> ConnectionStrings = key => ConfigurationManager.ConnectionStrings[key].ConnectionString;
        public static Func<string, string> AppSettings = key => ConfigurationManager.AppSettings[key];
    }

    public static class ConnectionString<T>
    {
        public static string Value => ConfigurationManagerWrapper.ConnectionStrings(typeof(T).Name);
    }

    public class AppSettings
    {
        public static string Value<T>()
        {
            return ConfigurationManagerWrapper.AppSettings(typeof(T).Name);
        }

        public static TDefault Value<T, TDefault>(TDefault defaultValue)
        {
            var value = ConfigurationManagerWrapper.AppSettings(typeof(T).Name);
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            try
            {
                return (TDefault)Convert.ChangeType(value, typeof(TDefault));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
