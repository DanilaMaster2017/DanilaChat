using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace DanilaChat
{
    public class ChatScreen : Form1
    {
        Label shadowLabel;
        public ChatScreen(string[] messages, Human I, Human friend)
        {
            double ratio = 128 / 14.0;

            TitleLabel.Text = friend.Name + " " + friend.Surname;

            Panel bottomPanel = new Panel();
            bottomPanel.BackColor = Color.White;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = (int)(Body.Height / ratio);

            Body.Controls.Add(bottomPanel);

            shadowLabel = new Label();
            shadowLabel.Text = "Ваше сообщение...";
            shadowLabel.ForeColor = Color.FromArgb(146, 158, 176);
            shadowLabel.Font = chatFont;
            shadowLabel.AutoSize = true;
            shadowLabel.Enabled = false;

            bottomPanel.Controls.Add(shadowLabel);
            
            TextBox textBox = new TextBox();
            textBox.Font = chatFont;
            textBox.Multiline = true;
            textBox.KeyDown += TextBox_KeyDown;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.BorderStyle = BorderStyle.None;
            

            int imageSize = (int)(bottomPanel.Height / 2.0);
            int imagePadding = (int)(imageSize / 2.0);

            PictureBox clipPicture = new PictureBox();
            clipPicture.Size = new Size(imageSize, imageSize);
            clipPicture.Location = new Point(imagePadding, imagePadding);
            clipPicture.Image = Properties.Resources.clipImage;
            clipPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            clipPicture.BringToFront();

            bottomPanel.Controls.Add(clipPicture);

            PictureBox smailPicture = new PictureBox();
            smailPicture.Size = new Size(imageSize, imageSize);
            smailPicture.Location = new Point
                (bottomPanel.Width - 2 * imagePadding - imageSize, imagePadding);
            smailPicture.Image = Properties.Resources.smailImage;
            smailPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            smailPicture.BringToFront();

            bottomPanel.Controls.Add(smailPicture);
            bottomPanel.Controls.Add(textBox);

            Point textBoxLocation = new Point(imageSize + 2 * imagePadding, 
                (int)(bottomPanel.Height / 3.0));
            textBox.Location = textBoxLocation;
            textBox.Size = new Size
                (bottomPanel.Width - 2 * imageSize - 4 * imagePadding, 
                (int)(bottomPanel.Height/3.0));

            shadowLabel.Location = new Point(textBoxLocation.X + 1, 
                textBoxLocation.Y);

            int eventHeight = (int)(0.6 * bottomPanel.Height); 
            Panel eventPanel = new Panel();
            eventPanel.Size = new Size(Body.Width, eventHeight);
            eventPanel.Location = new Point(0, bottomPanel.Top - eventPanel.Height);
            
            Body.Controls.Add(eventPanel);

            List<string> listMessage = PrepareListString(messages);
            int begIndex = listMessage.Count - 1;

            for(int i = 0;  i < listMessage.Count / 3; i++)
            {
                
                Label messageLabel = new Label();
                messageLabel.ForeColor = Color.Black;
                messageLabel.AutoSize = true;
                messageLabel.Text = listMessage[begIndex - 1 - 3 * i];
                messageLabel.Font = chatFont;
                int padding = 10;

                AvatarBox avatar = new AvatarBox();
                avatar.SizeMode = PictureBoxSizeMode.StretchImage;
                int avatarSize = (int)(bottomPanel.Height / 1.5);
                avatar.Size = new Size(avatarSize, avatarSize);

                Panel messagePanel = new Panel();
                messagePanel.Controls.Add(messageLabel);

                int paddingContainer = (int)(padding / 2.0);
                Panel containerPanel = new Panel();

                containerPanel.Controls.Add(avatar);
                containerPanel.Controls.Add(messagePanel);
                Body.Controls.Add(containerPanel);

                Size messageSize = messageLabel.Size;
                messagePanel.Size = new Size
                    (messageSize.Width + 2 * padding,
                    messageSize.Height + 2 * padding);
                messagePanel.Padding = new Padding(padding);

                BunifuElipse ellipse = new BunifuElipse();
                ellipse.TargetControl = messagePanel;
                ellipse.ElipseRadius = padding;

                
                                containerPanel.Size = new Size
                    (Body.Width, messagePanel.Height + 2 * paddingContainer);
               // containerPanel.Padding = new Padding(paddingContainer);

                Control lastControl = Body.Controls[Body.Controls.Count - 2];
                containerPanel.Location = new Point
                    (0, lastControl.Top - containerPanel.Height);

   
                
                int avatarPadding = (int)(avatarSize / 5.0);

                if (friend.UserId == int.Parse(listMessage[begIndex - 2 - 3 * i]))
                {
                    avatar.Location = new Point(avatarPadding, 
                        containerPanel.Height - avatarSize - paddingContainer);

                    if (friend.Avatar == null)
                        avatar.Image = Properties.Resources.AvatarDefault;

                    int leftMessage = avatar.Right + avatarPadding;
                    messagePanel.Location = new Point(leftMessage, paddingContainer);
                    messagePanel.BackColor = Color.White;
                }
                else
                {
                    int leftMessage = Body.Width - 2 * avatarPadding
                        - avatarSize - messagePanel.Width;
                    messagePanel.Location = new Point(leftMessage, paddingContainer);
                    messagePanel.BackColor = Color.FromArgb(212, 231, 250);

                    avatar.Location = new Point(messagePanel.Right + avatarPadding,
                        containerPanel.Height - avatarSize -paddingContainer);

                    if (I.Avatar == null)
                        avatar.Image = Properties.Resources.AvatarDefault;
                    
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.TextLength == 0) shadowLabel.Show();
            else shadowLabel.Hide(); 
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) return;

                textBox.Text = "";
            }
            
        }

        List<string> PrepareListString(string[] messages)
        {
            List<string> returnValue = new List<string>();
            int insideIdex = 1;

            while (insideIdex < messages.Length - 1)
            {
                returnValue.Add(messages[insideIdex - 1]);

                string message = "";
                int countSymbolInRow = 0;


                bool notEnd = true;
                while (notEnd)
                {
                    if (messages[insideIdex].Contains("#EndMessage#"))
                    {
                        notEnd = false;
                        messages[insideIdex] =
                            messages[insideIdex].Replace("#EndMessage#", "");
                    }

                    if (countSymbolInRow + messages[insideIdex].Length > 28)
                    {
                        countSymbolInRow = 0;
                        message += "\n";
                    }

                    countSymbolInRow += messages[insideIdex].Length + 1;
                    message += messages[insideIdex] + " ";
                    insideIdex++;
                }
                message = message.Remove(message.Length - 1);
                returnValue.Add(message);

                returnValue.Add(messages[insideIdex] + " "
                    + messages[insideIdex + 1]);

                insideIdex += 3;
            }

            return returnValue;
        }
    }
}
