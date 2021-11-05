using System;
using System.Net;
using System.Threading.Tasks;
using device_wall_backend.Models;
using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace device_wall_backend.IntegrationTests
{
    public class DashboardTests: IntegrationTest
    {
        [Fact]
        public async Task UnsupportedURI_NotFound()
        {
            var response = await TestClient.GetAsync("unsupportedURI");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
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
        public async Task GET_DeviceDetails_NotFound()
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard/{int.MaxValue}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    
        //Filtertests
        
        [Theory]
        [InlineData("iOS")]
        [InlineData("Android")]
        public async Task GET_Dashboard_Filter_OperatingSystem(string operatingSystem)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?operatingSystem={operatingSystem}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert 
            deviceResults.Should().NotBeEmpty();
            foreach (Device device in deviceResults)
            {
                device.OperatingSystem.Should().Be(operatingSystem);
            }
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GET_Dashboard_Filter_OperatingSystem_EmptyQuery(string operatingSystem)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?operatingSystem={operatingSystem}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert 
            deviceResults.Should().NotBeEmpty();
            bool iosProvided = false;
            bool androidProvided = false;
            foreach (Device device in deviceResults)
            {
                iosProvided = device.OperatingSystem.Equals("iOS") || iosProvided;
                androidProvided = device.OperatingSystem.Equals("Android") || androidProvided;
            }
            Assert.True(iosProvided && androidProvided);
        }
        
        [Theory]
        [InlineData("fakeOS")]
        public async Task GET_Dashboard_Filter_OperatingSystem_DoesntExist(string operatingSystem)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?operatingSystem={operatingSystem}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert 
            deviceResults.Should().BeEmpty();
        }
        
        [Theory]
        [InlineData("10.3.4")]
        [InlineData("6.0.1")]
        public async Task GET_Dashboard_Filter_Version(string version)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?version={version}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().NotBeEmpty();
            foreach (Device device in deviceResults)
            {
                device.Version.Should().Be(version);
            }
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GET_Dashboard_Filter_Version_EmptyQuery(string version)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?version={version}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert 
            deviceResults.Should().NotBeEmpty();
            bool version1Provided = false;
            bool version2Provided = false;
            foreach (Device device in deviceResults)
            {
                version1Provided = device.Version.Equals("10.3.4") || version1Provided;
                version2Provided = device.Version.Equals("6.0.1") || version2Provided;
            }
            
            Assert.True(version1Provided && version2Provided);
        }
        
        [Theory]
        [InlineData("FakeVersion")]
        public async Task GET_Dashboard_Filter_Version_DoesntExist(string version)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?version={version}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().BeEmpty();
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GET_Dashboard_Filter_IsTablet(bool isTablet)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isTablet={isTablet}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().NotBeEmpty();
            foreach (Device device in deviceResults)
            {
                device.IsTablet.Should().Be(isTablet);
            }
        }
        
        [Theory]
        [InlineData(null)]
        public async Task GET_Dashboard_Filter_IsTablet_EmptyQuery(bool? isTablet)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isTablet={isTablet}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().NotBeEmpty();
            bool tabletProvided = false;
            bool phoneProvided = false;
            foreach (Device device in deviceResults)
            {
                tabletProvided = device.IsTablet || tabletProvided;
                phoneProvided = !device.IsTablet || phoneProvided;
            }
            
            Assert.True(tabletProvided && phoneProvided);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GET_Dashboard_Filter_IsLent(bool isLent)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isLent={isLent}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().NotBeEmpty();
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
        
        [Theory]
        [InlineData(null)]
        public async Task GET_Dashboard_Filter_IsLent_EmptyQuery(bool? isLent)
        {
            // Act
            var response = await TestClient.GetAsync($"Dashboard?isLent={isLent}");
            var deviceResults = JsonConvert.DeserializeObject<Device[]>(await response.Content.ReadAsStringAsync());
            // Assert
            deviceResults.Should().NotBeEmpty();
            bool isLentProvided = false;
            bool notLentProvided = false;
            foreach (Device device in deviceResults)
            {
                isLentProvided = (device.CurrentLending != null) || isLentProvided;
                notLentProvided = (device.CurrentLending == null) || notLentProvided;
            }
            
            Assert.True(isLentProvided && notLentProvided);
        }
    }
}