using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using System.Text.Json;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public AuthController(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { Message = "Invalid request" });
                }
                
                Console.WriteLine($"Received login request for: {request.Company_Code}");
                
                // Call the external API
                var apiUrl = "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";
                
                var jsonContent = JsonSerializer.Serialize(request);
                Console.WriteLine($"Sending to external API: {jsonContent}");
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                // Log the response for debugging
                Console.WriteLine($"External API Response: {responseContent}");
                  if (response.IsSuccessStatusCode)
                {
                    // Parse the response
                    var apiResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    
                    // Check if the response has the expected structure
                    if (apiResponse.TryGetProperty("Status_Code", out var statusCode) && 
                        statusCode.GetInt32() == 200 &&
                        apiResponse.TryGetProperty("Response_Body", out var responseBody) &&
                        responseBody.GetArrayLength() > 0)
                    {                        var firstUser = responseBody[0];
                        Console.WriteLine($"First user data: {firstUser.GetRawText()}");
                        if (firstUser.TryGetProperty("User_Locations", out var userLocationsElement))
                        {
                            Console.WriteLine($"User_Locations found: {userLocationsElement.GetRawText()}");
                            var userLocations = JsonSerializer.Deserialize<List<UserLocation>>(userLocationsElement.GetRawText());
                            
                            if (userLocations != null && userLocations.Any())
                            {
                                // Clear existing location details
                                _context.Location_Details.RemoveRange(_context.Location_Details);
                                  // Save to database
                                foreach (var location in userLocations)
                                {
                                    _context.Location_Details.Add(new LocationDetail
                                    {
                                        Location_Code = location.Location_Code,
                                        Location_Name = location.Location_Name,
                                        Stock_Handle = location.Stock_Handle,
                                        Address = location.Address,
                                        Phone = location.Phone,
                                        Status = location.Status
                                    });
                                }
                                
                                await _context.SaveChangesAsync();
                                
                                return Ok(new LoginResponse
                                {
                                    Success = true,
                                    Message = "Login successful",
                                    User_Locations = userLocations
                                });
                            }
                        }
                    }
                }
                
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Login failed"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var locations = await Task.FromResult(_context.Location_Details.ToList());
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving locations: {ex.Message}" });
            }
        }
    }
}
