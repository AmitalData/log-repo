import {GenericRequestParams} from './GenericRequestParams';

export class ImporterDeclarationRequestParams extends GenericRequestParams {
    public IsByType: boolean;
    public IsByExpireDate: boolean;
    public ImporterNumber: string;
    public DeclarationExpire: Date;
    public DeclarationConect: string;
    public Code: string;
    public FromDate: Date;
    public ToDate: Date;
    public JoinCustomsVendors: boolean;
    

}
