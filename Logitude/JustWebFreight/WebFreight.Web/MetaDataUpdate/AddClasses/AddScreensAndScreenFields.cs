using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddScreensAndScreenFields
    {
        public static Screen AddScreen(ScreenDetails screenDetails, ScreensRepository screenRepository,Dictionary<string,Screen>tenantZeroScreens)
        {
            if (tenantZeroScreens.Keys.Contains(screenDetails.Code + screenDetails.ObjectTableId))
            {
                Screen screen = tenantZeroScreens[screenDetails.Code + screenDetails.ObjectTableId];
                screen.IsReadOnly = screenDetails.IsReadOnly;
                screen.NumberOfColumns = screenDetails.NumberOfColumns;
                screen.NumberOfRows = screenDetails.NumberOfRows;
                screen.Name = screenDetails.Name;


                screenRepository.Update(screen);
                return screen;
            }
            else
            {
                Screen newScreen = new Screen()
                {
                    NumberOfRows = screenDetails.NumberOfRows,
                    NumberOfColumns = screenDetails.NumberOfColumns,
                    IsReadOnly = screenDetails.IsReadOnly,
                    Code = screenDetails.Code,
                    Name=screenDetails.Name,
                    Id = IdCounter.GetNumber("Screen",screenDetails.Tenant).ToString(),
                    ObjectTableId = screenDetails.ObjectTableId,
                    Tenant = 0,

                };
                screenRepository.Add(newScreen);
                return newScreen;

            }
        }

        public static ScreenField AddScreenField(ScreenFieldDetails screenFieldDetails, ScreenFieldsRepository screenFieldsRepository, Dictionary<string, ScreenField> tenantScreenFields)
        {
            if (tenantScreenFields.Keys.Contains(screenFieldDetails.ScreenCode + screenFieldDetails.ObjectFieldCode))
            {
                ScreenField screenfield = tenantScreenFields[screenFieldDetails.ScreenCode + screenFieldDetails.ObjectFieldCode];
                screenfield.Column = screenFieldDetails.Column;
                screenfield.Row = screenFieldDetails.Row;
                
                screenFieldsRepository.Update(screenfield);
                return screenfield;
            }
            else
            {
                ScreenField newscreenfield = new ScreenField()
                {
                    Row = screenFieldDetails.Row,
                    Column = screenFieldDetails.Column,
                    Id = IdCounter.GetNumber("ScreenField", screenFieldDetails.Tenant).ToString(),
                    ObjectFieldId = screenFieldDetails.ObjectFieldId,
                    ScreenId = screenFieldDetails.ScreenId,
                    ScreenCode = screenFieldDetails.ScreenCode,
                    Tenant = screenFieldDetails.Tenant,
                    ObjectFieldCode = screenFieldDetails.ObjectFieldCode,

                };
                screenFieldsRepository.Add(newscreenfield);
                return newscreenfield;
            }
        }
    }
}