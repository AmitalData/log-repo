import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { GenericRequestParams } from './GenericRequestParams';

export class PendingRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public MAWB: string;
    public PendingCode: string[];
    public DeclarationsList: string[];
    public IsWorkSheetFromExcel: boolean;


}
