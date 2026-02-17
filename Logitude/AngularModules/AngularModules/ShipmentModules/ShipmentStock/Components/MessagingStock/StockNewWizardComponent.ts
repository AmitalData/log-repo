import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {DateTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {MessagingStockPM} from '../../../../Shipment/EntityPMs/MessagingStockPM';
import {MessagingStockPMService} from '../../../../Shipment/Services/StandardPMs/MessagingStockPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './StockNewWizardComponent.html',
})

export class StockNewWizardComponent extends BaseComponent {
    public ObjectTableName: string = "MessagingStock";
    public DataContext: StockNewWizardComponent = this;
    public ValidationErrorsList: string[] = [];
    public TenantsList: CodeNameClass[];
    public StockTypesList: CodeNameClass[];
    public EntityPM: MessagingStockPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantsList = [];
        this.StockTypesList = [];
        this.StockTypesList.push(new CodeNameClass(1, 'Champ'));
        this.StockTypesList.push(new CodeNameClass(2, 'INTTRA'));
        this.CreateEntityPM();
        this.LoadAWBTenants();
        this.SetUIProperties();
    }

    private CreateEntityPM() {
        this.EntityPM = new MessagingStockPM();
        this.EntityPM.DummyTenant = InfraSettings.TenantPM.Id;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = this.EntityPM.CreateDate;
        this.EntityPM.CreatedByUserId = SessionInfo.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionInfo.LoggedUserId;
    }

    private myDomainService: GlobalDomainService;
    private LoadedTenantsList: any[] = [];
    private LoadAWBTenants() {
        this.TenantsList = [];

        if (this.myDomainService == null) {
            this.myDomainService = new GlobalDomainService();
        }

        this.myDomainService.GetMessagingStockTenantsList(InfraSettings.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                this.LoadedTenantsList = myResponse.Result;
            }
        });
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("StockType", this.ObjectTableName, this.SelectedStockTypeItem == null ? true : false);
        this.UIProperties.SetRequired("TenantNumber", this.ObjectTableName, this.SelectedTenantItem == null ? true : false);
    }

    public SelectedTenantItem: CodeNameClass = null;
    public SelectedStockTypeItem: CodeNameClass = null;
    SelectedItemChanged(item: CodeNameClass) {
        if (this.SelectedTenantItem != item) {
            this.SelectedTenantItem = item;

            var myResult: number = null;

            if (item != null) {
                myResult = item.Code;
            }

            this.TenantNumber = myResult;
        }
    }
    SelectedStockTypeChanged(item: CodeNameClass) {
        if (this.SelectedStockTypeItem != item) {
            this.SelectedStockTypeItem = item;

            var myResult: string = null;

            if (item != null) {
                myResult = item.Name;
            }

            this.StockType = myResult;
        }
    }

    get StockType() { return this.EntityPM.StockType; }
    set StockType(value: string) {
        if (this.EntityPM.StockType != value) {
            this.EntityPM.StockType = value;
            this.SetUIProperties();

            this.TenantsList = [];
            this.SelectedItemChanged(null);

            this.LoadedTenantsList.forEach(item => {
                if (value == "Champ") {
                    if (item.IsAWBStockPrepaid) {
                        this.TenantsList.push(new CodeNameClass(item.Id, item.Name));
                    }
                }

                else {
                    if (item.IsINTTRAStockPrepaid) {
                        this.TenantsList.push(new CodeNameClass(item.Id, item.Name));
                    }
                }                
            });
        }
    }

    get TenantNumber() { return this.EntityPM.TenantNumber; }
    set TenantNumber(newValue: number) {
        if (this.EntityPM.TenantNumber != newValue) {
            this.EntityPM.TenantNumber = newValue;
            this.SetUIProperties();
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;
        }
    }

    get EndDate() { return this.EntityPM.EndDate; }
    set EndDate(newValue: Date) {
        if (this.EntityPM.EndDate != newValue) {
            this.EntityPM.EndDate = newValue;
        }
    }

    get Amount() { return this.EntityPM.Amount; }
    set Amount(newValue: number) {
        if (this.EntityPM.Amount != newValue) {
            this.EntityPM.Amount = newValue;
        }
    }

    get Remaining() { return this.EntityPM.Remaining; }
    set Remaining(newValue: number) {
        if (this.EntityPM.Remaining != newValue) {
            this.EntityPM.Remaining = newValue;
        }
    }

    get TotalPrice() { return this.EntityPM.TotalPrice; }
    set TotalPrice(newValue: number) {
        if (this.EntityPM.TotalPrice != newValue) {
            this.EntityPM.TotalPrice = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.SelectedTenantItem == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("MessagingStock.F.TenantNumber")));
        }

        if (this.StartDate != null && this.EndDate != null) {
            var todayDate: Date = DateTool.GetCurrentDateAsUtc();

            if (this.EndDate.valueOf() < todayDate.valueOf()) {
                errors.push("End date cant be past date");
            }

            else if (this.EndDate <= this.StartDate) {
                errors.push("End date must be bigger than start date");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreating();
        }
    }

    private SubmitCreating() {
        this.CurrentSession.StartBusyIndicatorCreating();

        var myService: MessagingStockPMService = new MessagingStockPMService();
        myService.insert(this.EntityPM).subscribe((myResult: any) => {

            var mm: any = myResult;
            if (!mm.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('OK');
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        }
            , error => {
                this.CurrentSession.StopBusyIndicator();
                var dd: any = error;
                console.log(dd.text);
            });
    }
}

class CodeNameClass {
    public Code: number;
    public Name: string;    
    constructor(code: number, name: string) {
        this.Code = code;
        this.Name = name;
    }
}
