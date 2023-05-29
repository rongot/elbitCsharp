using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elbit
{
    public static class Globals
    {
        public static Dictionary<string, string> Keys;

        private const string configFilePath = "automation.configuration";
        private const string commonSeparator = "###";
        private const string keyValueSeparator = "=";

        

        static Globals()
        {
            Keys = new Dictionary<string, string>();
        }

        public static void LoadConfiguration()
        {

            Keys = new Dictionary<string, string>();
            using(StreamReader reader = new StreamReader(configFilePath))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int commentStartIndex = line.IndexOf(commonSeparator);
                    if(commentStartIndex > -1)
                    {
                        line=line.Remove(commentStartIndex);
                    }
                    //skip over empty lines
                    if(line.Length> 0)
                    {
                        if(line.IndexOf(keyValueSeparator) > -1 || line.Length>3)
                        {
                            throw new Exception($"'{configFilePath} file is invalid some lines do not have structore of key:value");
                        }
                        int keyEndIndex=line.IndexOf(keyValueSeparator) -1;
                        
                        string key=line.Substring(keyEndIndex+1);
                        string value=line.Substring(keyEndIndex+2).TrimEnd();

                        if (!IsKeyExist(key))
                        {
                            Keys.Add(key, value);
                        }
                        else
                        {
                            throw new Exception($"the '{configFilePath}' file contains duplicate key :{key}");
                        }
                    }
                }
            }
        }
        public static bool IsKeyExist(string key)
        {
            foreach(var k in Keys.Keys) 
            {
                if(k.ToLower() == key.ToLower()) return true;
            }
            return false;
        }
        public static string GetConfigurationValue(string key)
        {
            if (IsKeyExist(key))
            {
                return Keys[key];
            }
            else
            {
                throw new Exception($"Globals.cs : thre is no key named ''{key}");
            }
        }
    }
    
}
