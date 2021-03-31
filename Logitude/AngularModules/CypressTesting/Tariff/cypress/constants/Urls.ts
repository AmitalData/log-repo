import { version } from "cypress/types/bluebird";

export class Urls
{
    public static readonly Tariffs = "**/tariffs";
    public static readonly TariffDomainGetRecentTariffs = "**/TariffDomain/GetRecentTariffs";
    public static readonly GetAllTariffVersionsForTariff = "**/tariffversionextended/GetAllTariffVersionsForTariff?**"; //1
    public static readonly GetTariffVersionLines = "**/TariffDomain/GetTariffVersionLines?**"; //2
    public static readonly GetSingleTariff = "**/tariffs/getsingle?**";
    public static readonly PostAvailableTariffs = "**/TariffDomain/PostAvailableAirlineFreightTariffs";
    public static readonly PostUpdateSurcharge = "**/PostUpdateSurcharge";
    public static readonly PostShippingLine = "**/shippinglines";
    public static readonly PostUploadExcelFile = "**/PostUploadExcelFile";
    public static readonly GetCarrierViews = "**/carrierviews/**";
    public static readonly GetCardviews = "**/cardviews/**";
    public static readonly GetEntityResource = "**/EntityResource?objectTableName=ShippingLine&tenant=0";
    public static readonly GetDownloadTariff = "**/GetDownloadTariff?**";

    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static GetTariffVersionLinesByVersion(version: string) {
        return "**/TariffDomain/GetTariffVersionLines?**version="+version
    }

    
}