using ColorblindEnhancer.Filters;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorblindEnhancer;

public partial class Form1 : Form
{
    readonly IFilter[] Filters = FilterTools.GetFilters();

    public Form1()
    {
        InitializeComponent();
        this.TransparencyKey = Color.LimeGreen;

        foreach (IFilter filter in Filters)
            comboBox1.Items.Add(filter.Name);

        comboBox1.SelectedIndex = 0;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateCapture();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        UpdateCapture();
    }

    private void cbReverse_CheckedChanged(object sender, EventArgs e)
    {
        UpdateCapture();
    }

    private void UpdateCapture()
    {
        using Bitmap bmp = new(Width, Height, PixelFormat.Format32bppArgb);
        using Graphics gfx = Graphics.FromImage(bmp);

        Point sourcePoint = pictureBox1.PointToScreen(Point.Empty);
        gfx.CopyFromScreen(
            sourceX: sourcePoint.X,
            sourceY: sourcePoint.Y,
            destinationX: 0,
            destinationY: 0,
            blockRegionSize: pictureBox1.Size);

        IFilter filter = Filters[comboBox1.SelectedIndex];

        if (filter is IReversible reversibleFilter)
        {
            cbReverse.Enabled = true;
            reversibleFilter.IsReversed = cbReverse.Checked;
        }
        else
        {
            cbReverse.Enabled = false;
        }

        Bitmap filtered = FilterTools.Apply(bmp, filter);

        Image? oldImage = pictureBox2.Image;
        pictureBox2.Image = filtered;
        pictureBox2.Invalidate();
        oldImage?.Dispose();
    }
}
