using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class GenerateTariffsHelper : BatchTaskExecutionsService
    {
        public GenerateTariffsHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
            GenerateTariffsArgs parameterArgs = serializer.Deserialize(stringReader) as GenerateTariffsArgs;

            ITariffModuleContext iContext = TariffModuleContext.GetContext(parameterArgs.Tenant);
            TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == parameterArgs.Tenant select d).FirstOrDefault();

            if (iTariffSetting != null)
            {
                Random random = new Random();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(parameterArgs.Tenant);

                ICommonDataContext context = CommonDataContext.GetContext(parameterArgs.Tenant);

                UserRepository userRepository = new UserRepository(context);
                User loggedUser = userRepository.GetSingleUserByEmail(parameterArgs.LoggedUserEmail, parameterArgs.Tenant, false);

                List<Card> airlines = context.Cards.Where(d => d.Tenant == parameterArgs.Tenant && d.PartnerTypeId == "AL").Take(20).ToList();
                List<string> currencyIds = context.Currencies.Where(d => d.Tenant == parameterArgs.Tenant).Select(s => s.Id).ToList();
                List<Port> ports = context.Ports.Where(d => d.Tenant == parameterArgs.Tenant).ToList();

                var myCount = 0;
                for (int i = 0; i < 20; i++)
                {
                    List<TariffVersion> versions = new List<TariffVersion>();

                    Card airline = airlines[random.Next(airlines.Count)];

                    Tariff tariff = new Tariff()
                    {
                        Id = IdCounter.GetNumber("Tariff", parameterArgs.Tenant),
                        TariffNumber = CodeCounter.GetNumber("Tariff", parameterArgs.Tenant).ToString(),
                        PriceSteps = iTariffSetting.DefaultPriceSteps,
                        Tenant = parameterArgs.Tenant,
                        CreateDate = todayDate,
                        UpdateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        UpdatedByUserId = loggedUser.Id,
                        SellerId = airline.Id,
                        Name = "Test Tariff " + i,
                        StartDate = todayDate.AddMonths(i),
                        ExpirationDate = todayDate.AddYears(1),
                        CurrencyId = currencyIds[random.Next(currencyIds.Count)],
                        LastVersion = 50,
                        TypeCode = "AFC",
                        SearchFields = "Test Tariff " + i + "," + airline.EnglishName,
                    };
                    myCount++;

                    TariffVersion draftVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 1,
                        IsDraft = true,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate,
                        ExpirationDate = todayDate.AddYears(1),
                        Tenant = parameterArgs.Tenant,
                    };
                    iContext.TariffVersions.Add(draftVersion);
                    myCount++;

                    TariffVersion activeVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 2,
                        IsDraft = false,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate,
                        ExpirationDate = todayDate.AddMonths(2),
                        Tenant = parameterArgs.Tenant,
                        ApproveDate = todayDate,
                        ApprovedByUserId = loggedUser.Id,
                        ParentVersionNumber = 1,
                    };
                    iContext.TariffVersions.Add(activeVersion);
                    myCount++;

                    versions.Add(draftVersion);
                    versions.Add(activeVersion);

                    for (int j = 3; j <= 50; j++)
                    {
                        TariffVersion version = new TariffVersion()
                        {
                            TariffId = tariff.Id,
                            Version = j,
                            IsDraft = false,
                            CreateDate = todayDate,
                            CreatedByUserId = loggedUser.Id,
                            StartDate = todayDate,
                            ExpirationDate = todayDate.AddMonths(-j),
                            Tenant = parameterArgs.Tenant,
                            ApproveDate = todayDate,
                            ApprovedByUserId = loggedUser.Id,
                            ParentVersionNumber = j - 1,
                        };
                        iContext.TariffVersions.Add(version);
                        myCount++;

                        versions.Add(version);
                    }

                    foreach (TariffVersion item in versions)
                    {
                        for (int k = 0; k < 50; k++)
                        {
                            Port fromPort = ports[random.Next(ports.Count)];
                            Port toPort = ports[random.Next(ports.Count)];
                            string[] steps = iTariffSetting.DefaultPriceSteps.Split(',');

                            TariffLine line = new TariffLine()
                            {
                                Id = IdCounter.GetNumber("TariffLine", parameterArgs.Tenant),
                                Tenant = parameterArgs.Tenant,
                                TariffId = tariff.Id,
                                Version = item.Version,
                                StartDate = item.StartDate,
                                ExpirationDate = item.ExpirationDate,
                                OriginPortId = fromPort.Id,
                                DestinationPortId = toPort.Id,
                                Index = k,
                                LineUniqueKey = fromPort.Code + "," + toPort.Code,
                                LineUniqueKeyText = fromPort.Code + "," + toPort.Code + k,
                                MinPrice = RandomDecimal(random),
                            };

                            for (var s = 1; s <= steps.Length; s++)
                            {
                                PropertyInfo propInfo = typeof(TariffLine).GetProperty("Step" + s + "Price");
                                propInfo.SetValue(line, RandomDecimal(random));
                            }
                            
                            myCount++;
                            iContext.TariffLines.Add(line);
                        }
                    }

                    TariffVersion lastVersion = versions.Where(d => d.Version == 50).FirstOrDefault();
                    tariff.LastStartDate = lastVersion.StartDate;
                    tariff.LastExpirationDate = lastVersion.ExpirationDate;
                    
                    iContext.Tariffs.Add(tariff);
                    airlines.Remove(airline);

                    //if (myCount == 1000)
                    //{
                    //    iContext.SaveChanges();
                    //    myCount = 0;
                    //}

                }

                iContext.SaveChanges();
            }
        }

        private decimal RandomDecimal(Random random)
        {
            int precision = random.Next(1, 3);
            int scale = random.Next(0, precision);

            Decimal d = 0m;
            for (int i = 0; i < precision; i++)
            {
                int r = random.Next(0, 10);
                d = d * 10m + r;
            }

            for (int s = 0; s < scale; s++)
            {
                d /= 10m;
            }

            return d;
        }
    }

    public class GenerateTariffsArgs
    {
        public int Tenant { get; set; }
        public string LoggedUserEmail { get; set; }
    }
}