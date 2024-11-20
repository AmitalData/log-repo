

using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.CheckHealthHelper;

namespace Logitude.Customs.BL.BL
{
    public class CheckHealthService
    {
        public void CheckHealth()
        {
            try
            {
                CheckHealthRepository checkHealthHelper = new CheckHealthRepository();
                 checkHealthHelper.CheckHealth();
            }
            catch (Exception e)
            {
                throw new Exception("Health check failed", e);
            }
        }
        //public async Task<ActionResult> Index()
        //{
           

        //    if (cpuPercentage > 80)
        //    {
        //        Response.StatusCode = 500;
        //        Response.StatusDescription = "Machine is not healthy";
        //    }
        //    else
        //    {
        //        Response.StatusCode = 200;
        //        Response.StatusDescription = "OK";
        //    }

        //    Response.Flush();
        //    Response.End();
        //    return null;
        //}

        public async Task<double> GetCpuPercentageAsync()
        {
            var resourceId = Environment.GetEnvironmentVariable("ResourceId") ;

            var cred = new DefaultAzureCredential();
            var monitor = new MetricsQueryClient(cred);

            var options = new MetricsQueryOptions()
            {
                TimeRange = TimeSpan.FromMinutes(5),
                Granularity = TimeSpan.FromMinutes(1),
                MetricNamespace = "microsoft.web/serverfarms",
                Filter = $"Instance eq '{Environment.MachineName}'"
            };
            options.Aggregations.Add(MetricAggregationType.Average);
            var cpuResponse = await monitor.QueryResourceAsync(
                resourceId,
                new[] { "CpuPercentage" },
                options);

            var metric = cpuResponse.Value.Metrics.FirstOrDefault();
            return metric?.TimeSeries.Max(m => m.Values.Max(mm => mm.Average)) ?? 0;
        }

    }
}
