//==========================================================
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Topping
    {
        public string Type { get; set; }

        public Topping() { }

        public Topping(string input_type)
        {
            Type = input_type;
        }

        public override string ToString()
        {
            return $" + {Type}";
        }
    }
}
