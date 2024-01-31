//==========================================================
// Student Number : S10255965
// Student Name : Jake Chan Man Lock
// Partner Name : An Yong Shyan
//==========================================================

namespace S10258126_PRG2Assignment
{
    class PointCard
    {
        public int Points { get; set; } = 0;
        public int PunchCard { get; set; } = 0;
        public string Tier { get; set; } = "Ordinary";

        public PointCard() { }

        public PointCard(int input_points, int input_punchCard)
        {
            AddPoints(input_points);
            PunchCard = input_punchCard;
        }

        public void AddPoints(int add_points)
        {
            Points += add_points;
            if (Tier == "Ordinary")
            {
                if (Points >= 150) Tier = "Gold";
                else if (Points >= 50) Tier = "Silver";
            }
            else if (Tier == "Silver")
            {
                if (Points >= 100) Tier = "Gold";
            }
        }

        public void RedeemPoints(int use_points)
        {
            Points -= use_points;
        }

        public void Punch()
        {
            PunchCard++;
            if (PunchCard == 11)
            {
                PunchCard = 0;
            }
        }

        public override string ToString()
        {
            return $"{Tier,-17}{Points,-17}{PunchCard}";
        }
    }
}
