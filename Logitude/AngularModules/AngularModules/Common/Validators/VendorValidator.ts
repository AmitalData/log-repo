export class VendorValidator {
    public  Validate(entityPM: any) {


        return [];
    }

    public IsVendorCountryValid(VendorCountry : string) : boolean {
        return VendorCountry !== null && VendorCountry !== "1" && VendorCountry !== "--";
    }
}