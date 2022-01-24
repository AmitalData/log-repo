import { GenericRequestParams } from "./GenericRequestParams";

export class AddMultiPendingsRequestParams extends GenericRequestParams {
    public CourierMasterId: string;
    public PendingCode: string;
    public DeclarationsList:string[];
}
