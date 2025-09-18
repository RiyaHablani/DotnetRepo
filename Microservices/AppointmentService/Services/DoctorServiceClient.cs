using AppointmentService.Models.DTOs;
using System.Security.Claims;

namespace AppointmentService.Services
{
    public class DoctorServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public DoctorServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("http://localhost:5002"); // Correct port for DoctorService
        }
        
        public async Task<DoctorDto?> GetDoctorAsync(int doctorId)
        {
            try
            {
                // Add authentication header if available
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
                }

                var response = await _httpClient.GetAsync($"/api/doctors/{doctorId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DoctorDto>();
                }
                
                // Log the error for debugging
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"DoctorService error: {response.StatusCode} - {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoctorServiceClient error: {ex.Message}");
                return null;
            }
        }
    }
}
