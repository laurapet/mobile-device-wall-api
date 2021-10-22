using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using device_wall_backend;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Xunit;
using Assert = NUnit.Framework.Assert;

//using Moq;
namespace Tests
{
    public class DashboardTests
    {
        private readonly TestServer _server;
        private HttpClient _client;
        private HttpResponseMessage response;

        public DashboardTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        
        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5000/Dashboard/");
            response = _client.GetAsync("").Result;
        }

        [Fact]
        public async Task GET_Dashboard_Ok()
        {
            
        }
        
        

        [Test]
        [Fact]
        public void Test1()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
