import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './ARPaymentGeneralTabComponent.html',
})
export class ARPaymentGeneralTabComponent extends BaseComponent implements OnInit {
  public MetodoPagoCode: any;

    public EntityPM: ARPaymentPM;
    public ObjectTableName: string = "ARPayment";
    // public TenantPM: TenantPM;
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: ARPaymentGeneralTabComponent = this;
    private ScreenCode: string = "ARPayment.GeneralTabScreen";
    public DisplaySATSettings: boolean = false;
    public DisplayFechaPago: boolean = false;

    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.EntityPM = entityArgs.EntityPM;
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
            if (this.EntityPM.PaymentInvoices.length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.MetodoPagoCode)) {
                this.EntityPM.UIProperties.SetEnabled("MetodoPagoCode", this.ObjectTableName, false);
            }

            
                this.DisplayFechaPago = true;
          

        }

        this.FillTipoCadenaPagoList();
        //this.TenantPM = SessionLocator.TenantPM;
        this.RunComponent();

    }

    ngOnInit() {

    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
    }



    // Properties 
    get SATPaymentMethodCode() { return this.EntityPM.SATPaymentMethodCode; }
    set SATPaymentMethodCode(newValue: string) {
        if (this.EntityPM.SATPaymentMethodCode != newValue) {
            this.EntityPM.SATPaymentMethodCode = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
  }


    public TipoCadenaPagoList: TipoCadenaPagoClass[] = [];
    FillTipoCadenaPagoList() {
        this.TipoCadenaPagoList.push({ Code: null, Name: null });
        this.TipoCadenaPagoList.push({ Code: "01", Name: "SPEI (Electronic Payment System between Banks)" });

    }

    private selectedTipoCadenaPago: TipoCadenaPagoClass = null;
    get SelectedTipoCadenaPago() {

        var tipoCadenaPago: string = null;
        if (!AppTool.IsNullOrEmpty(this.TipoCadenaPago)) {
            tipoCadenaPago = this.TipoCadenaPago.toUpperCase();
        }

        switch (tipoCadenaPago) {

            case "01":
                {
                    this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(d => d.Code == tipoCadenaPago)[0];
                    break;
                }

            default:
                {
                    this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(d => d.Code == null)[0];
                    break;
                }
        }

        return this.selectedTipoCadenaPago;
    }
    set SelectedTipoCadenaPago(newValue: TipoCadenaPagoClass) {
        if (this.selectedTipoCadenaPago != newValue) {
            this.selectedTipoCadenaPago = newValue;

            if (newValue == null) {
                this.TipoCadenaPago = null;
            }

            else {
                this.TipoCadenaPago = newValue.Code;
            }
        }

        this.ValidateTipoCadenaPagoFields();
       
    }

    ValidateTipoCadenaPagoFields() {
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (!AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (AppTool.IsNullOrEmpty(this.CertPago))
                    this.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CertPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(this.CadPago))
                    this.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CadPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(this.SelloPago))
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    }

    get TipoCadenaPago() {
        if (this.EntityPM != null) {
            return this.EntityPM.TipoCadenaPago;
        }
        else
            return null;
    }
    set TipoCadenaPago(newValue: string) {
        if (this.EntityPM.TipoCadenaPago != newValue) {
            this.EntityPM.TipoCadenaPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }

    get CadPago() {
        if (this.EntityPM != null) {
            return this.EntityPM.CadPago;
        }
        else
            return null;
    }
    set CadPago(newValue: string) {
        if (this.EntityPM.CadPago != newValue) {
            this.EntityPM.CadPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }

    get CertPago() {
        if (this.EntityPM != null) {
            return this.EntityPM.CertPago;
        }
        else
            return null;
    }
    set CertPago(newValue: string) {
        if (this.EntityPM.CertPago != newValue) {
            this.EntityPM.CertPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }
    get SelloPago() {
        if (this.EntityPM != null) {
            return this.EntityPM.SelloPago;
        }
        else
            return null;
    }
    set SelloPago(newValue: string) {
        if (this.EntityPM.SelloPago != newValue) {
            this.EntityPM.SelloPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }

    get FechaPago() {
        if (this.EntityPM != null) {
            return this.EntityPM.FechaPago;
        }
        else
            return null;
    }
    set FechaPago(newValue: Date) {
        if (this.EntityPM.FechaPago != newValue) {
            this.EntityPM.FechaPago = newValue;
             
        }
    }

  //get MetodoPagoCode() { return this.EntityPM.MetodoPagoCode; }
  //set MetodoPagoCode(newValue: string) {
  //  if (this.EntityPM.MetodoPagoCode != newValue) {
  //    this.EntityPM.MetodoPagoCode = newValue;
  //  }
  //}


   

}
class TipoCadenaPagoClass {
    public Code: string;
    public Name: string;
}
