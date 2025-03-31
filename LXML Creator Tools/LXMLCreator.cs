using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class LXMLCreator
    {
        static List<string> MissingDataTypes = new List<string>();
        static List<string> properties = new List<string>();
        static List<string> FKproperties = new List<string>();
        static List<string> keys = new List<string>();
        static Dictionary<string,string> foreignkeys = new  Dictionary<string, string>();
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
            if (!Directory.Exists(pocodirectoryPath))
            {
                throw new DirectoryNotFoundException($"The directory {pocodirectoryPath} does not exist.");
            }

            if (!Directory.Exists(lxmldirectoryPath))
            {
                Directory.CreateDirectory(lxmldirectoryPath);
            }


            #region Main Tables
            var connectionString = "Persist Security Info=False;User ID=amitaladmin;Password=ut6py6VH7QkEiwR;Initial Catalog=Main;Server=sql-amital-dev-il.database.windows.net";
            // Retrieve all tables
            Tables = new TableRetriever(connectionString).GetAllTables();
            Columns = new SqlColumnRetriever(connectionString).GetAllColumns();
            Relations = new RelationsRetriever(connectionString).GetAllRelations();
            #endregion
            #region Global tables
            connectionString = "Persist Security Info=False;User ID=amitaladmin;Password=ut6py6VH7QkEiwR;Initial Catalog=Global;Server=sql-amital-dev-il.database.windows.net";
            // Retrieve all tables
            Tables.AddRange( new TableRetriever(connectionString).GetAllTables());
            Columns.AddRange( new SqlColumnRetriever(connectionString).GetAllColumns());
            Relations.AddRange(new RelationsRetriever(connectionString).GetAllRelations());
            #endregion

            #region Logs tables
            connectionString = "Persist Security Info=False;User ID=amitaladmin;Password=ut6py6VH7QkEiwR;Initial Catalog=Logs;Server=sql-amital-dev-il.database.windows.net";
            // Retrieve all tables
            Tables.AddRange(new TableRetriever(connectionString).GetAllTables());
            Columns.AddRange(new SqlColumnRetriever(connectionString).GetAllColumns());
            Relations.AddRange(new RelationsRetriever(connectionString).GetAllRelations());
            #endregion







            GetAllClasses("C:\\projects\\log-repo-amital-main\\AmitalCloud");
            //GetAllClasses("C:\\projects\\log-repo-amital-main\\Logitude");
            DataTypes.Add("string", "Text");
            DataTypes.Add("int", "Integer");
            DataTypes.Add("Int32", "Integer");
            DataTypes.Add("Int64", "Integer");
            DataTypes.Add("Int16", "Integer");
            DataTypes.Add("bool", "Boolean");
            DataTypes.Add("byte[]", "Byte[]");
            DataTypes.Add("decimal", "Decimal");
            DataTypes.Add("double", "Double");
            DataTypes.Add("datetime2", "DateTime");
            DataTypes.Add("datetime", "DateTime");

            DataTypes.Add("varchar", "Text");
            DataTypes.Add("nvarchar", "nText");
            DataTypes.Add("bit", "Boolean");
            DataTypes.Add("bigint", "BigInteger");
            DataTypes.Add("float", "Double");
            DataTypes.Add("numeric", "Integer");
            DataTypes.Add("varbinary", "Byte[]");



            var csFiles = Directory.GetFiles(pocodirectoryPath, "*.cs");

            foreach (var csFile in csFiles)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csFile);
                var xmlFilePath = Path.Combine(lxmldirectoryPath, fileNameWithoutExtension + ".lxml");
                if (File.Exists(xmlFilePath))
                {
                    continue;
                }
                FKproperties = new List<string>();
                properties = new List<string>();
                propertiesNames = new Dictionary<string, string>();
                keys = new List<string>();
                foreignkeys = new Dictionary<string, string>();
                bool isKey = false;
                using (var reader = new StreamReader(csFile))
                {
                    string line;
                    string foreignKey = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith("[Key]"))
                        {
                            isKey = true;
                        }
                        if (line.Trim().StartsWith("[ForeignKey("))
                        {
                            foreignKey = line.Trim().Split('"')[1];
                        }
                        if (line.Trim().StartsWith("public") || line.Trim().StartsWith("internal"))
                        {
                            if (line.Contains("class"))
                            {
                                continue;
                            }
                            if (isKey)
                            {
                                keys.Add(line.Trim());
                                isKey = false;
                            }
                            if (foreignKey != "")
                            {
                                foreignkeys.Add(foreignKey, line.Trim());
                                foreignKey = "";
                                SplitLine(line);
                                continue;
                            }
                            properties.Add(line.Trim());

                            SplitLine(line);

                        }
                    }
                }
                if (properties.Count == 0) continue;
                TableInfo table = GetTableInfoByFileName(fileNameWithoutExtension);
                if (table == null)
                {
                    continue;
                }
                XDocument xDocument = CreateNewDoc(table, fileNameWithoutExtension);
                if (xDocument == null)
                {
                    continue;
                }
                Columns.Where(x => x.TableName.ToUpper() == (table.TableName).ToUpper()).OrderBy(x => x.OrdinalPosition).ToList().ForEach(x => xDocument.Root?.Add(CreateNewProperty(x)));
                Relations.Where(x => x.PrimaryKeyTable.ToUpper() == (table.TableName).ToUpper()).ToList().ForEach(x => xDocument.Root?.Add(CreateNewVirtualProperty(x)));

                //foreach (var property in properties)
                //{
                //    xDocument.Root?.Add(CreateNewField_old(fileNameWithoutExtension,property));
                //}
                xDocument.Save(xmlFilePath);
            }

            foreach (var missingDataType in MissingDataTypes)
            {
                Console.WriteLine($"Missing data type: {missingDataType}");
            }
            foreach (var missingClass in MissingClassesMap)
            {
                Console.WriteLine($"Missing class: {missingClass.Key} - {missingClass.Value}");
            }
        }

        private static TableInfo GetTableInfoByFileName(string fileNameWithoutExtension)
        {
            var table = Tables.FirstOrDefault(x => x.TableName.ToUpper() == (fileNameWithoutExtension + "s").ToUpper());
            if (table == null)
            {
                table = Tables.FirstOrDefault(x => x.TableName.ToUpper() == (fileNameWithoutExtension).ToUpper());
            }
            if (table == null)
            {
                var temp = Tables.Where(x => x.TableName.ToUpper().StartsWith( (fileNameWithoutExtension).ToUpper().Substring(0, fileNameWithoutExtension.Length-1)));
                if (temp.Count() == 1)
                {
                    table = temp.First();
                }   
            }

            return table;
        }

        internal static void GetAllClasses(string directory)
        {
            var csFiles = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            foreach (var csFile in csFiles) {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csFile);
                if (Classes.ContainsKey(fileNameWithoutExtension))
                {
                    continue;
                }
                Classes.Add(fileNameWithoutExtension, File.ReadAllText(csFile));
                if (!ClassesMap.ContainsKey(fileNameWithoutExtension.ToUpper())) ClassesMap.Add(fileNameWithoutExtension.ToUpper(), fileNameWithoutExtension);
            }
        }

        private static void SplitLine(string line)
        {
            line.Split(' ').ToList().ForEach(x =>
            {
                if (!propertiesNames.ContainsKey(x.ToUpper()))
                {
                    propertiesNames.Add(x.ToUpper(), x);
                }
            });
        }

        private static XDocument CreateNewDoc(TableInfo table,string name)
        {
            string dxmlDatabaseSchemaCode = table?.SchemaName ?? "dbo";
            string dxmlDatabaseTypeCode = table?.TableCatalog ?? "Main";
            string tableName = table?.TableName ?? name;
            if (tableName.Substring(0,name.Length) != name)
            {
                tableName = tableName.ToLower().Replace(name.ToLower(), name);
            }
            var element = new XElement("entity", new XAttribute("Name", name));
            element.Add(new XAttribute("Id", Guid.NewGuid()));
            element.Add(new XAttribute("ObjectTableName", name));
            element.Add(new XAttribute("DBTableName", tableName));
            element.Add(new XAttribute("ObjectTableSingular", name));
            element.Add(new XAttribute("ObjectTablePlural", tableName));
            element.Add(new XAttribute("DefaultText", name ));
            element.Add(new XAttribute("IsNewWizard", "false"));
            element.Add(new XAttribute("HasCustomValidator", "false"));
            element.Add(new XAttribute("KeyPropertyPath", "&quot;Code&quot;"));
            element.Add(new XAttribute("AutoCompleteSearchWindow", "false"));
            element.Add(new XAttribute("IsClosed", "true"));
            element.Add(new XAttribute("CacheOnClient", "false"));
            element.Add(new XAttribute("EditableFromAutoCompleteWindow", "false"));
            element.Add(new XAttribute("HasCounter", "false"));
            element.Add(new XAttribute("EnableEditFromLOV", "false"));
            element.Add(new XAttribute("EnableAddFromLOV", "false"));
            element.Add(new XAttribute("IsRestrictable", "false"));
            element.Add(new XAttribute("IsMain", "true"));
            element.Add(new XAttribute("IsMetadataOnlyTable", "false"));
            element.Add(new XAttribute("IsAutoComplete", "false"));
            element.Add(new XAttribute("CustomFieldsCount", "0"));
            element.Add(new XAttribute("HasCustomFields", "false"));
            element.Add(new XAttribute("InActive", "false"));
            element.Add(new XAttribute("SearchFields", ""));
            element.Add(new XAttribute("IsSaveButtonVisible", "true"));
            element.Add(new XAttribute("EnableSecurity", "false"));
            element.Add(new XAttribute("ObjectTableTypeCode", " & quot;MD&quot;"));
            element.Add(new XAttribute("IsComposition", "false"));
            element.Add(new XAttribute("MaxNumberOfCustomFields", "0"));
            element.Add(new XAttribute("AllowCustomFields", "false"));
            element.Add(new XAttribute("HasDynamicHeader", "false"));
            element.Add(new XAttribute("HasDocuments", "false"));
            element.Add(new XAttribute("IsLookUp", "false"));
            element.Add(new XAttribute("IsEditable", "true"));
            element.Add(new XAttribute("CloseTableCode", " & quot;Code&quot;"));
            element.Add(new XAttribute("CloseTableName", " & quot;Name&quot;"));
            element.Add(new XAttribute("DxmlDatabaseTypeCode", dxmlDatabaseTypeCode));
            element.Add(new XAttribute("DxmlDatabaseSchemaCode", dxmlDatabaseSchemaCode));
            return new XDocument(element);
        }

        private static XElement CreateNewProperty(ColumnInfo columnInfo) //(string table,string property)
        {

            string propertyName = ConvertName(columnInfo.ColumnName);
            //string isPrimaryKey = columnInfo.IsPrimary ? "true" : "false";
            string isNullable = columnInfo.IsNullable ? "true" : "false";
            string isMaxLength = columnInfo.MaxLength == -1 ? "true" : "false";
            string fieldDataType = columnInfo.DataType;
            int length = columnInfo.MaxLength != null ? columnInfo.MaxLength.Value : 0;
            if (length == -1)
            {
                length = 1000;
            }
            if (DataTypes.ContainsKey(fieldDataType))
            {
                fieldDataType = DataTypes[fieldDataType];
            }
            if (fieldDataType == "Byte[]")
            {
                isNullable = "false";
            }
            string isForeignKey = "false";
            string isMulti = "false";
            string foreignEntity = "";
            string navigationPropertyName = "";
            var foreignkey = Relations.FirstOrDefault(x => x.ForeignKeyColumn == columnInfo.ColumnName && x.ForeignKeyTable == columnInfo.TableName);
            if (foreignkey != null)
            {
                isForeignKey = "true";
                foreignEntity = GetClassByTableName(foreignkey.PrimaryKeyTable);
                if (foreignEntity == "")
                {
                    foreignEntity = foreignkey.PrimaryKeyTable;
                }
                navigationPropertyName = foreignEntity;
                if (FKproperties.Contains(navigationPropertyName))
                {
                    navigationPropertyName = $"{propertyName}{foreignEntity}";
                }
                FKproperties.Add(navigationPropertyName);
            }
            var element = new XElement("field", "");
            element.Add(new XAttribute("Id", Guid.NewGuid()));
            element.Add(new XAttribute("FieldName", propertyName));
            element.Add(new XAttribute("ObjectTableName", ConvertName(columnInfo.TableName)));
            element.Add(new XAttribute("FieldsDataType", fieldDataType));
            //element.Add(new XAttribute("DataTypeCode" , fieldDataType));
            element.Add(new XAttribute("MaxLength", length));
            element.Add(new XAttribute("IsRequiered", "true"));
            element.Add(new XAttribute("IsCustom", "false"));
            element.Add(new XAttribute("MinLength", "0"));
            element.Add(new XAttribute("DisplayOnLookUp", "false"));
            element.Add(new XAttribute("CanFilter", "false"));
            element.Add(new XAttribute("DisplayOnly", "false"));
            element.Add(new XAttribute("SystemRequired", "false"));
            element.Add(new XAttribute("SystemMaxLength", length));
            element.Add(new XAttribute("DisplayInList", "true"));
            element.Add(new XAttribute("IsCustomFilter", "false"));
            element.Add(new XAttribute("Operator", " & quot;StartsWith&quot;"));
            element.Add(new XAttribute("MultiLine", isMulti));
            element.Add(new XAttribute("IsTimeFrameFilter", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowList", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowFilters", "false"));
            element.Add(new XAttribute("PMPropertyPath", propertyName));
            element.Add(new XAttribute("ListPropertyPath", propertyName));
            element.Add(new XAttribute("DisplayInLookUpIndex", "0"));
            element.Add(new XAttribute("AutomaticField", "false"));
            element.Add(new XAttribute("UniqueField", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowListIndex", "0"));
            element.Add(new XAttribute("DisplayInSearchWindowFiltersIndex", "0"));
            element.Add(new XAttribute("IsMulti", isMulti));
            element.Add(new XAttribute("DependencyFilter1IsList", "false"));
            element.Add(new XAttribute("DependencyFilter2IsList", "false"));
            element.Add(new XAttribute("IsRestrictable", "false"));
            element.Add(new XAttribute("DisplayInEntityVariables", "true"));
            element.Add(new XAttribute("DigitsAfterPoint", "0"));
            element.Add(new XAttribute("InActive", "false"));
            element.Add(new XAttribute("DisplayLongName", "false"));
            if (isForeignKey == "true")
            {
                //isNullable = "false";
                element.Add(new XAttribute("IsForeignKey", isForeignKey));
                element.Add(new XAttribute("ForeignEntity", foreignEntity));
                element.Add(new XAttribute("DontBuildRelationOnDB", "false"));
                element.Add(new XAttribute("NavigationPropertyName", navigationPropertyName));
            }
            element.Add(new XAttribute("IsNullable", isNullable));
            element.Add(new XAttribute("IsPrimaryKey", columnInfo.IsPrimary ? "true" : "false"));
            element.Add(new XAttribute("NumberOfDigits", "0"));
            element.Add(new XAttribute("IsMaxLength", isMaxLength));
            element.Add(new XAttribute("AllowedinAutomationConditions", "false"));
            element.Add(new XAttribute("AutomationEmailRecipient", "false"));
            element.Add(new XAttribute("CanAutomateSetValue", "false"));
            element.Add(new XAttribute("AllowedInAirlineMessaging", "false"));
            element.Add(new XAttribute("FullFieldLable", propertyName));
            element.Add(new XAttribute("DefaultText", propertyName));
            element.Add(new XAttribute("HelpTextCode", propertyName));
            element.Add(new XAttribute("HelpTextDefaultText", " & quot;&quot;"));
            element.Add(new XAttribute("HasDataBaseField", "true"));
            element.Add(new XAttribute("Code", propertyName));
            element.Add(new XAttribute("DependencyFilter3IsList", "false"));
            element.Add(new XAttribute("AllowedInCustomerFieldsSettings", "false"));
            element.Add(new XAttribute("DisplayInDocumentReferences", "false"));
            element.Add(new XAttribute("CopyToDW", "false"));
            element.Add(new XAttribute("HasTemplate", "false"));
            element.Add(new XAttribute("IsRequired", "true"));
            element.Add(new XAttribute("IsSpellCheckedFullFieldLable", "true"));
            element.Add(new XAttribute("IsSpellCheckedHelpLocalDefaultText", "true"));
            element.Add(new XAttribute("NoMetaDataField", "false"));
            element.Add(new XAttribute("HasPMField", "true"));
            element.Add(new XAttribute("GenerateInList", "true"));
            return element;
        }

        private static XElement CreateNewVirtualProperty(RelationInfo relationInfo) //(string table,string property)
        {
            string propertyName = GetClassByTableName(relationInfo.ForeignKeyTable) + "s";
            if (Relations.Where(x => x.PrimaryKeyTable == relationInfo.PrimaryKeyTable
            && x.ForeignKeyTable == relationInfo.ForeignKeyTable
            ).Count() > 1)
            {
                propertyName = propertyName + "_" + ConvertName(relationInfo.ForeignKeyColumn);
            }
            string fieldDataType = "List";
            string isMulti = "true";
            string multiTableName = GetClassByTableName(relationInfo.ForeignKeyTable);
            if (multiTableName == "")
            {
                return null;
            }

            string associationName = multiTableName + GetClassByTableName(relationInfo.PrimaryKeyTable);
            string otherKey = ConvertName(relationInfo.ForeignKeyColumn);
            string thisKey = ConvertName(relationInfo.PrimaryKeyColumn);
            var element = new XElement("field", "");
            element.Add(new XAttribute("Id", Guid.NewGuid()));
            element.Add(new XAttribute("FieldName", propertyName));
            element.Add(new XAttribute("ObjectTableName", GetClassByTableName(relationInfo.PrimaryKeyTable)));
            element.Add(new XAttribute("FieldsDataType", fieldDataType));
            //element.Add(new XAttribute("DataTypeCode" , fieldDataType));
            element.Add(new XAttribute("MaxLength", "0"));
            element.Add(new XAttribute("IsRequiered", "false"));
            element.Add(new XAttribute("IsCustom", "false"));
            element.Add(new XAttribute("MinLength", "0"));
            element.Add(new XAttribute("DisplayOnLookUp", "false"));
            element.Add(new XAttribute("CanFilter", "false"));
            element.Add(new XAttribute("DisplayOnly", "false"));
            element.Add(new XAttribute("SystemRequired", "false"));
            element.Add(new XAttribute("SystemMaxLength", "0"));
            element.Add(new XAttribute("DisplayInList", "true"));
            element.Add(new XAttribute("IsCustomFilter", "false"));
            element.Add(new XAttribute("Operator", " & quot;StartsWith&quot;"));
            element.Add(new XAttribute("MultiLine", isMulti));
            element.Add(new XAttribute("IsTimeFrameFilter", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowList", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowFilters", "false"));
            element.Add(new XAttribute("PMPropertyPath", propertyName));
            element.Add(new XAttribute("ListPropertyPath", propertyName));
            element.Add(new XAttribute("DisplayInLookUpIndex", "0"));
            element.Add(new XAttribute("AutomaticField", "false"));
            element.Add(new XAttribute("UniqueField", "false"));
            element.Add(new XAttribute("DisplayInSearchWindowListIndex", "0"));
            element.Add(new XAttribute("DisplayInSearchWindowFiltersIndex", "0"));
            element.Add(new XAttribute("IsMulti", isMulti));
            element.Add(new XAttribute("MultiTableName", multiTableName));
            element.Add(new XAttribute("DependencyFilter1IsList", "false"));
            element.Add(new XAttribute("DependencyFilter2IsList", "false"));
            element.Add(new XAttribute("IsRestrictable", "false"));
            element.Add(new XAttribute("DisplayInEntityVariables", "true"));
            element.Add(new XAttribute("DigitsAfterPoint", "0"));
            element.Add(new XAttribute("InActive", "false"));
            element.Add(new XAttribute("DisplayLongName", "false"));
            element.Add(new XAttribute("IsNullable", "false"));
            element.Add(new XAttribute("IsPrimaryKey", "false"));
            element.Add(new XAttribute("NumberOfDigits", "0"));
            element.Add(new XAttribute("IsMaxLength", "false"));
            element.Add(new XAttribute("AllowedinAutomationConditions", "false"));
            element.Add(new XAttribute("AutomationEmailRecipient", "false"));
            element.Add(new XAttribute("CanAutomateSetValue", "false"));
            element.Add(new XAttribute("AllowedInAirlineMessaging", "false"));
            element.Add(new XAttribute("FullFieldLable", propertyName));
            element.Add(new XAttribute("DefaultText", propertyName));
            element.Add(new XAttribute("HelpTextCode", propertyName));
            element.Add(new XAttribute("HelpTextDefaultText", " & quot;&quot;"));
            element.Add(new XAttribute("HasDataBaseField", "false"));
            element.Add(new XAttribute("Code", propertyName));
            element.Add(new XAttribute("DependencyFilter3IsList", "false"));
            element.Add(new XAttribute("AllowedInCustomerFieldsSettings", "false"));
            element.Add(new XAttribute("DisplayInDocumentReferences", "false"));
            element.Add(new XAttribute("CopyToDW", "false"));
            element.Add(new XAttribute("HasTemplate", "false"));
            element.Add(new XAttribute("IsRequired", "false"));
            element.Add(new XAttribute("IsSpellCheckedFullFieldLable", "true"));
            element.Add(new XAttribute("IsSpellCheckedHelpLocalDefaultText", "true"));
            element.Add(new XAttribute("NoMetaDataField", "false"));
            element.Add(new XAttribute("HasPMField", "true"));
            element.Add(new XAttribute("GenerateInList", "false"));
            element.Add(new XAttribute("ThisKey", thisKey));
            element.Add(new XAttribute("OtherKey", otherKey));
            element.Add(new XAttribute("AssociationName", associationName));
            return element;
        }

        //private static XElement CreateNewField_old(ColumnInfo columnInfo) //(string table,string property)
        //{

        //    //if (keys.Contains(property))
        //    //{
        //    //    isPrimaryKey = "true";
        //    //}
        //    var propertyParts = property.Split(' ');
        //    bool virtualProperty = propertyParts.Contains("virtual");
        //    string dataType = propertyParts[1].Trim();
        //    string propertyName = propertyParts[2].Trim();


        //    if (dataType == "class") return null;
        //    if (dataType == "partial") return null;
        //    if (dataType == "static") return null;
        //    string isPrimaryKey = "false";
        //    string isForeignKey = "false";
        //    string isNullable = "false";
        //    int length = 0;
        //    string isMulti = "false";
        //    string fieldDataType = dataType;
        //    string multiTableName = "";
        //    string foreignEntity = "";
        //    string navigationPropertyName = "";
        //    if (foreignkeys.ContainsKey(propertyName))
        //    {
        //        var FKDetails = foreignkeys.FirstOrDefault(x => x.Key == propertyName).Value.Split(' ');
        //        isForeignKey = "true";
        //        if (FKDetails.Contains("virtual"))
        //        {
        //            foreignEntity = FKDetails[2].Trim();
        //            navigationPropertyName = FKDetails[3].Trim();
        //        }
        //        else
        //        {
        //            foreignEntity = FKDetails[1].Trim();
        //            navigationPropertyName = FKDetails[2].Trim();
        //        }
        //    }



        //    if (virtualProperty)
        //    {
        //        dataType = propertyParts[2].Trim();
        //        propertyName = propertyParts[3].Trim();
        //        if (propertyParts[2].Contains("List"))
        //        { 
        //            isMulti = "true";
        //            fieldDataType = "List";
        //            multiTableName = dataType.Split('<')[1].Split('>')[0];
        //        }
        //    }
        //    if (fieldDataType.EndsWith("?"))
        //    {
        //        fieldDataType = fieldDataType.Substring(0, fieldDataType.Length - 1);
        //        isNullable = "true";
        //    }
        //    if (DataTypes.ContainsKey(fieldDataType))
        //    {
        //        fieldDataType = DataTypes[fieldDataType];
        //    }
        //    else
        //    {
        //        if (!MissingDataTypes.Contains(fieldDataType))
        //        {
        //            MissingDataTypes.Add(fieldDataType);
        //        }
        //    }





        //    //var columnInfo = Columns.FirstOrDefault(x => x.ColumnName.ToUpper() == propertyName.ToUpper() && x.TableName.ToUpper() == table.ToUpper());
        //    if (columnInfo == null)
        //    {
        //        if (columnInfo.IsPrimary)
        //        {
        //            isPrimaryKey = "true";
        //        }

        //        if (columnInfo.IsNullable)
        //        {
        //            isNullable = "true";
        //        }
        //        if (columnInfo.MaxLength != null)
        //        {
        //            length = columnInfo.MaxLength.Value;
        //        }
        //        fieldDataType = columnInfo.DataType;
        //    }

        //    var element =  new XElement("field", "");
        //    element.Add(new XAttribute("Id", Guid.NewGuid()));
        //    element.Add(new XAttribute("FieldName" , propertyName));
        //    element.Add(new XAttribute("ObjectTableName" , table));
        //    element.Add(new XAttribute("FieldsDataType" , fieldDataType));
        //    //element.Add(new XAttribute("DataTypeCode" , fieldDataType));
        //    element.Add(new XAttribute("MaxLength" , length));
        //    element.Add(new XAttribute("IsRequiered" , "true"));
        //    element.Add(new XAttribute("IsCustom" , "false"));
        //    element.Add(new XAttribute("MinLength" , "0"));
        //    element.Add(new XAttribute("DisplayOnLookUp" , "false"));
        //    element.Add(new XAttribute("CanFilter" , "false"));
        //    element.Add(new XAttribute("DisplayOnly" , "false"));
        //    element.Add(new XAttribute("SystemRequired" , "false"));
        //    element.Add(new XAttribute("SystemMaxLength" , "0"));
        //    element.Add(new XAttribute("DisplayInList" , "true"));
        //    element.Add(new XAttribute("IsCustomFilter" , "false"));
        //    element.Add(new XAttribute("Operator" , " & quot;StartsWith&quot;"));
        //    element.Add(new XAttribute("MultiLine" , isMulti));
        //    element.Add(new XAttribute("IsTimeFrameFilter" , "false"));
        //    element.Add(new XAttribute("DisplayInSearchWindowList" , "false"));
        //    element.Add(new XAttribute("DisplayInSearchWindowFilters" , "false"));
        //    element.Add(new XAttribute("PMPropertyPath" , propertyName));
        //    element.Add(new XAttribute("ListPropertyPath" , propertyName));
        //    element.Add(new XAttribute("DisplayInLookUpIndex" , "0"));
        //    element.Add(new XAttribute("AutomaticField" , "false"));
        //    element.Add(new XAttribute("UniqueField" , "false"));
        //    element.Add(new XAttribute("DisplayInSearchWindowListIndex" , "0"));
        //    element.Add(new XAttribute("DisplayInSearchWindowFiltersIndex" , "0"));
        //    element.Add(new XAttribute("IsMulti" , isMulti));

        //    if (isMulti == "true")
        //    {
        //        element.Add(new XAttribute("MultiTableName", multiTableName));
        //        //element.Add(new XAttribute("MultiTableName", multiTableName));
        //    }


        //    element.Add(new XAttribute("DependencyFilter1IsList" , "false"));
        //    element.Add(new XAttribute("DependencyFilter2IsList" , "false"));
        //    element.Add(new XAttribute("IsRestrictable" , "false"));
        //    element.Add(new XAttribute("DisplayInEntityVariables" , "true"));
        //    element.Add(new XAttribute("DigitsAfterPoint" , "0"));
        //    element.Add(new XAttribute("InActive" , "false"));
        //    element.Add(new XAttribute("DisplayLongName" , "false"));
        //    if (isForeignKey == "true")
        //    {
        //        isNullable = "false";
        //        element.Add(new XAttribute("IsForeignKey" , isForeignKey));
        //        element.Add(new XAttribute("ForeignEntity" , foreignEntity));
        //        element.Add(new XAttribute("DontBuildRelationOnDB" , "false"));
        //        element.Add(new XAttribute("NavigationPropertyName" , navigationPropertyName));
        //    }
        //    element.Add(new XAttribute("IsNullable", isNullable));
        //    element.Add(new XAttribute("IsPrimaryKey" , isPrimaryKey));
        //    element.Add(new XAttribute("NumberOfDigits" , "0"));
        //    element.Add(new XAttribute("IsMaxLength" , "false"));
        //    element.Add(new XAttribute("AllowedinAutomationConditions" , "false"));
        //    element.Add(new XAttribute("AutomationEmailRecipient" , "false"));
        //    element.Add(new XAttribute("CanAutomateSetValue" , "false"));
        //    element.Add(new XAttribute("AllowedInAirlineMessaging" , "false"));
        //    element.Add(new XAttribute("FullFieldLable" , propertyName));
        //    element.Add(new XAttribute("DefaultText" , propertyName));
        //    element.Add(new XAttribute("HelpTextCode" , propertyName));
        //    element.Add(new XAttribute("HelpTextDefaultText" , " & quot;&quot;"));
        //    element.Add(new XAttribute("HasDataBaseField" , "true"));
        //    element.Add(new XAttribute("Code" , propertyName));
        //    element.Add(new XAttribute("DependencyFilter3IsList" , "false"));
        //    element.Add(new XAttribute("AllowedInCustomerFieldsSettings" , "false"));
        //    element.Add(new XAttribute("DisplayInDocumentReferences" , "false"));
        //    element.Add(new XAttribute("CopyToDW" , "false"));
        //    element.Add(new XAttribute("HasTemplate" , "false"));
        //    element.Add(new XAttribute("IsRequired" , "true"));
        //    element.Add(new XAttribute("IsSpellCheckedFullFieldLable" , "true"));
        //    element.Add(new XAttribute("IsSpellCheckedHelpLocalDefaultText" , "true"));
        //    element.Add(new XAttribute("NoMetaDataField" , "false"));
        //    element.Add(new XAttribute("HasPMField" , "true"));
        //    element.Add(new XAttribute("GenerateInList" , "true"));
        //    return element;
        //}


        private static string ConvertName(string name)
        {
            if (ClassesMap.ContainsKey(name.ToUpper()))
            {
                return ClassesMap[name.ToUpper()];
            }
            if (propertiesNames.ContainsKey(name.ToUpper()))
            {
                return propertiesNames[name.ToUpper()];
            }   
            return ToCamelCase(name);
        }
        private static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var words = input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                return input;
            }

            var result = "";
            for (int i = 0; i < words.Length; i++)
            {
                result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            return result;
        }
        internal static string GetClassByTableName(string tableName)
        {
            string fulltableName = tableName;
            if (tableName.ToLower().EndsWith("ies"))
            {
                tableName = tableName.Substring(0, tableName.Length - 3) + "Y";
            }
            //if (tableName.ToLower().EndsWith("es"))
            //{
            //    tableName = tableName.Substring(0, tableName.Length - 2) ;
            //}
            if (tableName.ToLower().Last() == 's')
            {
                tableName = tableName.Substring(0, tableName.Length - 1);
            }
            string classname = ClassesMap.FirstOrDefault(x => x.Key == tableName).Value;
            if (classname == null && fulltableName != tableName)
            {
                classname = ClassesMap.FirstOrDefault(x => x.Key == fulltableName).Value;

            }
            if (classname == null)
            {
                if (!MissingClassesMap.ContainsKey(tableName))
                MissingClassesMap.Add(tableName, fulltableName);
                //return ConvertName(tableName);
                classname = "";
            }
            return classname;
        }
    }

}
