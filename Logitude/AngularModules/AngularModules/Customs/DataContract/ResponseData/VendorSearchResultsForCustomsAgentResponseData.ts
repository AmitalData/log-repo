import {ResponseDataBase} from './ResponseDataBase';
import {VendorCommunicationResult} from './VendorCommunicationResult';
export class VendorSearchResultsForCustomsAgentResponseData extends ResponseDataBase
{
    NumberOfResult: number;
    VendorResults: VendorResult[];
}

export class VendorResult {
    Id: string;
    Tenant : number;
    StatusCode: string;
    CityName: string;
    CountryCode: string;
    DunsNumber: string;
    MainAddressLine: string;
    PostalCode: string;
    SubCountryCode: string;
    VendorName: string;
    VendorTypeCode: string;
    CountryName: string;
    SubCountryName: string;
    VendorNumber: string;
    VATNumber: string;
    Exists : boolean;
    InActive : boolean;
    IsPalestinian : boolean;
    VendorCommunications: VendorCommunicationResult[];
    VendorTypeName: string;
    StatusName: string;
}
