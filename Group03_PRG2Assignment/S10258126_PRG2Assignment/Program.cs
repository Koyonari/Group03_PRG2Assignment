using S10258126_PRG2Assignment;

//Basic Features

//Create paths to csv files and read by using relative path to csv file as current working directory is in bin > Debug > .net 6.0 >
string path_customers = Path.Combine("..", "..", "..", "customers.csv");
string path_flavours = Path.Combine("..", "..", "..", "flavours.csv");
string path_options = Path.Combine("..", "..", "..", "options.csv");
string path_orders = Path.Combine("..", "..", "..", "orders.csv");
string path_toppings = Path.Combine("..", "..", "..", "toppings.csv");

string[] data_customers = File.ReadAllLines(path_customers);
string[] data_flavours = File.ReadAllLines(path_flavours);
string[] data_options = File.ReadAllLines(path_options);
string[] data_toppings = File.ReadAllLines(path_toppings);

//Create customers, orders from csv file and make dictionaries/queues
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); // Create a Dictionary to store Customer Objects
Queue<Order> gold_queue = new Queue<Order>();
Queue<Order> regular_queue = new Queue<Order>();

//Create menu method to call
int option = -1;
void DisplayMenu()
{
    try
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

        //Check if option is valid
        if (option < 0 || option > 6)
        {
            throw new ArgumentOutOfRangeException();
        }
    }
    //Catch exceptions for alphabets and options not in range
    catch (FormatException ex)
    {
        Console.WriteLine("Invalid input format! Please enter a number.\n");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Invalid option. Please choose a number between 0 and 6.");
    }
    finally
    {
        Console.WriteLine();
    }
}

//1. List all customers - Done
void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    //Print header
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");

    //Print customer information line by line and skip header
    for (int i = 1; i < data_customers.Length; i++)
    {
        string[] customer_data = data_customers[i].Split(",");
        int customerId = int.Parse(customer_data[1]);
        Customer customer = new Customer(customer_data[0], customerId, DateTime.Parse(customer_data[2]));

        //Add key-pair value in customer dictionary
        customerDict.Add(customerId, customer);

        // Print the combined information
        Console.WriteLine($"{customer}{customer_data[3],-17}{int.Parse(customer_data[4]),-17}{int.Parse(customer_data[5])}");
    }
    Console.WriteLine();
}


//3. Register a new customer - Done
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
        string data = $"{customer.Name},{customer.MemberId},{customer.Dob.ToString("dd/MM/yyyy")},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";
        File.AppendAllText(path_customers, data + Environment.NewLine);

        //Display message to indicate registration status
        Console.WriteLine("Customer registered succesfully!");
    }
    Console.WriteLine();
}

//4. Create a customer's order
void CreateOrder(Dictionary<int, Customer> customerDict)
{
    //List customers name from csv file
    ListAllCustomers(customerDict);

    //Prompt user to select a customer and retrieve selected
    Console.Write("\nEnter customer account Id : ");
    int customer_index = Convert.ToInt32(Console.ReadLine());

    Order current_order = new Order(customer_index, DateTime.Now); //Create Order object

    while (true)
    {
        IceCream iceCream = CreateIceCream(); //Create IceCream object via IceCream Method
        current_order.AddIceCream(iceCream); //Add IceCream object to IceCreamList attribute Order object

        customerDict[customer_index].CurrentOrder = current_order; //Add Order object to CurrentOrder attribute in Customer object

        Console.WriteLine("\nIce Cream added to order.");

        Console.Write("\nAdd another ice cream? y/n : ");
        string continue_order = Console.ReadLine();

        if (continue_order.ToLower() == "n" || continue_order.ToLower() == "no") break; //Check if customer wants to continue adding more IceCream objects
    }

    customerDict[customer_index].OrderHistory.Add(current_order); //Add Order to OrderHistory attribute in Customer object

    //Queue orders
    if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(current_order);
    else regular_queue.Enqueue(current_order);

    Console.WriteLine("Order successfully made.\n");
}

//IceCream Method
IceCream CreateIceCream()
{
    IceCream? iceCream = null; //Initalize IceCream object
    string[] option_menu = { "Cup", "Cone", "Waffle" };
    bool dipped = false;
    string waffle = "";

    Console.WriteLine("\n-Type-");

    //Display Option
    Console.WriteLine("Avilable types:");
    for (int i = 0; i < option_menu.Length; i++)
    {
        Console.WriteLine($"[{i + 1}] {option_menu[i]}");
    }

    //Option
    Console.Write("\nEnter option : ");
    int option = Convert.ToInt32(Console.ReadLine());

    if (option == 2) dipped = Cone();
    else if (option == 3) waffle = Waffle();

    //Scoops
    Console.Write("\n-Scoops-\nEnter number of scoops : ");
    int scoops = Convert.ToInt16(Console.ReadLine());

    //Flavours & Toppings
    List<Flavour> f_list = Flavours(scoops);
    List<Topping> t_list = Toppings();

    switch (option_menu[option - 1]) //Check for Option types
    {
        case "Cup":
            iceCream = new Cup("Cup", scoops, f_list, t_list);
            break;
        case "Cone":
            iceCream = new Cone("Cone", scoops, f_list, t_list, dipped);
            break;
        case "Waffle":
            iceCream = new Waffle("Waffle", scoops, f_list, t_list, waffle);
            break;
    }

    return iceCream;
}

//Cone Method
bool Cone()
{
    Console.Write("\nAdd chocolate-dipped cone? y/n : ");
    string dipped = Console.ReadLine();
    if (dipped.ToLower() == "y" || dipped.ToLower() == "yes") return true;
    else return false;
}

//Waffle Method
string Waffle()
{
    string[] waffle_menu = { "Red velvet", "Charcoal", "Pandan" };
    Console.WriteLine("\nAvailable waffle flavours: ");

    //Display Waffle flavours
    for (int e = 0; e < waffle_menu.Length; e++)
    {
        Console.WriteLine($"[{e + 1}] {waffle_menu[e]}");
    }
    Console.Write("\nSelect waffle flavour : ");
    int w_opt = Convert.ToInt32(Console.ReadLine());
    return waffle_menu[w_opt - 1];
}

//Flavour Method
List<Flavour> Flavours(int scoops)
{
    //Make list of flavours
    List<string> flavour_menu = new List<string>();
    for (int i = 1; i < data_flavours.Length; i++)
    {
        string[] flavour_name = data_flavours[i].Split(",");
        flavour_menu.Add(flavour_name[0]);
    }

    List<Flavour> f_list = new List<Flavour>();

    Console.Write("\n-Flavours-");

    //Loop for number of scoops
    for (int i = 0; i < scoops;)
    {
        bool premium = false;
        int quantity = 1;

        //Display Flavours
        Console.WriteLine("\nAvailable Flavours: ");
        for (int j = 0; j < flavour_menu.Count; j++)
        {
            if (j == 0) Console.WriteLine("Ordinary");
            else if (j == 3) Console.WriteLine("Premium");
            Console.WriteLine($"[{j + 1}] {flavour_menu[j]}");
        }

        Console.Write("\nEnter flavour option : ");
        int f_opt = Convert.ToInt16(Console.ReadLine());
        if (f_opt > 3 && f_opt < 7) premium = true; //Check if flavour selected is premium
        if (scoops == 1 || (i == 1 && scoops != 3) || (i == 2 && scoops == 3))
        {
            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            break;
        }
        else
        {
            Console.Write("Enter scoops of flavour : ");
            quantity = Convert.ToInt16(Console.ReadLine());
            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            i += quantity;
        }
    }
    return f_list;
}

//Topping Method
List<Topping> Toppings()
{
    List<string> toppings_menu = new List<string>();
    for (int i = 1; i < data_toppings.Length; i++)
    {
        string[] topping_name = data_toppings[i].Split(",");
        toppings_menu.Add(topping_name[0]);
    }
    List<Topping> t_list = new List<Topping>();

    Console.WriteLine("\n-Toppings-");

    while (true)
    {
        if (t_list.Count == 4) //Check for max number of toppings
        {
            Console.WriteLine("Topping limit reached.");
            break;
        }

        Console.Write("Add toppings? y/n : ");
        string continue_topping = Console.ReadLine();
        if (continue_topping == "n") break; //Check if user wants to add toppings
        else if (continue_topping == "y")
        {
            //Display Toppings
            Console.WriteLine("\nAvailable Toppings:");
            for (int i = 0; i < toppings_menu.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {toppings_menu[i]}");
            }

            Console.Write("\nEnter topping option : ");
            int t_opt = Convert.ToInt16(Console.ReadLine());
            Topping topping = new Topping(toppings_menu[t_opt - 1]);
            t_list.Add(topping);
        }
    }
    return t_list;
}

//MAIN CODE
//Use while loop to keep calling menu method until exited
do
{
    DisplayMenu();

    try
    {
        //1. List all customers
        if (option == 1)
        {
            customerDict.Clear(); //Ensures that any existing customer data is removed to prevent duplicate keys.
            ListAllCustomers(customerDict);
        }
        //3. Register a new customer
        else if (option == 3)
        {
            RegisterCustomer();
        }
        //4. Create a customer's order
        else if (option == 4)
        {
            customerDict.Clear(); //Ensures that any existing customer data is removed to prevent duplicate keys.
            CreateOrder(customerDict);
        }
    }
    //Catch exceptions
    catch (FormatException ex)
    {
        Console.WriteLine("Invalid input format! Please enter a number.\n");
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine("CSV file(s) not found. Please check the file path and try again.\n");
    }
    catch (IOException ex)
    {
        Console.WriteLine("Error reading or writing to CSV file(s). Please ensure it's accessible.\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occurred: " + ex.Message + "\n");
    }

} while (option != 0); //Loop until user enters 0 to exit