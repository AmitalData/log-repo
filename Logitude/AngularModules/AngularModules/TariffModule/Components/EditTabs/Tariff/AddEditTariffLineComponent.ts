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
    public IsAir: boolean = false;

    constructor() {
        
    }
    
    SetWindowArgs(args) {
        this.DataContext = args['DataContext'];
        this.EntityPM = args['EntityPM'];
        this.TariffType = args['TariffType'];
        this.SetOriginDependencyFilterValue();
        this.GetTariffType();
        this.Clone();
    }

    SetOriginDependencyFilterValue() {
        if (this.TariffType == "OLC" || this.TariffType == "OSC" || this.TariffType == "OFC" || this.TariffType == "OFS") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
        }
    }
    
    GetTariffType() {
        if (this.TariffType == "AFC" || this.TariffType == "ASC") {
            this.IsAir = true;
        }
    }

    GetDisplayMemberPath() {
        return this.IsAir ? "Code" : "CombinedCode";
    }

    get OriginPortText() { return this.EntityPM.OriginPortText; }
    get DestinationPortText() { return this.EntityPM.DestinationPortText; }
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

            else if (this.TariffType == "ASC" || this.TariffType == "OSC" || this.TariffType == "OFS") {
                if (AppTool.IsNullOrEmpty(this.DataContext.DestinationPortId) && !this.DataContext.IsToAllOtherPorts) {
                    errors.push("To port or To All Other Ports is Required");
                }

                if (AppTool.IsNullOrEmpty(this.DataContext.OriginPortId) && !this.DataContext.IsFromAllOtherPorts) {
                    errors.push("From port or From All Other Ports is Required");
                }

                if (AppTool.IsNullOrEmpty(this.DataContext.CurrencyId)) {
                    errors.push("Currency Field is Required");
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
            
            if (this.TariffType == "OFS") {
                if (this.DataContext.ContainerPricesItemsSource) {
                    this.DataContext.ContainerPricesItemsSource.forEach((item) => {
                        if (item.IsNewEntity && (!AppTool.IsNullOrZero(item.Price1) || !AppTool.IsNullOrZero(item.Price2) || !AppTool.IsNullOrZero(item.Price3)
                            || !AppTool.IsNullOrZero(item.Price4) || !AppTool.IsNullOrZero(item.Price5)) || !AppTool.IsNullOrZero(item.CostPrice)) {

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
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDate');

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

        else if (this.TariffType == "ASC" || this.TariffType == "OSC" || this.TariffType == "OFC" || this.TariffType == "OFS") {
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

            this.myCloner.AddField('IsFromAllOtherPorts');
            this.myCloner.AddField('IsToAllOtherPorts');
            this.myCloner.AddField('Index');
            this.myCloner.AddField('OriginPortText');
            this.myCloner.AddField('DestinationPortText');
            this.myCloner.AddField('CurrencyId');
            this.myCloner.AddField('CurrencyCode');
        }

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.CurrentVersion);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
