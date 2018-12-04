using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddMenuButtonGroupAndMenuButtons
    {
        public static MenuButtonGroup AddMenuButtonGroup(MenuButtonGroupDetails menuButtonGroupDetails, MenuButtonGroupRepository menuButtonGroupRepository, Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups)
        {

            ObjectTableRepository Repo = new ObjectTableRepository(0);
            var table = Repo.GetSingleObjectTable(menuButtonGroupDetails.ObjectTableId, menuButtonGroupDetails.Tenant, false);
            string NewKey = "";
            if (ObjectTablesKeys.Keys.Keys.Contains(table.Name))
            {
                NewKey = ObjectTablesKeys.Keys[table.Name];
            }

            if (table.UpdateKey != NewKey || string.IsNullOrEmpty(NewKey))
            {
                if (tenantMenuButtonGroups.Keys.Contains(menuButtonGroupDetails.Name))
                {
                    MenuButtonGroup menuButtonGroup = tenantMenuButtonGroups[menuButtonGroupDetails.Name];
                    menuButtonGroup.ObjectTableId = menuButtonGroupDetails.ObjectTableId;
                    menuButtonGroup.Tenant = menuButtonGroupDetails.Tenant;
                    menuButtonGroupRepository.Update(menuButtonGroup);
                    //table.UpdateKey = NewKey;
                    //Repo.Update(table);
                    //Repo.SubmitChanges();
                    return menuButtonGroup;

                }
                else
                {
                    MenuButtonGroup newMenuButtonGroup = new MenuButtonGroup()
                    {
                        Tenant = menuButtonGroupDetails.Tenant,
                        ObjectTableId = menuButtonGroupDetails.ObjectTableId,
                        Id = IdCounter.GetNumber("MenuButtonGroup", menuButtonGroupDetails.Tenant).ToString(),
                        MenuButtonGroupType = menuButtonGroupDetails.MenuButtonGroupType,
                        Name = menuButtonGroupDetails.Name,

                    };
                    menuButtonGroupRepository.Add(newMenuButtonGroup);
                    //table.UpdateKey = NewKey;
                    //Repo.Update(table);
                    //Repo.SubmitChanges();
                    return newMenuButtonGroup;
                }
            }
            else
            {
                return tenantMenuButtonGroups[menuButtonGroupDetails.Name];
            }
        }

        public static MenuButton AddMenuButton(MenuButtonDetails menuButtonDetails, MenuButtonRepository menuButtonRepository, Dictionary<string, MenuButton> tenantMenuButtons, TextCodeRepository textCodeRepository, Dictionary<string, TextCode> textCodes)
        {
            ObjectTableRepository Repo = new ObjectTableRepository(0);
            var table = Repo.GetSingleObjectTable(menuButtonDetails.ObjectTableId, menuButtonDetails.Tenant, false);
            string NewKey = "";
            if (ObjectTablesKeys.Keys.Keys.Contains(table.Name))
            {
                NewKey = ObjectTablesKeys.Keys[table.Name];
            }

            if (table.UpdateKey != NewKey || string.IsNullOrEmpty(NewKey))
            {
                if (tenantMenuButtons.Keys.Contains(menuButtonDetails.EventCode + menuButtonDetails.MenuButtonGroupId))
                {
                    MenuButton menuButton = tenantMenuButtons[menuButtonDetails.EventCode + menuButtonDetails.MenuButtonGroupId];
                    menuButton.FeatureId = menuButtonDetails.FeatureId;
                    menuButton.Index = menuButtonDetails.Index;
                    menuButton.IsActive = menuButtonDetails.IsActive;
                    menuButton.ParentMenuButtonId = menuButtonDetails.ParentMenuButtonId;
                    menuButton.Tenant = menuButtonDetails.Tenant;
                    menuButton.MenuButtonType = menuButtonDetails.MenuButtonType;
                    menuButton.DropDownControl = menuButtonDetails.DropDownControl;
                    menuButton.Style = menuButtonDetails.Style;
                    menuButton.ControlPath = menuButtonDetails.ControlPath;
                    menuButton.Width = menuButtonDetails.Width;
                    menuButton.HtmlComponentPath = menuButtonDetails.HtmlComponentPath;
                    menuButtonRepository.Update(menuButton);

                    if (textCodes.Keys.Contains(menuButtonDetails.LabelTextCodeCode))
                    {
                        TextCode textCode = textCodes[menuButtonDetails.LabelTextCodeCode];
                        if (!textCode.IsSpellChecked)
                        {
                            textCode.DefaultText = menuButtonDetails.LabelTextCodeDefaultText;
                            textCode.LocalDefaultText = menuButtonDetails.LocalDefaultText;
                        }
                        textCode.InActive = false;
                        textCodeRepository.Update(textCode);
                    }
                    else if (textCodes.Keys.Contains(menuButtonDetails.LabelTextCodeCode + menuButtonDetails.Tenant + menuButtonDetails.ObjectTableId))
                    {
                        TextCode textCode = textCodes[menuButtonDetails.LabelTextCodeCode + menuButtonDetails.Tenant + menuButtonDetails.ObjectTableId];
                        if (!textCode.IsSpellChecked)
                        {
                            textCode.DefaultText = menuButtonDetails.LabelTextCodeDefaultText;
                            textCode.LocalDefaultText = menuButtonDetails.LocalDefaultText;
                        }
                        textCode.InActive = false;
                        textCodeRepository.Update(textCode);

                    }
                    else
                    {
                        var textCode = new TextCode()
                        {
                            DefaultText = menuButtonDetails.LabelTextCodeDefaultText,
                            Id = IdCounter.GetNumber("TextCode", menuButtonDetails.Tenant).ToString(),
                            TextCodeTypeCode = "B",
                            Code = menuButtonDetails.LabelTextCodeCode,
                            ObjectTableId = menuButtonDetails.ObjectTableId,
                            Tenant = menuButtonDetails.Tenant,
                            LocalDefaultText = menuButtonDetails.LocalDefaultText,
                            InActive = false
                        };

                        textCodeRepository.Add(textCode);
                    }
                    return menuButton;
                }
                else
                {
                    TextCode newTextCode = null;
                    if (textCodes.Keys.Contains(menuButtonDetails.LabelTextCodeCode))
                    {
                        newTextCode = textCodes[menuButtonDetails.LabelTextCodeCode];

                    }
                    else
                    {
                        newTextCode = new TextCode()
                        {
                            DefaultText = menuButtonDetails.LabelTextCodeDefaultText,
                            Id = IdCounter.GetNumber("TextCode", menuButtonDetails.Tenant).ToString(),
                            TextCodeTypeCode = "B",
                            Code = menuButtonDetails.LabelTextCodeCode,
                            ObjectTableId = menuButtonDetails.ObjectTableId,
                            Tenant = menuButtonDetails.Tenant,
                            LocalDefaultText = menuButtonDetails.LocalDefaultText,

                        };
                        textCodeRepository.Add(newTextCode);
                    }


                    MenuButton newMenuButton = new MenuButton()
                    {
                        Tenant = menuButtonDetails.Tenant,
                        ParentMenuButtonId = menuButtonDetails.ParentMenuButtonId,
                        IsActive = menuButtonDetails.IsActive,
                        Index = menuButtonDetails.Index,
                        FeatureId = menuButtonDetails.FeatureId,
                        MenuButtonGroupId = menuButtonDetails.MenuButtonGroupId,
                        EventCode = menuButtonDetails.EventCode,
                        MenuButtonType = menuButtonDetails.MenuButtonType,
                        DropDownControl = menuButtonDetails.DropDownControl,
                        ControlPath = menuButtonDetails.ControlPath,
                        HtmlComponentPath = menuButtonDetails.HtmlComponentPath,
                        Id = IdCounter.GetNumber("MenuButton", menuButtonDetails.Tenant).ToString(),
                        LabelTextCodeId = newTextCode.Id,
                        Style = menuButtonDetails.Style
                    };
                    menuButtonRepository.Add(newMenuButton);

                    return newMenuButton;
                }
            }
            else
            {
                return tenantMenuButtons[menuButtonDetails.EventCode + menuButtonDetails.MenuButtonGroupId];
            }
        }
    }


}
