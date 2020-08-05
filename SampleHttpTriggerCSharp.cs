using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Company.Function
{
    public static class SampleHttpTriggerCSharp
    {
        [FunctionName("SampleHttpTriggerCSharp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

        // Get the connection string from app settings and use it to create a connection.
            var str = "Server=tcp:enbdbserver.database.windows.net,1433;Initial Catalog=ENBDB1;Persist Security Info=False;User ID=enbdbadmin;Password=Test@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(str))
            {
                try{
                conn.Open();
                var CommandText =  "insert into personal_information values ('"+ name +"', 'K', 'CKC-HYD', 'Hyderabad', '323530')";

                using (SqlCommand cmd = new SqlCommand(CommandText, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were inserted");
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            }
            
            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

             //string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult("Data Inserted Successfully");
        }
    }
}
