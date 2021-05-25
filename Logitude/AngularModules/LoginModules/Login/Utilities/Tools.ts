
export class Tools {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    static NewRandomString() {
        var chars = 'abcdefghijklmnopqrstuvwxyz';
        var length = 32;
        var result: string = '';
        for (var i = length; i > 0; --i) result += chars[Math.floor(Math.random() * chars.length)];
        return result;
    }

    public static DynamicLoader: any = null;





    public static GetSystemURL() {
        let systemUrl = location.href.replace('index.html', '');
        if (location.href.indexOf('localhost') > -1) {
            systemUrl = 'http://localhost:9996/';
        }
        else {
            let userLoginUrl = location.href.split("/index.html")[0];
            userLoginUrl = userLoginUrl.replace(userLoginUrl.substring(userLoginUrl.lastIndexOf('/'), userLoginUrl.length), "");
            systemUrl = userLoginUrl + "/";
        }

        return systemUrl;
    }

}