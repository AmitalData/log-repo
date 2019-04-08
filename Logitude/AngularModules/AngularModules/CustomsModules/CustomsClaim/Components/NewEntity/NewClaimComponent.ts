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
import { ClaimPM } from '../../../../Customs/EntityPMs/ClaimPM';
import { ClaimPMService } from '../../../../Customs/Services/StandardPMs/ClaimPMService';
import { ClaimWebService } from '../../../../Customs/Services/WebServices/ClaimWebService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewClaimComponent',
    moduleId: module.id,
    templateUrl: './NewClaimComponent.html',
})

export class NewClaimComponent extends BaseComponent implements OnInit {
    public EntityPM: ClaimPM;
    public DataContext: NewClaimComponent = this;
    public ObjectTableName: string = "Customs.Claim";
    public ValidationErrorsList: string[] = [];
    QueryNameText: string = "";

    private _ClaimPMService: ClaimPMService = new ClaimPMService();
    private _ClaimWebService: ClaimWebService = new ClaimWebService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();

        this.EntityPM = new ClaimPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(response => { });
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.QueryNameText = AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    }

    ngOnInit() {

    }

    //#region Properties
    get ClaimOfficeCode() { return this.EntityPM.CustomsBranchCode; }
    set ClaimOfficeCode(value: string) {
        if (this.EntityPM.CustomsBranchCode != value) {
            this.EntityPM.CustomsBranchCode = value;
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;
        }
    }
    //#endregion

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O.ClientIsMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.ClaimOfficeCode)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationOfficeCodeMandatory"));
        }

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this._ClaimWebService.CheckIfCorporationNameExists(this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.SubmitChanges(myResponse, true);
            });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {

        if (myResponse.Result != null) {
            var exists: boolean = myResponse.Result;
            if (!exists) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claims.O.CorporationNameNotExists"));
                return;
            }
        }

        this._ClaimPMService.insert(this.EntityPM).subscribe(myResult => {
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

    AddCustomerClicked() {

        var args = new NewEntityArgs();
        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.CustomerId = s;
            }
        });
    }
}
