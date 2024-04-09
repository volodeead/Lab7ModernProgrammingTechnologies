using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReservationClient
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5244"; // Змініть на URL вашого API

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        // Отримання всіх клієнтів
        public async Task<List<Client>> GetClientsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/clients");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Client>>(content);
        }

        // Отримання одного клієнта за ID
        public async Task<Client> GetClientAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/clients/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Client>(content);
        }

        // Оновлення клієнта
        public async Task<bool> UpdateClientAsync(int id, Client client)
        {
            var content = new StringContent(JsonConvert.SerializeObject(client), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/clients/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Client not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        // Створення нового клієнта
        public async Task<Client> CreateClientAsync(Client client)
        {
            var serializedClient = JsonConvert.SerializeObject(client);
            var content = new StringContent(serializedClient, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine($"Sending POST request to {_baseUrl}/api/clients with body: {serializedClient}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/clients", content);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseData}");

            return JsonConvert.DeserializeObject<Client>(responseData);
        }


        // Видалення клієнта
        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Client not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        // Додаткові методи для роботи з автобусами

        public async Task<List<Bus>> GetBusesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/buses");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Bus>>(content);
        }

        public async Task<Bus> GetBusAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/buses/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Bus>(content);
        }

        public async Task<Bus> CreateBusAsync(Bus bus)
        {
            var content = new StringContent(JsonConvert.SerializeObject(bus), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/buses", content);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Bus>(responseData);
        }

        public async Task<bool> UpdateBusAsync(int id, Bus bus)
        {
            var content = new StringContent(JsonConvert.SerializeObject(bus), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/buses/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Bus not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteBusAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/buses/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Bus not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }


        // Додаткові методи для роботи з квитками

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/tickets");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Ticket>>(content);
        }

        public async Task<Ticket> GetTicketAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/tickets/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(content);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            var content = new StringContent(JsonConvert.SerializeObject(ticket), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/tickets", content);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(responseData);
        }

        public async Task<bool> UpdateTicketAsync(int id, Ticket ticket)
        {
            var content = new StringContent(JsonConvert.SerializeObject(ticket), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/tickets/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Ticket not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/tickets/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Ticket not found");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
