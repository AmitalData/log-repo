using Logitude.DashboardModule.MetaDataTool.Models;
using Logitude.DashboardModule.MetaDataTool.Models.FieldModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logitude.DashboardModule.MetaDataTool.Helpers
{
    public class JsonHelper
    {
        public static void GenerateJsonFileFromTool(AnalyticsFactsMetaDataViewModel table)
        {
            AnalyticsFactsMetaData analyticsFactsMetaData = CleanUnwantedProp(table);
            analyticsFactsMetaData.HashString = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(App.DirectOpenPath))
            {
                MessageBox.Show("file path in not valid!");
                return;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(analyticsFactsMetaData, Formatting.Indented, settings);
            File.WriteAllText(App.DirectOpenPath, json);
        }

        public static AnalyticsFactsMetaData CleanUnwantedProp(AnalyticsFactsMetaDataViewModel table)
        {
            table.AnalyticsFactsFieldsMetaDatas = new List<AnalyticsFactsFieldsMetaData>();
            foreach (var item in table.AnalyticsFactsFieldsMetaDataViewModels)
            {
                table.AnalyticsFactsFieldsMetaDatas.Add(JsonConvert.DeserializeObject<AnalyticsFactsFieldsMetaData>(JsonConvert.SerializeObject(item)));
            }
            return JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(table));
        }

        public static AnalyticsFactsMetaDataViewModel GetAnalyticsFactsMetaDataViewModel(string fileName)
        {
            try
            {
                AnalyticsFactsMetaDataViewModel analyticsFactsMetaDataView;
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    analyticsFactsMetaDataView = JsonConvert.DeserializeObject<AnalyticsFactsMetaDataViewModel>(json);
                }
                analyticsFactsMetaDataView.BuildObsList();
                return analyticsFactsMetaDataView;
            }
            catch (Exception)
            {
                return new AnalyticsFactsMetaDataViewModel();
            }

        }

    }
}
