declare var System: any;
declare var window: any;
import {Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef, Output} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityPartner} from '../../../../Infrastructure/DataContracts/EntityPartner';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {ComponentArgs} from '../../../../Infrastructure/DataContracts/ComponentArgs';
import {ParameterComponentArgs} from '../../../../Infrastructure/DataContracts/ParameterComponentArgs';
import {ActivityInviteePM} from '../../../../CRM/EntityPMs/ActivityInviteePM';
import {ActivityPM} from'../../../../CRM/EntityPMs/ActivityPM';
import {InviteeArgs} from '../../../../CRM/Args'; 
import {Guid} from '../../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditInviteesComponent.html',
})

export class AddEditInviteesComponent extends BaseComponent {
    public DataContext = this;
    private entityPM: ActivityPM;
    public items: any[] = [];
    public ObjectTableName: string = "Contact";
    myPartnerId: string;
    PartnersObslist: EntityPartner[];
    public RequiredList: ActivityInviteePM[] = [];
    public OptionalList: ActivityInviteePM[] = [];
    public ValidationErrorsList: string[] = [];
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() SearchFieldchangeevent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _entityListService: EntityListService, private cd: ChangeDetectorRef) {
        super();
        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res.Name == "InviteeCheckBoxComponent") {
                this.RefreshEmailList(res.select);
            }
        });
    }
    SetWindowArgs(args: InviteeArgs) {
        if (!this.CurrentSession.Sessionkey) {
            this.CurrentSession.Sessionkey = Guid.newGuid();
        }
        this.InitializeLists();
        this.entityPM = args.Entity;
        this.RequiredList = this.entityPM.ActivityInvitees.filter(d => d.IsRequired == true);
        this.OptionalList = this.entityPM.ActivityInvitees.filter(d => d.IsRequired == false);
        window.RequiredList = this.RequiredList;
        window.OptionalList = this.OptionalList;
        this.BuildRequiredEmailList();
        this.BuildOptionalEmailList();
        this.BuildColumns();
    }
    InitializeLists() {
        this.PartnersObslist = [];
        this.RequiredList = [];
        this.OptionalList = [];
        window.RequiredList = [];
        window.OptionalList = [];
        this.PartnersObslist.push(new EntityPartner("All", "", false));
        if (this.SelectedPartnerItem == null) {
            this.SelectedPartnerItem = this.PartnersObslist.filter(r => r.PartnerType == "All")[0];
        }
        ComponentArgs.AddComponent(new ParameterComponentArgs(this.CurrentSession.Sessionkey + "SendActivity", this));
    }
    BuildRequiredEmailList() {
        this.RequiredBoxText = "";
        this.RequiredEmailBoxText = "";

        this.RequiredList.forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.ContactId)) {
                this.RequiredBoxText += item.Email + ";";
            }
            else {
                this.RequiredEmailBoxText += item.Email + ";";
            }
        });
    }
    BuildOptionalEmailList() {
        this.OptionalBoxText = "";
        this.OptionalEmailBoxText = "";

        this.OptionalList.forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.ContactId)) {
                this.OptionalBoxText += item.Email + ";";
            }
            else {
                this.OptionalEmailBoxText += item.Email + ";";
            }
        }); 
    }
    RefreshEmailList(res: any) {
        var item = null;
        var index = 0;
        if (!AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            if (ComponentArgs && ComponentArgs.ComponentLists) {
                var sessionkey: string = this.CurrentSession.Sessionkey + "SendActivity";
                var Component = ComponentArgs.ComponentLists.filter(d => d.key == sessionkey)[0];
                if (Component) {
                    var myComponent = Component.Component;
                    if (myComponent) {

                        if (res.FieldName == "Required") {
                            if (res.IsCheck) {
                                item = myComponent.RequiredList.filter(d => d.Email == res.Email && d.ContactId == res.ContactId)[0];
                                if (item == null) {
                                    var invitee = new ActivityInviteePM(null);
                                    invitee.IsRequired = true;
                                    invitee.Email = res.Email;
                                    invitee.ContactId = res.ContactId;
                                    invitee.ContactName = res.ContactName;
                                    myComponent.RequiredList.push(invitee);
                                }
                            }
                            else {
                                item = myComponent.RequiredList.filter(d => d.Email == res.Email && d.ContactId == res.ContactId) [0];
                                if (item) {
                                    index = myComponent.RequiredList.indexOf(item);
                                    if (index != -1)
                                        myComponent.RequiredList.splice(index, 1);
                                }
                            }
                            window.RequiredList = myComponent.RequiredList;
                            this.RequiredList = myComponent.RequiredList;
                            this.BuildRequiredEmailList();
                            
                            //this.RequiredList = myComponent.RequiredList;
                            //this.BuildRequiredEmailList();
                        }
                        if (res.FieldName == "Optional") {
                            if (res.IsCheck) {
                                item = myComponent.OptionalList.filter(d => d.Email == res.Email && d.ContactId == res.ContactId)[0];
                                if (item == null) {
                                    var invitee = new ActivityInviteePM(null);
                                    invitee.IsRequired = false;
                                    invitee.Email = res.Email;
                                    invitee.ContactId = res.ContactId;
                                    invitee.ContactName = res.ContactName;
                                    myComponent.OptionalList.push(invitee);
                                }
                            }
                            else {
                                item = myComponent.OptionalList.filter(d => d.Email == res.Email && d.ContactId == res.ContactId)[0];
                                if (item) {
                                    index = myComponent.OptionalList.indexOf(item);
                                    if (index != -1)
                                        myComponent.OptionalList.splice(index, 1);
                                }
                            }
                            window.OptionalList = myComponent.OptionalList;
                            this.OptionalList = myComponent.OptionalList;
                            this.BuildOptionalEmailList();
                           
                        }
                        //res.IsCheck = false;
                    }
                }
            }
        }
    }
    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    filterAgrs: ApiQueryFilters;
    SelectedPartnerItem: EntityPartner;
    SelectionChanged(item: EntityPartner) {
        this.SelectedPartnerItem = item;
        this.filterAgrs = new ApiQueryFilters();
        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
        console.log(this.myPartnerId);
        this.onQueryChangeEvent.emit({ QueryCode: "", Filters: this.filterAgrs });
    }

    columns: any;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: "Required",
            DataTypeCode: 'String',
            Display: 'Required',
            IsCustomTemplate: true,
            Styles: { width: '70px' },
            HtmlListComponentName: 'InviteeCheckBoxComponent',
            HtmlListComponentUrl: './CRMModules/CRMActivity/Components/NewEntity/InviteeCheckBoxComponent',
        });
        this.columns.push({
            FieldName: "Optional",
            DataTypeCode: 'String',
            Display: 'Optional',
            IsCustomTemplate: true,
            Styles: { width: '70px' },
            HtmlListComponentName: 'InviteeCheckBoxComponent',
            HtmlListComponentUrl: './CRMModules/CRMActivity/Components/NewEntity/InviteeCheckBoxComponent',
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
    }

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;

        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerType.toUpperCase() != "ALL") {
                if (this.SelectedPartnerItem.PartnerId) {
                    if (!this.SelectedPartnerItem.IsUser) {
                        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
                    }
                }
            }
            else this.myPartnerId = null;
        }

        if (filters.AdditionalFilters.filter(a => a.FieldName == "CardId").length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "CardId");
        }
        filters.addAdditionalFilter("CardId", this.myPartnerId, null, null, "Equals", true, true, true, "Text");
        return this._entityListService.getByFilters(rowsObjectTable, filters);
    }

    // Props
    private requiredBoxText;
    get RequiredBoxText() { return this.requiredBoxText; }
    set RequiredBoxText(value: string) {
        this.requiredBoxText = value;
    }

    private optionalBoxText;
    get OptionalBoxText() { return this.optionalBoxText; }
    set OptionalBoxText(value: string) {
        this.optionalBoxText = value;
    }

    private requiredEmailBoxText = "";
    get RequiredEmailBoxText() { return this.requiredEmailBoxText; }
    set RequiredEmailBoxText(value: string) {
        this.requiredEmailBoxText = value;
    }

    private optionalEmailBoxText = "";
    get OptionalEmailBoxText() { return this.optionalEmailBoxText; }
    set OptionalEmailBoxText(value: string) {
        this.optionalEmailBoxText = value;
    }

    // Search 
    public searchFields: string = ""; 
    onSearchTextChangeEvent(searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    }

    // Commands
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    private errors = [];
    SaveButtonClicked() {
        this.errors = [];
        this.CheckIsValidEmails(this.RequiredEmailBoxText);
        this.CheckIsValidEmails(this.OptionalEmailBoxText);
        this.ValidationErrorsList = this.errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    CheckIsValidEmails(mailsList: string) {
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach((item) => {
                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        this.errors.push(item + " has invalid format");
                        return;
                    }
                }
            });
        }
        return IsOk;
    }
}
