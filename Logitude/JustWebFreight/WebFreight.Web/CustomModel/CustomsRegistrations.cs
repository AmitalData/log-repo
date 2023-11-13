
using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Customs.BL.Tasks;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.CustomsMessaging.Tasks;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Tasks;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel
{
    public class CustomsRegistrations
    {
        public static void Register()
        {
            // Update Service 
           // ContainerAccessor.Container.RegisterType<IJournalUpdateServiceExt, JournalUpdateServiceExt>("JournalUpdateServiceExt", new InjectionFactory(c => new JournalUpdateServiceExt()));
           
            // Query Service
            ContainerAccessor.Container.RegisterType<ICustomsDocumentQueryServiceExt, CustomsDocumentQueryServiceExt>("CustomsDocumentQueryServiceExt", new InjectionFactory(c => new CustomsDocumentQueryServiceExt()));



            ContainerAccessor.Container.RegisterType<ICreateUD2LTService, Logitude.CustomsMessaging.MessagingServices.CreateUD2LTService>("CreateUD2LTService", new InjectionFactory(c => new Logitude.CustomsMessaging.MessagingServices.CreateUD2LTService()));

            ContainerAccessor.Container.RegisterType<ISendBondedCustomDocumentService, SendBondedCustomDocumentService>("SendBondedCustomDocumentService", new InjectionFactory(c => new SendBondedCustomDocumentService()));


            ContainerAccessor.Container.RegisterType<ICustomsCloseCourierMasterService, CloseCourierMasterService>("CloseCourierMasterService", new InjectionFactory(c => new CloseCourierMasterService()));
            ContainerAccessor.Container.RegisterType<ICustomsSendManifestService, SendManifestService>("SendManifestService", new InjectionFactory(c => new SendManifestService()));
            ContainerAccessor.Container.RegisterType<ICustomsSendDeclarationStatus, SendDeclarationStatus>("SendDeclarationStatus", new InjectionFactory(c => new SendDeclarationStatus()));
            ContainerAccessor.Container.RegisterType<ICustomsSendReportExel, SendReportExel>("SendReportExel", new InjectionFactory(c => new SendReportExel()));
            ContainerAccessor.Container.RegisterType<ICustomsExchangeRatesQuery, ExchangeRatesQuery>("ExchangeRatesQuery", new InjectionFactory(c => new ExchangeRatesQuery()));
            ContainerAccessor.Container.RegisterType<ICustomsDeleteNotToCustomCommunication, DeleteNotToCustomCommunication>("DeleteNotToCustomCommunication", new InjectionFactory(c => new DeleteNotToCustomCommunication()));
            ContainerAccessor.Container.RegisterType<ICustomsDeleteCustomRequestSheet, DeleteCustomRequestSheet>("DeleteCustomRequestSheet", new InjectionFactory(c => new DeleteCustomRequestSheet()));
            ContainerAccessor.Container.RegisterType<ICustomsDeleteQueMessMoreDetails, DeleteQueMessMoreDetails>("DeleteQueMessMoreDetails", new InjectionFactory(c => new DeleteQueMessMoreDetails()));
            ContainerAccessor.Container.RegisterType<IPOAExpireReminder, POAExpireReminder>("POAExpireReminder", new InjectionFactory(c => new POAExpireReminder()));
            
            

            ContainerAccessor.Container.RegisterType<IUpdateOpenDeclarationInCourierMasterService, UpdateOpenDeclarationInCourierMasterService>("UpdateOpenDeclarationInCourierMasterService", new InjectionFactory(c => new UpdateOpenDeclarationInCourierMasterService()));



            ContainerAccessor.Container.RegisterType<IDICustomsSettingQueryService, Logitude.Customs.BL.EntityQueryServices.DICustomsSettingQueryService>("DICustomsSettingQueryService", new InjectionFactory(c => new Logitude.Customs.BL.EntityQueryServices.DICustomsSettingQueryService()));
            ContainerAccessor.Container.RegisterType<IDIUnifreightTaskService, Logitude.Customs.BL.Messaging.Amital.UnifreightTaskService.DIUnifreightTaskService >("DIUnifreightTaskService", new InjectionFactory(c => new Logitude.Customs.BL.Messaging.Amital.UnifreightTaskService.DIUnifreightTaskService()));
			ContainerAccessor.Container.RegisterType<ICustomsAutoDecClosing, CustomsAutoDecClosing>("CustomsAutoDecClosing", new InjectionFactory(c => new CustomsAutoDecClosing()));
			ContainerAccessor.Container.RegisterType<ICustomCreateTicket, CustomCreateTicket>("CustomCreateTicket", new InjectionFactory(c => new CustomCreateTicket()));

		}
	}
}