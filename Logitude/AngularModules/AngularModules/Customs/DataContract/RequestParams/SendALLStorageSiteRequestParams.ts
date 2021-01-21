import { GenericRequestParams } from './GenericRequestParams';

export class SendALLStorageSiteRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public StorageSiteCode: string;
    public UnLoadPortCode: string;

}
