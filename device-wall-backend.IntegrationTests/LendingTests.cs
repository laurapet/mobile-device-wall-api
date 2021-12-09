using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Xunit;

namespace device_wall_backend.IntegrationTests
{
    public class LendingTests: IntegrationTest
    {
        [Theory]
        [InlineData("lendings")]
        [InlineData("lendings?userID=1")]
        public async Task GET_Lendings_Ok(string uri)
        {
            // Act
            var response = await TestClient.GetAsync(uri);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task GET_OwnLendings_UserNotFound()
        {
            // Act
            var response = await TestClient.GetAsync($"lendings?userID={100}");
            var lendingResults = JsonConvert.DeserializeObject<OwnLendingDTO[]>(await response.Content.ReadAsStringAsync());
            
            // Assert
            lendingResults.Should().BeEmpty();
        }

        [Fact]
        public async Task POST_Lending_Ok()
        {
            // Arrange
            await TestClient.DeleteAsync($"lendings/{1}");
            await TestClient.DeleteAsync($"lendings/{2}");
            
            // Act
            var response = await TestClient.PostAsync($"lendings?userID={1}", new StringContent(
                    JsonConvert.SerializeObject(new Collection<LendingListDTO>()
                    {
                        new() {DeviceID = 1, IsLongterm = true},
                        new() {DeviceID = 2, IsLongterm = false}
                    }),
                    Encoding.UTF8,
                    "application/json"
                )
            );
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Theory]
        [InlineData(3,4)]
        [InlineData(1,4)]
        [InlineData(3,1)]
        public async Task POST_Lending_DeviceIsAlreadyLent(int deviceId1, int deviceId2)
        {
            // Arrange
            await TestClient.DeleteAsync($"lendings/{1}");
            
            // Act
            var response = await TestClient.PostAsync($"lendings?userID={1}", new StringContent(
                    JsonConvert.SerializeObject(new Collection<LendingListDTO>()
                    {
                        new() {DeviceID = deviceId1, IsLongterm = true},
                        new() {DeviceID = deviceId2, IsLongterm = true}
                    }),
                    Encoding.UTF8,
                    "application/json"
                )
            );
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData(int.MaxValue,1)]
        [InlineData(1,int.MaxValue)]
        public async Task POST_Lending_DeviceNotFound(int deviceId1, int deviceId2)
        {
            // Arrange
            await TestClient.DeleteAsync($"lendings/{1}");
            
            // Act
            var response = await TestClient.PostAsync($"lendings?userID={1}", new StringContent(
                    JsonConvert.SerializeObject(new Collection<LendingListDTO>()
                    {
                        new() {DeviceID = deviceId1, IsLongterm = true},
                        new() {DeviceID = deviceId2, IsLongterm = true}
                    }),
                    Encoding.UTF8,
                    "application/json"
                )
            );
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task PUT_Lending_Ok()
        {
            // Arrange
            await TestClient.PostAsync($"lendings?userID={1}", new StringContent(
                JsonConvert.SerializeObject(new Collection<LendingListDTO>()
                {
                    new(){DeviceID = 1, IsLongterm = true}
                }),
                Encoding.UTF8,
                "application/json"
                )
            );
            
            // Act
            var response = await TestClient.PutAsync("lendings/1?currentUserID=1&newUserID=2", new StringContent(
                JsonConvert.SerializeObject(null),
                Encoding.UTF8,
                "application/json"
            ));
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task PUT_Lending_DeviceNotFound()
        {
            // Act
            var response = await TestClient.PutAsync($"lendings/{int.MaxValue}?currentUserID=1&newUserID=2", new StringContent(
                JsonConvert.SerializeObject(null),
                Encoding.UTF8,
                "application/json"
            ));
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task DELETE_Lending_Ok()
        {
            // Act
            var response = await TestClient.DeleteAsync($"lendings/{1}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task DELETE_Lending_NotFound()
        {
            // Act
            var response = await TestClient.DeleteAsync($"lendings/{int.MaxValue}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}