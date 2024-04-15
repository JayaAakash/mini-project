using System;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;

namespace Trains
{
    class Program
    {
        static string TrainsConnectionString = ConfigurationManager.ConnectionStrings["Trains"].ConnectionString;
        static DataClasses1DataContext db = new DataClasses1DataContext(TrainsConnectionString);
        static string passengerName;
        static int passengerAge;
        static string passengerGender;
        static string bookingID;

        static void Main(string[] args)
        {
            Console.WriteLine("---------------- Welcome To Trains Booking application --------------");
            Console.WriteLine();

            bool isAdmin = false;
            Console.WriteLine("Enter A for Admin");
            Console.WriteLine("Enter C for Customer");
            Console.Write("Are you (A/C): ");

            string isAdminInput = Console.ReadLine().ToUpper();
            if (isAdminInput == "A")
            {
                isAdmin = true;
            }

            if (isAdmin)
            {
                AdminMenu();
            }
            else
            {
                CustomerMenu();
            }
        }

        static bool AdminLogin()
        {
            string adminUsername = "admin";
            string adminPassword = "India@18";

            Console.WriteLine("------------- Admin Login --------------");
            Console.Write("Enter the Admin Username: ");
            string usernameInput = Console.ReadLine();
            Console.Write("Enter the Admin Password: ");
            string passwordInput = Console.ReadLine();

            return usernameInput == adminUsername && passwordInput == adminPassword;
        }

        static void AdminMenu()
        {
            if (AdminLogin())
            {
                Console.WriteLine("Login Successful!");

                Console.WriteLine("---------- WelCome To Admin --------------");
                Console.WriteLine();

                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Add Train");
                Console.WriteLine("2. Update Train");
                Console.WriteLine("3. Delete Train");
                Console.WriteLine("4. View All Trains");
                Console.WriteLine("5. Exit");

                Console.WriteLine("Enter your choice:");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddTrain();
                        break;
                    case 2:
                        UpdateTrain();
                        break;
                    case 3:
                        DeleteTrain();
                        break;
                    case 4:
                        ViewAllTrains();
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid Credentials. Access Denied.");
            }
            Console.ReadLine();
        }

        static bool CustomerLogin()
        {
            Console.WriteLine("Customer Login:");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            // Check if the entered credentials exist in the Customers table
            bool isExistingCustomer = CheckExistingCustomer(username, password);

            if (isExistingCustomer)
            {
                Console.WriteLine("Login Successful!");
                // Return true indicating successful login
                return true;
            }
            else
            {
                Console.WriteLine("Login Failed. No existing customer found with the provided credentials.");
                Console.WriteLine("Would you like to create a new account? (Y/N)");
                string createNewAccount = Console.ReadLine().ToUpper();
                if (createNewAccount == "C")
                {
                    // Proceed with new customer account creation
                    CreateNewCustomer();
                }
                else
                {
                    // Handle login failure or other actions
                }
                // Return false indicating login failure
                return false;
            }
        }

        static bool CheckExistingCustomer(string username, string password)
        {
            // Check if the entered credentials exist in the Customers table
            using (SqlConnection connection = new SqlConnection(TrainsConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM Customers WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        static void CreateNewCustomer()
        {
            Console.WriteLine("Create New Account:");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            // Insert the new customer record into the Customers table
            using (SqlConnection connection = new SqlConnection(TrainsConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Customers (Username, Password) VALUES (@Username, @Password)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("New account created successfully!");
                    // Proceed with actions for the new customer
                    CustomerMenu();
                }
                else
                {
                    Console.WriteLine("Failed to create a new account. Please try again.");
                    // Handle failure to create a new account
                }
            }
        }

        static void CustomerMenu()
        {
            // Implement customer menu
            Console.WriteLine("---------- Train Reservation ----------------");
            Console.WriteLine();

            bool isLoggedIn = CustomerLogin(); // Check if the customer is logged in

            if (!isLoggedIn)
            {
                // If login failed, return from the method
                return;
            }

            // Customer is logged in, display menu options
            Console.WriteLine("Customer Menu:");
            Console.WriteLine("1. Book Ticket");
            Console.WriteLine("2. Cancel Ticket");
            Console.WriteLine("3. View Booked Tickets");
            Console.WriteLine("4. Exit");

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Enter your choice:");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    BookTicket();
                    break;
                case 2:
                    CancelTicket();
                    break;
                case 3:
                    ViewBookedTickets();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }

        static void BookTicket()
        {
            Console.WriteLine("------------- Train Reservation -----------");
            Console.WriteLine();

            // Assuming 'db' is a valid database context
            Console.Write("Enter From city: ");
            string departureCity = Console.ReadLine();
            var FromTrains = from train in db.Train_Books
                             where train.From == departureCity
                             select train;

            Console.Write("Enter To city: ");
            string arrivalCity = Console.ReadLine();
            var ToTrains = from train in db.Train_Books
                           where train.To == arrivalCity
                           select train;

            if (!FromTrains.Any())
            {
                Console.WriteLine($"No train available from {departureCity} to {arrivalCity}.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Trains Departing from {departureCity} to {arrivalCity}:");
            foreach (var train in FromTrains)
            {
                Console.WriteLine($"Train No: {train.Train_No}");
                Console.WriteLine($"Train Name: {train.Train_Name}");
                Console.WriteLine($"Class: {train.train_class}");
                Console.WriteLine($"Total birth: {train.Total_birth}");
                Console.WriteLine($"Available birth: {train.available_birth}");
                Console.WriteLine($"From: {train.From}");
                Console.WriteLine($"To: {train.To}");
                Console.WriteLine($"price: {train.price}");
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();

            Console.Write("Enter train number: ");
            string trainNumber = Console.ReadLine();
            var trainClasses = from trainClass in db.Train_Books
                               where trainClass.Train_No == trainNumber
                               select trainClass.Class;

            Console.WriteLine($"Available Classes for Train No {trainNumber}:");
            foreach (var trainClass in trainClasses)
            {
                Console.WriteLine(trainClass);
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();

            Console.Write("Enter class name: ");
            string className = Console.ReadLine();
            var classInfo = from tClass in db.Train_Books
                            where tClass.Train_No == trainNumber && tClass.Class == className
                            select new
                            {
                                tClass.Total_Birth,
                                tClass.Available_Birth,
                                tClass.Price // Include price in the query
                            };

            foreach (var info in classInfo)
            {
                Console.WriteLine($"Class: {className}, Total Birth: {info.Total_Birth}, Available Birth: {info.Available_Birth}");
            }

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();

            Console.Write("Enter the number of seats to book: ");
            int seatsToBook;
            if (!int.TryParse(Console.ReadLine(), out seatsToBook))
            {
                Console.WriteLine("Invalid input for number of seats.");
                return;
            }

            string bookingID = GenerateBookingID();
            Console.WriteLine("Generated Booking ID: " + bookingID);

            decimal totalPrice = 0;
            for (int i = 0; i < seatsToBook; i++)
            {
                Console.WriteLine($"Passenger {i + 1} Details:");

                Console.Write("Enter Passenger Name: ");
                string passengerName = Console.ReadLine();

                Console.Write("Enter Passenger Age: ");
                int passengerAge;
                if (!int.TryParse(Console.ReadLine(), out passengerAge))
                {
                    Console.WriteLine("Invalid input for passenger age.");
                    return;
                }

                Console.Write("Enter Passenger Gender (M/F): ");
                string passengerGender = Console.ReadLine().ToUpper();

                decimal? basePrice = classInfo.FirstOrDefault()?.Price;

                if (basePrice.HasValue)
                {
                    decimal passengerPrice = CalculatePassengerPrice(basePrice.Value, passengerAge, passengerGender);
                    totalPrice += passengerPrice;

                    Console.WriteLine($"Ticket Price for Passenger {passengerName}: ${passengerPrice}");
                }
                else
                {
                    Console.WriteLine("Price not found for the selected class.");
                    return;
                }

                Console.WriteLine($"Booking ID for Passenger {passengerName}: {bookingID}");

                // Now, we will insert the booking details into the database
                using (SqlConnection connection = new SqlConnection(TrainsConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO BookedTickets (BookingID, TrainNumber, Class, PassengerName, PassengerAge, PassengerGender) VALUES (@BookingID, @TrainNumber, @Class, @PassengerName, @PassengerAge, @PassengerGender)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@BookingID", bookingID);
                    command.Parameters.AddWithValue("@TrainNumber", trainNumber);
                    command.Parameters.AddWithValue("@Class", className);
                    command.Parameters.AddWithValue("@PassengerName", passengerName);
                    command.Parameters.AddWithValue("@PassengerAge", passengerAge);
                    command.Parameters.AddWithValue("@PassengerGender", passengerGender);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total Price for {seatsToBook} Seats: ${totalPrice}");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine();

            var classToUpdate = db.Train_Books.FirstOrDefault(t => t.Train_No == trainNumber && t.Class == className);
            if (classToUpdate != null)
            {
                classToUpdate.Available_Birth -= seatsToBook;
                db.SubmitChanges();
            }

            Console.WriteLine($"After Booking Available Birth for Class {className}: {classToUpdate.Available_Birth}");
            Console.WriteLine();

            var selectedTrain = FromTrains.FirstOrDefault(t => t.To == arrivalCity);
            if (selectedTrain != null)
            {
                Console.WriteLine($"Successfully {seatsToBook} Seats has been Booked, From {departureCity} To {arrivalCity}...");
                Console.WriteLine();
                Console.WriteLine("---------- Happy Journey -----------");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"No train available from {departureCity} to {arrivalCity}.");
            }
            Console.WriteLine();
        }

        static string GenerateBookingID()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000);
            string bookingID = randomNumber.ToString("D4");
            return bookingID;
        }
        static void CancelTicket()
        {
            Console.WriteLine("Enter Train Number:");
            string trainNumber = Console.ReadLine();

            Console.WriteLine("Enter Class:");
            string ticketClass = Console.ReadLine();

            var bookedTicket = db.Train_Books.FirstOrDefault(t => t.Train_No == trainNumber && t.Class == ticketClass);

            if (bookedTicket != null)
            {
                Console.WriteLine("Ticket found:");
                Console.WriteLine($"Train Name: {bookedTicket.Train_Name}");
                Console.WriteLine($"Class: {bookedTicket.Class}");
                Console.WriteLine($"Available Birth before cancellation: {bookedTicket.Available_Birth}");

                Console.Write("Enter number of tickets to cancel: ");
                int ticketsToCancel = int.Parse(Console.ReadLine());

                if (ticketsToCancel <= bookedTicket.Available_Birth)
                {
                    // Calculate the refund
                    decimal basePrice = bookedTicket.Price ?? 0; // Use default price if null
                    decimal refundAmount = CalculateRefund(basePrice, ticketsToCancel);

                    // Update available births
                    bookedTicket.Available_Birth += ticketsToCancel;
                    db.SubmitChanges();

                    Console.WriteLine($"Successfully canceled {ticketsToCancel} ticket(s).");
                    Console.WriteLine($"Refund Amount: ${refundAmount}");
                }
                else
                {
                    Console.WriteLine("Tickets to cancel exceed the available births.");
                }
            }
            else
            {
                Console.WriteLine("No booking found for the given details.");
            }


            using (SqlConnection connection = new SqlConnection(TrainsConnectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE " +
                    "BookedTickets SET IsCanceled = 1 WHERE BookingID = @BookingID";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Ticket canceled successfully.");
                }
                else
                {
                    Console.WriteLine("Ticket not found or already canceled.");
                }
            }
            Console.ReadLine();

        }

        private static decimal CalculateRefund(decimal basePrice, int ticketsToCancel)
        {
            // Assuming no refund for partially used tickets
            return basePrice * ticketsToCancel;
        }

        static void ViewBookedTickets()
        {
            Console.WriteLine("Enter Train Number:");
            string trainNumber = Console.ReadLine();

            Console.WriteLine("Enter Class:");
            string ticketClass = Console.ReadLine();

            var bookedTickets = db.Train_Books.Where(t => t.Train_No == trainNumber && t.Class == ticketClass);

            if (bookedTickets.Any())
            {
                Console.WriteLine("Booked Tickets:");
                foreach (var ticket in bookedTickets)
                {
                    Console.WriteLine($"Train Name: {ticket.Train_Name}");
                    Console.WriteLine($"Class: {ticket.Class}");
                    Console.WriteLine($"Price: {ticket.Price}");
                    Console.WriteLine($"Available Birth: {ticket.Available_Birth}");

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No booking found for the given details.");
            }

            using (SqlConnection connection = new SqlConnection(TrainsConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM BookedTickets WHERE TrainNumber = @TrainNumber AND Class = @Class";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@TrainNumber", trainNumber);
                command.Parameters.AddWithValue("@Class", ticketClass);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Display booked ticket details
                    Console.WriteLine($"Passenger Name: {reader["PassengerName"]}, Age: {reader["PassengerAge"]}, Gender: {reader["PassengerGender"]}");
                }
            }
            Console.WriteLine("------------ Have a Safe Journey !! ------------");
            Console.ReadLine();

        }

        static void AddTrain()
        {
            Console.WriteLine("Add New Train:");
            Console.Write("Enter Train Number: ");
            string trainNumber = Console.ReadLine();

            // Check if the train number already exists
            var existingTrain = db.Train_Books.FirstOrDefault(t => t.Train_No == trainNumber);
            if (existingTrain != null)
            {
                Console.WriteLine($"Train with number {trainNumber} already exists.");
                return;
            }

            // If the train number doesn't exist, proceed with adding the new train details
            Console.Write("Enter Train Name: ");
            string trainName = Console.ReadLine();
            Console.Write("Enter Class: ");
            string ticketClass = Console.ReadLine();
            Console.Write("Enter Total Birth: ");
            int totalBirth = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Available Birth: ");
            int availableBirth = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter From City: ");
            string fromCity = Console.ReadLine();
            Console.Write("Enter To City: ");
            string toCity = Console.ReadLine();
            Console.Write("Enter Price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            // Add the new train to the database
            var newTrain = new train_book
            {
                Train_No = trainNumber,
                Train_Name = trainName,
                Class = ticketClass,
                Total_Birth = totalBirth,
                Available_Birth = availableBirth,
                From = fromCity,
                To = toCity,
                Price = price
            };

            db.Train_Books.InsertOnSubmit(newTrain);
            db.SubmitChanges();

            Console.WriteLine("New train added successfully!");
            Console.ReadLine();
        }

        static void UpdateTrain()
        {
            Console.WriteLine("Update Train:");
            Console.Write("Enter Train Number to update: ");
            string trainNumber = Console.ReadLine();

            var trainToUpdate = db.Train_Books.FirstOrDefault(t => t.Train_No == trainNumber);
            if (trainToUpdate == null)
            {
                Console.WriteLine($"Train with number {trainNumber} not found.");
                return;
            }

            Console.WriteLine("Select field to update:");
            Console.WriteLine("1. Train Number");
            Console.WriteLine("2. Train Name");
            Console.WriteLine("3. Class");
            Console.WriteLine("4. Total Birth");
            Console.WriteLine("5. Available Birth");
            Console.WriteLine("6. From City");
            Console.WriteLine("7. To City");
            Console.WriteLine("8. Price");

            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter New Train Number: ");
                    trainToUpdate.Train_Name = Console.ReadLine();
                    break;

                case 2:
                    Console.Write("Enter New Train Name: ");
                    trainToUpdate.Train_Name = Console.ReadLine();
                    break;
              
                case 3:
                    Console.Write("Enter New Class: ");
                    trainToUpdate.Class = Console.ReadLine();
                    break;

                case 4:
                    Console.Write("Enter New Total Birth: ");
                    trainToUpdate.Total_Birth = int.Parse(Console.ReadLine());
                    break;

                case 5:
                    Console.Write("Enter New Available Birth: ");
                    trainToUpdate.Available_Birth = int.Parse(Console.ReadLine());
                    break;

                case 6:
                    Console.Write("Enter New From City: ");
                    trainToUpdate.From = Console.ReadLine();
                    break;

                case 7:
                    Console.Write("Enter New To City: ");
                    trainToUpdate.To = Console.ReadLine();
                    break;
           
                case 8:
                    Console.Write("Enter New Price: ");
                    trainToUpdate.Price = decimal.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }

            db.SubmitChanges();
            Console.WriteLine("Train updated successfully!");
            Console.ReadLine();

        }

        static void DeleteTrain()
        {
            Console.WriteLine("Delete Train:");
            Console.Write("Enter Train Number to delete: ");
            string trainNumber = Console.ReadLine();

            var trainToDelete = db.Train_Books.FirstOrDefault(t => t.Train_No == trainNumber);
            if (trainToDelete == null)
            {
                Console.WriteLine($"Train with number {trainNumber} not found.");
                return;
            }

            db.Train_Books.DeleteOnSubmit(trainToDelete);
            db.SubmitChanges();

            Console.WriteLine("Train deleted successfully!");
            Console.ReadLine();

        }

        static void ViewAllTrains()
        {
            Console.WriteLine("All Trains:");
            var allTrains = db.Train_Books;
            foreach (var train in allTrains)
            {
                Console.WriteLine($"Train No: {train.Train_No}, Train Name: {train.Train_Name}, From: {train.From}, To: {train.To}, Class: {train.Class} price: {train.price}");
            }
            Console.ReadLine();

        }

    }
}
