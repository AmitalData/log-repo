import { GLAccountPMService } from "Accounting/Services/StandardPMs/GLAccountPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { AppTool } from "Infrastructure/Tools";

export class VendorValidator {
    public  Validate(entityPM: any) {


        return [];
    }
    gLAccountPMService: GLAccountPMService = new GLAccountPMService();

    public async IsVendorCountryValid(value: string,VendorCountry:string): Promise<boolean> {
        let isValid = true;

        try {
            if(!(!AppTool.IsNullOrUndefined(VendorCountry) && VendorCountry !== "1" && VendorCountry !== "--" && !AppTool.IsNullOrEmpty(VendorCountry))){
                const result: ServiceResponse = await this.gLAccountPMService.get(value).toPromise();
                const entity = result.Result;
                if (entity) {
                    const VendorCountry = entity.CardCountryCode;
                    isValid = !AppTool.IsNullOrUndefined(VendorCountry) && VendorCountry !== "1" && VendorCountry !== "--" && !AppTool.IsNullOrEmpty(VendorCountry);
                }
            }
           
           
        } catch (error) {
            console.error("Error validating vendor country:", error);
            isValid = true;
        }

        return isValid;
    }


}