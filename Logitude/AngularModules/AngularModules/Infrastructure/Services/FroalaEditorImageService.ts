import { Injectable, EventEmitter } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

@Injectable()
export class FroalaEditorImageService {
    OnImageSelect: EventEmitter<string> = new EventEmitter();
    constructor() {

    }

   public ShowImageLibraryWindow() {
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 500;
        logitudeWindow.Title = "Insert Image";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Show("./Infrastructure/Components/LogitudeComponents/ImageLibraryComponent");
        this.ImageLibraryWindowSubscriber(logitudeWindow);
    }


    private ImageLibraryWindowSubscriber(logitudeWindow: LogitudeWindow) {
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if (!$event) return;
            this.OnImageSelect.emit(' <img  src=' + $event + ' class="rounded mb-3" style="width:150px;height:100px;">');         
        });
    }
}