export var SessionInfo = (function () {
    function SessionInfo() {
    }
    Object.defineProperty(SessionInfo, "PlShortName", {
        get: function () { return this.plShortName; },
        set: function (newValue) { this.plShortName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "MainLocation", {
        get: function () { return this.mainLocation; },
        set: function (newValue) { this.mainLocation = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserId", {
        get: function () { return this.loggedUserId; },
        set: function (newValue) { this.loggedUserId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserTenant", {
        get: function () { return this.loggedUserTenant; },
        set: function (newValue) { this.loggedUserTenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "Token", {
        get: function () { return this.token; },
        set: function (newValue) { this.token = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedSessionToken", {
        get: function () { return this.loggedSessionToken; },
        set: function (newValue) { this.loggedSessionToken = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserEmail", {
        get: function () { return this.loggedUserEmail; },
        set: function (newValue) { this.loggedUserEmail = newValue; },
        enumerable: true,
        configurable: true
    });
    SessionInfo.ClearExternalParams = function () {
        this.IsExternalParams = false;
    };
    SessionInfo.GetLogitudeURL = function () {
        var logitude_url = location.href.replace('index.html', '');
        if (location.href.indexOf('localhost') > -1) {
            logitude_url = 'http://localhost:9996/'; // test.logitudeworld.com/test/';
        }
        else {
            var urlArr = location.href.split("/index.html");
            var url = urlArr[0];
            url = url.replace(url.substring(url.lastIndexOf('/'), url.length), "");
            logitude_url = url + "/";
        }
        return logitude_url;
    };
    SessionInfo.plShortName = "";
    SessionInfo.myExternalParams = null;
    SessionInfo.IsExternalParams = false;
    return SessionInfo;
}());
//# sourceMappingURL=SessionInfo.js.map