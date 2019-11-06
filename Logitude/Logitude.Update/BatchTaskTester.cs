using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    public partial class BatchTaskTester : Form
    {
        public BatchTaskTester()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            string batchTaskId = BatchTaskIdTextBox.Text;
            int tenant;
            int.TryParse(BatchTaskIdTextBox.Text, out tenant);

            BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
            BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(batchTaskId, false, false);

            if (batchTaskExecutionPM != null)
            {
                //batchTaskExecutionPM.ClassName this is the path of the class i want to execute which inhirits from BatchTaskExecutionService plus the assembly name.
                List<object> args = new List<object>();
                args.Add(batchTaskExecutionPM);
                object[] ArrArgs = args.ToArray();
                string[] pathArr = batchTaskExecutionPM.ClassName.Split(',');
                string assemblyName = pathArr[1];
                string className = pathArr[0];



                string contextClassName = Assembly.CreateQualifiedName(assemblyName, className);
                Type executedClassType = Type.GetType(contextClassName);
                var batchTaskService = System.Activator.CreateInstance(executedClassType, ArrArgs) as BatchTaskExecutionsService;
                //if (!this.SupressStartThread)
                //{
                //    // open a new thread and call the class runcode.
                //    Thread thread = new Thread(batchTaskService.Execute);
                //    thread.Start();
                //}
                //else
                {
                    batchTaskService.Execute();
                }
            }
        }
    }
}
