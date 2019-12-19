import { Component } from "@angular/core";
import { BaseComponent } from "../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from "../../../../../Infrastructure/Utilities/ObservableCollection";
import { ServiceResponse } from "../../../../../Infrastructure/DataContracts/ServiceResponse";
import { SessionLocator } from "../../../../../Infrastructure/Utilities/SessionLocator";
import { DeclarationExtendedListService } from "../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService";
import { EntityArgs } from "../../../../../Infrastructure/DataContracts/EntityArgs";
import { DeclarationPM } from "../../../../../Customs/EntityPMs/DeclarationPM";
import { forEach } from "@angular/router/src/utils/collection";

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationAmendmentComponent.html',
    providers:[DeclarationExtendedListService]
})

export class DeclarationAmendmentComponent extends BaseComponent   {

    public amendmentObslist: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    private customFileNo: string;
    public EntityPM: DeclarationPM = null;
    public declarations: DeclarationPM[];
    IsLoaded: boolean;
    constructor(private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService,
    private entityArgs: EntityArgs) {
        super();
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.customFileNo = this.EntityPM.CustomFileNo;

                this.LoadDeclarationAmendmentsList();
            });

    }


    private LoadDeclarationAmendmentsList() {
        this.amendmentObslist = new ObservableCollection([]);
        this.CurrentSession.StartBusyIndicator("Loading...");

        this.declarationExtendedListService.GetDeclarationAmendmentListPMByCustomFileNo(this.customFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
              //  this.declarations = myResponse.Result;
               // debugger;
                this.GetDeclarationAmendmentsListsOp_Completed(myResponse, false);
                this.IsLoaded = true;
               // this.TapagIdEdit();
            });
    }

    private GetDeclarationAmendmentsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
          
            for (var i = 0; i < myResponse.Result.length; i++) {
                this.amendmentObslist.Insert(myResponse.Result[i]);
            }
            //myResponse.Result.forEach((item) => {
            //     this.amendmentObslist.Insert(item);
            //});
        }
    }


}
