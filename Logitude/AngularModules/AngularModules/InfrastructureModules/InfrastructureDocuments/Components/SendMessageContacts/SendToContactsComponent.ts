declare var System: any;
declare var window: any;
import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef}  from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {EntityPartner} from '../../../../Infrastructure/DataContracts/EntityPartner';
import {DocumentOutPMService} from '../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {FormControl}   from '@angular/forms';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ComponentArgs} from '../../../../Infrastructure/DataContracts/ComponentArgs';
import {ParameterComponentArgs} from '../../../../Infrastructure/DataContracts/ParameterComponentArgs';

@Component({
    moduleId: module.id,

    selector: 'SendToContacts',
    templateUrl: './SendToContactsComponent.html',
    providers: [EntityListService, DocumentOutPMService]


})

export class SendToContactsComponent implements OnInit {

    @Output() onQueryChangeEvent = new EventEmitter();
    public ComponentName: string = "SendTo";
    OnCloseSendToContactsEvent = new EventEmitter();


    public searchFields: string = "";
    public ObjectTableName: string = "Contact";
    public items: any[] = [];
    MainMenuItems: any;
    PartnersObslist: EntityPartner[] = [];
    SearchText: string;
    ComponentArgs: ComponentArgs;
    myPartnerId: string;
    public IsSearchIconVisible: boolean = true;

    @Output() SearchFieldchangeevent = new EventEmitter();
    ToEmailLists: string[];
    CcEmailLists: string[];
    BccEmailLists: string[];
    IsShowMessageCount: boolean;
    IsShowMessageCountTo: boolean;
    IsShowMessageCountCc: boolean;
    IsShowMessageCountBcc: boolean;
    CloseImageMouseOver: boolean;
    Componentkey: string = "";

    Bcc: string;
    Cc: string;
    ToEmail: string;

    public ShowBCC: boolean = true;
    public ShowCC: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _entityListService: EntityListService, public _documentOutPMService: DocumentOutPMService, private cd: ChangeDetectorRef) {


        this.ToEmailLists = [];
        this.CcEmailLists = [];
        this.BccEmailLists = [];

        window.ToEmailLists = [];
        window.CcEmailLists = [];
        window.BccEmailLists = [];

        this.CurrentSession.SessionEvent.subscribe((res) => {

            if (res && res.IsCheck) this.RefreshEmailList(res);


        });

    }

    ngOnInit(


    ) {

    }


    SetWindowArgs(args: any) {

        if (AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid.newGuid();
        }


        this.PartnersObslist = args.PartnersObslist;
        this.OnCloseSendToContactsEvent = args.OnCloseSendToContactsEvent;
        this.ToEmail = args.ToEmail;
        this.Cc = args.Cc;
        this.Bcc = args.Bcc;

        if (args.HideBCC) {
            this.ShowBCC = false;
        }
        if (args.HideCC) {
            this.ShowCC = false;
        }

        if (this.PartnersObslist) {

            if (!this.PartnersObslist.filter(d => d.PartnerType == "All")[0]) {
                this.PartnersObslist.push(new EntityPartner("All", "", false));
            }


            if (!this.PartnersObslist.filter(d => d.PartnerType == "All Users")[0]) {
                this.PartnersObslist.push(new EntityPartner("All Users", "", false));
            }

            this.SelectedPartnerItem = this.PartnersObslist.filter(r => r.PartnerType == "Customer" || r.PartnerType == "Customer Contacts")[0];

            if (this.SelectedPartnerItem == null) {
                this.SelectedPartnerItem = this.PartnersObslist.filter(r => r.PartnerType == "Owner")[0];
            }

            if (this.SelectedPartnerItem == null && args.IsUserFromReport) {
                this.SelectedPartnerItem = this.PartnersObslist[0];
            }


            if (this.SelectedPartnerItem == null) {
                this.SelectedPartnerItem = this.PartnersObslist.filter(r => r.PartnerType == "All")[0];
            }

            if (args.ObjectTableName) {
                this.ObjectTableName = args.ObjectTableName;
            }
        }



        ComponentArgs.AddComponent(new ParameterComponentArgs(this.CurrentSession.Sessionkey + "SendTo", this));



        // To  Email
        if (this.ToEmail) {
            this.ToEmail.split(';').forEach((item) => {
                if (item) {
                    this.ToEmailLists.push(item.toLowerCase());
                }
            });
            window.ToEmailLists = this.ToEmailLists;
        }

        // Cc  Email
        if (this.Cc) {
            this.Cc.split(';').forEach((item) => {
                if (item) {
                    this.CcEmailLists.push(item.toLowerCase());
                }
            });
            window.CcEmailLists = this.CcEmailLists;
        }

        // Bcc  Email
        if (this.Bcc) {
            this.Bcc.split(';').forEach((item) => {
                if (item) {
                    this.BccEmailLists.push(item.toLowerCase());
                }
            });

            window.BccEmailLists = this.BccEmailLists;
        }


        this.BuildColumns();

    }


    filterAgrs: ApiQueryFilters;
    SelectedPartnerItem: EntityPartner;
    SelectionChanged(item: EntityPartner) {
        this.SelectedPartnerItem = item;
        this.filterAgrs = new ApiQueryFilters();
        this.myPartnerId = this.SelectedPartnerItem.PartnerId;


        this.onQueryChangeEvent.emit({ QueryId: "", Filters: this.filterAgrs });


    }


    columns: any;
    BuildColumns() {

        this.columns = [];
        this.columns.push({
            FieldName: "To",
            DataTypeCode: 'String',
            Display: 'To',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });

        this.columns.push({
            FieldName: "Cc",
            DataTypeCode: 'String',
            Display: 'Cc',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });


        this.columns.push({
            FieldName: "Bcc",
            DataTypeCode: 'String',
            Display: 'Bcc',
            IsCustomTemplate: true,
            Styles: { width: '32px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });






        this.columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '180px' },

        });



        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '180px' },

        });

        this.columns.push({
            FieldName: "Company",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Company',
            Styles: { width: '150px' },

        });

        this.columns.push({
            FieldName: "Notes",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Notes',
            Styles: { width: '140px' },
            ServerSideSortable: true,
        });

        this.columns.push({
            FieldName: "Position",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Position',
            Styles: { width: '140px' },
            ServerSideSortable: true,
        });
        
    }


    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;


        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerType.toUpperCase() != "ALL" && this.SelectedPartnerItem.PartnerTypeCode.toUpperCase() != "ALLUSERS") {

                if (this.SelectedPartnerItem.PartnerId) {
                    if (!this.SelectedPartnerItem.IsUser) {
                        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
                    }
                }
            }
            else this.myPartnerId = null;
        }


        if (!AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }

        filters.addAdditionalFilter("CardId", this.myPartnerId, null, null, "Equals", true, true, true, "Text");
        filters.addAdditionalFilter("HasEmail", "", null, null, "NotEqual", true, false, false, "String");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");


        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerTypeCode.toUpperCase() == "ALLUSERS") {
                filters.addAdditionalFilter("HasUser", "", null, null, "NotEqual", true, false, false, "String");
            }
        }



        return this._entityListService.getByFilters(rowsObjectTable, filters);

    }


    RefreshEmailList(res: any) {

        var item = null;
        var index = 0;

        if (!AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            if (ComponentArgs && ComponentArgs.ComponentLists) {
                var sessionkey: string = this.CurrentSession.Sessionkey + "SendTo";
                var Component = ComponentArgs.ComponentLists.filter(d => d.key == sessionkey)[0];
                if (Component) {
                    var myComponent = Component.Component;
                    if (myComponent) {
                        if (res.FieldName == "To") {

                            item = myComponent.ToEmailLists.filter(d => d.toLowerCase() == res.Email.toLowerCase())[0];
                            if (item) {
                                index = myComponent.ToEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1) myComponent.ToEmailLists.splice(index, 1);

                            }

                            else myComponent.ToEmailLists.push(res.Email.toLowerCase());

                            if (myComponent.ToEmailLists.length > 50) {
                                myComponent.IsShowMessageCountTo = true;
                            } else myComponent.IsShowMessageCountTo = false;

                            window.ToEmailLists = myComponent.ToEmailLists;
                        }


                        if (res.FieldName == "Cc") {

                            item = myComponent.CcEmailLists.filter(d => d.toLowerCase() == res.Email.toLowerCase())[0];
                            if (item) {
                                index = myComponent.CcEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1) myComponent.CcEmailLists.splice(index, 1);

                            }

                            else myComponent.CcEmailLists.push(res.Email.toLowerCase());

                            if (myComponent.CcEmailLists.length > 50) {
                                myComponent.IsShowMessageCountCc = true;
                            } else myComponent.IsShowMessageCountCc = false;

                            window.CcEmailLists = myComponent.CcEmailLists;
                        }


                        if (res.FieldName == "Bcc") {

                            item = myComponent.BccEmailLists.filter(d => d.toLowerCase() == res.Email.toLowerCase())[0];
                            if (item) {
                                index = myComponent.BccEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1) myComponent.BccEmailLists.splice(index, 1);

                            }

                            else myComponent.BccEmailLists.push(res.Email.toLowerCase());

                            if (myComponent.BccEmailLists.length > 50) {
                                myComponent.IsShowMessageCountBcc = true;
                            } else myComponent.IsShowMessageCountBcc = false;

                            window.BccEmailLists = myComponent.BccEmailLists;
                        }

                        if (myComponent.IsShowMessageCountTo || myComponent.IsShowMessageCountCc || myComponent.IsShowMessageCountBcc) {

                            myComponent.IsShowMessageCount = true;
                        } else myComponent.IsShowMessageCount = false;



                    }
                }
            }
        }




        res.IsCheck = false;


    }

    onSearchTextChangeEvent(searchtext) {

        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);

    }


    DeleteEmail(fieldName: string, email: string) {

        if (fieldName == "To") window.ToEmailLists = this.ToEmailLists = this.ToEmailLists.filter(d => d.toLowerCase() != email.toLowerCase());
        if (fieldName == "Cc") window.CcEmailLists = this.CcEmailLists = this.CcEmailLists.filter(d => d.toLowerCase() != email.toLowerCase());
        if (fieldName == "Bcc") window.BccEmailLists = this.BccEmailLists = this.BccEmailLists.filter(d => d.toLowerCase() != email.toLowerCase());
        this.SearchFieldchangeevent.emit(this.searchFields);




    }

    CloseButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {
        this.OnCloseSendToContactsEvent.emit(this);
        this.CloseButtonClicked();
    }


    private timerToken: any;


}


