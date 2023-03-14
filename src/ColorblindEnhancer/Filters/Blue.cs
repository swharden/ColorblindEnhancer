namespace ColorblindEnhancer.Filters;

internal class Blue : IFilter
{
    public string Name => "Blue";

    public string Description => "Grayscale image using only the blue channel";

    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b)
    {
        return (b, b, b);
    }
}