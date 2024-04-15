import {Customer} from "./CargoTrackingShipmentDataSource";

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
    public Customers: Customer[] = [];
    public MilestonesCodes: string[] = [];
    public OpenDateGreaterThan: string = "";
    public ClearanceDateGreaterThan: string = "";
    public ATADateGreaterThan: string = "";
    public OpenDateLessThan: string = "";
    public ClearanceDateLessThan: string = "";
    public ATADateLessThan: string = "";
    public TransportModeCodes: string[] = [];
    public DirectionCodes: string[] = [];
    public SortType: string;
    public SortFieldName: string;
    public PageIndex: number;
    public PageSize: number;
    public FromDate: Date;
    public ToDate: Date;

}

