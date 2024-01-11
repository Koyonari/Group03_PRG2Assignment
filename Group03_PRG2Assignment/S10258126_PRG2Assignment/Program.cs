using S10258126_PRG2Assignment;

//Basic Features

//Create customers and orders from csv file
string path = Path.Combine("..", "..", "..", "customers.csv"); //Relative path to csv file as current working directory is in bin > Debug > .net 6.0 >
string[] data = File.ReadAllLines(path);

//Create menu method to call
int option = 10;
void DisplayMenu()
{
    Console.WriteLine("------------- MENU --------------");
    Console.WriteLine("[1] List all customers");
    Console.WriteLine("[2] List all current orders");
    Console.WriteLine("[3] Register a new customer");
    Console.WriteLine("[4] Create a customer's order");
    Console.WriteLine("[5] Display order details of a customer");
    Console.WriteLine("[6] Modify order details");
    Console.WriteLine("[0] Exit");
    Console.WriteLine("---------------------------------");
    Console.Write("Enter your option: ");
    option = int.Parse(Console.ReadLine());
}

//1. List all customers
void ListCustomers()
{
    Console.WriteLine();
}

//3. Register a new customer
void RegisterCustomer()
{
    //Prompt user for info
    Console.Write("Please enter customers information (name, id number, date of birth): ");
    string[] customer_data = Console.ReadLine().Split(",");

    //Create customer object with info
    if (customer_data.Length != 3)
    {
        Console.WriteLine("Invalid input. Please enter in the following format: Name, Id Number, Date of Birth");
    }
    else
    {
        string cname = customer_data[0];
        int cid = int.Parse(customer_data[1]);
        DateTime cdob = DateTime.Parse(customer_data[2]);
        Customer customer = new Customer(cname, cid, cdob);

        //Create Pointcard object
        PointCard newcard = new PointCard(0, 0);

        //Assign Pointcard object to customer
        customer.Rewards = newcard;

        //Append customer information to csv file
        string data = $"{customer.Name},{customer.MemberId},{customer.Dob.ToString("dd/MM/yyyy")}";
        File.AppendAllText(path, data + Environment.NewLine);

        //Display message to indicate registration status
        Console.WriteLine("Customer registered succesfully!");
    }
    Console.WriteLine();
}

//4. Create a customer's order
void CreateOrder()
{
    //List customers name from csv file
    string[] data = File.ReadAllLines(path);
    List<string> customer_names = data.ToList();
    customer_names.RemoveAt(0);
    foreach (string line in customer_names)
    {
        string[] customer_data = line.Split(",");
        Console.WriteLine(customer_data[0]);
    }

    //Prompt user to select a customer and retrieve selected
    Console.Write("Please select a customer: ");
    string customer_name = Console.ReadLine();

    bool found = false;
    foreach (string customer in customer_names)
    {
        string[] customer_data = customer.Split(",");

        if (customer_name.ToLower() == customer_data[0].ToLower())
        {
            found = true;
            Console.WriteLine($"Customer found: {customer_data[0]}");
            break;
        }
    }
    if (found == false)
    {
        Console.WriteLine("Customer not found. Please try again");
    }

    //Create order object
    Order neworder = new Order();

    string add_another = "";
    do
    {
        //Prompt user to enter ice cream details
        Console.Write("Please enter ice cream details (option, scoops, flavours, toppings): ");

        //Create ice cream object
        string[] icecream_details = Console.ReadLine().Split(",");

        string option = icecream_details[0];
        int scoops = int.Parse(icecream_details[1]);
        List<Flavour> flavours = new List<Flavour>();
        string[] type_premium_quantity = icecream_details[2].Split(" ");
        flavours.Add(new Flavour(type_premium_quantity[0], bool.Parse(type_premium_quantity[1]), int.Parse(type_premium_quantity[2])));
        List<Topping> toppings = new List<Topping>();
        toppings.Add(new Topping(icecream_details[3]));

        //Prompt user to add another ice cream
        Console.Write("Add another ice cream? (Y/N): ");
        add_another = Console.ReadLine();
    } while (add_another.ToLower() == "y" || add_another.ToLower() == "yes");

    Console.WriteLine();
}

//Use while loop to keep calling menu method until exited
do
{
    DisplayMenu();

    try
    {
        //1. List all customers
        if (option == 1)
        {

        }
        //3. Register a new customer
        else if (option == 3)
        {
            RegisterCustomer();
        }
        //4. Create a customer's order
        else if (option == 4)
        {
            CreateOrder();
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Invalid input format! Please enter a number.\n");
    }
} while (option != 0);