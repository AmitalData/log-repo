export class CargoTrackingShipmentFilters
{
    public Tenant: number;
    public SearchText: string;
    public CustomersIds: string[] = [];
    public CustomersIdsString: string;
    public TransportModeCodes: string;
    public DirectionCodes: string;
    public SortDescending: string;
    public SortFieldName: string;
    public SelectedInvitedCustomers: any[];
    public HasException: boolean = false;
}
