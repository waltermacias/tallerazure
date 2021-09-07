using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using tallerazure.Common.Models;
using tallerazure.Functions.Functions;
using tallerazure.Test.Helpers;
using Xunit;

namespace tallerazure.Test.Test
{
	public class TallerApiTest
	{
		private readonly ILogger logger = TestFactory.CreateLogger();

		[Fact]
		public async void CreateTaller_Should_Return_200()
		{
			// Arrenge
			MockCloudTableTaller mockTaller = new MockCloudTableTaller(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));

			Taller tallerRequest = TestFactory.GetTallerRequest();
			DefaultHttpRequest request = TestFactory.CreateHttpRequest(tallerRequest);
			// Act
			IActionResult response = await TallerApi.CreateTaller(request, mockTaller, logger);
			// Assert
			OkObjectResult result = (OkObjectResult)response;
			Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
		[Fact]
		public async void UpdateTodo_Should_Return_200()
		{
            // Arrange
            MockCloudTableTaller mockTodos = new MockCloudTableTaller(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Guid todoId = Guid.NewGuid();
		Taller todoRequest = TestFactory.GetTallerRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoId, todoRequest);

            // Act
            IActionResult response = await TallerApi.UpdateTaller(request, mockTodos, todoId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
	}
		[Fact]
		public async void UpdateTaller_Should_Return_200()
		{
			// Arrange
		MockCloudTableTaller mockTodos = new MockCloudTableTaller(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
			Guid todoId = Guid.NewGuid();
		Taller todoRequest = TestFactory.GetTallerRequest();
		DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoId, todoRequest);

			// Act
		IActionResult response = await TallerApi.UpdateTallerrequest, mockTaller, tallerId.ToString(), logger);

			// Assert
		OkObjectResult result = (OkObjectResult)response;
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
	}
}
