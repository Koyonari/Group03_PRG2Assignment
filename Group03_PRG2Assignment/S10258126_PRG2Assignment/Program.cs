using S10258126_PRG2Assignment;
using System.Runtime.Versioning;

//Create Dictionary to store Customer Objects and Queues to store gold/regular
string customerFile = "customers.csv";
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //Create a Dictionary to store Customer Objects
Queue<Order> gold_queue = new Queue<Order>();
Queue<Order> regular_queue = new Queue<Order>();

//Create methods to do the corresponding to help with features

//Method 1: Create Menu - Main
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

//Method 2: Read customers csv file - General use
void ExtractCustomer(string filename)
{
    string[] customer_file = File.ReadAllLines(filename); //Reads each line in customers csv file and store in array

    //For loop to read each line in customers csv file
    for (int i = 1; i < customer_file.Length; i++)
    {
        string[] customer_details = customer_file[i].Split(","); //Split to get each customer's details

        Customer customer = new Customer(customer_details[0], Convert.ToInt32(customer_details[1]), Convert.ToDateTime(customer_details[2])); //Creates Customer Object
        customer.Rewards = new PointCard(Convert.ToInt32(customer_details[4]), Convert.ToInt32(customer_details[5])); ////Create PointCard object in Customer object
        customerDict.Add(customer.MemberId, customer); //Adds Customer Object to Dictionary
    }
}

//Method 3: Data Validation for Integers - General use
int ValidateInt(string text, int range, bool allowzero)
{
    int min = 0;
    int option = 0;
    bool flag = true;

    if (allowzero == true) min = -1;

    while (flag)
    {
        try
        {
            Console.Write(text);
            option = Convert.ToInt32(Console.ReadLine());
            if (option > min && option <= range)
            {
                flag = false;
            }
            else Console.WriteLine("Option not available.");
        }
        catch
        {
            Console.WriteLine("Invalid Input. Please input a number.");
        }
    }
    return option;
}

//Basic Features
//Feature 1: List all customers - An Yong Shyan (Done)
void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    //Print header
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");

    //Print customer information line by line and skip header
    foreach (KeyValuePair<int, Customer> customer in customerDict)
    {
        Console.WriteLine($"{customer.Value}{customer.Value.Rewards.Tier,-17}{customer.Value.Rewards.Points,-17}{customer.Value.Rewards.PunchCard}");
    }
    Console.WriteLine();
}

//Feature 2: List all current orders in gold and regular queue - Jake Chan Man Lock (Done)
void ListAllCurrentOrders(Dictionary<int, Customer> customerDict, Queue<Order> gold_queue, Queue<Order> regular_queue)
{
    if (gold_queue.Count > 0) //checks for empty queue
    {
        Console.WriteLine("Gold member queue:");
        foreach (Order i in gold_queue)
        {
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Gold member queue empty.");

    if (regular_queue.Count > 0) //checks for empty queue
    {
        Console.WriteLine("Regular member queue:");
        foreach (Order i in regular_queue)
        {
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Regular member queue empty.");
}

//Feature 3: Register a new customer and store in csv file - An Yong Shyan (Done)
void RegisterNewCustomer(Dictionary<int, Customer> customerDict, string filename)
{
    Console.Write("Enter your name : ");
    string name = Console.ReadLine();

    Console.Write("Enter your ID : ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter your date of birth DD/MM/YYYY : ");
    DateTime dob = DateTime.Parse(Console.ReadLine(), null);

    Customer customer = new Customer(name, id, dob);    //Create Customer object
    customer.Rewards = new PointCard(0, 0);             //Create PointCard object in Customer object
    customerDict.Add(customer.MemberId, customer);      //Add Customer to customer dictionary

    string updatefile = $"{customer.Name},{Convert.ToInt32(customer.MemberId)},{customer.Dob.ToString("dd/MM/yyyy")},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

    File.AppendAllText(filename, updatefile + Environment.NewLine);
}

//Feature 4: Cone method
bool Cone()
{
    Console.Write("\nAdd chocolate-dipped cone? y/n : ");
    string dipped = Console.ReadLine();
    if (dipped == "y") return true;
    else return false;
}

//Feature 4: Waffle method
string Waffle()
{
    string[] waffle_menu = { "Red velvet", "Charcoal", "Pandan" };
    Console.WriteLine("\nAvailable waffle flavours: ");

    //Display Waffle flavours
    for (int e = 0; e < waffle_menu.Length; e++)
    {
        Console.WriteLine($"[{e + 1}] {waffle_menu[e]}");
    }

    //****
    //Console.Write("\nSelect waffle flavour : ");
    //int w_opt = Convert.ToInt32(Console.ReadLine());

    int w_opt = ValidateInt("\nSelect waffle flavour : ", 3, false);

    return waffle_menu[w_opt - 1];
}

//Feature 4: Flavour method
List<Flavour> Flavours(int scoops)
{
    string[] flavour_menu = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea salt" };
    List<Flavour> f_list = new List<Flavour>();

    Console.Write("\n-Flavours-");

    //Loop for number of scoops
    for (int i = 0; i < scoops;)
    {
        bool premium = false;
        int quantity = 1;

        //Display Flavours
        Console.WriteLine("\nAvailable Flavours: ");
        for (int j = 0; j < flavour_menu.Length; j++)
        {
            if (j == 0) Console.WriteLine("Ordinary");
            else if (j == 3) Console.WriteLine("Premium");
            Console.WriteLine($"[{j + 1}] {flavour_menu[j]}");
        }

        //****
        //Console.Write("\nEnter flavour option : ");
        //int f_opt = Convert.ToInt16(Console.ReadLine());

        int f_opt = ValidateInt("\nEnter flavour option : ", 6, false);

        if (f_opt > 3 && f_opt < 7) premium = true; //Check if flavour selected is premium
        if (scoops == 1 || (i == 1 && scoops != 3) || (i == 2 && scoops == 3))
        {
            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            break;
        }
        else
        {
            //****
            //Console.Write("Enter scoops of flavour : ");
            //quantity = Convert.ToInt16(Console.ReadLine());

            quantity = ValidateInt("\nEnter scoops of flavour : ", scoops - i, false);

            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            i += quantity;
        }
    }
    return f_list;
}

//Feature 4: Topping method
List<Topping> Toppings()
{
    string[] topping_menu = { "Sprinkles", "Mochi", "Sago", "Oreos" };
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
            for (int i = 0; i < topping_menu.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {topping_menu[i]}");
            }

            //****
            //Console.Write("\nEnter topping option : ");
            //int t_opt = Convert.ToInt16(Console.ReadLine());

            int t_opt = ValidateInt("\nEnter topping option : ", 4, false);

            Topping topping = new Topping(topping_menu[t_opt - 1]);
            t_list.Add(topping);
        }
    }
    return t_list;
}

//Feature 4: IceCream Method
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
    //****
    //Console.Write("\nEnter option : ");
    //int option = Convert.ToInt32(Console.ReadLine());

    int option = ValidateInt("\nEnter option : ", 3, false);

    if (option == 2) dipped = Cone();
    else if (option == 3) waffle = Waffle();

    //Scoops
    Console.WriteLine("-Scoops-");
    //****
    //Console.Write("\n-Scoops-\nEnter number of scoops : ");
    //int scoops = Convert.ToInt16(Console.ReadLine());

    int scoops = ValidateInt("\nEnter number of scoops : ", 3, false);

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

//Feature 4: Create customer order - An Yong Shyan
void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{
    ListAllCustomers(customerDict); //Feature 1

    Console.Write("\nEnter customer account Id : ");
    int customer_index = Convert.ToInt32(Console.ReadLine());

    Order current_order = new Order(customer_index, DateTime.Now); //Create Order object

    while (true)
    {
        string continue_order = "";
        IceCream iceCream = CreateIceCream(); //Create IceCream object via IceCream Method
        current_order.AddIceCream(iceCream); //Add IceCream object to IceCreamList attribute Order object

        customerDict[customer_index].CurrentOrder = current_order; //Add Order object to CurrentOrder attribute in Customer object

        Console.WriteLine("\nIce Cream added to order.");

        try
        {
            while (true)
            {
                Console.Write("\nAdd another ice cream? y/n : ");
                continue_order = Console.ReadLine();

                if (continue_order.ToLower() == "n" || continue_order.ToLower() == "y") break; //Check if customer wants to continue adding more IceCream objects
                else if (continue_order.ToLower() != "y") Console.WriteLine("Invalid Input. Please input y/n.");
            }
            break;
        }
        catch
        {
            Console.WriteLine("Invalid Input. Please input y/n.");
        }
        if (continue_order.ToLower() != "y") break; //Check if customer wants to continue adding more IceCream objects
        else Console.WriteLine("Invalid Input. Please input y/n.");
    }
    customerDict[customer_index].OrderHistory.Add(current_order); //Add Order to OrderHistory attribute in Customer object

    //Queue orders
    if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(current_order);
    else regular_queue.Enqueue(current_order);

    Console.WriteLine("Order successfully made.\n");
}

//Feature 5: Display Past Order History Method
void DisplayOrderHistory(Dictionary<int, Customer> customerDict, int index)
{
    Console.WriteLine("Order History:");
    Console.WriteLine("______________________________________");

    //Loops through order history & Prints each order
    for (int i = 0; i < customerDict[index].OrderHistory.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {customerDict[index].OrderHistory[i]}");
        Console.WriteLine($"{"_________________________________",-20}\n");

        //Loops through order & Prints ice cream
        for (int j = 0; j < customerDict[index].OrderHistory[i].IceCreamList.Count; j++)
        {
            Console.WriteLine($"{"(" + (j + 1) + ")",8}{customerDict[index].OrderHistory[i].IceCreamList[j]}");

            //Loops through ice cream flavours & Prints each flavour
            for (int k = 0; k < customerDict[index].OrderHistory[i].IceCreamList[j].Flavours.Count; k++)
            {
                Console.WriteLine("\t" + customerDict[index].OrderHistory[i].IceCreamList[j].Flavours[k]);
            }

            //Loops through ice cream toppings & Prints each topping
            for (int k = 0; k < customerDict[index].OrderHistory[i].IceCreamList[j].Toppings.Count; k++)
            {
                Console.WriteLine("\t" + customerDict[index].OrderHistory[i].IceCreamList[j].Toppings[k]);
            }
            Console.WriteLine();
        }
        Console.WriteLine("______________________________________");
    }
}

//Feature 5: Display Current Order History Method
void DisplayCurrentOrder(Dictionary<int, Customer> customerDict, int index)
{
    Console.WriteLine("\nCurrent Order:");
    Console.WriteLine("______________________________________\n");

    //Loops through order & Prints ice cream
    for (int j = 0; j < customerDict[index].CurrentOrder.IceCreamList.Count; j++)
    {
        Console.WriteLine($"{"(" + (j + 1) + ")",-4}{customerDict[index].CurrentOrder.IceCreamList[j]}");

        //Loops through ice cream flavours & Prints each flavour
        for (int k = 0; k < customerDict[index].CurrentOrder.IceCreamList[j].Flavours.Count; k++)
        {
            Console.WriteLine("    " + customerDict[index].CurrentOrder.IceCreamList[j].Flavours[k]);
        }

        //Loops through ice cream toppings & Prints each topping
        for (int k = 0; k < customerDict[index].CurrentOrder.IceCreamList[j].Toppings.Count; k++)
        {
            Console.WriteLine("    " + customerDict[index].CurrentOrder.IceCreamList[j].Toppings[k]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("______________________________________");
}

//Feature 5: Display Order Details - Jake Chan Man Lock
void DisplayOrderDetails(Dictionary<int, Customer> customerDict)
{
    ListAllCustomers(customerDict); //Feature 1

    Console.Write("\nEnter customer account Id : ");
    int customer_index = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine();

    DisplayOrderHistory(customerDict, customer_index); //Display order history Method
    DisplayCurrentOrder(customerDict, customer_index); //Display current order Method

    Console.WriteLine("______________________________________");
}

//Feature 6: Option 1 Method
void Option1(Dictionary<int, Customer> customerDict, int index)
{
    string iceCream_menu =
        "=== Edit Ice Cream ===\n" +
        "[1] Option\n" +
        "[2] Scoops\n" +
        "[3] Flavours\n" +
        "[4] Toppings";

    //****
    //Console.Write("Select Ice Cream to modify : ");
    //int m_opt = Convert.ToInt32(Console.ReadLine());

    int m_opt = ValidateInt("\nSelect Ice Cream to modify : ", customerDict[index].CurrentOrder.IceCreamList.Count, false);

    Console.Write("\n" + iceCream_menu + "");

    //****
    //Console.Write("Select component to modify : ");
    //int i_opt = Convert.ToInt32(Console.ReadLine());

    int i_opt = ValidateInt("\nSelect component to modify : ", 4, false);

    IceCream modify_IceCream = customerDict[index].CurrentOrder.IceCreamList[m_opt - 1];

    switch (i_opt)
    {
        case 1: //Change Option
            string[] option_menu = { "Cup", "Cone", "Waffle" };

            Console.WriteLine("\nAvilable types:");
            for (int i = 0; i < option_menu.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {option_menu[i]}");
            }

            //****
            //Console.Write("\nEnter option : ");
            //int option = Convert.ToInt32(Console.ReadLine());

            int option = ValidateInt("\nEnter option : ", 3, false);

            if (option == 1)
            {
                modify_IceCream = new Cup("Cup", modify_IceCream.Scoops, modify_IceCream.Flavours, modify_IceCream.Toppings);
            }
            else if (option == 2)
            {
                bool dipped = Cone(); //Feature 4 - Cone Method
                modify_IceCream = new Cone("Cone", modify_IceCream.Scoops, modify_IceCream.Flavours, modify_IceCream.Toppings, dipped);
            }
            else if (option == 3)
            {
                string waffleFlavour = Waffle(); //Feature 4 - Waffle Method
                modify_IceCream = new Waffle("Waffle", modify_IceCream.Scoops, modify_IceCream.Flavours, modify_IceCream.Toppings, waffleFlavour);
            }
            customerDict[index].CurrentOrder.IceCreamList[m_opt - 1] = modify_IceCream; //Replace existing IceCream object with new object
            break;
        case 2: //Change Scoops
            Console.WriteLine("-Scoops-");
            //****
            //Console.Write("\nEnter number of scoops : ");
            //int scoops = Convert.ToInt16(Console.ReadLine());

            int scoops = ValidateInt("\nEnter number of scoops : ", 3, false);

            modify_IceCream.Scoops = scoops;
            modify_IceCream.Flavours = Flavours(scoops); //Feature 4 - Flavour Method
            break;
        case 3: //Change Flavours
            modify_IceCream.Flavours = Flavours(modify_IceCream.Scoops); //Feature 4 - Flavour Method
            break;
        case 4: //Change Toppings
            modify_IceCream.Toppings = Toppings(); //Feature 4 - Topping Method
            break;
    }
}

//Feature 6: Option 2 Method
void Option2(Dictionary<int, Customer> customerDict, int index)
{
    IceCream new_IceCream = CreateIceCream(); //Feature 4 - IceCream Method

    customerDict[index].CurrentOrder.AddIceCream(new_IceCream);
    Console.WriteLine("\nNew Ice Cream added successfully.");
}

//Feature 6: Option 3 Method
void Option3(Dictionary<int, Customer> customerDict, int index)
{
    if (customerDict[index].CurrentOrder.IceCreamList.Count > 1)
    {
        //****
        //Console.Write("Select Ice Cream to delete : ");
        //int d_opt = Convert.ToInt32(Console.ReadLine());

        int d_opt = ValidateInt("\nSelect Ice Cream to delete : ", customerDict[index].CurrentOrder.IceCreamList.Count, false);

        customerDict[index].CurrentOrder.DeleteIceCream(d_opt - 1);
    }
    else Console.WriteLine("You cannot have 0 ice creams in an order.");
}

//Feature 6: Modify order details - Jake Chan Man Lock
void ModifyOrderDetails(Dictionary<int, Customer> customerDict)
{
    string edit_menu =
        "=== Edit Order Menu ===\n" +
        "[1] Modify Ice Cream\n" +
        "[2] Add Ice Cream\n" +
        "[3] Delete Ice Cream\n";

    ListAllCustomers(customerDict); //Feature 1

    Console.Write("\nEnter customer account Id : ");
    int customer_index = Convert.ToInt32(Console.ReadLine());

    DisplayCurrentOrder(customerDict, customer_index); //Feature 5 - Display current order Method

    Console.Write("\n" + edit_menu);

    //****
    //Console.Write("\n" + edit_menu + "\nSelect option : ");
    //int menu_opt = Convert.ToInt32(Console.ReadLine());

    int menu_opt = ValidateInt("\nSelect option : ", 3, false);

    switch (menu_opt)
    {
        case 1:
            Option1(customerDict, customer_index);
            break;
        case 2:
            Option2(customerDict, customer_index);
            break;
        case 3:
            Option3(customerDict, customer_index);
            break;
    }

    DisplayCurrentOrder(customerDict, customer_index); //Feature 5 - Display current order Method
}

//Main Program
ExtractCustomer(customerFile); //Read customers csv file Method

do
{
    DisplayMenu();

    try
    {
        //1. List all customers
        if (option == 1)
        {
            ListAllCustomers(customerDict);
            Console.WriteLine();
            option = -1;
        }
        //2. List all current orders
        else if (option == 2)
        {
            ListAllCurrentOrders(customerDict, gold_queue, regular_queue);
            Console.WriteLine();
            option = -1;
        }
        //3. Register a new customer
        else if (option == 3)
        {
            RegisterNewCustomer(customerDict, customerFile);
            Console.WriteLine();
            option = -1;
        }
        //4. Create a customer's order
        else if (option == 4)
        {
            CreateCustomerOrder(customerDict);
            Console.WriteLine();
            option = -1;
        }
        //5. Display order details of a customer
        else if (option == 5)
        {
            DisplayOrderDetails(customerDict);
            Console.WriteLine();
            option = -1;
        }
        //6. Modify order details
        else if (option == 6)
        {
            ModifyOrderDetails(customerDict);
            Console.WriteLine();
            option = -1;
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


////Basic Features

////Create paths to csv files and read by using relative path to csv file as current working directory is in bin > Debug > .net 6.0 >
//string path_customers = Path.Combine("..", "..", "..", "customers.csv");
//string path_flavours = Path.Combine("..", "..", "..", "flavours.csv");
//string path_options = Path.Combine("..", "..", "..", "options.csv");
//string path_orders = Path.Combine("..", "..", "..", "orders.csv");
//string path_toppings = Path.Combine("..", "..", "..", "toppings.csv");

//string[] data_customers = File.ReadAllLines(path_customers);
//string[] data_flavours = File.ReadAllLines(path_flavours);
//string[] data_options = File.ReadAllLines(path_options);
//string[] data_toppings = File.ReadAllLines(path_toppings);

////Create customers, orders from csv file and make dictionaries/queues
//Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); // Create a Dictionary to store Customer Objects
//Queue<Order> gold_queue = new Queue<Order>();
//Queue<Order> regular_queue = new Queue<Order>();

////Create menu method to call
//int option = -1;
//void DisplayMenu()
//{
//    try
//    {
//        Console.WriteLine("------------- MENU --------------");
//        Console.WriteLine("[1] List all customers");
//        Console.WriteLine("[2] List all current orders");
//        Console.WriteLine("[3] Register a new customer");
//        Console.WriteLine("[4] Create a customer's order");
//        Console.WriteLine("[5] Display order details of a customer");
//        Console.WriteLine("[6] Modify order details");
//        Console.WriteLine("[0] Exit");
//        Console.WriteLine("---------------------------------");
//        Console.Write("Enter your option: ");
//        option = int.Parse(Console.ReadLine());

//        //Check if option is valid
//        if (option < 0 || option > 6)
//        {
//            throw new ArgumentOutOfRangeException();
//        }
//    }
//    //Catch exceptions for alphabets and options not in range
//    catch (FormatException ex)
//    {
//        Console.WriteLine("Invalid input format! Please enter a number.\n");
//    }
//    catch (ArgumentOutOfRangeException ex)
//    {
//        Console.WriteLine("Invalid option. Please choose a number between 0 and 6.");
//    }
//    finally
//    {
//        Console.WriteLine();
//    }
//}

////1. List all customers - Done
//void ListAllCustomers(Dictionary<int, Customer> customerDict)
//{
//    //Print header
//    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");

//    //Print customer information line by line and skip header
//    for (int i = 1; i < data_customers.Length; i++)
//    {
//        string[] customer_data = data_customers[i].Split(",");
//        int customerId = int.Parse(customer_data[1]);
//        Customer customer = new Customer(customer_data[0], customerId, DateTime.Parse(customer_data[2]));

//        //Add key-pair value in customer dictionary
//        customerDict.Add(customerId, customer);

//        // Print the combined information
//        Console.WriteLine($"{customer}{customer_data[3],-17}{int.Parse(customer_data[4]),-17}{int.Parse(customer_data[5])}");
//    }
//    Console.WriteLine();
//}


////3. Register a new customer - Done
//void RegisterCustomer()
//{
//    //Prompt user for info
//    Console.Write("Please enter customers information (name, id number, date of birth): ");
//    string[] customer_data = Console.ReadLine().Split(",");

//    //Create customer object with info
//    if (customer_data.Length != 3)
//    {
//        Console.WriteLine("Invalid input. Please enter in the following format: Name, Id Number, Date of Birth");
//    }
//    else
//    {
//        string cname = customer_data[0];
//        int cid = int.Parse(customer_data[1]);
//        DateTime cdob = DateTime.Parse(customer_data[2]);
//        Customer customer = new Customer(cname, cid, cdob);

//        //Create Pointcard object
//        PointCard newcard = new PointCard(0, 0);

//        //Assign Pointcard object to customer
//        customer.Rewards = newcard;

//        //Append customer information to csv file
//        string data = $"{customer.Name},{customer.MemberId},{customer.Dob.ToString("dd/MM/yyyy")},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";
//        File.AppendAllText(path_customers, data + Environment.NewLine);

//        //Display message to indicate registration status
//        Console.WriteLine("Customer registered succesfully!");
//    }
//    Console.WriteLine();
//}

////4. Create a customer's order
//void CreateOrder(Dictionary<int, Customer> customerDict)
//{
//    //List customers name from csv file
//    ListAllCustomers(customerDict);

//    //Prompt user to select a customer and retrieve selected
//    Console.Write("\nEnter customer account Id : ");
//    int customer_index = Convert.ToInt32(Console.ReadLine());

//    Order current_order = new Order(customer_index, DateTime.Now); //Create Order object

//    while (true)
//    {
//        IceCream iceCream = CreateIceCream(); //Create IceCream object via IceCream Method
//        current_order.AddIceCream(iceCream); //Add IceCream object to IceCreamList attribute Order object

//        customerDict[customer_index].CurrentOrder = current_order; //Add Order object to CurrentOrder attribute in Customer object

//        Console.WriteLine("\nIce Cream added to order.");

//        Console.Write("\nAdd another ice cream? y/n : ");
//        string continue_order = Console.ReadLine();

//        if (continue_order.ToLower() == "n" || continue_order.ToLower() == "no") break; //Check if customer wants to continue adding more IceCream objects
//    }

//    customerDict[customer_index].OrderHistory.Add(current_order); //Add Order to OrderHistory attribute in Customer object

//    //Queue orders
//    if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(current_order);
//    else regular_queue.Enqueue(current_order);

//    Console.WriteLine("Order successfully made.\n");
//}

////IceCream Method
//IceCream CreateIceCream()
//{
//    IceCream? iceCream = null; //Initalize IceCream object
//    string[] option_menu = { "Cup", "Cone", "Waffle" };
//    bool dipped = false;
//    string waffle = "";

//    Console.WriteLine("\n-Type-");

//    //Display Option
//    Console.WriteLine("Avilable types:");
//    for (int i = 0; i < option_menu.Length; i++)
//    {
//        Console.WriteLine($"[{i + 1}] {option_menu[i]}");
//    }

//    //Option
//    Console.Write("\nEnter option : ");
//    int option = Convert.ToInt32(Console.ReadLine());

//    if (option == 2) dipped = Cone();
//    else if (option == 3) waffle = Waffle();

//    //Scoops
//    Console.Write("\n-Scoops-\nEnter number of scoops : ");
//    int scoops = Convert.ToInt16(Console.ReadLine());

//    //Flavours & Toppings
//    List<Flavour> f_list = Flavours(scoops);
//    List<Topping> t_list = Toppings();

//    switch (option_menu[option - 1]) //Check for Option types
//    {
//        case "Cup":
//            iceCream = new Cup("Cup", scoops, f_list, t_list);
//            break;
//        case "Cone":
//            iceCream = new Cone("Cone", scoops, f_list, t_list, dipped);
//            break;
//        case "Waffle":
//            iceCream = new Waffle("Waffle", scoops, f_list, t_list, waffle);
//            break;
//    }

//    return iceCream;
//}

////Cone Method
//bool Cone()
//{
//    Console.Write("\nAdd chocolate-dipped cone? y/n : ");
//    string dipped = Console.ReadLine();
//    if (dipped.ToLower() == "y" || dipped.ToLower() == "yes") return true;
//    else return false;
//}

////Waffle Method
//string Waffle()
//{
//    string[] waffle_menu = { "Red velvet", "Charcoal", "Pandan" };
//    Console.WriteLine("\nAvailable waffle flavours: ");

//    //Display Waffle flavours
//    for (int e = 0; e < waffle_menu.Length; e++)
//    {
//        Console.WriteLine($"[{e + 1}] {waffle_menu[e]}");
//    }
//    Console.Write("\nSelect waffle flavour : ");
//    int w_opt = Convert.ToInt32(Console.ReadLine());
//    return waffle_menu[w_opt - 1];
//}

////Flavour Method
//List<Flavour> Flavours(int scoops)
//{
//    //Make list of flavours
//    List<string> flavour_menu = new List<string>();
//    for (int i = 1; i < data_flavours.Length; i++)
//    {
//        string[] flavour_name = data_flavours[i].Split(",");
//        flavour_menu.Add(flavour_name[0]);
//    }

//    List<Flavour> f_list = new List<Flavour>();

//    Console.Write("\n-Flavours-");

//    //Loop for number of scoops
//    for (int i = 0; i < scoops;)
//    {
//        bool premium = false;
//        int quantity = 1;

//        //Display Flavours
//        Console.WriteLine("\nAvailable Flavours: ");
//        for (int j = 0; j < flavour_menu.Count; j++)
//        {
//            if (j == 0) Console.WriteLine("Ordinary");
//            else if (j == 3) Console.WriteLine("Premium");
//            Console.WriteLine($"[{j + 1}] {flavour_menu[j]}");
//        }

//        Console.Write("\nEnter flavour option : ");
//        int f_opt = Convert.ToInt16(Console.ReadLine());
//        if (f_opt > 3 && f_opt < 7) premium = true; //Check if flavour selected is premium
//        if (scoops == 1 || (i == 1 && scoops != 3) || (i == 2 && scoops == 3))
//        {
//            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
//            f_list.Add(flavour);
//            break;
//        }
//        else
//        {
//            Console.Write("Enter scoops of flavour : ");
//            quantity = Convert.ToInt16(Console.ReadLine());
//            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
//            f_list.Add(flavour);
//            i += quantity;
//        }
//    }
//    return f_list;
//}

////Topping Method
//List<Topping> Toppings()
//{
//    List<string> toppings_menu = new List<string>();
//    for (int i = 1; i < data_toppings.Length; i++)
//    {
//        string[] topping_name = data_toppings[i].Split(",");
//        toppings_menu.Add(topping_name[0]);
//    }
//    List<Topping> t_list = new List<Topping>();

//    Console.WriteLine("\n-Toppings-");

//    while (true)
//    {
//        if (t_list.Count == 4) //Check for max number of toppings
//        {
//            Console.WriteLine("Topping limit reached.");
//            break;
//        }

//        Console.Write("Add toppings? y/n : ");
//        string continue_topping = Console.ReadLine();
//        if (continue_topping == "n") break; //Check if user wants to add toppings
//        else if (continue_topping == "y")
//        {
//            //Display Toppings
//            Console.WriteLine("\nAvailable Toppings:");
//            for (int i = 0; i < toppings_menu.Count; i++)
//            {
//                Console.WriteLine($"[{i + 1}] {toppings_menu[i]}");
//            }

//            Console.Write("\nEnter topping option : ");
//            int t_opt = Convert.ToInt16(Console.ReadLine());
//            Topping topping = new Topping(toppings_menu[t_opt - 1]);
//            t_list.Add(topping);
//        }
//    }
//    return t_list;
//}

////MAIN CODE
////Use while loop to keep calling menu method until exited
//do
//{
//    DisplayMenu();

//    try
//    {
//        //1. List all customers
//        if (option == 1)
//        {
//            customerDict.Clear(); //Ensures that any existing customer data is removed to prevent duplicate keys.
//            ListAllCustomers(customerDict);
//        }
//        //3. Register a new customer
//        else if (option == 3)
//        {
//            RegisterCustomer();
//        }
//        //4. Create a customer's order
//        else if (option == 4)
//        {
//            customerDict.Clear(); //Ensures that any existing customer data is removed to prevent duplicate keys.
//            CreateOrder(customerDict);
//        }
//    }
//    //Catch exceptions
//    catch (FormatException ex)
//    {
//        Console.WriteLine("Invalid input format! Please enter a number.\n");
//    }
//    catch (FileNotFoundException ex)
//    {
//        Console.WriteLine("CSV file(s) not found. Please check the file path and try again.\n");
//    }
//    catch (IOException ex)
//    {
//        Console.WriteLine("Error reading or writing to CSV file(s). Please ensure it's accessible.\n");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("An unexpected error occurred: " + ex.Message + "\n");
//    }

//} while (option != 0); //Loop until user enters 0 to exit