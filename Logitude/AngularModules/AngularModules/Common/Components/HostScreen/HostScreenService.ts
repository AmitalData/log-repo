import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";

export class HostScreenService {
    public static async open(title: string, logitudeCommandId: string): Promise<any> {
        const logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { logitudeCommandId: logitudeCommandId, windowInstance: logWindow };
        logWindow.Width = window.outerWidth;
        logWindow.Height = window.outerHeight;
        logWindow.Title = title
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./Common/Components/HostScreen/HostScreenComponent');

        return new Promise<any>(res => logWindow.WindowClosed.subscribe(dataReturn => res(dataReturn)));
    }
}

export type HostScreenOptions = {
    
}