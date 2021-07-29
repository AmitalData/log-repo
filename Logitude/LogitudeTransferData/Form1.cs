using Confluent.Kafka;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.KafkaConfigurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace LogitudeTransferData
{
    public partial class Form1 : Form
    {
        static string brokerList = ConfigurationManager.AppSettings["EH_FQDN"];
        static string connectionString = ConfigurationManager.AppSettings["EH_CONNECTION_STRING"];

        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = brokerList,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = "$ConnectionString",
            SaslPassword = connectionString
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<CardPM> cardPMs = GetAllCards(tenant);

            ProduceKafkaMessages<CardPM>(cardPMs, KakaMessageTypes.Card);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<ContactPM> contactPMs = GetAllContacts(tenant);

            ProduceKafkaMessages<ContactPM>(contactPMs, KakaMessageTypes.Contact);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<CountryPM> countryPMs = GetAllCountries(tenant);

            ProduceKafkaMessages<CountryPM>(countryPMs, KakaMessageTypes.Country);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<PortPM> portPMs = GetAllPorts(tenant);

            ProduceKafkaMessages<PortPM>(portPMs, KakaMessageTypes.Port);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<VesselPM> vesselPMs = GetAllVessels(tenant);

            ProduceKafkaMessages<VesselPM>(vesselPMs, KakaMessageTypes.Vessel);
        }

        private List<VesselPM> GetAllVessels(int tenant)
        {
            VesselQuery vesselQuery = new VesselQuery(tenant);
            List<VesselPM> vesselPMs = vesselQuery.GetVesselPMsByTenant(tenant).ToList();
            return vesselPMs;
        }

        private List<CardPM> GetAllCards(int tenant)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            List<CardPM> cardPMs = cardQuery.GetAllCardPMsByTenant(tenant);
            return cardPMs;
        }

        private List<ContactPM> GetAllContacts(int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            List<ContactPM> contactPMs = contactQuery.GetContactPMsWithoutPassWordsByTenant(tenant);
            return contactPMs;
        }

        private List<CountryPM> GetAllCountries(int tenant)
        {
            CountryQuery countryQuery = new CountryQuery(tenant);
            List<CountryPM> countryPMs = countryQuery.GetCountryPMsByTenant(tenant).ToList();
            return countryPMs;
        }

        private List<PortPM> GetAllPorts(int tenant)
        {
            PortQuery portQuery = new PortQuery(tenant);
            List<PortPM> portPMs = portQuery.GetPortPMsByTenant(tenant).ToList();
            return portPMs;
        }

        private void ProduceKafkaMessages<T>(List<T> PMs, long kakaMessageTypes)
        {
            try
            {
                int counter = 0;
                using (var producer = new ProducerBuilder<long, string>(config)
                    .SetKeySerializer(Serializers.Int64)
                    .SetValueSerializer(Serializers.Utf8)
                    .Build())
                {
                    foreach (T PM in PMs)
                    {
                        counter++;
                        var serializedContact = JsonConvert.SerializeObject(PM, Formatting.Indented);
                        var deliveryReport = producer.ProduceAsync(KafkaTopics.LookupsTopic, new Message<long, string> { Key = kakaMessageTypes, Value = serializedContact });
                        deliveryReport.Wait();
                        Console.WriteLine($"Upsert Lookup: {counter}");
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
