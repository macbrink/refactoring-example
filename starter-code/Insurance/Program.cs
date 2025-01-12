using System.Net;
using System.Net.Mail;

namespace NewCustomer;

internal class Program
{
    static void Main()
    {
        // Input applicant information

        string? name;
        while(string.IsNullOrWhiteSpace(name = Console.ReadLine()))
        {
            Console.WriteLine("Please enter a valid name.");
        }
        int bearthYear;
        while(!int.TryParse(Console.ReadLine(), out bearthYear) || bearthYear < 1900 || bearthYear > DateTime.Now.Year)
        {
            Console.WriteLine("Please enter a valid birth year.");
        }
        string? address;
        while(string.IsNullOrWhiteSpace(address = Console.ReadLine()))
        {
            Console.WriteLine("Please enter a valid address.");
        }
        string? postalCode;
        while(string.IsNullOrWhiteSpace(postalCode = Console.ReadLine()))
        {
            Console.WriteLine("Please enter a valid postal code.");
        }
        string? city;
        while(string.IsNullOrWhiteSpace(city = Console.ReadLine()))
        {
            Console.WriteLine("Please enter a valid city.");
        }

        string? email;
        while(string.IsNullOrWhiteSpace(email = Console.ReadLine()) || !email.Contains("@"))
        {
            Console.WriteLine("Please enter a valid email.");
        }

        // Check if the applicant is eligible for insurance

        // Check if the applicant is at least 18 years old
        if(DateTime.Now.Year - bearthYear >= 18)
        {
            // Set base insurace price
            decimal basePrice = 100;

            // Calculate upchrage based on applicant's postal code
            decimal upcharge = 0;
            if(postalCode.StartsWith("10") || postalCode.StartsWith("11"))
            {
                upcharge = 30;
            }
            else if(postalCode.StartsWith("35"))
            {
                upcharge = 30;
            }
            else if(postalCode.StartsWith("56"))
            {
                upcharge = 10;
            }
            else if(postalCode.StartsWith("30"))
            {
                upcharge = 50;
            }
            else if(postalCode.StartsWith("65"))
            {
                upcharge = 15;
            }
            else if(postalCode.StartsWith("97"))
            {
                upcharge = 70;
            }
            else if(postalCode.StartsWith("7"))
            {
                upcharge = 5;
            }

            decimal discount = 0;
            // If no upcharge has been applied, check for security certificate
            if(upcharge == 0)
            {
                Console.WriteLine("Does the applicant have a security certificate? (yes/no)");
                string? securityCertificate = Console.ReadLine().Trim().ToLower();
                if(securityCertificate != null && securityCertificate == "yes")
                {
                    discount = 15;
                }
            }

            // Calculate final price
            decimal finalPrice = basePrice + upcharge - discount; // TODO: Fire Hazard, etc.


            // Send confirmation email
            var smtpClient = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("username", "password"),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send("insurance@example.com", email, "Insurance Confirmation", $"Dear {name},\n\nYour insurance application has been approved. The final price is {finalPrice}.\n\nBest regards,\nInsurance Company");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the confirmation email: {ex.Message}");
            }

            // show confirmation message on screen
            Console.WriteLine("Insurance Confirmation");
            Console.WriteLine($"Dear {name},\n\nYour insurance application has been approved. The final price is {finalPrice}.\n\nBest regards,\nInsurance Company");

            // Save the applicant information to a file
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Insurance\\{email}.customer.txt");
            File.WriteAllText(path, $"Name: {name}\nBirth Year: {bearthYear}\nAddress: {address}\nPostal Code: {postalCode}\nCity: {city}\nEmail: {email}\nFinal Price: {finalPrice}");
        }
        else
        {
            Console.WriteLine("The applicant is not eligible for insurance.");
        }
    }
}
