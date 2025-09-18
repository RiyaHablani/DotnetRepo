using AppointmentService.Models.DTOs;

namespace AppointmentService.Services
{
    public class PatientServiceClient
    {
        private readonly HttpClient _httpClient;
        
        public PatientServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5002");
        }
        
        public async Task<PatientDto?> GetPatientAsync(int patientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/patients/{patientId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PatientDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
