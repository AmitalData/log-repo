import {GenericRequestParams} from './GenericRequestParams';

export class GatepassRequestMessageRequestParams extends GenericRequestParams {

    public MasterCourierId: string;
    public OriginSiteCode: string;
    public DesignateSiteCode: string;
    public UpdateCode: string;
    public TransportationTypeCode: string;
}
