using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Xml;
    using System.Xml.Linq;
    using Newtonsoft.Json;

    class Converter
    {


        internal static void ConvertXmlFilesToJson(string directoryPath)
        {
            string[] xmlFiles = Directory.GetFiles(directoryPath, "*.lxml");

            foreach (string xmlFile in xmlFiles)
            {
                try
                {
                    string xmlContent = File.ReadAllText(xmlFile);
                    XDocument xmlDoc = XDocument.Parse(xmlContent);
                    string jsonContent = JsonConvert.SerializeXNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
                    var json = JsonDocument.Parse(jsonContent);
                    string jsonFilePath = Path.ChangeExtension(xmlFile, ".json");
                    File.WriteAllText(jsonFilePath, jsonContent);

                    Console.WriteLine($"Converted: {Path.GetFileName(xmlFile)} -> {Path.GetFileName(jsonFilePath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {Path.GetFileName(xmlFile)}: {ex.Message}");
                }
            }
        }

        internal void LoadJson(string file)
        {
        var json = File.ReadAllText(file);

          var jsonDoc =   JsonDocument.Parse(json);
            jsonDoc.RootElement.GetProperty("name");
        }
    }

}
