import { AccountingIntegrityCheckPMService } from './../../Services/StandardPMs/AccountingIntegrityCheckPMService';
import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AccountingIntegrityCheckPM } from '../../EntityPMs/AccountingIntegrityCheckPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewIntegrityCheckComponent',
    moduleId: module.id,
    templateUrl: './NewIntegrityCheckComponent.html',
})

export class NewIntegrityCheckComponent extends BaseComponent implements OnInit {
    public EntityPM: AccountingIntegrityCheckPM = new AccountingIntegrityCheckPM();
    public DataContext: NewIntegrityCheckComponent = this;
    public ObjectTableName: string = "AccountingIntegrityCheck";
    public ValidationErrorsList: string[] = [];

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _AccountingIntegrityCheckPMService: AccountingIntegrityCheckPMService = new AccountingIntegrityCheckPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.EntityPM;
        }
    }

    ngOnInit() {

    }

    //#region Properties

    get FromMonthInclusive() { return this.EntityPM.FromMonthInclusive; }
    set FromMonthInclusive(value: Date) {
        if (this.EntityPM.FromMonthInclusive != value) {
            this.EntityPM.FromMonthInclusive = value;
        }
    }

    get ToMonthInclusive() { return this.EntityPM.ToMonthInclusive; }
    set ToMonthInclusive(value: Date) {
        if (this.EntityPM.ToMonthInclusive != value) {
            this.EntityPM.ToMonthInclusive = value;
        }
    }
    //#endregion

    OkButtonClicked() {
        var errors: string[] = [];


        // Required check
        if (AppTool.IsNullOrEmpty(this.FromMonthInclusive) || AppTool.IsNullOrEmpty(this.ToMonthInclusive)) {
            errors.push(TextCodeTranslator.Translate("Accounting.General.O.AllFieldsRequired"));
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {
            this.SubmitChanges();
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    SubmitChanges() {

        var errors: string[] = [];
        if (errors.length == 0) {

            if (this.EntityPM != null) {

                this.CurrentSession.StartBusyIndicatorSaving();
                this._AccountingIntegrityCheckPMService.insert(this.EntityPM).subscribe(myResult => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }

        } else {
            this.ValidationErrorsList = errors;
        }
    }


    //#endregion
}
