import { Component } from '@angular/core';
import { TariffLineData } from './VersionTabComponent';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditTariffLineComponent.html',
})

export class AddEditTariffLineComponent  {
    public TariffType: string;
    public EntityPM: TariffLinePM;
    public DataContext: TariffLineData;
    public ObjectTableName: string = "TariffLine";
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];

    constructor() {

    }

    SetDataContext(dataContext: TariffLineData) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.TariffType = dataContext.FatherComponent.EntityPM.TypeCode;
        this.Clone();
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
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.DataContext.DestinationPortId)) {
            errors.push(msg.replace("%FieldName", "To"));
        }
        if (AppTool.IsNullOrEmpty(this.DataContext.OriginPortId)) {
            errors.push(msg.replace("%FieldName", "From"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity) {

                this.DataContext.IsNewEntity = false;

                if (this.DataContext.FatherComponent.CurrentVersion.TariffLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.FatherComponent.CurrentVersion.AddTariffLine(this.EntityPM);
                    this.DataContext.FatherComponent.EntityPM.TariffLinesAdded = true;
                }
            }

            this.DataContext.FatherComponent.FillTariffLines();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('OriginPortId');
        this.myCloner.AddField('OriginPortCode');
        this.myCloner.AddField('OriginPortName');
        this.myCloner.AddField('DestinationPortId');
        this.myCloner.AddField('DestinationPortCode');
        this.myCloner.AddField('DestinationPortName');
        this.myCloner.AddField('MinPrice');
        this.myCloner.AddField('Step1Price');
        this.myCloner.AddField('Step2Price');
        this.myCloner.AddField('Step3Price');
        this.myCloner.AddField('Step4Price');
        this.myCloner.AddField('Step5Price');
        this.myCloner.AddField('Step6Price');
        this.myCloner.AddField('Step7Price');
        this.myCloner.AddField('Step8Price');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
