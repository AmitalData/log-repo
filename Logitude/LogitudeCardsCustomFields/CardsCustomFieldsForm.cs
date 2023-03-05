using LogitudeCardsCustomFields.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogitudeCardsCustomFields
{
    public partial class CardsCustomFieldsForm : Form
    {
        public CardsCustomFieldsForm()
        {
            InitializeComponent();
            this.BuildCardsObjectFieldsCheckBox.Checked = true;
            this.GenerateMetaDataScriptsCheckBox.Checked = true;
        }

        private void BuildCardsCustomFieldsBtn_Click(object sender, EventArgs e)
        {
            ChangeUIProperties(false);
            this.MetaDataTimeLabel.Text = "00:00:00.00";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread thread = new Thread(() => BuildCardsCustomFields())
            {
                IsBackground = true
            };
            thread.Start();
            this.MetaDataTimeLabel.Text = "In Progress..";
            thread.Join();
            stopWatch.Stop();
            this.MetaDataTimeLabel.Text = GetFormatedElapsedTime(stopWatch.Elapsed);
            ChangeUIProperties(true);
        }

        private void BuildCardsCustomFields()
        {
            try
            {
                CardsCustomFieldsMetaDataBuilder cardsCustomFieldsMetaDataBuilder = new CardsCustomFieldsMetaDataBuilder(this.BuildCardsObjectFieldsCheckBox.Checked, this.GenerateMetaDataScriptsCheckBox.Checked);
                cardsCustomFieldsMetaDataBuilder.Build();
            }
            catch (Exception exception)
            {
                string exceptionMessage = exception.Message + (exception.InnerException != null ? exception.InnerException.ToString() : "");
                if (exceptionMessage.Length > 1500) exceptionMessage = exceptionMessage.Substring(0, 1500);
                MessageBox.Show(exceptionMessage);
            }
        }

        private void BuildCardsCustomFieldsDataButton_Click(object sender, EventArgs e)
        {
            ChangeUIProperties(false);
            this.DataTimeLabel.Text = "00:00:00.00";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread thread = new Thread(() => BuildCardsCustomFieldsData())
            {
                IsBackground = true
            };
            thread.Start();
            this.DataTimeLabel.Text = "In Progress..";
            thread.Join();
            stopWatch.Stop();
            this.DataTimeLabel.Text = GetFormatedElapsedTime(stopWatch.Elapsed);
            ChangeUIProperties(true);
        }

        private void BuildCardsCustomFieldsData()
        {
            try
            {
                CardsCustomFieldsDataBuilder cardsCustomFieldsDataBuilder = new CardsCustomFieldsDataBuilder();
                cardsCustomFieldsDataBuilder.Build();
            }
            catch (Exception exception)
            {
                string exceptionMessage = exception.Message + (exception.InnerException != null ? exception.InnerException.ToString() : "");
                if (exceptionMessage.Length > 1500) exceptionMessage = exceptionMessage.Substring(0, 1500);
                MessageBox.Show(exceptionMessage);
            }
        }

        private void ChangeUIProperties(bool isEnabled)
        {
            this.BuildCardsCustomFieldsBtn.Enabled = isEnabled;
            this.GenerateMetaDataScriptsCheckBox.Enabled = isEnabled;
            this.BuildCardsCustomFieldsDataButton.Enabled = isEnabled;
            this.BuildCardsObjectFieldsCheckBox.Enabled = isEnabled;
        }

        private string GetFormatedElapsedTime(TimeSpan timeSpan)
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                timeSpan.Milliseconds / 10);
            return elapsedTime;
        }
    }
}
