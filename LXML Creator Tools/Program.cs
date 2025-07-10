using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            Console.Write("Choose the option: 1- convert LXML to json-entity , 2 - Create LXML ");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ConvertLxmlToJson();
                    break;
                case "2":
                    CreateLxml();
                    break;
                case "3":
                    CreateLxmlV1();
                    break;
                default:
                    test();
                    break;
            }
        }

        private static void CreateLxmlV1()
        {
            Console.Write("Enter the directory path containing entity files: <C:\\projects\\log-repo-amital-main\\AmitalCloud\\AmitalCloud.Infrastructure.Domain\\EntityPOCOs\\Manual>");
            string pocodirectoryPath = Console.ReadLine();
            if (pocodirectoryPath == "")
            {
                pocodirectoryPath = @"C:\projects\log-repo-amital-main\AmitalCloud\AmitalCloud.Infrastructure.Domain\EntityPOCOs\Manual";
            }

            Console.Write("Enter the directory path for LXML files: <C:\\projects\\New Lxml>");
            string lxmldirectoryPath = Console.ReadLine();
            if (lxmldirectoryPath == "")
            {
                lxmldirectoryPath = @"C:\projects\New LxmlFromDB";
            }


            if (Directory.Exists(pocodirectoryPath))
            {
                LXMLCreatorV1.CreateLXML(pocodirectoryPath, lxmldirectoryPath);
                Console.WriteLine("Conversion completed successfully.");
            }
            else
            {
                Console.WriteLine("The specified directory does not exist.");
            }
        }

        private static void ConvertLxmlToJson()
        {

            Console.Write("Enter the directory path containing XML files: ");
            string directoryPath = Console.ReadLine();

            if (Directory.Exists(directoryPath))
            {
                Converter.ConvertXmlFilesToJson(directoryPath);
                Console.WriteLine("Conversion completed successfully.");
            }
            else
            {
                Console.WriteLine("The specified directory does not exist.");
            }
        }

        private static void CreateLxml()
        {
            Console.Write("Enter the directory path containing entity files: <C:\\projects\\log-repo-amital-main\\AmitalCloud\\AmitalCloud.Infrastructure.Domain\\EntityPOCOs\\Manual>");
            string pocodirectoryPath = Console.ReadLine();
            if (pocodirectoryPath == "")
            {
                pocodirectoryPath = @"C:\projects\log-repo-amital-main\AmitalCloud\AmitalCloud.Infrastructure.Domain\EntityPOCOs\Manual";
            }

            Console.Write("Enter the directory path for LXML files: <C:\\projects\\New LxmlFromDB>");
            string lxmldirectoryPath = Console.ReadLine();
            if (lxmldirectoryPath == "") 
            {
                lxmldirectoryPath = @"C:\projects\New LxmlFromDB";
            }


            if (Directory.Exists(pocodirectoryPath))
            {
                LXMLCreator.CreateLXML(pocodirectoryPath, lxmldirectoryPath);
                Console.WriteLine("Conversion completed successfully.");
            }
            else
            {
                Console.WriteLine("The specified directory does not exist.");
            }
        }
        private static void GetColumns()
        {
            var connectionString = "your_connection_string_here";
            var retriever = new SqlColumnRetriever(connectionString);
            var columns = retriever.GetAllColumns();
            foreach (var column in columns)
            {
                Console.WriteLine($"Table: {column.TableName}, Column: {column.ColumnName}, Data Type: {column.DataType}, Is Nullable: {column.IsNullable}, Max Length: {column.MaxLength}");
            }
        }
        private static void test()
        {
            //var connectionString = "Main,amitaladmin,ut6py6VH7QkEiwR,sql-amital-dev-il.database.windows.net";


            var connectionString = "Persist Security Info=False;User ID=amitaladmin;Password=ut6py6VH7QkEiwR;Initial Catalog=Main;Server=sql-amital-dev-il.database.windows.net";
            // Retrieve all tables
            var tableRetriever = new TableRetriever(connectionString);
            var tables = tableRetriever.GetAllTables();
            foreach (var table in tables)
            {
                Console.WriteLine($"Table: {table}");
            }

            // Retrieve all columns
            var columnRetriever = new SqlColumnRetriever(connectionString);
            var columns = columnRetriever.GetAllColumns();

            var tablecolumns =  columns.Where(a => a.TableName == "dbo.Users").ToList();

            foreach (var column in columns)
            {
                Console.WriteLine($"Table: {column.TableName}, Column: {column.ColumnName}, Data Type: {column.DataType}, Is Nullable: {column.IsNullable}, Max Length: {column.MaxLength}");
            }



            // Retrieve all relations
            var relationsRetriever = new RelationsRetriever(connectionString);
            var relations = relationsRetriever.GetAllRelations();
            foreach (var relation in relations)
            {
                Console.WriteLine($"FK Table: {relation.ForeignKeyTable}, FK Column: {relation.ForeignKeyColumn}, PK Table: {relation.PrimaryKeyTable}, PK Column: {relation.PrimaryKeyColumn}");
            }




        }
    }
}