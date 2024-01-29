//==========================================================
// Features: 1, 3, 4
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Chan Man Lock Jake
//==========================================================
// Features: 2, 5, 6
// Student Number : S10255965
// Student Name : Chan Man Lock Jake
// Partner Name : An Yong Shyan
//==========================================================
using ICTreatsSystem;

string customerFile = "customers.csv";
string orderFile = "orders.csv";
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //Create a Dictionary to store Customer Objects
Queue<Order> gold_queue = new Queue<Order>();
Queue<Order> regular_queue = new Queue<Order>();
int orderID = 0;

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
            for (int j =11; j < 15; j++)
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
    Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}\n");
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
    Console.Write("Enter your name : ");
    string name = Console.ReadLine();

    //Get account ID
    int id;
    while (true)
    {
        try
        {
            Console.Write("\nEnter your ID : ");
            id = Convert.ToInt32(Console.ReadLine());
            if (Convert.ToString(id).Length == 6)
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
    ListAllCustomers(customerDict); //Feature 1

    orderID++;
    int customer_index = ValidateUserID(customerDict);

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
            //string continue_order;

            IceCream iceCream = CreateIceCream(); //Create IceCream object via IceCream Method
            new_order.AddIceCream(iceCream); //Add IceCream object to IceCreamList attribute Order object

            current_order = new_order; //Add Order object to CurrentOrder attribute in Customer object

            Console.WriteLine("\nIce Cream added to order.");

            string continue_order = ValidateBool("\nAdd another ice cream? y/n : ");

            if (continue_order == "n") break; //Check if customer wants to continue adding more IceCream objects
        }

        customerDict[customer_index].CurrentOrder = current_order; //Add Order to CurrentORder attribute in Customer object

        //Queue orders
        if (customerDict[customer_index].Rewards.Tier == "Gold") gold_queue.Enqueue(new_order);
        else regular_queue.Enqueue(new_order);

        Console.WriteLine("Order successfully made.\n");
    }
}

//Cone Method
bool Cone()
{
    string dipped = ValidateBool("\nAdd chocolate - dipped cone ? y / n : ");

    if (dipped == "y") return true;
    else return false;
}

//Waffle Method
string Waffle()
{
    string[] waffle_menu = { "Red velvet", "Charcoal", "Pandan", "Original" };
    Console.WriteLine("\nAvailable waffle flavours: ");

    //Display Waffle flavours
    for (int e = 0; e < waffle_menu.Length; e++)
    {
        Console.WriteLine($"[{e + 1}] {waffle_menu[e]}");
    }
    int w_opt = ValidateInt("\nSelect waffle flavour : ", 4, false, 1);

    return waffle_menu[w_opt - 1];
}

//Flavour Method
List<Flavour> Flavours(int scoops)
{
    string[] flavour_menu = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea Salt" };
    List<Flavour> f_list = new List<Flavour>();

    Console.Write("\n-Flavours-");

    //Loop for number of scoops
    for (int i = 0; i < scoops;)
    {
        bool flag = true;
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
        int f_opt = ValidateInt("\nEnter flavour option : ", 6, false, 0);

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
            quantity = ValidateInt("\nEnter scoops of flavour : ", scoops - i, false, 2);

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
        string continue_topping = ValidateBool("Add toppings? y/n : ");

        if (continue_topping == "n") break; //Check if user wants to add toppings
        else if (continue_topping == "y")
        {
            //Display Toppings
            Console.WriteLine("\nAvailable Toppings:");
            for (int i = 0; i < topping_menu.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {topping_menu[i]}");
            }

            int t_opt = ValidateInt("\nEnter topping option : ", 4, false, 0);

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
    int option = ValidateInt("\nEnter option : ", 3, false, 0);

    if (option == 2) dipped = Cone();
    else if (option == 3) waffle = Waffle();

    //Scoops
    Console.Write("\n-Scoops-");

    int scoops = ValidateInt("\nEnter number of scoops : ", 3, false, 2);

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

    int customer_index = ValidateUserID(customerDict);

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
    string iceCream_menu =
        "=== Edit Ice Cream ===\n" +
        "[1] Option\n" +
        "[2] Scoops\n" +
        "[3] Flavours\n" +
        "[4] Toppings";

    int m_opt = ValidateInt("\nSelect Ice Cream to modify : ", customerDict[index].CurrentOrder.IceCreamList.Count, false, 3);

    Console.Write("\n" + iceCream_menu + "");

    int i_opt = ValidateInt("\nSelect component to modify : ", 4, false, 0);

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

void ModifyOrderDetails(Dictionary<int, Customer> customerDict)
{
    string edit_menu =
        "=== Edit Order Menu ===\n" +
        "[1] Modify Ice Cream\n" +
        "[2] Add Ice Cream\n" +
        "[3] Delete Ice Cream\n";

    ListAllCustomers(customerDict); //Feature 1

    int customer_index = ValidateUserID(customerDict);

    DisplayCurrentOrder(customerDict, customer_index); //Feature 5 - Display current order Method

    if (customerDict[customer_index].CurrentOrder != null)
    { 
        Console.Write("\n" + edit_menu);

        int menu_opt = ValidateInt("\nSelect option : ", 3, false, 0);

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

//Option 7
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
                //total += i.CalculatePrice();
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
                    //total += i.CalculatePrice();
                    cost.Add(i.CalculatePrice());
                }
            }
            else
            {
                Console.WriteLine("Regular queue is empty.");
                break;
            }
        }
        //Console.WriteLine($"Total bill amount : ${total.ToString("0.00")}");

        foreach (KeyValuePair<int, Customer> kvp in customerDict)
        {
            if (kvp.Value.CurrentOrder != null && kvp.Value.CurrentOrder.Id == order.Id)
            {
                customer = kvp.Value;
                Console.WriteLine($"{"Name",-10}{"MemberId",-10}{"DateOfBirth",-12}{"MembershipStatus",-17}{"MembershipPoints",-17}{"PunchCard"}");
                Console.WriteLine(customer);
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
            string do_offset = ValidateBool("Redeem points to offset bill? y/n : ");
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
        Console.WriteLine($"Points earned : ${points:0.00}");
        customer.Rewards.AddPoints(points);

        Console.WriteLine("Press any key to make payment . . .");
        Console.ReadLine();

        order.TimeFulfilled = DateTime.Now;
        customer.CurrentOrder = null;
        customer.OrderHistory.Add(order); //Add Order to OrderHistory attribute in Customer object

        break;
    }
}

//Main Program
ExtractCustomer(customerFile);
ExtractOrder(orderFile, customerDict);

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
        "[7] Order Checkout\n" +
        "[0] Exit system";

    Console.WriteLine(sys_menu);

    int menu_opt = ValidateInt("\nSelect option : ", 7, true, 0);

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
        case 7:
            ProcessOrderCheckout(customerDict, gold_queue, regular_queue);
            break;
    }
}
Console.ReadLine();