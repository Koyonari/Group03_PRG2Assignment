﻿using ICTreatsSystem;

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

//Feature 1
void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}\n");
    foreach (KeyValuePair<int, Customer> kvp in customerDict)
    {
        Console.WriteLine(kvp.Value);
    }
}

//Feature 2
void ListAllCurrentOrders(Dictionary<int, Customer> customerDict, Queue<Order> gold_queue, Queue<Order> regular_queue)
{
    if (gold_queue.Count > 0)
    {
        Console.WriteLine("Gold member queue:");
        foreach (Order i in gold_queue)
        {
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Gold member queue empty.");

    if (regular_queue.Count > 0)
    { 
        Console.WriteLine("Regular member queue:");
        foreach (Order i in regular_queue)
        {
            Console.WriteLine(i);
        }
    }
    else Console.WriteLine("Regular member queue empty.");
}

//Feature 3
void RegisterNewCustomer(Dictionary<int, Customer> customerDict, string filename)
{
    Console.Write("Enter your name : ");
    string name = Console.ReadLine();
    Console.Write("Enter your ID : ");
    int id = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter your date of birth DD/MM/YYYY : ");
    //DateTime dob = Convert.ToDateTime(Console.ReadLine());
    DateTime dob = DateTime.ParseExact(Console.ReadLine(),"dd/MM/yyyy" , null);

    Customer customer = new Customer(name, id, dob);

    customer.Rewards = new PointCard();
    customerDict.Add(customer.MemberId, customer);

    string updatefile = $"{customer.Name},{Convert.ToInt32(customer.MemberId)},{customer.Dob.ToString("dd/MM/yyyy")}";

    using (StreamWriter sw = File.AppendText(filename))
    {
        sw.WriteLine(updatefile);
    }
}

//Feature 4
void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{
    //Retrieve the selected customer
    ListAllCustomers(customerDict);

    Console.Write("\nEnter customer account Id : ");
    int opt = Convert.ToInt32(Console.ReadLine());

    //Create order object
    Order order = new Order(opt, DateTime.Now);

    while (true)
    {
        //Create ice cream
        IceCream iceCream = CreateIceCream();
        order.AddIceCream(iceCream);

        //Add order tp current order
        customerDict[opt].CurrentOrder = order;

        Console.WriteLine("\nIce Cream added to order.");

        Console.Write("\nAdd another ice cream? y/n : ");
        string cont_o = Console.ReadLine();

        if (cont_o == "n")
        {
            break;
        }
    }

    //Add order to order history
    customerDict[opt].OrderHistory.Add(order);

    //Queue orders
    if (customerDict[opt].Rewards.Tier == "Gold") gold_queue.Enqueue(order);
    else regular_queue.Enqueue(order);

    Console.WriteLine("Order successfully made.\n");
}

//Cone Method
bool Cone()
{
    Console.Write("\nAdd chocolate-dipped cone? y/n : ");
    string dip = Console.ReadLine();
    if (dip == "y") return true;
    else return false;
}

//Waffle Method
string Waffle()
{
    string[] w_menu = { "Red velvet", "Charcoal", "Pandan" };
    Console.WriteLine("\nAvailable waffle flavours: ");
    for (int e = 0; e < w_menu.Length; e++)
    {
        Console.WriteLine($"[{e + 1}] {w_menu[e]}");
    }
    Console.Write("\nSelect waffle flavour : ");
    int waffF = Convert.ToInt32(Console.ReadLine());
    return w_menu[waffF - 1];
}

//Flavours Method
List<Flavour> Flavours(int scoops)
{
    string[] f_menu = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea salt" };
    List<Flavour> f_list = new List<Flavour>();

    Console.Write("\n-Flavours-");

    for (int i = 0; i < scoops;)
    {
        bool premium = false;
        int quantity = 1;

        //print flavour options menu
        Console.WriteLine("\nAvailable Flavours: ");
        for (int j = 0; j < f_menu.Length; j++)
        {
            if (j == 0) Console.WriteLine("Ordinary");
            else if (j == 3) Console.WriteLine("Premium");
            Console.WriteLine($"[{j + 1}] {f_menu[j]}");
        }

        Console.Write("\nEnter flavour option : ");
        int f_opt = Convert.ToInt16(Console.ReadLine());
        if (f_opt > 3 && f_opt < 7) premium = true;

        if (scoops == 1 || (i == 1 && scoops != 3) || (i == 2 && scoops == 3))
        {
            Flavour flavour = new Flavour(f_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            break;
        }
        //Add more
        else
        {
            Console.Write("Enter scoops of flavour : ");
            quantity = Convert.ToInt16(Console.ReadLine());
            Flavour flavour = new Flavour(f_menu[f_opt - 1], premium, quantity);
            f_list.Add(flavour);
            i += quantity;
        }
    }
    return f_list;
}

IceCream CreateIceCream()
{
    //Create ice cream
    IceCream? iceCream = null;

    //Option
    string[] o_menu = { "Cup", "Cone", "Waffle" };
    string[] w_menu = { "Red velvet", "Charcoal", "Pandan" };
    bool dipped = false;
    string waffleFlavour = "";

    Console.WriteLine("\n-Type-");

    //print type options menu
    Console.WriteLine("Avilable types:");
    for (int i = 0; i < o_menu.Length; i++)
    {
        Console.WriteLine($"[{i + 1}] {o_menu[i]}");
    }

    Console.Write("\nEnter option : ");
    int option = Convert.ToInt32(Console.ReadLine());

    if (option == 2) dipped = Cone();
    else if (option == 3) waffleFlavour = Waffle();

    //Scoops
    Console.Write("\n-Scoops-\nEnter number of scoops : ");
    int scoops = Convert.ToInt16(Console.ReadLine());

    List<Flavour> f_list = Flavours(scoops);
    List<Topping> t_list = Toppings();

    switch (o_menu[option - 1])
    {
        case "Cup":
            iceCream = new Cup("Cup", scoops, f_list, t_list);
            break;
        case "Cone":
            iceCream = new Cone("Cone", scoops, f_list, t_list, dipped);
            break;
        case "Waffle":
            iceCream = new Waffle("Waffle", scoops, f_list, t_list, waffleFlavour);
            break;
    }

    return iceCream;
}

//Topping Method
List<Topping> Toppings()
{
    string[] t_menu = { "Sprinkles", "Mochi", "Sago", "Oreos" };
    List<Topping> t_list = new List<Topping>();

    Console.WriteLine("\n-Toppings-");

    while (true)
    {
        if (t_list.Count == 4)
        {
            Console.WriteLine("Topping limit reached.");
            break;
        }

        Console.Write("Add toppings? y/n : ");
        string t_cont = Console.ReadLine();
        if (t_cont == "n") break;
        else if (t_cont == "y")
        {
            //print topping options menu
            Console.WriteLine("\nAvailable Toppings:");
            for (int i = 0; i < t_menu.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {t_menu[i]}");
            }

            Console.Write("\nEnter topping option : ");
            int t_opt = Convert.ToInt16(Console.ReadLine());
            Topping topping = new Topping(t_menu[t_opt - 1]);
            t_list.Add(topping);
        }
    }
    return t_list;
}

//Feature 5
void DisplayOrderDetails(Dictionary<int, Customer> customerDict)
{
    //List customers
    ListAllCustomers(customerDict);

    //Select customer account
    Console.Write("\nEnter customer account Id : ");
    int opt = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine();

    DisplayOrderHistory(customerDict, opt);
    DisplayCurrentOrder(customerDict, opt);
    
    Console.WriteLine("______________________________________");
}

//Display order history
void DisplayOrderHistory(Dictionary<int, Customer> customerDict, int index)
{
    //Loops through order history & Prints each order
    Console.WriteLine("Order History:");
    Console.WriteLine("______________________________________");
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

//Display current order
void DisplayCurrentOrder(Dictionary<int, Customer> customerDict, int index)
{
    //Loops through current order & Prints each order
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
    int menu_index = Convert.ToInt32(Console.ReadLine());

    Console.Write("\n" + iceCream_menu + "\nSelect option to modify : ");
    int iceCream_index = Convert.ToInt32(Console.ReadLine());

    IceCream ice = customerDict[index].CurrentOrder.IceCreamList[menu_index - 1];

    switch (iceCream_index)
    {
        case 1:
            string[] o_menu = { "Cup", "Cone", "Waffle" };

            Console.WriteLine("\nAvilable types:");
            for (int i = 0; i < o_menu.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {o_menu[i]}");
            }

            Console.Write("\nEnter option : ");
            int option = Convert.ToInt32(Console.ReadLine());

            if (option == 1)
            {
                ice = new Cup("Cup", ice.Scoops, ice.Flavours, ice.Toppings);
            }
            else if (option == 2)
            {
                bool dipped = Cone();
                ice = new Cone("Cone", ice.Scoops, ice.Flavours, ice.Toppings, dipped);
            }
            else if (option == 3)
            {
                string waffleFlavour = Waffle();
                ice = new Waffle("Waffle", ice.Scoops, ice.Flavours, ice.Toppings, waffleFlavour);
            }
            customerDict[index].CurrentOrder.IceCreamList[menu_index - 1] = ice;
            break;
        case 2:
            Console.Write("\n-Scoops-\nEnter number of scoops : ");
            int scoops = Convert.ToInt16(Console.ReadLine());

            ice.Scoops = scoops;
            ice.Flavours = Flavours(scoops);
            break;
        case 3:
            ice.Flavours = Flavours(ice.Scoops);
            break;
        case 4:
            ice.Toppings = Toppings();
            break;
    }
}

void Option2(Dictionary<int, Customer> customerDict, int index)
{
    IceCream new_IceCream = CreateIceCream();

    customerDict[index].CurrentOrder.AddIceCream(new_IceCream);
    Console.WriteLine("\nNew Ice Cream added successfully.");
}

void Option3(Dictionary<int, Customer> customerDict, int index)
{
    if (customerDict[index].CurrentOrder.IceCreamList.Count > 1)
    {
        Console.Write("Select Ice Cream to delete : ");
        int menu_opt = Convert.ToInt32(Console.ReadLine());

        customerDict[index].CurrentOrder.DeleteIceCream(menu_opt - 1);
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

    //List customers
    ListAllCustomers(customerDict);

    //Select customer account
    Console.Write("\nEnter customer account Id : ");
    int customer_index = Convert.ToInt32(Console.ReadLine());

    //Display current order
    DisplayCurrentOrder(customerDict, customer_index);

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

    DisplayCurrentOrder(customerDict, customer_index);
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