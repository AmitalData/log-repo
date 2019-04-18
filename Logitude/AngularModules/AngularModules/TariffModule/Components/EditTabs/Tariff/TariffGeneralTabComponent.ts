import { Component, ViewChild, ViewContainerRef,  } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';

@Component({
    moduleId: module.id,
    templateUrl: './TariffGeneralTabComponent.html',
})

export class TariffGeneralTabComponent extends BaseComponent {
    public EntityPM: TariffPM = new TariffPM();
    public ObjectTableName: string = "Tariff";
    public DataContext = this;
    public TariffsLinesSource: ObservableCollection;
    private EntityArgs: EntityArgs;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityArgs = entityArgs;
        this.Intialize();  
    }

    Intialize() {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.EntityPM = this.EntityArgs.EntityPM;
        this.LoadTariffLines(); 
    } 

    LoadTariffLines() {
        this.TariffsLinesSource.Clear();
        var itemsCollection: TariffLineData[] = []; 
        this.EntityPM.TariffLines.forEach(item => {
            itemsCollection.push(new TariffLineData(item));
        });
        this.TariffsLinesSource.InsertCollection(itemsCollection);
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


    AddTariffLine() {


    }
    UploadExcel() {

    }
    DownloadExcel() {
        

    }

}

export class TariffLineData {
    public TariffLine: TariffLinePM;

    constructor(entity: TariffLinePM) {
        this.TariffLine = entity;
    }

    get MinPrice() {
        return this.TariffLine.MinPrice;
    }
    set MinPrice(value: number) {
        if (this.TariffLine.MinPrice != value) {
            this.TariffLine.MinPrice = value;
        }
    }

    get DestinationPortId() {
        return this.TariffLine.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.TariffLine.DestinationPortId != value) {
            this.TariffLine.DestinationPortId = value;
        }
    }

    get OriginPortId() {
        return this.TariffLine.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.TariffLine.OriginPortId != value) {
            this.TariffLine.OriginPortId = value;
        }
    }

    get Step1Price() {
        return this.TariffLine.Step1Price;
    }
    set Step1Price(value: number) {
        if (this.TariffLine.Step1Price != value) {
            this.TariffLine.Step1Price = value;
        }
    }

    get Step2Price() {
        return this.TariffLine.Step2Price;
    }
    set Step2Price(value: number) {
        if (this.TariffLine.Step2Price != value) {
            this.TariffLine.Step2Price = value;
        }
    }

    get Step3Price() {
        return this.TariffLine.Step3Price;
    }
    set Step3Price(value: number) {
        if (this.TariffLine.Step3Price != value) {
            this.TariffLine.Step3Price = value;
        }
    }

    get Step4Price() {
        return this.TariffLine.Step4Price;
    }
    set Step4Price(value: number) {
        if (this.TariffLine.Step4Price != value) {
            this.TariffLine.Step4Price = value;
        }
    }

    get Step5Price() {
        return this.TariffLine.Step5Price;
    }
    set Step5Price(value: number) {
        if (this.TariffLine.Step5Price != value) {
            this.TariffLine.Step5Price = value;
        }
    }

    get Step6Price() {
        return this.TariffLine.Step6Price;
    }
    set Step6Price(value: number) {
        if (this.TariffLine.Step6Price != value) {
            this.TariffLine.Step6Price = value;
        }
    }

    get Step7Price() {
        return this.TariffLine.Step7Price;
    }
    set Step7Price(value: number) {
        if (this.TariffLine.Step7Price != value) {
            this.TariffLine.Step7Price = value;
        }
    }

    get Step8Price() {
        return this.TariffLine.Step8Price;
    }
    set Step8Price(value: number) {
        if (this.TariffLine.Step8Price != value) {
            this.TariffLine.Step8Price = value;
        }
    }


}


