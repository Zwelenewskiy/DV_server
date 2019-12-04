using SharpConfig;

namespace DV_server
{
    class INIfileUtils
    {
        /*[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        private static readonly int BUFFER_SIZE = 1024;

        public static string ReadKey(string path, string section, string key)
        {
            StringBuilder result = new StringBuilder(BUFFER_SIZE);

            GetPrivateString(section, key, null, result, BUFFER_SIZE, path);

            return result.ToString();
        }*/

        public static string ReadKey(string path, string section, string key)
        {
            Configuration cfg = Configuration.LoadFromFile(path);

            return cfg[section][key].StringValue;
        }
    }
}