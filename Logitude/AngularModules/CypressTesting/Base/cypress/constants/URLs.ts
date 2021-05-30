export class BaseURLs
{
    public static readonly GetByCompactFilters = '**/GetByCompactFilters?**';
    public static readonly GetByFilters = '**/getbyfilters?**';
    public static readonly GetMenuButtonGroups = "**/ngMetaData/getmenubuttongrouppms?**";
    public static readonly Contacts = "**/contacts";
    public static readonly Warehouses = "**/warehouses";
    public static readonly InfoIconImage="**/Images/InfoIcon.png"
    public static readonly PostSendhtmlDocument = '**/HtmlEditor/postsendhtmldocument';

    public static GetQuickSearch(ShipmentNumber: string): string{
        return "**/GetQuickSearch?**" + ShipmentNumber;
    }

    public static GetFilterSearch(filterBy:string){
        return '**/warehouseviews/getbyfilters?**'+filterBy+'**'
    }
    
    public static readonly GetTraceEventsForEntity = "**/TraceEventsDomain/GetTraceEventsForEntity?**";
}