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
        public ChatScroll()
        {
            scrollBar.ScrollbarSize = 
                (int)(scrollBar.ScrollbarSize * 1.5); 
        }

        protected override void ScrollPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            if (scrollBar.Height == Height)
            {
                scrollBar.Hide();
            }
            else
            {
                scrollBar.Show();
                scrollBar.Maximum = Height -  2 * scrollBar.Height 
                    + Parent.Controls[0].Height;

                scrollBar.Value = scrollBar.Maximum;
            }
        }
    }
}
