import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { BaseComponent } from "../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { DeclarationRemarksService } from "../../../Common/Services/ExtendedPMs/DeclarationRemarksService";
import { DeclarationRemarks } from "../../../Customs/EntityPMs/Extended/DeclarationRemarks";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { ObservableCollection } from "../../../Infrastructure/Utilities/ObservableCollection";
import { DateTool } from "../../../Infrastructure/Tools";
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'DeclarationRemarksComponent',
    templateUrl: './DeclarationRemarksComponent.html',
})
export class DeclarationRemarksComponent
    extends BaseComponent {
    public DataContext: any = this;
    public DeclarationRemarksQueryObservableList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    private Entity: DeclarationRemarks[] = [];

    _declarationRemarksService: DeclarationRemarksService = new DeclarationRemarksService();
    constructor() {
        super();
        this.DeclarationRemarksQueryObservableList = new ObservableCollection([]);
    }
    title: any;
    SetWindowArgs(args: any) {
        if (this.EntityPM != null) {
            this.EntityPM = new DeclarationRemarks();
        }
        this.Entity = args.EntityPM;
        for (let item of this.Entity) {
            this.DeclarationRemarksQueryObservableList.Insert(new RemarksComponent(item));
        }
        for (let entity of this.DeclarationRemarksQueryObservableList.Collection) {
            var myFormats = DateTool.GetDateFormats(entity.StatusDate);
            entity.StatusDate = myFormats.DateString;
            entity.StatusTime = myFormats.ShortTimeString;
        }
        this.title = args.title;
    }
    ExpandComment(entity: any, $event: any) {
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 400;
        logitudeWindow.Width = 700;
        logitudeWindow.ShowCloseButton = true;
        windowArgs.remarks = entity.StatusComment;
        logitudeWindow.Title = this.title;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsReferant/Components/RemarksPopUp');
    }
    public get StatusComment() { return this.EntityPM.StatusComment; }
    public set StatusComment(newValue: string) { this.EntityPM.StatusComment = newValue; }
}
export class RemarksComponent extends BaseComponent {
    public DataContext: any = this;
    constructor(public entityPM: DeclarationRemarks) {
        super();
    }
    public get Id() { return this.entityPM.StatusId; }
    public set Id(newValue: string) { this.entityPM.StatusId = newValue; }

    public get StatusDate() { return this.entityPM.StatusDate; }
    public set StatusDate(newValue: string) { this.entityPM.StatusDate = newValue; }

    public get StatuseTime() { return this.entityPM.StatuseTime; }
    public set StatuseTime(newValue: string) { this.entityPM.StatuseTime = newValue; }

    public get StatusComment() {
        if (this.entityPM.StatusComment == "") {
            return null
        }
        return this.entityPM.StatusComment;
    }
    public set StatusComment(newValue: string) { this.entityPM.StatusComment = newValue; }

    public get StatusName() { return this.entityPM.StatusName; }
    public set StatusName(newValue: string) { this.entityPM.StatusName = newValue; }

}




