using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class BIReportXMLDataService
    {
        public BIReportXMLData GetByBIReportId(BIReportXMLDataServiceArgs bIReportXMLDataServiceArgs)
        {
            string Id = bIReportXMLDataServiceArgs.BIReportId;
            string dWQueryId = bIReportXMLDataServiceArgs.DWQueryId;
            int tenant = bIReportXMLDataServiceArgs.Tenant;

            BIReportQueryService query = new BIReportQueryService(tenant);
            BIReportPM entityPM = query.GetSingle(Id, false, false);
            BIReportXMLData QueryData = new BIReportXMLData();

            DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(tenant);
            DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePMByQueryid(dWQueryId, tenant);
            DWQueryData DWQueryData = new DWQueryData();
            DWQueryData.PageIndex = 0;
            DWQueryData.PageSize = 0;
            DWQueryData.FactTableName = entityPM.FactTableName;
            bool isUpdated = false;

            List<DWObjectFieldsDetails> Columns = null;
            if (dWSubQueryPM != null)
            {
                Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);
                DWQueryData.SubQueryData = dWSubQueryPM;
                DWQueryData.Columns = Columns;
                DWQueryData.Filters = bIReportXMLDataServiceArgs.FiltersData != null ? bIReportXMLDataServiceArgs.FiltersData : Filters;
            }
            QueryData.DWQueryData = DWQueryData;


            if (entityPM != null)
            {
                QueryData.BIReportPM = entityPM;
                QueryData.BIReportId = entityPM.Id;
                var sortingList = new List<Column>();

                if (!string.IsNullOrEmpty(entityPM.AGGridOptionsXML))
                {
                    var bITabularViewSettings = LogitudeXmlSerializer.DeserializeObject<BITabularViewSettings>(entityPM.AGGridOptionsXML);
                    if (bITabularViewSettings != null && Columns != null)
                    {
                        foreach (Column item in bITabularViewSettings.Columns.ToList())
                        {
                            if (item.SortDirction != null)
                            {
                                sortingList.Add(item);
                            }

                            var queryColumn = Columns.Where(a => a.DisplayName.Replace("[", "").Replace("]", "") == item.Code).FirstOrDefault();
                            if (queryColumn == null)
                            {
                                isUpdated = true;
                                bITabularViewSettings.Columns.RemoveAll(a => a.Code == item.Code);
                            }
                        }
                    }

                    if (sortingList != null && sortingList.Count() > 0)
                    {
                        foreach (Column item in sortingList.OrderBy(o => o.SortOrder).ToList())
                        {
                            QueryData.DWQueryData.ColumnsSort += "[" + item.Code + "]" + " " + item.SortDirction + ",";
                        }
                        QueryData.DWQueryData.ColumnsSort = QueryData.DWQueryData.ColumnsSort.TrimEnd(',');

                    }

                    foreach (var item in Columns)
                    {
                        var queryColumn = bITabularViewSettings.Columns.Where(a => a.Code == item.DisplayName.Replace("[", "").Replace("]", "")).FirstOrDefault();
                        if (queryColumn == null)
                        {
                            isUpdated = true;
                            bITabularViewSettings.Columns.Add(new Column
                            {
                                Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                                Name = item.DisplayName,
                                IsChecked = true,
                                Width = this.GetDefultColumWidthForBIReport(item.DisplayName),
                                DataTypeCode = item.DataTypeCode,
                                Index = bITabularViewSettings.Columns.Count == 0 ? 0 : bITabularViewSettings.Columns.Max(a => a.Index) + 1,
                                FieldCode = item.Code,
                            });
                        }
                        else
                        {
                            if (queryColumn.FieldCode != item.Code)
                            {
                                queryColumn.FieldCode = item.Code;
                                isUpdated = true;


                            }
                        }
                    }

                    QueryData.BITabularViewSettings = bITabularViewSettings;
                    if (isUpdated)
                    {
                        var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.BITabularViewSettings);
                        IInfrastructureContext objectContext = InfrastructureContext.GetContext(tenant);
                        BIReportRepository repository = new BIReportRepository(objectContext);
                        var entityPOCO = repository.GetSingle(entityPM.Id, entityPM.Tenant);
                        if (entityPM != null)
                        {
                            entityPOCO.AGGridOptionsXML = ColumnsXML;
                            repository.Update(entityPOCO);
                            repository.SubmitChanges();
                        }

                        isUpdated = false;
                    }
                }

                else
                {
                    var bITabularViewSettings = new BITabularViewSettings();
                    bITabularViewSettings.Columns = new List<Column>();
                    foreach (var item in Columns)
                    {
                        bITabularViewSettings.Columns.Add(new Column
                        {
                            Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                            Name = item.DisplayName,
                            IsChecked = true,
                            Width = GetDefultColumWidthForBIReport(item.DisplayName.Replace("[", "").Replace("]", "")),
                            DataTypeCode = item.DataTypeCode,
                            FieldCode = item.Code,

                        });
                    }
                    QueryData.BITabularViewSettings = bITabularViewSettings;
                    var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.BITabularViewSettings);
                    IInfrastructureContext objectContext = InfrastructureContext.GetContext(tenant);
                    BIReportRepository repository = new BIReportRepository(objectContext);
                    var entityPOCO = repository.GetSingle(entityPM.Id, entityPM.Tenant);
                    if (entityPM != null)
                    {
                        entityPOCO.AGGridOptionsXML = ColumnsXML;
                        repository.Update(entityPOCO);
                        repository.SubmitChanges();
                    }
                }
            }
            else
            {
                var bITabularViewSettings = new BITabularViewSettings();
                bITabularViewSettings.Columns = new List<Column>();
                foreach (var item in Columns)
                {
                    bITabularViewSettings.Columns.Add(new Column
                    {
                        Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                        Name = item.DisplayName,
                        IsChecked = true,
                        Width = GetDefultColumWidthForBIReport(item.DisplayName.Replace("[", "").Replace("]", "")),
                        DataTypeCode = item.DataTypeCode,
                        FieldCode = item.Code,

                    });
                }
                QueryData.BITabularViewSettings = bITabularViewSettings;
            }

            return QueryData;
        }

        private int GetDefultColumWidthForBIReport(string headerName)
        {
            int columWidth = 0;
            int per = 8;
            foreach (char character in headerName)
            {
                columWidth += per;
            }
            if (columWidth < 150) columWidth = 150;

            return columWidth;

        }
    }

    public class BIReportXMLDataServiceArgs
    {
        public string BIReportId { get; set; }
        public string DWQueryId { get; set; }
        public int Tenant { get; set; }
        public DWObjectFieldsDetails FiltersData { get; set; }
    }
}