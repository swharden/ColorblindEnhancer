using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorblindEnhancer;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        this.TransparencyKey = Color.LimeGreen;
        comboBox1.Items.Add("Grayscale");
        comboBox1.SelectedIndex = 0;
    }

    private void timer1_Tick(object sender, EventArgs e)
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

        Bitmap filtered = Process.Grayscale(bmp);

        Image? oldImage = pictureBox2.Image;
        pictureBox2.Image = filtered;
        pictureBox2.Invalidate();
        oldImage?.Dispose();
    }
}
