import { BaseComponent } from "../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Component } from "@angular/core";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "../../../Infrastructure/Utilities/TextCodeTranslator";
import { AppTool } from "../../../Infrastructure/Tools";
import { GLAccountExtendedListService } from "../../Services/ExtendedLists/GLAccountExtendedListService";
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";

@Component({
    moduleId: module.id,
    selector: 'FullAccountingAddControlComponent',
    templateUrl: './FullAccountingAddControlComponent.html',
    
})

export class FullAccountingAddControlComponent extends BaseComponent {

    MyChartOfAccountsTypeCode: string = "";
    public DataContext: FullAccountingAddControlComponent = this;

    public ObjectTableName = "GLAccount";
    _ChartOfAccountsId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];

    private _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    _ControlAccountId: string;

    public SetWindowArgs(arg) {
        this._ControlAccountId = arg.ControlAccountId;
        switch (arg.ControlAccountId) {
            case "CustomerControlAccountId": { this.MyChartOfAccountsTypeCode = "3";} break;
            case "VendorControlAccountId": { this.MyChartOfAccountsTypeCode = "4";} break;
            //case "VendorControlAccountId": { this.MyChartOfAccountsTypeCode = "4" } break;
            default:
                {
                    this.MyChartOfAccountsTypeCode = "6"; ///ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject
                }
                break;
        }
        this.CurrentSession.StopBusyIndicator();
    }
    get ChartOfAccountsId() { return this._ChartOfAccountsId; }
    set ChartOfAccountsId(value: string) {
        if (this._ChartOfAccountsId != value) {
            this._ChartOfAccountsId = value;
            
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var FIELD_IS_REQUIERD= TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (AppTool.IsNullOrEmpty(this._ChartOfAccountsId)) {
            var s: string = "קבוצת מאזן שדה חובה"//FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.ChartOfAccountsId"));
            errors.push(s);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            this._GLAccountExtendedListService.GetInsertControlAccount(this._ControlAccountId,this._ChartOfAccountsId)
                .subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult = myResponse.Result;
                        var accId = myResult.AccountId;
                        this.CurrentSession.CloseCurrentWindowEmit(accId);
                    } else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                }
            });

        }
    }
}
