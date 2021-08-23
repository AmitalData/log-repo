declare var window: any;
import {Component, OnInit, EventEmitter, Output, Input} from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {ListComponentArgs} from '../../Args';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';

@Component({
    selector: 'UsersQueryList',
    template: `<div *ngIf="UserQueries.length > 0">
                
                <div>
                    <div class="ScrollContent">
                        <p class="QueryLink" *ngFor="let Query of UserQueries" (click)="ViewQuery(Query.Code,Query.NameTextCodeCode)" >{{Query.NameTextCodeCode | TextCodeTranslationPipe}}</p>
                    </div>
                </div>                
               </div>
               <label class="QueryLink" *ngIf="ShowNoViews && UserQueries.length == 0" style="vertical-align:top;font-family:Arial; font-size:11px;color:gray;cursor: default;"> No views </label>
              `,
    inputs: ['ObjectTableName', 'BackButtonTitle','ReloadUserQueries','ShowNoViews'],
})

export class UsersQueryList implements OnInit {

    public UserQueries: any[];
    SearchFieldsId: string;
    PlaceHolder: string;
    ObjectTableName: string;
    ObjectTableId: string;
    BackButtonTitle: string;
    filterAgrs: ApiQueryFilters;
    @Output() BackCompletedEvent = new EventEmitter();
    @Input() WorkspaceFilters= new ApiQueryFilters();
    ReloadUserQueries: EventEmitter<any>;
    ShowNoViews: boolean = false;
    // public SearchTextValue: Control;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    ngOnInit() {
        var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.UserQueries = window.Queries.filter(x => x.ObjectTableId === ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo.LoggedUserTenant);
        if (this.ReloadUserQueries) {
            this.ReloadUserQueries.subscribe((res) => {
                this.UserQueries = window.Queries.filter(x => x.ObjectTableId === ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo.LoggedUserTenant);
            });
        }
    }

    ViewQuery(myQueryCode: string, NameTextCodeCode: string) {
        if (myQueryCode != null) {
            var objectTableName = "";
            var queryCode = "";
            var MethodName = null;
            var displayTitle = "";
            var backButtonTitle = this.BackButtonTitle;
            this.filterAgrs = new ApiQueryFilters();

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            if (this.WorkspaceFilters != null) {
                listArgs.Filters = this.WorkspaceFilters;
            }
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = this.ObjectTableName;
            listArgs.DisplayTitle = TextCodeTranslator.Translate(NameTextCodeCode);
            listArgs.BackButtonTitle = this.BackButtonTitle != "" && this.BackButtonTitle != null ? this.BackButtonTitle : "Back";
            listArgs.MethodName = MethodName;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.BackCompletedEvent.emit("Completed"));
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }
}
