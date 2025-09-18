using AppointmentService.Models.DTOs;
using System.Security.Claims;

namespace AppointmentService.Services
{
    public class PatientServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public PatientServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("http://localhost:5003"); // Correct port for PatientService
        }
        
        public async Task<PatientDto?> GetPatientAsync(int patientId)
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

                var response = await _httpClient.GetAsync($"/api/patients/{patientId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PatientDto>();
                }
                
                // Log the error for debugging
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"PatientService error: {response.StatusCode} - {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PatientServiceClient error: {ex.Message}");
                return null;
            }
        }
    }
}
