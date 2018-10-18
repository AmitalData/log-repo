using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddQuoteStage
    {
        public static void AddEntity(QuoteStageDetails dataDetails, QuoteStageRepository dataRepository, Dictionary<string, QuoteStage> dataDictionary)
        {
            if (dataDictionary.Keys.Contains(dataDetails.Code))
            {
                QuoteStage entity = dataDictionary[dataDetails.Code];
                entity.Name = dataDetails.Name;
                entity.MaxDays = dataDetails.MaxDays;
                //entity.InActive = dataDetails.InActive;

                dataRepository.Update(entity);
            }

            else
            {
                QuoteStage entity = new QuoteStage()
                {
                    Id = IdCounter.GetNumber("QuoteStage", dataDetails.Tenant).ToString(),
                    Tenant = dataDetails.Tenant,
                    Code = dataDetails.Code,
                    Name = dataDetails.Name,
                    MaxDays = dataDetails.MaxDays,
                    //InActive = dataDetails.InActive,
                    SearchFields = dataDetails.Code + "," + dataDetails.Name,
                };

                dataRepository.Add(entity);
            }

        }        
    }
}