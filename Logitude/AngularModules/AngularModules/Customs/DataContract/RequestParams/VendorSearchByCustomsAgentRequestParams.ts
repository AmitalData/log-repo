import {RequestParamsBase} from './RequestParamsBase';

export class VendorSearchByCustomsAgentRequestParams extends RequestParamsBase {
    VendorName: string;
    DunsNumber: number;
    CityName: string;
    CountryCode: string;
    MainAddressLine: string;
    PostalCode: string;
    SubCountryCode: string;
    isSearchPreviousName: number;
    LicensedDealerNumber: string;
    VendorNumber: number;
    VendorTypeCode: number;
    RecallSuppliersFromFileRequest: boolean;
    IsPalestinian: boolean;
}