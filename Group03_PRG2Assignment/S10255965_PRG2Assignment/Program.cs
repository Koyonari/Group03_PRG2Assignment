using ICTreatsSystem;

string customerFile = "customers.csv";
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //Create a Dictionary to store Customer Objects
Queue<Order> gold_queue = new Queue<Order>();
Queue<Order> regular_queue = new Queue<Order>();

void ExtractCustomer(string filename) //Reads File, Creates objects
{
    string[] customer_file = File.ReadAllLines(filename); //Array

    for (int i = 0; i < customer_file.Length; i++)
    {
        string[] customer_details = customer_file[i].Split(","); //Nested Array
        if (i != 0)
        {
            Customer customer = new Customer(customer_details[0], Convert.ToInt32(customer_details[1]), Convert.ToDateTime(customer_details[2])); //Creates Customer Object

            customer.Rewards = new PointCard();
            customerDict.Add(customer.MemberId, customer); //Adds Customer Object to Dictionary
        }
    }
}

//Feature 1 - List all customers
void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}\n");
    foreach (KeyValuePair<int, Customer> kvp in customerDict)
    {
        Console.WriteLine(kvp.Value);
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

//Feature 3 - Register new customers and store data in csv file
void RegisterNewCustomer(Dictionary<int, Customer> customerDict, string filename)
{
    Console.Write("Enter your name : ");
    string name = Console.ReadLine();

    Console.Write("Enter your ID : ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter your date of birth DD/MM/YYYY : ");
    DateTime dob = DateTime.ParseExact(Console.ReadLine(),"dd/MM/yyyy" , null);

    Customer customer = new Customer(name, id, dob); //Create Customer object
    customer.Rewards = new PointCard(); //Create PointCard object in Customer object
    customerDict.Add(customer.MemberId, customer); //Add Customer to customer dictionary

    string updatefile = $"{customer.Name},{Convert.ToInt32(customer.MemberId)},{customer.Dob.ToString("dd/MM/yyyy")}";

    using (StreamWriter sw = File.AppendText(filename)) //Update csv file with new customer data
    {
        sw.WriteLine(updatefile);
    }
}

//Feature 4 - Create customer order
void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{
    ListAllCustomers(customerDict); //Feature 1

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

        if (continue_order == "n") break; //Check if customer wants to continue adding more IceCream objects
    }

    customerDict[customer_index].OrderHistory.Add(current_order); //Add Order to OrderHistory attribute in Customer object

    //Queue orders
    if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(current_order);
    else regular_queue.Enqueue(current_order);

    Console.WriteLine("Order successfully made.\n");
}

//Cone Method
bool Cone()
{
    Console.Write("\nAdd chocolate-dipped cone? y/n : ");
    string dipped = Console.ReadLine();
    if (dipped == "y") return true;
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

            Console.Write("\nEnter topping option : ");
            int t_opt = Convert.ToInt16(Console.ReadLine());
            Topping topping = new Topping(topping_menu[t_opt - 1]);
            t_list.Add(topping);
        }
    }
    return t_list;
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

//Feature 5
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

//Display order history Method
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
            Console.WriteLine($"{"(" + ( j + 1 ) + ")",8}{customerDict[index].OrderHistory[i].IceCreamList[j]}");

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

//Display current order Method
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

//Feature 6
void Option1(Dictionary<int, Customer> customerDict, int index)
{
    string iceCream_menu =
        "=== Edit Ice Cream ===\n" +
        "[1] Option\n" +
        "[2] Scoops\n" +
        "[3] Flavours\n" +
        "[4] Toppings\n";

    Console.Write("Select Ice Cream to modify : ");
    int m_opt = Convert.ToInt32(Console.ReadLine());

    Console.Write("\n" + iceCream_menu + "\nSelect component to modify : ");
    int i_opt = Convert.ToInt32(Console.ReadLine());

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

            Console.Write("\nEnter option : ");
            int option = Convert.ToInt32(Console.ReadLine());

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
            Console.Write("\n-Scoops-\nEnter number of scoops : ");
            int scoops = Convert.ToInt16(Console.ReadLine());

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
        Console.Write("Select Ice Cream to delete : ");
        int d_opt = Convert.ToInt32(Console.ReadLine());

        customerDict[index].CurrentOrder.DeleteIceCream(d_opt - 1);
    }
    else Console.WriteLine("You cannot have 0 ice creams in an order.");
}

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

    Console.Write("\n" + edit_menu + "\nSelect option : ");
    int menu_opt = Convert.ToInt32(Console.ReadLine());

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
ExtractCustomer(customerFile);

while (true)
{
    string sys_menu =
        "=== IceCream Menu ===\n" +
        "[1] List all customers\n" +
        "[2] List all current orders\n" +
        "[3] Register a new customer\n" +
        "[4] Create customer order\n" +
        "[5] Display order details\n" +
        "[6] Modify order details\n" +
        "[0] Exit system\n";

    Console.Write(sys_menu + "\nSelect option : ");
    int menu_opt = Convert.ToInt32(Console.ReadLine());
    if (menu_opt == 0)
    {
        Console.WriteLine("Exited.");
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
    }
}
Console.ReadLine();