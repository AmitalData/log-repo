import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {MAWBStackPM} from '../../../../Common/EntityPMs/MAWBStackPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AWBStackDomainService} from '../../../../Common/Services/AWBStackDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './AirlineAWBStockTabComponent.html',
})

export class AirlineAWBStockTabComponent {
    public EntityPM: AirlinePM;
    public ObjectTableName: string = "Airline";
    public ItemsCount: number = 0;
    public ItemsSource: MAWBStackPM[] = [];
    public IsVisibile: boolean = false;
    private StackDomainService: AWBStackDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("MAWBStack", 0).subscribe(response=> {
            this.IsVisibile = true;
            this.EntityPM = entityArgs.EntityPM;
            this.StackDomainService = new AWBStackDomainService();
            this.LoadData();
        });
    }

    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.SelectedItem = null;

        this.StackDomainService.GetMAWBStackPMsByAirlineId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BuildItemsSource(myResponse.Result);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: MAWBStackPM[]) {
        var itemsCount = 0;
        var itemsSource: MAWBStackPM[] = [];

        if (items != null) {
            itemsCount = items.length;
            itemsSource = items;
        }

        this.ItemsCount = itemsCount;
        this.ItemsSource = itemsSource;
    }

    // Commands
    public SelectedItem: MAWBStackPM = null;
    get IsAddEnabled() {
        var myResult = false;

        if (this.EntityPM.CheckDigit) {
            myResult = true;
        }

        return myResult;
    }
    get IsRemoveEnabled() {
        var myResult = false;

        if (this.SelectedItem != null) {
            myResult = true;
        }

        return myResult;
    }

    AddStockClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("MAWBStack.O.NewAirWayBillNumbers");
        logWindow.WindowArgs = { AirlineId: this.EntityPM.Id, IsCustomerMode: false, AirlineStacksList: this.ItemsSource };
        logWindow.Show('./Common/Components/Partners/AWBStock/NewStackComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    AssignClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Assign to shipper";
        logWindow.WindowArgs = { AirlineId: this.EntityPM.Id, IsCustomerMode: false };
        logWindow.Show('./Common/Components/Partners/AWBStock/AssignComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    RemoveClicked() {
        this.Remove(false);
    }
    RemoveSeriesClicked() {
        this.Remove(true);
    }
    Remove(isDeletingSeries: boolean) {

        if (this.SelectedItem != null) {

            if (isDeletingSeries == true && this.SelectedItem.InsertionDate == null) {
                var win = new MessageWindow();
                win.Show("This stack does not have an insertion date");
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = TextCodeTranslator.Translate("MAWBStack.O.RemovingStack");
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Delete");
                confirmWindow.NoButtonText = "Cancel";

                var message = TextCodeTranslator.Translate("MAWBStack.M.DeleteStackNumber");

                if (isDeletingSeries) {
                    var myGetDateFormats = DateTool.GetDateFormats(this.SelectedItem.InsertionDate);
                    message = TextCodeTranslator.Translate("MAWBStack.M.DeleteStackSeries");
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";                    
                }

                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                }

                confirmWindow.Show(message);

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {

                        this.CurrentSession.StartBusyIndicatorSaving();

                        this.StackDomainService.DeleteMAWBStacksOperation(this.SelectedItem.Id, this.SelectedItem.AirlineId, isDeletingSeries).subscribe((myResponse: ServiceResponse) => {

                            this.CurrentSession.StopBusyIndicator();

                            if (myResponse != null) {
                                if (myResponse.HasError) {
                                    var win = new MessageWindow();
                                    win.Show("Remove Stack Failed: " + myResponse.ErrorsArray[0]);
                                }

                                else {
                                    this.LoadData();
                                }
                            }
                        });
                    }
                });
            }
        }
    }
}
