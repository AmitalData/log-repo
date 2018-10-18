import {RequestParamsBase} from './RequestParamsBase';
import {VendorCommunicationResult} from '../ResponseData/VendorCommunicationResult';
export class VendorInsertUpdateDeleteMessageRequestParams extends RequestParamsBase{

    OperationType: OperationTypes;
    VendorNumber : string;
    VendorTypeCode : string;
    VendorName : string;
    CountryCode : string;
    SubCountryCode : string;
    CityName : string;
    MainAddressLine : string;
    PostalCode : string;
    DunsNumber : string;
    VATNumber : string;
    StatusCode : string;
    TransactionTypeID: string;
    IsPalestinian: boolean;
    InActive: boolean;
    ExternalId : string;
    ConcurrencyGUID : string;
    IsAfterWarning : boolean;
    CommunicationDevices: VendorCommunicationResult[];

}

export enum OperationTypes {
    Add = 1,
    Update = 2,
    Delete = 3,
}