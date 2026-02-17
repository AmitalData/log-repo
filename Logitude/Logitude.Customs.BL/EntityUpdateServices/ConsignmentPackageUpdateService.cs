using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.ComponentModel;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ConsignmentPackageUpdateService : EntityUpdateService<ConsignmentPackage, ConsignmentPackagePM, ConsignmentPM>
    {
        protected override void OnCreating(ConsignmentPackagePM entityPM, ConsignmentPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.ConsignmentNumber = entityParentPM.ConsignmentNumber;

            entityParentPM.ConsignmentPackagLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.ConsignmentPackagLastLineNumber;
            entityPM.GrossMassMeasureTypeCode = "KGM";
            entityPM.PackageQuantityTypeCode = "EA";
        }

        protected override void AfterUpdating(ConsignmentPackagePM entityPM,ConsignmentPM entityParentPM)
        {
            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            //{

               
                
            //        SubmitChanges();
            //        ICustomContext context = MainContext as CustomContext;
            //        ConsignmentPackageRepository consignmentPackageRepository = new ConsignmentPackageRepository(context);
            //        List<ConsignmentPackage> consignments = consignmentPackageRepository.GetMulti(new ConsignmentKeys() { ConsignmentNumber = entityPM.ConsignmentNumber, DeclarationId = entityPM.DeclarationId });
            //        consignments = consignments.OrderBy(d => d.LineNumber).ToList();
            //        int index = 0;
            //        foreach (ConsignmentPackage item in consignments)
            //        {
            //            index += 1;
            //            item.SequenceNumeric = index;
            //            consignmentPackageRepository.Update(item);
            //            if (item.DeclarationId == entityPM.DeclarationId && item.ConsignmentNumber == entityPM.ConsignmentNumber && item.LineNumber == entityPM.LineNumber)
            //            {
            //                entityPM.SequenceNumeric = item.SequenceNumeric;
            //            }
            //        }
            //        consignmentPackageRepository.SubmitChanges();
               
               
            //}
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.ConsignmentPackageRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
