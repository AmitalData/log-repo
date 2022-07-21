import { GenericRequestParams } from './GenericRequestParams';

export class SendMultiUpdateRequestParams extends GenericRequestParams {
    Declarationid: string;
    ProcessTypeCode: string;
    TaxExemptCode: string;
    ClassificationCode: string;
    DeclarationIds: string[];
    CourierMasterId: string;
    allWithoutdeclarationIdsList: string[];
    checkboxAll: boolean;
}
