import {Component, OnInit} from 'angular2/core';
import {NgStyle} from 'angular2/common';

@Component({
    selector: 'IconButton',
    inputs: ['Name', 'IsSmall', 'Width', 'Height', 'IsEnabled'],
    templateUrl: 'app/ApplicationControls/IconButton.html',
    directives: [NgStyle],
})

export class IconButton implements OnInit {
    public Name: string;
    public Width: number;
    public Height: number;
    public Source: string;
    public IsSmall: boolean;
    public IsEnabled: boolean;
    public LeftIndent: string = "1px";
    private mySource: string;
    private mySourceOver: string;
    ngOnInit() {

        if (this.Name != null) {

            if (this.Width == null || this.Width === undefined || this.Width == 0) {
                this.Width = this.IsSmall ? 16 : 20;
            }

            if (this.Height == null || this.Height === undefined || this.Height == 0) {
                this.Height = this.IsSmall ? 16 : 19;
            }

            if (this.IsSmall) {
                this.LeftIndent = "2px";
            }

            switch (this.Name.toLowerCase()) {
                case "settings": {
                    this.mySource = 'images/Buttons/Settings.png';
                    break;
                }

                case "refresh": {
                    this.mySource = 'images/Buttons/Refresh.png';
                    break;
                }

                case "add": {
                    this.mySource = "images/Buttons/Add.png";
                    this.mySourceOver = "images/Buttons/Add.over.png";
                    break;
                }

                case "edit": {
                    this.mySource = "images/Buttons/Edit.png";
                    this.mySourceOver = "images/Buttons/Edit.over.png";
                    break;
                }

                case "delete": {
                    this.mySource = "images/Buttons/Delete.png";
                    this.mySourceOver = "images/Buttons/Delete.over.png";
                    break;
                }
            }

            this.Source = this.mySource;
            if (this.mySourceOver == null) {
                this.mySourceOver = this.mySource;
            }
        }
    }

    OnMouseEnter() {
        this.Source = this.mySourceOver;
    }

    OnMouseLeave() {
        this.Source = this.mySource;
    }
}