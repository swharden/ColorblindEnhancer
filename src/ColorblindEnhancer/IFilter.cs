namespace ColorblindEnhancer;

public interface IFilter
{
    public string Name { get; }
    public string Description { get; }
    public (byte r, byte g, byte b) Filter(byte r, byte g, byte b);
}
