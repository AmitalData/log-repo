import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GetStackWindowArgs} from '../../../Args';
import {AWBStackDomainService} from '../../../Services/AWBStackDomainService';
import {MAWBStackPM} from '../../../EntityPMs/MAWBStackPM';
import {CardList} from '../../../EntityLists/CardList';
import {CardListService} from '../../../Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';

@Component({    
    moduleId: module.id,
    templateUrl: './StackSelectionComponent.html',
})

export class StackSelectionComponent {
    public ShipperId: string = null;
    public AirlineId: string = null;
    public AirlineName: string = null;
    private StackDomainService: AWBStackDomainService;
    public ItemsCount: number = 0;
    public ItemsSource: MAWBStackPM[];
    public SelectedItem: MAWBStackPM = null;
    public ObjectTableName: string = "MAWBStack";
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.ItemsSource = [];
        this.StackDomainService = new AWBStackDomainService();
        this.CurrentSession.StartBusyIndicator("Loading Air Waybill Numbers");
    }

    private args: GetStackWindowArgs;
    SetWindowArgs(args: GetStackWindowArgs) {
        this.args = args;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;

            if (!AppTool.IsNullOrEmpty(args.ShipperId)) {
                this.ShipperId = args.ShipperId;
            }

            if (!AppTool.IsNullOrEmpty(args.CardId)) {
                this.AirlineId = args.CardId;

                var myCardService = new CardListService;
                myCardService.getSingle(args.CardId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var list: CardList = myResponse.Result;
                            if (list != null) {
                                this.AirlineName = list.EnglishName;
                            }
                        }
                    }
                });
            }

            this.Load();
        });
    }

    private Load() {
        this.ItemsCount = 0;
        this.ItemsSource = [];

        if (this.AirlineId != null) {
            this.StackDomainService.GetMAWBStackPMsByAirlineIdAndShipperId(this.AirlineId, this.ShipperId, 100, 0).subscribe((myResult:any) => {
                var data: MAWBStackPM[] = myResult;
                this.ItemsSource = data.sort((a, b) => { return a.Number - b.Number });
                this.CurrentSession.StopBusyIndicator();
            });

            this.StackDomainService.GetMAWBStackPMsCountByAirlineIdAndShipperId(this.AirlineId, this.ShipperId).subscribe((myResult:any) => {
                this.ItemsCount = myResult;
            });

            this.StackDomainService.GetCardHasAssignedMawbStacks(this.AirlineId, this.ShipperId).subscribe((myResult:any) => {
                this.args.HasAssignedStocks = myResult;
            });            
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (this.SelectedItem != null) {
            this.args.SelectedStack = this.SelectedItem;

            if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                if (this.args.SelectedStack.AssignedToId != this.ShipperId) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Title = "Stock Number";
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.Show("The MAWB number that you selected is not assigned to this shipper. Continue anyway?");
                    confirmWindow.WindowClosed.subscribe(c => {
                        if (confirmWindow.Yes) {
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    });
                }

                else {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            }

            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }

        //if (this.SelectedItem != null) {
        //    this.args.SelectedStack = this.SelectedItem;

        //    if (this.args.SelectedStack.AssignedToId == null && this.args.HasAssignedStocks) {
        //        var confirmWindow = new ConfirmWindow();
        //        confirmWindow.Title = "Stock Number";
        //        confirmWindow.Width = 450;
        //        confirmWindow.Height = 190;
        //        confirmWindow.Show("The MAWB number that you selected is not assigned to this shipper. Continue anyway?");
        //        confirmWindow.WindowClosed.subscribe(c => {
        //            if (confirmWindow.Yes) {
        //                this.CurrentSession.CloseCurrentWindowEmit("OK");
        //            }
        //        });
        //    }

        //    else {
        //        this.CurrentSession.CloseCurrentWindowEmit("OK");
        //    }
        //}
    }
}
