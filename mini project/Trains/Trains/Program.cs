using System;
using System.Data.SqlClient;

namespace Trains
{
    class Program
    {
        private static string connectionString = "Server=ICS-LT-841G3F3;Database=Trains;Integrated Security=True;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        private static int UserId; // This should be set after successful user login
        private static int AdminId;

        static void Main(string[] args)
        {
            Home();
            Console.ReadKey();
        }

        static void Home()
        {
            try
            {
                connection.Open();

                while (true)
                {
                    Console.WriteLine("\t\t\t\t\t============================================= \n");
                    Console.WriteLine("\t\t\t\t\t Welcome to Railway Ticket Booking System ");
                    Console.WriteLine("\t\t\t\t\t========================================== \n");
                    Console.WriteLine(" 1.  Admin \n");
                    Console.WriteLine(" 2.  User \n");
                    Console.WriteLine(" 3. Exit \n");
                    Console.WriteLine("Enter your choice:");

                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("\n Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AdminLogin();
                            break;
                        case 2:
                            UserActions();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("\n Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Database connection closed. \n");
                }
            }
        }

        static void AdminLogin()
        {
            Console.WriteLine("Enter Admin Username:");
            string Username = Console.ReadLine();

            Console.WriteLine("Enter Admin Password:");
            string Password = Console.ReadLine();

            string query = "SELECT AdminId FROM Admins WHERE Username = @Username AND Password = @Password";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Password);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    AdminActions();
                }
                else
                {
                    Console.WriteLine("Invalid admin credentials.");
                }
            }
        }

        static void AdminActions()
        {
            Console.WriteLine("\t\t\t\t\t ==================================\n");
            Console.WriteLine("\t\t\t\t\t WELCOME TO ADMIN SECTION");
            Console.WriteLine("\t\t\t\t\t ===================================\n");
            Console.WriteLine("1. Add Train \n ");
            Console.WriteLine("2. Update Train \n ");
            Console.WriteLine("3. Activate Train \n ");
            Console.WriteLine("4. Deactivate Train \n ");
            Console.WriteLine("5. Show All Trains \n");
            Console.WriteLine("6. Logout \n");
            Console.WriteLine("Enter your choice: \n ");

            int adminChoice;
            if (!int.TryParse(Console.ReadLine(), out adminChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            switch (adminChoice)
            {
                case 1:
                    Admin.AddTrain(connection);
                    break;
                case 2:
                    Admin.UpdateTrain(connection);
                    break;
                case 3:
                    Admin.SoftActivate(connection);
                    break;
                case 4:
                    Admin.SoftDelete(connection);
                    break;
                case 5:
                    Admin.ShowAllTrains(connection);
                    break;
                case 6:
                    AdminId = 0; // Logout by resetting AdminId
                    Console.WriteLine("Logged out successfully.");
                    Home(); // Redirect back to Home page
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }



        static void UserActions()
        {
            Console.WriteLine("\t\t\t\t\t ===========================\n");
            Console.WriteLine("\t\t\t\t\t WELCOME TO USER SECTION");
            Console.WriteLine("\t\t\t\t\t ===========================\n");
            Console.WriteLine("1. Existing User Login");
            Console.WriteLine("2. New User Registration");
            Console.WriteLine("Enter your choice:");

            int userChoice;
            if (!int.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            switch (userChoice)
            {
                case 1:
                    UserLogin();
                    break;
                case 2:
                    RegisterUser();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        static void UserLogin()
        {
            Console.WriteLine("Enter Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            string password = Console.ReadLine();

            string query = "SELECT UserId FROM Users WHERE Username = @Username AND Password = @Password";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    UserId = Convert.ToInt32(result);
                    Console.WriteLine("Login successful.");
                    UserActionsAfterLogin(); // Redirect to actions after login
                }
                else
                {
                    Console.WriteLine("Invalid username or password.");
                    UserActions(); // Redirect back to user action page
                }
            }
        }

        static void RegisterUser()
        {
            Console.WriteLine("Enter Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            string password = Console.ReadLine();

            string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("User registered successfully.");
                    UserLogin(); // Redirect to login after registration
                }
                else
                    Console.WriteLine("Failed to register user.");
            }
        }

        static void UserActionsAfterLogin()
        {
            Console.WriteLine("\t\t\t\t\t ===========================\n");
            Console.WriteLine("\t\t\t\t\t WELCOME TO USER SECTION");
            Console.WriteLine("\t\t\t\t\t ===========================\n");
            Console.WriteLine("1. Book Ticket");
            Console.WriteLine("2. Cancel Ticket");
            Console.WriteLine("3. Show Available Trains");
            Console.WriteLine("4. Logout");
            Console.WriteLine("Enter your choice:");
            

            int userChoice;
            if (!int.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Home(); // Redirect back to actions after login
                return;
            }

            switch (userChoice)
            {
                case 1:
                    UserBooking();
                    break;
                case 2:
                    CancelTicket();
                    break;
                case 3:
                    ShowAvailableTrains();
                    break;
                case 4:
                    UserId = 0; // Logout by resetting UserId
                    Console.WriteLine("Logged out successfully.");
                    UserActions(); // Redirect back to user action page
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    UserActionsAfterLogin(); // Redirect back to actions after login
                    break;
                    
            }
        }
      

        static void UserBooking()
        {
            if (UserId == 0)
            {
                Console.WriteLine("Please log in to book tickets.");
                UserLogin();
            }

            Console.WriteLine("Enter Train ID to Book Ticket:");
            int trainId;
            if (!int.TryParse(Console.ReadLine(), out trainId))
            {
                Console.WriteLine("Invalid Train ID. Please enter a valid number.");
                ShowAvailableTrains();
                UserActionsAfterLogin();
                return;
            }

            Console.WriteLine("Select Class (1st/2nd/Sleeper):");
            string selectedClass = Console.ReadLine();

            Console.WriteLine("Enter Number of Tickets to Book:");
            int ticketsToBook;
            if (!int.TryParse(Console.ReadLine(), out ticketsToBook) || ticketsToBook <= 0)
            {
                Console.WriteLine("Invalid number of tickets. Please enter a valid number.");
                UserActionsAfterLogin();
                return;
            }

            // Check available berths in the selected class
            int availableBerths = GetAvailableBerths(trainId, selectedClass);
            if (availableBerths <= 0 || ticketsToBook > availableBerths)
            {
                Console.WriteLine("Not enough available berths.");
                UserActionsAfterLogin();
                return;
            }

            // Book ticket
            BookTicket(trainId, selectedClass, ticketsToBook);
        }


        static void CancelTicket()
        {
            if (UserId == 0)
            {
                Console.WriteLine("Please log in to cancel tickets.");
                UserLogin();
            }

            Console.WriteLine("Enter Booking ID to Cancel Ticket:");
            int bookingId;
            if (!int.TryParse(Console.ReadLine(), out bookingId))
            {
                Console.WriteLine("Invalid Booking ID. Please enter a valid number.");
                UserActionsAfterLogin();
                return;
            }

            // Check if the booking exists
            if (!CheckBookingExists(bookingId))
            {
                Console.WriteLine("Booking does not exist.");
                UserActionsAfterLogin();
                return;
            }

            // Retrieve the train ID and number of tickets to cancel
            int trainId = 0;
            int ticketsToCancel = 0;
            string classType = "";

            string queryBookingDetails = "SELECT TrainId, ClassType, NumTickets FROM Bookings WHERE BookingId = @BookingId";
            using (SqlCommand command = new SqlCommand(queryBookingDetails, connection))
            {
                command.Parameters.AddWithValue("@BookingId", bookingId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        trainId = Convert.ToInt32(reader["TrainId"]);
                        classType = reader["ClassType"].ToString();
                        ticketsToCancel = Convert.ToInt32(reader["NumTickets"]);
                    }
                }
            }

            if (trainId == 0 || ticketsToCancel == 0)
            {
                Console.WriteLine("Error retrieving booking details.");
                UserActionsAfterLogin();
                return;
            }

            // Cancel the ticket by deleting from the Bookings table
            string queryCancelTicket = "DELETE FROM Bookings WHERE BookingId = @BookingId AND UserId = @UserId";

            using (SqlCommand command = new SqlCommand(queryCancelTicket, connection))
            {
                command.Parameters.AddWithValue("@BookingId", bookingId);
                command.Parameters.AddWithValue("@UserId", UserId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Ticket canceled successfully, update available berths in Trains table
                    string columnToUpdate = "";
                    switch (classType.ToLower())
                    {
                        case "1st":
                        case "first":
                        case "1st class":
                            columnToUpdate = "FirstClassBerths";
                            break;
                        case "2nd":
                        case "second":
                        case "2nd class":
                            columnToUpdate = "SecondClassBerths";
                            break;
                        case "sleeper":
                            columnToUpdate = "SleeperBerths";
                            break;
                        default:
                            Console.WriteLine("Invalid class type.");
                            UserActionsAfterLogin();
                            return;
                    }

                    string queryUpdateBerths = $"UPDATE Trains SET {columnToUpdate} = {columnToUpdate} + @TicketsToCancel WHERE TrainId = @TrainId";

                    using (SqlCommand updateCommand = new SqlCommand(queryUpdateBerths, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@TicketsToCancel", ticketsToCancel);
                        updateCommand.Parameters.AddWithValue("@TrainId", trainId);

                        int updateRows = updateCommand.ExecuteNonQuery();
                        if (updateRows > 0)
                            Console.WriteLine("Ticket has been cancelled successfully !!...");
                        else
                            Console.WriteLine("Failed to update available berths.");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to cancel ticket.");
                }
            }
            UserActionsAfterLogin(); // Redirect back to actions after cancellation
        }


        static void ShowAvailableTrains()
        {
            string query = "SELECT TrainId, TrainName, FirstClassBerths, SecondClassBerths, SleeperBerths, Source, Destination FROM Trains WHERE IsActive = 1";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Available Trains:");
                    Console.WriteLine("---------------------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Train ID: {reader["TrainId"]}");
                        Console.WriteLine($"Train Name: {reader["TrainName"]}");
                        Console.WriteLine($"1st Class Berths: {reader["FirstClassBerths"]}");
                        Console.WriteLine($"2nd Class Berths: {reader["SecondClassBerths"]}");
                        Console.WriteLine($"Sleeper Berths: {reader["SleeperBerths"]}");
                        Console.WriteLine($"Route: {reader["Source"]} to {reader["Destination"]}");
                        //Console.WriteLine($"Price{reader["Price"]}");
                     

                        Console.WriteLine("---------------------------------------------");
                    }
                }
            }
            UserActionsAfterLogin(); // Redirect back to actions after showing available trains
        }

        static int GetAvailableBerths(int trainId, string selectedClass)
        {
            int availableBerths = -1;
            string column = "";
            switch (selectedClass.ToLower())
            {
                case "1st":
                case "first":
                case "1st class":
                    column = "FirstClassBerths";
                    break;
                case "2nd":
                case "second":
                case "2nd class":
                    column = "SecondClassBerths";
                    break;
                case "sleeper":
                    column = "SleeperBerths";
                    break;
                default:
                    Console.WriteLine("Invalid class selection.");
                    return availableBerths;
            }

            string query = $"SELECT {column} FROM Trains WHERE TrainId = @TrainId AND IsActive = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainId", trainId);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    availableBerths = Convert.ToInt32(result);
                }
            }
            return availableBerths;
        }

        static bool CheckBookingExists(int bookingId)
        {
            bool bookingExists = false;
            string query = "SELECT COUNT(*) FROM Bookings WHERE BookingId = @BookingId AND UserId = @UserId ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookingId", bookingId);
                command.Parameters.AddWithValue("@UserId", UserId);
                object result = command.ExecuteScalar();
                if (result != null && Convert.ToInt32(result) > 0)
                {
                    bookingExists = true;
                }
            }
            return bookingExists;
        }

        static void BookTicket(int trainId, string selectedClass, int ticketsToBook)
        {
            string query = "INSERT INTO Bookings (UserId, TrainId, ClassType, NumTickets) VALUES (@UserId, @TrainId, @ClassType, @NumTickets); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@ClassType", selectedClass);

                command.Parameters.AddWithValue("@NumTickets", ticketsToBook);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    int bookingId = Convert.ToInt32(result);
                    // Update available berths
                    UpdateAvailableBerths(trainId, selectedClass, ticketsToBook);
                    Console.WriteLine("Ticket booked successfully.");
                    Console.WriteLine("Thanks For Booking The Ticket");
                    Console.WriteLine($"Your Booking ID is: {bookingId}");

                }
                else
                {
                    Console.WriteLine("Failed to book ticket.");
                }
            }
            UserActionsAfterLogin(); // Redirect back to actions after booking
        }

        static void UpdateAvailableBerths(int trainId, string selectedClass, int ticketsToBook)
        {
            string column = "";
            switch (selectedClass.ToLower())
            {
                case "1st":
                case "first":
                case "1st class":
                    column = "FirstClassBerths";
                    break;
                case "2nd":
                case "second":
                case "2nd class":
                    column = "SecondClassBerths";
                    break;
                case "sleeper":
                    column = "SleeperBerths";
                    break;
                default:
                    Console.WriteLine("Invalid class selection.");
                    return;
            }

            string query = $"UPDATE Trains SET {column} = {column} - @TicketsToBook WHERE TrainId = @TrainId AND IsActive = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketsToBook", ticketsToBook);
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.ExecuteNonQuery();
            }
        }
    }

    static class Admin
    {
        public static void AddTrain(SqlConnection connection)
        {
            Console.WriteLine("Enter Train ID:");
            int trainId;
            if (!int.TryParse(Console.ReadLine(), out trainId))
            {
                Console.WriteLine("Invalid Train ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter Train Name:");
            string trainName = Console.ReadLine();

            Console.WriteLine("Enter Total Berths (1st Class):");
            int firstClassBerths;
            if (!int.TryParse(Console.ReadLine(), out firstClassBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter Total Berths (2nd Class):");
            int secondClassBerths;
            if (!int.TryParse(Console.ReadLine(), out secondClassBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter Total Berths (Sleeper):");
            int sleeperBerths;
            if (!int.TryParse(Console.ReadLine(), out sleeperBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter Source:");
            string source = Console.ReadLine();

            Console.WriteLine("Enter Destination:");
            string destination = Console.ReadLine();

            string query = "INSERT INTO Trains (TrainId, TrainName, FirstClassBerths, SecondClassBerths, SleeperBerths, Source, Destination, IsActive) " +
                           "VALUES (@TrainId, @TrainName, @FirstClassBerths, @SecondClassBerths, @SleeperBerths, @Source, @Destination, 1)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@TrainName", trainName);
                command.Parameters.AddWithValue("@FirstClassBerths", firstClassBerths);
                command.Parameters.AddWithValue("@SecondClassBerths", secondClassBerths);
                command.Parameters.AddWithValue("@SleeperBerths", sleeperBerths);
                command.Parameters.AddWithValue("@Source", source);
                command.Parameters.AddWithValue("@Destination", destination);
                //command.Parameters.AddWithValue("@Price", Price);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Train added successfully.");
                else
                    Console.WriteLine("Failed to add train.");
            }
        }


        public static void UpdateTrain(SqlConnection connection)
        {
            Console.WriteLine("Enter Train ID to Modify:");
            int trainId;
            if (!int.TryParse(Console.ReadLine(), out trainId))
            {
                Console.WriteLine("Invalid Train ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter New Total Berths (1st Class):");
            int newFirstClassBerths;
            if (!int.TryParse(Console.ReadLine(), out newFirstClassBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter New Total Berths (2nd Class):");
            int newSecondClassBerths;
            if (!int.TryParse(Console.ReadLine(), out newSecondClassBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter New Total Berths (Sleeper):");
            int newSleeperBerths;
            if (!int.TryParse(Console.ReadLine(), out newSleeperBerths))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter New Source:");
            string newSource = Console.ReadLine();

            Console.WriteLine("Enter New Destination:");
            string newDestination = Console.ReadLine();

            string query = "UPDATE Trains SET Source = @NewSource, Destination = @NewDestination ,FirstClassBerths = @NewFirstClassBerths, SecondClassBerths = @NewSecondClassBerths, SleeperBerths = @NewSleeperBerths WHERE TrainId = @TrainId AND IsActive = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@NewFirstClassBerths", newFirstClassBerths);
                command.Parameters.AddWithValue("@NewSecondClassBerths", newSecondClassBerths);
                command.Parameters.AddWithValue("@NewSleeperBerths", newSleeperBerths);
                command.Parameters.AddWithValue("@NewSource", newSource);
                command.Parameters.AddWithValue("@NewDestination", newDestination);
                //command.Parameters.AddWithValue("@NewPrice", newPrice);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Train modified successfully.");
                else
                    Console.WriteLine("Failed to modify train.");
            }
        }

        public static void SoftActivate(SqlConnection connection)
        {
            Console.WriteLine("Enter Train ID to Delete:");
            int trainId;
            if (!int.TryParse(Console.ReadLine(), out trainId))
            {
                Console.WriteLine("Invalid Train ID. Please enter a valid number.");
                return;
            }

            string query = "UPDATE Trains SET IsActive = 1 WHERE TrainId = @TrainId AND IsActive = 0";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainId", trainId);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Train is Activated successfully.");
                else
                    Console.WriteLine("Failed to activate the train.");
            }
        }


        public static void SoftDelete(SqlConnection connection)
        {
            Console.WriteLine("Enter Train ID to Delete:");
            int trainId;
            if (!int.TryParse(Console.ReadLine(), out trainId))
            {
                Console.WriteLine("Invalid Train ID. Please enter a valid number.");
                return;
            }

            string query = "UPDATE Trains SET IsActive = 0 WHERE TrainId = @TrainId AND IsActive = 1";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainId", trainId);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Train is deactivated successfully.");
                else
                    Console.WriteLine("Failed to deactivate the train.");
            }
        }
        public static void ShowAllTrains(SqlConnection connection)
        {
            string query = "SELECT * FROM Trains";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Available Trains:");
                    Console.WriteLine("---------------------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Train ID: {reader["TrainId"]}");
                        Console.WriteLine($"Train Name: {reader["TrainName"]}");
                        Console.WriteLine($"1st Class Berths: {reader["FirstClassBerths"]}");
                        Console.WriteLine($"2nd Class Berths: {reader["SecondClassBerths"]}");
                        Console.WriteLine($"Sleeper Berths: {reader["SleeperBerths"]}");
                        Console.WriteLine($"Route: {reader["Source"]} to {reader["Destination"]}");
                        Console.WriteLine($"Train Available: {reader["IsActive"]}");
                        //Console.WriteLine($"Price: {reader["Price"]}");
                        Console.WriteLine("---------------------------------------------");
                    }
                }
            }
        }
    }
}