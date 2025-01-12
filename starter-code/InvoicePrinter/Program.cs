using System.Drawing;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace InvoicePrinter;


[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal class Program
{
    private static string printString = string.Empty;

    static void Main(string[] args)
    {
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrintPage += new PrintPageEventHandler(PrintPage);
        try
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Insurance");
            List<string> files = Directory.GetFiles(path, "*.customer.txt").ToList();
            foreach(var file in files)
            {
                string name = string.Empty;
                string email = string.Empty;
                string postalCode = string.Empty;
                string price = string.Empty;
                foreach(var line in File.ReadLines(file))
                {
                    if(line.StartsWith("Name:"))
                    {
                       name = line.Substring(6);
                    }
                    else if(line.StartsWith("Email:"))
                    {
                        email = line.Substring(7);
                    }
                    else if(line.StartsWith("Postal Code:"))
                    {
                        postalCode = line.Substring(13);
                    }
                    else if(line.StartsWith("Final Price:"))
                    {
                        price = line.Substring(13);
                    }
                }
                printString = $"Invoice for Insurance\nName: {name}\nEmail: {email}\nPostal Code: {postalCode}\nFinal Price: {price}";
                Console.WriteLine(printString);
                printDocument.Print();
            }
        }
        catch(UnauthorizedAccessException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch(PathTooLongException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void PrintPage(object sender, PrintPageEventArgs e)
    {
        e.Graphics.DrawString(printString, new Font("Arial", 12), new SolidBrush(Color.Black), 10, 25);
    }
}
