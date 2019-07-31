import {Component, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {VatTypePM} from '../../../EntityPMs/VatTypePM';
import {VATTypesGroupPM} from '../../../EntityPMs/VATTypesGroupPM';
import {VatTypePercentagePM} from '../../../EntityPMs/VatTypePercentagePM';
import {VatTypeList} from '../../../EntityLists/VatTypeList';
import {VatTypeListService} from '../../../Services/StandardLists/VatTypeListService';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CommonDomainService} from '../../../Services/CommonDomainService';
import {AccountingSettingPM} from '../../../EntityPMs/AccountingSettingPM';

@Component({
    moduleId: module.id,
    templateUrl: './VatTypeGeneralTabComponent.html',
})

export class VatTypeGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: VatTypePM;
    public DataContext = this;
    public IsNewEntity: boolean = true;
    public ObjectTableName: string = "VatType";
    public ItemsSource: MultiPercentageItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public args: EntityArgs) {
        super();

        this.EntityPM = args.EntityPM;

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNewEntity = true;
            this.NewEntityPercentageDate = DateTool.GetCurrentDateAsUtc();
        }

        else{
            this.IsNewEntity = false;
            this.Listen();
        }

        this.SetUIProperties();
        this.LoadMultiPercentages();
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

    public IsPercentagesAreaVisible: boolean = false;
    SetUIProperties() {
        var isPercentagesAreaVisible = false;

        if (this.IsNewEntity) {
            var isNewEntityPercentageRequired = false;
            var isNewEntityPercentageDateRequired = false;

            if (SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                isPercentagesAreaVisible = true;
            }

            if (!this.IsMultiPercentage) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentage)) {
                    isNewEntityPercentageRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentageDate)) {
                    isNewEntityPercentageDateRequired = true;
                }
            }

            this.UIProperties.SetRequired("NewEntityPercentage", this.ObjectTableName, isNewEntityPercentageRequired);
            this.UIProperties.SetRequired("NewEntityPercentageDate", this.ObjectTableName, isNewEntityPercentageDateRequired);
        }

        else {
            if (this.IsMultiPercentage) {
                isPercentagesAreaVisible = true;
            }

            else {
                isPercentagesAreaVisible = false;
            }
        }

        this.IsPercentagesAreaVisible = isPercentagesAreaVisible;
    }

    private allVatTypes: VatTypeList[] = [];
    private allVatPercentages: VatTypePercentagePM[] = [];
    LoadMultiPercentages() {

        var myVatsService: VatTypeListService = new VatTypeListService();
        myVatsService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allVatTypes = myResponse.Result;

                var myService: CommonDomainService = new CommonDomainService();
                myService.GetVatTypePercentagePMByDate(DateTool.GetCurrentDateAsUtc()).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.allVatPercentages = myResponse2.Result;
                        this.BuildItemsSource();
                    }
                });
            }
        });
    }
    BuildItemsSource() {
        this.ItemsSource = [];

        if (this.IsNewEntity) {
            this.allVatTypes.filter(f => f.IsMultiPercentage == false).forEach(item => {
                if (item.Id != this.EntityPM.Id) {
                    var myPercentagesPM = this.allVatPercentages.filter(f => f.VatTypeId == item.Id)[0];
                    this.ItemsSource.push(new MultiPercentageItem(item, myPercentagesPM, this));
                }
            });
        }

        else {
            this.EntityPM.VatTypeGroups.forEach(item => {
                var myVatTypeList = this.allVatTypes.filter(f => f.Id == item.SingleVATTypeId)[0];
                var myPercentagesPM = this.allVatPercentages.filter(f => f.VatTypeId == item.SingleVATTypeId)[0];
                this.ItemsSource.push(new MultiPercentageItem(myVatTypeList, myPercentagesPM, this));
            });
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get NewEntityPercentage() { return this.EntityPM.NewEntityPercentage; }
    set NewEntityPercentage(value: number) {
        if (this.EntityPM.NewEntityPercentage != value) {
            this.EntityPM.NewEntityPercentage = value;
            this.SetUIProperties();
        }
    }

    get NewEntityPercentageDate() { return this.EntityPM.NewEntityPercentageDate; }
    set NewEntityPercentageDate(value: Date) {
        if (this.EntityPM.NewEntityPercentageDate != value) {
            this.EntityPM.NewEntityPercentageDate = value;
            this.SetUIProperties();
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get LocalDescription() { return this.EntityPM.LocalDescription; }
    set LocalDescription(value: string) {
        if (this.EntityPM.LocalDescription != value) {
            this.EntityPM.LocalDescription = value;
        }
    }

    get IsMultiPercentage() { return this.EntityPM.IsMultiPercentage; }
    set IsMultiPercentage(value: boolean) {
        if (this.EntityPM.IsMultiPercentage != value) {
            this.EntityPM.IsMultiPercentage = value;
            this.SetUIProperties();
        }
    }
}
export class MultiPercentageItem {
    public Name: string;
    public Percentage: number;
    public PercentageText: string;
    public EntityPM: VATTypesGroupPM;
    constructor(itemList: VatTypeList, itemPM: VatTypePercentagePM, private fatherComponent: VatTypeGeneralTabComponent) {
        this.Name = itemList.EnglishName;

        if (itemPM) {
            this.Percentage = itemPM.Percentage;
            this.PercentageText = itemPM.Percentage + "%";
        }

        this.EntityPM = fatherComponent.EntityPM.VatTypeGroups.filter(f => f.SingleVATTypeId == itemList.Id)[0];
        if (this.EntityPM) {
            this.isChecked = true;
        }

        else {
            this.EntityPM = new VATTypesGroupPM(null);
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.GroupVATTypeId = this.fatherComponent.EntityPM.Id;
            this.EntityPM.SingleVATTypeId = itemList.Id;
        }
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            if (value) {
                this.fatherComponent.EntityPM.AddVATTypesGroupPM(this.EntityPM);
            }

            else {
                this.fatherComponent.EntityPM.RemoveVATTypesGroupPM(this.EntityPM);
            }
        }
    }
}
