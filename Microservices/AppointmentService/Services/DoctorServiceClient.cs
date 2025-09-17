using AppointmentService.Models.DTOs;

namespace AppointmentService.Services
{
    public class DoctorServiceClient
    {
        private readonly HttpClient _httpClient;
        
        public DoctorServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5003");
        }
        
        public async Task<DoctorDto?> GetDoctorAsync(int doctorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/doctors/{doctorId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DoctorDto>();
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
