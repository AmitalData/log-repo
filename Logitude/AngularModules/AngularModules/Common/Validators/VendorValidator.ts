import { GLAccountPMService } from "Accounting/Services/StandardPMs/GLAccountPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { AppTool } from "Infrastructure/Tools";

export class VendorValidator {
    public  Validate(entityPM: any) {


        return [];
    }
    gLAccountPMService: GLAccountPMService = new GLAccountPMService();

    public async IsVendorCountryValid(value: string,VendorCountry:string): Promise<boolean> {
        let isValid = false;

        try {
            if(!this.ValidateVendorCountry(VendorCountry)){
                const result: ServiceResponse = await this.gLAccountPMService.get(value).toPromise();
                const entity = result?.Result;
                if (entity) {
                    if(!AppTool.IsNullOrEmpty(entity.CardId) ||  !AppTool.IsNullOrEmpty(entity.ParentCurrencyGLAccountCardId)){
                       const VendorCountry = entity.CardCountryCode;
                       isValid = this.ValidateVendorCountry(VendorCountry);
                    }
                }
            }
           
            else{
                isValid = true;
            }
           
        } catch (error) {
            console.error("Error validating vendor country:", error);
            isValid = true;
        }

        return isValid;
    }
    public ValidateVendorCountry(VendorCountry:string) {
        return !AppTool.IsNullOrUndefined(VendorCountry) && VendorCountry !== "1" && VendorCountry !== "--" && !AppTool.IsNullOrEmpty(VendorCountry);
    }

}