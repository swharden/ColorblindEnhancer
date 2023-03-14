namespace ColorblindEnhancer.Filters;

internal class Red : IFilter
{
    public string Name => "Red";

    public string Description => "Grayscale image using only the red channel";

    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b)
    {
        return (r, r, r);
    }
}