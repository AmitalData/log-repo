import {Component, OnInit} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {PackagePM} from '../../../../Common/EntityPMs/PackagePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {PackagePMService} from '../../../../Common/Services/StandardPMs/PackagePMService';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ExcelExportService} from '../../../../Common/Services/Others/ExcelExportService'
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
declare var UploadLogoFile, base64ToArrayBuffer, saveByteArray, ArrayBufferToBase64: any;
@Component({
    moduleId: module.id,
    templateUrl: './UserPackagesComponent.html',
})

export class UserPackagesComponent implements OnInit {
    public ItemsSource: UserPackageItemClass[] = [];
    private loadedDataList: PackagePM[] = [];
    public EntityPMService: PackagePMService;
    public DomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.EntityPMService = new PackagePMService();
        this.DomainService = new InfrastructureDomainService();
    }

    ngOnInit() {
        this.LoadData(true);
    }

    LoadData(startBusyIndicator: boolean) {

        if (startBusyIndicator) {
            this.CurrentSession.StartBusyIndicatorLoading();
        }

        this.DomainService.GetPackagesBMs().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.loadedDataList = myResponse.Result;
            }

            this.BuildItemsSource();

            if (startBusyIndicator) {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.BuildItemsSource();
    }

    private showInactive: boolean = false;
    public get ShowInactive() { return this.showInactive; }
    public set ShowInactive(value: boolean) {
        if (this.showInactive != value) {
            this.showInactive = value;
            this.BuildItemsSource();
        }
    }

    private filterTypeCode: string = "AL";
    public get FilterTypeCode() { return this.filterTypeCode; }
    public set FilterTypeCode(value: string) {
        if (this.filterTypeCode != value) {
            this.filterTypeCode = value;
            this.BuildItemsSource();
        }
    }


    ExportFeaturesToCSVFile() {
        var service: ExcelExportService = new ExcelExportService();
        service.ExportFeaturesToCSVFile().subscribe(res => {
            if (!res.HasError) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("Export?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        var defaultname: string = "package features" + "_" + new Date().toLocaleDateString();
                        var data = base64ToArrayBuffer(res.Result);
                        saveByteArray(defaultname, data, ".csv");
                    }
                });

            }
        });
    }

    public ImportFeaturesFileHtmlId: string = Guid.NewRandomString();
    ImportFeaturesToFile() {
        document.getElementById(this.ImportFeaturesFileHtmlId).click();
    }

    ImportFeaturesFile(event: any) {
        var file: any = UploadLogoFile(this.ImportFeaturesFileHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    }

    ImportFeatures(data: any) {
        var service: ExcelExportService = new ExcelExportService();
        var file: ImageParameter = new ImageParameter();
        file.Base64String = data;

        service.ImportFeaturePackages(file).subscribe(res => {
            this.CurrentSession.StopBusyIndicator();

            var wind = new MessageWindow();
            wind.Show("Import completed successfully");
        });
    }


    ArrayBufferToBase64(file: any, viewmode: any) {
        if (file) {
            var reader: FileReader = new FileReader();
            var reader = new FileReader();

            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmode.ImportFeatures(window.btoa(binary));
            };

            reader.onerror = function (e) {                
                SessionLocator.SelectedSession.StopBusyIndicator();

                var wind = new MessageWindow();
                wind.Show("Error Importing file");
            };

            reader.readAsArrayBuffer(file);
        }
    }

    
    BuildItemsSource() {
        this.ItemsSource = [];

        var items: PackagePM[] = this.loadedDataList;

        if (this.FilterTypeCode != "AL") {
            items = items.filter(f => f.FeaturePackageTypeCode == this.FilterTypeCode);
        }

        if (!this.ShowInactive) {
            items = items.filter(f => f.InActive == false);
        }

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });

        items.filter(f => f.FeaturePackageTypeCode == "AD").forEach(item => {
            this.ItemsSource.push(new UserPackageItemClass(item, this));
        });

        items.filter(f => f.FeaturePackageTypeCode == "BS").forEach(item => {
            this.ItemsSource.push(new UserPackageItemClass(item, this));
        });

        items.filter(f => f.FeaturePackageTypeCode == "PK").forEach(item => {
            this.ItemsSource.push(new UserPackageItemClass(item, this));
        });
    }

    NewPackageClicked() {       
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add New Package";
        logWindow.WindowArgs = { PackagePM: null };

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {

                    if (comp.FeaturePackageTypeCode == "BS") {
                        this.LoadData(false);

                        var item = new UserPackageItemClass(comp.EntityPM, this);
                        this.EditPackageClicked(item);
                    }

                    else {
                        this.LoadData(true);
                    }
                }
            });
        });

        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/AddEditUserPackageComponent');
    }
    EditPackageClicked(item: UserPackageItemClass) {
        if (item.EntityPM.FeaturePackageTypeCode == "BS") {
            var logWindow = new LogitudeWindow();
            logWindow.IsFillScreen = true;
            logWindow.Title = "Edit " + item.Name + " Package Features";
            logWindow.WindowArgs = { PackageCode: item.Code };
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/EditPackageFeaturesComponent');
        }

        else {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit " + item.Name + " Package";
            logWindow.WindowArgs = { PackagePM: item.EntityPM };
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/AddEditUserPackageComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadData(true);
                }
            });
        }
    }
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
export class UserPackageItemClass {
    public EntityPM: PackagePM;
    constructor(item: PackagePM, private fatherComponent: UserPackagesComponent) {
        this.EntityPM = item;
    }

    get Code() { return this.EntityPM.Code; }
    get Name() { return this.EntityPM.Name; }
    get Type() { return this.EntityPM.FeaturePackageTypeName; }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;

            this.fatherComponent.EntityPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            });
        }
    }
}
