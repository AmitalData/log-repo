using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddHelpResources
    {
        public static void AddHelpResource(HelpResourceDetails helpDetails, HelpResourceRepository helpResourceRepository, Dictionary<string, HelpResource> tenantHelpResources)
        {
            if (tenantHelpResources.Keys.Contains(helpDetails.Code))
            {
                HelpResource help = tenantHelpResources[helpDetails.Code];
                help.Code = helpDetails.Code;
                help.Name = helpDetails.Name;
                help.Language = helpDetails.Language;
                help.Type = helpDetails.Type;
                help.Category = helpDetails.Category;
                help.VideoURL = helpDetails.VideoURL;
                help.Duration = helpDetails.Duration;
                help.FileName = helpDetails.FileName;
                help.FeatureCode = helpDetails.FeatureCode;
                help.SearchFields = helpDetails.FileName;

                if (helpDetails.IsNew)
                {
                    help.UpdateDate = TenantServerConfigration.GetCurrentDateTime(0);
                    help.IsNew = helpDetails.IsNew;
                }

                else
                {
                    help.IsNew = false;
                }

                helpResourceRepository.Update(help);
            }
            else
            {
                HelpResource newHelp = new HelpResource()
                {
                    Code = helpDetails.Code,
                    Name = helpDetails.Name,
                    Language = helpDetails.Language,
                    Type = helpDetails.Type,
                    Category = helpDetails.Category,
                    VideoURL = helpDetails.VideoURL,
                    Duration = helpDetails.Duration,
                    FileName = helpDetails.FileName,
                    FeatureCode = helpDetails.FeatureCode,
                    SearchFields = helpDetails.FileName,
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    IsNew = helpDetails.IsNew,
                };

                helpResourceRepository.Add(newHelp);
            }
        }
    }
}