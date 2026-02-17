import {Component} from '@angular/core';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {PackagePM} from '../../../../Common/EntityPMs/PackagePM';
import {PackageConnectedPackagePM} from '../../../../Common/EntityPMs/PackageConnectedPackagePM';
import {PackagePMService} from '../../../../Common/Services/StandardPMs/PackagePMService';
import {PackageList} from '../../../../Common/EntityLists/PackageList';
import {PackageListService} from '../../../../Common/Services/StandardLists/PackageListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditUserPackageComponent.html',
})

export class AddEditUserPackageComponent extends BaseComponent {
    public EntityPM: PackagePM;
    public DataContext = this;
    public ObjectTableName: string = "Package";
    public ValidationErrorsList: string[] = [];
    public ItemsSource: ConnectedPackageItemClass[] = [];
    public IsResourcesReady: boolean = false;
    public IsNewMode: boolean = false;
    public IsEditMode: boolean = false;
    private allExistingPackagesCodes: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['PackagePM'];

        if (!this.EntityPM) {
            this.IsNewMode = true;
            this.EntityPM = new PackagePM();
        }

        else {
            this.IsEditMode = true;
        }

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.SetUIProperties();
            this.BuildItemsSource();

            if (this.IsEditMode) {
                this.Clone();
            }
        });
    }

    public IsGridViewVisible: boolean = false;
    SetUIProperties() {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, this.IsNewMode);
        this.UIProperties.SetEnabled("FeaturePackageTypeCode", this.ObjectTableName, this.IsNewMode);
        this.IsGridViewVisible = this.FeaturePackageTypeCode == "PK" || this.FeaturePackageTypeCode == "AD" ? true : false;
    }

    public get Code() { return this.EntityPM.Code; }
    public set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    public get FeaturePackageTypeCode() { return this.EntityPM.FeaturePackageTypeCode; }
    public set FeaturePackageTypeCode(value: string) {
        if (this.EntityPM.FeaturePackageTypeCode != value) {
            this.EntityPM.FeaturePackageTypeCode = value;
            this.IsGridViewVisible = this.FeaturePackageTypeCode == "PK" || this.FeaturePackageTypeCode == "AD" ? true : false;
        }
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    BuildItemsSource() {

        this.EntityPM.ConnectedPackages.forEach(itemConnected => {
            this.ItemsSource.push(new ConnectedPackageItemClass(itemConnected, true));
        });

        var myService = new PackageListService();
        myService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                var allPackages: PackageList[] = myResponse.Result;

                allPackages = allPackages.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });

                allPackages.forEach(item => {
                    this.allExistingPackagesCodes.push(item.Code.toLowerCase());

                    if (item.FeaturePackageTypeCode == "BS" && item.InActive == false) {
                        if (this.EntityPM.ConnectedPackages.filter(f => f.ConnectedPackageCode == item.Code).length == 0) {
                            var itemPM = new PackageConnectedPackagePM(null);
                            itemPM.PackageCode = this.Code;
                            itemPM.ConnectedPackageCode = item.Code;
                            itemPM.ConnectedPackageName = item.Name;
                            this.ItemsSource.push(new ConnectedPackageItemClass(itemPM, false));
                        }
                    }
                });
            }
        });
    }

    CancelButtonClicked() {
        if (this.IsEditMode) {
            this.RejectChanges();
        }

        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.IsNewMode) {
            if (this.Code != null) {
                var isExists: boolean = ArrayTool.Contains(this.allExistingPackagesCodes, this.Code.toLowerCase());
                if (isExists) {
                    errors.push("Package with same code already exists");
                }
            }
        }

        if (this.FeaturePackageTypeCode != null) {
            if (this.FeaturePackageTypeCode.toUpperCase() == "PK" || this.FeaturePackageTypeCode.toUpperCase() == "AD") {
                var allCheckedItems = this.ItemsSource.filter(d => d.IsChecked);

                if (allCheckedItems.length == 0) {
                    errors.push("You must select 1 package at least");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.FeaturePackageTypeCode.toUpperCase() == "BS") {
                this.EntityPM.ConnectedPackages = [];
            }

            else {
                this.ItemsSource.forEach((item: ConnectedPackageItemClass) => {
                    if (item.EntityPM.PackageCode != this.EntityPM.Code) {
                        item.EntityPM.PackageCode = this.EntityPM.Code;
                    }

                    if (item.IsChecked) {
                        this.EntityPM.AddPackageConnectedPackagePM(item.EntityPM);
                    }

                    else {
                        this.EntityPM.RemovePackageConnectedPackagePM(item.EntityPM);
                    }
                });
            }

            var myService = new PackagePMService();

            if (this.IsNewMode) {
                myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }

            else {
                myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    }

    private myCloner: Cloner;
    private AllCloners: Cloner[] = [];
    private Clone() {

        this.myCloner = new Cloner(this);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('FeaturePackageTypeCode');
        this.myCloner.AddEntity(this.EntityPM);

        this.ItemsSource.forEach(item => {
            var itemCloner = new Cloner(item);
            itemCloner.AddField('IsChecked');
            itemCloner.AddEntity(item.EntityPM);
            this.AllCloners.push(itemCloner);
        });
    }
    private RejectChanges() {

        this.AllCloners.forEach(myCloner => {
            myCloner.RejectChanges();
        });

        this.myCloner.RejectChanges();
    }
}
export class ConnectedPackageItemClass {
    public EntityPM: PackageConnectedPackagePM;
    constructor(item: PackageConnectedPackagePM, isConnected: boolean) {
        this.EntityPM = item;
        this.isChecked = isConnected;
    }

    public get Name() { return this.EntityPM.ConnectedPackageName; }

    private isChecked: boolean = false;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }
}
