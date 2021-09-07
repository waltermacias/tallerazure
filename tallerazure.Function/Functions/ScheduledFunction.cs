using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using tallerazure.Functions.Functions;

namespace tallerazure.Functions.Functions
{
	public static class ScheduledFunction
	{
		[FunctionName("ScheduledFunction")]
		public static async Task Run(
			[TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
			[Table("taller", Connection = "AzureWebJobsStorage")] CloudTable tallerTable,
			ILogger log)
		{
			log.LogInformation($"Deleting completed funtion execute at: {DateTime.Now}");

			string filter = TableQuery.GenerateFilterConditionForBool("IsConpleted", QueryComparisons.Equal, true);
			TableQuery<TableEntity> query = new TableQuery<TableEntity>().Where(filter);
			TableQuerySegment<TableEntity> completedTallers = await tallerTable.ExecuteQuerySegmentedAsync(query, null);
			int deleted = 0;
			foreach (TableEntity completedTaller in completedTallers)
			{
				await tallerTable.ExecuteAsync(TableOperation.Delete(completedTaller));

			}
			log.LogInformation($"Deleted: {deleted} items at:{DateTime.Now}");
		}
	}
}
