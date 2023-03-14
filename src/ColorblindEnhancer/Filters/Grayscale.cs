namespace ColorblindEnhancer.Filters;

internal class Grayscale : IFilter, IReversible
{
    public string Name => "Grayscale";

    public string Description => "White intensity based upon the mean of all color channels";

    public bool IsReversed { get; set; } = false;

    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b)
    {
        byte mean = (byte)((r + g + b) / 3);

        if (IsReversed)
            mean = (byte)(255 - mean);

        return (mean, mean, mean);
    }
}