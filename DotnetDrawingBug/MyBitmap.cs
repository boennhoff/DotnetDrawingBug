using System.Drawing;
using System.Drawing.Imaging;

namespace DotnetDrawingBug
{
    public sealed class MyBitmap : IDisposable
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphic;

        public MyBitmap(int width = 16, int height = 16)
        {
            bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            graphic = Graphics.FromImage(bitmap);

            graphic.FillRectangle(new SolidBrush(Color.Pink), new Rectangle(new Point(0, 0), new Size(width, height)));
        }

        public void Dispose()
        {
            graphic.Dispose();
            bitmap.Dispose();
        }

        public long GetHash()
        {
            return ObjectHash.ContentBased(this);
        }
    }
}
