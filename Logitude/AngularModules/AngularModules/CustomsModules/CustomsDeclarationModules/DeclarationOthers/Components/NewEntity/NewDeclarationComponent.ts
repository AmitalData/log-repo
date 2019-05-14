import {Component, ChangeDetectorRef, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DeclarationList} from '../../../../../Customs/EntityLists/DeclarationList';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {NewEntityArgs} from '../../../../../Infrastructure/Args';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {ConsignmentPM} from '../../../../../Customs/EntityPMs/ConsignmentPM';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {DeclarationExtendedListService} from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {CustomsHouseTypeExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewDeclarationComponent',
    moduleId: module.id,
    templateUrl: './NewDeclarationComponent.html',
})

export class NewDeclarationComponent extends BaseComponent implements OnInit {
    public EntityPM: DeclarationPM;
    public DataContext: NewDeclarationComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public ValidationErrorsList: string[] = [];
    QueryNameText: string = "";

    IsTransportModeMatch: boolean = true;

    private declarationPMService: DeclarationPMService = new DeclarationPMService();
    private declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private customsHouseTypeExtendedPMService: CustomsHouseTypeExtendedPMService = new CustomsHouseTypeExtendedPMService;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();

        this.EntityPM = new DeclarationPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsTransportMode").subscribe(response => { });

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.QueryNameText = AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    }

    ngOnInit() {

    }

    //#region Properties
    get CustomFileNo() { return this.EntityPM.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.EntityPM.CustomFileNo != value) {
            this.EntityPM.CustomFileNo = value;
            console.log("check number ...");
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;
        }
    }

    get DeclarationOfficeCode() { return this.EntityPM.DeclarationOfficeCode; }
    set DeclarationOfficeCode(value: string) {
        if (this.EntityPM.DeclarationOfficeCode != value) {
            this.EntityPM.DeclarationOfficeCode = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.setHouseTypeForDeclaration();
            }
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(value: string) {
        if (this.EntityPM.TransportModeId != value) {
            this.EntityPM.TransportModeId = value;
            this.IsTransportModeMatch = true;
            if (!AppTool.IsNullOrEmpty(value) && !AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
                this.checkMatchTransportMode();
            }
        }
    }
    //#endregion

    OkButtonClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = [];

        // Validation: Required and match transport
        if (AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ClientIsMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.DeclarationOfficeCode)) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationOfficeCodeMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.TransportModeId)) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.TransportModeIdMandatory"));
        }
        if (!this.IsTransportModeMatch) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.Match"));
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }

        // Exist check

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.SubmitChanges();
        }
        else {
            this.CustomFileNo = this.CustomFileNo.replace(/\s/g, '');
            this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo).subscribe((myResponse: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(myResponse)) {
                    var entity = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(entity)) {
                        // exist
                        errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CustomsFileNoExists"));
                        this.ValidationErrorsList = errors;
                    }
                    else {
                        this.SubmitChanges();
                    }
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges() {
        this.declarationPMService.insert(this.EntityPM).subscribe(myResult => {

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
                //console.log(s);
                this.CustomerId = s;
            }
        });
    }

    // Server Requests
    checkMatchTransportMode() {
        this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe((result: ServiceResponse) => {
            if (!AppTool.IsNullOrEmpty(result)) {
                var houseType = result.Result;
                console.log("-- houseType: ", houseType);
                if (!AppTool.IsNullOrEmpty(houseType)) {
                    if (!AppTool.IsNullOrEmpty(houseType.TransportModeId)) {
                        if (houseType.TransportModeId != this.TransportModeId) {
                            this.IsTransportModeMatch = false;
                        }
                    }
                }
            }
        });
    }
    setHouseTypeForDeclaration() {
        this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe((result: ServiceResponse) => {
            if (!AppTool.IsNullOrEmpty(result)) {
                var houseType = result.Result;
                if (!AppTool.IsNullOrEmpty(houseType)) {
                    this.TransportModeId = houseType.TransportModeId;
                    console.log("-- Transport Mode set to: ", houseType.TransportModeId);
                }
            }
        });
    }
}
