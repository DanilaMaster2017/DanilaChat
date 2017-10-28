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
        Human I;
        Human friend;

        Label shadowLabel;
        int avatarSize;

        public ChatScreen(string[] messages, Human I, Human friend)
        {
            this.I = I;
            this.friend = friend;

            double ratio = 128 / 14.0;

            TitleLabel.Text = friend.Name + " " + friend.Surname;

            Panel bottomPanel = new Panel();
            bottomPanel.BackColor = Color.White;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = (int)(Body.Height / ratio);

            Body.Controls.Add(bottomPanel);
            avatarSize = (int)(bottomPanel.Height / 1.5);

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
            textBox.KeyUp += TextBox_KeyUp;
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

            List<string> listMessage = PrepareString(messages);
            bool iSender;

            for(int i = 0;  i < listMessage.Count / 3; i++)
            {
                iSender = (int.Parse(listMessage[3 * i]) == I.UserId) ?
                    true : false;

                ShowMessage(listMessage[1 + 3 * i], iSender);
            }
        }

 

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.TextLength == 0) shadowLabel.Show();
            else shadowLabel.Hide(); 
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) return;
                textBox.Text = "";

            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) return;
                
                string messageText = textBox.Text;
                textBox.Text = "";

                ShowMessage(messageText, true);
            }
            
        }

        void ShiftTopControl(int shift)
        {
            for (int i = Body.Controls.Count - 2; i > 1; i--)
            {
                Body.Controls[i].Top -= shift;
            }
        }

        void ShowMessage(string text, bool iSender)
        {
            text = LinesFromMessage(text);

            Label messageLabel = new Label(); 
            messageLabel.ForeColor = Color.Black;
            messageLabel.AutoSize = true;
            messageLabel.Text = text;
            messageLabel.Font = chatFont;
            int padding = 10;

            AvatarBox avatar = new AvatarBox();
            avatar.SizeMode = PictureBoxSizeMode.StretchImage;
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

            Control eventPanel = Body.Controls[1];
            containerPanel.Location = new Point
                (0, eventPanel.Top - containerPanel.Height);


            int avatarPadding = (int)(avatarSize / 5.0);


            if (!iSender)
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
                    containerPanel.Height - avatarSize - paddingContainer);

                if (I.Avatar == null)
                    avatar.Image = Properties.Resources.AvatarDefault;

            }

            ShiftTopControl(containerPanel.Height);
        }

        string LinesFromMessage(string message)
        {
            string[] words = message.Split(' ');
            int countSymbolInRow = 0;
            message = "";

            for(int i = 0; i < words.Length; i++)
            {                
                if (countSymbolInRow + words[i].Length > 28)
                {
                    countSymbolInRow = 0;
                    message += "\n";
                }

                countSymbolInRow += words[i].Length + 1;
                message += words[i] + " ";
            }
            message = message.Remove(message.Length - 1);

            return message;
        }

        List<string> PrepareString(string[] messages)
        {
            List<string> returnValue = new List<string>();
            int insideIndex = 1;

            string message;
            bool notEnd;

            while (insideIndex < messages.Length - 1)
            {
                returnValue.Add(messages[insideIndex - 1]);

                message = "";
                notEnd = true;

                while (notEnd)
                { 
                    if (messages[insideIndex].Contains("#EndMessage#"))
                    {
                        notEnd = false;
                        messages[insideIndex] =
                            messages[insideIndex].Replace("#EndMessage#", "");
                    }
                    message += messages[insideIndex] + " ";
                    insideIndex++;
                }  
                                             
                message = message.Remove(message.Length - 1);
                returnValue.Add(message);

                returnValue.Add(messages[insideIndex] + " "
                    + messages[insideIndex + 1]);

                insideIndex += 3;
            }

            return returnValue;
        }
    }
}

