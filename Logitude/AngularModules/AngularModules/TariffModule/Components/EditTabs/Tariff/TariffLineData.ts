import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { AppTool, FontTool, FormatTool } from '../../../../Infrastructure/Tools';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { VersionTabComponent } from './VersionTabComponent';
import { SurchargeVersionTabComponent } from './SurchargeVersionTabComponent';
import { OceanFCLVersionTabComponent } from './OceanFCLVersionTabComponent';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';

export class AirCostTariffLineData extends BaseComponent {
    
    public EntityPM: TariffLinePM;
    public DataContext: AirCostTariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;

    constructor(entity: TariffLinePM, public FatherComponent: VersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.SetUIProperties();
        this.SetCellColorsForPriceCheck();
    }
    
    public CellColor: string = "transparent";
    private SetCellColorsForPriceCheck() {
        if (!AppTool.IsNullOrEmpty(this.FatherComponent.LineIdFromPriceCheck) && this.FatherComponent.LineIdFromPriceCheck == this.EntityPM.Id) {
            this.CellColor = "#f7dc6e";
        }

        else {
            if (this.IsEditEnabled) {
                this.CellColor = "transparent";                
            }

            else {
                this.CellColor = "rgba(230, 231, 232, 0.5)";
            }
        }
    }

    public MinPriceComparingPrice: number;
    public MinPriceComparingTextColor: string = null;
    public Step1ComparingPrice: number;
    public Step1ComparingTextColor: string = null;
    public Step2ComparingPrice: number;
    public Step2ComparingTextColor: string = null;
    public Step3ComparingPrice: number;
    public Step3ComparingTextColor: string = null;
    public Step4ComparingPrice: number;
    public Step4ComparingTextColor: string = null;
    public Step5ComparingPrice: number;
    public Step5ComparingTextColor: string = null;
    public Step6ComparingPrice: number;
    public Step6ComparingTextColor: string = null;
    public Step7ComparingPrice: number;
    public Step7ComparingTextColor: string = null;
    public Step8ComparingPrice: number;
    public Step8ComparingTextColor: string = null;
    private DefaultColor = "blue";

    SetCellsComparingText() {
        if (this.ComparedEntity != null) {
            var value1 = AppTool.IsNullOrZero(this.MinPrice) ? 0 : this.MinPrice;
            var value2 = AppTool.IsNullOrZero(this.ComparedEntity.MinPrice) ? 0 : this.ComparedEntity.MinPrice;
            var minPriceComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(minPriceComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.MinPriceComparingPrice = AppTool.Round((minPriceComparingValue / value2) * 100, 2);
                this.MinPriceComparingTextColor = this.ComputeWarningPercentageColor(this.MinPriceComparingPrice);
            }
            else {
                this.MinPriceComparingPrice = null;
                this.MinPriceComparingTextColor = this.DefaultColor;
            }
            // step 1
            value1 = AppTool.IsNullOrZero(this.Step1Price) ? 0 : this.Step1Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step1Price) ? 0 : this.ComparedEntity.Step1Price;
            var step1ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step1ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step1ComparingPrice = (step1ComparingValue / value2) * 100;
                this.Step1ComparingTextColor = this.ComputeWarningPercentageColor(this.Step1ComparingPrice);
            }
            else {
                this.Step1ComparingPrice = null;
                this.Step1ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step2Price) ? 0 : this.Step2Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step2Price) ? 0 : this.ComparedEntity.Step2Price;
            var step2ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step2ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step2ComparingPrice = (step2ComparingValue / value2) * 100;
                this.Step2ComparingTextColor = this.ComputeWarningPercentageColor(this.Step2ComparingPrice);
            }
            else {
                this.Step2ComparingPrice = null;
                this.Step2ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step3Price) ? 0 : this.Step3Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step3Price) ? 0 : this.ComparedEntity.Step3Price;
            var step3ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step3ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step3ComparingPrice = (step3ComparingValue / value2) * 100;
                this.Step3ComparingTextColor = this.ComputeWarningPercentageColor(this.Step3ComparingPrice);
            }
            else {

                this.Step3ComparingPrice = null;
                this.Step3ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step4Price) ? 0 : this.Step4Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step4Price) ? 0 : this.ComparedEntity.Step4Price;
            var step4ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step4ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step4ComparingPrice = (step4ComparingValue / value2) * 100;
                this.Step4ComparingTextColor = this.ComputeWarningPercentageColor(this.Step4ComparingPrice);
            }
            else {
                this.Step4ComparingPrice = null;
                this.Step4ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step5Price) ? 0 : this.Step5Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step5Price) ? 0 : this.ComparedEntity.Step5Price;
            var step5ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step5ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step5ComparingPrice = (step5ComparingValue / value2) * 100;
                this.Step5ComparingTextColor = this.ComputeWarningPercentageColor(this.Step5ComparingPrice);
            }
            else {
                this.Step5ComparingPrice = null;
                this.Step5ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step6Price) ? 0 : this.Step6Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step6Price) ? 0 : this.ComparedEntity.Step6Price;
            var step6ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step6ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step6ComparingPrice = (step6ComparingValue / value2) * 100;
                this.Step6ComparingTextColor = this.ComputeWarningPercentageColor(this.Step6ComparingPrice);
            }
            else {
                this.Step6ComparingPrice = null;
                this.Step6ComparingTextColor = this.DefaultColor;

            }

            value1 = AppTool.IsNullOrZero(this.Step7Price) ? 0 : this.Step7Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step7Price) ? 0 : this.ComparedEntity.Step7Price;
            var step7ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step7ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step7ComparingPrice = (step7ComparingValue / value2) * 100;
                this.Step7ComparingTextColor = this.ComputeWarningPercentageColor(this.Step7ComparingPrice);
            }
            else {

                this.Step7ComparingPrice = null;
                this.Step7ComparingTextColor = this.DefaultColor;
            }

            value1 = AppTool.IsNullOrZero(this.Step8Price) ? 0 : this.Step8Price;
            value2 = AppTool.IsNullOrZero(this.ComparedEntity.Step8Price) ? 0 : this.ComparedEntity.Step8Price;
            var step8ComparingValue = value1 - value2;
            if (!AppTool.IsNullOrZero(step8ComparingValue) && !AppTool.IsNullOrZero(value2)) {
                this.Step8ComparingPrice = (step8ComparingValue / value2) * 100;
                this.Step8ComparingTextColor = this.ComputeWarningPercentageColor(this.Step8ComparingPrice);
            }
            else {
                this.Step8ComparingPrice = null;
                this.Step8ComparingTextColor = this.DefaultColor;
            }
        }
    }

    ComputeWarningPercentageColor(price: number) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {

            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    }
    
    get HasErrors() {
        return this.EntityPM.HasErrors;
    }
    set HasErrors(value: boolean) {
        if (this.EntityPM.HasErrors != value) {
            this.EntityPM.HasErrors = value;
        }
    }

    get ErrorText() {
        return this.EntityPM.ErrorText;
    }
    set ErrorText(value: string) {
        if (this.EntityPM.ErrorText != value) {
            this.EntityPM.ErrorText = value;
        }
    }

    private CheckIfLineHasError() {
        if (this.ErrorText != 'Line is a duplicate') {
            var error: boolean = false;
            var errorText: string;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                }

                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found"
                }
            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Origin Port";
                }

                else {
                    errorText = errorText + ", Missing Origin Port"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                }

                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found"
                }
            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Destination Port";
                }

                else {
                    errorText = errorText + ", Missing Destination Port"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.MinPriceText) && AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Min Price format is invalid";
                }

                else {
                    errorText = errorText + ", Min Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 1 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 1 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 2 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 2 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 3 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 3 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 4 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 4 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 5 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 5 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 6 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 6 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 7 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 7 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 8 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Step 8 Price format is invalid"
                }
            }

            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
    }

    // Origin Port
    get OriginPortId() {
        return this.EntityPM.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.EntityPM.OriginPortId != value) {
            this.EntityPM.OriginPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get OriginPortCode() {
        return this.EntityPM.OriginPortCode;
    }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    get OriginPortCombinedCode() {
        return this.EntityPM.OriginPortCombinedCode;
    }
    set OriginPortCombinedCode(value: string) {
        if (this.EntityPM.OriginPortCombinedCode != value) {
            this.EntityPM.OriginPortCombinedCode = value;
        }
    }

    originPort: PortList;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortList) {
        if (this.originPort != value) {
            this.originPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginPortCode = value.Code;
            this.OriginPortCombinedCode = value.CombinedCode;
        } else {
            this.OriginPortCode = null;
            this.OriginPortCombinedCode = null;
        }
    }

    get OriginPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
                return this.EntityPM.OriginPortCode;
            }

            else {
                return this.EntityPM.OriginPortText;
            }
        } else {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCombinedCode)) {
                return this.EntityPM.OriginPortCombinedCode;
            }

            else {
                return this.EntityPM.OriginPortText;
            }
        }
    }

    get OriginPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Destination Port
    get DestinationPortId() {
        return this.EntityPM.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.EntityPM.DestinationPortId != value) {
            this.EntityPM.DestinationPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get DestinationPortCode() {
        return this.EntityPM.DestinationPortCode;
    }
    set DestinationPortCode(value: string) {
        if (this.EntityPM.DestinationPortCode != value) {
            this.EntityPM.DestinationPortCode = value;
        }
    }

    get DestinationPortCombinedCode() {
        return this.EntityPM.DestinationPortCombinedCode;
    }
    set DestinationPortCombinedCode(value: string) {
        if (this.EntityPM.DestinationPortCombinedCode != value) {
            this.EntityPM.DestinationPortCombinedCode = value;
        }
    }

    destinationPort: PortList;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortList) {
        if (this.destinationPort != value) {
            this.destinationPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DestinationPortCode = value.Code;
            this.DestinationPortCombinedCode = value.CombinedCode;
        } else {
            this.DestinationPortCode = null;
            this.DestinationPortCombinedCode = null;
        }
    }

    get DestinationPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
                return this.EntityPM.DestinationPortCode;
            }

            else {
                return this.EntityPM.DestinationPortText;
            }
        } else {

            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCombinedCode)) {
                return this.EntityPM.DestinationPortCombinedCode;
            }

            else {
                return this.EntityPM.DestinationPortText;
            }
        }
    }

    get DestinationPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Via Port
    get ViaPortId() {
        return this.EntityPM.ViaPortId;
    }
    set ViaPortId(value: string) {
        if (this.EntityPM.ViaPortId != value) {
            this.EntityPM.ViaPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get ViaPortCode() {
        return this.EntityPM.ViaPortCode;
    }
    set ViaPortCode(value: string) {
        if (this.EntityPM.ViaPortCode != value) {
            this.EntityPM.ViaPortCode = value;
        }
    }

    get ViaPortCombinedCode() {
        return this.EntityPM.ViaPortCombinedCode;
    }
    set ViaPortCombinedCode(value: string) {
        if (this.EntityPM.ViaPortCombinedCode != value) {
            this.EntityPM.ViaPortCombinedCode = value;
        }
    }

    viaPort: PortList;
    get ViaPort() { return this.viaPort; }
    set ViaPort(value: PortList) {
        if (this.viaPort != value) {
            this.viaPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ViaPortCode = value.Code;
            this.ViaPortCombinedCode = value.CombinedCode;
        } else {
            this.ViaPortCode = null;
            this.ViaPortCombinedCode = null;
        }
    }

    get ViaPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortCode)) {
                return this.EntityPM.ViaPortCode;
            }

            else {
                return this.EntityPM.ViaPortText;
            }
        } else {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortCombinedCode)) {
                return this.EntityPM.ViaPortCombinedCode;
            }

            else {
                return this.EntityPM.ViaPortText;
            }
        }
    }

    get ViaPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }


    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get TransitTime() {
        return this.EntityPM.TransitTime;
    }
    set TransitTime(value: string) {
        if (this.EntityPM.TransitTime != value) {
            this.EntityPM.TransitTime = value;
        }
    }

    // Min Price
    get MinPrice() {
        return this.EntityPM.MinPrice;
    }
    set MinPrice(value: number) {
        if (this.EntityPM.MinPrice != value) {
            this.EntityPM.MinPrice = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get MinPriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
            return FormatTool.FormatNumber(this.EntityPM.MinPrice, "N3");
        }

        else {
            return this.EntityPM.MinPriceText;
        }
    }

    get MinPriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 1
    get Step1Price() {
        return this.EntityPM.Step1Price;
    }
    set Step1Price(value: number) {
        if (this.EntityPM.Step1Price != value) {
            this.EntityPM.Step1Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step1PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step1Price, "N3");
        }

        else {
            return this.EntityPM.Step1PriceText;
        }
    }

    get Step1PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 2
    get Step2Price() {
        return this.EntityPM.Step2Price;
    }
    set Step2Price(value: number) {
        if (this.EntityPM.Step2Price != value) {
            this.EntityPM.Step2Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step2PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step2Price, "N3");
        }

        else {
            return this.EntityPM.Step2PriceText;
        }
    }

    get Step2PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 3
    get Step3Price() {
        return this.EntityPM.Step3Price;
    }
    set Step3Price(value: number) {
        if (this.EntityPM.Step3Price != value) {
            this.EntityPM.Step3Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step3PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step3Price, "N3");
        }

        else {
            return this.EntityPM.Step3PriceText;
        }
    }

    get Step3PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 4
    get Step4Price() {
        return this.EntityPM.Step4Price;
    }
    set Step4Price(value: number) {
        if (this.EntityPM.Step4Price != value) {
            this.EntityPM.Step4Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step4PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step4Price, "N3");
        }

        else {
            return this.EntityPM.Step4PriceText;
        }
    }

    get Step4PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 5
    get Step5Price() {
        return this.EntityPM.Step5Price;
    }
    set Step5Price(value: number) {
        if (this.EntityPM.Step5Price != value) {
            this.EntityPM.Step5Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step5PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step5Price, "N3");
        }

        else {
            return this.EntityPM.Step5PriceText;
        }
    }

    get Step5PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 6
    get Step6Price() {
        return this.EntityPM.Step6Price;
    }
    set Step6Price(value: number) {
        if (this.EntityPM.Step6Price != value) {
            this.EntityPM.Step6Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step6PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step6Price, "N3");
        }

        else {
            return this.EntityPM.Step6PriceText;
        }
    }

    get Step6PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 7
    get Step7Price() {
        return this.EntityPM.Step7Price;
    }
    set Step7Price(value: number) {
        if (this.EntityPM.Step7Price != value) {
            this.EntityPM.Step7Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step7PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step7Price, "N3");
        }

        else {
            return this.EntityPM.Step7PriceText;
        }
    }

    get Step7PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 8
    get Step8Price() {
        return this.EntityPM.Step8Price;
    }
    set Step8Price(value: number) {
        if (this.EntityPM.Step8Price != value) {
            this.EntityPM.Step8Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Step8PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step8Price, "N3");
        }

        else {
            return this.EntityPM.Step8PriceText;
        }
    }

    get Step8PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    private isLineSelected: boolean = false;
    get IsLineSelected() { return this.isLineSelected; }
    set IsLineSelected(value: boolean) {
        if (this.isLineSelected != value) {
            this.isLineSelected = value;
        }
    }
}

export class AirSurchargeTariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: AirSurchargeTariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;
    private initialIndex: number;
    private lineCurrencyId: string;
    constructor(entity: TariffLinePM, public FatherComponent: SurchargeVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.initialIndex = entity.Index;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.SetUIProperties();
        this.SetCellColorsForPriceCheck();
        this.lineCurrencyId = this.EntityPM.CurrencyId;
    }

    public CellColor: string = "transparent";
    private SetCellColorsForPriceCheck() {
        if (!AppTool.IsNullOrEmpty(this.FatherComponent.LineIdFromPriceCheck) && this.FatherComponent.LineIdFromPriceCheck == this.EntityPM.Id) {
            this.CellColor = "#f7dc6e";
        }

        else {
            if (this.IsEditEnabled) {
                this.CellColor = "transparent";
            }

            else {
                this.CellColor = "rgba(230, 231, 232, 0.5)";
            }
        }
    }

    private CheckIfLineHasError() {
        if (this.ErrorText != 'Line is a duplicate') {
            var error: boolean = false;
            var errorText: string;

            if (!this.IsFromAllOtherPorts) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                    }

                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found"
                    }
                }
                else if (AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Origin Port";
                    }

                    else {
                        errorText = errorText + ", Missing Origin Port"
                    }
                }
            }

            if (!this.IsToAllOtherPorts) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                    }

                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found"
                    }
                }
                else if (AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Destination Port";
                    }

                    else {
                        errorText = errorText + ", Missing Destination Port"
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 1 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 1 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 2 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 2 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 3 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 3 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 4 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 4 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 5 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 5 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 6 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 6 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 7 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 7 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 8 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 8 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 9 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 9 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 10 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 10 Price format is invalid"
                }
            }

            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    }
    
    private SetUIProperties() {
        this.SetUIProperties_From();
        this.SetUIProperties_To();
        this.SetUIProperties_MinPrices();
        this.SetUIProperties_Currency();
    }
    private SetUIProperties_From() {
        if (this.IsFromAllOtherPorts) {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsFromAllOtherPorts", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        }
    }
    private SetUIProperties_To() {
        if (this.IsToAllOtherPorts) {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsToAllOtherPorts", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
        }
    }
    private SetUIProperties_Currency() {
        var isDefaultCurrencyRequired: boolean = false;
        var isDefaultCurrencyEnabled: boolean = false;

        if (this.IsDifferentCurrenciesPerCharge) {
            isDefaultCurrencyRequired = false;
            isDefaultCurrencyEnabled = false;
        }
        else {
            isDefaultCurrencyEnabled = true;
            if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
                isDefaultCurrencyRequired = true;
            }
        }

        this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, isDefaultCurrencyRequired);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, isDefaultCurrencyEnabled);

        for (var i = 1; i <= 10; i++) {
            this.UIProperties.SetEnabled("Surcharge" + i + "CurrencyId", this.ObjectTableName, !isDefaultCurrencyEnabled);
        }
    }
    private SetUIProperties_MinPrices() {
        this.UIProperties.SetVisibility("Surcharge1MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(1));
        this.UIProperties.SetVisibility("Surcharge2MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(2));
        this.UIProperties.SetVisibility("Surcharge3MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(3));
        this.UIProperties.SetVisibility("Surcharge4MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(4));
        this.UIProperties.SetVisibility("Surcharge5MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(5));
        this.UIProperties.SetVisibility("Surcharge6MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(6));
        this.UIProperties.SetVisibility("Surcharge7MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(7));
        this.UIProperties.SetVisibility("Surcharge8MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(8));
        this.UIProperties.SetVisibility("Surcharge9MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(9));
        this.UIProperties.SetVisibility("Surcharge10MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(10));
    }
    private IsMeasurmentFixed(index: number): boolean {
        var isFixed: boolean = false;

        if (this.FatherComponent.AllMeasurements) {
            var iMeasurement = this.FatherComponent.AllMeasurements.filter(f => f.Id == this.FatherComponent.EntityPM['Surcharge' + index + 'UOM'])[0];
            if (iMeasurement) {
                if (iMeasurement.Code == "FIXD") {
                    isFixed = true;
                }
            }
        }

        return isFixed;
    }

    public Surcharge1ComparingPrice: number;
    public Surcharge1ComparingTextColor: string = null;
    public Surcharge2ComparingPrice: number;
    public Surcharge2ComparingTextColor: string = null;
    public Surcharge3ComparingPrice: number;
    public Surcharge3ComparingTextColor: string = null;
    public Surcharge4ComparingPrice: number;
    public Surcharge4ComparingTextColor: string = null;
    public Surcharge5ComparingPrice: number;
    public Surcharge5ComparingTextColor: string = null;
    public Surcharge6ComparingPrice: number;
    public Surcharge6ComparingTextColor: string = null;
    public Surcharge7ComparingPrice: number;
    public Surcharge7ComparingTextColor: string = null;
    public Surcharge8ComparingPrice: number;
    public Surcharge8ComparingTextColor: string = null;
    public Surcharge9ComparingPrice: number;
    public Surcharge9ComparingTextColor: string = null;
    public Surcharge10ComparingPrice: number;
    public Surcharge10ComparingTextColor: string = null;

    public MinPrice1ComparingPrice: number;
    public MinPrice1ComparingTextColor: string = null;
    public MinPrice2ComparingPrice: number;
    public MinPrice2ComparingTextColor: string = null;
    public MinPrice3ComparingPrice: number;
    public MinPrice3ComparingTextColor: string = null;
    public MinPrice4ComparingPrice: number;
    public MinPrice4ComparingTextColor: string = null;
    public MinPrice5ComparingPrice: number;
    public MinPrice5ComparingTextColor: string = null;
    public MinPrice6ComparingPrice: number;
    public MinPrice6ComparingTextColor: string = null;
    public MinPrice7ComparingPrice: number;
    public MinPrice7ComparingTextColor: string = null;
    public MinPrice8ComparingPrice: number;
    public MinPrice8ComparingTextColor: string = null;
    public MinPrice9ComparingPrice: number;
    public MinPrice9ComparingTextColor: string = null;
    public MinPrice10ComparingPrice: number;
    public MinPrice10ComparingTextColor: string = null;
    private DefaultColor = "blue";

    SetCellsComparingText() {
        this.CompareSurcharge1Price();
        this.CompareSurcharge2Price();
        this.CompareSurcharge3Price();
        this.CompareSurcharge4Price();
        this.CompareSurcharge5Price();
        this.CompareSurcharge6Price();
        this.CompareSurcharge7Price();
        this.CompareSurcharge8Price();
        this.CompareSurcharge9Price();
        this.CompareSurcharge10Price();

        this.CompareSurchargeMinPrices();
    }
    private CompareSurchargeMinPrices() {
        this.CompareMainPrice(1);
        this.CompareMainPrice(2);
        this.CompareMainPrice(3);
        this.CompareMainPrice(4);
        this.CompareMainPrice(5);
        this.CompareMainPrice(6);
        this.CompareMainPrice(7);
        this.CompareMainPrice(8);
        this.CompareMainPrice(9);
        this.CompareMainPrice(10);
    }

    private CompareMainPrice(index: number) {
        if (this.ComparedEntity != null) {
            this['MinPrice' + index + 'ComparingPrice'] = null;
            this['MinPrice' + index + 'ComparingTextColor'] = this.DefaultColor;

            if (this.ComparedEntity['Surcharge' + index + 'MinPrice'] != null) {
                var priceValue = this['Surcharge' + index + 'MinPrice'] - this.ComparedEntity['Surcharge' + index + 'MinPrice'];
                if (!AppTool.IsNullOrZero(priceValue) && !AppTool.IsNullOrZero(this.ComparedEntity['Surcharge' + index + 'MinPrice'])) {
                    this['MinPrice' + index + 'ComparingPrice'] = (priceValue / this.ComparedEntity['Surcharge' + index + 'MinPrice']) * 100;
                    this['MinPrice' + index + 'ComparingTextColor'] = this.ComputeWarningPercentageColor(this['MinPrice' + index + 'ComparingPrice']);
                }
            }
        }
    }

    private CompareSurcharge1Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge1ComparingPrice = null;
            this.Surcharge1ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge1Price != null) {
                var surcharge1ComparingValue = this.Surcharge1Price - this.ComparedEntity.Surcharge1Price;
                if (!AppTool.IsNullOrZero(surcharge1ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge1Price)) {
                    this.Surcharge1ComparingPrice = (surcharge1ComparingValue / this.ComparedEntity.Surcharge1Price) * 100;
                    this.Surcharge1ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge1ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge2Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge2ComparingPrice = null;
            this.Surcharge2ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge2Price != null) {
                var surcharge2ComparingValue = this.Surcharge2Price - this.ComparedEntity.Surcharge2Price;
                if (!AppTool.IsNullOrZero(surcharge2ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge2Price)) {
                    this.Surcharge2ComparingPrice = (surcharge2ComparingValue / this.ComparedEntity.Surcharge2Price) * 100;
                    this.Surcharge2ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge2ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge3Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge3ComparingPrice = null;
            this.Surcharge3ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge3Price != null) {
                var surcharge3ComparingValue = this.Surcharge3Price - this.ComparedEntity.Surcharge3Price;
                if (!AppTool.IsNullOrZero(surcharge3ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge3Price)) {
                    this.Surcharge3ComparingPrice = (surcharge3ComparingValue / this.ComparedEntity.Surcharge3Price) * 100;
                    this.Surcharge3ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge3ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge4Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge4ComparingPrice = null;
            this.Surcharge4ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge4Price != null) {
                var surcharge4ComparingValue = this.Surcharge4Price - this.ComparedEntity.Surcharge4Price;
                if (!AppTool.IsNullOrZero(surcharge4ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge4Price)) {
                    this.Surcharge4ComparingPrice = (surcharge4ComparingValue / this.ComparedEntity.Surcharge4Price) * 100;
                    this.Surcharge4ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge4ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge5Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge5ComparingPrice = null;
            this.Surcharge5ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge5Price != null) {
                var surcharge5ComparingValue = this.Surcharge5Price - this.ComparedEntity.Surcharge5Price;
                if (!AppTool.IsNullOrZero(surcharge5ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge5Price)) {
                    this.Surcharge5ComparingPrice = (surcharge5ComparingValue / this.ComparedEntity.Surcharge5Price) * 100;
                    this.Surcharge5ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge5ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge6Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge6ComparingPrice = null;
            this.Surcharge6ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge6Price != null) {
                var surcharge6ComparingValue = this.Surcharge6Price - this.ComparedEntity.Surcharge6Price;
                if (!AppTool.IsNullOrZero(surcharge6ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge6Price)) {
                    this.Surcharge6ComparingPrice = (surcharge6ComparingValue / this.ComparedEntity.Surcharge6Price) * 100;
                    this.Surcharge6ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge6ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge7Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge7ComparingPrice = null;
            this.Surcharge7ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge7Price != null) {
                var surcharge7ComparingValue = this.Surcharge7Price - this.ComparedEntity.Surcharge7Price;
                if (!AppTool.IsNullOrZero(surcharge7ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge7Price)) {
                    this.Surcharge7ComparingPrice = (surcharge7ComparingValue / this.ComparedEntity.Surcharge7Price) * 100;
                    this.Surcharge7ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge7ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge8Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge8ComparingPrice = null;
            this.Surcharge8ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge8Price != null) {
                var surcharge8ComparingValue = this.Surcharge8Price - this.ComparedEntity.Surcharge8Price;
                if (!AppTool.IsNullOrZero(surcharge8ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge8Price)) {
                    this.Surcharge8ComparingPrice = (surcharge8ComparingValue / this.ComparedEntity.Surcharge8Price) * 100;
                    this.Surcharge8ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge8ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge9Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge9ComparingPrice = null;
            this.Surcharge9ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge9Price != null) {
                var surcharge9ComparingValue = this.Surcharge9Price - this.ComparedEntity.Surcharge9Price;
                if (!AppTool.IsNullOrZero(surcharge9ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge9Price)) {
                    this.Surcharge9ComparingPrice = (surcharge9ComparingValue / this.ComparedEntity.Surcharge9Price) * 100;
                    this.Surcharge9ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge9ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge10Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge10ComparingPrice = null;
            this.Surcharge10ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge10Price != null) {
                var surcharge10ComparingValue = this.Surcharge10Price - this.ComparedEntity.Surcharge10Price;
                if (!AppTool.IsNullOrZero(surcharge10ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge10Price)) {
                    this.Surcharge10ComparingPrice = (surcharge10ComparingValue / this.ComparedEntity.Surcharge10Price) * 100;
                    this.Surcharge10ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge10ComparingPrice);
                }
            }
        }
    }
    ComputeWarningPercentageColor(price: number) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {

            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    }

    get HasErrors() {
        return this.EntityPM.HasErrors;
    }
    set HasErrors(value: boolean) {
        if (this.EntityPM.HasErrors != value) {
            this.EntityPM.HasErrors = value;
        }
    }

    get ErrorText() {
        return this.EntityPM.ErrorText;
    }
    set ErrorText(value: string) {
        if (this.EntityPM.ErrorText != value) {
            this.EntityPM.ErrorText = value;
        }
    }

    // Origin Port
    get OriginPortId() {
        return this.EntityPM.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.EntityPM.OriginPortId != value) {
            this.EntityPM.OriginPortId = value;
            this.EntityPM.LineEdited = true;

            if (this.IsFromAllOtherPorts) {
                this.EntityPM.IsFromAllOtherPorts = false;
            }
            this.SetUIProperties_From();
            this.CheckIfLineHasError();
        }
    }

    get OriginPortCode() {
        return this.EntityPM.OriginPortCode;
    }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    get OriginPortCombinedCode() {
        return this.EntityPM.OriginPortCombinedCode;
    }
    set OriginPortCombinedCode(value: string) {
        if (this.EntityPM.OriginPortCombinedCode != value) {
            this.EntityPM.OriginPortCombinedCode = value;
        }
    }

    originPort: PortList;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortList) {
        if (this.originPort != value) {
            this.originPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginPortCode = value.Code;
            this.OriginPortCombinedCode = value.CombinedCode; 
        } else {
            this.OriginPortCode = null;
            this.OriginPortCombinedCode = null;
        }
    }

    get OriginPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
                return this.EntityPM.OriginPortCode;
            }

            else {
                return this.EntityPM.OriginPortText;
            }
        } else {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCombinedCode)) {
                return this.EntityPM.OriginPortCombinedCode;
            }

            else {
                return this.EntityPM.OriginPortText;
            }
        }
    }

    get OriginPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Destination Port
    get DestinationPortId() {
        return this.EntityPM.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.EntityPM.DestinationPortId != value) {
            this.EntityPM.DestinationPortId = value;
            this.EntityPM.LineEdited = true;

            if (this.IsToAllOtherPorts) {
                this.EntityPM.IsToAllOtherPorts = false;
            }
            this.SetUIProperties_To();
            this.CheckIfLineHasError();
        }
    }

    get DestinationPortCode() {
        return this.EntityPM.DestinationPortCode;
    }
    set DestinationPortCode(value: string) {
        if (this.EntityPM.DestinationPortCode != value) {
            this.EntityPM.DestinationPortCode = value;
        }
    }

    get DestinationPortCombinedCode() {
        return this.EntityPM.DestinationPortCombinedCode;
    }
    set DestinationPortCombinedCode(value: string) {
        if (this.EntityPM.DestinationPortCombinedCode != value) {
            this.EntityPM.DestinationPortCombinedCode = value;
        }
    }

    destinationPort: PortList;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortList) {
        if (this.destinationPort != value) {
            this.destinationPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DestinationPortCode = value.Code;
            this.DestinationPortCombinedCode = value.CombinedCode; 
        } else {
            this.DestinationPortCode = null;
            this.DestinationPortCombinedCode = null;
        }
    }

    get DestinationPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
                return this.EntityPM.DestinationPortCode;
            }

            else {
                return this.EntityPM.DestinationPortText;
            }
        } else {

            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCombinedCode)) {
                return this.EntityPM.DestinationPortCombinedCode;
            }

            else {
                return this.EntityPM.DestinationPortText;
            }
        }
    }

    get DestinationPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Via Port
    get ViaPortId() {
        return this.EntityPM.ViaPortId;
    }
    set ViaPortId(value: string) {
        if (this.EntityPM.ViaPortId != value) {
            this.EntityPM.ViaPortId = value;
            this.EntityPM.LineEdited = true;

            this.SetUIProperties_To();
            this.CheckIfLineHasError();
        }
    }

    get ViaPortCode() {
        return this.EntityPM.ViaPortCode;
    }
    set ViaPortCode(value: string) {
        if (this.EntityPM.ViaPortCode != value) {
            this.EntityPM.ViaPortCode = value;
        }
    }

    get ViaPortCombinedCode() {
        return this.EntityPM.ViaPortCombinedCode;
    }
    set ViaPortCombinedCode(value: string) {
        if (this.EntityPM.ViaPortCombinedCode != value) {
            this.EntityPM.ViaPortCombinedCode = value;
        }
    }

    viaPort: PortList;
    get ViaPort() { return this.viaPort; }
    set ViaPort(value: PortList) {
        if (this.viaPort != value) {
            this.viaPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ViaPortCode = value.Code;
            this.ViaPortCombinedCode = value.CombinedCode;
        } else {
            this.ViaPortCode = null;
            this.ViaPortCombinedCode = null;
        }
    }

    get ViaPortValue() {
        if (this.FatherComponent.IsAir) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortCode)) {
                return this.EntityPM.ViaPortCode;
            }

            else {
                return this.EntityPM.ViaPortText;
            }
        } else {

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortCombinedCode)) {
                return this.EntityPM.ViaPortCombinedCode;
            }

            else {
                return this.EntityPM.ViaPortText;
            }
        }
    }

    get ViaPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    currency: CurrencyList;
    get Currency() { return this.currency; }
    set Currency(value: CurrencyList) {
        if (this.currency != value) {
            this.currency = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CurrencyCode = value.Code;
        } else {
            this.CurrencyCode = null;
        }
    }

    get CurrencyCode() {
        return this.EntityPM.CurrencyCode;
    }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
        }
    }
    
    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.lineCurrencyId = this.EntityPM.CurrencyId;

            this.SetUIProperties_Currency();
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get IsDifferentCurrenciesPerCharge() {
        return this.EntityPM.IsDifferentCurrenciesPerCharge;
    }
    set IsDifferentCurrenciesPerCharge(value: boolean) {
        if (this.EntityPM.IsDifferentCurrenciesPerCharge != value) {
            this.EntityPM.IsDifferentCurrenciesPerCharge = value;

            this.SetUIProperties_Currency();
            this.SurchargesCurrencies(this.CurrencyId);

            if (value) {
                this.EntityPM.CurrencyId = null;
            }
            else {
                this.CurrencyId = this.lineCurrencyId;
                for (var i = 1; i <= 10; i++) {
                    this["Surcharge" + i + "CurrencyId"] = null;
                }
            }
        }
    }

    get Surcharge1CurrencyId() {
        return this.EntityPM.Surcharge1CurrencyId;
    }
    set Surcharge1CurrencyId(value: string) {
        if (this.EntityPM.Surcharge1CurrencyId != value) {
            this.EntityPM.Surcharge1CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge2CurrencyId() {
        return this.EntityPM.Surcharge2CurrencyId;
    }
    set Surcharge2CurrencyId(value: string) {
        if (this.EntityPM.Surcharge2CurrencyId != value) {
            this.EntityPM.Surcharge2CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge3CurrencyId() {
        return this.EntityPM.Surcharge3CurrencyId;
    }
    set Surcharge3CurrencyId(value: string) {
        if (this.EntityPM.Surcharge3CurrencyId != value) {
            this.EntityPM.Surcharge3CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge4CurrencyId() {
        return this.EntityPM.Surcharge4CurrencyId;
    }
    set Surcharge4CurrencyId(value: string) {
        if (this.EntityPM.Surcharge4CurrencyId != value) {
            this.EntityPM.Surcharge4CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge5CurrencyId() {
        return this.EntityPM.Surcharge5CurrencyId;
    }
    set Surcharge5CurrencyId(value: string) {
        if (this.EntityPM.Surcharge5CurrencyId != value) {
            this.EntityPM.Surcharge5CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge6CurrencyId() {
        return this.EntityPM.Surcharge6CurrencyId;
    }
    set Surcharge6CurrencyId(value: string) {
        if (this.EntityPM.Surcharge6CurrencyId != value) {
            this.EntityPM.Surcharge6CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge7CurrencyId() {
        return this.EntityPM.Surcharge7CurrencyId;
    }
    set Surcharge7CurrencyId(value: string) {
        if (this.EntityPM.Surcharge7CurrencyId != value) {
            this.EntityPM.Surcharge7CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge8CurrencyId() {
        return this.EntityPM.Surcharge8CurrencyId;
    }
    set Surcharge8CurrencyId(value: string) {
        if (this.EntityPM.Surcharge8CurrencyId != value) {
            this.EntityPM.Surcharge8CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge9CurrencyId() {
        return this.EntityPM.Surcharge9CurrencyId;
    }
    set Surcharge9CurrencyId(value: string) {
        if (this.EntityPM.Surcharge9CurrencyId != value) {
            this.EntityPM.Surcharge9CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Surcharge10CurrencyId() {
        return this.EntityPM.Surcharge10CurrencyId;
    }
    set Surcharge10CurrencyId(value: string) {
        if (this.EntityPM.Surcharge10CurrencyId != value) {
            this.EntityPM.Surcharge10CurrencyId = value;
            this.EntityPM.LineEdited = true;
        }
    }

    // Surcharge 1
    get Surcharge1Price() {
        return this.EntityPM.Surcharge1Price;
    }
    set Surcharge1Price(value: number) {
        if (this.EntityPM.Surcharge1Price != value) {
            this.EntityPM.Surcharge1Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge1Price();
        }
    }

    get Surcharge1MinPrice() {
        return this.EntityPM.Surcharge1MinPrice;
    }
    set Surcharge1MinPrice(value: number) {
        if (this.EntityPM.Surcharge1MinPrice != value) {
            this.EntityPM.Surcharge1MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(1);
        }
    }

    get Surcharge1PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge1Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge1PriceText;
        }
    }

    get Surcharge1PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge1MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge1MinPrice, "N3");
    }

    // Surcharge 2
    get Surcharge2Price() {
        return this.EntityPM.Surcharge2Price;
    }
    set Surcharge2Price(value: number) {
        if (this.EntityPM.Surcharge2Price != value) {
            this.EntityPM.Surcharge2Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge2Price();
        }
    }

    get Surcharge2MinPrice() {
        return this.EntityPM.Surcharge2MinPrice;
    }
    set Surcharge2MinPrice(value: number) {
        if (this.EntityPM.Surcharge2MinPrice != value) {
            this.EntityPM.Surcharge2MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(2);
        }
    }

    get Surcharge2PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge2Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge2PriceText;
        }
    }

    get Surcharge2PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge2MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge2MinPrice, "N3");
    }

    // Surcharge 3
    get Surcharge3Price() {
        return this.EntityPM.Surcharge3Price;
    }
    set Surcharge3Price(value: number) {
        if (this.EntityPM.Surcharge3Price != value) {
            this.EntityPM.Surcharge3Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge3Price();
        }
    }

    get Surcharge3MinPrice() {
        return this.EntityPM.Surcharge3MinPrice;
    }
    set Surcharge3MinPrice(value: number) {
        if (this.EntityPM.Surcharge3MinPrice != value) {
            this.EntityPM.Surcharge3MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(3);
        }
    }

    get Surcharge3PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge3Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge3PriceText;
        }
    }

    get Surcharge3PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge3MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge3MinPrice, "N3");
    }

    // Surcharge 4
    get Surcharge4Price() {
        return this.EntityPM.Surcharge4Price;
    }
    set Surcharge4Price(value: number) {
        if (this.EntityPM.Surcharge4Price != value) {
            this.EntityPM.Surcharge4Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge4Price();
        }
    }

    get Surcharge4MinPrice() {
        return this.EntityPM.Surcharge4MinPrice;
    }
    set Surcharge4MinPrice(value: number) {
        if (this.EntityPM.Surcharge4MinPrice != value) {
            this.EntityPM.Surcharge4MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(4);
        }
    }

    get Surcharge4PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge4Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge4PriceText;
        }
    }

    get Surcharge4PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge4MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge4MinPrice, "N3");
    }

    // Surcharge 5
    get Surcharge5Price() {
        return this.EntityPM.Surcharge5Price;
    }
    set Surcharge5Price(value: number) {
        if (this.EntityPM.Surcharge5Price != value) {
            this.EntityPM.Surcharge5Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge5Price();
        }
    }

    get Surcharge5MinPrice() {
        return this.EntityPM.Surcharge5MinPrice;
    }
    set Surcharge5MinPrice(value: number) {
        if (this.EntityPM.Surcharge5MinPrice != value) {
            this.EntityPM.Surcharge5MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(5);
        }
    }

    get Surcharge5PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge5Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge5PriceText;
        }
    }

    get Surcharge5PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge5MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge5MinPrice, "N3");
    }

    // Surcharge 6
    get Surcharge6Price() {
        return this.EntityPM.Surcharge6Price;
    }
    set Surcharge6Price(value: number) {
        if (this.EntityPM.Surcharge6Price != value) {
            this.EntityPM.Surcharge6Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge6Price();
        }
    }

    get Surcharge6MinPrice() {
        return this.EntityPM.Surcharge6MinPrice;
    }
    set Surcharge6MinPrice(value: number) {
        if (this.EntityPM.Surcharge6MinPrice != value) {
            this.EntityPM.Surcharge6MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(6);
        }
    }

    get Surcharge6PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge6Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge6PriceText;
        }
    }

    get Surcharge6PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge6MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge6MinPrice, "N3");
    }

    // Surcharge 7
    get Surcharge7Price() {
        return this.EntityPM.Surcharge7Price;
    }
    set Surcharge7Price(value: number) {
        if (this.EntityPM.Surcharge7Price != value) {
            this.EntityPM.Surcharge7Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge7Price();
        }
    }

    get Surcharge7MinPrice() {
        return this.EntityPM.Surcharge7MinPrice;
    }
    set Surcharge7MinPrice(value: number) {
        if (this.EntityPM.Surcharge7MinPrice != value) {
            this.EntityPM.Surcharge7MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(7);
        }
    }

    get Surcharge7PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge7Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge7PriceText;
        }
    }

    get Surcharge7PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge7MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge7MinPrice, "N3");
    }

    // Surcharge 8
    get Surcharge8Price() {
        return this.EntityPM.Surcharge8Price;
    }
    set Surcharge8Price(value: number) {
        if (this.EntityPM.Surcharge8Price != value) {
            this.EntityPM.Surcharge8Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge8Price();
        }
    }

    get Surcharge8MinPrice() {
        return this.EntityPM.Surcharge8MinPrice;
    }
    set Surcharge8MinPrice(value: number) {
        if (this.EntityPM.Surcharge8MinPrice != value) {
            this.EntityPM.Surcharge8MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(8);
        }
    }

    get Surcharge8PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge8Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge8PriceText;
        }
    }

    get Surcharge8PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge8MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge8MinPrice, "N3");
    }

    // Surcharge 9
    get Surcharge9Price() {
        return this.EntityPM.Surcharge9Price;
    }
    set Surcharge9Price(value: number) {
        if (this.EntityPM.Surcharge9Price != value) {
            this.EntityPM.Surcharge9Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge9Price();
        }
    }

    get Surcharge9MinPrice() {
        return this.EntityPM.Surcharge9MinPrice;
    }
    set Surcharge9MinPrice(value: number) {
        if (this.EntityPM.Surcharge9MinPrice != value) {
            this.EntityPM.Surcharge9MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(9);
        }
    }

    get Surcharge9PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge9Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge9PriceText;
        }
    }

    get Surcharge9PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge9MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge9MinPrice, "N3");
    }

    // Surcharge 10
    get Surcharge10Price() {
        return this.EntityPM.Surcharge10Price;
    }
    set Surcharge10Price(value: number) {
        if (this.EntityPM.Surcharge10Price != value) {
            this.EntityPM.Surcharge10Price = value;
            this.EntityPM.LineEdited = true;
            this.CheckIfLineHasError();
            this.CompareSurcharge10Price();
        }
    }

    get Surcharge10MinPrice() {
        return this.EntityPM.Surcharge10MinPrice;
    }
    set Surcharge10MinPrice(value: number) {
        if (this.EntityPM.Surcharge10MinPrice != value) {
            this.EntityPM.Surcharge10MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(10);
        }
    }

    get Surcharge10PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge10Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge10PriceText;
        }
    }

    get Surcharge10PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge10MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge10MinPrice, "N3");
    }

    ////////////////////////////
    get IsFromAllOtherPorts() { return this.EntityPM.IsFromAllOtherPorts; }
    set IsFromAllOtherPorts(value: boolean) {
        if (this.EntityPM.IsFromAllOtherPorts != value) {
            this.EntityPM.IsFromAllOtherPorts = value;

            if (value) {
                this.OriginPortId = null;
                this.EntityPM.OriginPortText = null;
            }

            this.SetUIProperties_From();
            this.CheckIfLineHasError();
            this.ComputeIndex();
        }
    }

    get IsToAllOtherPorts() { return this.EntityPM.IsToAllOtherPorts; }
    set IsToAllOtherPorts(value: boolean) {
        if (this.EntityPM.IsToAllOtherPorts != value) {
            this.EntityPM.IsToAllOtherPorts = value;

            if (value) {
                this.DestinationPortId = null;
                this.EntityPM.DestinationPortText = null;
            }

            this.SetUIProperties_To();
            this.CheckIfLineHasError();
            this.ComputeIndex();
        }
    }

    private ComputeIndex() {
        if (this.IsFromAllOtherPorts || this.IsToAllOtherPorts) {
            this.EntityPM.Index = -1;
        }

        else {
            if (this.IsNewEntity) {
                this.EntityPM.Index = this.initialIndex;
            }

            else {
                if (this.initialIndex == -1) {
                    this.EntityPM.Index = 0;

                }

                else {
                    this.EntityPM.Index = this.initialIndex;
                }
            }            
        }
    }

    private isLineSelected: boolean = false;
    get IsLineSelected() { return this.isLineSelected; }
    set IsLineSelected(value: boolean) {
        if (this.isLineSelected != value) {
            this.isLineSelected = value;
        }
    }

    private SurchargesCurrencies(defaultCurrencyId: string) {
        for (var i = 1; i <= 10; i++) {
            if (this.FatherComponent["Surcharge" + i + "PriceVisibility"]) {
                this["Surcharge" + i + "CurrencyId"] = defaultCurrencyId;
            }
        }
    }
}

export class OceanFCLFreightTariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: OceanFCLFreightTariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;
    constructor(entity: TariffLinePM, public FatherComponent: OceanFCLVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.SetUIProperties();
        this.SetCellColorsForPriceCheck();
    }

    public CellColor: string = "transparent";
    private SetCellColorsForPriceCheck() {
        if (!AppTool.IsNullOrEmpty(this.FatherComponent.LineIdFromPriceCheck) && this.FatherComponent.LineIdFromPriceCheck == this.EntityPM.Id) {
            this.CellColor = "#f7dc6e";
        }

        else {
            if (this.IsEditEnabled) {
                this.CellColor = "transparent";
            }

            else {
                this.CellColor = "rgba(230, 231, 232, 0.5)";
            }
        }
    }

    public Container1ComparingPrice: number;
    public Container1ComparingTextColor: string = null;
    public Container2ComparingPrice: number;
    public Container2ComparingTextColor: string = null;
    public Container3ComparingPrice: number;
    public Container3ComparingTextColor: string = null;
    public Container4ComparingPrice: number;
    public Container4ComparingTextColor: string = null;
    public Container5ComparingPrice: number;
    public Container5ComparingTextColor: string = null;
    private DefaultColor = "blue";

    SetCellsComparingText() {
        if (this.ComparedEntity != null) {
            this.CompareContainer1Price();
            this.CompareContainer2Price();
            this.CompareContainer3Price();
            this.CompareContainer4Price();
            this.CompareContainer5Price();
        }
    }
    private CompareContainer1Price() {
        if (this.ComparedEntity != null) {
            this.Container1ComparingPrice = null;
            this.Container1ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge1Price != null) {
                var surcharge1ComparingValue = this.Surcharge1Price - this.ComparedEntity.Surcharge1Price;
                if (!AppTool.IsNullOrZero(surcharge1ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge1Price)) {
                    this.Container1ComparingPrice = (surcharge1ComparingValue / this.ComparedEntity.Surcharge1Price) * 100;
                    this.Container1ComparingTextColor = this.ComputeWarningPercentageColor(this.Container1ComparingPrice);
                }
            }
        }
    }
    private CompareContainer2Price() {
        if (this.ComparedEntity != null) {
            this.Container2ComparingPrice = null;
            this.Container2ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge2Price != null) {
                var surcharge2ComparingValue = this.Surcharge2Price - this.ComparedEntity.Surcharge2Price;
                if (!AppTool.IsNullOrZero(surcharge2ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge2Price)) {
                    this.Container2ComparingPrice = (surcharge2ComparingValue / this.ComparedEntity.Surcharge2Price) * 100;
                    this.Container2ComparingTextColor = this.ComputeWarningPercentageColor(this.Container2ComparingPrice);
                }
            }
        }
    }
    private CompareContainer3Price() {
        if (this.ComparedEntity != null) {
            this.Container3ComparingPrice = null;
            this.Container3ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge3Price != null) {
                var surcharge3ComparingValue = this.Surcharge3Price - this.ComparedEntity.Surcharge3Price;
                if (!AppTool.IsNullOrZero(surcharge3ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge3Price)) {
                    this.Container3ComparingPrice = (surcharge3ComparingValue / this.ComparedEntity.Surcharge3Price) * 100;
                    this.Container3ComparingTextColor = this.ComputeWarningPercentageColor(this.Container3ComparingPrice);
                }
            }
        }
    }
    private CompareContainer4Price() {
        if (this.ComparedEntity != null) {
            this.Container4ComparingPrice = null;
            this.Container4ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge4Price != null) {
                var surcharge4ComparingValue = this.Surcharge4Price - this.ComparedEntity.Surcharge4Price;
                if (!AppTool.IsNullOrZero(surcharge4ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge4Price)) {
                    this.Container4ComparingPrice = (surcharge4ComparingValue / this.ComparedEntity.Surcharge4Price) * 100;
                    this.Container4ComparingTextColor = this.ComputeWarningPercentageColor(this.Container4ComparingPrice);
                }
            }
        }
    }
    private CompareContainer5Price() {
        if (this.ComparedEntity != null) {
            this.Container5ComparingPrice = null;
            this.Container5ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge5Price != null) {
                var surcharge5ComparingValue = this.Surcharge5Price - this.ComparedEntity.Surcharge5Price;
                if (!AppTool.IsNullOrZero(surcharge5ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge5Price)) {
                    this.Container5ComparingPrice = (surcharge5ComparingValue / this.ComparedEntity.Surcharge5Price) * 100;
                    this.Container5ComparingTextColor = this.ComputeWarningPercentageColor(this.Container5ComparingPrice);
                }
            }
        }
    }

    ComputeWarningPercentageColor(price: number) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {

            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    }

    get HasErrors() {
        return this.EntityPM.HasErrors;
    }
    set HasErrors(value: boolean) {
        if (this.EntityPM.HasErrors != value) {
            this.EntityPM.HasErrors = value;
        }
    }

    get ErrorText() {
        return this.EntityPM.ErrorText;
    }
    set ErrorText(value: string) {
        if (this.EntityPM.ErrorText != value) {
            this.EntityPM.ErrorText = value;
        }
    }

    private CheckIfLineHasError() {
        if (this.ErrorText != 'Line is a duplicate') {
            var error: boolean = false;
            var errorText: string;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                }

                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found"
                }
            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Origin Port";
                }

                else {
                    errorText = errorText + ", Missing Origin Port"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                }

                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found"
                }
            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Destination Port";
                }

                else {
                    errorText = errorText + ", Missing Destination Port"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Container 1 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Container 1 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Container 2 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Container 2 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Container 3 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Container 3 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Container 4 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Container 4 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Container 5 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Container 5 Price format is invalid"
                }
            }

            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
    }

    // Origin Port
    get OriginPortId() {
        return this.EntityPM.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.EntityPM.OriginPortId != value) {
            this.EntityPM.OriginPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get OriginPortCode() {
        return this.EntityPM.OriginPortCode;
    }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    get OriginPortCombinedCode() {
        return this.EntityPM.OriginPortCombinedCode;
    }
    set OriginPortCombinedCode(value: string) {
        if (this.EntityPM.OriginPortCombinedCode != value) {
            this.EntityPM.OriginPortCombinedCode = value;
        }
    }

    originPort: PortList;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortList) {
        if (this.originPort != value) {
            this.originPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginPortCode = value.Code;
            this.OriginPortCombinedCode = value.CombinedCode;
        } else {
            this.OriginPortCode = null;
            this.OriginPortCombinedCode = null;
        }
    }

    get OriginPortValue() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCombinedCode)) {
            return this.EntityPM.OriginPortCombinedCode;
        }

        else {
            return this.EntityPM.OriginPortText;
        }
    }

    get OriginPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Destination Port
    get DestinationPortId() {
        return this.EntityPM.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.EntityPM.DestinationPortId != value) {
            this.EntityPM.DestinationPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get DestinationPortCode() {
        return this.EntityPM.DestinationPortCode;
    }
    set DestinationPortCode(value: string) {
        if (this.EntityPM.DestinationPortCode != value) {
            this.EntityPM.DestinationPortCode = value;
        }
    }

    get DestinationPortCombinedCode() {
        return this.EntityPM.DestinationPortCombinedCode;
    }
    set DestinationPortCombinedCode(value: string) {
        if (this.EntityPM.DestinationPortCombinedCode != value) {
            this.EntityPM.DestinationPortCombinedCode = value;
        }
    }

    destinationPort: PortList;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortList) {
        if (this.destinationPort != value) {
            this.destinationPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DestinationPortCode = value.Code;
            this.DestinationPortCombinedCode = value.CombinedCode;
        } else {
            this.DestinationPortCode = null;
            this.DestinationPortCombinedCode = null;
        }
    }

    get DestinationPortValue() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCombinedCode)) {
            return this.EntityPM.DestinationPortCombinedCode;
        }

        else {
            return this.EntityPM.DestinationPortText;
        }
    }

    get DestinationPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Via Port
    get ViaPortId() {
        return this.EntityPM.ViaPortId;
    }
    set ViaPortId(value: string) {
        if (this.EntityPM.ViaPortId != value) {
            this.EntityPM.ViaPortId = value;
            this.SetUIProperties();
            this.CheckIfLineHasError();
        }
    }

    get ViaPortCode() {
        return this.EntityPM.ViaPortCode;
    }
    set ViaPortCode(value: string) {
        if (this.EntityPM.ViaPortCode != value) {
            this.EntityPM.ViaPortCode = value;
        }
    }

    get ViaPortCombinedCode() {
        return this.EntityPM.ViaPortCombinedCode;
    }
    set ViaPortCombinedCode(value: string) {
        if (this.EntityPM.ViaPortCombinedCode != value) {
            this.EntityPM.ViaPortCombinedCode = value;
        }
    }

    viaPort: PortList;
    get ViaPort() { return this.viaPort; }
    set ViaPort(value: PortList) {
        if (this.viaPort != value) {
            this.viaPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ViaPortCode = value.Code;
            this.ViaPortCombinedCode = value.CombinedCode;
        } else {
            this.ViaPortCode = null;
            this.ViaPortCombinedCode = null;
        }
    }

    get ViaPortValue() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortCombinedCode)) {
            return this.EntityPM.ViaPortCombinedCode;
        }

        else {
            return this.EntityPM.ViaPortText;
        }
    }

    get ViaPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ViaPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }


    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get TransitTime() {
        return this.EntityPM.TransitTime;
    }
    set TransitTime(value: string) {
        if (this.EntityPM.TransitTime != value) {
            this.EntityPM.TransitTime = value;
        }
    }

    // Container 1
    get Surcharge1Price() {
        return this.EntityPM.Surcharge1Price;
    }
    set Surcharge1Price(value: number) {
        if (this.EntityPM.Surcharge1Price != value) {
            this.EntityPM.Surcharge1Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Container1PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge1Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge1PriceText;
        }
    }

    get Container1PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Container 2
    get Surcharge2Price() {
        return this.EntityPM.Surcharge2Price;
    }
    set Surcharge2Price(value: number) {
        if (this.EntityPM.Surcharge2Price != value) {
            this.EntityPM.Surcharge2Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Container2PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge2Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge2PriceText;
        }
    }

    get Container2PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Container 3
    get Surcharge3Price() {
        return this.EntityPM.Surcharge3Price;
    }
    set Surcharge3Price(value: number) {
        if (this.EntityPM.Surcharge3Price != value) {
            this.EntityPM.Surcharge3Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Container3PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge3Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge3PriceText;
        }
    }

    get Container3PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Container 4
    get Surcharge4Price() {
        return this.EntityPM.Surcharge4Price;
    }
    set Surcharge4Price(value: number) {
        if (this.EntityPM.Surcharge4Price != value) {
            this.EntityPM.Surcharge4Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Container4PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge4Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge4PriceText;
        }
    }

    get Container4PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Container 5
    get Surcharge5Price() {
        return this.EntityPM.Surcharge5Price;
    }
    set Surcharge5Price(value: number) {
        if (this.EntityPM.Surcharge5Price != value) {
            this.EntityPM.Surcharge5Price = value;
            this.CheckIfLineHasError();
            this.SetCellsComparingText();
        }
    }

    get Container5PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge5Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge5PriceText;
        }
    }

    get Container5PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    private isLineSelected: boolean = false;
    get IsLineSelected() { return this.isLineSelected; }
    set IsLineSelected(value: boolean) {
        if (this.isLineSelected != value) {
            this.isLineSelected = value;
        }
    }
}
