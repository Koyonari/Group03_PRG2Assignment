﻿using ICTreatsSystem;

string customerFile = "customers.csv";
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //Create a Dictionary to store Customer Objects
Queue<Customer> gold_queue = new Queue<Customer>();
Queue<Customer> reg_queue = new Queue<Customer>();

void ExtractCustomer(string filename) //Reads File, Creates objects
{
    string[] customer_file = File.ReadAllLines(filename); //Array

    for (int i = 0; i < customer_file.Length; i++)
    {
        string[] customer_details = customer_file[i].Split(","); //Nested Array
        if (i != 0)
        {
            Customer new_customer = new Customer(customer_details[0], Convert.ToInt32(customer_details[1]), Convert.ToDateTime(customer_details[2])); //Creates Customer Object
            customerDict.Add(new_customer.MemberId, new_customer); //Adds Customer Object to Dictionary
        }
    }
}

void ListAllCustomers(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}\n");
    foreach (KeyValuePair<int, Customer> kvp in customerDict)
    {
        Console.WriteLine(kvp.Value);
    }
}

void ListAllCurrentOrders(Dictionary<int, Customer> customerDict)
{
    foreach (KeyValuePair<int, Customer> kvp in customerDict)
    {
        kvp.Value.MakeOrder();
        Console.WriteLine(kvp.Value.CurrentOrder);
    }
}

void RegisterNewCustomer(Dictionary<int, Customer> customerDict, string filename)
{
    Console.Write("Enter your name : ");
    string name = Console.ReadLine();
    Console.Write("Enter your ID : ");
    int id = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter your date of birth DD/MM/YYYY : ");
    DateTime dob = Convert.ToDateTime(Console.ReadLine());

    Customer customer = new Customer(name, id, dob);
    PointCard pointcard = new PointCard();

    customer.Rewards = pointcard;
    customerDict.Add(customer.MemberId, customer);

    string updatefile = $"{customer.Name},{Convert.ToInt32(customer.MemberId)},{customer.Dob.ToString("dd/MM/yyyy")}";

    using (StreamWriter sw = File.AppendText(filename))
    {
        sw.WriteLine(updatefile);
    }
}

void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{
    //Retrieve the selected customer
    ListAllCustomers(customerDict);

    Console.Write("\nEnter customer account Id : ");
    int opt = Convert.ToInt32(Console.ReadLine());

    //Create order object
    Order new_order = new Order(opt, DateTime.Now);

    //Link order tp current order
    customerDict[opt].CurrentOrder = new_order;

    //Create ice cream
    IceCream new_icecream = new Cup();

    Console.WriteLine("-Type-");
    while (true)
    {
        //Option
        string[] o_menu = { "Cup", "Cone", "Waffle" };
        string[] w_menu = { "Red velvet", "Charcoal", "Pandan" };
        bool dipped = false;
        string waffleFlavour = "";

        Console.WriteLine("Avilable types:");
        for (int i = 0; i < o_menu.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {o_menu[i]}");
        }

        Console.Write("\n-Type-\nEnter option : ");
        int option = Convert.ToInt32(Console.ReadLine());
        new_icecream.Option = o_menu[option - 1];

        if (option == 2)
        {
            Console.Write("Add chocolate-dipped cone? y/n : ");
            string dip = Console.ReadLine();
            if (dip == "y") dipped = true;
        }
        else if (option == 3)
        {
            Console.WriteLine("Available waffle flavours: ");
            for (int e = 0; e < o_menu.Length; e++)
            {
                Console.WriteLine($"[{e + 1}] {w_menu[e]}");
            }
            Console.Write("Select waffle flavour : ");
            int waffF = Convert.ToInt32(Console.ReadLine());
            waffleFlavour = w_menu[waffF - 1];
        }

        //Scoops
        Console.Write("\n-Scoops-\nEnter number of scoops : ");
        int scoops = Convert.ToInt16(Console.ReadLine());
        new_icecream.Scoops = scoops;

        //Flavours
        string[] f_menu = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea salt" };
        List<Flavour> f_list = new List<Flavour>();
        string f_type = "";
        bool premium = false;
        int quantity = 1;

        Console.Write("\n-Flavours-");

        for (int i = 0; i < scoops;)
        {
            Console.WriteLine("\nAvailable Flavours: ");
            for (int e = 0; e < f_menu.Length; e++)
            {
                if (e == 0) Console.WriteLine("Ordinary");
                else if (e == 3) Console.WriteLine("Premium");
                Console.WriteLine($"[{e + 1}] {f_menu[e]}");
            }

            Console.Write("\nEnter flavour option : ");
            int f_opt = Convert.ToInt16(Console.ReadLine());
            if (f_opt >= 3 && f_opt <= 6) premium = true;
            if (scoops == 1)
            {
                Flavour flavour = new Flavour(f_type, premium, quantity);
                new_icecream.Flavours.Add(flavour);
                break;
            }
            else
            {
                Console.Write("Enter scoops of flavour : ");
                quantity = Convert.ToInt16(Console.ReadLine());
                Flavour f_add = new Flavour(f_type, premium, quantity);
                new_icecream.Flavours.Add(f_add);
                i += quantity;
            }
        }

        //Toppings
        string[] t_menu = { "Sprinkles", "Mochi", "Sago", "Oreos" };
        List<Topping> t_list = new List<Topping>();
        string t_type = "";

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
                //print menu
                Console.WriteLine("\nAvailable Toppings:");
                for (int i = 0; i < t_menu.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {t_menu[i]}");
                }

                Console.Write("\nEnter topping option : ");
                int t_opt = Convert.ToInt16(Console.ReadLine());
                Topping topping = new Topping(t_menu[t_opt - 1]);
                t_list.Add(topping);
                new_icecream.Toppings.Add(topping);
            }
        }
        Console.WriteLine(new_icecream);
    }
}

ExtractCustomer(customerFile);

while (true)
{
    string sys_menu =
        "=== IceCream Menu ===\n" +
        "[1] List all customers\n" +
        "[2] List all current orders\n" +
        "[3] Register a new customer\n" +
        "[4] Create customer order\n" +
        "[0] Exit system\n";

    Console.Write(sys_menu + "\nSelect option : ");
    int menu_opt = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine($"=== {menu_opt} ===");
    switch (menu_opt)
    {
        case 0:
            Console.WriteLine("Exited.");
            break;
        case 1:
            ListAllCustomers(customerDict);
            break;
        case 2:
            break;
        case 3:
            RegisterNewCustomer(customerDict, customerFile);
            break;
        case 4:
            CreateCustomerOrder(customerDict);
            break;
        case 5:
            Console.WriteLine(customerDict[685582]);
            break;
    }
}
Console.WriteLine();
//ListAllCurrentOrders(customerDict);

//PointCard pp = new PointCard(150,0);
//Console.WriteLine($"{pp}");

Console.ReadLine();