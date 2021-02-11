import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionUploadedExcelPM } from '../../../EntityPMs/TariffVersionUploadedExcelPM';
import { TariffDomainService } from '../../../Services/TariffDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    templateUrl: './UploadedExcelsComponent.html',
})

export class UploadedExcelsComponent {
    public TariffId: string;
    public Version: number;
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: ExcelFileItem[];
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.TariffId = args['TariffId'];
        this.Version = args['Version'];

        this.LoadUploadedExcelFiles();
    }

    private LoadUploadedExcelFiles() {
        this.ItemsSource = [];

        this.CurrentSession.StartBusyIndicatorLoading();
        var myService: TariffDomainService = new TariffDomainService();
        myService.GetUploadedExcelByTariffAndVersion(this.TariffId, this.Version).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var myResult: TariffVersionUploadedExcelPM[] = response.Result;

                myResult.sort((a, b) => a.Index - b.Index).forEach(item => {
                    this.ItemsSource.push(new ExcelFileItem(item, this));                    
                });
            }

            this.CurrentSession.StopBusyIndicator();
        });

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class ExcelFileItem {
    public EntityPM: TariffVersionUploadedExcelPM;    
    constructor(item: TariffVersionUploadedExcelPM, private fatherComponent: UploadedExcelsComponent) {
        this.EntityPM = item;
    }

    get Id() { return this.EntityPM.Id; }
    get Index() { return this.EntityPM.Index; }
    get UploadDate() { return this.EntityPM.UploadDate; }
    get UploadedByUserName() { return this.EntityPM.UploadedByUserName; }
    get NumberOfLines() { return this.EntityPM.NumberOfLines; }
    get FileName() { return this.EntityPM.FileName; }
    
    DownloadClicked() {
        DownloadManager.DownloadPage(this.EntityPM.DocumentId + "*" + this.FileName);
    }
}

