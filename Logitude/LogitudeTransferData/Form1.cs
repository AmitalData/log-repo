using Confluent.Kafka;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.KafkaConfigurations;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.Repositories;
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

        private void button6_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<DocumentTypePM> documentTypePMs = GetAllDocumentTypes(tenant);

            ProduceKafkaMessages<DocumentTypePM>(documentTypePMs, KakaMessageTypes.DocumentType);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<CurrencyPM> currencyPMs = GetAllCurrencies(tenant);

            ProduceKafkaMessages<CurrencyPM>(currencyPMs, KakaMessageTypes.Currency);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<SpecialServicesTypePM> specialServicesTypePMs = GetAllSpecialServicesTypes(tenant);

            ProduceKafkaMessages<SpecialServicesTypePM>(specialServicesTypePMs, KakaMessageTypes.SpecialServicesType);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<EntityStatusPM> entityStatusPMs = GetAllEntityStatus(tenant);

            ProduceKafkaMessages<EntityStatusPM>(entityStatusPMs, KakaMessageTypes.EntityStatus);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<PackageTypePM> packageTypePMs = GetAllPackageTypes(tenant);

            ProduceKafkaMessages<PackageTypePM>(packageTypePMs, KakaMessageTypes.PackageType);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(textBox1.Text);
            List<ObjectFieldPM> AllObjectFieldPMs = GetAllCustomObjectFields(tenant);
            // Here we get all CustomFields except Lookup and PickList types
            List<ObjectFieldPM> notPickListObjectFieldPMs = AllObjectFieldPMs.FindAll(o => o.DataTypeCode != "PickList");
            ProduceKafkaMessages<ObjectFieldPM>(notPickListObjectFieldPMs, KakaMessageTypes.CustomField);

            // Here we want to get CustomPickList values
            List<CustomPickListPM> customPickListPMs = GetAllCustomPickLists(tenant);
            ProduceKafkaMessages<CustomPickListPM>(customPickListPMs, KakaMessageTypes.CustomPickList);

            // Then send PickList object fields
            List<ObjectFieldPM> pickListObjectFieldPMs = AllObjectFieldPMs.FindAll(o => o.DataTypeCode == "PickList");
            ProduceKafkaMessages<ObjectFieldPM>(pickListObjectFieldPMs, KakaMessageTypes.CustomField);
        }

        private List<CustomPickListPM> GetAllCustomPickLists(int tenant)
        {
            CustomPickListQuery customPickListQuery = new CustomPickListQuery(tenant);
            List<CustomPickListPM> customPickListPMs = customPickListQuery.GetCustomPickListPMsByTenant(tenant).ToList();
            return customPickListPMs;
        }

        private List<ObjectFieldPM> GetAllCustomObjectFields(int tenant)
        {
            ObjectTablePM shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
            string shipmentObjectId = shipmentObject.Id;

            ObjectFieldRepository ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            ObjectFieldQuery objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            List<ObjectFieldPM> objectFieldPMs = objectFieldsQuery.GetCustomFieldsByTableIdForCTool(shipmentObjectId, tenant).ToList();
            return objectFieldPMs;
        }

        private List<PackageTypePM> GetAllPackageTypes(int tenant)
        {
            PackageTypeQuery packageTypeQuery = new PackageTypeQuery(tenant);
            List<PackageTypePM> packageTypePMs = packageTypeQuery.GetPackageTypePMsByTenant(tenant).ToList();
            return packageTypePMs;
        }

        private List<CurrencyPM> GetAllCurrencies(int tenant)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            List<CurrencyPM> currencyPMs = currencyQuery.GetCurrencyPMsByTenant(tenant).ToList();
            return currencyPMs;
        }
        private List<EntityStatusPM> GetAllEntityStatus(int tenant)
        {
            EntityStatusQuery entityStatusQuery = new EntityStatusQuery(tenant);
            List<EntityStatusPM> entityStatusPMs = entityStatusQuery.GetEntityStatusPMsByTenant(tenant).ToList();
            return entityStatusPMs;
        }
        private List<SpecialServicesTypePM> GetAllSpecialServicesTypes(int tenant)
        {
            SpecialServicesTypeQuery specialServicesTypeQuery = new SpecialServicesTypeQuery(tenant);
            List<SpecialServicesTypePM> specialServicesTypePMs = specialServicesTypeQuery.GetSpecialServicesTypePMsByTenant(tenant).ToList();
            return specialServicesTypePMs;
        }

        private List<DocumentTypePM> GetAllDocumentTypes(int tenant)
        {
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypePM> documentTypePMs = documentTypeQuery.GetDocumentTypePMsByTenant(tenant)
                                                                    .Where(d => d.IsDocIn.Equals(true))
                                                                    .ToList();
            return documentTypePMs;
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
