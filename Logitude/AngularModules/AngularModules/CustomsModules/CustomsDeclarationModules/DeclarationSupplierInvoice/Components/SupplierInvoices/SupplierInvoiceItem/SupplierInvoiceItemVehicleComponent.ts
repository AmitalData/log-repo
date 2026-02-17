declare var window: any;

import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SupplierInvoiceItemPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import {ObservableCollection} from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import {TextCodeTranslator} from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../../Controls/Windows/MessageWindow';
import { SupplierInvoiceItemVehiclePM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemVehiclePM';
import { SupplierInvoicePM } from '../../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../../../Infrastructure/Services/EntityResourceService';
import {Validator} from '../../../../../../Infrastructure/Validators/Validator';
import {VehicleExtendedPMService} from '../../../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService';
import { VehiclePM } from '../../../../../../Customs/EntityPMs/VehiclePM';
import {LogitudeWindow} from '../../../../../../Controls/Windows/LogitudeWindow';
import {VehicleList} from '../../../../../../Customs/EntityLists/VehicleList';
import {FeatureLocator} from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { VehiclePMService } from '../../../../../../Customs/Services/StandardPMs/VehiclePMService';

import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsSettingList } from '../../../../../../Customs/EntityLists/CustomsSettingList';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { CardListService } from '../../../../../../Common/Services/StandardLists/CardListService'
import { CardList } from '../../../../../../Common/EntityLists/CardList';
import { DeclarationPM } from '../../../../../../Customs/EntityPMs/DeclarationPM';
import {CustomsSettingExtendedListService} from '../../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';

@Component({
    moduleId: module.id,
    templateUrl: './SupplierInvoiceItemVehicleComponent.html',
})



export class SupplierInvoiceItemVehicleComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.SupplierInvoiceItemVehicle";
    public DataContext = this;
    public invoiceItemPM: SupplierInvoiceItemPM;
    public ItemsSource: ObservableCollection;
    public declarationPM: DeclarationPM;
    public supplierInvoicePM: SupplierInvoicePM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    public ValidationErrorsList: string[] = [];
    public OriginalItemPM: SupplierInvoiceItemPM;
    public ClonedItemPM: SupplierInvoiceItemPM;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public IsReleaseFileDisplay: boolean = false;

    vehiclePMService: VehiclePMService = new VehiclePMService();
    vehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();
    IsVisibile: boolean;

    cardListService: CardListService = new CardListService();
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    _CustomsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        console.log("....|| SupplierInvoiceItemVehicleComponent ||....");
    }

    parent;

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemVehicle").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(response => {


                    this.IsVisibile = true;
                    this.invoiceItemPM = args.SupplierInvoiceItemPM;
                    this.BuildVehicleList();
                    this.IsDisplayOnly = args.IsDisplayOnly;
                    this.OriginalItemPM = args.SupplierInvoiceItemPM;
                    this.parent = args.parent;
                    this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);
                    var table = window.ObjectTables.filter(d => d.Name === 'Customs.Vehicle')[0];
                    var feature = FeatureLocator.Features.filter(f => (f.FeatureTypeCode == "MODL") && f.ObjectTableId == table.Id)[0];
                    if (feature) {
                        this.SearchButtonVisibility = true;
                    }
                    this.declarationPM = args.declarationPM;
                    if (this.declarationPM.IsReleaseFile) {
                        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_VEHICLE_BLK", "NON", "NON", SessionLocator.Tenant)
                        .subscribe(
                        (response: ServiceResponse) => {
                            let obj = response.Result;
                            if (obj) {
                                let DefaultValue = obj['DefaultValue'];
                                if (DefaultValue == "Y") {
                                    this.IsReleaseFileDisplay = true;
                                }
                            }
                        }
                        );
                    }
                    


                    this.supplierInvoicePM = args.SupplierInvoicePM;

                    var featureVehicle = FeatureLocator.Features.filter(f => f.Code == "LOADVEHICLESFROMUNI")[0];
                    if (featureVehicle) {
                        this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
                            var list: CustomsSettingList[] = response.Result;

                            if (!AppTool.IsNullOrEmpty(list)) {
                                var customsSetting = list.filter(d => d.Tenant == SessionLocator.Tenant)[0];

                                if (!AppTool.IsNullOrEmpty(customsSetting)) {
                                    if (customsSetting.IsConnectedToUniFreight) {
                                        this.VehiclesFilesButtonVisibility = true;
                                        ////let myDec = this.CurrentSession.CurrentEditComponent.EntityPM;
                                        //let myDec = args.declarationPM;
                                        //if (AmitalGatewayUtil.Instance.IsDeclarationInUse(myDec.CustomFileNo, myDec.IsConvertedDeclaration, myDec.IsConnectedToUnifreight)) {
                                        //    this.VehiclesFilesButtonVisibility = true;
                                        //}
                                    }
                                }
                            }
                        });

                    }
                });
            });
        }
    }
    
    VehiclesFilesButtonVisibility: boolean = false;
    SearchButtonVisibility: boolean;
    Add(supplierInvoiceItemVehiclePM: SupplierInvoiceItemVehiclePM) {
        if (!this.IsDisplayOnly&&!this.IsReleaseFileDisplay) {

            //if (!this.ValidateList())
            //    return;

            ////last item valid
            //var lastitem = this.ItemsSource.Collection[this.ItemsSource.Length - 1];
            //if (lastitem) if (lastitem.valid) return;

            var counter: number = 0;

            //Get LineNumber
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {

                var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
                if (items.length == 0) counter = 0;
                else {
                    counter = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].LineNumber;
                }
            }
            counter += 1;

            //Get SequenceNumeric
            var sequence: number = 0;
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
                if (items.length == 0) sequence = 0;
                else {
                    sequence = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].SequenceNumeric;
                }
            }
            sequence += 1;


            var item: SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(this.invoiceItemPM);

            item.DeclarationId = this.invoiceItemPM.DeclarationId;
            item.Tenant = this.invoiceItemPM.Tenant;
            item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
            item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
            item.LineNumber = counter;
            item.SequenceNumeric = sequence;
            item.ChangeSetOp = "insert";

            if (supplierInvoiceItemVehiclePM != null) {
                item.RichbitFileNumber = supplierInvoiceItemVehiclePM.RichbitFileNumber;
                item.VehicleChassisNumber = supplierInvoiceItemVehiclePM.VehicleChassisNumber;
                item.RichbitFileStatus = supplierInvoiceItemVehiclePM.RichbitFileStatus;
            }

            if (!this.invoiceItemPM.SupplierInvoiceItemVehicles.includes(item)) {
                this.invoiceItemPM.AddSupplierInvoiceItemVehicle(item);
                this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
            }
        }
    }

    BuildVehicleList() {
        this.ItemsSource.Clear();
        for (let item of this.invoiceItemPM.SupplierInvoiceItemVehicles) {
            this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
        }


    }

    CancelButtonClicked() {


        if (this.invoiceItemPM.IsDirty && !this.IsDisplayOnly ) {
            var confirm = new ConfirmWindow();

            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.OkButtonClicked();



                }
                else {
                    this.RejectChanges();
                    this.CurrentSession.CloseCurrentWindow();
                }

            });

        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }



    }

    vehExisitInScreen: boolean = false;

    // OK
    // OK 
    vehicles: VehiclePM[] = [];
    OkButtonClicked() {
        
        if (this.ItemsSource.Length > 0) {


            // Build richbit numbers list - string
            var richbitNumbersList: string = "";
            this.ItemsSource.Collection.forEach((veh: InvoiceItemVehicleLine) => {

                var entity = veh.entityPM;
                var oldEntity: any = veh.entityPM.OldEntityPM;

                if (entity.IsDirty)
                {
                    if (entity.ChangeSetOp == "insert") {
                        if (veh.RichbitFileNumber)
                            richbitNumbersList += veh.RichbitFileNumber + ",";

                    }
                    else
                        if (oldEntity)
                            if (entity.RichbitFileNumber != oldEntity.RichbitFileNumber && entity.RichbitFileNumber != oldEntity.richbitFileNumber)
                                if (veh.RichbitFileNumber)
                                    richbitNumbersList += veh.RichbitFileNumber + ",";

                }

            });

            // validate it in server
            if (richbitNumbersList)
            {
                this.vehicleExtendedPMService.GetCheckRichbitNumbersError(richbitNumbersList).subscribe(response => {
                    if (response) {

                        var vehicleValidationError: VehicleValidationError[] = [];
                        vehicleValidationError = response.Result;

                        if (vehicleValidationError && vehicleValidationError.length > 0) {
                            console.log("[Server Validation] ==> ", vehicleValidationError);

                            
                            this.ValidationErrorsList = [];
                            var VehiclesErrorsList = [];
                            vehicleValidationError.forEach(error =>
                            {
                                // validate (Delete-Add) problem
                                // if same declaration and same invoice, validate same invoice problem
                                if (error.DeclarationId == this.declarationPM.Id && error.InvoiceCounterKey == this.supplierInvoicePM.InvoiceCounterKey)
                                {
                                    //check if invalid itemveh is deleted from the same invoice or not
                                    var deleted = true;
                                    for (var j = 0; j < this.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                                        var item = this.parent.EntityPM.SupplierInvoiceItems[j];

                                        var sameItem: SupplierInvoiceItemVehiclePM = item.SupplierInvoiceItemVehicles.find((veh: SupplierInvoiceItemVehiclePM) => veh.RichbitFileNumber == error.RichbitFileNumber);
                                        if (sameItem)
                                        {
                                            if (sameItem.ChangeSetOp != "insert")
                                            {
                                                //check updated?
                                                if (sameItem.RichbitFileNumber == (sameItem.OldEntityPM.RichbitFileNumber ? sameItem.OldEntityPM.RichbitFileNumber : sameItem.RichbitFileNumber)) //richbit not changed
                                                    deleted = false;
                                            }
                                        }

                                    }

                                    //show result
                                    if (deleted)
                                    {
                                        //show message you must save item after adding it again
                                        this.ValidationErrorsList.push("שלדה זו קיימת בחשבון ספק. חובה לשמור את חשבון הספק קודם"); //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                                    }
                                    else {
                                        //ext: 
                                        //used in same invoice 
                                        if (error.InvoiceItemLineNumber != this.invoiceItemPM.LineNumber) {
                                            //var msg = (error.IsVehicle ? "The vehicle (" : "The richbit (") + error.RichbitFileNumber + ") is used in another item in this invoice";
                                            var msg = (error.IsVehicle ? "שילדה (" : "ריכיבת (") + error.RichbitFileNumber + ") קיימת בפרט אחר בחשבון ספק";// + " - חשבון " + error.InvoiceCounterKey;
                                        } else {
                                            //var msg = (error.IsVehicle ? "Vehicle (" : "Richbit no.(") + error.RichbitFileNumber + ") is used in this invoice item!";
                                            var msg = "רכב זה (" + error.RichbitFileNumber + ") כבר הוזן בשורות קודמות";
                                        }
                                        //txe
                                        this.ValidationErrorsList.push(msg);
                                    }

                                }
                                else if (error.DeclarationId == this.declarationPM.Id && error.InvoiceCounterKey != this.supplierInvoicePM.InvoiceCounterKey) {
                                    //the validation error in same declaration, but different invoice
                                    //ext:
                                    var msg = (error.IsVehicle ? "שילדה (" : "ריכיבת (") + error.RichbitFileNumber + ") קיימת בחשבון ספק אחר בהצהרה";// + " - חשבון " + error.InvoiceCounterKey;;
                                    this.ValidationErrorsList.push(msg);
                                    //txe 
                                }
                                else
                                {
                                    //richbit/vehicle found in another declaration
                                    if (error.IsVehicle) // richbit can be duplicated in another declaration
                                        VehiclesErrorsList.push(error.ValidationText);
                                }
                            });

                            if (this.ValidationErrorsList.length == 0) {

                                if (VehiclesErrorsList.length > 0) {
                                    //show message box
                                    //var confirmWindow = new ConfirmWindow();
                                    //confirmWindow.Width = 400;
                                    //var confirmMsg = "";
                                    //VehiclesErrorsList.forEach(error => {confirmMsg += error + "\n";});
                                    //confirmWindow.Show(confirmMsg);
                                    var windowArgs: any = {};
                                    windowArgs.Errors = VehiclesErrorsList;
                                    windowArgs.CancelButtonVisibility = true;
                                    windowArgs.SaveButtonText = "עדכן";
                                    windowArgs.CancelButtonText = "בטל";
                                    windowArgs.ComponentHeight = '328px';

                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 600;
                                    logWindow.Height = 400;
                                    //logWindow.Title = windowTitle;
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.WindowClosed.subscribe(($event: any) => {
                                        switch ($event) {
                                            case "ok": {
                                                //ok
                                                if (this.ValidateList())
                                                    this.SubmitChanges();

                                                break;
                                            }
                                            case "cancel": {

                                                break;
                                            }
                                        }
                                    });

                                    logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                                    this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                    

                                } else {
                                    //ok
                                    if (this.ValidateList())
                                        this.SubmitChanges();
                                }
                            }
                        }
                        else {
                            //ok
                            if (this.ValidateList())
                                this.SubmitChanges();
                            
                        }
                    }
                });

            } else {
               // if (this.ValidateList())
                    this.SubmitChanges();
            }

        }
        else
        {
            
            // no input
            //if (this.ValidateList())
                this.SubmitChanges();
        }

    }

    SubmitChanges() {

        //if (!this.ValidateList())
        //    return;

        //if (this.EnableOkButton) {
        this.ValidationErrorsList = [];
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);



        for (let item of this.invoiceItemPM.SupplierInvoiceItemVehicles) {
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (AppTool.IsNullOrEmpty(item.RichbitFileNumber) && AppTool.IsNullOrEmpty(item.VehicleChassisNumber)) {
                errors.push(TextCodeTranslator.Translate("Customs.General.O.EmptyVehicle"));
            }


        }

        if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
            this.invoiceItemPM.VehicleStatus = true;
            // trigger.VehicleStatusVisibility = Visibility.Visible;
        }
        else {
            this.invoiceItemPM.VehicleStatus = true;
            //  trigger.VehicleStatusVisibility = Visibility.Collapsed;
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            for (let item of this.invoiceItemPM.SupplierInvoiceItemVehicles) {
                if (!AppTool.IsNullOrEmpty(item.VehicleId) || !AppTool.IsNullOrEmpty(item.RichbitFileNumber)) {
                    item.VehicleTypeCode = "ZZZ";
                }


                else {
                    item.VehicleTypeCode = "CN";
                }
            }
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                this.invoiceItemPM.VehicleStatus = true;
            }
            else {
                this.invoiceItemPM.VehicleStatus = false;
            }

            this.ResequenceLines();


            ////add new vehs to temprory list in general tab
            //if (this.invoiceItemPM.SupplierInvoiceItemVehicles) {
            //    this.invoiceItemPM.SupplierInvoiceItemVehicles.forEach((veh) => {
            //        var exist = this.parent.addedVehicles.find(d => d == veh);
            //        if (!exist)
            //            this.parent.addedVehicles.push(veh);
            //    });
            //}

            this.CurrentSession.CloseCurrentWindow();
        }
        //} else {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push("Please check items");
        //}

    }

    ResequenceLines() {
        var index = 0;
        var list = this.invoiceItemPM.SupplierInvoiceItemVehicles;
        if (list.length > 0) {
            //list.sort(a => a.SequenceNumeric).forEach((item) =>
            list.sort((a, b) => { return (a.SequenceNumeric > b.SequenceNumeric) ? 1 : ((b.SequenceNumeric > a.SequenceNumeric) ? -1 : 0); })
                .forEach((item) =>
                {
                    index++;
                    if (item.SequenceNumeric != index)
                        item.SequenceNumeric = index;


                });
        }
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    CloneEntity(entityToClone: SupplierInvoiceItemPM) {

        var clonedEntity: SupplierInvoiceItemPM;
        clonedEntity = new SupplierInvoiceItemPM(entityToClone.EntityParentPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        clonedEntity.SupplierInvoiceItemVehicles = [];
        entityToClone.SupplierInvoiceItemVehicles.forEach((itemMod) => {
            var clonedItemMod = new SupplierInvoiceItemVehiclePM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvoiceItemVehicles.push(clonedItemMod);
        });
        
        return clonedEntity;
    }


    OnRowEnded($event) {
        if (($event) == this.ItemsSource.Length) {
            this.Add(null);

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add(null);
        }
    }
    enableOkButton: boolean = true;
    get EnableOkButton() { return this.enableOkButton; }
    set EnableOkButton(value: boolean) { this.enableOkButton = value; }

    SearchButtonClicked() {
      
            var windowArgs: any = {};
           
           

            windowArgs.Parent = this;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 600;
           
          
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
          //  logWindow.WindowClosed.subscribe(($event: any) => this.SelectVehicleCompleted($event));
      logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/VehiclesSearchComponent');
       
    }

    selectedVehicles: VehicleList[] = [];
    SelectVehicleCompleted(selectedVehicles: VehicleList[] ) {
        if (selectedVehicles != null) {
            for (let vehicle of selectedVehicles)
            {

                var lineNumber:number = 0;
                var sequence:number = 0;

                if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {

                    var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
                    if (items.length == 0) lineNumber = 0;
                    else {
                        lineNumber = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].LineNumber;
                        }


                    var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
                    if (items.length == 0) sequence = 0;
                    else {
                        sequence = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].SequenceNumeric;
                    }

                                 
                 
                }

                lineNumber += 1;
                sequence += 1;

                var item: SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(this.invoiceItemPM);
                item.ChangeSetOp = "insert";

                item.DeclarationId = this.invoiceItemPM.DeclarationId;
                    item.Tenant = this.invoiceItemPM.Tenant;
                    item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
                    item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
                    item.LineNumber = lineNumber;
                    //item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    item.VehicleChassisNumber = vehicle.VehicleChassisNumber;
                    item.RichbitFileNumber = vehicle.RichbitFileNumber;
                    item.RichbitFileStatus = vehicle.StatusName;
                    item.SequenceNumeric = sequence;
                    item.VehicleId = vehicle.Id;

                    var itemvehicle = this.invoiceItemPM.SupplierInvoiceItemVehicles.find(d => d.VehicleChassisNumber == item.VehicleChassisNumber && d.RichbitFileNumber == item.RichbitFileNumber);

                    if (!itemvehicle){
                  
                        this.invoiceItemPM.AddSupplierInvoiceItemVehicle(item);
                        this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
                    }
                // this code moved to server.
                    //this.vehiclePMService.get(vehicle.Id).subscribe(response => {
                    //    if (response) {
                    //        if (!response.HasError) {
                    //            var vehicle: VehiclePM = response.Result;
                    //            if (vehicle) {
                    //                vehicle.DeclarationId = this.invoiceItemPM.DeclarationId;
                    //            }
                    //           this.vehiclePMService.update(vehicle).subscribe(response => {
                    //           });
                    //        }
                    //    }
                    //});
              


            }
        }
    }

    VehiclesFilesButtonClicked() {
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(

            (myUnifreightMessageM: UnifreightMessageM) => {
                if (myUnifreightMessageM.LogitudeViewModel == "SupplierInvoiceItemVehicleComponent" &&
                    (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                      myUnifreightMessageM.LogitudeEntityNumber == this.invoiceItemPM.DeclarationId) {
                        sub.unsubscribe();
                        this.CurrentSession.StopBusyIndicator();
                        this.UnifreightGetRihbitFromTransmissionsCallbackAction(myUnifreightMessageM);
                }
            });

        this.CurrentSession.StartBusyIndicator("Loading ...");

        this.cardListService.getSingle(this.declarationPM.CustomerId)
            .subscribe(res => {
                let cardList: CardList = res.Result;
                let unifaceCustId: string = ""
                if (!AppTool.IsNullOrEmpty(cardList)) {
                    unifaceCustId = cardList.Code;
                }
                AmitalGatewayUtil.Instance
                    .GetRihbitFromTransmissions(
                    this.declarationPM.CustomFileNo,
                    this.invoiceItemPM.DeclarationId,
                    "SupplierInvoiceItemVehicleComponent",
                    unifaceCustId);
            });
    }


    _MyResponseObject: any = null;
    private UnifreightGetRihbitFromTransmissionsCallbackAction(unifreightMessageM: UnifreightMessageM) {

        let rihbitDatalist: string = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.RihbitData");

        if (AppTool.IsNullOrEmpty(rihbitDatalist)) {
            console.log("Rihbit Data is null - u did not choose any Vehicle")
            return;
        }

        if (!AppTool.IsNullOrEmpty(rihbitDatalist)) {
            this._MyResponseObject = JSON.parse(rihbitDatalist);

            if (this._MyResponseObject != null && this._MyResponseObject.Vehicle != null){
                for (let item of this._MyResponseObject.Vehicle) {
                    var supplierInvoiceItemVehiclePM: SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(this.invoiceItemPM);
                    supplierInvoiceItemVehiclePM.RichbitFileNumber = item.RichbitFileNumber;
                    supplierInvoiceItemVehiclePM.VehicleChassisNumber = item.VehicleChassisNumber;
                    supplierInvoiceItemVehiclePM.RichbitFileStatus = item.RichbitFileStatus;
                    this.Add(supplierInvoiceItemVehiclePM);
                }
            }
        }
    }

    ValidateList() {
        var errMsg = TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " ";
        var valid = true;
        var errors = [];

        if (this.ItemsSource.Collection) {

            //validate duplicated vehicle in same screen
            this.ItemsSource.Collection.forEach((item1: InvoiceItemVehicleLine) => {
                //var vehicle: VehiclePM = null;
                //if (this.vehicles && this.vehicles.length > 0) {
                //    vehicle = this.vehicles.filter(d => d.RichbitFileNumber == item1.RichbitFileNumber)[0];
                //}
                //if (vehicle) {
                    this.ItemsSource.Collection.forEach((item2: InvoiceItemVehicleLine) => {

                        if (item1.SequenceNumeric != item2.SequenceNumeric) {
                            if (item1.RichbitFileNumber && item2.RichbitFileNumber) {
                                if (item1.RichbitFileNumber == item2.RichbitFileNumber) {
                                    var msg = "רכב זה (" + item2.RichbitFileNumber + ") כבר הוזן בשורות קודמות";
                                    errors.push(msg); //duplicated vehicle
                                    valid = false;
                                }
                            }
                        }

                    });
               // }
            });


            //validate duplicated vehicle in another item in this declaration
            for (var i = 0; i < this.ItemsSource.Collection.length; i++) {
                var vehicle = this.ItemsSource.Collection[i];
                //var vehicleNotItem: VehiclePM = null;
                //if (this.vehicles && this.vehicles.length > 0) {
                //    vehicleNotItem = this.vehicles.filter(d => d.RichbitFileNumber == vehicle.RichbitFileNumber)[0];
                //}
                //if (vehicleNotItem) {
                    for (var j = 0; j < this.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                        var item = this.parent.EntityPM.SupplierInvoiceItems[j];
                        var exist;
                        if (vehicle.RichbitFileNumber) {
                            exist = item.SupplierInvoiceItemVehicles
                                .some((veh: SupplierInvoiceItemVehiclePM) =>
                                    veh.InvoiceItemLineNumber != this.invoiceItemPM.LineNumber &&
                                    veh.RichbitFileNumber == vehicle.RichbitFileNumber
                                );
                        }
                        if (exist) {
                            var txt = "ריכיבית (" + vehicle.RichbitFileNumber + ") קיימת בהצהרה מספר (" + this.declarationPM.CustomFileNo + ")";
                            errors.push(txt);
                            valid = false;
                        }

                    }
              //  }
            }
            

            //fil validation list
            if (valid) {
                this.ValidationErrorsList = [];
                return true;
            }
            else {
                this.ValidationErrorsList = errors;
                return false;
            }


        } else {
            this.ValidationErrorsList = [];
            return true;
        }

    }

}


export class InvoiceItemVehicleLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.SupplierInvoiceItemVehicle";
    public entityPM: SupplierInvoiceItemVehiclePM;
    public invoiceItem: SupplierInvoiceItemPM;
    public parent: SupplierInvoiceItemVehicleComponent;
    vehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();
    vehiclePMService: VehiclePMService = new VehiclePMService();

    public valid: boolean = true;

    public vehicle: VehiclePM;
    oldVehicle: VehiclePM;

    vehicleSelected: boolean = false;

    constructor(EntityPM: SupplierInvoiceItemVehiclePM, Parent: SupplierInvoiceItemVehicleComponent, Item: SupplierInvoiceItemPM) {
        super();
        this.entityPM = EntityPM;
        this.parent = Parent;
        this.invoiceItem = Item;

        if (this.RichbitFileNumber != null && this.VehicleChassisNumber == null) {
            this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);
        }

        if (this.RichbitFileNumber == null && this.VehicleChassisNumber != null) {
            this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
        }
       

    }

    //#region properties

  
    get SequenceNumeric() { return this.entityPM.SequenceNumeric; }


    get RichbitFileNumber() {

        return this.entityPM.RichbitFileNumber;
    }
    set RichbitFileNumber(value: string) {
        if (this.entityPM.RichbitFileNumber != value) {
            this.entityPM.RichbitFileNumber = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.entityPM.RichbitFileNumber = value.trim();
            }

        }
    }

    get VehicleChassisNumber() {

        return this.entityPM.VehicleChassisNumber;
    }
    set VehicleChassisNumber(value: string) {
        if (this.entityPM.VehicleChassisNumber != value) {
            this.entityPM.VehicleChassisNumber = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.entityPM.VehicleChassisNumber = value.trim();
            }

        }
    }
    get RichbitFileStatus() { return this.entityPM.RichbitFileStatus; }
    set RichbitFileStatus(value: string) {
        if (this.entityPM.RichbitFileStatus != value) {
            this.entityPM.RichbitFileStatus = value;

        }
    }


    get ExcludeFromInterface() { return this.entityPM.ExcludeFromInterface; }
    set ExcludeFromInterface(value: boolean) {
        if (this.entityPM.ExcludeFromInterface != value) {
            this.entityPM.ExcludeFromInterface = value;

        }
    }
    
    //#endregion
   

    SetFileNumber(logCellTemplate: any, VehicleChassisNumberTextBox: any) {
        //popup validation removed due to some problems whith popup show 
        this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(this.VehicleChassisNumber, null).subscribe(response => {
            if (response) {
                this.vehicle = response.Result;
                if (this.vehicle != null) {
                    if (this.vehicle.DeclarationId != null) {

                        ////validation: if the vehicle deleted and added before saving invoice
                        //if (this.vehicle.DeclarationId == this.entityPM.DeclarationId) {
                        //    for (var j = 0; j < this.parent.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                        //        var item = this.parent.parent.EntityPM.SupplierInvoiceItems[j];

                        //        var exist = item.SupplierInvoiceItemVehicles
                        //            .some((veh: SupplierInvoiceItemVehiclePM) =>
                        //                //veh.InvoiceItemLineNumber != this.invoiceItem.LineNumber
                        //                veh.RichbitFileNumber == this.RichbitFileNumber
                        //            //&& veh.VehicleChassisNumber == this.VehicleChassisNumber
                        //            );
                        //        if (exist) {
                        //            //
                        //        } else {
                        //            if (this.entityPM.ChangeSetOp == "insert") {
                        //                var messageWindow = new MessageWindow();
                        //                var msg = "שלדה זו קיימת בחשבון ספק, חובה לשמור את חשבון הספק קודם"; //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                        //                messageWindow.RTL = true;
                        //                messageWindow.Show(msg);
                        //                this.VehicleChassisNumber = "";
                        //            }
                        //            return;
                        //        }
                        //    }
                        //}


                        this.valid = false;
                        this.vehicleSelected = false;

                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false, TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " " + this.vehicle.CustomFileNumber);
                        this.parent.EnableOkButton = false;

                        ////lock focus
                        //SessionLocator.SustainFocusOnCell = true;
                        //this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: VehicleChassisNumberTextBox.InputId });


                        
                        this.entityPM.RichbitFileNumber = this.vehicle.RichbitFileNumber;
                        this.entityPM.RichbitFileStatus = this.vehicle.StatusName;

                        this.entityPM.VehicleId = this.vehicle.Id;
                        this.RichbitFileStatus = this.vehicle.StatusName;
                        //this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        this.oldVehicle = this.vehicle;
                    }
                    else {
                        this.valid = true;
                        this.vehicleSelected = true;

                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        this.entityPM.RichbitFileNumber = this.vehicle.RichbitFileNumber;
                        this.entityPM.RichbitFileStatus = this.vehicle.StatusName;


                        this.entityPM.VehicleId = this.vehicle.Id;
                        this.RichbitFileStatus = this.vehicle.StatusName;
                        this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
                        this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        this.oldVehicle = this.vehicle;
                        this.parent.EnableOkButton = true;
                    }


                }
                else {
                    this.vehicleSelected = true;

                    this.valid = true;
                    this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
                    //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                    this.entityPM.RichbitFileNumber = null;
                    this.entityPM.VehicleId = null;
                    this.RichbitFileStatus = null;
                    this.parent.EnableOkButton = true;
                    if (this.oldVehicle != null) {
                        this.oldVehicle.DeclarationId = null;
                    }

                }
            }

        });
    }

    SetChassisNumber(logCellTemplate: any, RichbitFileNumbernTextBox: any) {
        //popup validation removed due to some problems whith popup show 
        this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(null, this.RichbitFileNumber).subscribe(response => {
            if (response) {
                this.vehicle = response.Result;
                if (this.vehicle != null) {
                    if (this.vehicle.DeclarationId != null) {

                        ////validation: if the vehicle deleted and added before saving invoice
                        //if (this.vehicle.DeclarationId == this.entityPM.DeclarationId) {
                        //    for (var j = 0; j < this.parent.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                        //        var item = this.parent.parent.EntityPM.SupplierInvoiceItems[j];

                        //        var exist = item.SupplierInvoiceItemVehicles
                        //            .some((veh: SupplierInvoiceItemVehiclePM) =>
                        //                //veh.InvoiceItemLineNumber != this.invoiceItem.LineNumber
                        //                veh.RichbitFileNumber == this.RichbitFileNumber
                        //            //&& veh.VehicleChassisNumber == this.VehicleChassisNumber
                        //            );
                        //        if (exist) {
                        //            //
                        //        } else {
                        //            if (this.entityPM.ChangeSetOp == "insert") {

                                        
                        //                var messageWindow = new MessageWindow();
                        //                var msg = "שלדה זו קיימת בחשבון ספק, חובה לשמור את חשבון הספק קודם"; //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                        //                messageWindow.RTL = true;
                        //                messageWindow.Show(msg);
                        //                this.RichbitFileNumber = "";


                        //            }
                        //            return;
                        //        }
                        //    }
                        //}

                        //connected to dec --- NOT VALID
                        this.valid = false;
                        this.vehicleSelected = false;

                        this.parent.EnableOkButton = false;



                        this.entityPM.VehicleChassisNumber = this.vehicle.VehicleChassisNumber;
                        this.entityPM.RichbitFileStatus = this.vehicle.StatusName;

                        this.entityPM.VehicleId = this.vehicle.Id;
                        this.RichbitFileStatus = this.vehicle.StatusName;
                        //this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        this.oldVehicle = this.vehicle;

                        //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false, TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " " + this.vehicle.CustomFileNumber);

                        ////lock focus
                        //SessionLocator.SustainFocusOnCell = true;
                        //this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: RichbitFileNumbernTextBox.InputId });

                    }
                    else {
                        //VALID 
                        this.valid = true;
                        this.vehicleSelected = true;
                        this.parent.EnableOkButton = true;

                        //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);

                        this.entityPM.VehicleChassisNumber = this.vehicle.VehicleChassisNumber;
                        this.entityPM.RichbitFileStatus = this.vehicle.StatusName;

                        this.entityPM.VehicleId = this.vehicle.Id;
                        this.RichbitFileStatus = this.vehicle.StatusName;
                        this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        this.oldVehicle = this.vehicle;


                    }

                }
                else {
                    // NO Vehicle
                    this.valid = true;
                    this.vehicleSelected = true;
                    this.parent.EnableOkButton = true;

                    //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                    this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);
                    this.entityPM.VehicleChassisNumber = null;
                    this.entityPM.VehicleId = null;
                    this.RichbitFileStatus = null;
                    if (this.oldVehicle != null) {
                        this.oldVehicle.DeclarationId = null;
                    }


                }
            }

        });

    }

    DeleteButtonClicked() {

        this.parent.ItemsSource.Remove(this);
        if (this.parent.invoiceItemPM.SupplierInvoiceItemVehicles.includes(this.entityPM)) {
            this.parent.invoiceItemPM.RemoveSupplierInvoiceItemVehicle(this.entityPM);

            //delete from general
            //var invoice = this.parent.parent.declarationPM.SupplierInvoices.find(d => d.InvoiceCounterKey == this.parent.supplierInvoicePM.InvoiceCounterKey);
            var invoice = this.parent.supplierInvoicePM;
            var exist = false;
            invoice.SupplierInvoiceItems.forEach((item) => {
                var veh = item.SupplierInvoiceItemVehicles.find(d => d.RichbitFileNumber == this.RichbitFileNumber && d.VehicleChassisNumber == this.VehicleChassisNumber);
                if (veh) exist = true;
            });
            if (!exist) {
                var i = this.parent.parent.addedVehicles.findIndex(d => d.RichbitFileNumber == this.RichbitFileNumber && d.VehicleChassisNumber == this.VehicleChassisNumber);
                if (!AppTool.IsNullOrEmpty(i)) this.parent.parent.addedVehicles.splice(i, 1);
            }
            ////Commentd=> moved to the server by mohammad
            //this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(this.VehicleChassisNumber, this.RichbitFileNumber).subscribe(response => {
            //    if (response) {
            //        if (!response.HasError) {
            //            var vehicle: VehiclePM = response.Result;
            //            if (vehicle) {
            //                vehicle.DeclarationId = null;

            //                this.vehiclePMService.update(vehicle).subscribe(response => {
            //                });
            //            } 
            //        }
            //    }

            //});
        }
        

    }

    OnRichbitFileNumbernLostFocus(logCellTemplate: any, RichbitFileNumbernTextBox: any) {

        if (this.RichbitFileNumber) {
            if (!this.vehicleSelected)
                this.SetChassisNumber(logCellTemplate, RichbitFileNumbernTextBox);
        }
        else { // enable all
            //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            this.vehicleSelected = false;
            this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true);
            this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true);
        }

    }

    OnVehicleChassisNumberLostFocus(logCellTemplate: any, VehicleChassisNumberTextBox: any) {

        if (this.VehicleChassisNumber) {
            if (!this.vehicleSelected)
                this.SetFileNumber(logCellTemplate, VehicleChassisNumberTextBox);
        }
        else { // enable all
            //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            this.vehicleSelected = false;

            this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true);
            this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true);
        }

    }

}

export class VehicleValidationError {
    ValidationText: string;
    DeclarationId: string;
    CustomFileNumber: string;
    RichbitFileNumber: string;
    InvoiceCounterKey: number;
    InvoiceItemLineNumber: number;
    IsVehicle: boolean = false;
}
