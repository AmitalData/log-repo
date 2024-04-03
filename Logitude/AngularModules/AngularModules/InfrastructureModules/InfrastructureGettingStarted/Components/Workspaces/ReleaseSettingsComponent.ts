import { Component, EventEmitter, OnDestroy, Output } from "@angular/core";
import { HelpResource } from "Common/Services/CommonDomainService";
import { GlobalDomainService } from "Common/Services/GlobalDomainService";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { UIProperty } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectsLocator } from "Infrastructure/Locators/ObjectsLocator";
import { ReleaseArgs, WebFreightDomainService } from "Infrastructure/Services/WebFreightDomainService";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { HelpResourceArgs } from "./GettingStartedComponent";
import { FeatureLocator } from "Infrastructure/Utilities/FeatureLocator";





@Component({
    templateUrl: './ReleaseSettingsComponent.html',
})

export class ReleaseSettingsComponent extends BaseComponent implements OnDestroy{
    public DataContext: ReleaseSettingsComponent = this;
    private ReleaseDateString: string;
    private monthReleaseDate: string;
    private yearReleaseDate: number;
    public IsDeleteReleaseURL: boolean;
    public ReleaseCode: string;
    public releaseArgs: ReleaseArgs;
    public HelpResourceNames = [];
    public ObjectTableName: string = "Setting";
    public Monthes = ['January', 'February', 'March', 'April', 'May', 'June',
                      'July',  'August', 'September', 'October', 'November', 'December'];
    private myDomainService: WebFreightDomainService = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private ObjectFieldName: string = null;
    
    constructor (){
        super();
        
        this.ReleaseDateString = ObjectsLocator.GlobalSetting.ReleaseDateString;
        this.ReleaseCode = ObjectsLocator.GlobalSetting.ReleaseNotesURL;
        let ReleaseDateStrings = this.ReleaseDateString.split(" ");
        this.monthReleaseDate = ReleaseDateStrings[0];
        this.yearReleaseDate = +ReleaseDateStrings[1];

        this.Listen();
        this.LoadHelpResources(); 
    }

    private Listen() {
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "YearValueChanged") {
                this.yearReleaseDate = this.DataContext[this.ObjectFieldName];
            }            
        });
    }

    private SessionEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }
   
    public MonthReleaseDateChange(MonthReleaseDate: string) {
            this.monthReleaseDate = MonthReleaseDate;
    }

    public YearButtonClicked(isIncreas: boolean) {
    
        if (isIncreas) this.YearReleaseDate += 1;            
        else this.YearReleaseDate -= 1;            
    }
    public IsDeleteChecked(IsDeleteReleaseURL: boolean){
            this.IsDeleteReleaseURL = IsDeleteReleaseURL;
    }

    public UrlReleaseChange(ReleaseNotesUrl: string) {
        this.ReleaseCode = ReleaseNotesUrl;
   }

   private LoadHelpResources() {
    var service = new GlobalDomainService();
    service.GetReleaseHelpResources().subscribe((response: ServiceResponse) => {
        if (!response.HasError) {
            var allItems: HelpResource[] = response.Result;
            this.BuildHelpResourse(allItems);
        }
    });
    }
    private BuildHelpResourse(myList: HelpResource[]){
        myList.sort((a, b) => { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1 }).forEach(item => {
            if (FeatureLocator.HasFeaturePermession("HelpResource", item.FeatureCode)) {
                var helpResource = { name: item.Name, code: item.Code };
                this.HelpResourceNames.push(helpResource);
            }
        });  
    }
    public GetSelectedHelpResorce() {
        return this.HelpResourceNames.find(x => x.code == this.ReleaseCode);
    }

    public OkButtonClicked() {
        this.ReleaseDateString = this.monthReleaseDate + " " + this.yearReleaseDate;
        ObjectsLocator.GlobalSetting.ReleaseDateString = this.ReleaseDateString;
        ObjectsLocator.GlobalSetting.ReleaseNotesURL = this.ReleaseCode;

        this.myDomainService = new WebFreightDomainService();
        this.myDomainService.PutReleaseSettings(this.ReleaseDateString, this.IsDeleteReleaseURL, this.ReleaseCode).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {  
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            }
        });
        
    }

    get MonthReleaseDate() { return this.monthReleaseDate; }
    set MonthReleaseDate(value: string) {
        
        if (this.monthReleaseDate != value) {
            this.monthReleaseDate = value;
        }
    }
   
    get YearReleaseDate() { return this.yearReleaseDate; }
    set YearReleaseDate(value: number) {
        
        if (this.yearReleaseDate != value) {
            this.yearReleaseDate = value;
            this.DataContext[this.ObjectFieldName] = this.yearReleaseDate;
            this.CurrentSession.FireEvent("YearValueChanged");
        }
    }

}