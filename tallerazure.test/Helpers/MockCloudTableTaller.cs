using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace tallerazure.Test.Helpers
{
	public class MockCloudTableTaller : CloudTable
	{
		public MockCloudTableTaller(Uri tableAddress) : base(tableAddress)
		{
		}

		public MockCloudTableTaller(StorageUri tableAddress, StorageCredentials credentials) : base
			(tableAddress, credentials)
		{
		}

		public MockCloudTableTaller(Uri tableAbsoluteUri, StorageCredentials credentials) : base
			(tableAbsoluteUri, credentials)
		{
		}
		public override async Task<TableResult> ExecuteAsync(TableOperation operation)
		{
			return await Task.FromResult(new TableResult
			{
				HttpStatusCode = 200,
				Result = TestFactory.GetTodoEntity()
			});
		}
	}
}
