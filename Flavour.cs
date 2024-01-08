//==========================================================
// Student Number : S10258126B
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    public class Flavour
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
            return base.ToString();
        }
    }
}
