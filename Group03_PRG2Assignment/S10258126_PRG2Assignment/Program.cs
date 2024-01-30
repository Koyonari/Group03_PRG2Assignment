//===================================================================================================
// Features: 1, 3, 4, Advanced B
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//===================================================================================================
// Features: 2, 5, 6, Advanced A
// Student Number : S10255965
// Student Name : Jake Chan Man Lock
// Partner Name : An Yong Shyan
//===================================================================================================
//Advanced C: Implemented a simple ui for the user to interact with the program - Spectre.Console
//===================================================================================================

using Spectre.Console;
using S10258126_PRG2Assignment;

//Access csv files
string customerFile = "customers.csv";
string orderFile = "orders.csv";
string flavoursFile = "flavours.csv";
string optionsFile = "options.csv";
string toppingsFile = "toppings.csv";

//Create Dictionary and queues
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Queue<Order> gold_queue = new Queue<Order>();
Queue<Order> regular_queue = new Queue<Order>();
int orderID = 0;

//Welcome Message
AnsiConsole.Write(
    new FigletText("Welcome to")
        .LeftJustified());
AnsiConsole.Write(
    new FigletText("I.C.Treats")
        .Centered()
        .Color(Color.Aqua));
AnsiConsole.Write(
    new FigletText("------------")
        .LeftJustified()
        .Color(Color.Teal));

//Display Menu Method - General use
int DisplayMenu()
{
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("\n=== IceCream Menu ===")
        .PageSize(10)
        .AddChoices(new[] {
            "[[1]] List all customers",
            "[[2]] List all current orders",
            "[[3]] Register a new customer",
            "[[4]] Create customer order",
            "[[5]] Display order details",
            "[[6]] Modify order details",
            "[[7]] Order Checkout",
            "[[8]] Order price breakdown",
            "[[0]] Exit"
    }));
    return choice[2] - '0';
}

//Menu for list of Customers - Feature 4, 5, 6 use
int ListCustomers(Dictionary<int, Customer> customerDict)
{
    // Format customer information for display
    List<string> customerChoices = new List<string>();
    int index = 1;
    foreach (KeyValuePair<int, Customer> customer in customerDict)
    {
        //If else to format the spacing according to the index
        if (index < 10 && index > 0)
        {
            customerChoices.Add($"[[{"0" + index}]]   {customer.Value}");
            index++;
        }
        else
        {
            customerChoices.Add($"[[{index}]]   {customer.Value}");
            index++;
        }
    }

    // Prompt the user to select a customer using AnsiConsole
    string customerSelection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"----------------------------------List of Customers----------------------------------\n\n{"  Number",-9}{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}")
        .PageSize(10)
        .AddChoices(customerChoices));

    // Extract the customer index
    int customerIndex = int.Parse(customerSelection.Substring(2, 2)) - 1;
    int memberId = customerDict.ElementAt(customerIndex).Key;
    return memberId;
}

void ExtractCustomer(string filename) //Reads File, Creates objects
{
    string[] customer_file = File.ReadAllLines(filename); //Array

    for (int i = 0; i < customer_file.Length; i++)
    {
        string[] customer_details = customer_file[i].Split(","); //Nested Array
        if (i != 0)
        {
            Customer customer = new Customer(customer_details[0], Convert.ToInt32(customer_details[1]), Convert.ToDateTime(customer_details[2])); //Creates Customer Object
            customer.Rewards = new PointCard(Convert.ToInt32(customer_details[4]), Convert.ToInt32(customer_details[5])); ////Create PointCard object in Customer object
            customer.Rewards.Tier = customer_details[3];
            customerDict.Add(customer.MemberId, customer); //Adds Customer Object to Dictionary
        }
    }
}

void ExtractOrder(string filename, Dictionary<int, Customer> customerDict)
{
    string[] order_file = File.ReadAllLines(filename); //Array

    for (int i = 0; i < order_file.Length; i++)
    {
        List<IceCream> icecreamlist = new List<IceCream>();
        string[] order_details = order_file[i].Split(","); //Nested Array
        if (i != 0)
        {
            //Option
            string option = order_details[4];

            //Scoops
            int scoops = Convert.ToInt32(order_details[5]);

            //Flavour
            List<Flavour> flavour = new List<Flavour>();
            bool premium;
            bool flag = true;
            for (int j = 8; j < scoops + 8; j++)
            {
                premium = false;
                if (order_details[j] == "Durian" || order_details[j] == "Ube" || order_details[j] == "Sea Salt") premium = true;
                Flavour add_flavour = new Flavour(order_details[j], premium, 1);

                foreach (Flavour k in flavour)
                {
                    if (k.Type == add_flavour.Type)
                    {
                        k.Quantity += 1;
                        flag = false;
                    }
                }
                if (flag == true) flavour.Add(add_flavour);
                flag = true;
            }

            //Topping
            List<Topping> topping = new List<Topping>();
            for (int j = 11; j < 15; j++)
            {
                if (order_details[j] == "") break;
                topping.Add(new Topping(order_details[j]));
            }

            IceCream? icecream = null;

            switch (option) //Check for Option types
            {
                case "Cup":
                    icecream = new Cup("Cup", scoops, flavour, topping);
                    break;
                case "Cone":
                    icecream = new Cone("Cone", scoops, flavour, topping, Convert.ToBoolean(order_details[6]));
                    break;
                case "Waffle":
                    icecream = new Waffle("Waffle", scoops, flavour, topping, order_details[7]);
                    break;
            }
            icecreamlist.Add(icecream);
            Order order_history = new Order(Convert.ToInt32(order_details[0]), Convert.ToDateTime(order_details[2]));
            order_history.TimeFulfilled = Convert.ToDateTime(order_details[3]);
            order_history.IceCreamList = icecreamlist;
            if (customerDict[Convert.ToInt32(order_details[1])].OrderHistory.Count != 0 && order_history.Id == customerDict[Convert.ToInt32(order_details[1])].OrderHistory[customerDict[Convert.ToInt32(order_details[1])].OrderHistory.Count - 1].Id)
            {
                customerDict[Convert.ToInt32(order_details[1])].OrderHistory[customerDict[Convert.ToInt32(order_details[1])].OrderHistory.Count - 1].AddIceCream(order_history.IceCreamList[0]);
            }
            else customerDict[Convert.ToInt32(order_details[1])].OrderHistory.Add(order_history);
        }

    }
}

int ValidateInt(string text, int range, bool allowzero, int methodID)
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
                break;
            }
            switch (methodID)
            {
                case 0:
                    Console.WriteLine("Option not available.");
                    break;
                case 1:
                    Console.WriteLine("Waffle flavour not available.");
                    break;
                case 2:
                    Console.WriteLine("Invalid number of scoops.");
                    break;
                case 3:
                    Console.WriteLine("Option not in order.");
                    break;
                case 4:
                    Console.WriteLine("Invalid number of points.");
                    break;
            }
        }
        catch
        {
            Console.WriteLine("Invalid Input. Please input a number.");
        }
    }
    return option;
}

string ValidateBool(string text)
{
    string yes_no;
    while (true)
    {
        try
        {
            Console.Write(text);
            yes_no = Console.ReadLine();

            if (yes_no != "y" && yes_no != "n")
            {
                Console.WriteLine("Invalid Input. Please input y/n.");
                continue;
            }
            break;
        }
        catch
        {
            Console.WriteLine("Invalid Input. Please input y/n.");
        }
    }
    return yes_no;
}

int ValidateUserID(Dictionary<int, Customer> customerDict)
{
    int customer_index;
    while (true)
    {
        try
        {
            Console.Write("\nEnter customer account Id : ");
            customer_index = Convert.ToInt32(Console.ReadLine());
            if (Convert.ToString(customer_index).Length == 6)
            {
                bool inside = customerDict.ContainsKey(customer_index);
                if (inside == true) break;
                else Console.WriteLine("Invalid ID. ID not in database.");
            }
            else Console.WriteLine("Invalid ID.");
        }
        catch
        {
            Console.WriteLine("Invalid Input.");
        }
    }
    return customer_index;
}

//Feature 1 - List all customers
void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");
    foreach (KeyValuePair<int, Customer> customer in customerDict)
    {
        Console.WriteLine($"{customer.Value}");
    }
}

//Feature 2 - List all current orders in gold and regular queue
void ListAllCurrentOrders(Dictionary<int, Customer> customerDict, Queue<Order> gold_queue, Queue<Order> regular_queue)
{
    if (gold_queue.Count > 0) //checks for empty queue
    {
        Console.WriteLine("Gold member queue:");
        foreach (Order i in gold_queue)
        {
            foreach (KeyValuePair<int, Customer> j in customerDict)
            {
                if (j.Value.CurrentOrder != null && j.Value.CurrentOrder.Id == i.Id) Console.WriteLine("Customer ID : " + j.Key);
            }
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Gold member queue empty.");

    Console.WriteLine();

    if (regular_queue.Count > 0) //checks for empty queue
    {
        Console.WriteLine("Regular member queue:");
        foreach (Order i in regular_queue)
        {
            foreach (KeyValuePair<int, Customer> j in customerDict)
            {
                if (j.Value.CurrentOrder != null && j.Value.CurrentOrder.Id == i.Id) Console.WriteLine("Customer ID : " + j.Key);
            }
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Regular member queue empty.");
}

//Feature 3 - Register new customers and store data in csv file
void RegisterNewCustomer(Dictionary<int, Customer> customerDict, string filename)
{
    //Get account name
    Console.Write("Enter your name: ");
    string name = Console.ReadLine().Trim();

    //Get account ID
    int id;
    while (true)
    {
        try
        {
            Console.Write("\nEnter your ID: ");
            string input_id = Console.ReadLine();
            id = Convert.ToInt32(input_id);
            if (input_id.Length == 6) //Check for the length of the input, used string input_id so that if it starts with 0 it is still 6 digits
            {
                bool inside = customerDict.ContainsKey(id);
                if (inside == false) break;
                else Console.WriteLine("Invalid ID. ID in database.");
            }
            else Console.WriteLine("Invalid ID. Please input a 6-digit number.");
        }
        catch
        {
            Console.WriteLine("Invalid Input. Please input a 6-digit number.");
        }
    }

    //Get user DoB
    DateTime dob;
    while (true)
    {
        try
        {
            Console.Write("Enter your date of birth DD/MM/YYYY : ");
            dob = DateTime.Parse(Console.ReadLine(), null);
            break;
        }
        catch
        {
            Console.WriteLine("Invalid Input.");
        }
    }

    Customer customer = new Customer(name, id, dob); //Create Customer object
    customer.Rewards = new PointCard(); //Create PointCard object in Customer object
    customerDict.Add(customer.MemberId, customer); //Add Customer to customer dictionary

    string updatefile = $"{customer.Name},{Convert.ToInt32(customer.MemberId)},{customer.Dob.ToString("dd/MM/yyyy")},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

    using (StreamWriter sw = File.AppendText(filename)) //Update csv file with new customer data
    {
        sw.WriteLine(updatefile);
    }
}

//Feature 4 - Create customer order
void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{   
    orderID++;
    int customer_index = ListCustomers(customerDict);

    if (customerDict[customer_index].CurrentOrder != null && customerDict[customer_index].CurrentOrder.TimeFulfilled == null)
    {
        Console.WriteLine("Previous Order not fulfilled.");
    }
    else
    {
        Order new_order = new Order(orderID, DateTime.Now); //Create Order object
        Order current_order = customerDict[customer_index].MakeOrder(); //Initialise CurrentOrder in Customer Class

        while (true)
        {
            IceCream iceCream = CreateIceCream(); //Create IceCream object via IceCream Method
            new_order.AddIceCream(iceCream); //Add IceCream object to IceCreamList attribute Order object

            current_order = new_order; //Add Order object to CurrentOrder attribute in Customer object

            Console.WriteLine("\nIce Cream added to order.");

            string continue_order = AddIC();

            if (continue_order == "n") break; //Check if customer wants to continue adding more IceCream objects
        }

        customerDict[customer_index].CurrentOrder = current_order; //Add Order to CurrentORder attribute in Customer object

        //Queue orders
        if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(new_order);
        else regular_queue.Enqueue(new_order);

        //Indicate that order has been made successfully
        Console.WriteLine("Order successfully made.\n");
    }
}

//Add Ice Cream Menu Method
string AddIC()
{
    // Prompt the user to select a customer using AnsiConsole
    string addic = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Add Another Ice Cream--")
        .PageSize(3)
        .AddChoices(new[] {
            "Yes",
            "No"
        }));

    // Extract the customer index
    if (addic == "Yes") return "y";
    else return "n";
}

//Cone Method
bool Cone()
{
    string dipped = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Add Chocolate-Dipped Cone--")
        .PageSize(3)
        .AddChoices(new[] {
            "Yes",
            "No"
        }));

    if (dipped == "Yes") return true;
    else return false;
}

//Waffle Method
string Waffle()
{
    string w_opt = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Available Waffle Flavours--")
        .PageSize(4)
        .AddChoices(new[] {
        "[[1]] Red Velvet",
        "[[2]] Charcoal",
        "[[3]] Pandan",
        "[[4]] Original"
        }));

    return w_opt.Substring(5);
}

//Flavour Menu Method
int Flavour_Menu()
{
    string flavour_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Available Ice Cream Flavours--")  // Removed "Flavours" from the title
        .PageSize(6)
        .AddChoices(new[] {
            "[[1]] Vanilla",
            "[[2]] Chocolate",
            "[[3]] Strawberry",
            "[[4]] Durian (Premium)",
            "[[5]] Ube (Premium)",
            "[[6]] Sea Salt (Premium)"
        }));

    // Extract the flavour index (no change here)
    int flavourIndex = int.Parse(flavour_menu.Substring(2, 1)) + 1;
    return flavourIndex;
}

//Flavour Method
List<Flavour> Flavours(int scoops)
{
    string[] flavour_menu = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea Salt" };
    List<Flavour> f_list = new List<Flavour>();

    //Loop for number of scoops
    for (int i = 0; i < scoops;)
    {
        bool flag = true;
        bool premium = false;
        int quantity = 1;

        //Display Flavours
        int f_opt = Flavour_Menu();

        if (f_opt > 3 && f_opt < 7) premium = true; //Check if flavour selected is premium
        if (scoops == 1 || (i == 1 && scoops != 3) || (i == 2 && scoops == 3))
        {
            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            foreach (Flavour k in f_list)
            {
                if (k.Type == flavour.Type)
                {
                    k.Quantity += 1;
                    flag = false;
                }
            }
            if (flag == true) f_list.Add(flavour);
            flag = true;
            break;
        }
        else
        {
            if (i == 0)
            {
                if (scoops == 3) quantity = Scoop_Menu();
                else if (scoops == 2) quantity = Scoop_Menu2();
                else if (scoops == 1) quantity = Scoop_Menu1();
            }
            //Check for the valid number of scoops
            else
            {
                if (scoops - quantity == 2 && i > 0) quantity = Scoop_Menu2();
                else if (scoops - quantity == 1 && i > 0) quantity = Scoop_Menu1();
            }

            Flavour flavour = new Flavour(flavour_menu[f_opt - 1], premium, quantity);
            foreach (Flavour k in f_list)
            {
                if (k.Type == flavour.Type)
                {
                    k.Quantity += 1;
                    flag = false;
                }
            }
            if (flag == true) f_list.Add(flavour);
            flag = true;
            i += quantity;
        }
    }
    return f_list;
}

//Topping Menu Method
int Topping_Menu()
{
    string topping_menu = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
    .Title("--Available Toppings--")
    .PageSize(4)
    .AddChoices(new[]
    {
        "[[1]] Sprinkles",
        "[[2]] Mochi",
        "[[3]] Sago",
        "[[4]] Oreos",

    }));

    // Extract the topping index
    int toppingIndex = int.Parse(topping_menu.Substring(2, 1));
    return toppingIndex;
}

bool Topping_Check()
{
    // Prompt the user to select a customer using AnsiConsole
    string addt = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Add Toppings--")
        .PageSize(3)
        .AddChoices(new[] {
            "Yes",
            "No"
        }));

    // Extract the customer index
    if (addt == "Yes") return true;
    else return false;
}

//Topping Method
List<Topping> Toppings()
{
    string[] topping_menu = { "Sprinkles", "Mochi", "Sago", "Oreos" };
    List<Topping> t_list = new List<Topping>();

    while (true)
    {
        if (t_list.Count == 4) //Check for max number of toppings
        {
            Console.WriteLine("Topping limit reached.");
            break;
        }
        bool continue_topping = Topping_Check();

        if (continue_topping == false) break; //Check if user wants to add toppings
        else if (continue_topping == true)
        {
            //Display Toppings
            int t_opt = Topping_Menu();

            Topping topping = new Topping(topping_menu[t_opt - 1]);
            t_list.Add(topping);
        }
    }
    return t_list;
}

//IceCream Menu Method
int IceCream_Menu()
{
    string iceCream_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Available Types--")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] Cup",
            "[[2]] Cone",
            "[[3]] Waffle"
        }));

    // Extract the flavour index
    int iceCreamIndex = int.Parse(iceCream_menu.Substring(2, 1));

    return iceCreamIndex;
}

//Scoops Menu Method
int Scoop_Menu_Total()
{
    string scoop_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Total Number of Scoops--")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] One",
            "[[2]] Two",
            "[[3]] Three"
        }));

    // Extract the scoop index
    int scoopIndex = int.Parse(scoop_menu.Substring(2, 1));

    return scoopIndex;
}

    //Scoops Menu Method
    int Scoop_Menu()
{
    string scoop_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Number of Scoops--")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] One",
            "[[2]] Two",
            "[[3]] Three"
        }));

    // Extract the scoop index
    int scoopIndex = int.Parse(scoop_menu.Substring(2, 1));

    return scoopIndex;
}

int Scoop_Menu2()
{
    string scoop_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Number of Scoops--")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] One",
            "[[2]] Two"
        }));

    // Extract the scoop index
    int scoopIndex = int.Parse(scoop_menu.Substring(2, 1));

    return scoopIndex;
}


int Scoop_Menu1()
{
    string scoop_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Number of Scoops--")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] One"
        }));

    // Extract the scoop index
    int scoopIndex = int.Parse(scoop_menu.Substring(2, 1));

    return scoopIndex;
}


//IceCream Method
IceCream CreateIceCream()
{
    IceCream? iceCream = null; //Initalize IceCream object
    string[] option_menu = { "Cup", "Cone", "Waffle" };
    bool dipped = false;
    string waffle = "";

    //Option
    int option = IceCream_Menu();

    if (option == 2) dipped = Cone();
    else if (option == 3) waffle = Waffle();

    //Scoops
    int scoops = Scoop_Menu_Total();

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

//Feature 5
void DisplayOrderDetails(Dictionary<int, Customer> customerDict)
{
    int customer_index = ListCustomers(customerDict);

    Console.WriteLine();

    DisplayOrderHistory(customerDict, customer_index); //Display order history Method
    DisplayCurrentOrder(customerDict, customer_index); //Display current order Method

    Console.WriteLine("______________________________________");
}

//Display order history Method
void DisplayOrderHistory(Dictionary<int, Customer> customerDict, int index)
{
    if (customerDict[index].OrderHistory.Count != 0)
    {
        Console.WriteLine("Order History:");
        Console.WriteLine("______________________________________");

        //Loops through order history & Prints each order
        for (int i = 0; i < customerDict[index].OrderHistory.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {customerDict[index].OrderHistory[i]}");
            Console.WriteLine($"{"___________________________________",38}\n");

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
    else
    {
        Console.WriteLine("No Order History.");
    }
}

//Display current order Method
void DisplayCurrentOrder(Dictionary<int, Customer> customerDict, int index)
{
    if (customerDict[index].CurrentOrder != null)
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
    else
    {
        Console.WriteLine("No Current Order.");
    }
}

//Feature 6
void Option1(Dictionary<int, Customer> customerDict, int index)
{
    int m_opt = ValidateInt("\nSelect Ice Cream to modify : ", customerDict[index].CurrentOrder.IceCreamList.Count, false, 3);

    Console.WriteLine();

    int i_opt = EditIC();

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

            int option = ValidateInt("\nEnter option : ", 3, false, 0);

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
            Console.Write("\n-Scoops-");

            int scoops = ValidateInt("\nEnter number of scoops : ", 3, false, 2);

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

void Option2(Dictionary<int, Customer> customerDict, int index)
{
    IceCream new_IceCream = CreateIceCream(); //Feature 4 - IceCream Method

    customerDict[index].CurrentOrder.AddIceCream(new_IceCream);
    Console.WriteLine("\nNew Ice Cream added successfully.");
}

void Option3(Dictionary<int, Customer> customerDict, int index)
{
    if (customerDict[index].CurrentOrder.IceCreamList.Count > 1)
    {
        int d_opt = ValidateInt("\nSelect Ice Cream to delete : ", customerDict[index].CurrentOrder.IceCreamList.Count, false, 3);

        customerDict[index].CurrentOrder.DeleteIceCream(d_opt - 1);
    }
    else Console.WriteLine("You cannot have 0 ice creams in an order.");
}

//Modify Ice Cream Menu Method
int EditIC()
{
    string edit_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("--Edit Ice Cream--")
        .PageSize(4)
        .AddChoices(new[]
        {
            "[[1]] Option",
            "[[2]] Scoops",
            "[[3]] Flavours",
            "[[4]] Toppings"
        }));

    // Extract the scoop index
    int scoopIndex = int.Parse(edit_menu.Substring(2, 1));

    return scoopIndex;
}

//Modify Menu Method
int ModifyMenu()
{
    string flavour_menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("=== Edit Order Menu ===")
            .PageSize(3)
            .AddChoices(new[] {
                "[[1]] Modify Ice Cream",
                "[[2]] Add Ice Cream",
                "[[3]] Delete Ice Cream"
            }));

    // Extract the flavour index
    int flavourIndex = int.Parse(flavour_menu.Substring(2, 1));

    return flavourIndex;
}

void ModifyOrderDetails(Dictionary<int, Customer> customerDict)
{
    int customer_index = ListCustomers(customerDict);

    DisplayCurrentOrder(customerDict, customer_index); //Feature 5 - Display current order Method

    if (customerDict[customer_index].CurrentOrder != null)
    {
        int menu_opt = ModifyMenu();

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
}

//Advanced Features

//Feature 7
//Process Order Checkout Menu
string ProcessOrderMenu()
{
    string process_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("=== Redeem Points to Offset Order ===")
        .PageSize(3)
        .AddChoices(new[]
        {
            "[[1]] Yes",
            "[[2]] No"
        }));
    
    if (process_menu == "[[1]] Yes") return "y"; else return "n";
}

void ProcessOrderCheckout(Dictionary<int, Customer> customerDict, Queue<Order> gold_queue, Queue<Order> regular_queue)
{
    Order? order;
    double total = 0;
    bool isbday;
    Customer customer = new Customer();
    List<double> cost = new List<double>();
    while (true)
    {
        if (gold_queue.Count != 0)
        {
            order = gold_queue.Dequeue();
            foreach (IceCream i in order.IceCreamList)
            {
                Console.WriteLine($"{"-"}{i} ${i.CalculatePrice().ToString("0.00")}");
                cost.Add(i.CalculatePrice());
            }
        }
        else
        {
            Console.WriteLine("Gold queue is empty.");
            if (regular_queue.Count != 0)
            {
                order = regular_queue.Dequeue();
                foreach (IceCream i in order.IceCreamList)
                {
                    Console.WriteLine($"{"-"}{i} ${i.CalculatePrice().ToString("0.00")}");
                    cost.Add(i.CalculatePrice());
                }
            }
            else
            {
                Console.WriteLine("Regular queue is empty.");
                break;
            }
        }

        foreach (KeyValuePair<int, Customer> kvp in customerDict)
        {
            if (kvp.Value.CurrentOrder != null && kvp.Value.CurrentOrder.Id == order.Id)
            {
                customer = kvp.Value;
                Console.WriteLine($"\n{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");
                Console.WriteLine(customer);
                Console.WriteLine();
                if (customer.IsBirthday() == true)
                {
                    Console.WriteLine("It's your birthday!");
                    for (int i = 0; i < cost.Count; i++)
                    {
                        if (cost[i] == cost.Max())
                        {
                            cost[i] = 0;
                            break;
                        }
                    }
                }
                break;
            }
        }
        if (customer.Rewards.PunchCard == 10)
        {
            if (cost[0] != 0) cost[0] = 0;
            else if (cost.Count > 1) cost[1] = 0;
        }
        customer.Rewards.Punch();

        foreach (double i in cost)
            total += i;
        Console.WriteLine($"Final bill : ${total:0.00}");

        if (customer.Rewards.Tier != "Ordinary" && customer.Rewards.Points != 0)
        {
            string do_offset = ProcessOrderMenu();
            if (do_offset == "y")
            {
                int redeem_points = ValidateInt("Enter number of points to redeem  : ", customer.Rewards.Points, false, 4);
                while (true)
                {
                    if (0.02 * redeem_points > total)
                    {
                        Console.WriteLine("Too many points used.");
                        redeem_points = ValidateInt("Enter number of points to redeem  : ", customer.Rewards.Points, false, 4);
                    }
                    else break;
                }

                double cost_offset = 0.02 * redeem_points;
                Console.WriteLine($"Cost offset : ${cost_offset:0.00}");
                total -= cost_offset;
                customer.Rewards.RedeemPoints(redeem_points);
            }
        }
        int points = Convert.ToInt32(Math.Floor(total * 0.72));
        Console.WriteLine($"Points earned : ${points}");
        customer.Rewards.AddPoints(points);

        Console.WriteLine("Press any key to make payment . . .");
        Console.ReadLine();

        //Play a payment animation
        AnsiConsole.Status()
            .Start("Processing payment...", ctx =>
            {
                //Delay for 1.5 seconds
                Thread.Sleep(1500);

                //Update the spinner status and color
                ctx.Status("Payment successful!");
                ctx.SpinnerStyle(Style.Parse("green"));

                //Prompt user to press enter to continue
                AnsiConsole.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            });

        order.TimeFulfilled = DateTime.Now;
        customer.CurrentOrder = null;
        customer.OrderHistory.Add(order); //Add Order to OrderHistory attribute in Customer object

        break;
    }
}

//Feature 8
//Year Menu Method
string YearMenu()
{
    string year_menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("=== Display Monthly and Yearly Charges ===")
        .PageSize(5)
        .AddChoices(new[] {
            "[[1]]  2024",
            "[[2]]  2023",
            "[[3]]  2022",
            "[[4]]  2021",
            "[[5]]  2020",
            "[[6]]  2019",
            "[[7]]  2018",
            "[[8]]  2017",
            "[[9]]  2016",
            "[[10]] 2015",
            "[[11]] 2014",
            "[[12]] 2013",
            "[[13]] 2012",
            "[[14]] 2011",
            "[[15]] 2010"
        }));

    // Extract the year directly from the selected option
    string selectedYear = year_menu.Substring(6); // Get the year part (starting from index 5)
    return selectedYear;
}

void CalculateYear(string filename)
{
    //Call Menu
    string year = YearMenu();

    //Make a new list for fulfilled orders and montly price dictionary
    List<string> fulfilled_list =  new List<string>();
    Dictionary<string, double> monthly = new Dictionary<string, double>(12)
    {
        //Add key months to dictionary
        { "January", 0 },
        { "February", 0 },
        { "March", 0 },
        { "April", 0 },
        { "May", 0 },
        { "June", 0 },
        { "July", 0 },
        { "August", 0 },
        { "September", 0 },
        { "October", 0 },
        { "November", 0 },
        { "December", 0 }
    };

    //Read file
    string[] lines = File.ReadAllLines(filename);
    string[] flavours_lines = File.ReadAllLines(flavoursFile);
    string[] options_lines = File.ReadAllLines(optionsFile);
    string[] toppings_lines = File.ReadAllLines(toppingsFile);
    
    //Create dictionaries for flavours
    Dictionary<string, string> flavourDict = new Dictionary<string, string>();
    foreach (string f in flavours_lines)
    {
        string[] i = f.Split(',');
        flavourDict.Add(i[0], i[1]);
    }

    //Create dictionaries for toppings
    Dictionary<string, string> toppingDict = new Dictionary<string, string>();
    foreach (string t in toppings_lines)
    {
        string[] i = t.Split(',');
        toppingDict.Add(i[0], i[1]);
    }

    //Loop through orders to find fulfilled orders in the year
    for (int i = 1; i < lines.Length; i++)
    {
        string[] data = lines[i].Split(',');
        int data_year = DateTime.Parse(data[3]).Year;

        if (data_year == Convert.ToInt32(year))
        {
            fulfilled_list.Add(lines[i]);  // Add the entire line to the list
        }
    }

    //Loop through fulfilled orders to add to monthly price
    List<Order> order_list = new List<Order>();
    foreach (string q in fulfilled_list)
    {
        double price = 0;

        string[] i = q.Split(',');
        string[] iceCream = new string[] {i[4], i[5], i[6], i[7], i[8], i[9], i[10], i[11], i[12], i[13], i[14] };

        //Adds price of each flavour
        if (flavourDict.ContainsKey(iceCream[4]))
        {
            double flavourPrice = double.Parse(flavourDict[iceCream[4]]); // Access the value for the specific key
            price += flavourPrice;
        }
        if (flavourDict.ContainsKey(iceCream[5]))
        {
            double flavourPrice = double.Parse(flavourDict[iceCream[5]]);
            price += flavourPrice;
        }
        if (flavourDict.ContainsKey(iceCream[6]))
        {
            double flavourPrice = double.Parse(flavourDict[iceCream[6]]);
            price += flavourPrice;
        }

        //Adds price of each topping
        if (toppingDict.ContainsKey(iceCream[7]))
        {
            double toppingPrice = double.Parse(toppingDict[iceCream[7]]);
            price += toppingPrice;
        }
        if (toppingDict.ContainsKey(iceCream[8]))
        {
            double toppingPrice = double.Parse(toppingDict[iceCream[8]]);
            price += toppingPrice;
        }
        if (toppingDict.ContainsKey(iceCream[9]))
        {
            double toppingPrice = double.Parse(toppingDict[iceCream[9]]);
            price += toppingPrice;
        }
        if (toppingDict.ContainsKey(iceCream[10]))
        {
            double toppingPrice = double.Parse(toppingDict[iceCream[10]]);
            price += toppingPrice;
        }

        //Adds price of each option
        foreach (string o in options_lines)
        {
            string options = $"{iceCream[0]},{iceCream[1]},{iceCream[2]},{iceCream[3]}";
            if (o.Contains(options))
            {
                string[] oindex = o.Split(',');
                price += double.Parse(oindex[4]);
                break;
            }
        }

        //Retrieve order objects
        Order order = new Order(Convert.ToInt32(i[0]), Convert.ToDateTime(i[2]));
        order_list.Add(order);

        //Add to monthly dictionary
        string month = Convert.ToDateTime(i[3]).ToString("MMMM");
        if (monthly.ContainsKey(month))
        {
            monthly[month] += price;
        }
    }

    //Create table for display
    Table table = new Table();

    //Add table title
    table.Title($"Breakdown for {year}");

    //Add columns
    table.AddColumn("Month/Total");
    table.AddColumn("Price");

    //Add rows
    foreach (KeyValuePair<string, double> m in monthly)
    {
        string formattedValue = m.Value <= 0 ? "$0" : $"${m.Value:F2}";
        table.AddRow(m.Key, formattedValue);
    }

    //Add total price row
    table.AddRow($"\nTotal Price", $"\n${monthly.Select(m => m.Value).Sum():F2}");
    AnsiConsole.Write(table);
    Console.WriteLine();
}

//Main Program
ExtractCustomer(customerFile);
ExtractOrder(orderFile, customerDict);

while (true)
{
    int menu_opt = DisplayMenu();

    if (menu_opt == 0)
    {
        Console.WriteLine("Exited...");
        break;
    }

    Console.WriteLine($"\n=== {menu_opt} ===\n");
    switch (menu_opt)
    {
        case 1:
            ListAllCustomers(customerDict);
            Console.WriteLine();
            break;
        case 2:
            ListAllCurrentOrders(customerDict, gold_queue, regular_queue);
            Console.WriteLine();
            break;
        case 3:
            RegisterNewCustomer(customerDict, customerFile);
            Console.WriteLine();
            break;
        case 4:
            CreateCustomerOrder(customerDict);
            break;
        case 5:
            DisplayOrderDetails(customerDict);
            break;
        case 6:
            ModifyOrderDetails(customerDict);
            break;
        case 7:
            ProcessOrderCheckout(customerDict, gold_queue, regular_queue);
            break;
        case 8:
            CalculateYear(orderFile);
            break;
    }
}