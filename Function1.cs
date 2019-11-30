using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace When_is_my_birthday
{
    public static class Function1
    {
        [FunctionName("When-is-my-bitrhday")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, ILogger log)
        {
            if (req.Method.ToString() == "POST")
            {
                try
                {
                    var data = req.Content.ReadAsAsync<DialogFlowRequest>().Result;
                    var age = data.queryResult.parameters.number;

                    var Datetime = DateTime.Now;
                    var Year = Datetime.Year;
                    var borned_year1 = Year - age;
                    var borned_year2 = borned_year1--;

                    var ResponceObject = new DialogFlowResponce();

                    var wareki_1 = Wareki(age, borned_year1);
                    var wareki_2 = Wareki(age, borned_year2);

                    ResponceObject.fulfillmentText = $"{borned_year1}年({wareki_1})、もしくは{borned_year2}年({wareki_2})です。";
                    string json = JsonConvert.SerializeObject(ResponceObject);

                    var ReturnObject = new ObjectResult(json);
                    return ReturnObject;

                }
                catch (Exception)
                {
                    var ResponceObject = new DialogFlowResponce();

                    ResponceObject.fulfillmentText = $"ごめんなさい計算できません。";
                    string json = JsonConvert.SerializeObject(ResponceObject);

                    var ReturnObject = new ObjectResult(json);
                    return ReturnObject; ;
                }
            }
            else
            {
                return new ObjectResult("");
            }

        }
        public static string Wareki(float age, float year)
        {
            var culture = new CultureInfo("ja-JP", true);
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();

            string strTime_1 = year + "/01/01";
            string strTime_2 = year + "/12/13";

            var data_1 = DateTime.Parse(strTime_1);
            var data_2 = DateTime.Parse(strTime_2);

            var borned_year_1 = data_1.ToString("ggyy年", culture);
            var borned_year_2 = data_2.ToString("ggyy年", culture);

            if (borned_year_1 == borned_year_2)
            {
                return borned_year_1;
            }
            else
            {
                return borned_year_1 + "," + borned_year_2;
            }
        }
        class DialogFlowResponce
        {
            public string fulfillmentText { get; set; }
        }
    }
}
