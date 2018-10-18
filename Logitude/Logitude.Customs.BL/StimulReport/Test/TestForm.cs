using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;

using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;

using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Windows.Forms;



namespace Logitude.Customs.BL.StimulReport.Test
{
    public partial class TestForm : Form
    {
        //public Button button1;

        public TestForm()
        {
            InitializeComponent();

        }
           

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello World");

            
            DeclarationPM MyDeclarationPM;
            var context = CustomContext.GetContext(1);
            var myQueryService = new DeclarationQueryService(context);
            MyDeclarationPM = myQueryService.GetSingle("1-139", true, false);
            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            var myFile = declarationSRMapping.Get(MyDeclarationPM);
            var xml = XmlGenericUtil<DeclarationSReport>.SerializeObject(myFile);
             
        }

         [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new TestForm());
        }

        
    }
}
