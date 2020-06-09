using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace icecream_api
{
    public static class getrating
    {
        [FunctionName("getrating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getrating/{userid}/{ratingId}")] HttpRequest req,
            string userId,
            string ratingId,
            [Table("ratings", "{userId}", "{ratingId}"), StorageAccount("AzureWebJobsStorage")] RatingTable rating,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation($"User ID: {userId}");
            log.LogInformation($"Rating ID: {ratingId}");
            log.LogInformation($"Body: {data}");

            string responseMessage = String.Empty;

            if (rating == null)
            {
                responseMessage = $"Rating with id {ratingId} not found.";
                return new NotFoundObjectResult(responseMessage);
            }
            else
            {
                responseMessage = $"{rating.Text}";
                return new OkObjectResult(responseMessage);
            }
        }
    }
}
