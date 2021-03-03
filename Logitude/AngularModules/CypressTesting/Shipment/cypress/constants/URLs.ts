export class URLs
{
    public static readonly Shipment = '**/shipment';
    public static readonly SplitShipment = '**/SplitShipment';
    public static readonly CardViews = '**/cardviews/**';
    public static readonly AddressViews = '**/addressviews/**';
    public static readonly HtmlEditor = '**/HtmlEditor/**';
    public static readonly DocumentsFilingExtended = '**/DocumentsFilingExtended/**';
    public static readonly TraceEventsDomain = "**/TraceEventsDomain/GetTraceEventsForEntity?**"
    public static readonly ContainerFollowUpViewsGetbyfilters = "**/ContainerFollowUpViews/getbyfilters?**"

    //INTTRA
    public static readonly FTPDetails = "**/ftpdetails";
    public static readonly GetINTTRASettings = "**/INTTRADomain/GetINTTRASettings";
    public static readonly PutINTTRASettings = "**/INTTRADomain/PutINTTRASettings";
    public static readonly GetBookingMessageResultValidate = "**/INTRAWebService/GetBookingMessageResultValidate?**";
    public static readonly GetShippingInstructionMessageResultValidate = "**/INTRAWebService/GetMessageResultValidate?**";
    public static readonly INTTRAWebServiceSendEBooking = "**/INTRAWebService/GetSendEBooking?**";
    public static readonly INTRAWebServiceGetMessageResult = "**/INTRAWebService/GetMessageResult?**";
    public static readonly ContactViewsGetByFilters = "**/contactviews/getbyfilters?**";
    public static readonly ShipmentGetSingle="**/shipment/GetSingle?**"

    public static ShipmentviewsGetbyfilters(ShipmentNumber: string): string{
        return "**/shipmentviews/getbyfilters?**" + ShipmentNumber + "**";
    }
    public static readonly GetAll = "**/getall"
}