import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { GenericRequestParams } from './GenericRequestParams';

export class SendClosePendingRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public PendingCode: string;
    public DeclarationsList:string[];
}
