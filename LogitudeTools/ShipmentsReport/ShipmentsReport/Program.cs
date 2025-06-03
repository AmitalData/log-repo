using CsvHelper;
using Microsoft.Data.SqlClient;
using ShipmentsReport;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using NetCommonHelper.Logger;

namespace sqltest
{
    class Program
    {
        static readonly DevLog logger = DevLog.Instance;

        static void Main(string[] args)
        {
            try
            {
                logger.WriteInfo("ShipmentsReport started");

                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                IConfigurationSection? dbSettings = config.GetRequiredSection("Settings:db");
                IConfigurationSection? emailSettings = config.GetRequiredSection("Settings:email");
                IConfigurationSection? querySettings = config.GetRequiredSection("Settings:query");

                EmailService? es = new EmailService(emailSettings["smtp"], emailSettings["userName"], emailSettings["password"]);

                List<ReportLine> report = new List<ReportLine>();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = dbSettings["connectionString"];
                builder.UserID = dbSettings["userName"];
                builder.Password = dbSettings["password"];
                builder.InitialCatalog = dbSettings["initialCatalog"];


                string[] recipients = emailSettings.GetSection("recipients").Get<string[]>();

                logger.WriteDebug($"finish init configuration, Connecting to database: {builder.DataSource}, User: {builder.UserID}, Catalog: {builder.InitialCatalog}");
                logger.WriteDebug("starting to query database");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    DateTime previousMonth = DateTime.Now.Date.AddMonths(-1);
                    int day = emailSettings.GetValue<int>("day");
                    DateTime fromDate = new DateTime(previousMonth.Year, previousMonth.Month, day);
                    DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);

                    connection.Open();
                    string commandText = "SELECT t.Id AS 'Tenant', t.Company, COUNT(s.Id) AS 'Amount' FROM Tenants AS t LEFT JOIN Shipments AS s ON t.Id = s.Tenant AND s.CreateDateTime >= @fromDate AND s.CreateDateTime <= @toDate WHERE t.Id IN ({0}) GROUP BY t.Id, t.Company";
                    string[]? tenants = querySettings.GetSection("tenants").Get<string[]>();
                    commandText = string.Format(commandText, string.Join(',', tenants));
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@fromDate", SqlDbType.Date);
                    command.Parameters["@fromDate"].Value = fromDate;
                    command.Parameters.Add("@toDate", SqlDbType.Date);
                    command.Parameters["@toDate"].Value = toDate;
                    using (command)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                report.Add(new ReportLine
                                {
                                    Tenant = reader.GetInt32(0),
                                    Company = reader.GetString(1),
                                    Amount = reader.GetInt32(2)
                                });
                            }

                            logger.WriteDebug("query database finished, reports:");
                            report.ForEach(line => logger.WriteDebug($"Tenant: {line.Tenant}, Company: {line.Company}, Amount: {line.Amount}"));
                            logger.WriteDebug("starting to create csv");

                            using (var mem = new MemoryStream())
                            using (var writer = new StreamWriter(mem))
                            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csvWriter.WriteRecords(report);
                                writer.Flush();
                                
                                logger.WriteDebug("csv created, starting to send email");

                                mem.Position = 0;
                                Attachment attachment = new Attachment(mem, new ContentType("text/csv"));
                                attachment.Name = string.Format(emailSettings["attachmentName"], DateTime.Now.ToShortDateString());
                                es.Send(recipients.ToList(), emailSettings["subject"], "", attachment);
                            }
                        }
                    }
                }

                logger.WriteInfo("ShipmentsReport finished successfully");
            }
            catch (SqlException e)
            {
                logger.WriteFatal(e, "send shipment report failed");
            }
        }
    }

    public class ReportLine
    {
        public int Tenant { get; set; }
        public string Company { get; set; }
        public int Amount { get; set; }
    }
}