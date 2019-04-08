import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {MessagingStockPM} from '../../../../Shipment/EntityPMs/MessagingStockPM';
import {MessagingStockPMService} from '../../../../Shipment/Services/StandardPMs/MessagingStockPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantManagementAWBStockTabComponent, StockArgs} from './TenantManagementAWBStockTabComponent';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAWBStockComponent.html',
})

export class AddEditAWBStockComponent extends BaseComponent {
    public EntityPM: MessagingStockPM;
    public DataContext: AddEditAWBStockComponent = this;
    public ObjectTableName: string = "MessagingStock";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    private IsNew: boolean;
    private FatherComponent: TenantManagementAWBStockTabComponent;
    SetWindowArgs(windowArgs: StockArgs) {
        this.IsNew = windowArgs.IsNewEntity;
        this.FatherComponent = windowArgs.FatherComponent;

        if (windowArgs.IsNewEntity) {
            this.EntityPM = new MessagingStockPM();
            this.EntityPM.TenantNumber = windowArgs.FatherComponent.EntityPM.Id;
            this.EntityPM.DummyTenant = SessionLocator.Tenant;
            this.EntityPM.StockType = "Champ";
        }

        else {
            this.LoadEntityPM(windowArgs.EntityId);
        }

        this.SetUIProperties();
        this.Clone();
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("TenantNumber", this.ObjectTableName, false);

        if (this.StartDate == null) {
            this.UIProperties.SetRequired("StartDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("StartDate", this.ObjectTableName, false);
        }

        if (this.EndDate == null) {
            this.UIProperties.SetRequired("EndDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("EndDate", this.ObjectTableName, false);
        }

        if (this.Amount == null) {
            this.UIProperties.SetRequired("Amount", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("Amount", this.ObjectTableName, false);
        }
    }

    private LoadEntityPM(id: string) {
        var myService: MessagingStockPMService = new MessagingStockPMService();

        myService.get(id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;

                    if (this.EntityPM != null) {
                        this.SetUIProperties();
                    }
                }
            }
        });
    }

    get TenantNumber() { return this.EntityPM != null ? this.EntityPM.TenantNumber : 0; }
    set TenantNumber(newValue: number) {
        if (this.EntityPM.TenantNumber != newValue) {
            this.EntityPM.TenantNumber = newValue;

            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    get StartDate() { return this.EntityPM != null ? this.EntityPM.StartDate : null; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;

            this.SetUIProperties();
            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    get EndDate() { return this.EntityPM != null ? this.EntityPM.EndDate : null; }
    set EndDate(newValue: Date) {
        if (this.EntityPM.EndDate != newValue) {
            this.EntityPM.EndDate = newValue;

            this.SetUIProperties();
            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    get Amount() { return this.EntityPM != null ? this.EntityPM.Amount : 0; }
    set Amount(newValue: number) {
        if (this.EntityPM.Amount != newValue) {
            this.EntityPM.Amount = newValue;

            this.SetUIProperties();
            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    get TotalPrice() { return this.EntityPM != null ? this.EntityPM.TotalPrice : 0; }
    set TotalPrice(newValue: number) {
        if (this.EntityPM.TotalPrice != newValue) {
            this.EntityPM.TotalPrice = newValue;

            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    get Notes() { return this.EntityPM != null ? this.EntityPM.Notes : null; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;

            this.EntityPM.IsOtherFieldsChanged = true;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var isValidating: boolean = true;
        if (!this.DataContext.IsNew) {
            if (this.DataContext.EntityPM.IsTotalPriceChanged) {
                if (!this.DataContext.EntityPM.IsOtherFieldsChanged) {
                    isValidating = false;
                }
            }
        }

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];

        if (isValidating) {
            Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);

            if (this.DataContext.EntityPM.StartDate != null && this.DataContext.EntityPM.EndDate != null) {
                var todayDate = DateTool.GetCurrentDateAsUtc();

                if (this.DataContext.EntityPM.EndDate.valueOf() < todayDate.valueOf()) {
                    errors.push("End date cant be past date");
                }

                else if (this.DataContext.EntityPM.EndDate.valueOf() <= this.DataContext.EntityPM.StartDate.valueOf()) {
                    errors.push("End date must be bigger than start date");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNew) {
                this.CurrentSession.StartBusyIndicatorCreating();
            }

            else {
                this.CurrentSession.StartBusyIndicator("Updating...");
            }

            this.Submit();
        }
    }

    private Submit() {
        if (this.DataContext.EntityPM.IsDirty) {
            var myService: MessagingStockPMService = new MessagingStockPMService();

            if (this.DataContext.EntityPM.Id == null) {
                myService.insert(this.DataContext.EntityPM).subscribe(myResult => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.FatherComponent.BuilItemsSource();
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();

                    }
                }
                    , error => {
                        this.CurrentSession.StopBusyIndicator();
                    });
            }

            else {
                myService.update(this.DataContext.EntityPM).subscribe(myResult => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.FatherComponent.BuilItemsSource();
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();

                    }
                }
                    , error => {
                        this.CurrentSession.StopBusyIndicator();
                    });
            }
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('TenantNumber');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('EndDate');
        this.myCloner.AddField('Amount');
        this.myCloner.AddField('TotalPrice');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.DataContext.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
