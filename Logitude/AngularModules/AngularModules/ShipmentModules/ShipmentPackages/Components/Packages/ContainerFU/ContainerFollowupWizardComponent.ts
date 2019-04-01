import {Component, OnDestroy, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ContainerFollowupWizardTemplate} from './ContainerFollowupWizardTemplate';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {ShipmentPMService} from '../../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {ShipmentValidator} from '../../../../../Shipment/Validators/ShipmentValidator';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {RoutingHelper} from '../../../../../Shipment/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ContainerFollowupWizardComponent.html',
})

export class ContainerFollowupWizardComponent extends BaseComponent {
    public ShipmentPM: ShipmentPM;
    public EntityPM: ShipmentPackagePM;
    public EntityId: string;
    public Shipmentd: string;
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public TemplateComponent: ContainerFollowupWizardTemplate;
    @ViewChild('Child', { read: ViewContainerRef }) ChildViewContainerRef: ViewContainerRef; 
    private entityPMService: ShipmentPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.entityPMService = new ShipmentPMService();
    }

    public IsDeliveryConnectedWithMultiContainers: boolean = false;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName("Shipment").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe((res2: any) => {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe((res3: any) => {
                    var entityId: string = args['EntityId'];

                    if (entityId) {

                        this.EntityId = entityId.split(':')[0];
                        this.Shipmentd = entityId.split(':')[1];

                        this.CurrentSession.StartBusyIndicatorLoading();

                        this.entityPMService.get(this.Shipmentd).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {

                                this.ShipmentPM = myResponse.Result;
                                this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityId)[0];

                                if (this.EntityPM) {
                                    if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                                        var allPackages = this.ShipmentPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.DeliveryId);
                                        if (allPackages.length > 1) {
                                            this.IsDeliveryConnectedWithMultiContainers = true;
                                        }
                                    }
                                }

                                this.RunComponent();

                                this.IsResourcesReady = true;
                            }

                            else {
                                this.ValidationErrorsList = myResponse.ErrorsArray;
                            }

                            this.CurrentSession.StopBusyIndicator();
                        });
                    }
                });
            });
        });
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.ChildViewContainerRef) {
            this.isLoaderReady = true;
            this.LoadTemplate();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    LoadTemplate() {
        if (this.ChildViewContainerRef) {
            this.ChildViewContainerRef.clear();

            var myComponentPath = './ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWizardTemplate';

            SessionLocator.DynamicLoader.Load(myComponentPath, this.ChildViewContainerRef)
                .then(cmpRef => {

                    this.TemplateComponent = cmpRef.instance;

                    cmpRef.instance.Run({ FatherComponent: this });                    
                });
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.EntityId);
    }
    SaveClicked() {
        var isValid: boolean = this.Validate();

        if (isValid) {
            this.Save();
        }
    }
    Validate() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "ShipmentPackage", errors);
        Validator.TryValidateObject(this.ShipmentPM, "Shipment", errors);

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.ShipmentPM, this.EntityPM, errors, "D");
        RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.ShipmentPM, this.EntityPM, errors, "R");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.EntityPM.DeliveryATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD")));
        }

        if (!DateTool.IsActualDateValid(this.EntityPM.DeliveryATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA")));
        }

        if (!DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD")));
        }

        if (!DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA")));
        }

        if (errors.length == 0) {
            var myShipmentValidator = new ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors = myShipmentErrors;
                //errors.push(TextCodeTranslator.Translate("Shipment.M.Routings.CantProceedAddingDelivery"));
            }
        }

        this.ValidationErrorsList = errors;

        return errors.length == 0 ? true : false;
    }
    Save(myCommandCode: string = null) {

        if (this.ShipmentPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();

            this.entityPMService.update(this.ShipmentPM).subscribe((myResponse: ServiceResponse) => {

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.ShipmentPM = myResponse.Result;
                    this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityId)[0];
                    this.LoadTemplate();
                    this.OnSaveCompleted(myCommandCode);
                }

                this.CurrentSession.StopBusyIndicator();

            });

        }

        else {
            this.OnSaveCompleted(myCommandCode);
        }
    }
    OnSaveCompleted(myCommandCode: string = null) {
        switch (myCommandCode) {
            case "ActionLink_D": {

                if (AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                    this.AddRouting("D");
                }

                else {
                    this.EditRouting("D");
                }

                break;
            }

            case "ActionLink_R": {

                if (AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId)) {
                    this.AddRouting("R");
                }

                else {
                    this.EditRouting("R");
                }

                break;
            }
        }
    }
    AddRouting(typeCode: string) {

        var myDeliveryIndex = 1;
        var myWindowTitle: string = null;
        var myPickUpDeliveryTypeCode: string = null;
        switch (typeCode) {
            case "R": {
                myPickUpDeliveryTypeCode = "EMPT";

                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.AddEmptyCR");

                if (this.ShipmentPM.ShipmentContainerReturnIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentContainerReturnIndex + 1;
                }

                break;
            }

            default: {
                myPickUpDeliveryTypeCode = "DELV";

                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");

                if (this.ShipmentPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentDeliveryIndex + 1;
                }

                break;
            }
        }

        var newDeliveryPM = new ShipmentDeliveryPM(null);
        newDeliveryPM.FullResponsibility = true;
        newDeliveryPM.Tenant = this.ShipmentPM.Tenant;
        newDeliveryPM.ShipmentId = this.ShipmentPM.Id;
        newDeliveryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        newDeliveryPM.PickUpDeliveryNumber = this.ShipmentPM.ShipmentNumber + "/" + myDeliveryIndex;
        newDeliveryPM.PickUpDeliveryTypeCode = myPickUpDeliveryTypeCode;
        newDeliveryPM.ConnectedPackageId = this.EntityPM.Id;

        switch (typeCode) {
            case "D": {
                newDeliveryPM.ETD = this.EntityPM.DeliveryETD;
                newDeliveryPM.ATD = this.EntityPM.DeliveryATD;
                newDeliveryPM.ETA = this.EntityPM.DeliveryETA;
                newDeliveryPM.ATA = this.EntityPM.DeliveryATA;
                newDeliveryPM.TransportModeCode = this.EntityPM.DeliveryTransportModeCode;
                break;
            }

            case "R": {
                newDeliveryPM.ETD = this.EntityPM.EmptyContainerReturnETD;
                newDeliveryPM.ATD = this.EntityPM.EmptyContainerReturnATD;
                newDeliveryPM.ETA = this.EntityPM.EmptyContainerReturnETA;
                newDeliveryPM.ATA = this.EntityPM.EmptyContainerReturnATA;
                newDeliveryPM.TransportModeCode = this.EntityPM.ECRTransportModeCode;
                break;
            }
        }

        var newDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM(null);
        newDeliveryPackagePM.Tenant = this.EntityPM.Tenant;
        newDeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
        newDeliveryPackagePM.Description = this.EntityPM.Description;
        newDeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
        newDeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
        newDeliveryPackagePM.Quantity = this.EntityPM.Quantity;
        newDeliveryPackagePM.Volume = this.EntityPM.Volume;
        newDeliveryPackagePM.Weight = this.EntityPM.Weight;
        newDeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
        newDeliveryPackagePM.Width = this.EntityPM.Width;
        newDeliveryPackagePM.Height = this.EntityPM.Height;
        newDeliveryPackagePM.Length = this.EntityPM.Length;
        newDeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
        newDeliveryPackagePM.OriginalShipmentPackageId = this.EntityPM.Id;
        newDeliveryPM.AddPackage(newDeliveryPackagePM);

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: newDeliveryPM, IsNewEntity: true, ContainerReturnDeliveryId: this.EntityPM.DeliveryId, IsContainerFollowup: true };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;

        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.ShipmentPM = s;
                this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityId)[0];
                this.LoadTemplate();
            }
        });

        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
    }
    EditRouting(typeCode: string) {

        var myDeliveryId: string = null;
        var myWindowTitle: string = null;
        var myEditedDelivery: ShipmentDeliveryPM = null;

        switch (typeCode) {
            case "R": {
                myDeliveryId = this.EntityPM.EmptyContainerReturnId;
                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                break;
            }

            default: {
                myDeliveryId = this.EntityPM.DeliveryId;
                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                break;
            }
        }

        var myEditedDelivery = this.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == myDeliveryId)[0];

        if (myEditedDelivery) {
            var windowTitle = myWindowTitle + ": " + myEditedDelivery.PickUpDeliveryNumber;

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: myEditedDelivery, IsNewEntity: false, IsContainerFollowup: true };
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;

            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.ShipmentPM = s;
                    this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityId)[0];
                    this.LoadTemplate();
                }
            });

            logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        }
    }
    ViewShipmentClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.ShipmentPM.Id, ObjectTableName: 'Shipment', BackButtonLabel: "Shipment: " + this.ShipmentPM.ShipmentNumber });

                //let isEditComponentSaved = false;

                //cmpRef.instance.BackCompleted.subscribe(bk => {
                //    if (isEditComponentSaved) {
                //        this.entityArgs.EditComponent.ReloadEntityPM();
                //    }
                //});

                //cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                //    if (isSaveSuccess) {
                //        isEditComponentSaved = true;
                //    }
                //});

                //cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                //    if (isSaveSuccess) {
                //        isEditComponentSaved = true;
                //    }
                //});
            });
    }
}
