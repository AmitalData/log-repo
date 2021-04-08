using Confluent.Kafka;
using LogitudeTransferData.Models;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LogitudeTransferData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);

            ContactRepository contactRep = new ContactRepository(tenant);

            IQueryable<Contact> contacts = contactRep.GetActiveContacts(tenant);

            List<CTUser> usersLists = (from c in contacts
                                                    select new CTUser()
                                                    {
                                                        Tenant = c.Tenant,
                                                        FirstName = c.EnglishName,
                                                        LastName = c.EnglishName,
                                                        Email = c.Email,
                                                        Password = c.Email
                                                    }).ToList();

            string brokerList = ConfigurationManager.AppSettings["EH_FQDN"];
            string connectionString = ConfigurationManager.AppSettings["EH_CONNECTION_STRING"];
            string topic = ConfigurationManager.AppSettings["EH_Topic"];
            string caCertLocation = ConfigurationManager.AppSettings["CA_CERT_LOCATION"];

            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = brokerList,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.Plain,
                    SaslUsername = "$ConnectionString",
                    SaslPassword = connectionString,
                    SslCaLocation = caCertLocation,
                };
                using (var producer = new ProducerBuilder<long, string>(config).SetKeySerializer(Serializers.Int64).SetValueSerializer(Serializers.Utf8).Build())
                {
                    foreach (CTUser cTUser in usersLists)
                    {
                        if (cTUser.Email != null)
                        {
                            var serializedUser = JsonConvert.SerializeObject(cTUser, Formatting.Indented);
                            var deliveryReport = producer.ProduceAsync(topic, new Message<long, string> { Key = DateTime.UtcNow.Ticks, Value = serializedUser });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception Occurred - {0}", ex.Message));
            }
        }
    }
}
