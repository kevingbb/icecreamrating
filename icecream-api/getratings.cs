using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace icecream_api
{

    public class RatingEntity : TableEntity
    {
        public string Text { get; set; }
    }
    public static class getratings
    {
        [FunctionName("getratings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getratings/{userId}")] HttpRequest req,
            string userId,
            [Table("ratings") , StorageAccount("AzureWebJobsStorage")] CloudTable ratings,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation($"User ID: {userId}");
            log.LogInformation($"Body: {data}");

            string responseMessage = String.Empty;

            // Retrieve Ratings & Build Array of Results
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId);
            TableQuerySegment<RatingEntity> querySegment = await ratings.ExecuteQuerySegmentedAsync(new TableQuery<RatingEntity>().Where(query), null);
            if (querySegment.Results.Count > 0)
            {
                foreach (RatingEntity item in querySegment)
                {
                    log.LogInformation($"Data loaded: '{item.PartitionKey}' | '{item.RowKey}' | '{item.Text}'");
                    if (String.IsNullOrEmpty(builder.ToString()))
                    {
                        builder.Append("[");
                    }
                    else
                    {
                        builder.Append(",");
                    }
                    builder.Append(item.Text);
                }
                builder.Append("]");

                responseMessage = $"{builder.ToString()}";
                return new OkObjectResult(responseMessage);
            }
            else
            {
                responseMessage = $"No ratings for userId {userId} found.";
                return new NotFoundObjectResult(responseMessage);
            }
        }
    }
}
