using MeatadataGeneratorTool.Helpers;
using MeatadataGeneratorTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp1
{
    internal class LXMLCreatorV1
    {
        static List<string> MissingDataTypes = new List<string>();
        static List<string> properties = new List<string>();
        static List<string> keys = new List<string>();
        static Dictionary<string, string> foreignkeys = new Dictionary<string, string>();
        static Dictionary<string, string> DataTypes = new Dictionary<string, string>();
        static List<TableInfo> Tables = new List<TableInfo>();
        static List<ColumnInfo> Columns = new List<ColumnInfo>();
        static List<RelationInfo> Relations = new List<RelationInfo>();
        static Dictionary<string, string> propertiesNames = new Dictionary<string, string>();
        static Dictionary<string, string> Classes = new Dictionary<string, string>();
        static Dictionary<string, string> ClassesMap = new Dictionary<string, string>();
        static Dictionary<string, string> MissingClassesMap = new Dictionary<string, string>();
        internal static void CreateLXML(string pocodirectoryPath, string lxmldirectoryPath)
        {
            var connectionString = "Persist Security Info=False;User ID=amitaladmin;Password=ut6py6VH7QkEiwR;Initial Catalog=Main;Server=sql-amital-dev-il.database.windows.net";
            // Retrieve all tables
            Tables = new TableRetriever(connectionString).GetAllTables();
            Columns = new SqlColumnRetriever(connectionString).GetAllColumns();
            Relations = new RelationsRetriever(connectionString).GetAllRelations();
            GetAllClasses("C:\\projects\\log-repo-amital-main");
            LXMLCreator.GetAllClasses("C:\\projects\\log-repo-amital-main\\AmitalCloud");    
            foreach (var table in Tables)
            {
                CreateLXML(table);
            }
        }
        private static void CreateLXML(TableInfo tableInfo)
        {
            if (ClassesMap.ContainsKey(tableInfo.TableName.ToUpper()))
            {
                return;
            }

            string table = LXMLCreator.GetClassByTableName(tableInfo.TableName);


            XmlDocument document = new XmlDocument();
            //document.Load(stream);
            XmlParserHelper ParserHelper = new XmlParserHelper();
            ObjectTableViewModel model = ParserHelper.LoadObjectTableData(document);





        }



        private static void GetAllClasses(string directory)
        {
            var csFiles = Directory.GetFiles(directory, "*.lxml", SearchOption.AllDirectories);

            foreach (var csFile in csFiles)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csFile);
                if (Classes.ContainsKey(fileNameWithoutExtension))
                {
                    continue;
                }
                //Classes.Add(fileNameWithoutExtension, File.ReadAllText(csFile));
                if (ClassesMap.ContainsKey(fileNameWithoutExtension.ToUpper()))
                {
                    continue;
                }
                ClassesMap.Add(fileNameWithoutExtension.ToUpper(), fileNameWithoutExtension);
            }
        }

    }
}
