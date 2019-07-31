import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {FSRWebService, FSRResultClass} from '../../../../Infrastructure/Services/WebServices/FSRWebService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BranchList} from '../../../../Common/EntityLists/BranchList';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';

@Component({
    moduleId: module.id,
    templateUrl: './SendShipmentFSRComponent.html',
})

export class SendShipmentFSRComponent extends BaseComponent implements OnInit {
    public DataContext: SendShipmentFSRComponent = this;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Master";
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ValidationErrorsList = [];
        this.InitializeComponent();        
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    private workSpaceComponent: any;
    public SetWindowArgs(workSpace: any) {
        this.workSpaceComponent = workSpace
    }

    private InitializeComponent() {
        this.EntityPM = ShipmentTool.GetNewShipmentPM();
        this.EntityPM.IsSendFSRCreatingShipment = true;
        this.EntityPM.ShipmentLevelCode = "C";
        this.EntityPM.TransportModeId = "A";
        this.EntityPM.Ratio = 6;
        this.EntityPM.AWBDeclaredValueForCarriage = "NVD";
        this.EntityPM.AWBDeclaredValueForCustoms = "NCV";
        this.EntityPM.AWBInsurrenceValue = "XXX";
        this.EntityPM.RateClassCode = "Q";
        this.EntityPM.CASSCode = SessionLocator.TenantPM.CASSCode;
        this.EntityPM.IssuingCarrierIATACode = SessionLocator.TenantPM.IATA;
        this.EntityPM.IssuingCarrierAgentId = SessionLocator.TenantPM.AgentId;
        this.EntityPM.IssuingCarrierAddressId = SessionLocator.TenantPM.AddressId;

        this.GetAWBSignature();

        this.Master = null;
        this.DirectionId = "I";
        this.AirlinePrefix = null;
        this.MainCarriageCarrierId = null;
        this.GetCardData();
    }
    private GetAWBSignature() {

        if (this.EntityPM.BranchId) {

            var myResult: string = null;

            var myService = new BranchListService();
            myService.getSingleFromCache(this.EntityPM.BranchId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: BranchList = myResponse.Result;
                    if (list) {
                        myResult = list.Signature;
                    }

                    if (AppTool.IsNullOrEmpty(myResult)) {
                        myResult = SessionLocator.TenantPM.Signature;
                    }

                    this.EntityPM.AWBSignature = myResult;
                }
            });
        }

        else {
            this.EntityPM.AWBSignature = SessionLocator.TenantPM.Signature;
        }
    }

    private SetUIProperties() {
        this.SetUIProperties_Prefix();
        this.SetUIProperties_Master();
    }
    private SetUIProperties_Prefix() {

        this.UIProperties.SetRequired("AirlinePrefix", this.ObjectTableName, false);
        this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, true, "");

        if (AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            this.UIProperties.SetRequired("AirlinePrefix", this.ObjectTableName, true);
        }

        else if (!FormatTool.IsNumeric(this.AirlinePrefix)) {
            this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, false, "Invalid Format");
        }

        else if (this.AirlinePrefix.length != 3) {
            this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, false, "Prefix length is 3 digits");
        }
    }
    private SetUIProperties_Master() {

        this.UIProperties.SetRequired("Master", this.ObjectTableName, false);
        this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");

        if (AppTool.IsNullOrEmpty(this.Master)) {
            this.UIProperties.SetRequired("Master", this.ObjectTableName, true);
        }

        else {
            var myResult: string = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, true, true);
            if (!AppTool.IsNullOrEmpty(myResult)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
            }
        }
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(newValue: string) {
        if (this.EntityPM.AirlinePrefix != newValue) {
            this.EntityPM.AirlinePrefix = newValue;
            this.SetUIProperties_Prefix();
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(newValue: string) {
        if (this.EntityPM.Master != newValue) {
            this.EntityPM.Master = newValue;
            this.SetUIProperties_Master();
        }
    }

    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;

            if (newValue == "E") {
                this.EntityPM.FreightPrepaidCollectId = SessionLocator.TenantPM.MasterExportFreightPrepaidCollectId;
                this.EntityPM.OtherPrepaidCollectId = SessionLocator.TenantPM.MasterExportOtherPrepaidCollectId;
            }

            else {
                this.EntityPM.FreightPrepaidCollectId = SessionLocator.TenantPM.MasterImportFreightPrepaidCollectId;
                this.EntityPM.OtherPrepaidCollectId = SessionLocator.TenantPM.MasterImportOtherPrepaidCollectId;
            }

            ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
        }
    }

    private GetCardData() {
        if (this.EntityPM.IssuingCarrierAgentId != null) {

            var myService: CardListService = new CardListService();

            myService.getSingle(this.EntityPM.IssuingCarrierAgentId).subscribe(myResult => {
                var myCard: any = myResult;

                if (myCard != null) {
                    this.EntityPM.IssuingCarrierAgentNote = myCard.Notes;
                    this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                    this.EntityPM.IssuingCarrierAddressId = myCard.MainAddressId;
                }
            });
        }
    }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
        }
    }

    OnPrefixLostFocus(myPrefix: string) {
        if (!AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            if (this.AirlinePrefix.length == 3) {
                this.GetAirlineByPrefix();
            }
        }
    }

    GetAirlineByPrefix() {
        var myService = new PartnersDomainService();
        myService.GetAirlineByPrefix(this.AirlinePrefix).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: AirlineList = myResponse.Result;
                if (list) {
                    this.MainCarriageCarrierId = list.Id;
                }
            }
        });
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendButtonClicked() {

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            errors.push(msg.replace("%FieldName", "Prefix"));
        }

        else if (!FormatTool.IsNumeric(this.AirlinePrefix)) {
            errors.push("Prefix invalid format");
        }

        else if (this.AirlinePrefix.length != 3) {
            errors.push("Prefix length is 3 digits");
        }

        if (AppTool.IsNullOrEmpty(this.Master)) {
            errors.push(msg.replace("%FieldName", "Master"));
        }

        else {
            var myResult: string = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, true, true);
            if (!AppTool.IsNullOrEmpty(myResult)) {
                errors.push(myResult);
            }
        }

        if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.TTY)) {
            errors.push("Tenant communication parameter (TTY) is missing");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicator("Sending...");

            if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D") {
                this.EntityPM.ShipperId = this.EntityPM.IssuingCarrierAgentId;
                this.EntityPM.ShipperAddressId = this.EntityPM.IssuingCarrierAddressId;
                this.EntityPM.ShipperNote = this.EntityPM.IssuingCarrierAgentNote;
                this.EntityPM.ShipperName = this.EntityPM.IssuingCarrierAgentName;
            }

            else {
                this.EntityPM.ConsigneeId = this.EntityPM.IssuingCarrierAgentId;
                this.EntityPM.ConsigneeAddressId = this.EntityPM.IssuingCarrierAddressId;
                this.EntityPM.ConsigneeNote = this.EntityPM.IssuingCarrierAgentNote;
                this.EntityPM.ConsigneeName = this.EntityPM.IssuingCarrierAgentName;
            }

            this.Send();
        }
    }

    private Send() {

        var myService = new FSRWebService()
        myService.SendFSRShipment(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                if (myResponse.Result instanceof FSRResultClass) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("FSR was sent successfully Please check airline updates query for carrier’s response");

                    this.InitializeComponent();

                    if (this.workSpaceComponent != null) {
                        this.workSpaceComponent.LoadQueriesCounts();
                        this.workSpaceComponent.LoadRecentShipments();
                    }
                }

                else {
                    var errors: string[] = [];
                    errors.push("Sending FSR failed");
                    this.ValidationErrorsList = errors;
                }
            }
        });
    }

}
