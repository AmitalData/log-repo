import { Component } from '@angular/core';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

@Component({
    
    templateUrl: './AddEditTariffLineComponent.html',
})

export class AddEditTariffLineComponent  {
  public StartDate: any;
    public TariffType: string;
    public EntityPM: TariffLinePM;
    public DataContext: any;
    public ObjectTableName: string = "TariffLine";
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];
    public OriginDependencyFilterValue: string = "A";
    public DestinationDependencyFilterValue = "A";
    public ViaDependencyFilterValue = "A";
    public IsViaFieldVisible: boolean = true;
    public IsPortsVisible: boolean = true;
    public IsAir: boolean = false;
    public UnitOfMeasurementCode: string = "";
    constructor() {
        
    }

    SetWindowArgs(args) {
        this.DataContext = args['DataContext'];
        this.EntityPM = args['EntityPM'];
        this.TariffType = args['TariffType'];
        this.UnitOfMeasurementCode = args['UnitOfMeasurementCode'];
        this.SetOriginDependencyFilterValue();
        this.GetTariffType();
        this.SetFieldsVisiblity();
        this.SetUnitOfMeasurementCode();
        this.Clone();
    }

    SetFieldsVisiblity() {
        if (this.TariffType == "OFS" || this.TariffType == "OSC" || this.TariffType == "ASC" || this.TariffType == "ICC" || this.TariffType == "ECC" || this.TariffType == "IFT" || this.TariffType == "ICS" || this.TariffType == "ECS") {
            this.IsViaFieldVisible = false;

            if (this.TariffType == "ICC" || this.TariffType == "ECC" || this.TariffType == "ICS" || this.TariffType == "ECS") {
                this.IsPortsVisible = false;
            }
        }
    }

    SetOriginDependencyFilterValue() {
        if (this.TariffType == "OLC" || this.TariffType == "OSC" || this.TariffType == "OFC" || this.TariffType == "OFS") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
            this.ViaDependencyFilterValue = "O";
        }

        else if (this.TariffType == "IFT") {
            this.OriginDependencyFilterValue = "I";
            this.DestinationDependencyFilterValue = "I";
            this.ViaDependencyFilterValue = "I";
        }
    }
    
    GetTariffType() {
        if (this.TariffType == "AFC" || this.TariffType == "ASC") {
            this.IsAir = true;
        }
    }

    SetUnitOfMeasurementCode() {
        if (this.TariffType == "AFC" || this.TariffType == "OLC") {
            this.EntityPM.UnitOfMeasurementCode = this.UnitOfMeasurementCode;
        }
    }

    GetDisplayMemberPath() {
        return this.IsAir ? "Code" : "CombinedCode";
    }

    get OriginPortText() { return this.EntityPM.OriginPortText; }
    get DestinationPortText() { return this.EntityPM.DestinationPortText; }
    get ViaPortText() { return this.EntityPM.ViaPortText; }
    get MinPriceText() { return this.EntityPM.MinPriceText; }
    get Step1PriceText() { return this.EntityPM.Step1PriceText; }
    get Step2PriceText() { return this.EntityPM.Step2PriceText; }
    get Step3PriceText() { return this.EntityPM.Step3PriceText; }
    get Step4PriceText() { return this.EntityPM.Step4PriceText; }
    get Step5PriceText() { return this.EntityPM.Step5PriceText; }
    get Step6PriceText() { return this.EntityPM.Step6PriceText; }
    get Step7PriceText() { return this.EntityPM.Step7PriceText; }
    get Step8PriceText() { return this.EntityPM.Step8PriceText; }

    get Surcharge1PriceText() { return this.EntityPM.Surcharge1PriceText; }
    get Surcharge2PriceText() { return this.EntityPM.Surcharge2PriceText; }
    get Surcharge3PriceText() { return this.EntityPM.Surcharge3PriceText; }
    get Surcharge4PriceText() { return this.EntityPM.Surcharge4PriceText; }
    get Surcharge5PriceText() { return this.EntityPM.Surcharge5PriceText; }
    get Surcharge6PriceText() { return this.EntityPM.Surcharge6PriceText; }
    get Surcharge7PriceText() { return this.EntityPM.Surcharge7PriceText; }
    get Surcharge8PriceText() { return this.EntityPM.Surcharge8PriceText; }
    get Surcharge9PriceText() { return this.EntityPM.Surcharge9PriceText; }
    get Surcharge10PriceText() { return this.EntityPM.Surcharge10PriceText; }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DataContext.FatherComponent.CurrentVersion.TariffLines.length + 1 > 1000) {
            errors.push("Can't add lines to the tariff bigger than the 1000");
        }

        else {
            var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (this.TariffType == "AFC" || this.TariffType == "OLC" || this.TariffType == "OFC") {
                if (AppTool.IsNullOrEmpty(this.DataContext.DestinationPortId)) {
                    errors.push(msg.replace("%FieldName", "To"));
                }

                if (AppTool.IsNullOrEmpty(this.DataContext.OriginPortId)) {
                    errors.push(msg.replace("%FieldName", "From"));
                }
            }

            else if (this.TariffType == "ASC" || this.TariffType == "OSC" || this.TariffType == "OFS" || this.TariffType == "IFT") {
                if (AppTool.IsNullOrEmpty(this.DataContext.DestinationPortId) && !this.DataContext.IsToAllOtherPorts) {
                    errors.push("To port or To All Other Ports is Required");
                }

                if (AppTool.IsNullOrEmpty(this.DataContext.OriginPortId) && !this.DataContext.IsFromAllOtherPorts) {
                    errors.push("From port or From All Other Ports is Required");
                }

                if (this.DataContext.IsDifferentCurrenciesPerCharge) {
                    if (this.TariffType == "OFS" || this.TariffType == "IFT") {
                        if (this.DataContext.ContainerPricesItemsSource.filter(d => AppTool.IsNullOrEmpty(d.CurrencyId)).length > 0) {
                            errors.push("Some Containers Prices missing Currency");
                        }
                    }

                    else {
                        for (var i = 1; i <= 10; i++) {
                            if (this.DataContext.FatherComponent["Surcharge" + i + "PriceVisibility"]) {
                                if (AppTool.IsNullOrEmpty(this.DataContext["Surcharge" + i + "CurrencyId"])) {
                                    errors.push("Surcharge " + i + " Currency Field is Required");
                                }
                            }
                        }
                    }
                }
                else {
                    if (AppTool.IsNullOrEmpty(this.DataContext.CurrencyId)) {
                        errors.push("Currency Field is Required");
                    }
                }
            }

            else if (this.TariffType == "ICC" || this.TariffType == "ECC") {
                if (AppTool.IsNullOrEmpty(this.DataContext.ToCountryId) && !this.DataContext.IsToAllOtherCountries) {
                    errors.push("To country or to all other countries is required");
                }

                if (AppTool.IsNullOrEmpty(this.DataContext.FromCountryId) && !this.DataContext.IsFromAllOtherCountries) {
                    errors.push("From country or from all other countries is required");
                }

                if (this.DataContext.IsDifferentCurrenciesPerCharge) {
                    for (var i = 1; i <= 10; i++) {
                        if (this.DataContext.FatherComponent["Surcharge" + i + "PriceVisibility"]) {
                            if (AppTool.IsNullOrEmpty(this.DataContext["Surcharge" + i + "CurrencyId"])) {
                                errors.push("Surcharge " + i + " Currency Field is Required");
                            }
                        }
                    }
                }
                else {
                    if (AppTool.IsNullOrEmpty(this.DataContext.CurrencyId)) {
                        errors.push("Currency Field is Required");
                    }
                }
            }

            else if (this.TariffType == "ICS" || this.TariffType == "ECS") {
                if (this.TariffType == "ICS" && AppTool.IsNullOrEmpty(this.DataContext.FromCountryId)) {
                    errors.push("From country or from all other countries is required");
                }
                if (this.TariffType == "ECS" && AppTool.IsNullOrEmpty(this.DataContext.ToCountryId)) {
                    errors.push("To country or to all other countries is required");
                }
                
                if (this.DataContext.IsDifferentCurrenciesPerCharge) {
                    for (var i = 1; i <= 10; i++) {
                        if (this.DataContext.FatherComponent["Surcharge" + i + "PriceVisibility"]) {
                            if (AppTool.IsNullOrEmpty(this.DataContext["Surcharge" + i + "CurrencyId"])) {
                                errors.push("Surcharge " + i + " Currency Field is Required");
                            }
                        }
                    }
                }
                else {
                    if (AppTool.IsNullOrEmpty(this.DataContext.CurrencyId)) {
                        errors.push("Currency Field is Required");
                    }
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.AddedManually = true;
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                
                if (this.DataContext.FatherComponent.CurrentVersion.TariffLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.FatherComponent.CurrentVersion.AddTariffLine(this.EntityPM);
                    this.DataContext.FatherComponent.EntityPM.TariffLinesAdded = true;
                }
            }
            
            if (this.TariffType == "OFS" || this.TariffType == "IFT") {
                if (this.DataContext.ContainerPricesItemsSource) {
                    this.DataContext.ContainerPricesItemsSource.forEach((item) => {
                        if (item.IsNewEntity && (!AppTool.IsNullOrZero(item.Price1) || !AppTool.IsNullOrZero(item.Price2) || !AppTool.IsNullOrZero(item.Price3)
                            || !AppTool.IsNullOrZero(item.Price4) || !AppTool.IsNullOrZero(item.Price5)) || !AppTool.IsNullOrZero(item.CostPrice)
                            || !AppTool.IsNullOrEmpty(item.CurrencyId)) {

                            if (this.EntityPM.ContainersPrices.indexOf(item.EntityPM) == -1) {
                                this.EntityPM.AddTariffLinesContainersPrice(item.EntityPM);
                            }
                        }
                    });
                }
            }

            this.DataContext.FatherComponent.FillTariffLines(this.DataContext.FatherComponent.CurrentVersion.TariffLines);
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('OriginPortId');
        this.myCloner.AddField('OriginPortCode');
        this.myCloner.AddField('OriginPortCombinedCode');
        this.myCloner.AddField('OriginPortName');
        this.myCloner.AddField('DestinationPortId');
        this.myCloner.AddField('DestinationPortCode');
        this.myCloner.AddField('DestinationPortCombinedCode');
        this.myCloner.AddField('DestinationPortName');
        this.myCloner.AddField('ViaPortId');
        this.myCloner.AddField('ViaPortCode');
        this.myCloner.AddField('ViaPortCombinedCode');
        this.myCloner.AddField('ViaPortName');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDate');
        this.myCloner.AddField('FromCountryId');
        this.myCloner.AddField('FromCountryCode');
        this.myCloner.AddField('FromCountryName');
        this.myCloner.AddField('ToCountryId');
        this.myCloner.AddField('ToCountryCode');
        this.myCloner.AddField('ToCountryName');

        if (this.TariffType == "AFC" || this.TariffType == "OLC") {
            this.myCloner.AddField('MinPrice');
            this.myCloner.AddField('Step1Price');
            this.myCloner.AddField('Step2Price');
            this.myCloner.AddField('Step3Price');
            this.myCloner.AddField('Step4Price');
            this.myCloner.AddField('Step5Price');
            this.myCloner.AddField('Step6Price');
            this.myCloner.AddField('Step7Price');
            this.myCloner.AddField('Step8Price');            
        }

        else if (this.TariffType == "ASC" || this.TariffType == "OSC" || this.TariffType == "OFC" || this.TariffType == "OFS" || this.TariffType == "ICC" || this.TariffType == "ECC" || this.TariffType == "IFT" || this.TariffType == "ICS" || this.TariffType == "ECS" ) {
            this.myCloner.AddField('Surcharge1Price');
            this.myCloner.AddField('Surcharge2Price');
            this.myCloner.AddField('Surcharge3Price');
            this.myCloner.AddField('Surcharge4Price');
            this.myCloner.AddField('Surcharge5Price');
            this.myCloner.AddField('Surcharge6Price');
            this.myCloner.AddField('Surcharge7Price');
            this.myCloner.AddField('Surcharge8Price');
            this.myCloner.AddField('Surcharge9Price');
            this.myCloner.AddField('Surcharge10Price');

            this.myCloner.AddField('Surcharge1MinPrice');
            this.myCloner.AddField('Surcharge2MinPrice');
            this.myCloner.AddField('Surcharge3MinPrice');
            this.myCloner.AddField('Surcharge4MinPrice');
            this.myCloner.AddField('Surcharge5MinPrice');
            this.myCloner.AddField('Surcharge6MinPrice');
            this.myCloner.AddField('Surcharge7MinPrice');
            this.myCloner.AddField('Surcharge8MinPrice');
            this.myCloner.AddField('Surcharge9MinPrice');
            this.myCloner.AddField('Surcharge10MinPrice');

            this.myCloner.AddField('Surcharge1CurrencyId');
            this.myCloner.AddField('Surcharge2CurrencyId');
            this.myCloner.AddField('Surcharge3CurrencyId');
            this.myCloner.AddField('Surcharge4CurrencyId');
            this.myCloner.AddField('Surcharge5CurrencyId');
            this.myCloner.AddField('Surcharge6CurrencyId');
            this.myCloner.AddField('Surcharge7CurrencyId');
            this.myCloner.AddField('Surcharge8CurrencyId');
            this.myCloner.AddField('Surcharge9CurrencyId');
            this.myCloner.AddField('Surcharge10CurrencyId');

            this.myCloner.AddField('IsFromAllOtherPorts');
            this.myCloner.AddField('IsToAllOtherPorts');
            this.myCloner.AddField('Index');
            this.myCloner.AddField('OriginPortText');
            this.myCloner.AddField('DestinationPortText');
            this.myCloner.AddField('ViaPortText');
            this.myCloner.AddField('CurrencyId');
            this.myCloner.AddField('CurrencyCode');
            this.myCloner.AddField('IsDifferentCurrenciesPerCharge');
            this.myCloner.AddField('IsFromAllOtherCountries');
            this.myCloner.AddField('IsToAllOtherCountries');
        }

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.CurrentVersion);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ContainerPricesItemsSource);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
