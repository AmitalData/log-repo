
using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Customs.BL.Tasks;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.CustomsMessaging.Tasks;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
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



        }
    }
}