//==========================================================
// Student Number : S10258126B
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    public class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }
        public PointCard() { }
        public PointCard(int p, int pc)
        {
            Points = pc;
            PunchCard = pc;
        }
        public AddPoints(int p)
        {

        }
        public RedeemPoints(int p)
        {

        }
        public Punch() { }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
