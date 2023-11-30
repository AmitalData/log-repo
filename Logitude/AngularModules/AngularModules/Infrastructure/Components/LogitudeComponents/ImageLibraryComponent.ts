import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ImageLibraryList } from 'Infrastructure/EntityLists/ImageLibraryList';
import { ImageLibraryExtendedListService } from 'Infrastructure/Services/ExtendedLists/ImageLibraryExtendedListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from 'Infrastructure/Tools';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

@Component({
    selector: 'ImageLibraryComponent',
    templateUrl: './ImageLibraryComponent.html',

})

export class ImageLibraryComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    private ImageLibraryExtendedListService: ImageLibraryExtendedListService;
    Images: ImageLibraryList[] = [];
    FilteredImages: ImageLibraryList[] = [];
    SearchText: string;
    private Tenant: number = SessionLocator.Tenant;
    SelectedTabCode: string = 'CT';

    constructor() {
        this.ImageLibraryExtendedListService = new ImageLibraryExtendedListService();
    }

    ngOnInit(): void {
        this.GetImages();
    }

    GetImages() {
        this.ImageLibraryExtendedListService.GetAll().subscribe((response: ServiceResponse) => {
            if (response.HasError) this.ShowErrorMessage(response);
            else this.FillImages(response);
        });
    }

    ShowErrorMessage(response: ServiceResponse) {
        if (!response.ErrorsArray || response.ErrorsArray.length == 0) return;
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Title = 'Error';
        messageWindow.Show(response.ErrorsArray.join(", "));
    }

    FillImages(response: ServiceResponse) {
        var images = response?.Result;
        if (images == undefined || images == null) return;
        this.Images = images;
        this.FilteredImages = this.GetTenantImages();
    }

    GetTenantImages(): ImageLibraryList[] {
        return this.Images.filter(x => x.Tenant == this.Tenant);
    }

    OnImageClick(image: ImageLibraryList) {
        this.CurrentSession.CurrentWindow.Close(JSON.stringify(image));
    }

    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";
        this.SearchText = searchText;
        this.FilterList(searchText);
    }

    FilterList(searchText: string) {
        this.FilteredImages = [];
        if (AppTool.IsNullOrEmpty(searchText)) this.FilteredImages = this.GetTenantImages();
        else this.FilteredImages = this.GetTenantImages().filter(d => (d.Name && d.Name.toLowerCase().indexOf(searchText.toLowerCase()) > -1));
    }

    SelectedTabChange(selectedTabCode: string) {
        this.SelectedTabCode = selectedTabCode;
        if (selectedTabCode == 'CT') this.Tenant = SessionLocator.Tenant;
        else this.Tenant = 0;
        this.onSearchTextChangeEvent(this.SearchText);
    }

    IsNotTenantZero(): boolean {
        return SessionLocator.Tenant != 0;
    }
}