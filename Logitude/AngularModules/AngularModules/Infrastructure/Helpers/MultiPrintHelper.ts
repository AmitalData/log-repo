import { MessageWindow } from "Controls/Windows/MessageWindow";
import { AppTool } from 'Infrastructure/Tools';
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

export function IsMultiPrintValid(title, rowCount): boolean {
    var messageWindow: MessageWindow = new MessageWindow();
    messageWindow.Title = "Multi Print " + title;
    if (rowCount == 0) {
        messageWindow.Show("Sorry! You can’t perform the batch print process. The number of " + title + " in the view can't be 0");
        return false;
    }

    if (rowCount > 50) {
        messageWindow.Show("Sorry! You can’t perform the batch print process. The number of " + title + " in the view mustn't exceed 50");
        return false;
    }

    if (AppTool.IsNullOrEmpty(rowCount)) {
        return false;
    }

    return true;

}

