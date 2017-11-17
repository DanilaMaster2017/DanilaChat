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
        public int TopUpControl { get; set; }
        public int BottomDownControl { get; set; }

        bool firstDownload = true;
        public ChatScroll()
        {
            scrollBar.ScrollbarSize = 
                (int)(scrollBar.ScrollbarSize * 1.5); 
        }

        protected override void checkRequestScroll()
        {
            if (BottomDownControl - TopUpControl > scrollBar.Height)
            {
                scrollBar.Maximum = int.MaxValue / 2;
                scrollBar.Minimum = scrollBar.Maximum -
                      (BottomDownControl - TopUpControl - scrollBar.Height);

                if (firstDownload)
                {
                    scrollBar.Value = scrollBar.Maximum;
                    firstDownload = false;
                }

                scrollBar.Show();
            }
        }

        protected override bool CheckRequestDownload()
        {
            return scrollBar.Minimum - scrollBar.Value < 2 * Height;
        }

    }
}
