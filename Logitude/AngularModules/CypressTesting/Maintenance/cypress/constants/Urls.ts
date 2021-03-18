export class Urls
{
    public static readonly ContactViewsGetByFilters = "**/contactviews/getbyfilters?**GetCount=false**";
    public static readonly GetByFilter = "**/getbyfilters?**";
    public static readonly Contacts = "**/contacts";
    public static readonly PartnersDomain = "**/PartnersDomain";
    public static readonly Vendor = "**/vendors";
    public static readonly ContactsGetSingle = "**/contacts/getsingle?**";
    public static readonly VendorsGetSingle = "**/vendors/getsingle?**";
    public static readonly VendorsviewGetSingle = "**/vendorviews/getsingle/?**";
    public static readonly PostChangePassword='**/PostChangePassword/**'
    public static GetFilterSearch(filterBy:string){
        return '**/getbyfilters?**'+filterBy+'**'
    }
}