//==========================================================
// Student Number : S10255965
// Student Name : Chan Man Lock Jake
// Partner Name : An Yong Shyan
//==========================================================
namespace ICTreatsSystem
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
            //Console.WriteLine(add_points);
            //Console.WriteLine(Tier);
        }

        public void RedeemPoints(int use_points)//Not Done
        {
            if (Points - use_points >= 0 && (Tier == "Silver" || Tier == "Gold"))
            {
                double cost_offset = use_points * 0.02;
                if (Tier == "Gold")
                {
                    int i = 0;
                }
            }
        }

        void punch()//Not Done
        {
            PunchCard++;
            if (PunchCard == 10)
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
