import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ImageLibraryList } from 'Infrastructure/EntityLists/ImageLibraryList';
import { ImageLibraryExtendedListService } from 'Infrastructure/Services/ExtendedLists/ImageLibraryExtendedListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'ImageLibraryComponent',
    templateUrl: './ImageLibraryComponent.html',

})


export class ImageLibraryComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    private ImageLibraryExtendedListService: ImageLibraryExtendedListService;
    Images: ImageLibraryList[] = [];

    constructor() {
        this.ImageLibraryExtendedListService = new ImageLibraryExtendedListService();
    }

    ngOnInit(): void {
        this.GetImages();
    }

    GetImages() {
        this.ImageLibraryExtendedListService.GetAll().subscribe((response: ServiceResponse) => {
            this.FillImages(response);
        });
    }

    FillImages(response: ServiceResponse) {
        var images = response?.Result;
        if (images == undefined || images == null) return;
        this.Images = images;
    }

    OnImageClick(image: ImageLibraryList) {
        this.CurrentSession.CurrentWindow.Close(JSON.stringify(image));
    }

}