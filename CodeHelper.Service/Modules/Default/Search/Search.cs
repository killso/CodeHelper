using CodeHelper.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace CodeHelper.Service.Modules.Default.Search
{
    
    internal class Search : Module
    {

        public int GetRandomNumber()
        {
            return new Random().Next();
        }

        /// <summary>
        /// By default, the method use the brouser defined to use https protocol.
        /// Valid arguments: http, https. 
        /// </summary>
        /// <returns> Path to executable</returns>
        static string GetDefaultBrouserPath(string protocol = "https")
        {
            string REG_KEY_DEFAULT_HTTPS =
            @$"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\{protocol}\UserChoice";

            string REG_KEY_PATH_TO_APP =
            @"shell\open\command";

            var appClass = Registry.CurrentUser
            .OpenSubKey(REG_KEY_DEFAULT_HTTPS)
            .GetValue("ProgID")
            .ToString();
             
            var value = Registry.ClassesRoot
            .OpenSubKey(@$"{appClass}\{REG_KEY_PATH_TO_APP}")
            .GetValue(default)
            .ToString();

            // result string in variable 'value' has format: "\"C:\\..\" -any start params \""
            var startIndex = 1; 
            var lastIndex = value.IndexOf("\"", startIndex);

            return value.Substring(startIndex, lastIndex);
        }



        IEnumerable<Query> Queries;

        IEnumerable<Solution> GetSolutions(Query query) => throw new NotImplementedException();
        
        

        
        public Process OpenBrouser(string uri)
        {
            return Process.Start(GetDefaultBrouserPath(), uri);
        }

    }
}
