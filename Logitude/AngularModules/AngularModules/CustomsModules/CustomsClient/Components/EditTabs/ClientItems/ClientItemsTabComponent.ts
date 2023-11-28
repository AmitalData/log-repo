import { Component, EventEmitter, Output } from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ClientMessagesService } from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { INF_MSG_GenericResponseData } from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { ClaimGeneralTabComponent } from '../../../../CustomsClaim/Components/EditTabs/General/ClaimGeneralTabComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { ClientItemListService } from 'Customs/Services/StandardLists/ClientItemListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { forEach } from 'cypress/types/lodash';
import { ClientItemPM } from 'Customs/EntityPMs/ClientItemPM';
import { AppTool } from 'Infrastructure/Tools';

@Component({

    templateUrl: './ClientItemsTabComponent.html',
})

export class ClientItemsTabComponent extends BaseComponent {

    ClientItemsL
    @Output() ClientItemsList: EventEmitter<any> = new EventEmitter();


    objectTableName: string = "Customs.ClientItem";
    entityPM: ClientPM;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    responseData: INF_MSG_GenericResponseData;
    public load = false
    SequenceNumeric: string = "מס'";
    Parent: ClaimGeneralTabComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    private myService: ClientItemListService = new ClientItemListService();;
    public ItemsList: ObservableCollection;
    public filters = new ApiQueryFilters();
    constructor(private _EntityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.ClientItem").subscribe((response: any) => {


        });
        // this.ItemsList = new ObservableCollection([]);



    }
    LoadDate() {

        this.ItemsList = new ObservableCollection([]);


        this.filters.PageIndex = 0;
        this.filters.PageSize = 100;

        this.filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        this.filters.addAdditionalFilter("ClientCode", this.entityPM.Code, null, null, "Contains", false, false, false, "string");


        this.myService.getByFilters(this.filters).subscribe((myResponse: ServiceResponse) => {

            myResponse?.Result.forEach(element => {

                this.ItemsList.Insert(new ClientItemModel(element, this))

            });
            this.ItemsList.Collection.forEach((obj, index) => obj.SequenceNumeric = index + 1)
            this.originalItemList.InsertCollection(this.ItemsList.Collection);
            this.load = true
        })


    }
    InitTab(EntityPM: ClientPM) {

        this.entityPM = EntityPM;
        this.LoadDate();
    }

    SetWindowArgs(args: any) {
        if (args != null) {

            this.entityPM = args.EntityPM;
            this.Parent = args.Parent;
        }
    }


    public operationType: any;

    ReloadEntityPM() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }
    originalItemList: ObservableCollection = new ObservableCollection([]);
    public SearchFilterChangedEvent: any;

    TextChanged(searchText) {
        
        if (AppTool.IsNullOrEmpty(searchText))
            this.LoadDate();
        else {
            var items: any = this.originalItemList;
            items = items.Collection.filter(f => f.ClassificationCode != null || f.ItemCode != null);
            var TempItemList: ClientItemPM[] = [];
            TempItemList = items;
            TempItemList = TempItemList.filter(f => f.ClassificationCode?.toUpperCase().includes(searchText?.toUpperCase().toString()) || f.ItemCode?.toUpperCase().includes(searchText?.toUpperCase().toString()));

            this.ItemsList.InsertCollection(TempItemList);
           
        }

    }





}
export class ClientItemModel extends BaseComponent {
    public ClientItemPM: ClientItemPM = null;

    public ObjectTableName = "Customs.ClientItem";
    public DataContext = this;

    constructor(private clientItemPM: ClientItemPM, private parent: ClientItemsTabComponent) {
        super();
        this.ClientItemPM = clientItemPM;
    }

    //#region Properties

    get ItemCode() { return this.ClientItemPM.ItemCode; }
    set ItemCode(value: string) {
        if (this.ClientItemPM.ItemCode != value) {
            this.ClientItemPM.ItemCode = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }

    get ClassificationCode() { return this.ClientItemPM.ClassificationCode; }
    set ClassificationCode(value: string) {
        if (this.ClientItemPM.ClassificationCode != value) {
            this.ClientItemPM.ClassificationCode = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }

    get ClientCode() { return this.ClientItemPM.ClientCode; }
    set ClientCode(value: string) {
        if (this.ClientItemPM.ClientCode != value) {
            this.ClientItemPM.ClientCode = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }

    get OriginCountryName() { return this.ClientItemPM.OriginCountryName; }
    set OriginCountryName(value: string) {
        if (this.ClientItemPM.OriginCountryName != value) {
            this.ClientItemPM.OriginCountryName = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }
    get OriginCountryCode() { return this.ClientItemPM.OriginCountryCode; }
    set OriginCountryCode(value: string) {
        if (this.ClientItemPM.OriginCountryCode != value) {
            this.ClientItemPM.OriginCountryCode = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }
    get ItemDescription() { return this.ClientItemPM.ItemDescription; }
    set ItemDescription(value: string) {
        if (this.ClientItemPM.ItemDescription != value) {
            this.ClientItemPM.ItemDescription = value;
            this.parent.ClientItemsList.emit(this.parent.ItemsList.Collection)
        }
    }


    //#endregion


    SetLocalName(entity, fieldName) {

        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

}


