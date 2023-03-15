using ColorblindEnhancer.Filters;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorblindEnhancer;

public partial class Form1 : Form
{
    readonly IFilter[] Filters = FilterTools.GetFilters();
    Bitmap? CaptureBmp;

    public Form1()
    {
        InitializeComponent();
        TransparencyKey = Color.LimeGreen;

        foreach (IFilter filter in Filters)
            comboBox1.Items.Add(filter.Name);

        comboBox1.SelectedIndex = 0;

        Move += (s, e) => cbEnhance.Checked = false;
        SizeChanged += (s, e) => cbEnhance.Checked = false;
        comboBox1.SelectedIndexChanged += (s, e) => ShowProcessedImage();
        cbReverse.CheckedChanged += (s, e) => ShowProcessedImage();
    }

    private void cbEnhance_CheckedChanged(object sender, EventArgs e)
    {
        if (cbEnhance.Checked)
        {
            SaveCapturedImage();
            ShowProcessedImage();
        }
        else
        {
            ShowTransparentBox();
        }
    }

    private void SaveCapturedImage()
    {
        var oldImage = CaptureBmp;

        CaptureBmp = new(Width, Height, PixelFormat.Format32bppArgb);
        using Graphics gfx = Graphics.FromImage(CaptureBmp);

        Point sourcePoint = pictureBox1.PointToScreen(Point.Empty);
        gfx.CopyFromScreen(
            sourceX: sourcePoint.X,
            sourceY: sourcePoint.Y,
            destinationX: 0,
            destinationY: 0,
            blockRegionSize: pictureBox1.Size);

        pictureBox1.Image = CaptureBmp;
        oldImage?.Dispose();
    }

    private void ShowTransparentBox()
    {
        var oldImage = pictureBox1.Image;
        pictureBox1.Image = null;
        oldImage?.Dispose();
    }

    private void ShowProcessedImage()
    {
        if (!cbEnhance.Checked)
            return;

        if (CaptureBmp is null)
            return;

        IFilter filter = Filters[comboBox1.SelectedIndex];

        cbReverse.Enabled = filter is IReversible;

        if (filter is IReversible reversibleFilter)
            reversibleFilter.IsReversed = cbReverse.Checked;

        Bitmap filtered = FilterTools.Apply(CaptureBmp, filter);
        var oldImage = pictureBox1.Image;
        pictureBox1.Image = filtered;

        if (oldImage != CaptureBmp)
            oldImage?.Dispose();
    }
}
