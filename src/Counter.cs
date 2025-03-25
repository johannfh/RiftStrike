namespace Riftstrike.src
{
    [GlobalClass]
    public partial class Counter : Resource
    {
        [Export]
        public int Value { get; private set; } = 0;

        public void Increment()
        {
            Value++;
            GD.Print($"Value increased to {Value}!");
        }
    }
}