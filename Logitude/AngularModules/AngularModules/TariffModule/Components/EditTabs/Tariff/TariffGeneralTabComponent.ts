import { Component, OnInit, OnDestroy  } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { PortPM } from '../../../../Common/EntityPMs/PortPM';
import { TariffDomainService } from '../../../../TariffModule/Services/TariffDomainService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './TariffGeneralTabComponent.html',
})

export class TariffGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public DataContext = this;
    public TariffsLinesSource: ObservableCollection;
    private EntityArgs: EntityArgs;
    public IsResourcesReady: boolean = false;
    private TariffDomainService: TariffDomainService;

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityArgs = entityArgs;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadTariffLines();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadTariffLines();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
    ngOnInit() {
        this.Intialize();  
    }

    Intialize() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            this.IsResourcesReady = true;
            this.TariffsLinesSource = new ObservableCollection([]);
            this.EntityPM = this.EntityArgs.EntityPM;
            this.TariffDomainService = new TariffDomainService();
            this.SetStepsLabelsAndVisibility();
            this.LoadTariffLines();
        });
    }

    LoadTariffLines() {
        this.TariffsLinesSource.Clear();
        var itemsCollection: TariffLineData[] = []; 
        this.EntityPM.TariffLines.forEach(item => {
            itemsCollection.push(new TariffLineData(item,this));
        });
        this.TariffsLinesSource.InsertCollection(itemsCollection);
    }

    get PriceSteps() {
        return this.EntityPM.PriceSteps;
    }
    set PriceSteps(value: string) {
        if (this.EntityPM.PriceSteps != value) {
            this.EntityPM.PriceSteps = value;
        }
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get StartDate() {
        return this.EntityPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.EntityPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }

    get Description() {
        return this.EntityPM.Description;
    }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    get SellerId() {
        return this.EntityPM.SellerId;
    }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;
        }
    }

    get ContractNumber() {
        return this.EntityPM.ContractNumber;
    }
    set ContractNumber(value: number) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }

    public Step1PriceLabel: string;
    public Step2PriceLabel: string;
    public Step3PriceLabel: string;
    public Step4PriceLabel: string;
    public Step5PriceLabel: string;
    public Step6PriceLabel: string;
    public Step7PriceLabel: string;
    public Step8PriceLabel: string;

    public Step1PriceVisibility: boolean;
    public Step2PriceVisibility: boolean;
    public Step3PriceVisibility: boolean;
    public Step4PriceVisibility: boolean;
    public Step5PriceVisibility: boolean;
    public Step6PriceVisibility: boolean;
    public Step7PriceVisibility: boolean;
    public Step8PriceVisibility: boolean;

    SetStepsLabelsAndVisibility() {
        if (!AppTool.IsNullOrEmpty(this.PriceSteps)) {
            if (this.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = [] = this.PriceSteps.split(",");
                var count = steps.length;
                if (count == 1) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step1PriceVisibility = true;
                }
                else if (count == 2) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                }
                else if (count == 3) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                }
                else if (count == 4) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                }
                else if (count == 5) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                }
                else if (count == 6) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                }
                else if (count == 7) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                }
                else if (count == 8) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step8PriceLabel = steps[7] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                    this.Step8PriceVisibility = true;
                }
            }
            else {
                this.Step1PriceLabel = this.PriceSteps;
                this.Step1PriceVisibility = true;
            }
        }
    }

    AddTariffLine() {
        var logWindow = new LogitudeWindow();
        var itemPM = new TariffLinePM(null);
        itemPM.StartDate = this.StartDate;
        itemPM.ExpirationDate = this.ExpirationDate;
        itemPM.Tenant = SessionLocator.Tenant;
        var itemComponent = new TariffLineData(itemPM, this, true);
        logWindow.DataContext = itemComponent;
        logWindow.Title = "New Tariff Line"; 
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: TariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.DataContext = item;
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    DeleteTariffButtonClicked(item: TariffLineData) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Tariff Line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.RemoveTariffLine(item.EntityPM);
                this.TariffsLinesSource.Remove(item);
                this.LoadTariffLines();
            }
        });
    }

    UploadExcel() {

    }

    DownloadExcel() {
        this.TariffDomainService.DownloadTariffLines(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {           
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "Tariffs" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
                {
                    window.open(url);
                }
            }
        });
    }
}

export class TariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: TariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;

    constructor(entity: TariffLinePM, public FatherComponent: TariffGeneralTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
    }

    get MinPrice() {
        return this.EntityPM.MinPrice;
    }
    set MinPrice(value: number) {
        if (this.EntityPM.MinPrice != value) {
            this.EntityPM.MinPrice = value;
        }
    }

    get DestinationPortId() {
        return this.EntityPM.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.EntityPM.DestinationPortId != value) {
            this.EntityPM.DestinationPortId = value;
            this.SetUIProperties();
        }
    }

    get DestinationPortCode() {
        return this.EntityPM.DestinationPortCode;
    }
    set DestinationPortCode(value: string) {
        if (this.EntityPM.DestinationPortCode != value) {
            this.EntityPM.DestinationPortCode = value;
        }
    }

    destinationPort: PortPM;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortPM) {
        if (this.destinationPort != value) {
            this.destinationPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DestinationPortCode = value.Code;
        } else {
            this.DestinationPortCode = null;
        }
    }

    get OriginPortCode() {
        return this.EntityPM.OriginPortCode;
    }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    originPort: PortPM;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortPM) {
        if (this.originPort != value) {
            this.originPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginPortCode = value.Code;
        } else {
            this.OriginPortCode = null;
        }
    }

    get OriginPortId() {
        return this.EntityPM.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.EntityPM.OriginPortId != value) {
            this.EntityPM.OriginPortId = value;
            this.SetUIProperties();
        }
    }

    get Step1Price() {
        return this.EntityPM.Step1Price;
    }
    set Step1Price(value: number) {
        if (this.EntityPM.Step1Price != value) {
            this.EntityPM.Step1Price = value;
        }
    }

    get Step2Price() {
        return this.EntityPM.Step2Price;
    }
    set Step2Price(value: number) {
        if (this.EntityPM.Step2Price != value) {
            this.EntityPM.Step2Price = value;
        }
    }

    get Step3Price() {
        return this.EntityPM.Step3Price;
    }
    set Step3Price(value: number) {
        if (this.EntityPM.Step3Price != value) {
            this.EntityPM.Step3Price = value;
        }
    }

    get Step4Price() {
        return this.EntityPM.Step4Price;
    }
    set Step4Price(value: number) {
        if (this.EntityPM.Step4Price != value) {
            this.EntityPM.Step4Price = value;
        }
    }

    get Step5Price() {
        return this.EntityPM.Step5Price;
    }
    set Step5Price(value: number) {
        if (this.EntityPM.Step5Price != value) {
            this.EntityPM.Step5Price = value;
        }
    }

    get Step6Price() {
        return this.EntityPM.Step6Price;
    }
    set Step6Price(value: number) {
        if (this.EntityPM.Step6Price != value) {
            this.EntityPM.Step6Price = value;
        }
    }

    get Step7Price() {
        return this.EntityPM.Step7Price;
    }
    set Step7Price(value: number) {
        if (this.EntityPM.Step7Price != value) {
            this.EntityPM.Step7Price = value;
        }
    }

    get Step8Price() {
        return this.EntityPM.Step8Price;
    }
    set Step8Price(value: number) {
        if (this.EntityPM.Step8Price != value) {
            this.EntityPM.Step8Price = value;
        }
    }
}


