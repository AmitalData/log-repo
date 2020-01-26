import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { PriceStepList } from '../../../Infrastructure/EntityLists/PriceStepList';

@Component({
    moduleId: module.id,
    templateUrl: './SelectBreaksComponent.html',
})

export class SelectBreaksComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "PriceStep";
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];

    private selectedPriceStepId: string = null;
    public get SelectedPriceStepId() { return this.selectedPriceStepId; }
    public set SelectedPriceStepId(value: string) {
        if (this.selectedPriceStepId != value) {
            this.selectedPriceStepId = value;
        }
    }

    private selectedPriceStepList: PriceStepList = null;
    public get SelectedPriceStepList() { return this.selectedPriceStepList; }
    public set SelectedPriceStepList(value: PriceStepList) {
        if (this.selectedPriceStepList != value) {
            this.selectedPriceStepList = value;
        }
    }

    CancelButtonClicked() {
        //this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];

        if (!this.SelectedPriceStepList) {
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var field = "PriceStep";
            errors.push(msg.replace("%FieldName", field));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit(this.SelectedPriceStepList.Steps);
        }
    }
}
