import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CurrencyList} from '../../../EntityLists/CurrencyList';
import {CommonDomainService} from '../../../Services/CommonDomainService';
import {CustomerFieldsUpdateSettingPMService} from '../../../Services/StandardPMs/CustomerFieldsUpdateSettingPMService';
import {CustomerFieldsUpdateSettingPM} from '../../../EntityPMs/CustomerFieldsUpdateSettingPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

declare var window: any;

@Component({
    selector: 'AccountingSettingsComponent',
    moduleId: module.id,
    templateUrl: './AddEditCustomerFieldsUpdateSettingComponent.html',
})

export class AddEditCustomerFieldsUpdateSettingComponent extends BaseComponent {

    public DataContext: AddEditCustomerFieldsUpdateSettingComponent = this;
    customerFieldsUpdateSettingPMService: CustomerFieldsUpdateSettingPMService = new CustomerFieldsUpdateSettingPMService();
    EntityPM: CustomerFieldsUpdateSettingPM;
    IsNewEntity: boolean = false;
    public ValidationErrorsList: string[];
    EntityId: any;
    public ObjectTableName: string = "CustomerFieldsUpdateSetting";
    public LabelColumnWidth: number = 100;
     
    entityResourceService: EntityResourceService;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.entityResourceService = new EntityResourceService();
        this.UpdateDirectionLists = [];
        this.UpdateDirectionLists.push(new UpdateDirection("NOUP", "No Update"));
        this.UpdateDirectionLists.push(new UpdateDirection("UNFU", "UNF updates Logitude"));



    }

    public ObjectFieldPMLists: ObjectFieldPM[];
    public UpdateDirectionLists: UpdateDirection[];
   
    private selectedUpdateDirection: UpdateDirection;
    public get SelectedUpdateDirection() {
        if (this.EntityPM) {
            this.selectedUpdateDirection = this.UpdateDirectionLists.filter(t => t.Code === this.EntityPM.UpdateDirection)[0];
        }

        return this.selectedUpdateDirection;

    }
    public set SelectedUpdateDirection(newValue: UpdateDirection) {
        if (this.selectedUpdateDirection != newValue) {
            this.selectedUpdateDirection = newValue;
            this.EntityPM.UpdateDirection = newValue.Code;

        }
    }

    private selectedObjectField: ObjectFieldPM;
    public get SelectedObjectField() {
        if (this.EntityPM) {
            this.selectedObjectField = this.ObjectFieldPMLists.filter(t => t.Id === this.EntityPM.ObjectFieldId)[0];
        }

        return this.selectedObjectField;

    }
    public set SelectedObjectField(newValue: ObjectFieldPM) {
        if (this.selectedObjectField != newValue) {
            this.selectedObjectField = newValue;
            this.EntityPM.ObjectFieldId = newValue.Id;

        }
    }



    SetWindowArgs(args: any) {
        if (args != null) {
            this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(res1 => {
                this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(res2 => {

                    var objectTablePM: ObjectTablePM = window.ObjectTables.filter(d => d.Name === "Customer")[0];
                    var tableObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === objectTablePM.Id);
                    var m = tableObjectFieldPM.filter(f => f.FieldName === "SalesmanUserId")[0];
                    this.ObjectFieldPMLists = window.ObjectFields.filter(d => d.AllowedInCustomerFieldsSettings === true && d.ObjectTableId === objectTablePM.Id);
                    this.EntityPM = args.EntityPM;
                    this.EntityId = args.EntityId;
                    if (args.IsNew) {

                        this.IsNewEntity = true;

                        this.EntityPM = this.customerFieldsUpdateSettingPMService.GetNewEntityPM();
                        this.EntityPM.Tenant = SessionLocator.Tenant;

                        this.IsResourcesReady = true;
                    }
                    else {

                        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
                        if (!AppTool.IsNullOrEmpty(this.EntityId)) {
                            this.customerFieldsUpdateSettingPMService.get(this.EntityId).subscribe(response => {
                                this.CurrentSession.StopBusyIndicator();

                                var pmResponse: ServiceResponse = response;
                                if (!pmResponse.HasError && pmResponse.Result) {
                                    this.EntityPM = pmResponse.Result;

                                    this.SelectedObjectField = this.ObjectFieldPMLists.filter(d => d.Id == this.EntityPM.ObjectFieldId)[0];
                                    this.SelectedUpdateDirection = this.UpdateDirectionLists.filter(t => t.Code === this.EntityPM.UpdateDirection)[0];
                                }

                                this.IsResourcesReady = true;
                            });
                        }
                    }
                });
            });
        }
    }

    OkButtonClicked() {
        var errors: string[] = [];

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        //// Required check
        //if (AppTool.IsNullOrEmpty(this.DepositBankAccountId) || AppTool.IsNullOrEmpty(this.CashBookId)) {
        //    errors.push("All Fields Required!");
        //} else {
        //    this.CheckIfThereIsCheques(this.CashBookId);
        //}

       // if (errors.length > 0) {
            this.ValidationErrorsList = errors;
       // }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            if (this.IsNewEntity) {
                this.customerFieldsUpdateSettingPMService.insert(this.EntityPM).subscribe(response => {

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        this.ValidationErrorsList = response.ErrorsArray;
                    }

                });
            } else {
                this.customerFieldsUpdateSettingPMService.update(this.EntityPM).subscribe(response => {

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        this.ValidationErrorsList = response.ErrorsArray;
                    }

                });
            }
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class UpdateDirection {
    constructor(public Code: string, public Name: string) {

    }
}
