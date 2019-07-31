import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../Infrastructure/Services/EntityListService';
import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';
import {AppTool} from '../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'SocialPeopleComponent',
    templateUrl: './SocialPeopleComponent.html',
    providers: [EntityListService],

})

export class SocialPeopleComponent implements OnInit {
    public ObjectTableName: string = "User";
    @Output() onQueryChangeEvent = new EventEmitter();
    private SocialContactLinkEvent: any = null;
    public items: any[] = [];
    @Output() SearchFieldchangeevent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _entityListService: EntityListService) {

    }
    public searchFields: string = ""; 
    ngOnInit(

    ) {

        this.BuildColumns();
        this.Listen();

    }

    SetWindowArgs(args: any) {

      
    }

    Listen() {
        if (!this.SocialContactLinkEvent) {
            this.SocialContactLinkEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s) {
                    if (s[0] == "SocialContactLinkEvent") {
                        this.ViewPostUserFeedsButtonClick(s[1]);
                    }
                }
            });
        }
    }
   
    columns: any;
    BuildColumns() {

        this.columns = [];


        this.columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'EnglishName',
            Styles: { width: '400px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Social/Components/QueryColumnsComponents/SocialContactNameLink',

        });


        this.columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '400px' },

        });

        this.columns.push({
            //FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: '',
            Styles: { width: '100px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Social/Components/QueryColumnsComponents/SocialPeopleFollowComponent',

        });
        
    }




    ViewPostUserFeedsButtonClick(user:any) {

            var postsArgs: PostsArgs = new PostsArgs();
            postsArgs.QueryName = "UserPosts";
            postsArgs.SubQueryName = "All";
            postsArgs.UserId = user.Id;
            postsArgs.ScreenCode = "UserPostsControl";
            postsArgs.IsUserMode = true;
            SessionLocator.DynamicLoader.Load("./Social/Components/SocialPostsComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.InitializePostComponent(postsArgs);
                });
    }









    ngOnDestroy() {

        if (this.SocialContactLinkEvent) {
            this.SocialContactLinkEvent.unsubscribe();
            this.SocialContactLinkEvent = null;
        }
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

        if (!AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }

        filters.addAdditionalFilter("HasEmail", "", null, null, "NotEqual", true, false, false, "String");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        filters.addAdditionalFilter("Id", SessionLocator.LoggedUserId, null, null, "NotEqual", false, false, false, "String");


        return this._entityListService.getByFilters(rowsObjectTable, filters);


    }

    onSearchTextChangeEvent(searchtext) {
        if (searchtext != null && searchtext != undefined) {
            this.searchFields = searchtext;
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.SearchFieldchangeevent.emit(this.searchFields);
        }
        else {
            this.SearchFieldchangeevent.emit("");
        }
    }
    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }








}
