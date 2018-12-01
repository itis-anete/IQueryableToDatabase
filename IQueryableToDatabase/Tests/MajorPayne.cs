namespace Tests
{
    public class MajorPayne
    {
        public MajorPayne(int height, Boots boots, string song, bool feet)
        {
            Height = height;
            Boots = boots;
            Song = song;
            Feet = feet;
        }

        public int Height { get; set; }
        public Boots Boots { get; set; }
        public string Song { get; set; }
        public bool Feet { get; set; }

        public override string ToString()
            => $"{Height}, {Boots}, {Song}, {Feet}";
    }

    public class Boots
    {
        public Boots(Color color, double size)
        {
            Color = color;
            Size = size;
        }

        public Color Color { get; set; }
        public double Size { get; set; }

        public override string ToString() => $"{Color} : {Size}";
    }

    public enum Color
    {
        Black,
        Red,
        Brown,
        White
    }
}
