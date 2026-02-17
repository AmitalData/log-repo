export class LocalStorageManager {
    public static SetItem(key: string, item: string): boolean {

        var isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;//MAC//WIN32
        if (isMac) {
            console.warn("============>This is a mac machine no caching is used!!!!");
            return false;
        }

        try {
            window.localStorage.setItem(key, item);
            return true;
        }
        catch (ex) {
            console.warn(ex);
            return false;
        }
    }

    public static GetItem(key: string): string {
        return window.localStorage.getItem(key);
    }

    public static RemoveItem(key: string) {
        try {
            window.localStorage.removeItem(key);
        }
        catch (ex) { console.warn(ex); }
    }
}
