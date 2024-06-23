import { AccountingNotePMService } from './../../Services/StandardPMs/AccountingNotePMService';
import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AccountingNoteList} from '../../EntityLists/AccountingNoteList';
import {AccountingNotePM} from '../../EntityPMs/AccountingNotePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool} from '../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';


@Component({
    selector: 'AccountingNoteComponent',
    moduleId: './Accounting/Components/Others/',
    templateUrl: 'AccountingNoteComponent.html',
})

export class AccountingNoteComponent extends BaseComponent {
    public entityPM: AccountingNotePM;
    public accountPM: GLAccountPM;
    public DataContext: AccountingNoteComponent = this;
    public ObjectTableName: string = "AccountingNote";
    public ValidationErrorsList: string[] = [];
    public isRTL: boolean = false;
    public isEditForm: boolean = false;
    public FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    private IsNotesEmpty: boolean = false;

     _AccountingNotePMService: AccountingNotePMService = new AccountingNotePMService();



    private CurrentSession = SessionLocator.SelectedSession;
    constructor () {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.SetUIProperty();

    }

    SetWindowArgs(args: any) {
        if (args != null) {

            this.entityPM = args.AccountingNotePM;
            this.accountPM = args.EntityPM;
            if(this.entityPM)
            {
                this.isEditForm = true;
            }
            else
            {
                // new
                this.entityPM = new AccountingNotePM();
                var loggedContact = SessionLocator.LoggedUserPM;
                if(this.accountPM.CardId) {
                    this.entityPM.Tenant = SessionLocator.Tenant;
                    this.entityPM.CardId = this.accountPM.CardId;
                    this.entityPM.CreateDate = new Date();
                    this.entityPM.UpdateDate= new Date();
                    this.entityPM.CreatedByUserId = loggedContact.Id;
                    this.entityPM.UpdatedByUserId = loggedContact.Id;
                    this.entityPM.UpdatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName||loggedContact.EnglishName);
                    this.entityPM.CreatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName||loggedContact.EnglishName);

                }else{
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingNote.O.NoCardIdInGLAccount"));
                    return;
                }

            }
        }
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Notes", this.ObjectTableName, true);
    }

    //#region Properties

    public get Notes() : string {
        return this.entityPM.Notes;
    }
    public set Notes(v : string) {
        this.entityPM.Notes = v;
        this.CheckIfNotesEmpty();
        this.SetNotesRequrierdState();
    }


    //#endregion

    //#region Buttons Handlers
    OkButtonClicked()
    {
        this.CheckIfNotesEmpty();
        if(this.isEditForm)
        {
            this.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.update(this.entityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError && !this.IsNotesEmpty) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.SetNotesRequierdErrorMessage();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
        else
        {
            this.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.insert(this.entityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError && !this.IsNotesEmpty) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.SetNotesRequierdErrorMessage();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }


    SetNotesRequrierdState() {
        if (AppTool.IsNullOrEmpty(this.Notes)) {
            this.UIProperties.SetRequired("Notes", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("Notes", this.ObjectTableName, false);
        }
    }
    CheckIfNotesEmpty() {
        if (AppTool.IsNullOrEmpty(this.Notes)) {
            this.IsNotesEmpty = true;
        }
        else {
            this.IsNotesEmpty = false;
        }
    }

    SetNotesRequierdErrorMessage() {
        if (AppTool.IsNullOrEmpty(this.Notes)) {
            var requierdErrorMessage: string = this.AddFieldNameToErrorMessage();
            this.ValidationErrorsList.push(requierdErrorMessage);
        }
    }

    private AddFieldNameToErrorMessage(): string {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.Notes"));
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    //#endregion

}
