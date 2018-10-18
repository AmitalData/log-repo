using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer
{
    class DeclarationErrorPointerTester
    {
        /*void tst()
        {
            var myDeclaration = new DeclarationError();
            myDeclaration.ErrorDetails = new List<Entity>();
            myDeclaration.ErrorDetails.Add(new Entity()
            {
                //Sequence = 1,
                //EntityErrors = GetEntityErrors(),
                //FieldsErrors = GetFieldsErrors ()
            }
            );

            //myDeclaration.Consigments = GetConsigmentsError();
        }

        private List<Consigment> GetConsigmentsError()
        {
            var myConsigments = new List<Consigment>();

            myConsigments.Add(new Consigment()
                {
                    ErrorDetails = new List<Entity>(),
                    Packages = GetPackages(),
                }
                );

            return myConsigments;
        }

        private List<Package> GetPackages()
        {
            var myPackages = new List<Package>();

            myPackages.Add(new Package()
                {
                    ErrorDetails = new List<Entity>()
                }
                );

            return myPackages;
        }

        private List<FieldError> GetFieldsErrors()
        {
            var o = new List<FieldError>();
            o.Add(new FieldError()
            {
                Code = "LoadPort",
                MessageError = "Doc XXX missing "
            });
            return o;
        }

        private List<EntityError> GetEntityErrors()
        {
            var myErrs = new List<EntityError>();
            myErrs.Add(new EntityError()
            { //Code = "LoadPort", 
                MessageError = "Doc XXX missing "
            });
            return myErrs;
        }*/
    }
}
