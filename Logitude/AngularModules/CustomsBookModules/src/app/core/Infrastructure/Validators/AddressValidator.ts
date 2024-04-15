import {AddressPM} from '../../Common/EntityPMs/AddressPM';
import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {AppTool, FormatTool} from '../Tools';

export class AddressValidator {
    public static IsMainAddressEnglishCharacters(entityPM: AddressPM) {
        var isValid = true;

        if (entityPM.AddressTypeId == "M") {           

            if (!AppTool.IsNullOrEmpty(entityPM.Name) && !FormatTool.IsEnglishText(entityPM.Name)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Description) && !FormatTool.IsEnglishText(entityPM.Description)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address1) && !FormatTool.IsEnglishText(entityPM.Address1)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address2) && !FormatTool.IsEnglishText(entityPM.Address2)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.City) && !FormatTool.IsEnglishText(entityPM.City)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.ATTN) && !FormatTool.IsEnglishText(entityPM.ATTN)) {
                isValid = false;
            }
        }

        return isValid;
    }

    public static IsMainAddressEnglishCharacters_Potential(entityPM: CustomerPM) {
        var isValid = true;

        if (!entityPM.IsLocalLanguage) {
            if (!AppTool.IsNullOrEmpty(entityPM.EnglishName) && !FormatTool.IsEnglishText(entityPM.EnglishName)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address1_Potential) && !FormatTool.IsEnglishText(entityPM.Address1_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address2_Potential) && !FormatTool.IsEnglishText(entityPM.Address2_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.City_Potential) && !FormatTool.IsEnglishText(entityPM.City_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.ATTN_Potential) && !FormatTool.IsEnglishText(entityPM.ATTN_Potential)) {
                isValid = false;
            }
        }

        return isValid;
    }
}