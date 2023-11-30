export class DeclarationApprovalArgs
{
    Tenant: number;
    ShipmentSecurityKey: string;
    ApprovedBy: string;
    Approved: boolean = false;
    Denied: boolean = false;
    DenyReason: string = "";
}
