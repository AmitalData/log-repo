export class Urls
{
    public static readonly Tariffs = "**/tariffs";
    public static readonly TariffDomainGetRecentTariffs = "**/TariffDomain/GetRecentTariffs";
    public static readonly GetAllTariffVersionsForTariff = "**/tariffversionextended/GetAllTariffVersionsForTariff?**";
    public static readonly GetTariffVersionLines = "**/TariffDomain/GetTariffVersionLines?**";
    public static readonly GetSingleTariff = "**/tariffs/getsingle?**";
    public static readonly PostAvailableTariffs = "**/TariffDomain/PostAvailableAirlineFreightTariffs";
    public static readonly PostUpdateSurcharge = "**/PostUpdateSurcharge";
    public static readonly PostShippingLine = "**/shippinglines";
    public static readonly PostUploadExcelFile = "**/PostUploadExcelFile";
    public static readonly GetCarrierViews = "**/carrierviews/**";
    public static readonly GetCardviews = "**/cardviews/**";
    public static readonly GetEntityResource = "**/EntityResource?objectTableName=ShippingLine&tenant=0";
    public static readonly GetDownloadTariff = "**/GetDownloadTariff?**";

}