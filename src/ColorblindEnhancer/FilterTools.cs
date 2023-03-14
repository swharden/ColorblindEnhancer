using System.Reflection;

namespace ColorblindEnhancer;

public static class FilterTools
{
    public static IFilter[] GetFilters()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IFilter)))
            .Select(x => (IFilter)Activator.CreateInstance(x)!)
            .ToArray();
    }

    public static Bitmap Apply(Bitmap bmp, IFilter filter)
    {
        byte[,,] bytes = Process.ImageToArray(bmp);

        for (int y = 0; y < bytes.GetLength(0); y++)
        {
            for (int x = 0; x < bytes.GetLength(1); x++)
            {
                byte r = bytes[y, x, 0];
                byte g = bytes[y, x, 1];
                byte b = bytes[y, x, 2];

                (byte r2, byte g2, byte b2) = filter.Filter(r, g, b);
                bytes[y, x, 0] = r2;
                bytes[y, x, 1] = g2;
                bytes[y, x, 2] = b2;
            }
        }

        return Process.ArrayToImage(bytes);
    }
}
