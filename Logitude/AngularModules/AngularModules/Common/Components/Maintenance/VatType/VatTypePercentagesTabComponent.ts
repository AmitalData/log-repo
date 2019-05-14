import {Component, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {VatTypePM} from '../../../EntityPMs/VatTypePM';
import {VatTypePercentagePM} from '../../../EntityPMs/VatTypePercentagePM';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './VatTypePercentagesTabComponent.html',
})

export class VatTypePercentagesTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: VatTypePM;
    public DataContext = this;
    public IsNewEntity: boolean = true;
    public ObjectTableName: string = "VatType";
    public ItemsSource: VatTypePercentagePM[] = [];
    public IsResourcesReady: boolean = false;
    get IsMultiPercentage() { return this.EntityPM.IsMultiPercentage; }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public args: EntityArgs, private entityResourceService: EntityResourceService) {
        super();

        this.entityResourceService.getEntityResourceByTableName("VatType").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("VatTypePercentage").subscribe((res2: any) => {
                this.IsResourcesReady = true;

                this.EntityPM = args.EntityPM;

                if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.IsNewEntity = false;
                    this.Listen();
                }

                this.SetUIProperties();
                this.BuildItemsSource();
            });
        });
    }

    private selectedItem: VatTypePercentagePM = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: VatTypePercentagePM) {
        if (value != this.selectedItem) {
            this.selectedItem = value;
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildItemsSource();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildItemsSource();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    SetUIProperties() {

    }

    BuildItemsSource() {
        var items = this.EntityPM.VatTypePercentages;

        items.sort((a, b) => { return (DateTool.GetDateFromDate(a.FromDate) === DateTool.GetDateFromDate(b.FromDate)) ? 0 : (DateTool.GetDateFromDate(a.FromDate) > DateTool.GetDateFromDate(b.FromDate)) ? -1 : 1 });

        this.ItemsSource = items;
        this.SelectedItem = null;
    }

    AddPercentage() {
        var itemPM = new VatTypePercentagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.VatTypeId = this.EntityPM.Id;
        itemPM.FromDate = DateTool.GetCurrentDateTimeAsUtc();

        var title = TextCodeTranslator.Translate("VatTypePercentage.O.AddVatTypePercentage");
        this.RunAddEditWindow(itemPM, title, true);        
    }
    EditPercentage(itemPM: VatTypePercentagePM) {
        var title = TextCodeTranslator.Translate("VatTypePercentage.O.EditVatTypePercentage");
        this.RunAddEditWindow(itemPM, title, false);
    }

    private isWindowOpened: boolean = false;
    RunAddEditWindow(itemPM: VatTypePercentagePM, windowTitle: string, isNewEntity: boolean) {
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;

            var logWindow = new LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = { EntityPM: itemPM, VatTypePM: this.EntityPM, IsNewEntity: isNewEntity };
            logWindow.Show('./Common/Components/Maintenance/VatType/NewVatTypePercentageComponent');

            logWindow.WindowClosed.subscribe(s => {
                this.isWindowOpened = false;
                this.BuildItemsSource();
            });
        }
    }
}
