 declare var System: any;
declare var window: any;
import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef}  from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {EntityPartner} from '../../../../Infrastructure/DataContracts/EntityPartner';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ComponentArgs} from '../../../../Infrastructure/DataContracts/ComponentArgs';
import {ParameterComponentArgs} from '../../../../Infrastructure/DataContracts/ParameterComponentArgs';
import {UserExtendedPMService} from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService';

@Component({
    
    templateUrl: './UserSearchComponent.html',
})

export class UserSearchComponent {
    @Output() onQueryChangeEvent = new EventEmitter();
    public ComponentName: string = "Users";
    //OnCloseSendToContactsEvent = new EventEmitter();


    public searchFields: string = "";
    public ObjectTableName: string = "User";
    public items: any[] = [];
    MainMenuItems: any;
    PartnersObslist: EntityPartner[] = [];
    SearchText: string;
    ComponentArgs: ComponentArgs;
    myPartnerId: string;
    userExtendedPMService: UserExtendedPMService;
    public IsSearchIconVisible: boolean = true;

    @Output() SearchFieldchangeevent = new EventEmitter();
    ToEmailLists: string[];
    
    IsShowMessageCount: boolean;
    IsShowMessageCountTo: boolean;
   
    CloseImageMouseOver: boolean;
    Componentkey: string = "";

    
    ToEmail: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _entityListService: EntityListService, private cd: ChangeDetectorRef) {

        this.userExtendedPMService = new UserExtendedPMService();
        this.ToEmailLists = [];
        
        window.ToEmailLists = [];
        
        if (AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid.newGuid();
        }

        this.CurrentSession.SessionEvent.subscribe((res) => {

            if (res && res.IsCheck) this.RefreshEmailList(res);


        });

        this.BuildColumns();

        this.Run();

    }

    Run() {
        this.userExtendedPMService.GetUsersTwoFactorAuthenticationEnabled(SessionLocator.Tenant).subscribe((resp:any) => {

            //this.PartnersObslist.push(new EntityPartner("All", "", false));
           

            this.filterAgrs = new ApiQueryFilters();
            this.filterAgrs.SortBy = "IsTwoFactorAuthenticatiEnabled";
            this.filterAgrs.SortDirection = "Descending";
            this.onQueryChangeEvent.emit({ QueryCode: "", Filters: this.filterAgrs });

            ComponentArgs.AddComponent(new ParameterComponentArgs(this.CurrentSession.Sessionkey + "SendTo", this));



            // To  Email
            if (resp.Result) {
                resp.Result.forEach((item) => {
                    if (item) {
                        this.ToEmailLists.push(item.toLowerCase());
                    }
                });
                window.ToEmailLists = this.ToEmailLists;
            }
            //if (this.ToEmail) {
            //    this.ToEmail.split(';').forEach((item) => {
            //        if (item) {
            //            this.ToEmailLists.push(item.toLowerCase());
            //        }
            //    });
            //    window.ToEmailLists = this.ToEmailLists;
            //}
        });
    }

    ngOnInit() {

    }

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
            FieldName: "SelectedUser",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
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
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '180px' },

        });

        //this.columns.push({
        //    FieldName: "Company",
        //    DataTypeCode: 'String',
        //    IsCustomTemplate: true,
        //    Display: 'Company',
        //    Styles: { width: '150px' },

        //});

        this.columns.push({
            FieldName: "Notes",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Notes',
            Styles: { width: '140px' },

        });



    }


    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Descending",
        sortingCol:"IsTwoFactorAuthenticatiEnabled",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        //if (filters == null) {
            filters = new ApiQueryFilters();
        //}
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = "IsTwoFactorAuthenticatiEnabled";
        filters.SortDirection = "Descending";
        filters.Tenant = SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;

        if (!AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }
         
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

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
                        if (res.FieldName == "SelectedUser") {

                            item = myComponent.ToEmailLists.filter(d => d.toLowerCase() == res.UserId.toLowerCase())[0];
                            if (item) {
                                index = myComponent.ToEmailLists.indexOf(res.UserId.toLowerCase());
                                if (index != -1) myComponent.ToEmailLists.splice(index, 1);

                            }

                            else myComponent.ToEmailLists.push(res.UserId.toLowerCase());

                            if (myComponent.ToEmailLists.length > 10) {
                                myComponent.IsShowMessageCountTo = true;
                            } else myComponent.IsShowMessageCountTo = false;

                            window.ToEmailLists = myComponent.ToEmailLists;
                        }


                        

                        if (myComponent.IsShowMessageCountTo) {

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

        if (fieldName == "SelectedUser") window.ToEmailLists = this.ToEmailLists = this.ToEmailLists.filter(d => d.toLowerCase() != email.toLowerCase());
        
        this.SearchFieldchangeevent.emit(this.searchFields);




    }

    CloseButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        //this.OnCloseSendToContactsEvent.emit(this);
        if (!AppTool.IsNullOrEmpty(window.ToEmailLists) && window.ToEmailLists.length > 0) {
            this.userExtendedPMService.PostUpdateTwoFactorAuthenticationEnabled(SessionLocator.Tenant, window.ToEmailLists).subscribe((res:any) => {

                this.CloseButtonClicked();
            });
        }
        else {

            this.ValidationErrorsList.push("Please define at least one user to be enabled for two factor authentication");
        }
    }


    private timerToken: any;


}


