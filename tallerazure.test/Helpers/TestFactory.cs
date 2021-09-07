using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;
using tallerazure.Common.Models;
using tallerazure.Functions.Entities;

namespace tallerazure.Test.Helpers
{
	public class TestFactory
	{
		public static TallerEntity GetTodoEntity()
		{
			return new TallerEntity
			{
				ETag = "*",
				PartitionKey = "TALLER",
				RowKey = Guid.NewGuid().ToString(),
				CreatedTime = DateTime.UtcNow,
				IsConsolidated = false,
				TypeReg = false,
				NameEmployee = "Alejandro Fernandez",
			};
		}

		public static DefaultHttpRequest CreateHttpRequest(Guid tallerId, Taller tallerRequest)
		{
			string request = JsonConvert.SerializeObject(tallerRequest);
			DefaultHttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext())
			{
				Body = GenerateStreamFromString(request),
				Path = $"/{tallerId}"
			};

			return httpRequest;
		}

		public static DefaultHttpRequest CreateHttpRequest(Guid tallerId)
		{
			DefaultHttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext())
			{
				Path = $"/{tallerId}"
			};

			return httpRequest;
		}

		public static DefaultHttpRequest CreateHttpRequest(Taller tallerRequest)
		{
			string request = JsonConvert.SerializeObject(tallerRequest);
			DefaultHttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext())
			{
				Body = GenerateStreamFromString(request)
			};

			return httpRequest;
		}

		public static DefaultHttpRequest CreateHttpRequest()
		{
			DefaultHttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext())
			{
			};

			return httpRequest;
		}

		public static Taller GetTallerRequest()
		{
			return new Taller
			{
				CreatedTime = DateTime.UtcNow,
				IsConsolidated = false,
				TypeReg = false,
				NameEmployee = "Alvaro Uribe"
				
			};
		}

		public static Stream GenerateStreamFromString(string stringToConvert)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(stringToConvert);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
		{
			ILogger logger;

			if (type == LoggerTypes.List)
			{
				logger = new ListLogger();
			}
			else
			{
				logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
			}

			return logger;
		}
	}
}
