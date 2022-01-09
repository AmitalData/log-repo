export class MoreFilter{
    public HasException: boolean = false;
    public OrdersOnly: boolean = false;
    public EstimatedArrivalOnly: boolean = false;
    public OperationalOpenedOnly: boolean = false;
}
export class CargoTrackingShipmentSearchInput extends MoreFilter
{
    public Tenant: number;
    public SearchText: string;
    public CustomersIds: string[] = [];
    public MilestonesCodes: string[] = [];
    public TransportModeCodes: string[] = [];
    public DirectionCodes: string[] = [];
    public SortType: string;
    public SortFieldName: string;
    public PageIndex: number;
    public PageSize: number;

}

