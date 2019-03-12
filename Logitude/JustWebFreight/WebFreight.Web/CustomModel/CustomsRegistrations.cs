
using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
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
            ContainerAccessor.Container.RegisterType<ICustomsRequiredFieldsValidatorExt, CustomsRequiredFieldsValidator>("CustomsRequiredFieldsValidator", new InjectionFactory(c => new CustomsDocumentQueryServiceExt()));
            


        }
    }
}