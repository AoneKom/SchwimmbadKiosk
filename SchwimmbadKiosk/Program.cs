using System;
using System.Data.SqlClient;

namespace SchwimmbadGastronomieverwaltung
{
    class Program
    {

        static string connectionString = "Server=.;Database=SchwimmbadKiosk;Trusted_Connection=True;";
        static string loggedInUser = null;

        static void Main(string[] args)
        {
        
            Console.Title = "Schwimmbad Gastronomieverwaltung";
        Login:
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("======================================================");
            Console.WriteLine("  Willkommen in der Schwimmbad Gastronomieverwaltung!");
            Console.WriteLine("======================================================\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            

            if (!Login())
            {

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ungültige Anmeldedaten!!!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Console.Clear();
                goto Login;
            }

            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Anmeldung erfolgreich.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Console.Clear();
                
            }
            

            while (true)
            {
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=== Hauptmenü ===\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Bestandsverwaltung");
                Console.WriteLine("2. Verkaufsabwicklung");
                Console.WriteLine("3. Tägliche Umsätze anzeigen");
                Console.WriteLine("4. Programm beenden");
                Console.Write("Bitte wählen Sie eine Option: ");
           
                

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageInventory();

                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        ProcessSales();
                        break;
                    case "3":
                        DisplayDailySales();
                        break;
                    case "4":
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Programm wird beendet. Auf Wiedersehen!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ungültige Auswahl. Bitte erneut versuchen.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
               
                Console.WriteLine("Drücken Sie eine beliebige Taste, um fortzufahren...");
               
            }
        }

      
        public static bool Login()
        {
            Console.Write("Benutzername: ");
            string username = Console.ReadLine();

            Console.Write("Passwort: ");
            string password = Console.ReadLine();
           

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Benutzer WHERE Benutzername = @username AND Passwort = @password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int userCount = (int)cmd.ExecuteScalar();
                        return userCount > 0;
                    }
                }
               
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler bei der Anmeldung: {ex.Message}");
                    return false;
                }
            }
        }

        // Bestandsverwaltung
        public static void ManageInventory()
        {
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Bestandsverwaltung ===\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Inventar anzeigen");
            Console.WriteLine("2. Bestand aktualisieren");
            Console.WriteLine("3. Zurück zum Hauptmenü");
            Console.Write("Bitte wählen Sie eine Option: ");
            Console.ReadKey();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    break;
                case "2":
                   
                    break;
                case "3":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ungültige Auswahl.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                    
            }
          
        }

        static void DisplayInventory()
        {
          
            string query = "SELECT * FROM Inventar";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("ID| Artikelname | Beschreibung | Preis | Bestand");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["ArtikelID"]} | {reader["Artikelname"]} | {reader["Beschreibung"]} | {reader["Preis"]}\t| {reader["Bestand"]}");
                    }
                }
            }
        }

        static void UpdateInventory()
        {
            Console.Write("Artikel-ID: ");
            int artikelID = int.Parse(Console.ReadLine());

            Console.Write("Neue Menge hinzufügen/entfernen (negativ für Entnahme): ");
            int menge = int.Parse(Console.ReadLine());

            string query = "UPDATE Inventar SET Bestand = Bestand + @menge WHERE ArtikelID = @artikelID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@artikelID", artikelID);
                    cmd.Parameters.AddWithValue("@menge", menge);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Bestand erfolgreich aktualisiert." : "Fehler bei der Aktualisierung.");
                }
            }
        }

        // Verkaufsabwicklung
        static void ProcessSales()
        {
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Verkaufsabwicklung ===\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Artikel-ID: ");
            int artikelID = int.Parse(Console.ReadLine());

            Console.Write("Menge: ");
            int menge = int.Parse(Console.ReadLine());

            string getPriceQuery = "SELECT Preis FROM Inventar WHERE ArtikelID = @artikelID";
            decimal preis = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(getPriceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@artikelID", artikelID);
                    preis = (decimal)cmd.ExecuteScalar();
                }
            }

            decimal gesamtpreis = preis * menge;

            string insertSaleQuery = "INSERT INTO Verkäufe (ArtikelID, Menge, Gesamtpreis) VALUES (@artikelID, @menge, @gesamtpreis)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(insertSaleQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@artikelID", artikelID);
                    cmd.Parameters.AddWithValue("@menge", menge);
                    cmd.Parameters.AddWithValue("@gesamtpreis", gesamtpreis);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Verkauf erfolgreich aufgezeichnet." : "Fehler bei der Verkaufsaufzeichnung.");
                }
            }
        }

        
        static void DisplayDailySales()
        {
           
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Tägliche Umsätze anzeigen ===\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Artikel-ID: ");
            _ = int.Parse(Console.ReadLine());
            string query = @"
                SELECT FORMAT(VerkaufDatum, 'yyyy-MM-dd') AS Datum, SUM(Gesamtpreis) AS Gesamtumsatz
                FROM Verkäufe
                GROUP BY FORMAT(VerkaufDatum, 'yyyy-MM-dd')
                ORDER BY Datum DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Datum       | Gesamtumsatz");
                    Console.WriteLine("--------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Datum"]} | {reader["Gesamtumsatz"]}");
                    }
                }
            }
        }
    }
}

