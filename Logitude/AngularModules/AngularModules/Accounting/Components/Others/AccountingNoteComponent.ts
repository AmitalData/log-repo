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

     _AccountingNotePMService: AccountingNotePMService = new AccountingNotePMService();




    constructor () {
        super();
        if(ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }

    SetWindowArgs(args: any) {
        if (args != null) {

            this.entityPM = args.AccountingNotePM;
            this.accountPM = args.AccountPM;
            if(this.entityPM)
            {
                this.isEditForm = true;
            }
            else
            {
                // new
                this.entityPM = new AccountingNotePM();
                if(this.accountPM.CardId) {
                    this.entityPM.Tenant = SessionLocator.Tenant;
                    this.entityPM.CardId = this.accountPM.CardId;
                    this.entityPM.CreateDate = new Date();
                    this.entityPM.UpdateDate= new Date();
                    this.entityPM.CreatedByUserId = "will";
                    this.entityPM.UpdatedByUserId = "replaced";
                }else{
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push("No card id in selected gl account!!!!!!!!!!");
                    return;
                }

            }
        }
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
    }

    //#region Properties

    public get Notes() : string {
        return this.entityPM.Notes;
    }
    public set Notes(v : string) {
        this.entityPM.Notes = v;
    }


    //#endregion

    //#region Buttons Handlers
    OkButtonClicked()
    {
        if(this.isEditForm)
        {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.update(this.entityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    SessionLocator.CurrentSession.CloseCurrentWindow();

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            });
        }
        else
        {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            this._AccountingNotePMService.insert(this.entityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    SessionLocator.CurrentSession.CloseCurrentWindow();

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    //#endregion

}
