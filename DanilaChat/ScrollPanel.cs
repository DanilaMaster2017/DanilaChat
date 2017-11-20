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
        delegate void scrollDelegate();
        public event EventHandler requestDownload;

        public MetroScrollBar scrollBar { get; set; }

        bool contentAdded = true;
        public bool ContentAdded
        {
            get { return contentAdded; }
            set
            {
                scrollDelegate scrollMethod;
                contentAdded = value;

                if (contentAdded)
                {
                    scrollMethod = scrollBar.Hide;
                    Invoke(scrollMethod);
                }
                else
                {
                    scrollMethod = checkRequestScroll;
                    Invoke(scrollMethod);
                }
            }
        }

        protected virtual void checkRequestScroll()
        {
            if (Height < PreferredSize.Height)
            {
                int bottomLastControl = Controls[Controls.Count - 1].Bottom;
                int topBeginControl = Controls[0].Top;

                scrollBar.Maximum = bottomLastControl - topBeginControl - Height;

                scrollBar.Show();
            }
        }

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
            ParentChanged += ScrollPanel_ParentChanged;
        }

        private void ScrollPanel_ParentChanged(object sender, EventArgs e)
        {
            scrollBar.Location = new Point(Right + 1, Top);
            scrollBar.Height = Height;
            Parent.Controls.Add(scrollBar);            
        }


        protected void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {            
            int shift = old_position - scrollBar.Value;
            ChangeDisplayArea(shift);
            old_position = scrollBar.Value;

            if (CheckRequestDownload())
            {
                requestDownload?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual bool CheckRequestDownload()
        {
            return scrollBar.Maximum - scrollBar.Value < 2 * Height;
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
