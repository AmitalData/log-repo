using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper
{
    public class TableStructureHelper
    {
        private DataSet xmlDataSet;
        private DataTable table;
        private DataTable tableCoulmns;
        private DataTable tableCoulmnsConstraint;
        private DataTable tableCoulmnsRelation;
        private DataTable tableIndexs;
        private DataTable tableUniqueConstraints;
        public string primarykeyColumn;
        public List<string> TableCoulmnsNameWithoutIDentity;

        public TableStructureHelper(string dxmlStructure)
        {
            InitializeDataTables(dxmlStructure);
        }
 

        private void InitializeDataTables(string dxmlStructure)
        {
            TableCoulmnsNameWithoutIDentity = new List<string>();
            xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(XmlReader.Create(new StringReader(dxmlStructure)));
            table = xmlDataSet.Tables["Table"];
            tableCoulmns = table.ChildRelations["Table_Column"].ChildTable;
            tableIndexs = table.ChildRelations["Table_Index"] == null ? null : table.ChildRelations["Table_Index"].ChildTable;
            tableCoulmnsRelation = table.ChildRelations["Table_Relation"] == null ? null : table.ChildRelations["Table_Relation"].ChildTable;
            tableUniqueConstraints = table.ChildRelations["Table_UniqueConstraint"] == null ? null : table.ChildRelations["Table_UniqueConstraint"].ChildTable;
            tableCoulmnsConstraint = tableCoulmns.ChildRelations["Column_Constraints"].ChildTable;
        }
        public string GetTableStructure(string tableName)
        {

            string TableStructure = InitializeTableStructure(tableName);
            TableStructure += GetTableStructurCoulmns();
            TableStructure += GetTableStructurePrimartKey(tableName);
            TableStructure += GetTableStructureIndexs(tableName);
            return TableStructure;

        }

        private string InitializeTableStructure(string tableName)
        {
            string TableStructure = "If not exists (select * from sysobjects where name='" + tableName + "' and xtype='U')\n" +
                                  "BEGIN \n" +
                                  "CREATE TABLE [dbo].[" + tableName + "]( \n";

            return TableStructure;

        }

        private string GetTableStructurCoulmns()
        {
            string TableStructure = "";
            for (int i = 0; i < tableCoulmns.Rows.Count; i++)
            {
                if (tableCoulmns.Columns.Contains("Name") && tableCoulmns.Columns.Contains("Type"))
                    TableStructure += "[" + tableCoulmns.Rows[i]["Name"] + "] " + tableCoulmns.Rows[i]["Type"];
                if (tableCoulmns.Columns.Contains("Size"))
                    TableStructure += GetCoulmnSize(tableCoulmns.Rows[i]["Size"]);
                if (tableCoulmns.Columns.Contains("Identity"))
                {   string IdentityText = GetIsCoulmnIdentity(tableCoulmns.Rows[i]["Identity"]);
                    TableStructure += IdentityText;
                    if (string.IsNullOrEmpty(IdentityText))
                        TableCoulmnsNameWithoutIDentity.Add((string)tableCoulmns.Rows[i]["Name"]);
                }
                else
                    TableCoulmnsNameWithoutIDentity.Add((string)tableCoulmns.Rows[i]["Name"]);
                if (tableCoulmnsConstraint.Columns.Contains("Nullable"))
                    TableStructure += GetIsCoulmnNullable(tableCoulmnsConstraint.Rows[i]["Nullable"]);
                if (tableCoulmnsConstraint.Columns.Contains("PrimaryKey") && tableCoulmns.Columns.Contains("Name"))
                    SetPrimartKeyCoulmn(tableCoulmnsConstraint.Rows[i]["PrimaryKey"], (string)tableCoulmns.Rows[i]["Name"]);
                TableStructure += ",\n";
            }
            return TableStructure;
        }

     
        public string GetTableName()
        {           
            string TableName = (string)table.Rows[0]["Name"];
            return TableName;
        }
 

        private string GetCoulmnSize(object coulmnnSize)
        {
            string TableCoulmnnSize = null;
            double? size = 0;
            try { size = double.Parse((string)coulmnnSize); }
            catch (Exception e) { size = 0; }
            if (size != null && size != 0)
                if(size==-1)
                    TableCoulmnnSize =  " (Max) ";
                else
                    TableCoulmnnSize = " (" + size + ") ";

            return TableCoulmnnSize;
        }
        private string GetIsCoulmnNullable( object  coulmnnNullable)
        {
            string TableIsCoulmnNullable = null;
            bool? Nullable = true;
            try { Nullable = bool.Parse((string)coulmnnNullable); }
            catch (Exception e) { Nullable = true; }
            if (Nullable == true) { TableIsCoulmnNullable += " null "; }
            else { TableIsCoulmnNullable += " not null "; }

            return TableIsCoulmnNullable;
        }

        private string GetIsCoulmnIdentity(object coulmnnIdentity)
        {
            string TableIsCoulmnIdentity = null;
            bool? IsIdentity = false;
            try { IsIdentity = bool.Parse((string)coulmnnIdentity); }
            catch (Exception e) { IsIdentity = false; }
            if (IsIdentity == true) { TableIsCoulmnIdentity += " IDENTITY(1,1) "; }

            return TableIsCoulmnIdentity;
        }

        private  void SetPrimartKeyCoulmn(object coulmnnPrimarykey,string columnName)
        {
            bool? IsPrimartKey = false;
            try { IsPrimartKey = bool.Parse((string)coulmnnPrimarykey); }
            catch (Exception e) { IsPrimartKey = false; }
            if (IsPrimartKey == true) { primarykeyColumn = columnName; }
        }

        private string GetTableStructurePrimartKey(string tableName)
        {
            string TablePrimaryKey = null;

            if (!string.IsNullOrEmpty(primarykeyColumn))
            {
                TablePrimaryKey  = "CONSTRAINT[PK_" + tableName + "] PRIMARY KEY([" + primarykeyColumn + "]) )\n";
            }

            return TablePrimaryKey;
        }

        public string GetTableStructureIndexs(string tableName)
        {
            string TableIndexs = "";
            if (tableIndexs!=null)
            {
                for (int j = 0; j < tableIndexs.Rows.Count; j++)
                {
                    if (tableIndexs.Columns.Contains("Columns"))
                    {
                        string CoulmnIndexs = (string)tableIndexs.Rows[j]["Columns"];
                        TableIndexs += "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + CoulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "](" + CoulmnIndexs + ")\n";
                        TableIndexs += "ALTER INDEX [IX_" + tableName + "_" + CoulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "] DISABLE \n";

                    }
               
                }
            }
            TableIndexs += " End \n";
            return TableIndexs;
        }

        public string GetTableStructureReBuildIndexs(string tableName)
        {
            string TableIndexs = "";
            if (tableIndexs!=null)
            {
                for (int j = 0; j < tableIndexs.Rows.Count; j++)
                {
                    if (tableIndexs.Columns.Contains("Columns"))
                    {
                        string CoulmnIndexs = (string)tableIndexs.Rows[j]["Columns"];
                        TableIndexs += "ALTER INDEX [IX_" + tableName + "_" + CoulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "] REBUILD \n";

                    }
                        
                }

            }
           
            return TableIndexs;
        }

        public string GetTableStructureRelations(string tableName)
        {
            string TableStructureRelations = "";
            if (tableCoulmnsRelation!=null)
            {
                for (int j = 0; j < tableCoulmnsRelation.Rows.Count; j++)
                {
                    if (tableCoulmnsRelation.Columns.Contains("ForeignKeyColumn") &&
                        tableCoulmnsRelation.Columns.Contains("ReferencedTable") &&
                        tableCoulmnsRelation.Columns.Contains("ReferencedColumn"))
                    {
                        string ForeignKeyColumn = (string)tableCoulmnsRelation.Rows[j]["ForeignKeyColumn"];
                        string ReferencedTable = (string)tableCoulmnsRelation.Rows[j]["ReferencedTable"];
                        string ReferencedColumn = (string)tableCoulmnsRelation.Rows[j]["ReferencedColumn"];
                        TableStructureRelations += "ALTER TABLE [dbo].[" + tableName + "] ADD CONSTRAINT [FK_" + tableName + "_" + ReferencedTable + "_" + ForeignKeyColumn + "] FOREIGN KEY([" + ForeignKeyColumn + "]) REFERENCES [dbo].[" + ReferencedTable + "]([" + ReferencedColumn + "])\n";
                        TableStructureRelations += "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + ForeignKeyColumn + "] ON [dbo].[" + tableName + "]([" + ForeignKeyColumn + "])\n";
                    }
                }

            }
            
            return TableStructureRelations;
        }

        public string GetTableStructureChangeNameScript(string oldTableName, string newTableName)
        {
            string TableStructureChaneNameScript = "";
            TableStructureChaneNameScript = "EXEC sp_rename '" + oldTableName + "', '" + newTableName + "' \n ";
            TableStructureChaneNameScript += " exec sp_rename 'PK_" + oldTableName + "', 'PK_" + newTableName + "', 'object' \n ";
            TableStructureChaneNameScript += GetTableStructureChaneNameScriptFromRelations(oldTableName, newTableName);
            TableStructureChaneNameScript += GetTableStructureChaneNameScriptFromUniqueConstraints(oldTableName, newTableName);


            return TableStructureChaneNameScript;
        }
        private string GetTableStructureChaneNameScriptFromUniqueConstraints(string oldTableName, string newTableName)
        {
            string TableStructureChaneNameScriptFromUniqueConstraints= "";
            if (tableUniqueConstraints != null && tableUniqueConstraints.Rows != null && tableUniqueConstraints.Rows.Count > 0)
            {
                for (int j = 0; j < tableUniqueConstraints.Rows.Count; j++)
                {
                    if (tableUniqueConstraints.Columns.Contains("Columns")){
                        string UniqeConstraintFields = (string)tableUniqueConstraints.Rows[j]["Columns"];
                        TableStructureChaneNameScriptFromUniqueConstraints += " exec sp_rename 'UQ_" + oldTableName + "_" + UniqeConstraintFields.Replace(',', '_') + "', 'UQ_" + newTableName + "_" + UniqeConstraintFields.Replace(',', '_') + "', 'object' \n ";

                    }

                }
            }

            return TableStructureChaneNameScriptFromUniqueConstraints;
        }
        private string GetTableStructureChaneNameScriptFromRelations(string oldTableName, string newTableName)
        {
            string TableStructureChaneNameScriptFromRelations = "";
            if (tableCoulmnsRelation != null && tableCoulmnsRelation.Rows != null && tableCoulmnsRelation.Rows.Count > 0)
            {

                for (int j = 0; j < tableCoulmnsRelation.Rows.Count; j++)
                {
                    if (tableCoulmnsRelation.Columns.Contains("ForeignKeyColumn") &&
                           tableCoulmnsRelation.Columns.Contains("ReferencedTable") &&
                           tableCoulmnsRelation.Columns.Contains("ReferencedColumn"))
                    {
                        string ForeignKeyColumn = (string)tableCoulmnsRelation.Rows[j]["ForeignKeyColumn"];
                        string ReferencedTable = (string)tableCoulmnsRelation.Rows[j]["ReferencedTable"];
                        TableStructureChaneNameScriptFromRelations += " exec sp_rename 'FK_" + oldTableName + "_" + ReferencedTable + "_" + ForeignKeyColumn + "', 'FK_" + newTableName + "_" + ReferencedTable + "_" + ForeignKeyColumn + "', 'object' \n ";
                    }
                     
                }
            }

            return TableStructureChaneNameScriptFromRelations;
        }

        public string GetTableStructureUniqueConstraints(string tableName)
        {
            string UniqueConstraints = "";
            if (tableUniqueConstraints!=null)
            {
                for (int j = 0; j < tableUniqueConstraints.Rows.Count; j++)
                {
                    if (tableUniqueConstraints.Columns.Contains("Columns"))
                    {
                        string UniqeConstraintFields = (string)tableUniqueConstraints.Rows[j]["Columns"];
                        UniqueConstraints += "ALTER TABLE [dbo].[" + tableName + "] ADD CONSTRAINT [UQ_" + tableName + "_" + UniqeConstraintFields.Replace(',', '_') + "] UNIQUE(" + UniqeConstraintFields + ")\n";
                    }
                 
                }
            }
           
            return UniqueConstraints;
        }




    }
}
