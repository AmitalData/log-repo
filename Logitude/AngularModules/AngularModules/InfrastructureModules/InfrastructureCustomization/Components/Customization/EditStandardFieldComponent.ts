import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {StandardFieldItem} from './StandardFieldsComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GeneralDomainService, FieldsTranslations, FieldsUpdateHelper} from '../../../../Infrastructure/Services/GeneralDomainService';
import {ObjectFieldPMService} from '../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
declare var window: any;

@Component({
    
    templateUrl: './EditStandardFieldComponent.html',
})

export class EditStandardFieldComponent extends BaseComponent {
    public DataContext: EditStandardFieldComponent = this;
    public EditedFieldItem: StandardFieldItem;
    public ValidationErrorsList: string[] = [];
    private myService: ObjectFieldPMService;
    private generalService: GeneralDomainService;
    public EntityPM: ObjectFieldPM;
    public EntityPMLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.EntityPM = new ObjectFieldPM();
        this.myService = new ObjectFieldPMService();
        this.generalService = new GeneralDomainService();
    }

    SetWindowArgs(args: StandardFieldItem) {
        this.EditedFieldItem = args;  

        this.generalService.GetSingleObjectFieldByFieldCodeFromZeroTenant(args.ObjectFieldCode).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;
                this.EntityPMLoaded = true;

                this.SetUIProperties();
                this.BuildContolFieldsLists();
            }
        });
    }

    public IsMaxLengthEnabled: boolean = true;
    public IsControlFieldsVisible: boolean = false;
    public IsFullLabelEnabled: boolean = false;
    public IsShortLabelEnabled: boolean = false;
    public IsListHeaderLabelEnabled: boolean = false;
    public IsHelpTextEnabled: boolean = false;
    public IsRequieredEnabled: boolean = true;
    public IsShowMaxLength: boolean = true;


    private SetUIProperties() {
        var isMaxLengthEnabled: boolean = true;
        var isControlFieldsVisible: boolean = false;
        var isFullLabelEnabled: boolean = false;
        var isShortLabelEnabled: boolean = false;
        var isListHeaderLabelEnabled: boolean = false;
        var isHelpTextEnabled: boolean = false;
        var isRequieredEnabled: boolean = true;
        var isShowMaxLength: boolean = false;

        if (this.EntityPM.DataTypeCode == "LookUp" || this.EntityPM.DataTypeCode == "DateTime") {
            isMaxLengthEnabled = false;
        }

        if (this.EntityPM.DataTypeCode == "Text" || this.EntityPM.DataTypeCode == "nText") {
            isShowMaxLength = true;
        }


        if (this.EntityPM.DataTypeCode == "LookUp") {
            isControlFieldsVisible = true;
        }

        if (this.EditedFieldItem.fullLabelObject != null) {
            isFullLabelEnabled = true;
        }

        if (this.EditedFieldItem.shortLabelObject != null) {
            isShortLabelEnabled = true;
        }

        if (this.EditedFieldItem.listLabelObject != null) {
            isListHeaderLabelEnabled = true;
        }

        if (this.EditedFieldItem.helpLabelObject != null) {
            isHelpTextEnabled = true;
        }

        if (this.EntityPM.TenantZeroIsRequired) {
            isRequieredEnabled = false;
        }
        this.IsShowMaxLength = isShowMaxLength;
        this.IsMaxLengthEnabled = isMaxLengthEnabled;
        this.IsControlFieldsVisible = isControlFieldsVisible;
        this.IsFullLabelEnabled = isFullLabelEnabled;
        this.IsShortLabelEnabled = isShortLabelEnabled;
        this.IsListHeaderLabelEnabled = isListHeaderLabelEnabled;
        this.IsHelpTextEnabled = isHelpTextEnabled;
        this.IsRequieredEnabled = isRequieredEnabled;
    }

    public ContolFieldsList1: ObjectFieldPM[];
    public ContolFieldsList2: ObjectFieldPM[];
    private BuildContolFieldsLists() {
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];

        var lookupTable: ObjectTablePM = window.ObjectTables.filter(t => t.Id == this.EntityPM.LookUpTableId)[0];

        if (lookupTable != null) {
            this.ContolFieldsList1 = this.EditedFieldItem.loadedFields.filter(f => f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.EntityPM.ObjectTableId);
            this.ContolFieldsList2 = this.EditedFieldItem.loadedFields.filter(f => f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.EntityPM.ObjectTableId);
        }
    }

    get SelectedControlFieldItem1() {
        var controlField1: ObjectFieldPM = null;
        if (this.EntityPM.ControlField1 != null) {
            controlField1 = this.ContolFieldsList1.filter(f => f.FieldName == this.EntityPM.ControlField1)[0];
        }

        return controlField1;
    }
    set SelectedControlFieldItem1(newValue: ObjectFieldPM) {
        var controlField1: ObjectFieldPM = newValue;

        if (controlField1 != null) {
            this.EntityPM.ControlField1 = controlField1.FieldName;
        }

        else {
            this.EntityPM.ControlField1 = null;
        }
    }

    get SelectedControlFieldItem2() {
        var controlField2: ObjectFieldPM = null;
        if (this.EntityPM.ControlField2 != null) {
            controlField2 = this.ContolFieldsList2.filter(f => f.FieldName == this.EntityPM.ControlField2)[0];
        }

        return controlField2;
    }
    set SelectedControlFieldItem2(newValue: ObjectFieldPM) {
        var controlField2: ObjectFieldPM = newValue;

        if (controlField2 != null) {
            this.EntityPM.ControlField2 = controlField2.FieldName;
        }

        else {
            this.EntityPM.ControlField2 = null;
        }
    }

    get FullLabelText() { return this.EditedFieldItem.fullLabelObject == null ? "" : this.EditedFieldItem.fullLabelObject.TranslatedText; }
    set FullLabelText(newValue: string) {
        if (this.EditedFieldItem.fullLabelObject != null) {
            if (this.EditedFieldItem.fullLabelObject.TranslatedText != newValue) {
                this.EditedFieldItem.fullLabelObject.TranslatedText = newValue;
            }
        }
    }

    get ShortLabelText() { return this.EditedFieldItem.shortLabelObject == null ? "" : this.EditedFieldItem.shortLabelObject.TranslatedText; }
    set ShortLabelText(newValue: string) {
        if (this.EditedFieldItem.shortLabelObject != null) {
            if (this.EditedFieldItem.shortLabelObject.TranslatedText != newValue) {
                this.EditedFieldItem.shortLabelObject.TranslatedText = newValue;
            }
        }
    }

    get ListHeaderLabelText() { return this.EditedFieldItem.listLabelObject == null ? "" : this.EditedFieldItem.listLabelObject.TranslatedText; }
    set ListHeaderLabelText(newValue: string) {
        if (this.EditedFieldItem.listLabelObject != null) {
            if (this.EditedFieldItem.listLabelObject.TranslatedText != newValue) {
                this.EditedFieldItem.listLabelObject.TranslatedText = newValue;
            }
        }
    }

    get HelpTextText() { return this.EditedFieldItem.helpLabelObject == null ? "" : this.EditedFieldItem.helpLabelObject.TranslatedText; }
    set HelpTextText(newValue: string) {
        if (this.EditedFieldItem.helpLabelObject != null) {
            if (this.EditedFieldItem.helpLabelObject.TranslatedText != newValue) {
                this.EditedFieldItem.helpLabelObject.TranslatedText = newValue;
            }
        }
    }

    get IsRequiered() { return this.EntityPM.IsRequiered; }
    set IsRequiered(newValue: boolean) {
        if (this.EntityPM.IsRequiered != newValue) {
            this.EntityPM.IsRequiered = newValue;
        }
    }
    
    get MaxLength() { return this.EntityPM.MaxLength; }
    set MaxLength(newValue: number) {
        if (this.EntityPM.MaxLength != newValue) {
            this.EntityPM.MaxLength = newValue;
        }
    }

    CancelButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "ObjectField", errors);

        if (this.EntityPM.TenantZeroMaxLength < this.EntityPM.MaxLength) {
            errors.push("Max Length can't be over " + this.EntityPM.TenantZeroMaxLength);
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            var list: FieldsTranslations[] = [];
            if (this.EditedFieldItem.fullLabelObject != null && this.EditedFieldItem.fullLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.fullLabelObject);
            }

            if (this.EditedFieldItem.shortLabelObject != null && this.EditedFieldItem.shortLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.shortLabelObject);
            }

            if (this.EditedFieldItem.listLabelObject != null && this.EditedFieldItem.listLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.listLabelObject);
            }

            if (this.EditedFieldItem.helpLabelObject != null && this.EditedFieldItem.helpLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.helpLabelObject);
            }

            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();

                this.myService.update(this.EntityPM).subscribe((myResult:any) => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        CachedDataManager.RefreshObjectFieldsModifications();
                        if (list.length == 0) {
                            CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                                this.CurrentSession.StopBusyIndicator();
                                this.CurrentSession.CloseCurrentWindowEmit("Ok");
                            });
                        }
                    }
                });
            }

            if (list.length > 0) {
                var myServiceHelper = new FieldsUpdateHelper();
                myServiceHelper.Tenant = SessionLocator.Tenant;
                myServiceHelper.Items = list;

                var generalService: GeneralDomainService = new GeneralDomainService();
                generalService.UpdateFieldsTranslations(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                            this.CurrentSession.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        });
                    }
                });
            }
        }            
    } 
}
