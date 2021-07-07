import { Component, Output, EventEmitter, OnInit, ComponentRef} from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomsDocumentsDefinitionPM } from '../../../Customs/EntityPMs/CustomsDocumentsDefinitionPM';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsDocumentsDefinitionListService } from '../../../Customs/Services/StandardLists/CustomsDocumentsDefinitionListService';
import { CustomsDocumentsDefinitionPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentsDefinitionPMService';
import { CustomsDocumentsDefinitionExtendedService } from '../../../Customs/Services/ExtendedPMs/CustomsDocumentsDefinitionExtendedService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';


@Component({
    
    templateUrl: './CustomsDocumentsDefinitionComponent.html',
})

export class CustomsDocumentsDefinitionComponent extends BaseComponent implements OnInit {
  public IsDisplayOnly: boolean = false;

    Search: any;
    public DataContext: CustomsDocumentsDefinitionComponent = this;
    public EntityPM: CustomsDocumentsDefinitionPM = new CustomsDocumentsDefinitionPM();
    public ObjectTableName: string = "Customs.CustomsDocumentsDefinition";
    private isControlEnabled: boolean = true;
    public IsLoaded: boolean = false;

    public _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _EntityListService: CustomsDocumentsDefinitionListService = new CustomsDocumentsDefinitionListService();
    private _EntityPMService: CustomsDocumentsDefinitionPMService = new CustomsDocumentsDefinitionPMService();
    private _EntityPMExtendedService: CustomsDocumentsDefinitionExtendedService = new CustomsDocumentsDefinitionExtendedService();

    public AllDocumentsDefinitionResultList: ObservableCollection; 
    public DocumentsDefinitionResultList: ObservableCollection; 
    public DeleteDocumentsDefinitionList: ObservableCollection; 
    public DocumentTypeFilterItems: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();

        this.AllDocumentsDefinitionResultList = new ObservableCollection([]);
        this.DocumentsDefinitionResultList = new ObservableCollection([]);
        this.DeleteDocumentsDefinitionList = new ObservableCollection([]);

        this.DocumentTypeFilterItems = new ApiQueryFilters();
        this.DocumentTypeFilterItems.addAdditionalFilter("Code", "380", null, null, "Exclude", false, false, false, "string", false, true);       
        this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "380", null, null, "NotContains", false, false, false, "string", false, true); 
        this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "380", null, null, "NotContains", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("Code", "271", null, null, "Exclude", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "271", null, null, "NotContains", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "271", null, null, "NotContains", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("Code", "864", null, null, "Exclude", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "864", null, null, "NotContains", false, false, false, "string", false, true);
        this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "864", null, null, "NotContains", false, false, false, "string", false, true);

        this.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this.CurrentSession.StopBusyIndicator();
            this.BuildDocumentsDefinitionList();
            this.IsLoaded = true;
        });
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    ngOnInit() {
        
    }

    BuildDocumentsDefinitionList() {

        this._EntityListService.getAll().subscribe((myResult:any) => {
            console.log("Get All Customs Documents Definition: ", myResult);
            if (myResult != null && myResult.Result != null){
                this.AllDocumentsDefinitionResultList = new ObservableCollection([]);
                this.DocumentsDefinitionResultList = new ObservableCollection([]);
                for (let item of myResult.Result) {
                    this.AllDocumentsDefinitionResultList.Insert(new DocumentsDefinitionPMComponent(item.DocumentTypeCode, item.ProcessTypeCode, item.TransportationTypeCode, item.CargoTypeCode, item.DeclarationTypeCode));
                    this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(item, false));
                }
                return;
            }
        });
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get DocumentTypeCode() { return this.EntityPM.DocumentTypeCode; }
    public set DocumentTypeCode(newValue: string) { this.EntityPM.DocumentTypeCode = newValue; }

    public get ProcessTypeCode() { return this.EntityPM.ProcessTypeCode; }
    public set ProcessTypeCode(newValue: string) { this.EntityPM.ProcessTypeCode = newValue; }

    public get TransportationTypeCode() { return this.EntityPM.TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) { this.EntityPM.TransportationTypeCode = newValue; }

    public get CargoTypeCode() { return this.EntityPM.CargoTypeCode; }
    public set CargoTypeCode(newValue: string) { this.EntityPM.CargoTypeCode = newValue; }

    public get DeclarationTypeCode() { return this.EntityPM.DeclarationTypeCode; }
    public set DeclarationTypeCode(newValue: string) { this.EntityPM.DeclarationTypeCode = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    CheckBeforeSave() {
        var isValide: boolean = true;
        //First Check Befor Save For Duplicates
        if (this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Collection.length > 0) {
            for (let item of this.DocumentsDefinitionResultList.Collection) {
                if (item.DocumentTypeCode == "380" || item.DocumentTypeCode == "271" || item.DocumentTypeCode == "864") {
                    var messageWindow = new MessageWindow();
                    messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                    messageWindow.Width = 250;
                    messageWindow.Height = 150;
                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show("לא ניתן להגדיר סוג מסמך מסוג 380/271/864");
                    isValide = false;
                    break;
                }

                if (this.AllDocumentsDefinitionResultList != null && this.AllDocumentsDefinitionResultList.Length > 0) {
                    var nullVM = this.DocumentsDefinitionResultList.Collection.filter
                        (vm => vm.DocumentTypeCode == item.DocumentTypeCode &&
                            vm.TransportationTypeCode == item.TransportationTypeCode &&
                            vm.CargoTypeCode == item.CargoTypeCode &&
                            vm.ProcessTypeCode == item.ProcessTypeCode &&
                            vm.DeclarationTypeCode == item.DeclarationTypeCode);
                    if (nullVM.length > 1) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show("לא ניתן להזין רשומות כפולות ");
                        isValide = false;
                        break;
                    }

                    var nullAllVM = this.AllDocumentsDefinitionResultList.Collection.filter
                        (vm => vm.DocumentTypeCode == item.DocumentTypeCode &&
                            vm.TransportationTypeCode == item.TransportationTypeCode &&
                            vm.CargoTypeCode == item.CargoTypeCode &&
                            vm.ProcessTypeCode == item.ProcessTypeCode &&
                            vm.DeclarationTypeCode == item.DeclarationTypeCode);
                    if (nullAllVM.length > 1 || (item.IsNew == true && nullAllVM.length >= 1) || nullVM.length > 1) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show("לא ניתן להזין רשומות כפולות ");
                        isValide = false;
                        break;
                    }
                }
            }
        }

        return isValide;
    }

    OkButtonClicked() {

        if (this.CheckBeforeSave() != true) {
            return;
        }

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));  

        if (this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Collection.length > 0) {
                this.DocumentsDefinitionResultList.Collection.forEach((item: DocumentsDefinitionComponent) => {

                    if (item.IsNew == true) {
                        item.entityPM.Tenant = SessionLocator.Tenant; // ????
                        this._EntityPMService.insert(item.entityPM).subscribe((response:any) => {
                            var res: ServiceResponse = response;
                            if (res.HasError) {
                                //this.ValidationErrorsList = res.ErrorsArray;
                                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                return;
                            }
                        });
                    }
                    else {
                        this._EntityPMService.update(item.entityPM).subscribe((response:any) => {
                            var res: ServiceResponse = response;
                            if (res.HasError) {
                                //this.ValidationErrorsList = res.ErrorsArray;
                                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                return;
                            }
                        });
                    }
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
                console.log("..Saved Successfully ");
        }
        if (this.DeleteDocumentsDefinitionList != null && this.DeleteDocumentsDefinitionList.Collection.length > 0) {
            this.DeleteDocumentsDefinitionList.Collection.forEach((item: DocumentsDefinitionComponent) => {
                if (!AppTool.IsNullOrEmpty(item.entityPM.Id)) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
                    this._EntityPMExtendedService.delete(item.entityPM.Id).subscribe((response:any) => {
                        var res: ServiceResponse = response;
                        if (res.HasError) {
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            return;
                        }
                    });
                }
            });
            console.log("..Deleted Successfully ");
        }

        this.CurrentSession.CloseCurrentWindow();
    }

    SortResultList() {

        if (this.DocumentsDefinitionResultList == null ||
            this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Length == 0) {
            return;
        }


        this.DocumentsDefinitionResultList.Collection.sort((a, b) => {
            return (a.CargoTypeCode === b.CargoTypeCode) ? 0 : (a.CargoTypeCode > b.CargoTypeCode) ? -1 : 1
        });
        this.DocumentsDefinitionResultList.Collection.sort((a, b) => {
            return (a.ProcessTypeCode === b.ProcessTypeCode) ? 0 : (a.ProcessTypeCode > b.ProcessTypeCode) ? -1 : 1
        });
        this.DocumentsDefinitionResultList.Collection.sort((a, b) => {
            return (a.TransportationTypeCode === b.TransportationTypeCode) ? 0 : (a.TransportationTypeCode > b.TransportationTypeCode) ? -1 : 1
        });

        this.DocumentsDefinitionResultList.Collection.sort((a, b) => {
            return (a.DocumentTypeCode === b.DocumentTypeCode) ? 0 : (a.DocumentTypeCode > b.DocumentTypeCode) ? -1 : 1
        });
    }

    AddDocumentsDefinitionCommand() {
        this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(new CustomsDocumentsDefinitionPM(), true));
    }

    DeleteDocumentsDefinitionCommand(item: DocumentsDefinitionComponent) {
        this.DocumentsDefinitionResultList.Remove(item);
        if (item.IsNew == false) {
            this.DeleteDocumentsDefinitionList.Insert(item);
        }
    }

    SearchDocumentsDefinitionCommand() {
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("DocumentTypeCode", this.DocumentTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("ProcessTypeCode", this.ProcessTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("TransportationTypeCode", this.TransportationTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("CargoTypeCode", this.CargoTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("DeclarationTypeCode", this.DeclarationTypeCode, null, null, "Equals", false, false, false, "Text");

        this._EntityListService.getByFilters(filters).subscribe((myResult:any) => {
            console.log("Customs Documents Definition: ", myResult);
            if (myResult == null ||
                (myResult != null && myResult.Result == null) ||
                (myResult != null && myResult.Result != null && myResult.Result.length == 0)) {
                var messageWindow = new MessageWindow();
                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("לא נמצאו נתונים");

                this.DocumentsDefinitionResultList.Clear();;
                return;
            }
        

            this.DocumentsDefinitionResultList = new ObservableCollection([]);
            for (let item of myResult.Result) {
                this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(item, false));
            }
            this.SortResultList();
        });
    }
}

export class DocumentsDefinitionComponent extends BaseComponent {
    public ObjectTableName = "Customs.CustomsDocumentsDefinition";
    public DataContext: DocumentsDefinitionComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityPM: CustomsDocumentsDefinitionPM, isNew: boolean) {
        super();
        this.IsNew = isNew;
        if (this.IsNew) {
            this.Mandatory = false;
            this.Inactive = false;
        }

    }

    public get DocumentTypeCode() { return this.entityPM.DocumentTypeCode; }
    public set DocumentTypeCode(newValue: string) { this.entityPM.DocumentTypeCode = newValue; }

    public get DocumentTypeName() { return this.entityPM.DocumentTypeName; }
    public set DocumentTypeName(newValue: string) { this.entityPM.DocumentTypeName = newValue; }

    public get ProcessTypeCode() { return this.entityPM.ProcessTypeCode; }
    public set ProcessTypeCode(newValue: string) { this.entityPM.ProcessTypeCode = newValue; }

    public get ProcessTypeName() { return this.entityPM.ProcessTypeName; }
    public set ProcessTypeName(newValue: string) { this.entityPM.ProcessTypeName = newValue; }

    public get TransportationTypeCode() { return this.entityPM.TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) { this.entityPM.TransportationTypeCode = newValue; }

    public get TransportationTypeName() { return this.entityPM.TransportationTypeName; }
    public set TransportationTypeName(newValue: string) { this.entityPM.TransportationTypeName = newValue; }

    public get CargoTypeCode() { return this.entityPM.CargoTypeCode; }
    public set CargoTypeCode(newValue: string) { this.entityPM.CargoTypeCode = newValue; }

    public get CargoTypeName() { return this.entityPM.CargoTypeName; }
    public set CargoTypeName(newValue: string) { this.entityPM.CargoTypeName = newValue; }

    public get DeclarationTypeCode() { return this.entityPM.DeclarationTypeCode; }
    public set DeclarationTypeCode(newValue: string) { this.entityPM.DeclarationTypeCode = newValue; }

    public get DeclarationTypeName() { return this.entityPM.DeclarationTypeName; }
    public set DeclarationTypeName(newValue: string) { this.entityPM.DeclarationTypeName = newValue; }


    public get Mandatory() { return this.entityPM.Mandatory; }
    public set Mandatory(newValue: boolean) { this.entityPM.Mandatory = newValue; }

    public get Inactive() { return this.entityPM.Inactive; }
    public set Inactive(newValue: boolean) { this.entityPM.Inactive = newValue; }

    private _IsNew: boolean;
    public get IsNew() { return this._IsNew; }
    public set IsNew(newValue: boolean) { this._IsNew = newValue; }

    //private _IsDelete: boolean;
    //public get IsDelete() { return this._IsDelete; }
    //public set IsDelete(newValue: boolean) { this._IsDelete = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
    DocumentTypeLostFocus(logCellTemplate: any, documentTypeCodeLovBox: any) {
        var newValue = documentTypeCodeLovBox.SelectedItemObject == null ? documentTypeCodeLovBox.SearchTextNgModel : documentTypeCodeLovBox.SelectedItemObject.Code;
        if (newValue == "380" || newValue == "271" || newValue == "864") {
            this.DocumentTypeCode = newValue;
            documentTypeCodeLovBox.selectedValue = newValue;
            
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.WindowClosed.subscribe((event: any) => {
                SessionLocator.SustainFocusOnCell = true;
                this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, OnBlurEvent: documentTypeCodeLovBox.OnBlurEvent });
            });
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן להגדיר סוג מסמך מסוג 380/271/864");
            return;
        }
    }


} 

export class DocumentsDefinitionPMComponent extends BaseComponent {
    public ObjectTableName = "Customs.CustomsDocumentsDefinition";
    public DataContext: DocumentsDefinitionPMComponent = this;

    private _DocumentTypeCode: string;
    private _ProcessTypeCode: string;
    private _TransportationTypeCode: string;
    private _CargoTypeCode: string;
    private _DeclarationTypeCode: string;


    constructor(documentTypeCode: string, processTypeCode: string, transportationTypeCode: string, cargoTypeCode: string, declarationTypeCode: string) {
        super();

        this.DocumentTypeCode = documentTypeCode;
        this.ProcessTypeCode = processTypeCode;
        this.TransportationTypeCode = transportationTypeCode;
        this.CargoTypeCode = cargoTypeCode;
        this.DeclarationTypeCode = declarationTypeCode;
    }

    public get DocumentTypeCode() { return this._DocumentTypeCode; }
    public set DocumentTypeCode(newValue: string) { this._DocumentTypeCode = newValue; }

    public get ProcessTypeCode() { return this._ProcessTypeCode; }
    public set ProcessTypeCode(newValue: string) { this._ProcessTypeCode = newValue; }

    public get TransportationTypeCode() { return this._TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) { this._TransportationTypeCode = newValue; }

    public get CargoTypeCode() { return this._CargoTypeCode; }
    public set CargoTypeCode(newValue: string) { this._CargoTypeCode = newValue; }

    public get DeclarationTypeCode() { return this._DeclarationTypeCode; }
    public set DeclarationTypeCode(newValue: string) { this._DeclarationTypeCode = newValue; }
    

}
