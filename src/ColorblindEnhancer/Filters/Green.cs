namespace ColorblindEnhancer.Filters;

internal class Green : IFilter
{
    public string Name => "Green";

    public string Description => "Grayscale image using only the green channel";

    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b)
    {
        return (g, g, g);
    }
}