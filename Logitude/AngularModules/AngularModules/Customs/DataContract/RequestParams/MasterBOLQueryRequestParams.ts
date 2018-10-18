import {GenericRequestParams} from './GenericRequestParams';

export class MasterBOLQueryRequestParams extends GenericRequestParams {

    public CustomFileNo: string;
    public Date: string;
    public MasterBillOfLading: string;
    public InternalIdentifier: string;
    public ReturnAllInernalCargos: boolean;
    public ExactMatch: boolean;
    public DeclarationId: string;
}