using System;
using System.Net;
using System.Threading.Tasks;
using device_wall_backend.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

// Arrange
            
// Act
            
// Assert
namespace device_wall_backend.IntegrationTests
{
    //TODO: Initialize TestDB
    public class DashboardTests: IntegrationTest
    {
        [Theory]
        [InlineData("Dashboard")]
        [InlineData("Dashboard/1")]
        public async Task GET_Dashboard_Ok(string url)
        {
            // Act
            var response = await TestClient.GetAsync(url);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        //testen ob request müll ist
        public async Task GET_DeviceDetails_NotFound()
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard/{100}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    
        //Filtertests
        
        [Theory]
        [InlineData("iOS")]
        [InlineData("Android")]
        public async Task GET_Dashboard_FilterContainsOperatingSystem(string operatingSystem)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?operatingSystem={operatingSystem}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert (nicht empty prüfen)
            foreach (Device device in deviceResults)
            {
                device.OperatingSystem.Should().Be(operatingSystem);
            }
        }
        
        [Theory]
        [InlineData("10.3.4")]
        [InlineData("6.0.1")]
        public async Task GET_Dashboard_FilterContainsVersion(string version)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?version={version}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            foreach (Device device in deviceResults)
            {
                device.Version.Should().Be(version);
            }
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GET_Dashboard_FilterContainsIsTablet(bool isTablet)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isTablet={isTablet}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            foreach (Device device in deviceResults)
            {
                device.IsTablet.Should().Be(isTablet);
            }
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GET_Dashboard_FilterContainsIsLent(bool isLent)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isLent={isLent}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            foreach (Device device in deviceResults)
            {
                if (isLent)
                {
                    device.CurrentLending.Should().NotBe(null);
                }
                else
                {
                    device.CurrentLending.Should().Be(null); 
                }
            }
        }
    }
}