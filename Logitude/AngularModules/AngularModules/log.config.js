var logitude_url = location.href.replace('index.html', '');
var Islam = false; // for testing the cached data services
var OneUsePasswordKey = "";
if (location.href.indexOf('localhost') > -1) {
    logitude_url = 'http://localhost:62611/';
}
else {
    var urlArr = location.href.split("/index.html");
    var url = urlArr[0];
    url = url.replace(url.substring(url.lastIndexOf('/'), url.length), "");
    logitude_url = url + "/";

}



