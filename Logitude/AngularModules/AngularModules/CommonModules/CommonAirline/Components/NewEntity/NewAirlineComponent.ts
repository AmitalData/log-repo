import {Component, OnInit} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {AirlinePMService} from '../../../../Common/Services/StandardPMs/AirlinePMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './NewAirlineComponent.html',
})

export class NewAirlineComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Airline";
    public DataContext: NewAirlineComponent = this;
    public ValidationErrorsList: string[] = [];
    public AirlinePM: AirlinePM = new AirlinePM();
    public TenantPM: TenantPM;
    public IsNewEntityCall: boolean = true;
    RequestPage: string;
    public IsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.entityResourceService.getEntityResourceByTableName("Airline",0).subscribe((response: any) => {
            this.IsVisible = true;
        });
        this.TenantPM = InfraSettings.TenantPM;
        this.Initialize();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.RequestPage = args.RequestPage;
            if (!AppTool.IsNullOrEmpty(args.DefaultValues)) {
             
                var defaultValuesArray: string[] = args.DefaultValues.split('^');
                if (this.AirlinePM) {
                    this.AirlinePM.Code = !AppTool.IsNullOrEmpty(defaultValuesArray[0]) ? defaultValuesArray[0] : "";

                    this.AirlinePM.EnglishName = !AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";

                    this.AirlinePM.LocalName = !AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                }
            }
        }
    }

    ngOnInit() {
        this.AirlinePM.Tenant = this.TenantPM.Id;
        this.AirlinePM.CarrierTypeId = "AL";
        this.AirlinePM.TransportModeId = "A";
        this.AirlinePM.AddedManually = true;
    }

    Initialize() {
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, false);
        this.ZeroExistsMessageVisibility = false;
        this.LoadAirlineListMethod();
    }

    // Properties 
    private zeroExistsMessageVisibility: boolean = false;
    get ZeroExistsMessageVisibility() { return this.zeroExistsMessageVisibility; }
    set ZeroExistsMessageVisibility(value: boolean) {
        this.zeroExistsMessageVisibility = value;
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

    get Code() { return this.AirlinePM.Code; }
    set Code(value: string) {
        if (this.AirlinePM.Code != value) {
            this.AirlinePM.Code = value;
            if (AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetEnabled("InActive", "Airline", false);
                this.UIProperties.SetRequired("Code", "Airline", true);
            }
            else {
                this.UIProperties.SetRequired("Code", "Airline", false);
            }
        }
    }

    get ICAO() { return this.AirlinePM.ICAO; }
    set ICAO(value: string) {
        if (this.AirlinePM.ICAO != value) {
            this.AirlinePM.ICAO = value;
        }
    }

    get EnglishName() { return this.AirlinePM.EnglishName; }
    set EnglishName(value: string) {
        if (this.AirlinePM.EnglishName != value) {
            this.AirlinePM.EnglishName = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, false);
            }
        }
    }

    get Prefix() { return this.AirlinePM.Prefix; }
    set Prefix(value: string) {
        if (this.AirlinePM.Prefix != value) {
            this.AirlinePM.Prefix = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("Prefix", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("Prefix", this.ObjectTableName, false);
            }
        }
    }

    get LocalName() { return this.AirlinePM.LocalName; }
    set LocalName(value: string) {
        if (this.AirlinePM.LocalName != value) {
            this.AirlinePM.LocalName = value;
        }
    }

    get Website() { return this.AirlinePM.Website; }
    set Website(value: string) {
        if (this.AirlinePM.Website != value) {
            this.AirlinePM.Website = value;
        }
    }

    get Remark() { return this.AirlinePM.Remark; }
    set Remark(value: string) {
        if (this.AirlinePM.Remark != value) {
            this.AirlinePM.Remark = value;
        }
    }

    get InActive() { return this.AirlinePM.InActive; }
    set InActive(value: boolean) {
        if (this.AirlinePM.InActive != value) {
            this.AirlinePM.InActive = value;
        }
    }


    //Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.AirlinePM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.SubmitCreatingAirline();
        }
    }

    SubmitCreatingAirline() {

        this.CurrentSession.StartBusyIndicatorSaving();

        var myService: AirlinePMService = new AirlinePMService();

        myService.insert(this.AirlinePM).subscribe((response: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (response != null) {
                if (!response.HasError) {
                    if (this.RequestPage == "SharedManifest") {
                        this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    } else {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                 
                }

                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
            }
        });
    }

    isICAOMode: boolean;
    codeHasError: boolean = false;
    icaoHasError: boolean = false;
    zeroCodeHasError: boolean = false;
    zeroIcaoHasError: boolean = false;

    CodeLostFocusMethod() {
        this.isICAOMode = false;
        this.codeHasError = false;
        this.zeroCodeHasError = false;

        if (!AppTool.IsNullOrEmpty(this.Code) && this.Code.length == 2) {

            this.GetZeroObjectIfExists();
        }
        else {
            this.BuildMessage();
            this.SetUIPropertiesPrefix(true);
        }
    }

    ICAOLostFocusMethod() {
        this.isICAOMode = true;
        this.icaoHasError = false;
        this.zeroIcaoHasError = false;

        if (!AppTool.IsNullOrEmpty(this.ICAO) && this.ICAO.length == 3) {
            this.GetZeroObjectIfExists();
        }
        else {
            this.BuildMessage();
        }
    }

    GetZeroObjectIfExists() {
        if (this.isICAOMode) {
            var x = this.AirlinesList.filter(a => !AppTool.IsNullOrEmpty(a.ICAO) && a.ICAO.toLocaleLowerCase() == this.ICAO.toLocaleLowerCase());
            if (x.length > 0) {
                this.icaoHasError = true;
                this.BuildMessage();
            }

            else {
                this.StartBusyIndicator("");
                this.GetAirlineByICAO(this.ICAO, 0);
            }
        }

        else {
            var x = this.AirlinesList.filter(a => a.Code.toLocaleLowerCase() == this.Code.toLocaleLowerCase());
            if (x.length > 0) {
                this.codeHasError = true;
                this.BuildMessage();
            }

            else {
                this.StartBusyIndicator("");
                this.GetAirlineByCode(this.Code, 0);
            }
        }
    }

    GetAirlineByCode(code: string, tenant: number) {
        var zeroEntity: AirlinePM;
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetAirlineByCode(code, tenant).subscribe((myResult:ServiceResponse) => {
            this.StopBusyIndicator();
            zeroEntity = myResult.Result;
            if (zeroEntity != null) {
                this.GetZeroEntity_Completed(zeroEntity);
            }

            this.BuildMessage();
        });
    }

    GetAirlineByICAO(code: string, tenant: number) {
        var zeroEntity: AirlinePM;
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetAirlineByICAO(code, tenant).subscribe((myResult:any) => {
            this.StopBusyIndicator();
            zeroEntity = myResult;

            if (zeroEntity != null) {
                this.GetZeroEntity_Completed(zeroEntity);
            }

            this.BuildMessage();
        });
    }

    GetZeroEntity_Completed(zeroEntity: AirlinePM) {
        if (this.isICAOMode) {
            this.zeroIcaoHasError = true;
        }

        else {
            this.zeroCodeHasError = true;
        }

        this.Code = zeroEntity.Code;
        this.EnglishName = zeroEntity.EnglishName;
        this.LocalName = zeroEntity.LocalName;
        this.Prefix = zeroEntity.Prefix;
        this.ICAO = zeroEntity.ICAO;
        this.Website = zeroEntity.Website;
        this.Remark = zeroEntity.Remark;

        this.SetUIPropertiesPrefix(false);
    }

    public AirlinesList: AirlineList[] = [];
    private LoadAirlineListMethod() {
        var myService: AirlineListService = new AirlineListService();
        myService.getAll().subscribe((response: ServiceResponse) => {
            this.AirlinesList = response.Result;
        });
    }

    BuildMessage() {

        var myMessage = "";
        var tenantZeroMessage = "This airline already exists in our database and on save it will be copied to your airlines list";

        if (this.codeHasError) {
            myMessage = "This airline already exists!";
        }

        else if (this.zeroCodeHasError) {
            myMessage = tenantZeroMessage;
        }

        if (this.icaoHasError) {
            myMessage += AppTool.IsNullOrEmpty(myMessage) ? "An airline with same ICAO already exists!" : ",     " + "An airline with same ICAO already exists!";
        }

        else if (this.zeroIcaoHasError) {
            if (!this.zeroCodeHasError) {
                myMessage += AppTool.IsNullOrEmpty(myMessage) ? tenantZeroMessage : ", " + tenantZeroMessage;
            }
        }

        if (AppTool.IsNullOrEmpty(myMessage)) {
            this.IsSaveEnabled = true;
            this.ZeroExistsMessageVisibility = false;
            this.UIProperties.SetEnabled("InActive", "Airline", true);
        }

        else {
            if (this.zeroIcaoHasError || this.zeroCodeHasError) {
                if (this.codeHasError || this.icaoHasError) {
                    this.IsSaveEnabled = false;
                }
                else {
                    this.IsSaveEnabled = true;
                }
            }

            else {
                this.IsSaveEnabled = false;
            }

            this.ZeroExistsMessageVisibility = true;
            this.UIProperties.SetEnabled("InActive", "Airline", false);
        }

        this.Message = myMessage;
    }

    SetUIPropertiesPrefix(isEnabled: boolean) {
        if (this.TenantPM.Id != 0) {
            this.UIProperties.SetEnabled("Prefix", "Airline", isEnabled);
        }
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }
}
