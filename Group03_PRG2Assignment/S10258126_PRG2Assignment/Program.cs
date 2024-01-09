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

}

//3. Register a new customer
void RegisterCustomer()
{
    //Prompt user for info
    Console.Write("Please enter customers information (name, id number, date of birth): ");
    string customer_info = Console.ReadLine();
    string[] customer_data = customer_info.Split(",");

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
}

//4. Create a customer's order
void CreateOrder()
{

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