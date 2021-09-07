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
		
		[FunctionName(nameof(UpdateTaller))]
		public static async Task<IActionResult> UpdateTaller(
		    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "taller/{id}")] HttpRequest req,
		    [Table("taller", Connection = "AzureWebJobsStorage")] CloudTable tallerTable,
		    string id,
			ILogger log)
		{
			log.LogInformation("Update for taller:{id}, received.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Taller taller = JsonConvert.DeserializeObject<Taller>(requestBody);

			// Validate ID

			TableOperation findOperation = TableOperation.Retrieve<TallerEntity>("TALLER", id);
			TableResult findResult = await tallerTable.ExecuteAsync(findOperation);
			if (findResult.Result == null)
			{ 
				return new BadRequestObjectResult(new Response
				{
					IsSuccess = false,
					Message = "Taller not found."
				});
			}

			//Update taller
			TallerEntity tallerEntity = (TallerEntity)findResult.Result;
			tallerEntity.IsConsolidated = taller.IsConsolidated;
			if (!string.IsNullOrEmpty(taller.NameEmployee))
			{
				tallerEntity.NameEmployee = taller.NameEmployee;

			}

			

			TableOperation addOperation = TableOperation.Replace(tallerEntity);
			await tallerTable.ExecuteAsync(addOperation);

			string message = $"Taller: {id}, updated in table";
			log.LogInformation(message);

			return new OkObjectResult(new Response
			{
				IsSuccess = true,
				Message = message,
				Result = tallerEntity
			});

		}

		[FunctionName(nameof(GetAllDates))]
		public static async Task<IActionResult> GetAllDates(
		    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "taller")] HttpRequest req,
		    [Table("taller", Connection = "AzureWebJobsStorage")] CloudTable tallerTable,
			ILogger log)
		{
			log.LogInformation("Get all Dates received.");

			TableQuery<TallerEntity> query = new TableQuery<TallerEntity>();
			TableQuerySegment<TallerEntity> tallers = await tallerTable.ExecuteQuerySegmentedAsync(query, null);
		

			string message = "Retrieved all dates";
			log.LogInformation(message);

			return new OkObjectResult(new Response
			{ 
				IsSuccess = true,
				Message = message,
				Result = tallers
			});
		}

		[FunctionName(nameof(GetTallerById))]
		public static IActionResult GetTallerById(
		    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "taller/{id}")] HttpRequest req,
		    [Table("taller", "TALLER", "{id}", Connection = "AzureWebJobsStorage")] TallerEntity tallerEntity,
		    string id,
			ILogger log)
		{
			log.LogInformation($"Get Date by ID: {id}, received.");

			if (tallerEntity == null)
			{
				return new BadRequestObjectResult(new Response
				{
					IsSuccess = false,
					Message = "Taller not found."
				});
			}

			string message = $"Taller: {id}, retrieved";
			log.LogInformation(message);

			return new OkObjectResult(new Response
			{
				IsSuccess = true,
				Message = message,
				Result = tallerEntity
			});
		}

		[FunctionName(nameof(DeleteReg))]
		public static async Task<IActionResult> DeleteReg(
		    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "taller/{id}")] HttpRequest req,
		    [Table("taller", "TALLER", "{id}", Connection = "AzureWebJobsStorage")] TallerEntity tallerEntity,
			[Table("taller", Connection = "AzureWebJobsStorage")] CloudTable tallerTable,
		    string id,
			ILogger log)
		{
			log.LogInformation($"Delete Registro: {id}, received.");

			if (tallerEntity == null)
			{
				return new BadRequestObjectResult(new Response
				{
					IsSuccess = false,
					Message = "Taller not found."
				});
			}

			await tallerTable.ExecuteAsync(TableOperation.Delete(tallerEntity));
			string message = $"Taller: {tallerEntity.RowKey}, deleted.";
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