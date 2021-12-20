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
    public SortDescending: string;
    public SortFieldName: string;
    public SelectedInvitedCustomers: any[];
    public HasException: boolean = false;
    public OrdersOnly: boolean = false;
    public EstimatedArrivalOnly: boolean = false;
    public OperationalClosedOnly: boolean = false;
}
