using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Asset_Tracking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            // Base examples list
            List<Electronics> objects = new List<Electronics>()
            {
                new Phones("iPhone","8","Spain",new DateTime(2018,12,29),970),
                new Computers("HP","Elitebook","Spain",new DateTime(2019,06,01),1423),
                new Phones("Xiaomi","Chinarubbish","Spain",new DateTime(2012,01,08),122.99f),
                new Phones("iPhone","11","Sweden",new DateTime(2020,09,25),990),
                new Phones("Nokia","3310","Sweden",new DateTime(2000,10,1),100),
                new Computers("MSI","Katana GF66","UK",new DateTime(2022,05,02),1499.99f),
                new Computers("Asus","W234 GF66","UK",new DateTime(2019,09,25),1200),
                new Phones("Motorola","Somethings","UK",new DateTime(2022,04,18),243.15f),
                new Computers("Lenovo","ThinkCentre","Sweden",new DateTime(2019,09,05),450)
            };

            // Do while loop for repeated adding
            while (true)
            {
                int whichType, counter, price;
                counter = 0;
                string brand, model, answer;
                DateTime date;
                string[] officesList = new string[3]
                {
                    "Spain",
                    "Sweden",
                    "UK"
                };
                string[] officesCurrency = new string[3]
                {
                    "EUR",
                    "SEK",
                    "GBP"
                };
                string[] typeNumber = new string[2] { "Computer", "Phone" };
                ConsoleKeyInfo key;

                // Asking to add more or not. Doing this at the start for testing purposes.
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Add another item? Y/N");
                    answer = Console.ReadLine();
                    if (answer.ToUpper() == "Y")
                    {
                        break;
                    }
                    else if (answer.ToUpper() == "N")
                    {
                        goto LeaveNestedLoop;
                    }
                }
                // asking phone or pc using whichType. 1 == PC, 2 == Phone
                while (true)
                {
                    Console.WriteLine("Is this a Computer or a Phone? 1/2");
                    answer = Console.ReadLine();
                    if (answer == "1" || answer == "2")
                    {
                        whichType = Convert.ToInt32(answer);

                        Console.WriteLine($"\nYou are adding a {typeNumber[whichType - 1]}");
                        break;
                    }
                    Console.Clear();
                }

                // Obtaining item brand and model
                Console.Write("Please provide the item's brand: ");
                brand = Console.ReadLine();
                Console.Write("Please provide the item's model: ");
                model = Console.ReadLine();

                // Obtaining the Office the item belongs to
                do
                {
                    Console.Clear();
                    Console.WriteLine("Which office does your item belong to?");
                    for (int i = 0; i < officesList.Length; i++)
                    {
                        if (i == counter)
                        {
                            Console.WriteLine(">>" + officesList[i]);
                        }
                        else
                        {
                            Console.WriteLine(officesList[i]);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Select your choice with the arrow keys.");
                    Console.ResetColor();
                    key = Console.ReadKey(true);

                    if (key.Key.ToString() == "DownArrow")
                    {
                        counter = (counter >= (officesList.Length - 1)) ? 0 : (counter + 1);
                    }
                    else if (key.Key.ToString() == "UpArrow")
                    {
                        counter = (counter == 0) ? officesList.Length - 1 : (counter - 1);
                    }
                }
                while (key.KeyChar != 13);

                // Reprinting previous statements for clarity
                Console.Clear();
                Console.WriteLine("Is this a Computer or a Phone? 1/2\n" + whichType);
                Console.WriteLine($"\nYou are adding a {typeNumber[whichType - 1]}");
                Console.WriteLine($"Please provide the item's brand: {brand}");
                Console.WriteLine($"Please provide the item's model: {model}");
                Console.WriteLine($"Your item belongs to the office: {officesList[counter]}");

                // Obtaining item price with trycatch for int
                while (true)
                {
                    Console.Write("Please provide the item's price (in GBP): ");
                    try
                    {
                        price = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("\nThis is not a valid number, please try again.");
                    }
                }

                // Obtaining the item's date
                while (true)
                {
                    Console.WriteLine("Is this item to be added today, or from a previous point in time? 1/2");
                    answer = Console.ReadLine();
                    if (answer == "1")
                    {
                        date = DateTime.Now;
                        break;
                    }
                    else if (answer == "2")
                    {
                    DateRetry:
                        Console.Write("Please input a date in the \"yyyy-MM-dd\" format: ");
                        try { date = Convert.ToDateTime(Console.ReadLine()); break; }
                        catch { Console.WriteLine("That was not a valid date. Please try again."); goto DateRetry; }
                    }
                    else { Console.WriteLine("Please answer with '1' or '2'"); }
                }

                // Adding (after prompt for confirmation)
                Console.Clear();
                Console.WriteLine($"Adding a {typeNumber[whichType - 1]}." +
                    $"\nIt is a: '{brand}', model '{model}'. Office {officesList[counter]}. Priced at {price} {officesCurrency[counter]}." +
                    $"\nItem to be added on {date}" +
                    $"\n\nIs this correct? Y/N");
                while (true)
                {
                    answer = Console.ReadLine();
                    if (answer.ToUpper() == "Y")
                    {
                        if (whichType - 1 == 0)
                        {
                            objects.Add(new Computers(brand, model, officesList[counter], date, price));
                        }
                        else if (whichType - 1 == 1)
                        {
                            objects.Add(new Phones(brand, model, officesList[counter], date, price));
                        }
                        break;
                    }
                    else if (answer.ToUpper() == "N")
                    {
                        Console.WriteLine("Item not added.\n");
                        break;
                    }
                }



            }

        LeaveNestedLoop:
            // Title row
            TitlePrinter();

            // Sorting list
            List<Electronics> sortedElec = objects.OrderBy(x => x.Office).ThenByDescending(x => x.dt).ToList();

            // Actual object printing
            float localPrice;
            foreach (var obj in sortedElec)
            {
                // Date checkers
                DateTime expiryDate = DateTime.Now.AddYears(-3);
                if (obj.dt < expiryDate.AddMonths(3))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (obj.dt < expiryDate.AddMonths(6))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                // Printing based on office for "current" prices
                Console.Write(obj.listLayer());
                if (obj.Office == "Spain")
                {
                    localPrice = obj.PriceinUK / 0.84f;
                    Console.WriteLine(String.Format("{0}{1,15}", "     EUR".PadRight(15), localPrice.ToString("C2", CultureInfo.CreateSpecificCulture("es-ES"))));
                }
                else if (obj.Office == "UK")
                {
                    localPrice = obj.PriceinUK;
                    Console.WriteLine(String.Format("{0}{1,15}", "     GBP".PadRight(15), localPrice.ToString("C2", CultureInfo.CreateSpecificCulture("en-GB"))));
                }
                else if (obj.Office == "Sweden")
                {
                    localPrice = obj.PriceinUK / 0.082f;
                    Console.WriteLine(String.Format("{0}{1,15}", "     SEK".PadRight(15), localPrice.ToString("C2", CultureInfo.CreateSpecificCulture("se-SE"))));
                }

                Console.ResetColor();
            }
        }

        static void TitlePrinter()
        {
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}",
               "Type".PadRight(15),
               "Brand".PadRight(15),
               "Model".PadRight(15),
               "Office".PadRight(15),
               "Purchase Date".PadRight(15),
               "Price in GBP".PadRight(15),
               "Currency".PadRight(10),
               "Local Price Today");

            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}",
                "----".PadRight(15),
                "-----".PadRight(15),
                "-----".PadRight(15),
                "------".PadRight(15),
                "-------------".PadRight(15),
                "-----------".PadRight(15),
                "--------".PadRight(10),
                "-----------------");
        }
    }

    class Electronics
    {
        public Electronics(string brand, string model, string office, DateTime dt, float price)
        {
            Brand = brand;
            Model = model;
            Office = office;
            this.dt = dt;
            PriceinUK = price;
        }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public DateTime dt { get; set; }
        public float PriceinUK { get; set; }

        public virtual string listLayer()
        {
            return string.Format("{0,-15}{1,-15}{2,-15}{3,-15}{4,10}",
                this.Brand,
                this.Model,
                this.Office,
                this.dt.ToString("yyyy-MM-dd"),
                this.PriceinUK.ToString("C2", CultureInfo.CreateSpecificCulture("en-GB")));
        }
    }

    class Phones : Electronics
    {
        public Phones(string brand, string model, string office, DateTime dt, float price) : base(brand, model, office, dt, price)
        {

        }
        public override string listLayer()
        {
            return "Phones".PadRight(15) + base.listLayer();
        }
    }

    class Computers : Electronics
    {
        public Computers(string brand, string model, string office, DateTime dt, float price) : base(brand, model, office, dt, price)
        {

        }
        public override string listLayer()
        {
            return "Computers".PadRight(15) + base.listLayer();
        }
    }
}
