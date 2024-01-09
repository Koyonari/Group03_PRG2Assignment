using ICTreatsSystem;

string customerFile = "customers.csv";
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //Create a Dictionary to store Customer Objects

void ExtractCustomer(string filename) //Reads File, Creates objects
{
    string[] customer_file = File.ReadAllLines(filename); //Array

    for (int i = 0; i < customer_file.Length; i++)
    {
        string[] customer_details = customer_file[i].Split(","); //Nested Array
        if (i != 0)
        {
            Customer new_customer = new Customer(customer_details[0], Convert.ToInt32(customer_details[1]), Convert.ToDateTime(customer_details[2])); //Creates Customer Object
            customerDict.Add(new_customer.memberId, new_customer); //Adds Customer Object to Dictionary
        }
    }
}

ExtractCustomer(customerFile);

foreach (KeyValuePair<int, Customer> kvp in customerDict)
{
    Console.WriteLine(kvp.Value);
}

Console.ReadLine();