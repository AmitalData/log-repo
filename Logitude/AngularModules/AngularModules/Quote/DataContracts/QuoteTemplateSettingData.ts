
export class QuoteTemplateSettingData {
    public PricesPackagesTableSettings: PricesFieldSettings[];
    public PricesContainersTableSettings: PricesFieldSettings[];
    public PackagesSplitBy: string;
    public ContainersSplitBy: string;
}

export class PricesFieldSettings {
    public Name: string;
    public Code: string;
    public Index: number;
    public InUse: boolean;
    public Key: string;
    public Show: string;
    public IsShowArrowUpDown: boolean;
}
