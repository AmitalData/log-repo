//OpenAccess please define!!! 
//logitude please undefine!!!  
//#define reserveword

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmitalOpenAccessTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string myDESC = "Please change //#define reserveword >> #define reserveword in all solution";
            using (var cntxt = AmitalDbContextUtil.GetContext())
            {
                var poco = cntxt.CCUCARLs.FirstOrDefault();
#if reserveword
                var pocoGGGQ = cntxt.GGGQs.FirstOrDefault();
                myDESC = pocoGGGQ.DESC;
#endif
                MessageBox.Show(myDESC);
            }
        }
    }
}
