using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQSRV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "RabbitMQ Service";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
            ////GWSFLOGITUDE > GGGFRABBITMQ
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{

            //}
            this.textBoxDate.Text = DateTime.Now.ToString();
            this.textBoxCompany.Text = "Mentfield Logistics";
            this.textBoxDB.Text = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"]?.ConnectionString;


            //this.textBoxCompany.Text = 

        }

        private void ate_Click(object sender, EventArgs e)
        {

        }
        List<Task> _Tasks = new List<Task>();
        private void btnStart_Click(object sender, EventArgs e)
        {
            int taskNo = -99;
            if (!int.TryParse(textBoxThreads.Text, out taskNo)){
                textBoxLog.Text="No.Threads is not valid number";
                return;
            }
            if (taskNo==1)
            {
                var rabbitMQReceiveWR = new RabbitMQReceiveWR();
                rabbitMQReceiveWR.Run();
                return;
            }
            WorkerRoleServiceLocator.PleaseShutDown = false;
            for (int i = 0; i < taskNo; i++)
            {
                _Tasks.Add(
                Task.Run(() =>
                {
                    var rabbitMQReceiveWR = new RabbitMQReceiveWR();
                    rabbitMQReceiveWR.Run();
                })
                );
                textBoxLog.Text += $"Thread {i + 1} started" + Environment.NewLine;
            }
            textBoxLog.Text += $"Working.." + Environment.NewLine;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            WorkerRoleServiceLocator.PleaseShutDown = true;
            textBoxLog.Text += $"PleaseShutDown.." + Environment.NewLine;
            Task.WaitAll(_Tasks.ToArray());
            textBoxLog.Text += $"ShutDown!!";
            textBoxLog.Text = "";
            _Tasks.Clear();
            
        }
    }
}
