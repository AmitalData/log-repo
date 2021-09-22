export class CargoTrackingShipmentFilters
{
    public Tenant: number;
    public SearchText: string;
    public CustomersIds: string[] = [];
    public CustomersIdsString: string;
    public MilestonesStatus: string;
    public SelectedMilestonesStatus: any[] = [];
    public TransportModeCodes: string;
    public DirectionCodes: string;
    public SortDescending: boolean = true;
    public SortFieldName: string;
    public SelectedInvitedCustomers: any[];
}
