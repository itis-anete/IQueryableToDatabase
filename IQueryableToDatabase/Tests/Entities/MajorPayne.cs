namespace Tests
{
    public class MajorPayne : Entity
    {
        public MajorPayne()
        {
        }

        public MajorPayne(int height, Boots boots, string song)
        {
            Height = height;
            Boots = boots;
            Song = song;
        }

        public int Height { get; set; }
        public Boots Boots { get; set; }
        public string Song { get; set; }
    }
}