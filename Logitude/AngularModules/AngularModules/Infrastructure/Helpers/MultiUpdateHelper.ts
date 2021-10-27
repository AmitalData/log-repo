import { MessageWindow } from "Controls/Windows/MessageWindow";


export function IsMultiUpdateValid(title, rowCount): boolean {
    var messageWindow: MessageWindow = new MessageWindow();
    messageWindow.Title = "Multi Update " + title;
    if (rowCount == 0) {
        messageWindow.Show("Sorry! You can’t perform the multiple update process. The number of " + title + " in the view can't be 0");
        return false;
    }

    if (rowCount > 100) {
        messageWindow.Show("Sorry! You can’t perform the multiple update process. The number of " + title + " in the view mustn't exceed 100");
        return false;
    }
    return true;
}

