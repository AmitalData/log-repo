using Logitude.Server.Tools.Counters;
using Microsoft.VisualBasic.FileIO;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    public partial class AddStates : Form
    {
        public AddStates()
        {
            InitializeComponent();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.InsertPortStates(this.GetStream(), "US");            
        }

        private void InsertPortStates(StreamReader streamReader,string countryCode)
        {
            if (streamReader != null)
            {

                List<StatesPorts> AllDataLines = new List<StatesPorts>();

                using (TextFieldParser csvParser = new TextFieldParser(streamReader))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    string[] columns = null;
                    while ((columns = csvParser.ReadFields()) != null)
                    {
                        string port = this.GetValue(columns, 0);
                      port= port.Substring(1);
                       port= port.Substring(0,port.Length-1);
                        string state = this.GetValue(columns, 1);
                        if (state != null)
                        {
                            state = state.Substring(0, state.Length - 1);
                        }
                        StatesPorts myItem = new StatesPorts();
                        myItem.Port = port;
                        myItem.State = state;
                        StatesPorts xItem = AllDataLines.Where(d => d.Port == port && myItem.State == state).FirstOrDefault();
                        if (xItem == null && myItem.Port!=null && myItem.State!=null)
                        {
                            AllDataLines.Add(myItem);
                        }
                    }
                    Thread thread = new Thread(() => this.RunAddingStatesPORTS(AllDataLines, countryCode));
                    thread.IsBackground = true;
                    thread.Start();

                }
            }
       
        }

        private void RunAddingStatesPORTS(List<StatesPorts> allDataLines, string countryCode)
        {
            
            if (countryCode == "US")
                SetControlPropertyValue(InsertStatesUSPortsLbl, "Text", "Updating ...");
            else if (countryCode == "MX")
                SetControlPropertyValue(InsertStatesMEXPortsLbl, "Text", "Updating ...");
            else if (countryCode == "CA")
                SetControlPropertyValue(InsertStatesCanadaPortsLbl, "Text", "Updating ...");
            else if (countryCode == "IN")
                SetControlPropertyValue(InsertStatesIndiaPortsLbl, "Text", "Updating ...");
            else if (countryCode == "AU")
                SetControlPropertyValue(InsertStatesAUSPortsLbl, "Text", "Updating ...");

            if (checkBox1.Checked)
            {
                

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(0);
                List<Tenant> tenants = myCommonContext.Tenants.ToList();
                if (tenants.Count > 0)
                {
                    foreach(Tenant tenantOBJ in tenants)
                    {
                        this.StartAddingStatesPorts(allDataLines, countryCode, tenantOBJ.Id);


                    }

                    if (countryCode == "US")
                        SetControlPropertyValue(InsertStatesUSPortsLbl, "Text", "Done All" );
                    else if (countryCode == "MX")
                        SetControlPropertyValue(InsertStatesMEXPortsLbl, "Text", "Done All");
                    else if (countryCode == "CA")
                        SetControlPropertyValue(InsertStatesCanadaPortsLbl, "Text", "Done All" );
                    else if (countryCode == "IN")
                        SetControlPropertyValue(InsertStatesIndiaPortsLbl, "Text", "Done All");
                    else if (countryCode == "AU")
                        SetControlPropertyValue(InsertStatesAUSPortsLbl, "Text", "Done All");



                }

            }
            else
            {
                int tenant = int.Parse(this.textBox1.Text);

                this.StartAddingStatesPorts(allDataLines, countryCode, tenant);
            }
           

        }

        private void StartAddingStatesPorts(List<StatesPorts> allDataLines,string countryCode,int tenant)
        {
            allDataLines = allDataLines.Where(d => !string.IsNullOrEmpty(d.Port)).ToList();
            if (allDataLines.Count > 0)
            {

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                Country country = myCommonContext.Countries.Where(p => p.Code == countryCode && p.Tenant == tenant).FirstOrDefault();
                if (country != null)
                {
                    var myCount = 0;
                    foreach (StatesPorts item in allDataLines)
                    {
                        Port portDB = myCommonContext.Ports.Where(p => p.Code == item.Port && p.Tenant == tenant && p.CountryId == country.Id).FirstOrDefault();
                        State state = null;
                        if (portDB != null)
                        {
                            state = myCommonContext.States.Where(p => p.Code == item.State && p.Tenant == tenant && p.CountryId==portDB.CountryId).FirstOrDefault();
                        }
                        if (state != null && portDB != null)
                        {
                            portDB.StateId = state.Id;
                            portDB.StateName = state.EnglishName;
                            myCommonContext.Ports.Attach(portDB);
                            myCommonContext.SetAsModified(portDB);
                        }
                        myCount++;
                        if (myCount > 500)
                        {
                            myCommonContext.SaveChanges();
                            myCount = 0;
                        }

                    }
                    myCommonContext.SaveChanges();

                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                if (countryCode == "US")
                    SetControlPropertyValue(InsertStatesUSPortsLbl, "Text", "Done in " + ts.ToString() + " Tenant" + tenant);
                else if (countryCode == "MX")
                    SetControlPropertyValue(InsertStatesMEXPortsLbl, "Text", "Done in " + ts.ToString() + " Tenant" + tenant);
                else if (countryCode == "CA")
                    SetControlPropertyValue(InsertStatesCanadaPortsLbl, "Text", "Done in " + ts.ToString() + " Tenant" + tenant);
                else if (countryCode == "IN")
                    SetControlPropertyValue(InsertStatesIndiaPortsLbl, "Text", "Done in " + ts.ToString() + " Tenant" + tenant);
                else if (countryCode == "AU")
                    SetControlPropertyValue(InsertStatesAUSPortsLbl, "Text", "Done in " + ts.ToString() + " Tenant" + tenant);


            }
        }
        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);


        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
                    this.InsertStates(this.GetStream());
                  
        }

        private StreamReader GetStream()
        {
            if (textBox1.Text.Trim() != "" || this.checkBox1.Checked)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "csv|*.csv";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Stream stream = dialog.OpenFile();
                    StreamReader streamReader = new StreamReader(stream);
                    return streamReader;                   
                }
            }
            else
            {
                MessageBox.Show("Please fill the Tenant Number first ! .");
            }
            return null;
        }

        private string TrimString(string myString)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(myString))
            {
                myResult = myString.Trim();
            }

            return myResult;
        }



        private string GetValue(string[] columns, int index)
        {
            string myResult = null;

            if (columns != null)
            {
                if (columns.Count() >= index + 1)
                {
                    myResult = this.TrimString(columns[index]);

                    if (myResult == "NULL")
                    {
                        myResult = null;
                    }
                }
            }

            return myResult;
        }

        private void InsertStates(StreamReader streamReader)
        {
            if (streamReader != null)
            {
                List<StatesPorts> AllDataLines = new List<StatesPorts>();

                using (TextFieldParser csvParser = new TextFieldParser(streamReader))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    string[] columns = null;
                    while ((columns = csvParser.ReadFields()) != null)
                    {

                        string port = this.GetValue(columns, 0);
                        string state = this.GetValue(columns, 1);



                        StatesPorts myItem = new StatesPorts();
                        myItem.Port = port;
                        myItem.State = state;
                        StatesPorts xItem = AllDataLines.Where(d => d.Port == port && myItem.State == state).FirstOrDefault();
                        if (xItem == null)
                        {
                            AllDataLines.Add(myItem);
                        }
                    }
                    Thread thread = new Thread(() => this.RunAddingStates(AllDataLines, "IndiaStates"));
                    thread.IsBackground = true;
                    thread.Start();

                }
            }
        }

        private void RunAddingStates(List<StatesPorts> allDataLines, string v)
        {

            allDataLines = allDataLines.Where(d => !string.IsNullOrEmpty(d.State)).ToList();
            int tenant = int.Parse(this.textBox1.Text);
            if (allDataLines.Count > 0)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                Country country = myCommonContext.Countries.Where(p => p.Code == "IN" && p.Tenant == tenant).FirstOrDefault();
                if (country != null)
                {
                    var myCount = 0;
                    foreach (StatesPorts item in allDataLines)
                    {
                        State stateDB = myCommonContext.States.Where(p => p.Code == item.Port && p.Tenant == tenant && p.CountryId==country.Id).FirstOrDefault();
                        if (stateDB == null)
                        {
                            State state = new State();
                            state.Code = item.Port;
                            state.EnglishName = item.State;
                            state.LocalName = state.LocalName;
                            state.CountryId = country.Id;
                            state.Tenant = tenant;
                            state.SearchFields = state.Code + "," + state.EnglishName + ",";
                            state.Id = IdCounter.GetNumber("State", tenant).ToString();
                            myCommonContext.States.Add(state);
                        }
                        myCount++;
                        if (myCount > 500)
                        {
                            myCommonContext.SaveChanges();
                        }

                    }
                }
                myCommonContext.SaveChanges();
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                    SetControlPropertyValue(InsertStatesTenantZeroLbl, "Text", "Done in " + ts.ToString());
            }

            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.InsertPortStates(this.GetStream(), "AU");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.InsertPortStates(this.GetStream(), "MX");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.InsertPortStates(this.GetStream(), "CA");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.InsertPortStates(this.GetStream(), "IN");
        }
    }
    public class StatesPorts
    {
        public string State { get; set; }
        public string Port { get; set; }
    }
}

  


