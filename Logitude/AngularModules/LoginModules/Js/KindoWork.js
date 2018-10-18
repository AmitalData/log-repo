//selectedcompany: any;
function onChange() {

    var combobox = $("#cmbTenants").data("kendoComboBox");
    var selectedItem = combobox.dataSource.view()[combobox._current.index()];
    selectedcompany = selectedItem;
    currentTenant = $('#cmbTenants').data('kendoComboBox').value();
    // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

}

function showTenantsCombo(companies) {

    companyList = companies;


    $("#cmbTenants").kendoComboBox(
    {
        dataTextField: "CompanyName",
        dataValueField: "Tenant",
        placeholder: "Choose your company",
        //index: 0,
        //select: onSelectedTenantChanged,
        change: onChange,
        filter: "contains",
        suggest: false,


        // template:
        //     '<dd>${ CompanyName }</dd>'
        //,

        dataSource:
        {
            //type: "odata",
            data: companies
            //serverFiltering: true,
            //serverPaging: true,
            //pageSize: 20,
        }
        //template: kendo.template($("#partnerTemplate").html())
    });

}

function getselectedcompany() {

    var combobox = $("#cmbTenants").data("kendoComboBox");
    var selectedItem = combobox.dataSource.view()[combobox._current.index()];
    selectedcompany = selectedItem;
    return selectedcompany;
    //currentTenant = $('#cmbTenants').data('kendoComboBox').value();
    // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

}