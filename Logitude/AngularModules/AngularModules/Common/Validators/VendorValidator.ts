import { AppTool } from "Infrastructure/Tools";

export class VendorValidator {
    public  Validate(entityPM: any) {


        return [];
    }

    public IsVendorCountryValid(VendorCountry : string) : boolean {
        return !AppTool.IsNullOrUndefined(VendorCountry) && VendorCountry !== "1" && VendorCountry !== "--" && !AppTool.IsNullOrEmpty(VendorCountry);
    }
}