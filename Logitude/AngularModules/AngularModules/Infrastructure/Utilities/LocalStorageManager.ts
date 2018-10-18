export class LocalStorageManager {
    public static  SetItem(key: string, item: string):boolean {
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