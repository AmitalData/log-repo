import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";

export class HostScreenService {
    public static open(title: string, logitudeCommandId: string): LogitudeWindow {
        const logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { logitudeCommandId: logitudeCommandId, windowInstance: logWindow };
        logWindow.Width = window.outerWidth;
        logWindow.Height = window.outerHeight;
        logWindow.Title = title
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./Common/Components/HostScreen/HostScreenComponent');        
        return  logWindow;
    }
}

export type HostScreenOptions = {
    
}