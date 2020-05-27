import {AppTool} from '../../../../../Infrastructure/Tools';
import {Component, EventEmitter, Output}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from     '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsVendorListService} from '../../../../../Customs/Services/StandardLists/CustomsVendorListService'
import { EntityListService } from   '../../../../../Infrastructure/Services/EntityListService';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import {VendorExtendedListService} from '../../../../../Customs/Services/ExtendedLists/VendorExtendedListService'
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import { CustomSendOptionsArgs} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ImporterDeclarationRequestParams } from '../../../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { VendorSearchByCustomsAgentRequestParams } from '../../../../../Customs/DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { VendorMessagesService } from '../../../../../Customs/Services/WebServices/VendorMessagesService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomsVendorPM } from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import { VendorCommunicationPM } from '../../../../../Customs/EntityPMs/VendorCommunicationPM';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { CustomsVendorPMService } from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';

@Component({
    
    templateUrl: './VendorExtendedSearchComponent.html',
})


export class VendorExtendedSearchComponent extends BaseComponent {
    @Output() MenuHeaderchangeevent = new EventEmitter();
    customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    private _entityListService: EntityListService = new EntityListService();
    vendorExtendedListService: VendorExtendedListService = new VendorExtendedListService();
    iIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    private EntityResourceService: EntityResourceService;
    ImporterId: string;
    IsDisplayOnly: boolean;
    vendorMessagesService: VendorMessagesService = new VendorMessagesService();
    customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._PeriodDeclarationList = new ObservableCollection([]);
        this.EntityResourceService = new EntityResourceService();
        this.BuildColumns();

    }

    searchText: string = null;
    Search(text: string) {
        this.searchText = text;
        if (!this._HavePeriodDecResult) {
            this.LoadData();
        } else {
            this.FilterLocal()
        }

    }
    FilterLocal() {

        if (AppTool.IsNullOrEmpty(this.searchText)) {
            this._PeriodDeclarationList.InsertCollection(this._PeriodDeclarationListSave);
            return;
        }

        var searchList = this._PeriodDeclarationListSave.filter(periodDecRow => this.ShowPeriodDeclaration(periodDecRow));
        this._PeriodDeclarationList.Clear();
        this._PeriodDeclarationList.InsertCollection(searchList);

    }
    ShowPeriodDeclaration(periodDeclaration: any): boolean {
        if (periodDeclaration == null) return false;
        let VendorID: string = "";
        if (!AppTool.IsNullOrEmpty(periodDeclaration.VendorID)) {
            VendorID = periodDeclaration.VendorID;
        }
        let VendorName: string = "";
        if (!AppTool.IsNullOrEmpty(periodDeclaration.VendorName)) {
            VendorName = periodDeclaration.VendorName;
        }
        let searchField: string = "";
        searchField += VendorID;
        searchField += VendorName;
        return searchField.toLowerCase().includes(this.searchText.toLowerCase());


    }


    SetWindowArgs(args: any) {

        this.ImporterId = args.ImporterId;
        this.IsDisplayOnly = args.IsDisplayOnly;
        this.LoadData();

    }

    LoadData() {
        this.filterAgrs = new ApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'VendorNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsVendor.F.VendorNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'VendorName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsVendor.F.VendorName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'CountryCode',//'TaxTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsVendor.F.CountryCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ImporterDespositionNumber',
            DataTypeCode: 'String',
            Display: "תצהיר",  //TextCodeTranslator.Translate("Customs.ImporterDesposition.F.DepositionNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'EndDate',
            DataTypeCode: 'DateTime',
            Display: "תוקף תצהיר",// TextCodeTranslator.Translate("Customs.ImporterDesposition.F.EndDate"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'EndDateComponent',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/EndDateComponent',



        });



    }

    DataSource = {

        pageSize: 10,
        rowCount: null,

        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };

    ViewInitCompleted($event) {

        this.filterAgrs = new ApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortDirection = "Ascending";
        var importer: string = null;


        //if (this.IsChecked || this.ShowOnlyValid) {
        //  //  importer = this.ImporterId;
        //}
        //else {
        //    importer = null;
        //}
        importer = this.ImporterId;

        return new Promise((resolve, reject) => {

            resolve(this.vendorExtendedListService.GetVendorsWithImporterDespositions(null, importer, this.ShowOnlyValid, this.IsChecked, this.searchText));
        });


    }


    filterAgrs: ApiQueryFilters;

    private isChecked: boolean = true;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(newValue: boolean) {
        this.isChecked = newValue;
        this.LoadData();
    }

    private showOnlyValid: boolean = true;
    public get ShowOnlyValid() { return this.showOnlyValid; }
    public set ShowOnlyValid(newValue: boolean) {
        this.showOnlyValid = newValue;
        this.LoadData();
    }
    public SelectedRow: any = null;
    OnItemRowSelected(selected: any) {
        this.SelectedRow = selected.rowData;
        this.CurrentSession.CloseCurrentWindowEmit("close");
    }

    CancelButtonClicked() {

        //if (this.SelectedRow) {
        //    this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow.VendorId);
        //}

        //else {
        this.CurrentSession.CloseCurrentWindow();
        //  }
    }

    NewVendorButtonClicked() {
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe((response:any) => {
                var vendor = new CustomsVendorPM();
                vendor.Tenant = SessionLocator.Tenant;
                vendor.VendorTypeCode = "1";

                var args: any = {};
                args.IsNewEntity = true;
                args.EntityPM = vendor;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.New");
                logWindow.WindowArgs = args;
                logWindow.ShowCloseButton = true;
                logWindow.Show('./CustomsModules/CustomsVendor/Components/Components/EditTabs/VendorEditComponent');

                logWindow.WindowClosed.subscribe(($event: any) => this.LoadData());

            });
        });

    }
    _PeriodDeclarationList: ObservableCollection;
    _PeriodDeclarationListSave: any[]=[];
    _HavePeriodDecResult: boolean = false;
    UpdateImporterDeposition() {
        this.searchText = "";
        this.CurrentSession.StartBusyIndicator("");
        let customSendOptionsArgs: CustomSendOptionsArgs = new CustomSendOptionsArgs();
        //var month = new Date().getMonth();
        //var Year = new Date().getFullYear();
        //var day = new Date().getDay();
        // replace the above code with: //mohammad. task 40432

        var month = new Date().getUTCMonth();
        var Year = new Date().getUTCFullYear();
        var day = new Date().getUTCDate();
        var date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(Year + 1);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);


        var currRequestParams = new ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.CurrentSession.CurrentEditComponent.EntityPM.ImporterCode;
        currRequestParams.IsByExpireDate = true;
        currRequestParams.DeclarationExpire = date;// new Date(Year + 1, month, day);


        currRequestParams.JoinCustomsVendors = true;

        this.iIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe((myServiceResponse: any) => {
                if (!AppTool.IsNullOrEmpty(myServiceResponse.Result)) {

                    //let tstPeriodDeclarationList = [];
                    //let dmmy = { PeriodDeclarationID: "70227976", "VendorID": "2873975", "VendorName": "PANDUIT LTD", "CreateDate": "01.11.2017", "ValidityFrom": "01.11.2017", "ExpirationDate": "31.10.2018", "Status": "1", "StatusName": "טרם נבדק", "DocumentID": "562775218" };
                    //tstPeriodDeclarationList.push(dmmy);
                    //myServiceResponse.Result.PeriodDeclarationList = tstPeriodDeclarationList;


                    if (myServiceResponse.Result.PeriodDeclarationList != null && myServiceResponse.Result.PeriodDeclarationList.length > 0) {
                        this._HavePeriodDecResult = true;
                        this.IsChecked = true;
                        this.ShowOnlyValid = true;

                        this._PeriodDeclarationListSave = myServiceResponse.Result.PeriodDeclarationList;
                        //this._PeriodDeclarationList.InsertCollection(myServiceResponse.Result.PeriodDeclarationList);
                        this.FilterLocal();
                    }
                    if ((myServiceResponse.Result.PeriodDeclarationList != null && myServiceResponse.Result.PeriodDeclarationList.length > 0)
                        || myServiceResponse.Result.HasException == true) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Title = "עדכון תצהירים מהמכס";
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show(myServiceResponse.Result.UserMessage);
                    }



                }
                //this.LoadData();
                this.CurrentSession.StopBusyIndicator();
            });
    }
    SearchAddVendorRequest(rowData) {
        this.SelectedRow = rowData;



        if (!AppTool.IsNullOrEmpty(rowData.DBVendorID)) {
            let mm = new MessageWindow();
            mm.Show("קיים במערכת")
            return;
        }
        var searchParams = new VendorSearchByCustomsAgentRequestParams()
        searchParams.IsFakeResponse = true;
        searchParams.LoggingEnabled = false;
        searchParams.RequestName = "Search For Vendor Request";
        searchParams.ResponseName = "Search For Vendor Response";
        searchParams.Tenant = SessionLocator.Tenant;

        ///searchParams.VendorName = rowData.VendorName;

        searchParams.VendorNumber = rowData.VendorID;// odi said ENOUGH
        searchParams.VendorTypeCode = null;


        ////   //{"$id":"1","NumberOfResult":null,"VendorResults":null,"HasException":true,"UserMessage":"SendWS failed:FaultException.Detail:FaultException`1\r\nThe content type text/xml of the response message does not match the content type of the binding (application/soap+xml; charset=utf-8). If using a custom encoder, be sure that the IsContentTypeSupported method is implemented properly. The first 39 bytes of the response were: '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n'.","Succeeded":false,"ContinueProcessInBackground":false,"CustomsRequestsSheetId":"6614faa1-e053-4556-ae24-16825f120d2d","CorrelationId":""}
        CustomMessageProgressComponent.ShowProgressBar(searchParams.PBId, "חיפוש ספק", true)
            .then((myServiceResponse) => {

                console.log("[Send] Response/ShowProgressBar : ", myServiceResponse);
                var response = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(response)) {
                    var VendorResults: any[] = response.VendorResults;
                    if (response.IsCustomWarning) {
                        //this.CurrentSession.CloseCurrentWindow();
                    }
                }
            }).catch((err) => {
                console.error("[ERROR] CustomMessageProgressComponent error: ", err);
            });


        this.vendorMessagesService.PostSearchVendorRequest(searchParams).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Send] Response/PostSearchVendorRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;

            if (!AppTool.IsNullOrEmpty(response)) {
                var VendorResults: any[] = response.VendorResults;

                if (VendorResults.length == 1) {

                    if (VendorResults[0].StatusCode == '3') {

                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Show("VendorResults[0].StatusCode == 3");
                    } else if (VendorResults[0].Exists == "true") {
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Show("Already Exists");
                    } else {

                        this.AddButtonClicked(VendorResults[0]);
                    }
                } else {

                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(VendorResults.length.toString() + "מצפה לספק אחד בלבד התקבל ");
                }





            }
            else {
                var message = "Service returned a null response!";
            }

            //this.OnSendCompleted();
        });

    }
    public ObjectTableName: string = "Customs.CustomsVendor";
    AddButtonClicked(item: any) {
        if (item.StatusCode == '3') return;

        var errors = [];
        //this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        var newVendor = new CustomsVendorPM();
        newVendor.Tenant = SessionLocator.Tenant;
        newVendor.VendorName = item.VendorName;
        newVendor.CityName = item.CityName;
        newVendor.CountryCode = item.CountryCode;
        newVendor.DunsNumber = item.DunsNumber;
        newVendor.MainAddressLine = item.MainAddressLine;
        newVendor.PostalCode = item.PostalCode;
        newVendor.StatusCode = item.StatusCode;
        newVendor.SubCountryCode = item.SubCountryCode;
        newVendor.VendorNumber = item.VendorNumber;
        newVendor.VendorTypeCode = item.VendorTypeCode;
        newVendor.VATNumber = item.VATNumber;


        Validator.TryValidateObject(newVendor, this.ObjectTableName, errors);

        if (errors.length == 0) {
            for (let vendorCommunicationResult of item.VendorCommunications) {
                var newVendorCommunication = new VendorCommunicationPM(newVendor);
                newVendorCommunication.Tenant = SessionLocator.Tenant;
                newVendorCommunication.CommunicationAddress = vendorCommunicationResult.CommunicationAddress;
                newVendorCommunication.CommunicationTypeCode = vendorCommunicationResult.CommunicationType;

                newVendor.VendorCommunications.push(newVendorCommunication);
            }

            // Call service to add vendor
            this.customsVendorPMService.insert(newVendor).subscribe((myResult:any) => {

                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    var entity = res.Result;

                    console.log("..Vendor added successfully ", entity);

                    this.SelectedRow.DBVendorID = newVendor.Id;
                    this.SelectedRow.DBCountryCode = newVendor.CountryCode;
                    // after success


                }
                else {
                    let confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(res.ErrorsArray[0]);
                    //this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
            });

        }
        else {
            this.CurrentSession.StopBusyIndicator();
            //this.ValidationErrorsList = errors;
            let confirmWindow = new ConfirmWindow();
            confirmWindow.Show(errors[0]);
        }
    }

    onPeriodDeclarationCellSelected(event, item) {
        if (AppTool.IsNullOrEmpty(item.DBVendorID)) {

            var messageWindow = new MessageWindow();
            //messageWindow.Title = "עדכון תצהירים מהמכס";
            messageWindow.Show("ספק לא הוקם ");
        } else {
            this.SelectedRow = item;
            this.CurrentSession.CloseCurrentWindowEmit("close");
            //periodDec.DBVendorID = dbVendor.Id;
            //periodDec.DBCountryCode = dbVendor.CountryCode;
        }





    }
}
