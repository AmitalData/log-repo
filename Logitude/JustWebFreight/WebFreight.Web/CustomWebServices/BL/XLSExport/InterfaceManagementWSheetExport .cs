using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using System.Runtime.Remoting.Contexts;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class InterfaceManagementWSheetExport
    {
        public byte[] ExportReport(int tenant)
        {



            //private IQueryable<InterfaceManagementList> GetIqueryableList(IQueryable<InterfaceManagement> iQueryable) {

            ICustomContext MyContext = CustomContext.GetContext(tenant);
            InterfaceTenantDefinitionRepository definitionRep = new InterfaceTenantDefinitionRepository(MyContext);
            InterfaceTenantDefinition definition = null;
            IQueryable<InterfaceTenantDefinition> interfaceManagementDefinitions = definitionRep.GetAll(tenant);


            IQueryable<InterfaceManagementList> query = (from a in MyContext.InterfaceManagements.Include("InterfaceSendOption").Include("SignatureType")

                                                         join d in interfaceManagementDefinitions.Include("InterfaceSendOption")
                                                         on a.Code equals d.Code into xy
                                                         from s in xy.DefaultIfEmpty()

                                                         select new InterfaceManagementList()
                                                         {

                                                             Code = a.Code,
                                                             DefaultPriority = a.DefaultPriority,
                                                             Description = a.Description,
                                                             InOut = a.InOut,
                                                             DefaultSendOptionName = a.InterfaceSendOption != null ? a.InterfaceSendOption.LocalName : null,
                                                             SignatureTypeName = a.SignatureType.LocalName,

                                                             HasDefinition = s.Id != null ? true : false,
                                                             InterfaceType = a.InterfaceType,
                                                             InterfaceTypeName = a.InterfaceType == "C" ? "Customs" : a.InterfaceType == "B" ? "Courier" : "All"


                                                         });
            



            DataTable dt = null;
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            dt = new DataTable("Interface Managements");


            settingCol.Columns.Add(new Column() { Index = 1, Code = "Code", Name = "Code", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "מספר מסר", ColumnName = "Code", DataType = System.Type.GetType("System.String"), });


            dt.Columns.Add(new DataColumn() { Caption = "תיאור מסר", ColumnName = "Description", DataType = System.Type.GetType("System.String") });
            settingCol.Columns.Add(new Column() { Index = 2, Code = "Description", Name = "Description", DataTypeCode = "String", Width = 150, });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "InOut", Name = "InOut", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "נכנס / יוצא", ColumnName = "InOut", DataType = System.Type.GetType("System.String") });


            settingCol.Columns.Add(new Column() { Index = 4, Code = "DefaultSendOptionName", Name = "DefaultSendOptionName", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "אפשרויות שליחה", ColumnName = "DefaultSendOptionName", DataType = System.Type.GetType("System.String") });


            settingCol.Columns.Add(new Column() { Index = 5, Code = "DefaultPriority", Name = "DefaultPriority", DataTypeCode = "Int32", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "עדיפות שליחה / ניתוח", ColumnName = "DefaultPriority", DataType = System.Type.GetType("System.Int32") });



            settingCol.Columns.Add(new Column() { Index = 6, Code = "HasDefinition", Name = "HasDefinition", DataTypeCode = "Boolean", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "יש הגדרה", ColumnName = "HasDefinition", DataType = System.Type.GetType("System.Boolean") });


            settingCol.Columns.Add(new Column() { Index = 7, Code = "SignatureTypeName", Name = "SignatureTypeName", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = "סוג חתימה", ColumnName = "SignatureTypeName", DataType = System.Type.GetType("System.String") });


            settingCol.Columns.Add(new Column() { Index = 8, Code = "InterfaceTypeName", Name = "InterfaceTypeName", DataTypeCode = "String", Width = 80, });
            dt.Columns.Add(new DataColumn() { Caption = "פעיל בסוג חברה", ColumnName = "InterfaceTypeName", DataType = System.Type.GetType("System.String") });




            var l = query.ToList();
            l.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.Code;
                newrow[1] = r.Description;
                newrow[2] = r.InOut;
                newrow[3] = r.DefaultSendOptionName;
                newrow[4] = r.DefaultPriority;
                newrow[5] = r.HasDefinition;
                newrow[6] = r.SignatureTypeName;
                newrow[7] = r.InterfaceTypeName;

                dt.Rows.Add(newrow);

            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);



            return res;

        }


        }

    } 