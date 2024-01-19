//==========================================================
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }
        public Flavour() { }
        public Flavour(string t, bool p, int q)
        {
            Type = t;
            Premium = p;
            Quantity = q;
        }
        public override string ToString()
        {
            if (Premium)
            {
                return $"{Quantity} Premium {Type}";
            }
            else
            {
                return $"{Quantity} {Type}";
            }
        }

    }
}
