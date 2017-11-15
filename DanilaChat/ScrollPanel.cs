using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using System.Runtime.InteropServices;

namespace DanilaChat
{
    public class ScrollPanel : Panel
    {
        protected MetroScrollBar scrollBar { get; set; }

        public int scrollPadding
        {
            get { return scrollBar.ScrollbarSize; }
        } 

        public ScrollPanel()
        {
            scrollBar = new MetroScrollBar();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            scrollBar.Theme = MetroFramework.MetroThemeStyle.Dark;
            scrollBar.Scroll += ScrollBar_Scroll;           
            scrollBar.ScrollbarSize = (int)(screenWidth / 227.5);
            ControlAdded += ScrollPanel_ControlAdded;
            ParentChanged += ScrollPanel_ParentChanged;
        }

        private void ScrollPanel_ParentChanged(object sender, EventArgs e)
        {
            scrollBar.Location = new Point(Right + 1, Top);
            scrollBar.Height = Height;
            Parent.Controls.Add(scrollBar);            
        }

        protected virtual void ScrollPanel_ControlAdded(object sender, ControlEventArgs e)
        { 
            if (Height >= PreferredSize.Height)
            {
                scrollBar.Hide();
            }
            else
            {
                scrollBar.Show();
                scrollBar.Maximum = PreferredSize.Height - Height;
            }
        }

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int shift = old_position - scrollBar.Value;
            ChangeDisplayArea(shift);
            old_position = scrollBar.Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ScrollWindow(
            IntPtr hWnd,
            int XAmount,
            int YAmount,
            RECT lpRect,
            RECT lpClipRect
            );

        int old_position = 0;

        private void ChangeDisplayArea(int shift)
        {

            RECT rect = new RECT
            {
                left = 0,
                top = 0,
                bottom = Height,
                right = Width
            };

            try
            {
                ScrollWindow(Handle, 0, shift, rect, rect);
            }
            catch
            {
                
            }
        }

    }
}
