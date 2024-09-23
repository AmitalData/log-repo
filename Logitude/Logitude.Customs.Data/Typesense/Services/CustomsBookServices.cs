using Logitude.Customs.Data.Typesence.Models;
using Simplog.Data.Typesense;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.Typesence.Services
{
    public class CustomsBookServices : TClientBase<CustomsItem>
    {
        public CustomsBookServices(string apiKey, string url) :

            base(
                apiKey,
                url,
                "CustomsBook",
                "ID",
                 new Field[]
                {
                    new Field("CB_ID", FieldType.String),
                    new Field("ID", FieldType.Int32),
                    new Field("CustomsItemID", FieldType.String),
                    new Field("FullClassification", FieldType.String),
                    new Field("IsLeaf", FieldType.Bool),
                    new Field("CustomsItemDetailsHistoryID", FieldType.Int32),
                    new Field("PropertiesDetailsHistoryID", FieldType.Int32),
                    new Field("PH_MeasurementUnitID", FieldType.Int32),
                    new Field("IsHistoryExists", FieldType.Bool),
                    new Field("IsRulesExists", FieldType.Bool),
                    new Field("StartDateInt", FieldType.Int64),                    
                    new Field("EndDateInt", FieldType.Int64),
                    new Field("CI_Parent_CustomsItemIDNum", FieldType.Int32),
                    new Field("CI_BaseFullClassification", FieldType.String),
                    new Field("CI_ComputedCheckDigit", FieldType.String),
                    new Field("CI_CustomsBookTypeIDNum", FieldType.String),
                    new Field("CI_CustomsItemCategoryIDNum", FieldType.String),
                    new Field("ItemHierarchicLocationID", FieldType.String),
                    new Field("CIH_Title", FieldType.String),
                    new Field("CIH_GoodsDescription", FieldType.String),
                    new Field("CustomsItemEntityStatusIDNum", FieldType.Int32),
                    new Field("PH_IsCarItem", FieldType.Bool),
                    new Field("FullGoodsDescription", FieldType.String)
                }
        )
        { }

        override public CustomsItem DataRowToObject(DataRow row)
        {
            CustomsItem customsBook = new CustomsItem
            {
                CB_ID = row["CB_ID"].ToString(),
                ID = GetIntDataRow(row, "ID"),
                CustomsItemID = row["CustomsItemID"].ToString(),
                FullClassification = row["FullClassification"].ToString(),
                IsLeaf = GetBooleanDataRow(row, "IsLeaf"),
                CustomsItemDetailsHistoryID = GetIntDataRow(row, "CustomsItemDetailsHistoryID"),
                PropertiesDetailsHistoryID = GetIntDataRow(row, "PropertiesDetailsHistoryID"),
                PH_MeasurementUnitID = GetIntDataRow(row, "PH_MeasurementUnitID"),
                IsHistoryExists = GetBooleanDataRow(row, "IsHistoryExists"),
                IsRulesExists = GetBooleanDataRow(row, "IsRulesExists"),
                StartDateInt = ((DateTime)row["StartDate"]).Ticks,
                EndDateInt = ((DateTime)row["EndDate"]).Ticks,
                CI_Parent_CustomsItemIDNum = GetIntDataRow(row, "CI_Parent_CustomsItemIDNum"),
                CI_BaseFullClassification = row["CI_BaseFullClassification"].ToString(),
                CI_ComputedCheckDigit = row["CI_ComputedCheckDigit"].ToString(),
                CI_CustomsBookTypeIDNum = row["CI_CustomsBookTypeIDNum"].ToString(),
                CI_CustomsItemCategoryIDNum = row["CI_CustomsItemCategoryIDNum"].ToString(),
                ItemHierarchicLocationID = row["ItemHierarchicLocationID"].ToString(),
                CIH_Title = row["CIH_Title"].ToString(),
                CIH_GoodsDescription = row["CIH_GoodsDescription"].ToString(),
                CustomsItemEntityStatusIDNum = GetIntDataRow(row, "CustomsItemEntityStatusIDNum"),
                PH_IsCarItem = GetBooleanDataRow(row, "PH_IsCarItem"),
                FullGoodsDescription = row["FullGoodsDescription"].ToString()
            };

            return customsBook;
        }

        public async Task<List<CustomsItem>> SearchCustomsAsync(string searchValue, string customsBookType)
        {
            string[] coloumnsForSearch = new string[] { "FullClassification", "CIH_GoodsDescription" };
            string filterBy = GetFilterBy(customsBookType);
            List<CustomsItem> responseList = await SearchAsync(searchValue, coloumnsForSearch, filterBy);
            return responseList;
        }

        public async Task<List<CustomsItem>> SearchByCustomsItemsAsync(string[] customsItemsIds, string customsBookType)
        {
            string filter = GetFilterBy(customsBookType);
            filter += $"&&CustomsItemID:=[{string.Join(",", customsItemsIds)}]";
            return await SearchByFilterOnlyAsync(filter);
        }

        private string GetFilterBy(string customsBookType) =>
            $"CI_CustomsItemCategoryIDNum:=1 && CustomsItemEntityStatusIDNum:=2 && CI_CustomsBookTypeIDNum:={customsBookType} && EndDateInt:>{DateTime.Now.Ticks} && StartDateInt:<{DateTime.Now.Ticks}";
    }
}