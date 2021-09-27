using Logitude.DocumentTests.Services.Preparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.Logitude.FullAccounting.Test.Hooks
{
    [Binding]
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare-GetDocumentType")]
        public static void PreprepareGetDocumentType()
        {
            new DocumentTypepPreparation().Prepare();
        }

    }
}
