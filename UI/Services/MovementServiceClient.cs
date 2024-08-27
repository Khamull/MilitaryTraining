using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.Services
{
    public class MovementServiceClient
    {
        private readonly HttpClient _httpClient;

        public MovementServiceClient()
        {
            _httpClient = new HttpClient
            {
                //http://localhost:5100/api/Movements/get
                BaseAddress = new Uri("http://localhost:5100/")
            };
        }

        public async Task<List<Movement>> GetAllMovementsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Movements/get"); // Ensure this matches your API
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Movement>>(jsonString);
                }
                return new List<Movement>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error: {ex.Message}"); // Display the error message
                return new List<Movement>(); // Return an empty list or handle accordingly
            }
        }
    }
}
