import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

import {AWBSpecialHandlingCodeList} from '../../../../Shipment/EntityLists/AWBSpecialHandlingCodeList';
import {IATACodeList} from '../../../../Infrastructure/EntityLists/IATACodeList';
import {BookingProductList} from '../../../../Booking/EntityLists/BookingProductList';
import {CommodityList} from '../../../../Common/EntityLists/CommodityList';
import {AirlineMessagingRuleList} from '../../../../Common/EntityLists/AirlineMessagingRuleList';

import {AWBSpecialHandlingCodePM} from '../../../../Shipment/EntityPMs/AWBSpecialHandlingCodePM';
import {IATACodePM} from '../../../../Infrastructure/EntityPMs/IATACodePM';
import {BookingProductPM} from '../../../../Booking/EntityPMs/BookingProductPM';
import {CommodityPM} from '../../../../Common/EntityPMs/CommodityPM';
import {AirlineMessagingRulePM} from '../../../../Common/EntityPMs/AirlineMessagingRulePM';

import {AWBSpecialHandlingCodeListService} from '../../../../Shipment/Services/StandardLists/AWBSpecialHandlingCodeListService';
import {IATACodeListService} from '../../../../Infrastructure/Services/StandardLists/IATACodeListService';
import {BookingProductListService} from '../../../../Booking/Services/StandardLists/BookingProductListService';
import {CommodityListService} from '../../../../Common/Services/StandardLists/CommodityListService';
import {AirlineMessagingRuleListService} from '../../../../Common/Services/StandardLists/AirlineMessagingRuleListService';

import {AWBSpecialHandlingCodePMService} from '../../../../Shipment/Services/StandardPMs/AWBSpecialHandlingCodePMService';
import {IATACodePMService} from '../../../../Infrastructure/Services/StandardPMs/IATACodePMService';
import {BookingProductPMService} from '../../../../Booking/Services/StandardPMs/BookingProductPMService';
import {CommodityPMService} from '../../../../Common/Services/StandardPMs/CommodityPMService';
import {AirlineMessagingRulePMService} from '../../../../Common/Services/StandardPMs/AirlineMessagingRulePMService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './AirlineAdaptationsTabComponent.html',
})

export class AirlineAdaptationsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: AirlinePM;
    public ObjectTableName: string = "Airline";
    public DataContext: AirlineAdaptationsTabComponent = this;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
            this.LoadAllData();
        }
    }

    private SetUIProperties() {
        if (this.IsManagingProduct) {
            this.UIProperties.SetEnabled("IsProductMandatory", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("IsProductMandatory", this.ObjectTableName, false);
            this.IsProductMandatory = false;
        }
    }

    private LoadAllData() {
        this.LoadSpecialCodes();
        this.LoadCommodities();
        this.LoadMessagingRules();
        this.LoadIATACodes();
        this.LoadBookingProducts();
    }

    // Settings
    get IsManagingProduct() { return this.EntityPM.IsManagingProduct; }
    set IsManagingProduct(newValue: boolean) {
        if (this.EntityPM.IsManagingProduct != newValue) {
            this.EntityPM.IsManagingProduct = newValue;

            this.SetUIProperties();
        }
    }

    get IsProductMandatory() { return this.EntityPM.IsProductMandatory; }
    set IsProductMandatory(newValue: boolean) {
        if (this.EntityPM.IsProductMandatory != newValue) {
            this.EntityPM.IsProductMandatory = newValue;
        }
    }

    get IsDescriptionOfGoodsFromList() { return this.EntityPM.IsDescriptionOfGoodsFromList; }
    set IsDescriptionOfGoodsFromList(newValue: boolean) {
        if (this.EntityPM.IsDescriptionOfGoodsFromList != newValue) {
            this.EntityPM.IsDescriptionOfGoodsFromList = newValue;
        }
    }

    get ScheduleDays() { return this.EntityPM.ScheduleDays; }
    set ScheduleDays(newValue: number) {
        if (this.EntityPM.ScheduleDays != newValue) {
            this.EntityPM.ScheduleDays = newValue;
        }
    }

    get NoAvailabilityInFVAMessages() { return this.EntityPM.NoAvailabilityInFVAMessages; }
    set NoAvailabilityInFVAMessages(newValue: boolean) {
        if (this.EntityPM.NoAvailabilityInFVAMessages != newValue) {
            this.EntityPM.NoAvailabilityInFVAMessages = newValue;
        }
    }

    // Special handling codes
    public HandlingCodesObslist: AirlineAdaptationItem[];
    public HandlingCodesObslistCount: number = 0;
    private LoadSpecialCodes() {
        this.HandlingCodesObslist = [];
        var myService: AWBSpecialHandlingCodeListService = new AWBSpecialHandlingCodeListService();

        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allData: AWBSpecialHandlingCodeList[] = myResponse.Result;

                if (allData != null) {
                    var myData: AWBSpecialHandlingCodeList[] = allData.filter(d => d.AirlineId == this.EntityPM.Id);

                    myData.filter(d => !d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.HandlingCodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "SpecialCode", this));
                    });

                    myData.filter(d => d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.HandlingCodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "SpecialCode", this));
                    });

                    this.HandlingCodesObslistCount = this.HandlingCodesObslist.length;
                }
            }
        });
    }

    // Commodities
    public CommoditiesObslist: AirlineAdaptationItem[];
    public CommoditiesObslistCount: number = 0;
    private LoadCommodities() {
        this.CommoditiesObslist = [];
        var myService: CommodityListService = new CommodityListService();

        myService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allData: CommodityList[] = myResponse.Result;

                if (allData != null) {
                    var myData: CommodityList[] = allData.filter(d => d.AirlineId == this.EntityPM.Id);

                    myData.filter(d => !d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.CommoditiesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "Commodity", this));
                    });

                    myData.filter(d => d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.CommoditiesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "Commodity", this));
                    });

                    this.CommoditiesObslistCount = this.CommoditiesObslist.length;
                }
            }
        });
    }

    // Messaging rules
    public MessagingRulesObslist: MessagingRuleItem[];
    public MessagingRulesObslistCount: number = 0;
    private LoadMessagingRules() {
        this.MessagingRulesObslist = [];
        var myService: AirlineMessagingRuleListService = new AirlineMessagingRuleListService();

        myService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allData: AirlineMessagingRuleList[] = myResponse.Result;

                if (allData != null) {
                    var myData: AirlineMessagingRuleList[] = allData.filter(d => d.AirlineId == this.EntityPM.Id);

                    myData.filter(d => !d.InActive).sort((a, b) => { return (a.MessageTypeCode === b.MessageTypeCode) ? 0 : (a.MessageTypeCode < b.MessageTypeCode) ? -1 : 1 }).forEach(item => {
                        this.MessagingRulesObslist.push(new MessagingRuleItem(item, this));
                    });

                    myData.filter(d => d.InActive).sort((a, b) => { return (a.MessageTypeCode === b.MessageTypeCode) ? 0 : (a.MessageTypeCode < b.MessageTypeCode) ? -1 : 1 }).forEach(item => {
                        this.MessagingRulesObslist.push(new MessagingRuleItem(item, this));
                    });

                    this.MessagingRulesObslistCount = this.MessagingRulesObslist.length;
                }
            }
        });
    }

    // IATA codes
    public IATACodesObslist: AirlineAdaptationItem[];
    public IATACodesObslistCount: number = 0;
    private LoadIATACodes() {
        this.IATACodesObslist = [];
        var myService: IATACodeListService = new IATACodeListService();

        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allData: IATACodeList[] = myResponse.Result;

                if (allData != null) {
                    var myData: IATACodeList[] = allData.filter(d => d.AirlineId == this.EntityPM.Id);

                    myData.filter(d => !d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.IATACodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "IATACode", this));
                    });

                    myData.filter(d => d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.IATACodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "IATACode", this));
                    });

                    this.IATACodesObslistCount = this.IATACodesObslist.length;
                }
            }
        });
    }

    // Booking products
    public BookingProductsObslist: AirlineAdaptationItem[];
    public BookingProductsObslistCount: number = 0;
    private LoadBookingProducts() {
        this.BookingProductsObslist = [];
        var myService: BookingProductListService = new BookingProductListService();

        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allData: BookingProductList[] = myResponse.Result;

                if (allData != null) {
                    var myData: BookingProductList[] = allData.filter(d => d.AirlineId == this.EntityPM.Id);

                    myData.filter(d => !d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.BookingProductsObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "BookingProduct", this));
                    });

                    myData.filter(d => d.InActive).sort((a, b) => { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1 }).forEach(item => {
                        this.BookingProductsObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "BookingProduct", this));
                    });

                    this.BookingProductsObslistCount = this.BookingProductsObslist.length;
                }
            }
        });
    }

    private entityType: string;
    AddClicked(type: string) {
        this.entityType = type;
        var logitudeWindow = new LogitudeWindow();
        var myWindowTitle: string = "Add ";
        var itemPM: any;
        var objectTableName: string;

        switch (type) {
            case "SpecialCode": {
                myWindowTitle += "AWB Special Handling Code";
                objectTableName = "AWBSpecialHandlingCode";

                itemPM = new AWBSpecialHandlingCodePM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }

            case "Commodity": {
                myWindowTitle += "Commodity";
                objectTableName = "Commodity";

                itemPM = new CommodityPM();
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }

            case "MessagingRule": {
                myWindowTitle += "Messaging Rule";
                objectTableName = "AirlineMessagingRule";

                itemPM = new AirlineMessagingRulePM();
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.AirlineId = this.EntityPM.Id;
                itemPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                itemPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                itemPM.CreatedByUserId = SessionLocator.LoggedUserId;
                itemPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                break;
            }

            case "IATACode": {
                myWindowTitle += "IATA Code";
                objectTableName = "IATACode";

                itemPM = new IATACodePM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }

            case "BookingProduct": {
                myWindowTitle += "Booking Product";
                objectTableName = "BookingProduct";

                itemPM = new BookingProductPM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }
        }

        logitudeWindow.Title = myWindowTitle;

        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName(objectTableName).subscribe(response => {
            if (type == "MessagingRule") {
                logitudeWindow.WindowArgs = { AirlinePM: this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: true, };
                logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineMessagingRuleComponent');
            }

            else {
                logitudeWindow.WindowArgs = { AirlinePM: this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: true, };
                logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAdaptationItemComponent');
            }
        });
    }

    EditClicked(item: AirlineAdaptationItem, type: string) {
        this.entityType = type;
        var myWindowTitle: string = "Edit ";
        var objectTableName: string;
        var myService: any;
        var itemPM: any;

        switch (type) {
            case "SpecialCode": {
                myWindowTitle += "AWB Special Handling Code";
                objectTableName = "AWBSpecialHandlingCode";

                myService = new AWBSpecialHandlingCodePMService();
                myService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;

                        if (itemPM != null) {
                            this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });

                break;
            }

            case "Commodity": {
                myWindowTitle += "Commodity";
                objectTableName = "Commodity";

                myService = new CommodityPMService();
                myService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;

                        if (itemPM != null) {
                            this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });

                break;
            }

            case "IATACode": {
                myWindowTitle += "IATA Code";
                objectTableName = "IATACode";

                myService = new IATACodePMService();
                myService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;

                        if (itemPM != null) {
                            this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });

                break;
            }

            case "BookingProduct": {
                myWindowTitle += "Booking Product";
                objectTableName = "BookingProduct";

                myService = new BookingProductPMService();
                myService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;

                        if (itemPM != null) {
                            this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });

                break;
            }
        }
    }
    private OpenEditWindow(myWindowTitle: string, itemPM: any, objectTableName: string) {
        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName(objectTableName).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = myWindowTitle;
            logitudeWindow.WindowArgs = { AirlinePM: this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: false, };
            logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
            logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAdaptationItemComponent');
        });
    }

    EditRuleClicked(item: MessagingRuleItem) {
        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName("AirlineMessagingRule").subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            var myWindowTitle: string = "Edit Messaging Rule";
            var objectTableName: string = "AirlineMessagingRule";

            var myService = new AirlineMessagingRulePMService();
            myService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var itemPM = myResponse.Result;

                    if (itemPM != null) {
                        logitudeWindow.Title = myWindowTitle;
                        logitudeWindow.WindowArgs = { AirlinePM: this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: false, };
                        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                        logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineMessagingRuleComponent');
                    }
                }
            });
        });
    }

    OnWindowClosed(message: string) {
        if (message == "ok") {
            switch (this.entityType) {
                case "SpecialCode": {
                    this.LoadSpecialCodes();
                    break;
                }

                case "Commodity": {
                    this.LoadCommodities();
                    break;
                }

                case "MessagingRule": {
                    this.LoadMessagingRules();
                    break;
                }

                case "IATACode": {
                    this.LoadIATACodes();
                    break;
                }

                case "BookingProduct": {
                    this.LoadBookingProducts();
                    break;
                }
            }
        }
    }
}

export class AirlineAdaptationItem extends BaseComponent {
    public Id: string;
    public Code: string;
    public Name: string;
    public InActive: boolean;
    private entityCode: string;
    constructor(entityId: string, entityCode: string, entityName: string, inactive: boolean, entityType: string, public fatherComponent: AirlineAdaptationsTabComponent) {
        super();

        this.Id = entityId;
        this.Code = entityCode;
        this.Name = entityName;
        this.InActive = inactive;
        this.entityCode = entityCode;
    }
}

export class MessagingRuleItem extends BaseComponent {
    private entityList: AirlineMessagingRuleList;
    constructor(entity: AirlineMessagingRuleList, public fatherComponent: AirlineAdaptationsTabComponent) {
        super();
        this.entityList = entity;
    }

    get Id() { return this.entityList.Id; }
    get MessageTypeCode() { return this.entityList.MessageTypeCode; }
    get RuleFieldName() { return this.entityList.RuleFieldName; }
    get InActive() { return this.entityList.InActive; }

}