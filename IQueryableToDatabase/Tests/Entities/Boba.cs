namespace Tests
{
    public class Boba : Entity
    {
        public Boba()
        {
        }

        public Boba(Boots boots, bool feet)
        {
            Boots = boots;
            Feet = feet;
        }

        public Boots Boots { get; set; }
        public bool Feet { get; set; }
    }
}