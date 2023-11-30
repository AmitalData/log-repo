using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace External_API_Load_Testing
{
    public partial class Form1 : Form
    {
        string Token;

        public Form1()
        {
            InitializeComponent();
        }

        private bool isConnected;
        private async void LoginWithCredentials()
        {
            try
            {
                isConnected = false;

                if (!isConnected)
                {
                    APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
                    {
                        PrimaryKey = txtCredentialsPrimary.Text,
                    };

                    using (var client = new HttpClient())
                    {
                        string AuthURI = txtServerUrl.Text + "APIAuthentication";
                        var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                        var result = await client.PostAsync(AuthURI, content);
                        var tempUser = result.Content.ReadAsStringAsync().Result;
                        ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
                        Token = User.Token;

                        if (User.HasError)
                        {
                            lblMessage.Text = "Authentication Error!";
                            lblMessage.ForeColor = Color.Red;
                        }

                        else
                        {
                            lblMessage.Text = "Connected!";
                            lblMessage.ForeColor = Color.Green;
                            isConnected = true;
                            create.Enabled = true;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = "Authentication Error: " + ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.LoginWithCredentials();
        }

        private async void CallEntityApi()
        {
            if (!string.IsNullOrEmpty(JsonText.Text) && !string.IsNullOrEmpty(NumberOfShipments.Text) && !string.IsNullOrEmpty(PeriodMin.Text))
            {
                for (int i = 0; i < int.Parse(NumberOfShipments.Text); i++)
                {
                    await PeriodByMinutes(i);
                    if (!string.IsNullOrEmpty(this.Token))
                    {
                        await CreateShipment();
                    }
                }
            }
            else
            {
                createErrorMsg.Text = "Must fill all the requierd fields";
                createErrorMsg.ForeColor = Color.Red;
            }
        }

        private async Task CreateShipment()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 10, 0);

                    client.DefaultRequestHeaders.Add("Token", Token);

                    var content = new StringContent(JsonText.Text, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await client.PostAsync(txtServerUrl.Text + "/" + "direct", content);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var resultData = response.Content.ReadAsStringAsync().Result;
                        CreatedShipment.Text = resultData;
                    }
                    else
                    {
                        CreatedShipment.Text = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                createErrorMsg.Text = "Creation Error: " + ex.Message;
                createErrorMsg.ForeColor = Color.Red;
            }

        }

        private async Task PeriodByMinutes(int index)
        {
            int msToMin = 1000; 
            if (index != 0)
            {
                await Task.Delay((int.Parse(PeriodMin.Text)) * msToMin);
            }
        }

        private void create_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                this.CallEntityApi();
            }
        }
    }
}
