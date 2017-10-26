using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DanilaChat
{
    class AvatarBox : PictureBox
    {
        public AvatarBox()
        {
            Paint += AvatarImage_Paint;
        }

        private void AvatarImage_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            SolidBrush brush = new SolidBrush(control.Parent.BackColor);

            Graphics g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(e.ClipRectangle);
            path.AddEllipse(e.ClipRectangle);

            g.FillPath(brush, path);
        }
    }
}
