import {Component, OnInit} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShippingLinePM} from '../../../../Common/EntityPMs/ShippingLinePM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ShippingLinePMService} from '../../../../Common/Services/StandardPMs/ShippingLinePMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShippingLineListService} from '../../../../Common/Services/StandardLists/ShippingLineListService';
import {ShippingLineList} from '../../../../Common/EntityLists/ShippingLineList';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './NewShippingLineComponent.html',
})

export class NewShippingLineComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "ShippingLine";
    public DataContext: NewShippingLineComponent = this;
    public ValidationErrorsList: string[] = [];
    public ShippingLinePM: ShippingLinePM = new ShippingLinePM();
    public TenantPM: TenantPM;
    public IsNewEntityCall: boolean = true;
    public RequestPage: string;
    public IsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.entityResourceService.getEntityResourceByTableName("ShippingLine", 0).subscribe((response: any) => {
            this.IsVisible = true;
        });
        this.TenantPM = InfraSettings.TenantPM;
        this.Initialize();
    }


    SetWindowArgs(args: any) {
    

        if (args != null) {
            this.RequestPage = args.RequestPage;
            if (this.RequestPage == "SharedManifest") {
                if (!AppTool.IsNullOrEmpty(args.DefaultValues)) {

                    var defaultValuesArray: string[] = args.DefaultValues.split('^');
                    if (this.ShippingLinePM) {
                        this.ShippingLinePM.Code = !AppTool.IsNullOrEmpty(defaultValuesArray[0]) ? defaultValuesArray[0] : "";

                        this.ShippingLinePM.EnglishName = !AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";

                        this.ShippingLinePM.LocalName = !AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                    }
                }
            }
        }


    }


    ngOnInit() {

        this.ShippingLinePM.Tenant = this.TenantPM.Id;
        this.ShippingLinePM.CarrierTypeId = "SL";
        this.ShippingLinePM.TransportModeId = "O";
        this.ShippingLinePM.AddedManually = true;
    }

    Initialize() {
        this.IsEditEnabled = false;
        this.ZeroExistsMessageVisibility = false;
        this.LoadShippingLineListMethod();
    }

    // Properties 

    private zeroExistsMessageVisibility: boolean = false;
    get ZeroExistsMessageVisibility() { return this.zeroExistsMessageVisibility; }
    set ZeroExistsMessageVisibility(value: boolean) {
        this.zeroExistsMessageVisibility = value;
    }

    private isEditEnabled: boolean;
    get IsEditEnabled() { return this.isEditEnabled; }
    set IsEditEnabled(value: boolean) {
        this.isEditEnabled = value;
    }

    get Code() { return this.ShippingLinePM.Code; }
    set Code(value: string) {
        if (this.ShippingLinePM.Code != value) {
            this.ShippingLinePM.Code = value;
        }
    }

    private message: string = "";
    get Message() { return this.message; }
    set Message(value: string) {
        this.message = value;
    }

    private isSaveEnabled: boolean = true;
    get IsSaveEnabled() { return this.isSaveEnabled; }
    set IsSaveEnabled(value: boolean) {
        this.isSaveEnabled = value;
    }

    get SCACCode() { return this.ShippingLinePM.SCACCode; }
    set SCACCode(value: string) {
        if (this.ShippingLinePM.SCACCode != value) {
            this.ShippingLinePM.SCACCode = value;
        }
    }

    get EnglishName() { return this.ShippingLinePM.EnglishName; }
    set EnglishName(value: string) {
        if (this.ShippingLinePM.EnglishName != value) {
            this.ShippingLinePM.EnglishName = value;
        }
    }

    get LocalName() { return this.ShippingLinePM.LocalName; }
    set LocalName(value: string) {
        if (this.ShippingLinePM.LocalName != value) {
            this.ShippingLinePM.LocalName = value;
        }
    }

    get Website() { return this.ShippingLinePM.Website; }
    set Website(value: string) {
        if (this.ShippingLinePM.Website != value) {
            this.ShippingLinePM.Website = value;
        }
    }

    get ShippingAgentId() { return this.ShippingLinePM.ShippingAgentId; }
    set ShippingAgentId(value: string) {
        if (this.ShippingLinePM.ShippingAgentId != value) {
            this.ShippingLinePM.ShippingAgentId = value;
        }
    }

    get Remark() { return this.ShippingLinePM.Remark; }
    set Remark(value: string) {
        if (this.ShippingLinePM.Remark != value) {
            this.ShippingLinePM.Remark = value;
        }
    }

    get InActive() { return this.ShippingLinePM.InActive; }
    set InActive(value: boolean) {
        if (this.ShippingLinePM.InActive != value) {
            this.ShippingLinePM.InActive = value;
        }
    }

    //Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.ShippingLinePM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.SubmitCreatingShippingLine();
        }

    }

    SubmitCreatingShippingLine() {

        this.CurrentSession.StartBusyIndicatorSaving();

        var myService: ShippingLinePMService = new ShippingLinePMService();

        myService.insert(this.ShippingLinePM).subscribe(myResult => {

            this.CurrentSession.StopBusyIndicator();

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                if (this.RequestPage == "SharedManifest") {
                    this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                }
                else {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
            }
        });
    }

    CodeLostFocusMethod(code: any) {
        this.IsEditEnabled = false;
        this.ZeroExistsMessageVisibility = false;

        if (!AppTool.IsNullOrEmpty(code)) {
            if (code.length >= 1) {
                this.GetZeroObjectIfExists(code);
            }
        }
    }

    GetZeroObjectIfExists(code: string) {
        var x = this.ShippingLinesList.filter(a => a.Code.toLowerCase() == code.toLowerCase());
        if (x.length != 0) {
            this.Message = "This shipping line already exists!";
            this.ZeroExistsMessageVisibility = true;
            this.IsSaveEnabled = false;
            return;
        }

        else {
            this.IsEditEnabled = true;
            this.IsSaveEnabled = true;
        }

        this.GetShippingLineByCode(code, 0);
    }

    GetShippingLineByCode(code: string, tenant: number) {
        var zeroEntity: ShippingLinePM;
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetShippingLineByCode(code, tenant).subscribe((myResult:any) => {
            zeroEntity = myResult;
            this.IsEditEnabled = true;
            if (zeroEntity != null) {
                this.EnglishName = zeroEntity.EnglishName;
                this.LocalName = zeroEntity.LocalName;
                this.SCACCode = zeroEntity.SCACCode;
                this.Website = zeroEntity.Website;
                this.Remark = zeroEntity.Remark;
                this.ShippingAgentId = zeroEntity.ShippingAgentId;
                this.Message = "This shipping line already exists in our database and on save it will be copied to your shipping lines list";
                this.ZeroExistsMessageVisibility = true;
            }
            else {
                this.ZeroExistsMessageVisibility = false;
            }

            this.IsEditEnabled = true;
        });
    }

    public ShippingLinesList: ShippingLineList[] = [];
    private LoadShippingLineListMethod() {
        var myService: ShippingLineListService = new ShippingLineListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            this.ShippingLinesList = myResult.Result;
        });
    }
}
