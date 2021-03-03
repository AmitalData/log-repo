export class BaseURLs
{
    public static readonly GetByCompactFilters = '**/GetByCompactFilters?**';
    public static readonly GetByFilters = '**/getbyfilters?**';
    public static readonly GetMenuButtonGroups = "**/ngMetaData/getmenubuttongrouppms?**";
    public static readonly Contacts = "**/contacts";

    public static GetQuickSearch(ShipmentNumber: string): string{
        return "**/GetQuickSearch?**" + ShipmentNumber;
    }
}