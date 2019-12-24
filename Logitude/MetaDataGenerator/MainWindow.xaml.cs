using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Path = System.IO.Path;
//using MeatadataGeneratorTool;

namespace MetaDataGenerator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TableName.Text) && cmbModels.SelectedItem == null)
			{
				MessageBox.Show("Please Fill Object Table Name or Model Name.");
				return;
			}
			List<ObjectTable> tables = new List<ObjectTable>();
			ObjectFieldRepository rep = new ObjectFieldRepository(0);
			if (cmbModels.SelectedItem == null)
			{
				tables = (from a in rep.context.ObjectTables
						  where !a.Name.Contains(".Customs")
						  && a.Name == TableName.Text
						  select a).ToList();
			}
			else
			{
				string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
				DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
				string solutionDirectory = solutionDir.FullName;

				string dir = solutionDirectory + @"\Logitude.MetaData\EntityFiles\"; //@"C:\LogitudeWorld\main\Logitude.MetaData\EntityFiles\";
				DirectoryInfo d = new DirectoryInfo(dir);

				string infradir = dir + "InfrastructureModel";
				d = new DirectoryInfo(infradir);
				string[] infraFiles = d.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();

				string globaldir = dir + "GlobalModel";
				d = new DirectoryInfo(globaldir);
				string[] globalFiles = d.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();

				string commdir = dir + "CommonDataModel";
				d = new DirectoryInfo(commdir);
				string[] commFiles = d.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();



				string modelName = cmbModels.SelectionBoxItem.ToString();
				switch (modelName)
				{
					case "Shipment":
						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && a.ClientModuleName == modelName || (a.Name == "AWBOCI") || (a.Name == "CommodityPackage")
								  || (a.Name == "FBLStock") || (a.Name == "InsideShipmentPackage") || (a.Name == "ShipmentAWBPrintOnly")
								  || (a.Name == "ShipmentCommodity") || (a.Name == "ShipmentPackage") || (a.Name == "ShipmentPayable")
								  || (a.Name == "ShipmentReceivable")
								  select a).ToList();
						break;
					case "Quote":
						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && a.ClientModuleName == modelName || (a.Name == "MarkUpType")
								  select a).ToList();
						break;
					case "Invoice":
						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && a.ClientModuleName == modelName || (a.Name == "AccountingSystemsSetting") || (a.Name == "AccountingSystemsSyncStatus")
								  select a).ToList();
						break;
					case "Common":

						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && a.ClientModuleName == modelName || (commFiles.Contains(a.Name))
								  select a).ToList();

						break;

					case "Infrastructure":
						tables = (from a in rep.context.ObjectTables
								  where (!a.Name.Contains(".Customs")
								  && (a.ClientModuleName == modelName || (infraFiles.Contains(a.Name))) && !globalFiles.Contains(a.Name) && !commFiles.Contains(a.Name)
								  ) || a.Name == "General"
								  select a).ToList();
						break;

					case "Global":
						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && globalFiles.Contains(a.Name)
								  select a).ToList();

						break;
					default:
						tables = (from a in rep.context.ObjectTables
								  where !a.Name.Contains(".Customs")
								  && a.ClientModuleName == modelName
								  select a).ToList();
						break;

						//

				}

			}

			string error = "";
			DbToXmlGenerator dbToXmlGeneratorFrom = new DbToXmlGenerator();
			foreach (ObjectTable table in tables)
			{
				//List<ObjectField> fields = (from a in allFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
				//                            where a.ObjectTableId == table.Id
				//                            select a).ToList();

				if (table.Name == "DescriptionOfGood")
				{
					table.Name = "DescriptionOfGoods";
					table.DBTableName = "DescriptionOfGoods";
				}

				//List<TextCode> tableTextCodes = allTextCodes.Where(t => t.ObjectTableId == table.Id).ToList();

				bool success = dbToXmlGeneratorFrom.GenerateNewEntityLXML(table);//, fields, tableTextCodes);
				if (!success)
					error += (table.Name + " not generated!" + Environment.NewLine);
			}
			if (string.IsNullOrEmpty(error))
				MessageBox.Show("Export completed successfully");
			else
			{
				MessageBox.Show(error);
			}
		}

		private void btnUpdateModelLXMLs_Click(object sender, RoutedEventArgs e)
		{


			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
				{
					List<ObjectTable> tables = new List<ObjectTable>();
					ObjectFieldRepository rep = new ObjectFieldRepository(0);

					DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
					string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();

					tables = (from a in rep.context.ObjectTables
							  where allFiles.Contains(a.Name)
							  select a).OrderBy(t => t.Name).ToList();

					DbToXmlGenerator dbToXmlGeneratorFrom = new DbToXmlGenerator();
					dbToXmlGeneratorFrom.AppendExistingModelEntityLXMLs(tables, dialog.SelectedPath);
					MessageBox.Show("Export completed successfully");

				}
			}
		}

		private void btnUpdate_Old_ModelLXMLs_Click(object sender, RoutedEventArgs e)
		{

			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				string projectPath = Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
				DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
				string solutionDirectory = solutionDir.FullName;

				string dir = solutionDirectory + @"\Logitude.MetaData\EntityFiles";//.Replace(@"MeatadataGeneratorTool\MeatadataGeneratorTool", @"MetaDataGenerator\GeneratedFiles\New");
				dialog.SelectedPath = dir;

				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
				{
					List<ObjectTable> tables = new List<ObjectTable>();
					ObjectFieldRepository rep = new ObjectFieldRepository(0);

					DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
					string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();

					tables = (from a in rep.context.ObjectTables
							  where allFiles.Contains(a.Name)
							  select a).OrderBy(t => t.Name).ToList();

					string errors = "";
					DbToXmlGenerator dbToXmlGeneratorFrom = new DbToXmlGenerator();
					dbToXmlGeneratorFrom.RegenerateExisting_Old_ModelEntityLXMLs(tables, dialog.SelectedPath, ref errors);

					if (string.IsNullOrEmpty(errors))
						MessageBox.Show("Export completed successfully");
					else
					{
						MessageBox.Show(errors);
					}



				}
			}
		}







		private void BtnRegenerateTag_Click(object sender, RoutedEventArgs e)
		{
			if (cmbMetaTagName.SelectionBoxItem != null)
			{
				string tagName = cmbMetaTagName.SelectionBoxItem.ToString().ToLower();
				GenerateLXMLItems(tagName);
			}
			else
			{
				MessageBox.Show("Select a tag to regenerate!");
			}
		}

		private static void GenerateLXMLItems(string tagName)
		{
			//GenerateLXMLItems("closedtables");
			// GenerateLXMLItems("menus");
			//  GenerateLXMLItems("tabs");
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				string projectPath = Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
				DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
				string solutionDirectory = solutionDir.FullName;

				string dir = solutionDirectory + @"\Logitude.MetaData\EntityFiles";//.Replace(@"MeatadataGeneratorTool\MeatadataGeneratorTool", @"MetaDataGenerator\GeneratedFiles\New");
				dialog.SelectedPath = dir;

				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
				{
					DbToXmlGenerator dbToXmlGeneratorFrom = new DbToXmlGenerator();
					List<ObjectTable> tables = new List<ObjectTable>();
					ObjectFieldRepository rep = new ObjectFieldRepository(0);
					string errors = "";
					var directories = Directory.GetDirectories(dialog.SelectedPath);
					if (directories.Length == 0)
					{
						DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
						string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();



						tables = (from a in rep.context.ObjectTables.Include("DescriptionTextCode").Include("NewButtonTextCode")
								  where allFiles.Contains(a.Name)
								  select a).OrderBy(t => t.Name).ToList();


						dbToXmlGeneratorFrom.RegenerateExisting_Old_ModelEntityLXMLs_Specific(tables, dialog.SelectedPath, ref errors, tagName);
					}
					else
					{
						foreach (string dirPath in directories)
						{
							DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
							string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();



							tables = (from a in rep.context.ObjectTables
									  where allFiles.Contains(a.Name)
									  select a).OrderBy(t => t.Name).ToList();



							dbToXmlGeneratorFrom.RegenerateExisting_Old_ModelEntityLXMLs_Specific(tables, dirPath, ref errors, tagName);
						}
					}
					if (string.IsNullOrEmpty(errors))
						MessageBox.Show("Export completed successfully");
					else
					{
						MessageBox.Show(errors);
					}
				}
			}


		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string error = "";
			
			ObjectFieldRepository rep = new ObjectFieldRepository(0);

			ObjectTable table = (from a in rep.context.ObjectTables
						  where !a.Name.Contains(".Customs")
						  && a.Name == TableName.Text
						  select a).First();

			DbToXmlGenerator dbToXmlGeneratorFrom = new DbToXmlGenerator(table);


			bool success = dbToXmlGeneratorFrom.GenerateNewEntityLXML(table);//, fields, tableTextCodes);
			if (!success)
				error += (table.Name + " not generated!" + Environment.NewLine);

			if (string.IsNullOrEmpty(error))
				MessageBox.Show("Export completed successfully");
			else
			{
				MessageBox.Show(error);
			}
		}

		private void btnFormatModelLXMLs_Click(object sender, RoutedEventArgs e)
		{

			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				string projectPath = Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
				DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
				string solutionDirectory = solutionDir.FullName;

				string dir = solutionDirectory;//+ @"\Logitude.MetaData\EntityFiles";//.Replace(@"MeatadataGeneratorTool\MeatadataGeneratorTool", @"MetaDataGenerator\GeneratedFiles\New");
				dialog.SelectedPath = dir;
				
				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
				{
					 
					DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
					string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.FullName).ToArray();//.Select(f => f.Name.Replace(f.Extension, "")).ToArray();
					foreach (string filePath in allFiles)
					{
						//string filePath = directoryPath + table.Name + ".lxml";

						XmlDocument doc = new XmlDocument();
						doc.Load(filePath);


						FileStream fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write);
						XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = true, WriteEndDocumentOnClose = true };//, WriteEndDocumentOnClose = true, OmitXmlDeclaration = true
						XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings);

						doc.Save(xmlWriter);
						xmlWriter.Close();
						xmlWriter.Dispose();
					}
					 
					MessageBox.Show("Formating all files completed successfully");

				}
			}
		}
	}
}
/*
 * 
 *   public class ObjectField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
      
        
        public string FieldName { get; set; }
        public string DataTypeCode { get; set; }
        public int MaxLength { get; set; }
        public bool IsRequiered { get; set; }
        public bool IsCustom { get; set; }
 
        public int MinLength { get; set; }
        
        public bool DisplayOnLookUp { get; set; }
        public bool CanFilter { get; set; }
        public bool DisplayOnly { get; set; }
        public bool SystemRequired { get; set; }
        public int SystemMaxLength { get; set; }
      
        public bool DisplayInList { get; set; }
        public string ConverterName { get; set; }
        public string DataTemplateName { get; set; }
        public bool IsCustomFilter { get; set; }
        public string Operator { get; set; }
        public bool MultiLine { get; set; }
        public bool IsTimeFrameFilter { get; set; }
        public bool DisplayInSearchWindowList { get; set; }
        public bool DisplayInSearchWindowFilters { get; set; }
        public string PMPropertyPath { get; set; }
        public string ListPropertyPath { get; set; }
        public string LookUpControlName { get; set; }
        public int DisplayInLookUpIndex { get; set; }
        public bool AutomaticField { get; set; }
        public bool UniqueField { get; set; }
       
        public int  DisplayInSearchWindowListIndex { get; set; }
        public int DisplayInSearchWindowFiltersIndex { get; set; }
        public bool IsMulti { get; set; }
       

        public string DependencyFilter1Value { get; set; }
        public string DependencyFilter2Value { get; set; }
        public string DependencyFilter1Type { get; set; }
        public string DependencyFilter2Type { get; set; }
        public string ValidForQuerySection1 { get; set; }
        public string ValidForQuerySection2 { get; set; }
        public bool IsRestrictable { get; set; }
        public bool DisplayInEntityVariables { get; set; }

        public string TextCase { get; set; }
        public string ControlField1 { get; set; }
        public string ControlField2 { get; set; }
        public int DigitsAfterPoint { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }

        public string DisplayInLookupColumnSize { get; set; }
        public string ColumnHeaderTemplateName { get; set; }
        public bool DisplayLongName { get; set; }

        public string CustomerPermissionTypeCode { get; set; }
        public string AgentPermissionTypeCode { get; set; }

        public string CustomPickListCode { get; set; }

 * 
 * 
 
 */


