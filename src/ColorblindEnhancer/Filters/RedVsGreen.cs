namespace ColorblindEnhancer.Filters;

internal class RedVsGreen : IFilter, IReversible
{
    public string Name => "Red vs Green";

    public string Description => "Display only red and green channels";

    public bool IsReversed { get; set; } = false;

    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b)
    {
        return IsReversed ? (g, r, g) : (r, g, r);
    }
}