using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using tallerazure.Common.Models;
using tallerazure.Common.Responses;
using tallerazure.Functions.Entities;

namespace tallerazure.Functions.Functions
{
	public static class TallerApi
	{

		[FunctionName(nameof(CreateTaller))]
		public static async Task<IActionResult> CreateTaller(
		    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "taller")] HttpRequest req,
		    [Table("taller", Connection = "AzureWebJobsStorage")] CloudTable tallerTable,
			ILogger log)
		{
			log.LogInformation("Recieved a new employee.");

			string name = req.Query["name"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Taller taller = JsonConvert.DeserializeObject<Taller>(requestBody);

			if (string.IsNullOrEmpty(taller?.NameEmployee))
			{
				return new BadRequestObjectResult(new Response
				{
					IsSuccess = false,
					Message = "The request must have a NameEmployee."
				});
			}

			TallerEntity tallerEntity = new TallerEntity
			{
				CreatedTime = DateTime.UtcNow,
				ETag = "*",
				IsConsolidated = false,
				PartitionKey = "TALLER",
				RowKey = Guid.NewGuid().ToString(),
				NameEmployee = taller.NameEmployee
			};

			TableOperation addOperation = TableOperation.Insert(tallerEntity);
			await tallerTable.ExecuteAsync(addOperation);

			string message = "New taller stored in Table";
			log.LogInformation(message);

			return new OkObjectResult(new Response
			{
				IsSuccess = true,
				Message = message,
				Result = tallerEntity
			});

		}
	}
}