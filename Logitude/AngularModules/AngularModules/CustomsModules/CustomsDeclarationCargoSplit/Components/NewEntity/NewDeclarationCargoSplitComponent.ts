import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { NewEntityArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationCargoSplitPM } from '../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import { DeclarationCargoSplitPMService } from '../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService';
import { DeclarationCargoSplitWebService } from '../../../../Customs/Services/WebServices/DeclarationCargoSplitWebService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewDeclarationCargoSplitComponent',
    moduleId: module.id,
    templateUrl: './NewDeclarationCargoSplitComponent.html',
})

export class NewDeclarationCargoSplitComponent extends BaseComponent implements OnInit {
    public EntityPM: DeclarationCargoSplitPM;
    public DataContext: NewDeclarationCargoSplitComponent = this;
    public ObjectTableName: string = "Customs.DeclarationCargoSplit";
    public ValidationErrorsList: string[] = [];
    QueryNameText: string = "";

    private _DeclarationCargoSplitPMService: DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService();
    private _DeclarationCargoSplitWebService: DeclarationCargoSplitWebService = new DeclarationCargoSplitWebService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();

        this.EntityPM = new DeclarationCargoSplitPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(response => { });
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.QueryNameText = AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    }

    ngOnInit() {

    }

    //#region Properties
    get ActionTypeCode() { return this.EntityPM.ActionTypeCode; }
    set ActionTypeCode(value: string) {
        if (this.EntityPM.ActionTypeCode != value) {
            this.EntityPM.ActionTypeCode = value;
        }
    }

    get CustomFileNo() { return this.EntityPM.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.EntityPM.CustomFileNo != value) {
            this.EntityPM.CustomFileNo = value;
        }
    }
    //#endregion

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileNumberMandatory"));
        }
        //if (AppTool.IsNullOrEmpty(this.DeclarationCargoSplitActionTypeCode)) {
        //    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O."));
        //}

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        //this._DeclarationCargoSplitWebService.CheckIfCorporationNameExists(this.EntityPM.Tenant)
           // .subscribe((myResponse: ServiceResponse) => {
           //     this.SubmitChanges(myResponse, true);
           // });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {

        if (myResponse.Result != null) {
            var exists: boolean = myResponse.Result;
            if (!exists) {
                //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeclarationCargoSplits.O.CorporationNameNotExists"));
                return;
            }
        }

        this._DeclarationCargoSplitPMService.insert(this.EntityPM).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                this.CurrentSession.CloseCurrentWindowEmit("ok");

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName, BackButtonLabel: this.QueryNameText });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

}
