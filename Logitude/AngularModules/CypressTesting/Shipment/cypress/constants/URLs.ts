export class URLs
{
    public static readonly Shipment = '**/shipment';
    public static readonly CardViews = '**/cardviews/**';
    public static readonly AddressViews = '**/addressviews/**';
    public static readonly HtmlEditor = '**/HtmlEditor/**';
    public static readonly DocumentsFilingExtended = '**/DocumentsFilingExtended/**';

    public static ShipmentviewsGetbyfilters(ShipmentNumber: string): string{
        return "**/shipmentviews/getbyfilters?**" + ShipmentNumber + "**";
    }
}