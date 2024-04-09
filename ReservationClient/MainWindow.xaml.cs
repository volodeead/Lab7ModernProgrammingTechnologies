using ReservationClient;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdventureWorksWPFClient
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Bus> Buses { get; set; }
        public ObservableCollection<Ticket> Tickets { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Clients = new ObservableCollection<Client>();
            Buses = new ObservableCollection<Bus>();
            Tickets = new ObservableCollection<Ticket>();
            DataContext = this;

            // Завантажити дані при старті вікна
            LoadAllClientsAsync_Click(this, null);
            GetAllBusesButton_Click(this, null);
        }

        // Юзер логіка кнопок

        private async void LoadAllClientsAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clients = await _apiService.GetClientsAsync();
                // Перевірка, чи колекція Clients була ініціалізована
                if (Clients == null) Clients = new ObservableCollection<Client>();
                else Clients.Clear();

                foreach (var client in clients)
                {
                    Clients.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CreateClientButton_Click(object sender, RoutedEventArgs e)
        {
            var firstName = FirstNameTextBox.Text; // Замініть ці рядки на отримання реальних даних
            var lastName = LastNameTextBox.Text;
            var newClient = new Client { FirstName = firstName, LastName = lastName };

            try
            {
                var createdClient = await _apiService.CreateClientAsync(newClient);
                Clients.Add(createdClient);
                // Очистіть поля після створення
                FirstNameTextBox.Clear();
                LastNameTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient)
            {
                // Оновлення інформації клієнта з введення користувача
                selectedClient.FirstName = FirstNameTextBoxUpdate.Text;
                selectedClient.LastName = LastNameTextBoxUpdate.Text;

                try
                {
                    var success = await _apiService.UpdateClientAsync(selectedClient.ClientId, selectedClient);
                    if (success)
                    {
                        // Перезавантажте або оновіть ваш DataGrid, щоб показати оновлені дані
                        LoadAllClientsAsync_Click(null, null);

                        MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a client to update.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient &&
                MessageBox.Show($"Are you sure you want to delete {selectedClient.FirstName} {selectedClient.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var success = await _apiService.DeleteClientAsync(selectedClient.ClientId);
                    if (success)
                    {
                        Clients.Remove(selectedClient);
                        MessageBox.Show("Client deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        // Автобус логіка кнопок

        private async void CreateBusButton_Click(object sender, RoutedEventArgs e)
        {
            var newBus = new Bus
            {
                LicensePlate = LicensePlateTextBox.Text,
                Model = ModelTextBox.Text,
                SeatsAvailable = int.TryParse(SeatsAvailableTextBox.Text, out int seats) ? seats : 0
            };

            try
            {
                var createdBus = await _apiService.CreateBusAsync(newBus);
                Buses.Add(createdBus);
                MessageBox.Show("Bus created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateBusButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem is Bus selectedBus)
            {
                selectedBus.LicensePlate = LicensePlateTextBox.Text;
                selectedBus.Model = ModelTextBox.Text;
                selectedBus.SeatsAvailable = int.TryParse(SeatsAvailableTextBox.Text, out int seats) ? seats : selectedBus.SeatsAvailable;

                try
                {
                    var success = await _apiService.UpdateBusAsync(selectedBus.BusId, selectedBus);
                    if (success)
                    {
                        GetAllBusesButton_Click(sender, e); // Reload buses
                        MessageBox.Show("Bus updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a bus to update.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteBusButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem is Bus selectedBus)
            {
                try
                {
                    var success = await _apiService.DeleteBusAsync(selectedBus.BusId);
                    if (success)
                    {
                        Buses.Remove(selectedBus);
                        MessageBox.Show("Bus deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a bus to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void GetAllBusesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var buses = await _apiService.GetBusesAsync();
                Buses.Clear();
                foreach (var bus in buses)
                {
                    Buses.Add(bus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Квитки логіка

        private async void LoadAllTicketsAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var tickets = await _apiService.GetTicketsAsync();
                Tickets.Clear();
                foreach (var ticket in tickets)
                {
                    Tickets.Add(ticket);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CreateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            // Припустимо, що у нас є ComboBox для вибору ClientId та BusId
            var clientId = int.Parse(ClientComboBox.SelectedValue.ToString());
            var busId = int.Parse(BusComboBox.SelectedValue.ToString());

            // Перевірка наявності місць
            var bus = await _apiService.GetBusAsync(busId);
            var ticketsForBus = await _apiService.GetTicketsAsync();
            var bookedTicketsForBus = ticketsForBus.Where(t => t.BusId == busId).Count();

            if (bookedTicketsForBus >= bus.SeatsAvailable)
            {
                MessageBox.Show("All seats for this bus are booked.", "Booking Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newTicket = new Ticket { ClientId = clientId, BusId = busId };
            try
            {
                var createdTicket = await _apiService.CreateTicketAsync(newTicket);
                Tickets.Add(createdTicket);
                MessageBox.Show("Ticket created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsDataGrid.SelectedItem is Ticket selectedTicket)
            {
                try
                {
                    var success = await _apiService.DeleteTicketAsync(selectedTicket.TicketId);
                    if (success)
                    {
                        Tickets.Remove(selectedTicket);
                        MessageBox.Show("Ticket deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void UpdateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsDataGrid.SelectedItem is Ticket selectedTicket)
            {
                // Assuming you have ComboBoxes for selecting a new client and bus
                var newClientId = ClientComboBox.SelectedValue as int?;
                var newBusId = BusComboBox.SelectedValue as int?;

                if (newClientId.HasValue && newBusId.HasValue)
                {
                    selectedTicket.ClientId = newClientId.Value;
                    selectedTicket.BusId = newBusId.Value;

                    try
                    {
                        var success = await _apiService.UpdateTicketAsync(selectedTicket.TicketId, selectedTicket);
                        if (success)
                        {
                            MessageBox.Show("Ticket updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select both a client and a bus.", "Input Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


    }
}
