using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.Resolvers;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
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
using WebFreight.Web.AccountingModel;

namespace Logitude.Update
{
    public partial class BatchTaskTester : Form
    {
        public BatchTaskTester()
        {
            InitializeComponent();
            RegisterUtils();
        }

        private void RegisterUtils()
        {
            LoggedContactResolver.RegisterLoggedContactUtil();
            DateTimeUtilResolver.RegisterDateTimeUtil();
            TranslateTextsClassUtilResolver.RegisterTranslateTextsClassUtil();
            IdCounterUtilResolver.RegisterIdCounterUtil();


            AccountingRegistrations.Register();
            InjectionContainer.Container.RegisterType<IObjectTablePropertyGetter, ObjectTablePropertyGetter>("ObjectTablePropertyGetter", new InjectionFactory(c => new ObjectTablePropertyGetter()));

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            List<string> textBoxesValidations = this.ValidateTextBoxes();
            if (textBoxesValidations.Count > 0)
            {
                FillMessagesBox(textBoxesValidations);
                return;
            }

            BatchTaskExecutionPM batchTaskExecutionPM = GetBatchTaskExecutionPM();

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
                batchTaskService.Execute();

                BatchTaskExecutionPM finalResult= GetBatchTaskExecutionPM();
                List<string> messages = new List<string>();
                messages.Add(finalResult.StatusName);
                messages.Add(finalResult.ErrorLog);
                FillMessagesBox(messages);

            }
        }

        private BatchTaskExecutionPM GetBatchTaskExecutionPM()
        {
            string batchTaskId = BatchTaskIdTextBox.Text;
            int tenant;
            int.TryParse(TenantTextBox.Text, out tenant);

            BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
            BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(batchTaskId, false, false);
            return batchTaskExecutionPM;
        }
        private List<string> ValidateTextBoxes()
        {
            List<string> validationMessages = new List<string>();
            if (string.IsNullOrEmpty(BatchTaskIdTextBox.Text))
            {
                validationMessages.Add("Please fill the batch task id");
            }
            if(string.IsNullOrEmpty(TenantTextBox.Text))
            {
                validationMessages.Add("Please fill the tenant");
            }
            return validationMessages;
        }

        private void FillMessagesBox(List<string> messages)
        {
            MessagesTextBox.Clear();
            if (messages.Count > 0)
            {
                foreach (string message in messages)
                {
                    MessagesTextBox.AppendText(message + Environment.NewLine);
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
