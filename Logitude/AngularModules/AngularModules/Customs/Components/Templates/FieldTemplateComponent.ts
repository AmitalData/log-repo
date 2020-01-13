import {Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CourierMasterService} from '../../Services/Others/CourierMasterService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public IsHeaderScreenTemplate: boolean = false;
    courierMasterService: CourierMasterService = new CourierMasterService();
    @ViewChild('SpotLight', { read: ViewContainerRef }) SpotLightViewContainerRef: ViewContainerRef;
    constructor() {

    }

    public Run(args: any) {
         this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
        }

        if (this.IsSpotLightTemplate) {
            this.RunComponent();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.SpotLightViewContainerRef) {
            this.SpotLightViewContainerRef.clear();

            var myComponentPath = "./Customs/Components/Spotlight/CustomsSpotlightComponent";
            SessionLocator.DynamicLoader.Load(myComponentPath, this.SpotLightViewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.Run(this.Entity.Id);
                });
        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    OpenCourierMaster() {
        //static entityResourceService: EntityResourceService = new EntityResourceService();

        //entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(response => {
            //entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(response => {
            this.courierMasterService.getCourierMasterByDeclarationId(this.Entity.Id).subscribe((response: ServiceResponse) => {
                if (response) {
                    if (!response.HasError) {
                        //this.EditEntity("Customs.CourierMaster", response.Result.Id, null, "COGN");
                        var windowArgs: any = {};
                        windowArgs.CurrentEntity = response.Result;
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1500;
                        logWindow.Height = 1000;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.ShowCloseButton = true;
                        //logWindow.IsHideHeader = true;
                        logWindow.IsFillScreen = true;
                        //AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
                        logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                        logWindow.WindowClosed.subscribe(($event1: any) => {
                            //this.ShowCourierMasterByIdReturnCloseSaveCallBack(isSaved);
                        });

                    }
                }
                });
            //});
        //});

    }



    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(res => {


        });

    }

}
