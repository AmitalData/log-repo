
import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {FBLStockPM} from '../../../../Shipment/EntityPMs/FBLStockPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {FBLStockExtenedPMService} from '../../../../Shipment/Services/ExtendedPMs/FBLStockExtenedPMService';

@Component({
    moduleId: module.id,
    templateUrl: './FBLStockMainComponent.html',
})

export class FBLStockMainComponent {
    
    public ObjectTableName: string = "FBLStock";
    public ItemsCount: number = 0;
    public ItemsSource: FBLStockPM[] = [];
    public IsVisibile: boolean = false;
    private FBLStockExtenedPMService: FBLStockExtenedPMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("FBLStock", 0).subscribe(response => {
            this.IsVisibile = true;
            //this.EntityPM = entityArgs.EntityPM;
            this.FBLStockExtenedPMService = new FBLStockExtenedPMService();
            this.LoadData();
        });
    }

    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.SelectedItem = null;

        this.FBLStockExtenedPMService.GetAllFBLStockPMsByTenant(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BuildItemsSource(myResponse.Result);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: FBLStockPM[]) {
        var itemsCount = 0;
        var itemsSource: FBLStockPM[] = [];

        if (items != null) {
            itemsCount = items.length;
            itemsSource = items;
        }

        this.ItemsCount = itemsCount;
        this.ItemsSource = itemsSource;
    }

    // Commands
    public SelectedItem: FBLStockPM = null;
    get IsAddEnabled() {
        var myResult = true;

        //if (this.EntityPM.CheckDigit) {
        //    myResult = true;
        //}

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
        logWindow.Title = TextCodeTranslator.Translate("FBLStock.O.NewFBLNumbers");
        logWindow.WindowArgs = { FBLStocksList: this.ItemsSource };
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/NewFBLStockComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    //AssignClicked() {
    //    var logWindow = new LogitudeWindow();
    //    logWindow.Title = "Assign to shipper";
    //    logWindow.WindowArgs = { AirlineId: this.EntityPM.Id, IsCustomerMode: false };
    //    logWindow.Show('./Common/Components/Partners/AWBStock/AssignComponent');
    //    logWindow.WindowClosed.subscribe(s => {
    //        if (s == "OK") {
    //            this.LoadData();
    //        }
    //    });
    //}

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
                win.Show("This stock does not have an insertion date");
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = TextCodeTranslator.Translate("FBLStock.O.RemovingStock");
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Delete");
                confirmWindow.NoButtonText = "Cancel";

                var message = TextCodeTranslator.Translate("FBLStock.M.DeleteStockNumber");

                if (isDeletingSeries) {
                    var myGetDateFormats = DateTool.GetDateFormats(this.SelectedItem.InsertionDate);
                    message = TextCodeTranslator.Translate("FBLStock.M.DeleteStockSeries");
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";
                }

                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                }

                confirmWindow.Show(message);

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {

                        this.CurrentSession.StartBusyIndicatorSaving();

                        this.FBLStockExtenedPMService.DeleteFBLStocksOperation(this.SelectedItem.Id, isDeletingSeries).subscribe((myResponse: ServiceResponse) => {

                            this.CurrentSession.StopBusyIndicator();

                            if (myResponse != null) {
                                if (myResponse.HasError) {
                                    var win = new MessageWindow();
                                    win.Show("Remove Stock Failed: " + myResponse.ErrorsArray[0]);
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

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
