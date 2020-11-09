
export  class AppHelper{

    constructor(){
        
    }
    public static AppBack(router, location,tenant)
    {
        var url = router.url; // /1/search/1000
        var splittedUrl = url.split('/');
        if (splittedUrl.length > 3 && splittedUrl[2] == 'search') {
            if (splittedUrl[3] == 'shipment') {
                var isDirectURL = history.length <= 3;
                if (isDirectURL)
                    router.navigate([tenant, 'search']);
                else
                    location.back();
            }
            else {
                // ex:  /1/search/1000
                //V
                router.navigate([tenant, 'search']);
            }
        }
        else{
            router.navigate([tenant, 'search']);
        }

    }
    public static GetBackEnabled(router)
    {
        var showBackButton = false;
        var url = router.url;
        var splittedUrl = url.split('/');
        if (splittedUrl.length > 3 && splittedUrl[2] == 'search')
            showBackButton = true;


        return showBackButton;
    }
}
