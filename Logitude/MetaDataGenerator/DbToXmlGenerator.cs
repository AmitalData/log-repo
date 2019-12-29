using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using System.Collections;
using System.IO;

namespace MetaDataGenerator
{
    public class DbToXmlGenerator
    {
        private List<ObjectField> allFields = new List<ObjectField>();
        private List<TextCode> allTextCodes = new List<TextCode>();

        private List<Query> allQueries = new List<Query>();
        private List<QueryColumn> allQueryColumns = new List<QueryColumn>();
        private List<AdvancedQueryFilter> allQueryFilters = new List<AdvancedQueryFilter>();
        private List<QueryGroup> allQueryGroups = new List<QueryGroup>();

        private List<Feature> allFeatures = new List<Feature>();
        private List<Screen> allScreens = new List<Screen>();
        private List<ScreenField> allScreenFields = new List<ScreenField>();
        private List<ObjectTableTab> allTabs = new List<ObjectTableTab>();
        private List<EventType> allEventTypes = new List<EventType>();
        private List<MenuButton> allMenuButtons = new List<MenuButton>();
        private List<MenuButtonGroup> allMenuButtonGroup = new List<MenuButtonGroup>();
        private List<EntityStatus> allEntityStatus = new List<EntityStatus>();
        public DbToXmlGenerator()
        {
            ObjectFieldRepository fieldsRep = new ObjectFieldRepository(0);
            TextCodeRepository textCodesRep = new TextCodeRepository(0);
            QueryRepository queryRep = new QueryRepository(0);
            QueryColumnRepository queryColumnRep = new QueryColumnRepository(0);
            AdvancedQueryFilterRepository advancedQueryFilterRepository = new AdvancedQueryFilterRepository(0);
            QueryGroupRepository queryGroupRep = new QueryGroupRepository(0);
            FeatureRepository featureRep = new FeatureRepository(0);
            ScreensRepository screensRep = new ScreensRepository(0);
            ScreenFieldsRepository screenFieldsRep = new ScreenFieldsRepository(0);
            ObjectTableTabRepository tabsRep = new ObjectTableTabRepository(0);
            EventTypeRepository eventTypesRep = new EventTypeRepository(0);
            MenuButtonRepository menuButtonRep = new MenuButtonRepository(0);
            MenuButtonGroupRepository menuButtonGroupRep = new MenuButtonGroupRepository(0);
            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(0);


            allFields = fieldsRep.GetObjectFieldsByTenant(0).Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable").ToList();
            allTextCodes = textCodesRep.GetTextCodesByTenant(0).ToList();



            allQueries = queryRep.GetQueriesByTenant(0).ToList();
            allQueryColumns = queryColumnRep.GetQueryColumnsByTenant(0).ToList();
            allQueryFilters = advancedQueryFilterRepository.GetAdvancedQueryFiltersByTenant(0).ToList();

            allQueryGroups = queryGroupRep.GetQueryGroups().ToList();

            allFeatures = featureRep.GetFeaturesByTenant(0).ToList();
            allScreens = screensRep.GetScreensByTenant(0).ToList();
            allScreenFields = screenFieldsRep.GetScreenFieldsByTenant(0).ToList();
            allTabs = tabsRep.GetObjectTableTabsByTenant(0).ToList();
            allEventTypes = eventTypesRep.GetEventTypesByTenant(0).ToList();
            allMenuButtons = menuButtonRep.GetMenuButtonsByTenant(0).ToList();
            allMenuButtonGroup = menuButtonGroupRep.GetMenuButtonGroupsByTenant(0).ToList();

            allEntityStatus = entityStatusRepository.GetEntityStatusByTenant(0).ToList();
        }



		public DbToXmlGenerator(ObjectTable table)
		{
			ObjectFieldRepository fieldsRep = new ObjectFieldRepository(0);
			TextCodeRepository textCodesRep = new TextCodeRepository(0);
			QueryRepository queryRep = new QueryRepository(0);
			QueryColumnRepository queryColumnRep = new QueryColumnRepository(0);
			AdvancedQueryFilterRepository advancedQueryFilterRepository = new AdvancedQueryFilterRepository(0);
			QueryGroupRepository queryGroupRep = new QueryGroupRepository(0);
			FeatureRepository featureRep = new FeatureRepository(0);
			ScreensRepository screensRep = new ScreensRepository(0);
			ScreenFieldsRepository screenFieldsRep = new ScreenFieldsRepository(0);
			ObjectTableTabRepository tabsRep = new ObjectTableTabRepository(0);
			EventTypeRepository eventTypesRep = new EventTypeRepository(0);
			MenuButtonRepository menuButtonRep = new MenuButtonRepository(0);
			MenuButtonGroupRepository menuButtonGroupRep = new MenuButtonGroupRepository(0);
			EntityStatusRepository entityStatusRepository = new EntityStatusRepository(0);



			allFields = fieldsRep.GetObjectFieldsByTenant(0).Where(f => f.ObjectTableId == table.Id).Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable").ToList();
            if (table.Name != "Master")
                allTextCodes = textCodesRep.GetTextCodesByTenantAndObjectTable(0, table.Name).ToList();
            else
                allTextCodes = textCodesRep.GetTextCodesByTenant(0).ToList();



            allQueries = queryRep.GetQueriesByTenant(0).ToList();
			allQueryColumns = queryColumnRep.GetQueryColumnsByTenant(0).ToList();
			allQueryFilters = advancedQueryFilterRepository.GetAdvancedQueryFiltersByTenant(0).ToList();

			allQueryGroups = queryGroupRep.GetQueryGroups().ToList();

			allFeatures = featureRep.GetFeaturesByTenant(0).Where(f=>f.ObjectTableId == table.Id).ToList();
			allScreens = screensRep.GetScreensByTenant(0).Where(f => f.ObjectTableId == table.Id).ToList();
			allScreenFields = screenFieldsRep.GetScreenFieldsByTenant(0).ToList();
			allTabs = tabsRep.GetObjectTableTabsByTenant(0).Where(f => f.ObjectTableId == table.Id).ToList();
			allEventTypes = eventTypesRep.GetEventTypesByTenant(0).Where(f => f.ObjectTableId == table.Id).ToList();
			allMenuButtons = menuButtonRep.GetMenuButtonsByTenant(0).ToList();
			allMenuButtonGroup = menuButtonGroupRep.GetMenuButtonGroupsByTenant(0).ToList();

			allEntityStatus = entityStatusRepository.GetEntityStatusByTenant(0).Where(f => f.ObjectTableId == table.Id).ToList();
		}



        public bool FormatExistingModelEntityLXMLs(List<ObjectTable> modelTables, string directoryPath)
        {
           using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = dialog.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
				{
					 

					DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
					string[] allFiles = dirInfo.GetFiles("*.lxml").Select(f => f.Name.Replace(f.Extension, "")).ToArray();

					 

				}
			}
            return true;
        }


        public bool AppendExistingModelEntityLXMLs(List<ObjectTable> modelTables, string directoryPath)
        {
            directoryPath = directoryPath + @"\";
            foreach (ObjectTable table in modelTables)
            {
                List<ObjectField> fields = (from a in this.allFields
                                            where a.ObjectTableId == table.Id
                                            select a).ToList();

                List<TextCode> tableTextCodes = allTextCodes.Where(t => t.ObjectTableId == table.Id).ToList();

                //string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                //DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
                //string solutionDirectory = solutionDir.FullName;
                string filePath = directoryPath + table.Name + ".lxml";

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlElement entityElement = (XmlElement)doc.GetElementsByTagName("entity")[0];

                GenerateMetadataEntities(table, fields, doc, entityElement, true);

                #region Write Xml To file


                string tableName = table.Name;
                if (table.Name.Contains("."))
                {
                    tableName = table.Name.Split('.')[1];
                }
                doc.Save(directoryPath + tableName + ".lxml");

                #endregion
            }

            return true;
        }



        public bool RegenerateExisting_Old_ModelEntityLXMLs(List<ObjectTable> modelTables, string directoryPath, ref string errors)
        {
            if (string.IsNullOrEmpty(errors))
                errors = "";
            directoryPath = directoryPath + @"\";

            foreach (ObjectTable table in modelTables)
            {
                List<ObjectField> fields = (from a in this.allFields
                                            where a.ObjectTableId == table.Id
                                            select a).ToList();

                List<TextCode> tableTextCodes = allTextCodes.Where(t => t.ObjectTableId == table.Id).ToList();

                //string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                //DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
                //string solutionDirectory = solutionDir.FullName;
                string filePath = directoryPath + table.Name + ".lxml";

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);


                XmlElement entityElement = (XmlElement)doc.GetElementsByTagName("entity")[0];
                this.UpdateEntityElement(entityElement, doc, table, tableTextCodes);

                List<XmlNode> dataContractsNodesList = new List<XmlNode>();
                foreach (XmlNode node in doc.GetElementsByTagName("DataContracts"))
                {
                    dataContractsNodesList.Add(node);
                }

                this.RemoveOldNodes(doc, entityElement, "DataContracts");
                if (!this.GenerateTableLXMLFields(doc, table, entityElement, fields, true))
                {
                    errors += (table.Name + "fields not generated!" + Environment.NewLine);
                    //continue;

                }

                GenerateMetadataEntities(table, fields, doc, entityElement, false);

                foreach (XmlNode node in dataContractsNodesList)
                {
                    XmlNode newNode = doc.CreateElement("DataContracts");
                    entityElement.AppendChild(node);
                }

                #region Write Xml To file


                string tableName = table.Name;
                if (table.Name.Contains("."))
                {
                    tableName = table.Name.Split('.')[1];
                }

                XmlDocument newdoc = new XmlDocument();
                doc.Save(directoryPath + tableName + ".lxml");

                #endregion
            }

            return true;
        }

        public bool RegenerateExisting_Old_ModelEntityLXMLs_Specific(List<ObjectTable> modelTables, string directoryPath, ref string errors, string updateName)
        {
            if (string.IsNullOrEmpty(errors))
                errors = "";
            directoryPath = directoryPath + @"\";

            foreach (ObjectTable table in modelTables)
            {
                List<ObjectField> fields = (from a in this.allFields
                                            where a.ObjectTableId == table.Id
                                            select a).ToList();

                List<TextCode> tableTextCodes = allTextCodes.Where(t => t.ObjectTableId == table.Id).ToList();

                //string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                //DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
                //string solutionDirectory = solutionDir.FullName;
                string filePath = directoryPath + table.Name + ".lxml";

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);


                XmlElement entityElement = (XmlElement)doc.GetElementsByTagName("entity")[0];
                //this.UpdateEntityElement(entityElement, doc, table, tableTextCodes);

                //List<XmlNode> dataContractsNodesList = new List<XmlNode>();
                //foreach (XmlNode node in doc.GetElementsByTagName("DataContracts"))
                //{
                //    dataContractsNodesList.Add(node);
                //}

                //this.RemoveOldNodes(doc, entityElement, "DataContracts");

                // if (updateName == "closed")
                //{
                //    if (table.IsClosed)
                //        GenerateTableDataRecords(doc, entityElement, table, fields);
                //}
                //if (updateName == "menus")
                //{
                //    GenerateMenuButtons(doc, entityElement, table, fields);
                //}

                switch (updateName)
                {
                    case "closedtables":
                        {
                            if (table.IsClosed)
                                GenerateTableDataRecords(doc, entityElement, table, fields);
                            break;
                        }

                    case "menus":
                        {
                            GenerateMenuButtons(doc, entityElement, table, fields);
                            break;
                        }
                    case "tabs":
                        {
                            GenerateTabs(doc, entityElement, table, fields);
                            break;
                        }
                    case "textcodes":
                        {
                            GenerateAdditionalTextCodes(doc, entityElement, table, fields);
                            break;
                        }
					case "features":
						{
							GenerateAdditionalFeatures(doc, entityElement, table, fields);
							break;
						}
					case "features and textCodes":
						{
							GenerateAdditionalTextCodes(doc, entityElement, table, fields);
							GenerateAdditionalFeatures(doc, entityElement, table, fields);

							break;
						}
					case "entity":
                        {
                            this.UpdateEntityElement(entityElement, doc, table, tableTextCodes);
                            break;
                        }
                }

                //foreach (XmlNode node in dataContractsNodesList)
                //{
                //    XmlNode newNode = doc.CreateElement("DataContracts");
                //    entityElement.AppendChild(node);
                //}

                #region Write Xml To file


                string tableName = table.Name;
                if (table.Name.Contains("."))
                {
                    tableName = table.Name.Split('.')[1];
                }

                XmlDocument newdoc = new XmlDocument();
                doc.Save(directoryPath + tableName + ".lxml");

                #endregion
            }

            return true;
        }

        #region GenerateEntityElement


        private XmlElement UpdateEntityElement(XmlElement entityElement, XmlDocument doc, ObjectTable table, List<TextCode> tableTextCodes)
        {
          
            //SetAttribute("Id", GetStringValue(Guid.NewGuid()), entityElement, null);
            if (string.IsNullOrEmpty(this.GetAttributeValue("ObjectTableName", entityElement)))
                SetAttribute("ObjectTableName", GetStringValue(table.Name), entityElement);
            if (string.IsNullOrEmpty(this.GetAttributeValue("DBTableName", entityElement)))
                SetAttribute("DBTableName", (!string.IsNullOrEmpty(table.DBTableName) ? GetStringValue(table.DBTableName) : GetStringValue("NONE")), entityElement);

            TextCode tableTextCode = (from a in tableTextCodes
                                      where a.Tenant == 0 && a.Code == table.Name
                                      select a).FirstOrDefault();
            if (tableTextCode != null)
            {
                SetAttribute("ObjectTableSingular", GetStringValue(tableTextCode.DefaultText), entityElement);
                SetAttribute("ObjectTablePlural", GetStringValue(tableTextCode.DefaultTextPlural), entityElement);
                SetAttribute("DefaultText", GetStringValue(tableTextCode.DefaultText), entityElement);
            }
            if (table.DescriptionTextCode != null)
            {
                SetAttribute("DescriptionDefaultText", GetStringValue(table.DescriptionTextCode.DefaultText), entityElement);
                SetAttribute("DescriptionLocalDefaultText", GetStringValue(table.DescriptionTextCode.LocalDefaultText), entityElement);

            }

            if (table.NewButtonTextCode != null)
            {
                SetAttribute("NewButtonDefaultText", GetStringValue(table.NewButtonTextCode.DefaultText), entityElement);
                SetAttribute("NewButtonLocalDefaultText", GetStringValue(table.NewButtonTextCode.LocalDefaultText), entityElement);

            }

            PropertyInfo[] objectTableProperties = table.GetType().GetProperties().Where(
                f => f.Name != "ObjectTableSingular" &&
                    f.Name != "ObjectTablePlural" &&
                    f.Name != "DescriptionDefaultText" &&
                     f.Name != "DescriptionTextCodeId" &&
                     f.Name != "NewButtonTextCodeId" &&
                     f.Name != "Id" &&
                      f.Name != "Tenant" &&
                      f.Name != "ObjectTableName" &&
                      f.Name != "DBTableName"
                      && f.Name != "HeaderScreenId"

                     ).ToArray();

            foreach (var prop in objectTableProperties)
            {
                string xmlAttrValue = this.GetAttributeValue(prop.Name, entityElement);

                object value = prop.GetValue(table);
                if (value != null)
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        SetAttribute(prop.Name, value.ToString().ToLower(), entityElement);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {

                        SetAttribute(prop.Name, GetStringValue(value), entityElement);

                    }
                    else if(prop.PropertyType == typeof(bool))
                    {
                        if (xmlAttrValue != value.ToString().ToLower() && xmlAttrValue == "false")
                        {
                            SetAttribute(prop.Name, value.ToString().ToLower(), entityElement);
                        }
                    }
                }
            }

            IEnumerable<IGrouping<string, Query>> tableQueries = this.allQueries.Where(q => q.ObjectTableId == table.Id && q.QueryGroupCode != null && q.SystemLevel == true).GroupBy(q => q.QueryGroupCode);

            int? index = null;
            foreach (var item in tableQueries)
            {
                QueryGroup qg = this.allQueryGroups.Where(g => g.Code == item.Key).FirstOrDefault();
                SetAttribute("Code" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Code), entityElement, null);
                SetAttribute("Name" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Name), entityElement, null);

                if (index == null)
                {
                    index = 1;
                }
                else
                    index++;
            }

            if (table.IsClosed)
            {
                ObjectField codeField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Code").FirstOrDefault();
                if (codeField == null)
                {
                    codeField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Id").FirstOrDefault();
                }

                ObjectField nameField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Name" || q.FieldName == "EnlglishName").FirstOrDefault();
                if (codeField != null)
                    SetAttribute("CloseTableCode", GetStringValue(codeField.FieldName), entityElement, null);

                if (nameField != null)
                    SetAttribute("CloseTableName", GetStringValue(nameField.FieldName), entityElement, null);
            }



            return entityElement;

        }

        public string GetAttributeValue(string attName, XmlElement entityElement)
        {

            XmlAttribute att = entityElement.Attributes[attName];
            string result = null;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    if (att.Value != null)
                    {
                        result = att.Value.Trim('"').ToLower();
                    }
                }
            }

            return result;
        }

        #endregion

        public bool GenerateNewEntityLXML(ObjectTable table)
        {
            List<ObjectField> fields = (from a in this.allFields
                                        where a.ObjectTableId == table.Id
                                        select a).ToList();

            List<TextCode> tableTextCodes = allTextCodes.Where(t => t.ObjectTableId == table.Id).ToList();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);
            XmlElement entityElement = GenerateEntityElement(doc, table, tableTextCodes);



            if (!this.GenerateTableLXMLFields(doc, table, entityElement, fields))
            {
                return false;
            }

            GenerateMetadataEntities(table, fields, doc, entityElement, false);

            #region Write Xml To file


            string tableName = table.Name;
            if (table.Name.Contains("."))
            {
                tableName = table.Name.Split('.')[1];
            }
            doc.Save("../../GeneratedFiles/New/" + tableName + ".lxml");

            #endregion

            return true;
        }

        private bool GenerateTableLXMLFields(XmlDocument doc, ObjectTable table, XmlElement entityElement, List<ObjectField> fields, bool updateLXML = false)
        {
			string tableName = table.Name;
			//if (table.Name.ToLower() == "master")
			//	tableName = "Shipment";

			string modelName = "CommonDataModel";
            string qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + tableName);

            System.Type tableClass = System.Type.GetType(qName);

            if (tableClass == null)
            {
                modelName = "ShipmentsModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + tableName);
                tableClass = System.Type.GetType(qName);
            }

            if (tableClass == null)
            {
                modelName = "QuoteModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + tableName);
                tableClass = System.Type.GetType(qName);
            }


            if (tableClass == null)
            {
                modelName = "InfrastructureModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + tableName);
                tableClass = System.Type.GetType(qName);
            }


            if (tableClass == null)
            {
                modelName = "GlobalModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data.Global", "Simplog.Global.Data.GlobalModel.EntityPOCOs." + tableName);
                tableClass = System.Type.GetType(qName);

                //Simplog.Global.Data.GlobalModel.EntityPOCOs
            }

            if (tableClass == null)
            {
                modelName = "InvoiceModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + tableName);
                tableClass = System.Type.GetType(qName);
            }

            string qPMName = Assembly.CreateQualifiedName("Logitude.BL", "Logitude.BL." + modelName + ".EntityPMs." + tableName + "PM");
            System.Type tablePMClass = System.Type.GetType(qPMName);

            string qListName = Assembly.CreateQualifiedName("Logitude.BL", "Logitude.BL." + modelName + ".EntityLists." + tableName + "List");
            System.Type tableListClass = System.Type.GetType(qListName);


            if (tableClass == null && tableName != "General" && tableName != "Master")// && tablePMClass == null)
            {
                return false;
            }

            PropertyInfo[] pocoProperties = { };
            PropertyInfo[] pmClassProperties = { };
            PropertyInfo[] listClassProperties = { };
            if (tableClass != null)
            {
                pocoProperties = tableClass.GetProperties();
            }
            if (tablePMClass != null)
            {
                pmClassProperties = tablePMClass.GetProperties();
            }
            if (tableListClass != null)
            {
                listClassProperties = tableListClass.GetProperties();
            }


            //RemoveOldNodes(doc, entityElement, "field");

            List<string> addeddFields = new List<string>();
            

            GeneratePocoFields(doc, entityElement, fields, table, pocoProperties, pmClassProperties, listClassProperties, addeddFields);
            GenerateObjectFieldFields(doc, entityElement, fields, pocoProperties, pmClassProperties, listClassProperties, addeddFields);
            GeneratePMOnlyFields(doc, entityElement, fields, table, pmClassProperties, pocoProperties, addeddFields);
            GenerateListOnlyFields(doc, entityElement, fields, table, listClassProperties, pocoProperties, addeddFields);

            return true;
        }

        private XmlElement GetElementNodeByTagAndAttributeName(XmlDocument doc, XmlElement entityElement, string tagName,string attrName,string attrValue,bool addIfNotExists = false)
        {
            XmlNodeList fieldsNodeList = doc.GetElementsByTagName(tagName);
            XmlElement resultnode = null;

            foreach (XmlNode node in fieldsNodeList)
            {
                if (node.Attributes[attrName].Value.Trim('"') == attrValue.Trim('"'))
                {

                    resultnode = (XmlElement)node;
                    break;
                }
            }

            if (resultnode == null && addIfNotExists)
            {
                resultnode = doc.CreateElement(tagName);
                entityElement.AppendChild(resultnode);
            }

            return resultnode;
        }

        private void GenerateMetadataEntities(ObjectTable table, List<ObjectField> fields, XmlDocument doc, XmlElement entityElement, bool updateExistingLXML = false)
        {
            GenerateQueries(doc, entityElement, table, fields, updateExistingLXML);
            GenerateScreens(doc, entityElement, table, fields);
            GenerateTabs(doc, entityElement, table, fields);
            GenerateMenuButtons(doc, entityElement, table, fields);
            GenerateEvents(doc, entityElement, table, fields);
            if (table.IsClosed)
                GenerateTableDataRecords(doc, entityElement, table, fields);

            GenerateAdditionalTextCodes(doc, entityElement, table, fields);
            GenerateAdditionalFeatures(doc, entityElement, table, fields);
        }

        private void GenerateAdditionalTextCodes(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
            //            select* from textcodes where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') 
            //and textcodetypecode<> 'f' and TextCodeTypeCode<> 'TH'
            //and Code not like '%.MenuButtons.%'
            //and Code not like '%.Features.%'
            //and Id not in (select LabelTextCodeId from MenuButtons where LabelTextCodeId is not null and Tenant = 0 and MenuButtonGroupId in (select Id from MenuButtonGroups where  ObjectTableId in (select id from objecttables where name = 'shipment'))
            //)
            //and(select COUNT(*)   from ObjectFields where Tenant = 0 and ObjectTableId in (select id from objecttables where name = 'shipment')
            //and(ObjectFields.FullNameTextCodeId = textcodes.Id or  textcodes.id = ObjectFields.ListTextCodeId  or textcodes.id = ObjectFields.HelpTextCodeId)) = 0
            //and Id not in (select NameTextCodeId from features where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') and NameTextCodeId is not null)
            //and Id not in (select NameTextCodeId from Queries where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') and NameTextCodeId is not null)
            XmlElement additionalTextCodesListXElement = (XmlElement)doc.GetElementsByTagName("AdditionalTextCodes")[0];
            if (additionalTextCodesListXElement == null)
            {
                additionalTextCodesListXElement = doc.CreateElement("AdditionalTextCodes");
                entityElement.AppendChild(additionalTextCodesListXElement);

            }


            RemoveOldNodes(doc, additionalTextCodesListXElement, "TextCode");
           // XmlElement 

            List<TextCode> additionalTextCodes = (from a in allTextCodes
                                                  where a.ObjectTableId == table.Id && a.Tenant == 0
                                                  && a.Id != table.DescriptionTextCodeId
                                                  && a.Id != table.NewButtonTextCodeId
                                                  && a.TextCodeTypeCode.ToLower() != "f" && a.TextCodeTypeCode.ToLower() != "th"
                                                  && a.TextCodeTypeCode.ToLower() != "h"
                                                  && a.TextCodeTypeCode.ToLower() != "t"
                                                  && a.TextCodeTypeCode.ToLower() != "ch"
                                                  && a.TextCodeTypeCode.ToLower() != "q"
                                                  && !a.Code.Contains(".MenuButtons.")
                                                  && !a.Code.Contains(".Features.")
                                                  && !allFeatures.Any(f => f.NameTextCodeId == a.Id)
                                                  && !allQueries.Any(f => f.NameTextCodeId == a.Id)
                                                  && !allMenuButtons.Any(f => f.LabelTextCodeId == a.Id)
                                                  && !tableObjectFields.Any(f => f.FullNameTextCodeId == a.Id || f.ListTextCodeId == a.Id || f.HelpTextCodeId == a.Id)
                                                  select a).ToList();

            foreach (TextCode tcode in additionalTextCodes)
            {
                XmlElement codeXElement = GetElementNodeByTagAndAttributeName(doc, additionalTextCodesListXElement, "TextCode", "Code", tcode.Code, true);//doc.CreateElement("TextCode");
                additionalTextCodesListXElement.AppendChild(codeXElement);

                SetAttribute("Code", GetStringValue(tcode.Code), codeXElement);
                SetAttribute("DefaultText", GetStringValue(tcode.DefaultText), codeXElement);
                SetAttribute("LocalDefaultText", GetStringValue(tcode.LocalDefaultText), codeXElement);
                SetAttribute("TextCodeTypeCode", GetStringValue(tcode.TextCodeTypeCode), codeXElement);
                SetAttribute("IsSpellChecked", tcode.IsSpellChecked.ToString().ToLower(), codeXElement);

            }
        }

        private void GenerateAdditionalFeatures(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
			//            select* from Features where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') 
			//and Code<> 'NEW'and Code<> 'UPDATE'and Code<> 'READ'and Code<> 'Module'
			//and Id not in (select FeatureId from MenuButtons where FeatureId is not null and Tenant = 0 and MenuButtonGroupId in (select Id from MenuButtonGroups where  ObjectTableId in (select id from objecttables where name = 'shipment'))
			//)
			//and Id not in (select FeatureId from ObjectTableTabs where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') and NameTextCodeId is not null)
			//and Id not in (select FeatureId from Queries where Tenant = 0 and objecttableid in (select id from objecttables where name = 'shipment') and NameTextCodeId is not null)

			XmlElement additionalFeaturesListXElement = (XmlElement)doc.GetElementsByTagName("AdditionalFeatures")[0];
			if (additionalFeaturesListXElement == null)
			{
				additionalFeaturesListXElement = doc.CreateElement("AdditionalFeatures");
				entityElement.AppendChild(additionalFeaturesListXElement);

			}


			RemoveOldNodes(doc, additionalFeaturesListXElement, "Feature");

			//RemoveOldNodes(doc, entityElement, "AdditionalFeatures");
   //         XmlElement additionalTextCodesListXElement = doc.CreateElement("AdditionalFeatures");
   //         entityElement.AppendChild(additionalTextCodesListXElement);

            List<Feature> additionalFeatures = (from a in allFeatures
                                                where a.ObjectTableId == table.Id && a.Tenant == 0
                                                && a.Code.ToUpper() != "NEW" && a.Code.ToUpper() != "UPDATE" && a.Code.ToUpper() != "READ"
                                                && a.Code.ToUpper() != "MODULE"
                                                && !allTabs.Any(f => f.FeatureId == a.Id)
                                                && !allQueries.Any(f => f.FeatureId == a.Id)
                                                && !allMenuButtons.Any(f => f.FeatureId == a.Id)

                                                select a).ToList();

            foreach (Feature feature in additionalFeatures)
            {

                XmlElement featureXElement = doc.CreateElement("Feature");
				additionalFeaturesListXElement.AppendChild(featureXElement);

                SetAttribute("Code", GetStringValue(feature.Code), featureXElement);
                SetAttribute("FeatureTypeCode", GetStringValue(feature.FeatureTypeCode), featureXElement);
                SetAttribute("IsPackagable", feature.Packagable.ToString().ToLower(), featureXElement);
                SetAttribute("IsBusinessUnitEnabled", feature.IsBusinessUnitEnabled.ToString().ToLower(), featureXElement);
                SetAttribute("IsOld", feature.IsOld.ToString().ToLower(), featureXElement);
                SetAttribute("IsCoreFeature", feature.IsCoreFeature.ToString().ToLower(), featureXElement);

                if (!string.IsNullOrEmpty(feature.NameTextCodeId))
                {
                    TextCode featureTextCode = allTextCodes.FirstOrDefault(t => t.Id == feature.NameTextCodeId);
                    SetAttribute("FeatureTextCodeCode", GetStringValue(featureTextCode.Code), featureXElement);
                    SetAttribute("FeatureDefaultText", GetStringValue(featureTextCode.DefaultText), featureXElement);
                }
            }
        }

        private EntityPropertiesInfo GetEntityClassProperities(ObjectTable table)
        {

            EntityPropertiesInfo entityPropertiesInfo = new EntityPropertiesInfo();

            List<ObjectField> fields = (from a in this.allFields
                                        where a.ObjectTableId == table.Id
                                        select a).ToList();




            string modelName = "CommonDataModel";
            string qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + table.Name);

            System.Type tableClass = System.Type.GetType(qName);

            if (tableClass == null)
            {
                modelName = "ShipmentsModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + table.Name);
                tableClass = System.Type.GetType(qName);
            }

            if (tableClass == null)
            {
                modelName = "QuoteModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + table.Name);
                tableClass = System.Type.GetType(qName);
            }


            if (tableClass == null)
            {
                modelName = "InfrastructureModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + table.Name);
                tableClass = System.Type.GetType(qName);
            }


            if (tableClass == null)
            {
                modelName = "GlobalModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data.Global", "Simplog.Global.Data.GlobalModel.EntityPOCOs." + table.Name);
                tableClass = System.Type.GetType(qName);

                //Simplog.Global.Data.GlobalModel.EntityPOCOs
            }

            if (tableClass == null)
            {
                modelName = "InvoiceModel";
                qName = Assembly.CreateQualifiedName("Simplog.Data", "Simplog.Data." + modelName + ".EntityPOCOs." + table.Name);
                tableClass = System.Type.GetType(qName);
            }

            string qPMName = Assembly.CreateQualifiedName("Logitude.BL", "Logitude.BL." + modelName + ".EntityPMs." + table.Name + "PM");
            System.Type tablePMClass = System.Type.GetType(qPMName);

            string qListName = Assembly.CreateQualifiedName("Logitude.BL", "Logitude.BL." + modelName + ".EntityLists." + table.Name + "List");
            System.Type tableListClass = System.Type.GetType(qListName);


            if (tableClass == null)
            {
                modelName = "Logitude.Accounting";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }

            if (tableClass == null)
            {
                modelName = "Logitude.CRM";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }
            if (tableClass == null)
            {
                modelName = "Logitude.BookingLib";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }
            if (tableClass == null)
            {
                modelName = "Logitude.Social";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }
            if (tableClass == null)
            {
                modelName = "Logitude.TimeManagement";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }
            if (tableClass == null)
            {
                modelName = "Logitude.Customs";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }
            if (tableClass == null)
            {
                modelName = "Logitude.WarehouseLib";
                GetModelClassTypes(table, modelName, out qName, out tableClass, out qPMName, out tablePMClass, out qListName, out tableListClass);
            }


            if (tableClass != null)// && tablePMClass == null)
            {


                PropertyInfo[] pocoProperties = tableClass.GetProperties();

                entityPropertiesInfo.POCOClassProperites = pocoProperties.ToList();

                PropertyInfo[] pmClassProperties = { };
                PropertyInfo[] listClassProperties = { };
                if (tablePMClass != null)
                {
                    pmClassProperties = tablePMClass.GetProperties();
                    entityPropertiesInfo.PMClassProperties = pmClassProperties.ToList();
                }


                if (tableListClass != null)
                {
                    listClassProperties = tableListClass.GetProperties();
                    entityPropertiesInfo.ListClassProperties = listClassProperties.ToList();
                }
            }
            else
                throw new Exception("Couldn't find the POCO class for " + table.Name);

            return entityPropertiesInfo;
        }

        private static void GetModelClassTypes(ObjectTable table, string modelName, out string qName, out Type tableClass, out string qPMName, out Type tablePMClass, out string qListName, out Type tableListClass)
        {
            qName = Assembly.CreateQualifiedName(modelName + ".Data", modelName + ".Data" + ".EntityPOCOs." + table.Name);
            tableClass = System.Type.GetType(qName);

            qPMName = Assembly.CreateQualifiedName(modelName + ".BL", modelName + ".BL" + ".EntityPMs." + table.Name + "PM");
            tablePMClass = System.Type.GetType(qPMName);

            //Logitude.Accounting.Data.EntityLists
            qListName = Assembly.CreateQualifiedName(modelName + ".Data", modelName + ".Data" + ".EntityLists." + table.Name + "List");
            tableListClass = System.Type.GetType(qListName);
        }

        private void GenerateTableDataRecords(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> fields)
        {
            EntityPropertiesInfo classPropInfo = this.GetEntityClassProperities(table);


            XmlElement recordsListXElement = (XmlElement)doc.GetElementsByTagName("Records")[0];
            if (recordsListXElement != null)
            {
                RemoveOldNodes(doc, recordsListXElement, "Record");
            }
            else
            {
                recordsListXElement = doc.CreateElement("Records");
                entityElement.AppendChild(recordsListXElement);
            }

            //RemoveOldNodes(doc, entityElement, "Records");
            //XmlElement recordsListXElement = doc.CreateElement("Records");
            //entityElement.AppendChild(recordsListXElement);

            IQueryable querableEntities = (IQueryable)TableQueryReflector.GetTableListData(table.Name);
            if (querableEntities == null)
                return;

            int addedRecords = 0;
            IEnumerator dataList = querableEntities.GetEnumerator();
            if (dataList != null)
            {
                while (dataList.MoveNext())
                {
                    object entity = dataList.Current;
                    XmlElement eventXElement = doc.CreateElement("Record");
                    recordsListXElement.AppendChild(eventXElement);
                    List<ObjectField> tableFields = this.allFields.Where(q => q.ObjectTableId == table.Id && classPropInfo.POCOClassProperites.Any(p => p.Name == q.FieldName)).OrderBy(f => f.DisplayInLookUpIndex).ToList();//&& q.FieldName != "SearchFields"
                    foreach (var field in tableFields)
                    {
                        PropertyInfo propInfo = entity.GetType().GetProperty(field.FieldName);
                        if (propInfo != null)
                        {
                            object propValue = propInfo.GetValue(entity);
                            if (propValue != null)
                            {
                                if (field.DataTypeCode.ToLower() == "text" || field.DataTypeCode.ToLower() == "ntext")
                                {
                                    SetAttribute(field.FieldName, GetStringValue(propValue.ToString()), eventXElement);
                                }
                                else if (field.DataTypeCode.ToLower() == "boolean")
                                {
                                    SetAttribute(field.FieldName, propValue.ToString().ToLower(), eventXElement);
                                }
                                else
                                {
                                    SetAttribute(field.FieldName, propValue.ToString(), eventXElement);
                                }

                            }
                        }
                    }

                    addedRecords++;
                }
            }

            if(addedRecords == 0)
            {
                Console.WriteLine("no record found for table: " + table.Name);
            }
        }

        private void GenerateQueries(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields, bool updateExistingLXML = false)
        {

            RemoveOldNodes(doc, entityElement, "Query");
            List<Query> tableQueries = this.allQueries.Where(q => q.ObjectTableId == table.Id && q.SystemLevel).OrderBy(q => q.IndexOrder).ToList();
            foreach (Query q in tableQueries)
            {
                List<QueryColumn> qQueryColumns = allQueryColumns.Where(c => c.QueryId == q.Id).OrderBy(c => c.IndexOrder).ToList();
                List<AdvancedQueryFilter> qFilters = allQueryFilters.Where(c => c.QueryId == q.Id).OrderBy(c => c.IndexOrder).ToList();
                //if (qQueryColumns.Count == 0)
                //    continue;

                Feature qFeature = allFeatures.FirstOrDefault(f => f.Id == q.FeatureId);
                TextCode qTextCode = allTextCodes.FirstOrDefault(t => t.Id == q.NameTextCodeId);


                XmlElement queryXElement = doc.CreateElement("Query");
                entityElement.AppendChild(queryXElement);

                SetAttribute("Code", GetStringValue(q.Code), queryXElement);
                SetAttribute("ObjectTableName", GetStringValue(table.Name), queryXElement);
                SetAttribute("DefaultSortDirection", GetStringValue(q.DefaultSortDirection), queryXElement);
                SetAttribute("DefaultSortName", GetStringValue(q.DefaultSortColumn), queryXElement);
                SetAttribute("IsAddNewEntity", q.IsAddNewEntityEnabled.ToString().ToLower(), queryXElement);
                SetAttribute("SystemLevel", q.SystemLevel.ToString().ToLower(), queryXElement);
                SetAttribute("QuerySection", GetStringValue(q.QuerySection), queryXElement);
                SetAttribute("IndexOrder", q.IndexOrder.ToString().ToLower(), queryXElement);

                if (!string.IsNullOrEmpty(q.QueryGroupCode))
                {
                    QueryGroup qg = this.allQueryGroups.Where(g => g.Code == q.QueryGroupCode).FirstOrDefault();
                    SetAttribute("QueryGroupCode", GetStringValue(q.QueryGroupCode), queryXElement);
                    SetAttribute("QueryGroupName", GetStringValue(qg.Name), queryXElement);
                }

                if (qTextCode != null)
                {
                    SetAttribute("LocalTextCode", GetStringValue(qTextCode.LocalDefaultText), queryXElement);
                    SetAttribute("TextCode", GetStringValue(qTextCode.DefaultText), queryXElement);
                    SetAttribute("TextCodeCode", GetStringValue(qTextCode.Code), queryXElement);
                    SetAttribute("IsSpellChecked", qTextCode.IsSpellChecked.ToString().ToLower(), queryXElement);
                }
                else
                {
                    SetAttribute("LocalTextCode", GetStringValue(q.Code), queryXElement);
                    SetAttribute("TextCode", GetStringValue(q.Code), queryXElement);

                }
                if (qFeature != null)
                {
                    SetAttribute("FeatureCode", GetStringValue(qFeature.Code), queryXElement);
                    SetAttribute("IsPackagable", qFeature.Packagable.ToString().ToLower(), queryXElement);

                    if (!string.IsNullOrEmpty(qFeature.NameTextCodeId))
                    {
                        TextCode featureTextCode = allTextCodes.FirstOrDefault(t => t.Id == qFeature.NameTextCodeId);
                        SetAttribute("FeatureTextCodeCode", GetStringValue(featureTextCode.Code), queryXElement);
                        SetAttribute("FeatureDefaultText", GetStringValue(featureTextCode.DefaultText), queryXElement);
                    }
                }
                else
                    SetAttribute("IsPackagable", "false", queryXElement);

                if (!string.IsNullOrEmpty(q.SpotlightDataTemplate))
                {
                    SetAttribute("SpotlightDataTemplate", GetStringValue(q.SpotlightDataTemplate), queryXElement);
                }

                if (!string.IsNullOrEmpty(q.EditWizardName))
                {
                    SetAttribute("EditWizardName", GetStringValue(q.EditWizardName), queryXElement);
                }

                if (!string.IsNullOrEmpty(q.EditWizardComponentPath))
                {
                    SetAttribute("EditWizardComponentPath", GetStringValue(q.EditWizardComponentPath), queryXElement);
                }

                XmlElement colListXElement = doc.CreateElement("QueryColumns");
                queryXElement.AppendChild(colListXElement);


                foreach (var qc in qQueryColumns)
                {
                    ObjectField qcField = tableObjectFields.FirstOrDefault(f => f.Id == qc.ObjectFieldId);
                    XmlElement colXElement = doc.CreateElement("QueryColumn");
                    colListXElement.AppendChild(colXElement);

                    SetAttribute("IndexOrder", qc.IndexOrder.ToString(), colXElement);
                    SetAttribute("ColumnWidth", qc.ColumnWidth.ToString(), colXElement);
                    SetAttribute("ObjectFieldName", GetStringValue(qcField.FieldName), colXElement);
                    SetAttribute("QueryCode", GetStringValue(q.Code), colXElement);
                }

                XmlElement filtersListXElement = doc.CreateElement("QueryFilters");
                queryXElement.AppendChild(filtersListXElement);


                foreach (var qf in qFilters)
                {
                    ObjectField qcField = tableObjectFields.FirstOrDefault(f => f.Id == qf.ObjectFieldId);
                    XmlElement colXElement = doc.CreateElement("QueryFilter");
                    filtersListXElement.AppendChild(colXElement);

                    SetAttribute("QueryCode", GetStringValue(q.Code), colXElement);
                    SetAttribute("IndexOrder", qf.IndexOrder.ToString(), colXElement);
                    SetAttribute("IsPredefined", qf.IsPredefined.ToString().ToLower(), colXElement);
                    SetAttribute("ObjectFieldName", GetStringValue(qcField.FieldName), colXElement);
                    SetAttribute("PredefinedValue", GetStringValue(qf.PredefinedValue), colXElement);
                    SetAttribute("PredefinedValue2", GetStringValue(qf.PredefinedValue2), colXElement);
                    SetAttribute("Operator", GetStringValue(qf.Operator), colXElement);

                }

            }

            if (updateExistingLXML)
            {
                IEnumerable<IGrouping<string, Query>> tableQueriesGroups = this.allQueries.Where(q => q.ObjectTableId == table.Id && q.QueryGroupCode != null && q.SystemLevel == true).GroupBy(q => q.QueryGroupCode);

                int? index = null;
                foreach (var item in tableQueriesGroups)
                {
                    QueryGroup qg = this.allQueryGroups.Where(g => g.Code == item.Key).FirstOrDefault();
                    SetAttribute("Code" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Code), entityElement, null);
                    SetAttribute("Name" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Name), entityElement, null);

                    if (index == null)
                    {
                        index = 1;
                    }
                    else
                        index++;
                }
            }

            //featureid and nametextcode
        }

        private void GenerateScreens(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
            RemoveOldNodes(doc, entityElement, "Screen");
            List<Screen> tableScreens = this.allScreens.Where(q => q.ObjectTableId == table.Id).ToList();
            foreach (Screen sc in tableScreens)
            {
                List<ScreenField> scFields = allScreenFields.Where(f => f.ScreenId == sc.Id).OrderBy(f => f.Column).ThenBy(f => f.Row).ToList();
                if (scFields.Count == 0)
                    continue;

                var duplicates = from arc in scFields
                                 group arc by new { arc.Row, arc.Column }
                                 into g
                                 where g.Count() > 1
                                 orderby g.Key.Row, g.Key.Column
                                 select g;

                if (duplicates.Count() == 0)
                {

                    XmlElement screenXElement = doc.CreateElement("Screen");
                    entityElement.AppendChild(screenXElement);

                    SetAttribute("Code", GetStringValue(sc.Code), screenXElement);
                    SetAttribute("Name", GetStringValue(sc.Name), screenXElement);
                    SetAttribute("ObjectTableName", GetStringValue(table.Name), screenXElement);
                    SetAttribute("IsHeaderScreen", (sc.Name.Contains("HeaderScreen") || sc.Code.Contains("HeaderScreen")) ? "true" : "false", screenXElement);
                    SetAttribute("IsReadOnly", sc.IsReadOnly.ToString().ToLower(), screenXElement);

                    int numberOfRows = sc.NumberOfRows;
                    int numberOfColumns = sc.NumberOfColumns;

                    XmlElement fieldsListXElement = doc.CreateElement("ScreenFields");
                    screenXElement.AppendChild(fieldsListXElement);

                    foreach (var sfield in scFields)
                    {
                        ObjectField qcField = tableObjectFields.FirstOrDefault(f => f.Id == sfield.ObjectFieldId);
                        if (qcField == null)
                        {
                            ObjectField wrongField = allFields.FirstOrDefault(f => f.Id == sfield.ObjectFieldId);
                            if (wrongField != null)
                            {
                                qcField = tableObjectFields.FirstOrDefault(f => f.FieldName == wrongField.FieldName);

                                qcField = qcField ?? wrongField;
                            }
                        }
                        XmlElement fieldXElement = doc.CreateElement("ScreenField");
                        fieldsListXElement.AppendChild(fieldXElement);

                        SetAttribute("ObjectFieldName", GetStringValue(qcField.FieldName), fieldXElement);
                        SetAttribute("ScreenName", sc.Code, fieldXElement);
                        SetAttribute("Row", sfield.Row.ToString(), fieldXElement);
                        SetAttribute("Column", sfield.Column.ToString(), fieldXElement);

                        if (sfield.Column > (numberOfColumns - 1))
                            numberOfColumns++;

                        if (sfield.Row > (numberOfRows - 1))
                            numberOfRows++;
                    }

                    SetAttribute("NumberOfRows", numberOfRows.ToString(), screenXElement);
                    SetAttribute("NumberOfColumns", numberOfColumns.ToString(), screenXElement);

                }
                else
                {
                    throw new Exception("Duplicated screen fields in " + sc.Name);
                }

            }
        }

        private void GenerateTabs(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
            //RemoveOldNodes(doc, entityElement, "Tab");
            List<ObjectTableTab> tableTabs = this.allTabs.Where(q => q.ObjectTableId == table.Id).OrderBy(q => q.IndexOrder).ToList();
            foreach (ObjectTableTab tab in tableTabs)
            {
                Feature tFeature = allFeatures.FirstOrDefault(f => f.Id == tab.FeatureId);
                TextCode tTextCode = allTextCodes.FirstOrDefault(t => t.Id == tab.TabNameTextCodeId);

                //XmlElement tabXElement = doc.CreateElement("Tab");
                //entityElement.AppendChild(tabXElement);

                XmlElement tabXElement = GetElementNodeByTagAndAttributeName(doc, entityElement, "Tab", "Code", GetStringValue(tab.Code), true);

                SetAttribute("Code", GetStringValue(tab.Code), tabXElement);
                SetAttribute("ObjectTableName", GetStringValue(table.Name), tabXElement);
                SetAttribute("IndexOrder", tab.IndexOrder.ToString(), tabXElement);
                SetAttribute("TextCode", GetStringValue(tTextCode.Code), tabXElement);
                SetAttribute("HtmlComponentName", GetStringValue(tab.HtmlComponentName), tabXElement);
                SetAttribute("HtmlComponentURL", GetStringValue(tab.HtmlComponentUrl), tabXElement);
                SetAttribute("ControlPath", GetStringValue(tab.ControlPath), tabXElement);
                SetAttribute("TabLocalName", GetStringValue(tTextCode.LocalDefaultText), tabXElement);
                SetAttribute("TabName", GetStringValue(tTextCode.DefaultText), tabXElement);
                SetAttribute("IsSpellChecked", tTextCode.IsSpellChecked.ToString().ToLower(), tabXElement);
                //SetAttribute("SpellCheckDate", GetStringValue(tTextCode.SpellCheckDate), tabXElement);

                if (tFeature != null)
                {
                    SetAttribute("IsPackagable", tFeature.Packagable.ToString(), tabXElement);
                    SetAttribute("FeatureCode", GetStringValue(tFeature.Code), tabXElement);
                    if (!string.IsNullOrEmpty(tFeature.NameTextCodeId))
                    {
                        TextCode featureTextCode = allTextCodes.FirstOrDefault(t => t.Id == tFeature.NameTextCodeId);
                        SetAttribute("FeatureTextCodeCode", GetStringValue(featureTextCode.Code), tabXElement);
                        SetAttribute("FeatureDefaultText", GetStringValue(featureTextCode.DefaultText), tabXElement);
                    }

                }

            }
        }


        private void GenerateEvents(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
            RemoveOldNodes(doc, entityElement, "EventTypes");
            XmlElement eventsListXElement = doc.CreateElement("EventTypes");
            entityElement.AppendChild(eventsListXElement);

            List<EventType> eventTypes = this.allEventTypes.Where(q => q.ObjectTableId == table.Id).ToList();
            foreach (EventType ev in eventTypes)
            {
                XmlElement eventXElement = doc.CreateElement("EventType");
                eventsListXElement.AppendChild(eventXElement);

                SetAttribute("Code", GetStringValue(ev.Code), eventXElement);
                SetAttribute("ObjectTableName", GetStringValue(table.Name), eventXElement);
                SetAttribute("ShortView", ev.ShortView.ToString().ToLower(), eventXElement);
                SetAttribute("IsManualEntry", ev.IsManualEntry.ToString().ToLower(), eventXElement);
                SetAttribute("LocalName", GetStringValue(ev.LocalName), eventXElement);
                SetAttribute("EnglishName", GetStringValue(ev.EnglishName), eventXElement);

                SetAttribute("EventTypeCategoryCode", GetStringValue(ev.EventTypeCategoryCode), eventXElement);
                SetAttribute("IsAgentView", ev.IsAgentView.ToString().ToLower(), eventXElement);
                SetAttribute("IsCustomerView", ev.IsCustomerView.ToString().ToLower(), eventXElement);
                SetAttribute("IsSharedLogisticsEnabled", ev.IsSharedLogisticsEnabled.ToString().ToLower(), eventXElement);
                SetAttribute("AllowedInAutomation", ev.AllowedInAutomation.ToString().ToLower(), eventXElement);
                SetAttribute("ManualActivatedFollowUp", ev.ManualActivatedFollowUp.ToString().ToLower(), eventXElement);
                SetAttribute("IsFollowUp", ev.IsFollowUp.ToString().ToLower(), eventXElement);
                SetAttribute("FollowUpEnglishName", GetStringValue(ev.FollowUpEnglishName), eventXElement);
                SetAttribute("FollowUpLocalName", GetStringValue(ev.FollowUpLocalName), eventXElement);
                if (!string.IsNullOrEmpty(ev.EntityStatusId))
                {
                    EntityStatus status = allEntityStatus.FirstOrDefault(f => f.Id == ev.EntityStatusId);
                    if (status != null)
                        SetAttribute("EntityStatusCode", GetStringValue(status.Code), eventXElement);
                }

            }
        }

        /*
         * 
         * <MenuButtons MenuButtonGroupType="&quot;WarehouseReleaseEdit&quot;" MenuButtonGroupName="&quot;WarehouseReleaseEditButtonsGroup&quot;">
                 <MenuButton EventCode="&quot;CreateDelivery&quot;" DefaultText="&quot;Create Delivery&quot;" MenuButtonType="&quot;button&quot;" IndexOrder="0" />
                 <MenuButton EventCode="&quot;CancelRelease&quot;" DefaultText="&quot;Cancel Release&quot;" MenuButtonType="&quot;button&quot;" IndexOrder="1" />
           </MenuButtons>
         * 
         * 
         * */

        private void GenerateMenuButtons(XmlDocument doc, XmlElement entityElement, ObjectTable table, List<ObjectField> tableObjectFields)
        {
            
            
            List<MenuButtonGroup> menuButtonGroups = this.allMenuButtonGroup.Where(g => g.ObjectTableId == table.Id).ToList();
            foreach (MenuButtonGroup group in menuButtonGroups)
            {
                XmlElement menuButtonsGroupXElement = (XmlElement)doc.GetElementsByTagName("MenuButtons")[0];
                if (menuButtonsGroupXElement != null)
                {
                    RemoveOldNodes(doc, menuButtonsGroupXElement, "MenuButton");
                }
                else
                {
                    menuButtonsGroupXElement = doc.CreateElement("MenuButtons");
                    entityElement.AppendChild(menuButtonsGroupXElement);
                }

                
                SetAttribute("MenuButtonGroupType", GetStringValue(group.MenuButtonGroupType), menuButtonsGroupXElement);
                SetAttribute("MenuButtonGroupName", GetStringValue(group.Name), menuButtonsGroupXElement);

                List<MenuButton> menuButtons = this.allMenuButtons.Where(m => m.MenuButtonGroupId == group.Id && m.ParentMenuButtonId == null).OrderBy(q => q.Index).ToList();
                int mindex = 0;
                foreach (MenuButton mb in menuButtons)
                {
                    Feature tFeature = allFeatures.FirstOrDefault(f => f.Id == mb.FeatureId);
                    TextCode lblTextCode = allTextCodes.FirstOrDefault(t => t.Id == mb.LabelTextCodeId);

                    XmlElement mBXElement = doc.CreateElement("MenuButton");
                    menuButtonsGroupXElement.AppendChild(mBXElement);

                    SetAttribute("EventCode", GetStringValue(mb.EventCode), mBXElement);
                    SetAttribute("TextCodeCode", GetStringValue(lblTextCode.Code), mBXElement);
                    SetAttribute("DefaultText", GetStringValue(lblTextCode.DefaultText), mBXElement);
                    SetAttribute("LocalDefaultText", GetStringValue(lblTextCode.LocalDefaultText), mBXElement);


                    SetAttribute("MenuButtonType", GetStringValue(mb.MenuButtonType), mBXElement);
                    SetAttribute("IndexOrder", mb.Index.ToString(), mBXElement);
                    SetAttribute("Style", GetStringValue(mb.Style), mBXElement);
                    if (tFeature != null)
                    {
                        SetAttribute("FeatureCode", GetStringValue(tFeature.Code), mBXElement);
                        SetAttribute("IsPackagable", tFeature.Packagable.ToString().ToLower(), mBXElement);
                        if (!string.IsNullOrEmpty(tFeature.NameTextCodeId))
                        {
                            TextCode featureTextCode = allTextCodes.FirstOrDefault(t => t.Id == tFeature.NameTextCodeId);
                            SetAttribute("FeatureTextCodeCode", GetStringValue(featureTextCode.Code), mBXElement);
                            SetAttribute("FeatureDefaultText", GetStringValue(featureTextCode.DefaultText), mBXElement);
                        }
                    }

                    List<MenuButton> menuButtonItems = this.allMenuButtons.Where(m => m.MenuButtonGroupId == group.Id && m.ParentMenuButtonId == mb.Id).OrderBy(q => q.Index).ToList();
                    int itemIndex = 0;
                    foreach (var item in menuButtonItems)
                    {
                        Feature itemFeature = allFeatures.FirstOrDefault(f => f.Id == item.FeatureId);
                        TextCode itemlblTextCode = allTextCodes.FirstOrDefault(t => t.Id == item.LabelTextCodeId);

                        XmlElement MenuItemElement = doc.CreateElement("MenuItem");
                        mBXElement.AppendChild(MenuItemElement);
                        SetAttribute("EventCode", GetStringValue(item.EventCode), MenuItemElement, null);
                        SetAttribute("TextCodeCode", GetStringValue(itemlblTextCode.Code), MenuItemElement);
                        SetAttribute("DefaultText", GetStringValue(itemlblTextCode.DefaultText), MenuItemElement, null);
                        SetAttribute("LocalDefaultText", GetStringValue(itemlblTextCode.LocalDefaultText), mBXElement);
                        SetAttribute("MenuButtonType", GetStringValue(item.MenuButtonType), MenuItemElement, null);
                        SetAttribute("Style", GetStringValue(item.Style), MenuItemElement, null);
                        SetAttribute("IndexOrder", item.Index.ToString(), MenuItemElement, null);
                        if (itemFeature != null)
                        {
                            SetAttribute("FeatureCode", GetStringValue(itemFeature.Code), MenuItemElement);
                            SetAttribute("IsPackagable", itemFeature.Packagable.ToString().ToLower(), MenuItemElement);
                            if (!string.IsNullOrEmpty(itemFeature.NameTextCodeId))
                            {
                                TextCode featureTextCode = allTextCodes.FirstOrDefault(t => t.Id == itemFeature.NameTextCodeId);
                                SetAttribute("FeatureTextCodeCode", GetStringValue(featureTextCode.Code), MenuItemElement);
                                SetAttribute("FeatureDefaultText", GetStringValue(featureTextCode.DefaultText), MenuItemElement);
                            }
                        }

                        itemIndex++;
                    }

                    mindex++;
                }
            }
        }



        #region GenerateObjectFieldFields


        private void GenerateObjectFieldFields(XmlDocument doc, XmlElement entityElement, List<ObjectField> fields, PropertyInfo[] pocoProperties, PropertyInfo[] pmClassProperties, PropertyInfo[] listClassProperties, List<string> addeddFields)
        {
            foreach (ObjectField f in fields)
            {
                addeddFields.Add(f.FieldName);

                XmlElement fieldElement = GetElementNodeByTagAndAttributeName(doc, entityElement, "field", "FieldName", f.FieldName);//doc.CreateElement("field");
                if (fieldElement == null)
                {
                    fieldElement = doc.CreateElement("field");
                    entityElement.AppendChild(fieldElement);
                    SetAttribute("Id", GetStringValue(Guid.NewGuid()), fieldElement, null);
                    SetAttribute("FieldName", GetStringValue(f.FieldName), fieldElement, null);
                }
                SetAttribute("ObjectTableName", GetStringValue(f.ObjectTable.Name), fieldElement, f);
                SetAttribute("FieldsDataType", GetStringValue(f.DataTypeCode), fieldElement, f);
                if (f.LookUpTableId != null)
                {
                    SetAttribute("LookUpTableName", GetStringValue(f.ObjectTable_LookUpTable.Name), fieldElement, f);
                }

                ////////////////////////////////////////////////////////////////////

                PropertyInfo[] objectfieldProperties = f.GetType().GetProperties().Where(
                d =>
                     d.Name != "Id" &&
                      d.Name != "Tenant" &&
                      d.Name != "FieldName" &&
                      d.Name != "ObjectTableName" &&
                      d.Name != "LookUpTableName" &&
                      d.Name != "MultiTableName" &&
                       d.Name != "FullFieldLable" &&
                        d.Name != "DefaultText" &&
                         d.Name != "FullLocalDefaultText" &&
                          d.Name != "ListFieldLable" &&
                           d.Name != "ListLableDefaultText" &&
                           d.Name != "ListLocalDefaultText" &&
                           d.Name != "HelpTextCode" &&
                           d.Name != "HelpTextDefaultText" &&
                             d.Name != "HelpLocalDefaultText" &&
                               d.Name != "ShortFieldLable" &&
                                 d.Name != "ShortFieldLableDefaultText" &&
                                   d.Name != "ShortLocalDefaultText" &&

                                   d.Name != "FullNameTextCodeId" &&
                                    d.Name != "HelpTextCodeId" &&
                                     d.Name != "ObjectTableId" &&
                                     d.Name != "ShortNameTextCodeId" &&
                                     d.Name != "ListTextCodeId" &&
                                     d.Name != "MultiTableId"
                                     && d.Name != "IsRequiered"
                                     && d.Name != "DataTypeCode"
                                     && d.Name != "AllowedInAirlineMessaging"
                                     && d.Name != "LookUpTableId"
                                     && d.Name != "HtmlListComponentUrl"
                                ).ToArray();

                foreach (var propInf in objectfieldProperties)
                {
                    object value = propInf.GetValue(f);
                    if (propInf.PropertyType == typeof(bool) || propInf.PropertyType == typeof(int))
                    {
                        string stringValue = (value != null ? value.ToString().ToLower() : null);
                        SetAttribute(propInf.Name, stringValue, fieldElement);
                    }
                    else if (propInf.PropertyType == typeof(string))
                    {

                        SetAttribute(propInf.Name, GetStringValue(value), fieldElement);

                    }
                }

                SetAttribute("IsRequired", f.IsRequiered.ToString().ToLower(), fieldElement, f);
                if (f.IsMulti && f.ObjectTable_MultiTable != null)
                {
                    SetAttribute("MultiTableName", GetStringValue(f.ObjectTable_MultiTable.Name), fieldElement, f);
                }


                if (f.FullNameTextCode != null)
                {

                    //FullNameTextCodeId = objectFieldDetails.ObjectTableName + ".F." + objectFieldDetails.FullFieldLable;
                    string fullLable = f.FullNameTextCode.Code.Split('.').Length > 2 ? f.FullNameTextCode.Code.Split('.')[2] : f.FullNameTextCode.Code.Split('.')[1];
                    SetAttribute("FullFieldLable", GetStringValue(fullLable), fieldElement, f);
                    SetAttribute("DefaultText", GetStringValue(f.FullNameTextCode.DefaultText), fieldElement, f);
                    SetAttribute("FullLocalDefaultText", GetStringValue(f.FullNameTextCode.LocalDefaultText), fieldElement, f);
                    SetAttribute("IsSpellCheckedFullFieldLable", f.FullNameTextCode.IsSpellChecked.ToString().ToLower(), fieldElement, f);
                }

                if (f.ListTextCode != null)
                {
                    //objectFieldDetails.ObjectTableName + ".CH." + objectFieldDetails.ListFieldLable
                    string lable = f.ListTextCode.Code.Split('.').Length > 2 ? f.ListTextCode.Code.Split('.')[2] : f.ListTextCode.Code.Split('.')[1];
                    SetAttribute("ListFieldLable", GetStringValue(lable), fieldElement, f);//f.FieldName + "ListLable"
                    SetAttribute("ListLableDefaultText", GetStringValue(f.ListTextCode.DefaultText), fieldElement, f);
                    SetAttribute("ListLocalDefaultText", GetStringValue(f.ListTextCode.LocalDefaultText), fieldElement, f);
                    SetAttribute("IsSpellCheckedListLocalDefaultText", f.ListTextCode.IsSpellChecked.ToString().ToLower(), fieldElement, f);

                }

                if (f.HelpTextCode != null)
                {

                    //helpTextTextCode.Code = objectFieldDetails.ObjectTableName + "." + objectFieldDetails.HelpTextCode + "HelpText";
                    string lable = f.HelpTextCode.Code.Split('.')[1].Replace("HelpText", "");
                    SetAttribute("HelpTextCode", GetStringValue(lable), fieldElement, f);//GetStringValue(f.FieldName)
                    if (!string.IsNullOrEmpty(f.HelpTextCode.DefaultText))
                    {
                        SetAttribute("HelpTextDefaultText", GetStringValue(f.HelpTextCode.DefaultText), fieldElement, f);
                    }
                    if (!string.IsNullOrEmpty(f.HelpTextCode.LocalDefaultText))
                    {
                        SetAttribute("HelpLocalDefaultText", GetStringValue(f.HelpTextCode.LocalDefaultText), fieldElement, f);
                    }
                    SetAttribute("IsSpellCheckedHelpLocalDefaultText", f.HelpTextCode.IsSpellChecked.ToString().ToLower(), fieldElement, f);


                }
                if (f.ShortNameTextCode != null)
                {
                    //fullFieldTextCode.Code = objectFieldDetails.ObjectTableName + ".F." + objectFieldDetails.ShortFieldLable + ".Short";
                    string lable = f.ShortNameTextCode.Code.Split('.').Length > 2 ? f.ShortNameTextCode.Code.Split('.')[2] : f.ShortNameTextCode.Code.Split('.')[1];
                    SetAttribute("ShortFieldLable", GetStringValue(lable), fieldElement, f);//
                    SetAttribute("ShortFieldLableDefaultText", GetStringValue(f.ShortNameTextCode.DefaultText), fieldElement, f);
                    SetAttribute("ShortLocalDefaultText", GetStringValue(f.ShortNameTextCode.LocalDefaultText), fieldElement, f);
                    SetAttribute("IsSpellCheckedShortLocalDefaultText", f.ShortNameTextCode.IsSpellChecked.ToString().ToLower(), fieldElement, f);


                }

                PropertyInfo prop = pocoProperties.Where(p => p.Name == f.FieldName).FirstOrDefault();
                if (prop != null)
                {
                    if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(KeyAttribute)).Any())
                    {
                        SetAttribute("IsPrimaryKey", "true", fieldElement, f);

                        SetAttribute("DisplayInList", "true", fieldElement, null);
                    }

                    if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(ColumnAttribute)).Any())
                    {
                        CustomAttributeData data = prop.CustomAttributes.Where(c => c.AttributeType == typeof(ColumnAttribute)).FirstOrDefault();
                        CustomAttributeNamedArgument arg = data.NamedArguments.Where(c => c.MemberName == "Order").FirstOrDefault();

                        if (arg != null && arg.MemberInfo != null)
                        {
                            SetAttribute("Order", arg.TypedValue.Value.ToString(), fieldElement, f);
                        }

                    }


                    if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(ForeignKeyAttribute)).FirstOrDefault() != null)
                    {
                        //CustomAttributeData data = prop.CustomAttributes.Where(c => c.AttributeType == typeof(ForeignKeyAttribute)).FirstOrDefault();
                        //string forignEntityName = data.ConstructorArguments[0].Value as string;
                        //associationProperties.Add(prop.Name, forignEntityName);

                        CustomAttributeData data = prop.CustomAttributes.Where(c => c.AttributeType == typeof(ForeignKeyAttribute)).FirstOrDefault();
                        string navigationPropertyName = data.ConstructorArguments[0].Value as string;
                        SetAttribute("IsForeignKey", "true", fieldElement, f);
                        SetAttribute("NavigationPropertyName", navigationPropertyName, fieldElement, f);

                        PropertyInfo navigationProperty = pocoProperties.Where(p => p.Name == navigationPropertyName).FirstOrDefault();
                        SetAttribute("ForeignEntity", navigationProperty.PropertyType.Name, fieldElement, f);
                    }

                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        SetAttribute("IsNullable", "true", fieldElement, null);
                    }
                }


                if (pocoProperties.Where(p => p.Name == f.FieldName).Any())
                {
                    SetAttribute("HasDataBaseField", "true", fieldElement, null);
                }
                else
                {
                    SetAttribute("HasDataBaseField", "false", fieldElement, null);
                }

                if (pmClassProperties.Where(p => p.Name == f.FieldName).Any())
                {
                    SetAttribute("HasPMField", "true", fieldElement, null);
                }
                else
                {
                    SetAttribute("HasPMField", "false", fieldElement, null);
                }

                if (listClassProperties.Where(p => p.Name == f.FieldName).Any())
                {

                    SetAttribute("GenerateInList", "true", fieldElement, null);
                }
                else
                {

                    SetAttribute("GenerateInList", "false", fieldElement, null);
                }


                SetAttribute("NoMetaDataField", "false", fieldElement, null);
            }
        }


        #endregion

        #region GeneratePocoFields


        private void GeneratePocoFields(XmlDocument doc, XmlElement entityElement, List<ObjectField> fields, ObjectTable table, PropertyInfo[] pocoProperties, PropertyInfo[] pmClassProperties, PropertyInfo[] listClassProperties, List<string> addeddFields)
        {
            foreach (PropertyInfo prop in pocoProperties)
            {
                if (prop != null)
                {
                    if (!fields.Where(f => f.FieldName.ToLower() == prop.Name.ToLower()).Any() && prop.PropertyType.Module.ScopeName == "CommonLanguageRuntimeLibrary")
                    {
                        addeddFields.Add(prop.Name);

                        XmlElement fieldElement = GetElementNodeByTagAndAttributeName(doc, entityElement, "field", "FieldName", prop.Name);//GetFieldElement(doc, entityElement, prop.Name);//doc.CreateElement("field");
                        if (fieldElement == null)
                        {
                            fieldElement = doc.CreateElement("field");
                            entityElement.AppendChild(fieldElement);
                            SetAttribute("Id", GetStringValue(Guid.NewGuid()), fieldElement, null);
                            SetAttribute("FieldName", GetStringValue(prop.Name), fieldElement, null);
                        }
                        
                        SetAttribute("ObjectTableName", GetStringValue(table.Name), fieldElement, null);

                        Type propertyType = prop.PropertyType;
                        if (propertyType.IsGenericType &&
                            propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }





                        SetAttribute("PMPropertyPath", GetStringValue(prop.Name), fieldElement, null);
                        SetAttribute("ListPropertyPath", GetStringValue(prop.Name), fieldElement, null);

                        SetAttribute("FullFieldLable", GetStringValue(prop.Name), fieldElement, null);
                        SetAttribute("DefaultText", GetStringValue(prop.Name), fieldElement, null);
                        SetAttribute("FullLocalDefaultText", GetStringValue(prop.Name), fieldElement, null);

                        SetAttribute("ListFieldLable", GetStringValue(prop.Name + "ListLable"), fieldElement, null);
                        SetAttribute("ListLableDefaultText", GetStringValue(prop.Name), fieldElement, null);
                        SetAttribute("ListLocalDefaultText", GetStringValue(prop.Name), fieldElement, null);
                        if (fields.Count > 0)
                        {
                            SetAttribute("ValidForQuerySection1", GetStringValue(fields.FirstOrDefault().ValidForQuerySection1), fieldElement, null);
                            SetAttribute("ValidForQuerySection2", GetStringValue(fields.FirstOrDefault().ValidForQuerySection2), fieldElement, null);
                        }
                        CustomAttributeData stringlengthdata = prop.CustomAttributes.Where(c => c.AttributeType == typeof(StringLengthAttribute)).FirstOrDefault();
                        if (stringlengthdata != null)
                        {
                            SetAttribute("MaxLength", stringlengthdata.ConstructorArguments[0].Value.ToString(), fieldElement, null);
                            SetAttribute("SystemMaxLength", stringlengthdata.ConstructorArguments[0].Value.ToString(), fieldElement, null);

                            if (stringlengthdata.NamedArguments.Count > 0)
                            {
                                SetAttribute("MinimumLength", stringlengthdata.NamedArguments[0].TypedValue.ToString(), fieldElement, null);
                            }
                            else
                            {
                                if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(RequiredAttribute)).Any())
                                {
                                    SetAttribute("MinimumLength", "1", fieldElement, null);
                                }
                                else
                                {
                                    SetAttribute("MinimumLength", "0", fieldElement, null);
                                }
                            }
                        }

                        if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(KeyAttribute)).Any())
                        {
                            SetAttribute("IsPrimaryKey", "true", fieldElement, null);
                        }
                        if (prop.CustomAttributes.Where(c => c.AttributeType == typeof(RequiredAttribute)).Any())
                        {
                            SetAttribute("IsRequired", "true", fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("IsRequired", "false", fieldElement, null);
                        }



                        CustomAttributeData columnData = prop.CustomAttributes.Where(c => c.AttributeType == typeof(ColumnAttribute)).FirstOrDefault();
                        if (columnData != null)
                        {
                            CustomAttributeNamedArgument arg = columnData.NamedArguments.Where(c => c.MemberName == "Order").FirstOrDefault();

                            if (arg != null && arg.MemberInfo != null)
                            {
                                SetAttribute("Order", arg.TypedValue.Value.ToString(), fieldElement, null);
                            }

                        }

                        CustomAttributeData foreignKeyData = prop.CustomAttributes.Where(c => c.AttributeType == typeof(ForeignKeyAttribute)).FirstOrDefault();
                        if (foreignKeyData != null)
                        {
                            string navigationPropertyName = foreignKeyData.ConstructorArguments[0].Value as string;
                            SetAttribute("IsForeignKey", "true", fieldElement, null);
                            SetAttribute("NavigationPropertyName", navigationPropertyName, fieldElement, null);
                            PropertyInfo navigationProperty = pocoProperties.Where(p => p.Name == navigationPropertyName).FirstOrDefault();
                            SetAttribute("ForeignEntity", navigationProperty.PropertyType.Name, fieldElement, null);
                        }


                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            SetAttribute("IsNullable", "true", fieldElement, null);
                        }


                        SetAttribute("HasDataBaseField", "true", fieldElement, null);

                        if (pmClassProperties.Where(p => p.Name == prop.Name).Any())
                        {
                            SetAttribute("HasPMField", "true", fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("HasPMField", "false", fieldElement, null);
                        }



                        if (listClassProperties.Where(p => p.Name == prop.Name).Any())
                        {
                            SetAttribute("DisplayInList", "true", fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("DisplayInList", "false", fieldElement, null);
                        }

                         SetAttribute("NoMetaDataField", "true", fieldElement, null);



                    }
                    else
                    {
                        //  XmlElement fieldElement = doc.

                        string nodeFieldName = "\"" + prop.Name.Replace("\"", "\u0022") + "\"";
                        XmlNodeList xnList = doc.GetElementsByTagName("field");
                        foreach (XmlNode node in xnList)
                        {
                            if (node.Attributes["FieldName"].Value.ToLower() == nodeFieldName.ToLower())
                            {
                                XmlAttribute att = doc.CreateAttribute("HasDataBaseField");
                                att.Value = "true";
                                node.Attributes.Append(att);

                                node.Attributes["FieldName"].Value = nodeFieldName;
                            }
                        }


                    }
                }
            }
        }


        #endregion


        #region GeneratePMOnlyFields


        private void GeneratePMOnlyFields(XmlDocument doc, XmlElement entityElement, List<ObjectField> fields, ObjectTable table, PropertyInfo[] pmClassProperties, PropertyInfo[] pocoProperties, List<string> addeddFields)
        {


            //[Include]
            //[Composition]
            //[Association("ConsignmentConsignmentPackage", "DeclarationId,ConsignmentNumber", "DeclarationId,ConsignmentNumber")]
            //public virtual List<ConsignmentPackagePM> ConsignmentPackages

            foreach (PropertyInfo pmProperty in pmClassProperties)
            {
                if (!addeddFields.Any(f => f.ToLower() == pmProperty.Name.ToLower()) && !pmProperty.Name.Contains("ChangeSet"))// && pmProperty.Name != "ChangeOp")
                {
                    addeddFields.Add(pmProperty.Name);

                    bool isListOfObjectsProperty = pmProperty.PropertyType.Name.Contains("List");

                    XmlElement fieldElement = GetElementNodeByTagAndAttributeName(doc, entityElement, "field", "FieldName", pmProperty.Name);//GetFieldElement(doc, entityElement, pmProperty.Name);//doc.CreateElement("field");
                    if (fieldElement == null)
                    {
                        fieldElement = doc.CreateElement("field");
                        entityElement.AppendChild(fieldElement);
                        SetAttribute("Id", GetStringValue(Guid.NewGuid()), fieldElement, null);
                        SetAttribute("FieldName", GetStringValue(pmProperty.Name), fieldElement, null);
                    }
                    if (isListOfObjectsProperty)
                    {
                        SetAttribute("FieldsDataType", GetStringValue("List"), fieldElement, null);
                    }
                    else
                    {
                        Type propertyType = pmProperty.PropertyType;
                        if (propertyType.IsGenericType &&
                            propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }

                    }
                    //SetAttribute("FieldName", GetStringValue(pmProperty.Name), fieldElement, null);

                    SetAttribute("PMPropertyPath", GetStringValue(pmProperty.Name), fieldElement, null);
                    SetAttribute("ListPropertyPath", GetStringValue(pmProperty.Name), fieldElement, null);

                    SetAttribute("FullFieldLable", GetStringValue(pmProperty.Name), fieldElement, null);
                    SetAttribute("DefaultText", GetStringValue(pmProperty.Name), fieldElement, null);
                    SetAttribute("FullLocalDefaultText", GetStringValue(pmProperty.Name), fieldElement, null);

                    SetAttribute("ListFieldLable", GetStringValue(pmProperty.Name + "ListLable"), fieldElement, null);
                    SetAttribute("ListLableDefaultText", GetStringValue(pmProperty.Name), fieldElement, null);
                    SetAttribute("ListLocalDefaultText", GetStringValue(pmProperty.Name), fieldElement, null);

                    SetAttribute("ObjectTableName", GetStringValue(table.Name), fieldElement, null);
                    if (fields.Count > 0)
                    {
                        SetAttribute("ValidForQuerySection1", GetStringValue(fields.FirstOrDefault().ValidForQuerySection1), fieldElement, null);
                        SetAttribute("ValidForQuerySection2", GetStringValue(fields.FirstOrDefault().ValidForQuerySection2), fieldElement, null);
                    }
                    SetAttribute("HasPMField", "true", fieldElement, null);

                    if (pocoProperties.Any(f => f.Name == pmProperty.Name))
                    {
                        SetAttribute("HasDataBaseField", "true", fieldElement, null);
                    }
                    else
                    {
                        SetAttribute("HasDataBaseField", "false", fieldElement, null);
                    }
                    if (!fields.Any(f => f.FieldName == pmProperty.Name))
                    {
                        SetAttribute("NoMetaDataField", "true", fieldElement, null);
                    }

                    if (isListOfObjectsProperty)
                    {
                        CustomAttributeData associationData = pmProperty.CustomAttributes.Where(c => c.AttributeType == typeof(AssociationAttribute)).FirstOrDefault();
                        if (associationData != null)
                        {
                            string associationEntityName = pmProperty.PropertyType.GenericTypeArguments.FirstOrDefault().Name;
                            SetAttribute("IsMulti", "true", fieldElement, null);
                            SetAttribute("MultiTableName", GetStringValue(associationEntityName.Replace("PM", "")), fieldElement, null);

                            SetAttribute("AssociationName", associationData.ConstructorArguments[0].Value.ToString(), fieldElement, null);
                            SetAttribute("ThisKey", associationData.ConstructorArguments[1].Value.ToString(), fieldElement, null);
                            SetAttribute("OtherKey", associationData.ConstructorArguments[2].Value.ToString(), fieldElement, null);



                            CustomAttributeData compoistion = pmProperty.CustomAttributes.Where(c => c.AttributeType.Name == "CompositionAttribute").FirstOrDefault();
                            if (compoistion != null)
                            {
                                SetAttribute("IsComposition", "true", fieldElement, null);
                            }
                            else
                            {
                                SetAttribute("IsComposition", "false", fieldElement, null);
                            }
                        }
                        else
                        {
                            var innerType = pmProperty.PropertyType.GetGenericArguments()[0];
                            SetAttribute("MultiTableName", GetStringValue(GetStringValue(innerType.Name.Replace("PM", ""))), fieldElement, null);

                        }
                    }
                    else
                    {
                        Type propertyType = pmProperty.PropertyType;
                        if (propertyType.IsGenericType &&
                            propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }
                        else
                        {
                            SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                        }

                    }
                }
                else
                {
                    string nodeFieldName = "\"" + pmProperty.Name.Replace("\"", "\u0022") + "\"";
                    XmlNodeList xnList = doc.GetElementsByTagName("field");
                    foreach (XmlNode node in xnList)
                    {
                        if (node.Attributes["FieldName"].Value.ToLower() == nodeFieldName.ToLower())
                        {
                            XmlAttribute att = doc.CreateAttribute("HasPMField");
                            att.Value = "true";
                            node.Attributes.Append(att);
                        }
                    }
                }
            }


        }

        #endregion

        #region GenerateListOnlyFields


        private void GenerateListOnlyFields(XmlDocument doc, XmlElement entityElement, List<ObjectField> fields, ObjectTable table, PropertyInfo[] listClassProperties, PropertyInfo[] pocoProperties, List<string> addeddFields)
        {


            //[Include]
            //[Composition]
            //[Association("ConsignmentConsignmentPackage", "DeclarationId,ConsignmentNumber", "DeclarationId,ConsignmentNumber")]
            //public virtual List<ConsignmentPackagePM> ConsignmentPackages

            foreach (PropertyInfo listProperty in listClassProperties)
            {
                if (!addeddFields.Any(f => f.ToLower() == listProperty.Name.ToLower()))
                {
                    addeddFields.Add(listProperty.Name);

                    XmlElement fieldElement = GetElementNodeByTagAndAttributeName(doc, entityElement, "field", "FieldName", listProperty.Name);//GetFieldElement(doc, entityElement, listProperty.Name);//doc.CreateElement("field");
                    if (fieldElement == null)
                    {
                        fieldElement = doc.CreateElement("field");
                        entityElement.AppendChild(fieldElement);
                        SetAttribute("Id", GetStringValue(Guid.NewGuid()), fieldElement, null);
                        SetAttribute("FieldName", GetStringValue(listProperty.Name), fieldElement, null);
                    }

                    SetAttribute("FieldName", GetStringValue(listProperty.Name), fieldElement, null);

                    SetAttribute("PMPropertyPath", GetStringValue(listProperty.Name), fieldElement, null);
                    SetAttribute("ListPropertyPath", GetStringValue(listProperty.Name), fieldElement, null);

                    SetAttribute("FullFieldLable", GetStringValue(listProperty.Name), fieldElement, null);
                    SetAttribute("DefaultText", GetStringValue(listProperty.Name), fieldElement, null);
                    SetAttribute("FullLocalDefaultText", GetStringValue(listProperty.Name), fieldElement, null);

                    SetAttribute("ListFieldLable", GetStringValue(listProperty.Name + "ListLable"), fieldElement, null);
                    SetAttribute("ListLableDefaultText", GetStringValue(listProperty.Name), fieldElement, null);
                    SetAttribute("ListLocalDefaultText", GetStringValue(listProperty.Name), fieldElement, null);

                    SetAttribute("ObjectTableName", GetStringValue(table.Name), fieldElement, null);
                    if (fields.Count > 0)
                    {
                        SetAttribute("ValidForQuerySection1", GetStringValue(fields.FirstOrDefault().ValidForQuerySection1), fieldElement, null);
                        SetAttribute("ValidForQuerySection2", GetStringValue(fields.FirstOrDefault().ValidForQuerySection2), fieldElement, null);
                    }
                    SetAttribute("HasPMField", "false", fieldElement, null);

                    if (pocoProperties.Any(f => f.Name == listProperty.Name))
                    {
                        SetAttribute("HasDataBaseField", "true", fieldElement, null);
                    }
                    else
                    {
                        SetAttribute("HasDataBaseField", "false", fieldElement, null);
                    }
                    if (!fields.Any(f => f.FieldName == listProperty.Name))
                    {
                        SetAttribute("NoMetaDataField", "true", fieldElement, null);
                    }


                    Type propertyType = listProperty.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                        SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                    }
                    else
                    {
                        SetAttribute("FieldsDataType", GetStringValue(GetDataType(propertyType.Name)), fieldElement, null);
                    }


                    SetAttribute("GenerateInList", "true", fieldElement, null);

                }
                else
                {
                    string nodeFieldName = "\"" + listProperty.Name.Replace("\"", "\u0022") + "\"";
                    XmlNodeList xnList = doc.GetElementsByTagName("field");
                    foreach (XmlNode node in xnList)
                    {
                        if (node.Attributes["FieldName"].Value.ToLower() == nodeFieldName.ToLower())
                        {
                            XmlAttribute att = doc.CreateAttribute("GenerateInList");
                            att.Value = "true";
                            node.Attributes.Append(att);



                        }
                    }
                }
            }


        }

        #endregion

        #region GenerateEntityElement


        private XmlElement GenerateEntityElement(XmlDocument doc, ObjectTable table, List<TextCode> tableTextCodes)
        {

            XmlElement entityElement = (XmlElement)doc.AppendChild(doc.CreateElement("entity"));

            SetAttribute("Id", GetStringValue(Guid.NewGuid()), entityElement, null);
            SetAttribute("ObjectTableName", GetStringValue(table.Name), entityElement);
            SetAttribute("DBTableName", (!string.IsNullOrEmpty(table.DBTableName) ? GetStringValue(table.DBTableName) : GetStringValue("NONE")), entityElement);

            TextCode tableTextCode = (from a in tableTextCodes
                                      where a.Tenant == 0 && a.Code == table.Name
                                      select a).FirstOrDefault();
            if (tableTextCode != null)
            {
                SetAttribute("ObjectTableSingular", GetStringValue(tableTextCode.DefaultText), entityElement);
                SetAttribute("ObjectTablePlural", GetStringValue(tableTextCode.DefaultTextPlural), entityElement);
                SetAttribute("DefaultText", GetStringValue(tableTextCode.DefaultText), entityElement);
            }
            if (table.DescriptionTextCode != null)
            {
                SetAttribute("DescriptionDefaultText", GetStringValue(table.DescriptionTextCode.DefaultText), entityElement);

            }

            PropertyInfo[] objectTableProperties = table.GetType().GetProperties().Where(
                f => f.Name != "ObjectTableSingular" &&
                    f.Name != "ObjectTablePlural" &&
                    f.Name != "DescriptionDefaultText" &&
                     f.Name != "Id" &&
                      f.Name != "Tenant" &&
                      f.Name != "ObjectTableName" &&
                      f.Name != "DBTableName"
                      && f.Name != "HeaderScreenId"

                     ).ToArray();

            foreach (var prop in objectTableProperties)
            {
                object value = prop.GetValue(table);
                if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(int))
                {
                    SetAttribute(prop.Name, value.ToString().ToLower(), entityElement);
                }
                else if (prop.PropertyType == typeof(string))
                {

                    SetAttribute(prop.Name, GetStringValue(value), entityElement);

                }
            }

            IEnumerable<IGrouping<string, Query>> tableQueries = this.allQueries.Where(q => q.ObjectTableId == table.Id && q.QueryGroupCode != null && q.SystemLevel == true).GroupBy(q => q.QueryGroupCode);

            int? index = null;
            foreach (var item in tableQueries)
            {
                QueryGroup qg = this.allQueryGroups.Where(g => g.Code == item.Key).FirstOrDefault();
                SetAttribute("Code" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Code), entityElement, null);
                SetAttribute("Name" + (index != null ? index.Value.ToString() : ""), GetStringValue(qg.Name), entityElement, null);

                if (index == null)
                {
                    index = 1;
                }
                else
                    index++;
            }

            if (table.IsClosed)
            {
                ObjectField codeField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Code").FirstOrDefault();
                if (codeField == null)
                {
                    codeField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Id").FirstOrDefault();
                }

                ObjectField nameField = this.allFields.Where(q => q.ObjectTableId == table.Id && q.FieldName == "Name" || q.FieldName == "EnlglishName").FirstOrDefault();
                if (codeField != null)
                    SetAttribute("CloseTableCode", GetStringValue(codeField.FieldName), entityElement, null);

                if (nameField != null)
                    SetAttribute("CloseTableName", GetStringValue(nameField.FieldName), entityElement, null);
            }



            return entityElement;

        }

        #endregion

        private void RemoveOldNodes(XmlDocument doc, XmlElement entityElement, string nodeName)
        {

            XmlNodeList querieLists = doc.GetElementsByTagName(nodeName);
            if (querieLists != null)
            {
                while (querieLists.Count > 0)
                {
                    entityElement.RemoveChild(querieLists[0]);
                }
            }
        }

        //private void RemoveOldNodeChildren(XmlDocument doc, XmlElement entityElement, string nodeName,string childNodes)
        //{

        //    XmlNodeList querieLists = doc.GetElementsByTagName(nodeName);
        //    if (querieLists != null)
        //    {
        //        while (querieLists.Count > 0)
        //        {
        //            entityElement.RemoveChild(querieLists[0]);
        //        }
        //    }
        //}































        #region GetDataType


        public static string GetDataType(string type)
        {
            string result = "Text";
            type = type.ToLower();
            switch (type)
            {
                case "varchar":
                    result = "Text";
                    break;
                case "nvarchar":
                    result = "nText";
                    break;
                case "decimal":
                    result = "Decimal";
                    break;

                case "double":
                    result = "Double";
                    break;

                case "int32":
                case "integer":
                    result = "Integer";
                    break;

                case "bool":
                case "boolean":
                    result = "Boolean";
                    break;

                case "datetime":
                case "date":
                    result = "DateTime";
                    break;

            }

            return result;
        }

        #endregion

        #region GetStringValue

        private static string GetStringValue(object value)
        {

            if (value != null)
            {
                //if (value.ToString().Contains("\""))
                //{
                //    value = "\"" + value.ToString().Replace("\"", @"\""") + "\"";
                //}
                //if(value.ToString().Contains("\n"))
                //{
                //    value = value.ToString().Replace("\n", "\"" + "\n" + "\"");
                //}
                return "\"" + value.ToString().Replace("\"", "\u0022") + "\"";//Char.ConvertFromUtf32(34);//

                //  return value.ToString();
            }
            else
            {
                return null;
            }
        }


        #endregion

        #region SetAttribute

        private static void SetAttribute(string atrrName, string attrValue, XmlElement fieldElement, ObjectField field)
        {
            if (attrValue != null)
            {
                fieldElement.SetAttribute(atrrName, attrValue);
            }
        }

        #endregion

        #region SetAttribute

        private static void SetAttribute(string atrrName, string attrValue, XmlElement fieldElement)
        {
            if (!string.IsNullOrEmpty(attrValue))
            {
                fieldElement.SetAttribute(atrrName, attrValue);
            }
        }
        #endregion
    }
    public class EntityPropertiesInfo
    {
        public EntityPropertiesInfo()
        {
            this.POCOClassProperites = new List<PropertyInfo>();
            this.PMClassProperties = new List<PropertyInfo>();
            this.ListClassProperties = new List<PropertyInfo>();
        }
        public List<PropertyInfo> POCOClassProperites { get; set; }
        public List<PropertyInfo> PMClassProperties { get; set; }
        public List<PropertyInfo> ListClassProperties { get; set; }
    }
}
