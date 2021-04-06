import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {AccountingSettingPM} from '../../../Common/EntityPMs/AccountingSettingPM';
import {AccountingSettingPMService} from '../../../Common/Services/StandardPMs/AccountingSettingPMService';
import {GlobalDomainService} from '../../../Common/Services/GlobalDomainService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectsUpdater} from '../../../Infrastructure/Locators/ObjectsUpdater';

@Component({
    selector: 'AccountingTransferComponent',
    
    templateUrl: './ExternalAccountingSystemComponent.html',
})

export class ExternalAccountingSystemComponent extends BaseComponent {
    public EntityPM: AccountingSettingPM;
    public DataContext = this;
    public ObjectTableName: string = "AccountingSetting";
    public ValidationErrorsList: string[];
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityResourceService: EntityResourceService) {
        super();

        entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe((res:any) => {
            this.InitializeServices();
            this.LoadData();
        });
    }

    private entityPMService: AccountingSettingPMService;
    private myGlobalDomainService: GlobalDomainService;
    InitializeServices() {
        this.entityPMService = new AccountingSettingPMService();
        this.myGlobalDomainService = new GlobalDomainService();
    }

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;
            }

            this.IsResourcesReady = true;
            this.SetUIProperties();
            this.SetQuickBookProperties();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    SetUIProperties() {

    }

    public get AccountingSystemCode() { return this.EntityPM.AccountingSystemCode; }
    public set AccountingSystemCode(value: string) {
        if (this.EntityPM.AccountingSystemCode != value) {
            this.EntityPM.AccountingSystemCode = value;
        }
    }

    public isQBO: boolean = false;
    public isLogedInQBO: boolean = false;
    SetQuickBookProperties() {
        this.isLogedInQBO = false;
    }

    private IsQuickBooksWindowOpened: boolean = false;
    DissConnectQBO() {        
        this.EntityPM.QBOrealMeID = null;

        this.CurrentSession.StartBusyIndicator("Disconnecting..");

        this.entityPMService.update(this.EntityPM).subscribe((myResponse1: ServiceResponse) => {
            if (myResponse1.HasError) {
                this.ValidationErrorsList = myResponse1.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                ObjectsUpdater.UpdateAccountingSettingPM(this.EntityPM);

                this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        SessionLocator.AccountingSystemPM = myResponse2.Result;
                    }
                });

                this.SetQuickBookProperties();
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    ViewXMLClicked() {
        var link = AppTool.GetLogitudeURL() + "QuickbooksOnlineAuth2.aspx?connect=true&tenant=" + SessionLocator.Tenant;
        window.open(link, '_blank', "location = 1, status = 1, scrollbars = 1, width = 400, height = 400");
        this.IsQuickBooksWindowOpened = true;
    }
    SelectedItemChanged(AccountingSystem) {
        if (AccountingSystem.Code == "QBO") {
            this.isQBO = true;
        }

        else {
            this.isQBO = false;
        }
    }
    RunConnectQuickBooks() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "";
        logWindow.Show('./Invoice/Components/Workspaces/QuickBooksLogin');
        logWindow.WindowClosed.subscribe(s => {

            if (s) {
                //this.LoadAllScreenData();
            }
        });
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        if (this.AccountingSystemCode != "QBO") {
            if (this.EntityPM.QBOrealMeID) {
                this.EntityPM.QBOrealMeID = null;
            }
        }

        if (this.IsQuickBooksWindowOpened && this.AccountingSystemCode == "QBO") {
            this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    var loadedEntity: AccountingSettingPM = myResponse.Result;

                    if (this.EntityPM.QBOrealMeID != loadedEntity.QBOrealMeID) {
                        this.EntityPM.QBOrealMeID = loadedEntity.QBOrealMeID;
                    }

                    this.SaveChanges();
                }
            });
        }

        else {
            this.SaveChanges();
        }
    }

    SaveChanges() {
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            this.entityPMService.update(this.EntityPM).subscribe((myResponse1: ServiceResponse) => {
                if (myResponse1.HasError) {
                    this.ValidationErrorsList = myResponse1.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    ObjectsUpdater.UpdateAccountingSettingPM(this.EntityPM);

                    this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe((myResponse2: ServiceResponse) => {
                        if (!myResponse2.HasError) {
                            SessionLocator.AccountingSystemPM = myResponse2.Result;
                        }
                    });

                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
