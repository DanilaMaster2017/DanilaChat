using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanilaChat
{
    public class ChatScroll : ScrollPanel
    {
        public Control TopControl { get; set; }
        public Control BottomControl { get; set; }

        bool firstDownload = true;
        public ChatScroll()
        {
            scrollBar.ScrollbarSize = 
                (int)(scrollBar.ScrollbarSize * 1.5);


             scrollBar.Maximum = int.MaxValue / 2;
             scrollBar.Value = scrollBar.Maximum;
        }

        protected override void checkRequestScroll()
        {
            if (Controls.Count == 0) return;

            if (BottomControl.Bottom - TopControl.Top > scrollBar.Height)
            {
                scrollBar.Maximum = int.MaxValue / 2;
                scrollBar.Minimum = scrollBar.Maximum -
                      (BottomControl.Bottom - TopControl.Top - scrollBar.Height);
                
                if (firstDownload)
                {
                   // scrollBar.Scroll -= ScrollBar_Scroll; 
                   scrollBar.Value = scrollBar.Maximum;
                    //scrollBar.Scroll += ScrollBar_Scroll;
                    firstDownload = false;
                }

                scrollBar.Show();
            }
        }

        private void ScrollBar_Scroll1(object sender, ScrollEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool CheckRequestDownload()
        {
            return scrollBar.Minimum - scrollBar.Value < 2 * Height;
        }

    }
}
