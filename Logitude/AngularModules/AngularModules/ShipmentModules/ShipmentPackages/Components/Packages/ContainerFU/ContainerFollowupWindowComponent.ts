import {Component, OnDestroy, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {PackagesTabComponent, ShipmentPackageItem} from './../PackagesTabComponent';
import {ContainerFollowupWindowTemplate} from './ContainerFollowupWindowTemplate';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {RoutingHelper} from '../../../../../Shipment/Tools';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ServiceLocator} from '../../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ContainerFollowupWindowComponent.html',
})

export class ContainerFollowupWindowComponent implements OnDestroy {
    public Code: string;
    public EntityPM: ShipmentPackagePM;
    public DataContext: ShipmentPackageItem;
    public TabComponent: PackagesTabComponent;
    public TemplateComponent: ContainerFollowupWindowTemplate;
    public ValidationErrorsList: string[];
    public IsNewFollowup: boolean;
    private IsNewFollowup_Totango: boolean;
    @ViewChild('Child', { read: ViewContainerRef }) ChildViewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.Code = args['Code'];
        this.DataContext = args['DataContext'];
        this.EntityPM = this.DataContext.EntityPM;
        this.TabComponent = this.DataContext.fatherComponent;
        this.IsNewFollowup = args['IsNewFollowup'];
        this.IsNewFollowup_Totango = args['IsNewFollowup'];
        this.CheckMultiConnected();

        this.Listen();
        this.RunComponent();
    }

    public IsDeliveryConnectedWithMultiContainers: boolean = false;
    CheckMultiConnected() {

        var isDeliveryConnectedWithMultiContainers = false;

        if (this.Code == "D") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.DeliveryId);
                if (allPackages.length > 1) {
                    isDeliveryConnectedWithMultiContainers = true;
                }
            }
        }

        this.IsDeliveryConnectedWithMultiContainers = isDeliveryConnectedWithMultiContainers;
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

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    LoadTemplate() {
        if (this.ChildViewContainerRef) {
            this.ChildViewContainerRef.clear();

            var myComponentPath = './ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWindowTemplate';

            SessionLocator.DynamicLoader.Load(myComponentPath, this.ChildViewContainerRef)
                .then(cmpRef => {
                    this.TemplateComponent = cmpRef.instance;
                    cmpRef.instance.Run({ Code: this.Code, DataContext: this.DataContext, FatherComponent: this });                    
                    this.Clone();

                    if (this.IsNewFollowup) {
                        this.IsNewFollowup = false;
                        this.TemplateComponent.IsFollowup = true;
                        this.TemplateComponent.TransportModeCode = "BYTR";
                    }
                });
        }
    }

    private SaveCompletedEvent: any = null;
    private SaveActionCompletedEvent: any = null;
    Listen() {
        this.SaveCompletedEvent = this.TabComponent.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                if (this.myRequestedCommand == "ActionLink" || this.myRequestedCommand == "DeleteLink") {
                    this.TabComponent.BuildItemsSource();
                    this.DataContext = this.TabComponent.ItemsSource.Collection.filter(f => f.EntityPM.Id == this.EntityPM.Id)[0];
                    this.EntityPM = this.DataContext.EntityPM;
                    this.CheckMultiConnected();
                    this.LoadTemplate();
                }
            }
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.SaveActionCompletedEvent);
        this.SaveCompletedEvent = null;
        this.SaveActionCompletedEvent = null;
    }

    private myRequestedCommand: string;
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var isValid: boolean = this.Validate();

        if (isValid) {
            this.Save("Ok");
        }
    }
    Validate() {
        var errors: string[] = [];

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.DataContext.ShipmentPM, this.EntityPM, errors, this.Code);

        // Actual Dates
        switch (this.Code) {
            case "D": {

                if (!DateTool.IsActualDateValid(this.EntityPM.DeliveryATD)) {
                    errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD")));
                }

                if (!DateTool.IsActualDateValid(this.EntityPM.DeliveryATA)) {
                    errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA")));
                }

                break;
            }

            case "R": {

                if (!DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATD)) {
                    errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD")));
                }

                if (!DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATA)) {
                    errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA")));
                }

                break;
            }
        }

        this.ValidationErrorsList = errors;

        return errors.length == 0 ? true : false;
    }
    Save(myCommand: string) {

        this.myRequestedCommand = myCommand;

        if (!this.SaveActionCompletedEvent) {
            this.SaveActionCompletedEvent = this.TabComponent.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    if (this.IsNewFollowup_Totango) {
                        ServiceLocator.SendTotangoUserActivity("Container F/U", "Added Container F/U");
                    }

                    this.TabComponent.BuildItemsSource();
                    this.DataContext = this.TabComponent.ItemsSource.Collection.filter(f => f.EntityPM.Id == this.EntityPM.Id)[0];
                    this.EntityPM = this.DataContext.EntityPM;
                    this.CheckMultiConnected();
                    this.OnSaveCompleted();
                }

                else {
                    this.ValidationErrorsList = this.DataContext.fatherComponent.entityArgs.EditComponent.ValidationErrorsList;
                }

                AppTool.KillEventEmitter(this.SaveActionCompletedEvent);
                this.SaveActionCompletedEvent = null;
            });

            this.TabComponent.entityArgs.EditComponent.SaveChanges();
        }
    }
    OnSaveCompleted() {
        switch (this.myRequestedCommand) {

            case "Ok": {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
                break;
            }

            case "ActionLink": {
                switch (this.Code) {
                    case "D": {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                            this.DataContext.AddRouting(this.Code);
                        }

                        else {
                            this.DataContext.EditRouting(this.Code);
                        }

                        break;
                    }

                    case "R": {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId)) {
                            this.DataContext.AddRouting(this.Code);
                        }

                        else {
                            this.DataContext.EditRouting(this.Code);
                        }

                        break;
                    }
                }

                break;
            }

            case "DeleteLink": {
                //this.DataContext.SetUIProperties_ContainerFU();
                break;
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.TemplateComponent);
        this.myCloner.AddField('IsFollowup');
        this.myCloner.AddField('ETD');
        this.myCloner.AddField('ATD');
        this.myCloner.AddField('ETA');
        this.myCloner.AddField('ATA');
        this.myCloner.AddField('From');
        this.myCloner.AddField('To');
        this.myCloner.AddField('TransportModeCode');        
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
