//==========================================================
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Chan Man Lock Jake
//==========================================================
namespace ICTreatsSystem
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
