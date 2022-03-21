using Logitude.Base.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTrackingTests.Configurations
{
    public class ConfigurationsGetter
    {
        private static readonly object _lock = new object ();
        private static ConfigurationsGetter instance;
        private static Configurations configurations;

        public static ConfigurationsGetter GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigurationsGetter();
                    }
                }
            }
            return instance;
        }
        ConfigurationsGetter()
        {
            GetConnectionStringsFromConfigFile();
        }
        public static string GetMainDBConnectionString()
        {
            return configurations?.MainDBConnection;
        }
        public static string GetCargoDBConnectionString()
        {
            return configurations?.CargoDBConnection;
        }
        private void GetConnectionStringsFromConfigFile()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string configurationsFilePath = Path.Combine(projectDirectory, @"Configurations\Configurations.xml");
            if (File.Exists(configurationsFilePath))
            {
                string configurationsXmlString = File.ReadAllText(configurationsFilePath);
                configurations = configurationsXmlString.ParseXML<Configurations>();
            }
        }
    }
}
