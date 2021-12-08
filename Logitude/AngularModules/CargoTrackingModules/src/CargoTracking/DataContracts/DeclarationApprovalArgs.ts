export class DeclarationApprovalArgs
{
    Tenant: number;
    ShipmentSecurityKey: string;
    Approved: boolean = false;
    Denied: boolean = false;
    DenyReason: string = "";
}
