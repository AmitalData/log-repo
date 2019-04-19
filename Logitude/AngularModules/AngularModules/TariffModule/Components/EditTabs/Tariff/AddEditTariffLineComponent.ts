import { Component } from '@angular/core';
import { TariffLineData } from './TariffGeneralTabComponent';
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
        this.Clone();
    }

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

                if (this.DataContext.FatherComponent.EntityPM.TariffLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.FatherComponent.EntityPM.AddTariffLine(this.EntityPM);
                }
            }

            this.DataContext.FatherComponent.LoadTariffLines();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('OriginPortId');
        this.myCloner.AddField('DestinationPortId');
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
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
