import { RegexSelectors } from "./RegexSelectors"

export class SettingsSelectors extends RegexSelectors{
     //#region Currency 
     public static readonly SettingButton = "img[src='./Images/Icons/Settings.png']"
     public static readonly RatesTableDate = "#date_RatesTable_ValueDate"
     public static readonly RatesTableRate = "#RatesTable_Rate"
     
     //#endregion
}
