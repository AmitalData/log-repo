declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeTemplateComponent} from './DocumentTypeTemplateComponent';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FormBuilder, FormGroup} from '@angular/forms';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
declare var insertAtSubject: any;
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    selector: 'DocumentTypeGeneral',
    templateUrl: './DocumentTypeGeneralTabComponent.html',
    providers: [DocumentTypeTemplatePMExtendedService],
})

export class DocumentTypeGeneralTabComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public myForm: FormGroup;
    public ObjectTablesList: ObjectTablePM[];
    public FormatList: string[];
    SelectedFormat: string = "";
    IsEnableFormat: boolean = false;
    ShowFeildTenant0: boolean;
    IsShowAdvanceLink: boolean;
    SelectedObjectTable: ObjectTablePM;
    IsLoadTemplate: boolean = false;
    public IsVisibile: boolean = false;
    IsEnableEdit: boolean;
    DataContext: any = this;
    CountryId: string;
    PointerEventsAreaStimulDocument: string = "none";
    PointerEventsHTMLDocument: string = "auto";

     OpacityAreaStimulDocument: string = "0.5";
     OpacityAreaHTMLDocument: string = "1";
     CountryLists: CountryList[] = [];
     DocumentTypeTemplates: DocumentTypeTemplatePM[];
     IsDisableObjectTable: boolean = false;

    public IsLogLovReady: boolean = false;
    public ObjectTablesFilterItems: ApiQueryFilters;
    public IsCustomObject: boolean = false;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(fb: FormBuilder, public entityArgs: EntityArgs, public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService) {
        super();
        this.myForm = fb.group({});
        this.CurrentSession.StartBusyIndicatorLoading();



    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate", 0).subscribe((response:any) => {

            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {
                var myService: CountryListService = new CountryListService();
                myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError && myResponse.Result) {
                        this.CountryLists = myResponse.Result;
                        if (this.CountryLists) {
                            if (!AppTool.IsNullOrEmpty(this.EntityPM.CountryCode)) {
                                var countryList: CountryList = this.CountryLists.filter(d => d.Code == this.EntityPM.CountryCode)[0];
                                if (countryList) {
                                    this.CountryId = countryList.Id;
                                }
                            }
                        }
                    }
                    this.IsVisibile = true;

                    this.Run();
                    this.LoadTemplate();

                });



            }
        });

    }



    Run() {

        this.EntityPM.UIProperties.SetEnabled("Code", "DocumentType", false);

        if (this.EntityPM.Code == "SLCIN" || this.EntityPM.Code == "SLCRP") {
            this.EntityPM.UIProperties.SetEnabled("Name", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsDocIn", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsDocOut", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("CountryCode", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsEnabledForCustomers", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsCopiedAtSignup", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("InActive", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("DocumentTypeCategoryCode", "DocumentType", false);
            this.IsEnableEdit = false;
        }

        else {
            this.IsEnableEdit = true;
        }





        if (!this.EntityPM.AddedManually) {

            if (!SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.Tenant != 0) {
                this.IsDisableObjectTable = true;
            }
        } 


 
        this.InitLOVFilters();
        this.IsLogLovReady = true;
        this.SetSelectedObjectTable();

        this.SelectedObjectTable = window.ObjectTables.filter((d: any) => d.Id == this.EntityPM.ObjectTableId)[0];
        if (!this.SelectedObjectTable) {
            this.SelectedObjectTable = window.ObjectTables[0];
        }

        this.FormatList = [];

        this.FormatList.push("Print");
        this.FormatList.push("Message");

        if (this.EntityPM.TemplateFormatCode == "P") {
            this.SelectedFormat = "Print";
            this.PointerEventsAreaStimulDocument = "auto";
            this.OpacityAreaStimulDocument = "1";


        }
        else if (this.EntityPM.TemplateFormatCode == "M") {
            this.SelectedFormat = "Message";
            //this.PointerEventsHTMLDocument = "auto";
           // this.OpacityAreaHTMLDocument = "1";
        }


        if (!this.EntityPM.IsDocOut) {
            this.PointerEventsAreaStimulDocument = "none";
            this.PointerEventsHTMLDocument = "none";

            this.OpacityAreaStimulDocument = "0.5";
            this.OpacityAreaHTMLDocument = "0.5";
        }


        if (FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES")) this.ShowFeildTenant0 = true;
        else this.ShowFeildTenant0 = false;

        //if (this.SelectedObjectTable.Name == "Shipment" || this.SelectedObjectTable.Name == "Quote") {
        //    this.IsShowAdvanceLink = true;
        //}
        //else this.IsShowAdvanceLink = false;

        if (this.EntityPM.IsDocOut) {
            this.IsEnableFormat = true;
        }
    }

    InitLOVFilters() {
        this.ObjectTablesFilterItems = new ApiQueryFilters();
        this.ObjectTablesFilterItems.addAdditionalFilter("AvailableInDocumentTypes", true, null, null, "Equals", true, false, false, "string");
        this.ObjectTablesFilterItems.Tenant = SessionLocator.Tenant;
    }
    SetSelectedObjectTable() {
        this.SelectedObjectTableId = this.EntityPM.ObjectTableId;
    }

    public get IsDocOut() { return this.EntityPM.IsDocOut }
    public set IsDocOut(value: boolean) {
        if (value == this.EntityPM.IsDocOut) return;
        this.EntityPM.IsDocOut = value;
    }
    private selectedObjectTableId: string;
    public get SelectedObjectTableId() { return this.selectedObjectTableId; }
    public set SelectedObjectTableId(value: string) {
        if (this.selectedObjectTableId == value) return;
        this.selectedObjectTableId = value;
        let objectTable = window.ObjectTables.filter(table => table.Id == value)[0];
        this.SetIsDocOutProperties(objectTable);
        this.ObjectTableValueChanged(objectTable);
    }
    private SetIsDocOutProperties(objectTable: any) {
        this.IsCustomObject = objectTable?.IsCustom && AppTool.IsNullOrEmpty(objectTable?.ParentObjectTableId);
        this.IsDocOut = this.IsCustomObject ? false : this.IsDocOut;
        this.IsDocOutChange(this.IsDocOut);
        this.EntityPM.UIProperties.SetEnabled("IsDocOut", "DocumentType", !this.IsCustomObject);
    }

    AdvanceLinkMethod() {


        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 380;
        logitudeWindow.Title = "Advance";
        logitudeWindow.DataContext = this.EntityPM;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeComponent');



    }

    FormatValueChanged(format: any) {
        this.PointerEventsAreaStimulDocument = "none";
        this.OpacityAreaStimulDocument = "0.5";


        if (format == "Print") {
            this.EntityPM.TemplateFormatCode = "P";
            this.PointerEventsAreaStimulDocument = "auto";
            this.OpacityAreaStimulDocument = "1";
        }

        else {

            this.EntityPM.TemplateFormatCode = "M";

        }





    }

    ObjectTableValueChanged(table: any) {
        var oldTalbeId: string = this.EntityPM.ObjectTableId; 
        var newTableID = table != null ? table.Id : null;
        if (table) {
            this.EntityPM.ObjectTableName = table.Name;
            this.EntityPM.ObjectTableId = table.Id;
            if (table.Name == "Shipment" || table.Name == "Quote") {
                this.IsShowAdvanceLink = true;
            }
            else this.IsShowAdvanceLink = false;
        } else {
            this.EntityPM.ObjectTableId = null;
            this.EntityPM.ObjectTableName = null
            this.SelectedObjectTable = null;
        }

        if (newTableID != oldTalbeId) {
            this.EntityPM.OnSendPopulateDateFieldName = null;
            this.EntityPM.OnPrintPopulateDateFieldName = null;
            this.EntityPM.OnUploadPopulateDateFieldName = null;
        }

    }


    public IsDocOutChange(isdocu: any) {


        if (isdocu) {
            this.IsEnableFormat = true;

            this.PointerEventsHTMLDocument = "auto";
            this.OpacityAreaHTMLDocument = "1";

            if (this.EntityPM.TemplateFormatCode == "P") {
                this.PointerEventsAreaStimulDocument = "auto";
                this.OpacityAreaStimulDocument = "1";

            }

        }
        else {
            this.IsEnableFormat = false;

            this.PointerEventsAreaStimulDocument = "none";
            this.PointerEventsHTMLDocument = "none";
            this.OpacityAreaStimulDocument = "0.5";
            this.OpacityAreaHTMLDocument = "0.5";

        }




    }
    InputFileNameId: string = "";
    InputFileNameIdGenerated(id:string) {
        this.InputFileNameId = id;
    }

    AddDataField() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ObjectTableId)) {


            var tableId: string = "";
            var table = window.ObjectTables.filter(d => d.Id == this.EntityPM.ObjectTableId)[0];
            if (table) tableId = table.Id;

            this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe((response:any) => {
                var windowArgs: any = {};
                windowArgs.ObjectTypeField = "DocuemntFileName";
               windowArgs.HideSystemDataTab = true;
                windowArgs.ObjectTableId = tableId;
                var logWindow = new LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event) {
                        this.EntityPM.FileName = insertAtSubject(this.InputFileNameId, $event);

                    }

                });
            });

        }
    }




    LoadTemplate() {


        this._documentTypeTemplatePMExtendedService.GetDocumentTypeTemplatesPMForDocumentType(this.EntityPM.Id, this.EntityPM.TemplateFormatCode, this.EntityPM.Tenant).subscribe((res:any) => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    this.DocumentTypeTemplates = result;
                }
            }
            this.IsLoadTemplate = true;
            this.CurrentSession.StopBusyIndicator();


        });


    }


    CountrySelectedChange(value:any) {
        if (value) {
            this.EntityPM.CountryCode = value.Code;
        } else this.EntityPM.CountryCode = "";

    }



}






