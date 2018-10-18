import {GenericRequestParams} from './GenericRequestParams';

export class ClientSearchRequestParams extends GenericRequestParams {

    public ExternalId: string;
    public PassportNumber: string;
    public PassportTypeCode: string;
    public PassportCountryCode: string;

}