using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SmcEslLib
{
    public class SmcDataToImage
    {
        public SmcDataToImage()
        {
        }

        public Bitmap ConvertBoxToImage(Bitmap mbmp, Label label, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                SolidBrush solidBrush = new SolidBrush(label.BackColor);
                Rectangle rectangle = new Rectangle(x, y, 65, 19);
                graphic.FillRectangle(solidBrush, rectangle);
                graphic.DrawString(label.Text, label.Font, new SolidBrush(textcolor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertImageToImage(Bitmap mbmp, Bitmap img, int x, int y)
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if ((pixel.R + pixel.B + pixel.G) / 3 < 180)
                    {
                        mbmp.SetPixel(i + x, j + y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, TextBox textbox, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                StringFormat stringFormat = new StringFormat();
                if (textbox.TextAlign == HorizontalAlignment.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    graphic.DrawString(textbox.Text, textbox.Font, new SolidBrush(textcolor), (float)(x + textbox.Width / 2), (float)y, stringFormat);
                }
                else if (textbox.TextAlign != HorizontalAlignment.Right)
                {
                    graphic.DrawString(textbox.Text, textbox.Font, new SolidBrush(textcolor), (float)x, (float)y);
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Far;
                    graphic.DrawString(textbox.Text, textbox.Font, new SolidBrush(textcolor), (float)(x + textbox.Width), (float)y, stringFormat);
                }
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, Label label, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphic.DrawString(label.Text, label.Font, new SolidBrush(textcolor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, string txt, Font font1, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphic.DrawString(txt, font1, new SolidBrush(textcolor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }
    }
}
