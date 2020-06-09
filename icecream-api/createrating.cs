using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace icecream_api
{
    public static class createrating
    {
        [FunctionName("createrating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createrating")] HttpRequest req,
            [Table("ratings"),StorageAccount("AzureWebJobsStorage")] ICollector<RatingTable> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation($"Body: {data}");
            string userId = data?.userId;
            string productId = data?.productId;
            log.LogInformation($"User ID: {userId}");
            log.LogInformation($"Product ID: {productId}");

            HttpClient httpClient = HttpClientFactory.Create();
            string responseMessage = String.Empty;

            // Check User ID
            HttpResponseMessage userIdResponse = await httpClient.GetAsync($"https://serverlessohuser.trafficmanager.net/api/GetUser?userid={userId}");
            if (userIdResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                log.LogInformation($"User ID Response Status Code: {userIdResponse.StatusCode}");
                responseMessage = await userIdResponse.Content.ReadAsStringAsync();
                log.LogInformation($"User ID Response Message: {responseMessage}");
                return new NotFoundObjectResult(responseMessage);
            }
            log.LogInformation("User ID check completed.");

            // Check Product ID
            HttpResponseMessage productIdResponse = await httpClient.GetAsync($"https://serverlessohproduct.trafficmanager.net/api/GetProduct?productid={productId}");
            if (productIdResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                log.LogInformation($"Product ID Response Status Code: {productIdResponse.StatusCode}");
                responseMessage = await productIdResponse.Content.ReadAsStringAsync();
                log.LogInformation($"Product ID Response Message: {responseMessage}");
                return new NotFoundObjectResult(responseMessage);

            }
            log.LogInformation("Product ID check completed.");

            // Check rating is Between 1 and 5
            if (data?.rating < 1 || data?.rating > 5)
            {
                responseMessage = "Rating needs to be between 1 and 5.";
                log.LogInformation($"{responseMessage}");
                return new BadRequestObjectResult(responseMessage);
            }
            log.LogInformation("Rating check completed.");

            // Add Additional Fields
            data.id = Guid.NewGuid();
            data.timestamp = System.DateTime.UtcNow;

            // Add Rating to DB
            try 
            {
                RatingTable ratingTable = new RatingTable();
                ratingTable.PartitionKey = data.userId;
                ratingTable.RowKey = data.id;
                ratingTable.Text = data.ToString();
                msg.Add(ratingTable);
            }
            catch (Exception exc)
            {
                responseMessage = $"Storing to Table failed: {exc.Message}";
                log.LogInformation($"{responseMessage}");
                return new BadRequestObjectResult(responseMessage);
            }
            log.LogInformation("Add rating to storage completed.");

            responseMessage = string.IsNullOrEmpty(data.ToString())
                ? "There is no data."
                : $"{data.ToString()}.";

            return new OkObjectResult(responseMessage);
        }
    }
}
