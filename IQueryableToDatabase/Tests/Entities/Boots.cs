namespace Tests
{
    public class Boots : Entity
    {
        public Boots()
        {
        }

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