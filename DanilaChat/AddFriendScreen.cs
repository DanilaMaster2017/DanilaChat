using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Bunifu.Framework.UI;

namespace DanilaChat
{
    public class AddFriendScreen : Form1
    {
        Panel leftPanel;
        Label shadowLabel;
        TextBox textBox;
        PictureBox resetPicture;
        Color lineColor = Color.FromArgb(237, 237, 237);

        double millimetr;

        // доработать text box

        public AddFriendScreen()
        {          
            Color labelColor = Color.Gray;

            double ratio = 1 / 94.0;

            millimetr = Width * ratio;
            int padding = (int)(millimetr * 5);

            Width = (int)(Width * 2.9);
            Height = (int)(Height * 1.5);
            millimetr *= 1.5; 


            leftPanel = new Panel();
            leftPanel.BackColor = Color.White;
            leftPanel.Location = new Point(0, header.Bottom + padding);
            leftPanel.Size = new Size((int)(38.6 * padding), 
                Height - header.Height - padding);
            Body.Controls.Add(leftPanel);

            Panel firstPanel = new Panel();
            firstPanel.BackColor = Color.FromArgb(250, 251, 252);
            firstPanel.Size = new Size(leftPanel.Width, (int)(13.5 * millimetr));

            leftPanel.Controls.Add(firstPanel);

            Label peopleLable = new Label();
            peopleLable.Text = "Люди  ";
            peopleLable.Font = new Font("Tahoma", 10, FontStyle.Bold);
            peopleLable.AutoSize = true;
            peopleLable.Location = new Point(padding, (int)(5 * millimetr));

            firstPanel.Controls.Add(peopleLable);

            Label countLable = new Label();
            countLable.Text = "100";
            countLable.AutoSize = true;
            countLable.Font = new Font("Tahoma", 11);
            countLable.ForeColor = Color.FromArgb(147, 147, 147);
            countLable.Location = new Point(peopleLable.Right, peopleLable.Top);            

            firstPanel.Controls.Add(countLable);

            BunifuSeparator firstLine = new BunifuSeparator();
            firstLine.LineColor = lineColor;
            firstLine.Height = 1;
            firstLine.Location = new Point(0, firstPanel.Height - firstLine.Height);
            firstLine.Width = leftPanel.Width;
            firstPanel.Controls.Add(firstLine);

            Panel secondPanel = new Panel();
            secondPanel.Size = new Size(leftPanel.Width, (int)(11.0 * millimetr));
            secondPanel.Location = new Point(0, firstPanel.Bottom + 1);

            leftPanel.Controls.Add(secondPanel);

            PictureBox searchPicture = new PictureBox();
            searchPicture.Image = Properties.Resources.search;
            searchPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            searchPicture.Size = new Size
                ((int)(4.8 * millimetr), (int)(4.8 * millimetr));
            int topImage = GetLeftPosition(searchPicture.Height, secondPanel.Height);
            searchPicture.Location = new Point(padding, topImage);

            secondPanel.Controls.Add(searchPicture);

            resetPicture = new PictureBox();
            resetPicture.Visible = false;
            resetPicture.Image = Properties.Resources.reset;
            resetPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            resetPicture.Size = new Size
                ((int)(3.5 * millimetr), (int)(3.5 * millimetr));
            topImage = GetLeftPosition(resetPicture.Height, secondPanel.Height);
            resetPicture.Location = new Point
                (secondPanel.Width - resetPicture.Width - padding, topImage);

            resetPicture.MouseEnter += ResetPicture_MouseEnter;
            resetPicture.MouseLeave += ResetPicture_MouseLeave;
            resetPicture.Click += ResetPicture_Click;

            secondPanel.Controls.Add(resetPicture);


            textBox = new TextBox();
            textBox.Font = chatFont;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Size = new Size
                (secondPanel.Width - resetPicture.Width 
                - searchPicture.Width - 4 * padding,
                secondPanel.Height);
            int textBoxTop = GetLeftPosition(textBox.Height, secondPanel.Height);

            textBox.Location = new Point(searchPicture.Right + padding, textBoxTop);
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyPress += TextBox_KeyPress;            

            shadowLabel = new Label();
            shadowLabel.Text = "Начните вводить любое имя";
            shadowLabel.Font = chatFont;
            shadowLabel.AutoSize = true;
            shadowLabel.Enabled = false;
            shadowLabel.Location = new Point(textBox.Left + 1, textBox.Top);

            secondPanel.Controls.Add(shadowLabel);
            secondPanel.Controls.Add(textBox);

            BunifuSeparator secondLine = new BunifuSeparator();
            secondLine.LineColor = lineColor;
            secondLine.Height = 1;
            secondLine.Location = new Point(0, secondPanel.Height - secondLine.Height);
            secondLine.Width = leftPanel.Width;
            secondPanel.Controls.Add(secondLine);

            Panel rightPanel = new Panel();
            rightPanel.BackColor = Color.White;
            rightPanel.Location = new Point(leftPanel.Right + padding, 
                leftPanel.Top);
            rightPanel.Size = new Size(Width - rightPanel.Left, 
                leftPanel.Height);
            Body.Controls.Add(rightPanel);

            int topBegin = (int)(rightPanel.Height / 5.0);

            BunifuSeparator topLine = new BunifuSeparator();
            topLine.LineColor = lineColor;
            topLine.Location = new Point(padding, topBegin);
            topLine.Width = rightPanel.Width - 2 * padding;
            rightPanel.Controls.Add(topLine);

            Label ageLabel = new Label();
            ageLabel.Text = "Возраст";
            ageLabel.Font = boldChatFont;
            ageLabel.AutoSize = true;
            ageLabel.ForeColor = labelColor;
            int bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            ageLabel.Location = new Point(padding, bottomLastControl);

            rightPanel.Controls.Add(ageLabel);


            int dropWidth = (int)(rightPanel.Width / 2.5);

            ComboBox  fromAgeDrop = new ComboBox();
            fromAgeDrop.DropDownStyle = ComboBoxStyle.DropDown;
            fromAgeDrop.Width = dropWidth;
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            fromAgeDrop.Location = new Point(padding, bottomLastControl + padding);
            DropDownComplete(fromAgeDrop, "От", 14, 80);

            rightPanel.Controls.Add(fromAgeDrop);

            Label separatorLabel = new Label();
            separatorLabel.Text = "-";
            separatorLabel.Font = boldChatFont;
            separatorLabel.AutoSize = true;
            separatorLabel.Location = new Point
                (fromAgeDrop.Right + 1, fromAgeDrop.Top);
            rightPanel.Controls.Add(separatorLabel);

            ComboBox toAgeDrop = new ComboBox();
            toAgeDrop.Width = dropWidth;
            toAgeDrop.DropDownStyle = ComboBoxStyle.DropDown;
            toAgeDrop.Location = new Point
                (separatorLabel.Right + 1, fromAgeDrop.Top);
            DropDownComplete(toAgeDrop, "До", 14, 80);

            rightPanel.Controls.Add(toAgeDrop);

            Label genderLabel = new Label();
            genderLabel.Text = "Пол";
            genderLabel.Font = boldChatFont;
            genderLabel.AutoSize = true;
            genderLabel.ForeColor = labelColor;
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            

            rightPanel.Controls.Add(genderLabel);

            genderLabel.Location = new Point(padding,
                bottomLastControl + toAgeDrop.Margin.Bottom + padding);

            genderLabel.Margin = new Padding(3);

            RadioButton female = new RadioButton();
            female.Font = chatFont;
            female.Text = "Женский";
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            female.Location = new Point(padding, bottomLastControl + padding);

            rightPanel.Controls.Add(female);

            RadioButton male = new RadioButton();
            male.Font = chatFont;
            male.Text = "Мужской";
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            male.Location = new Point(padding, bottomLastControl);

            rightPanel.Controls.Add(male);

            RadioButton any = new RadioButton();
            any.Font = chatFont;
            any.Text = "Любой";
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            any.Location = new Point(padding, bottomLastControl);

            rightPanel.Controls.Add(any);

            BunifuSeparator middleLine = new BunifuSeparator();
            middleLine.LineColor = lineColor;
            bottomLastControl = 
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            middleLine.Location = new Point(padding, bottomLastControl);
            middleLine.Width = rightPanel.Width - 2 * padding;
            rightPanel.Controls.Add(middleLine);

            CheckBox fotoCheckBox = new CheckBox();
            fotoCheckBox.Text = "с фотографией";
            fotoCheckBox.Font = chatFont;
            fotoCheckBox.AutoSize = true;
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            fotoCheckBox.Location = new Point(padding, bottomLastControl);
            rightPanel.Controls.Add(fotoCheckBox);

            CheckBox onlineCheckBox = new CheckBox();
            onlineCheckBox.Text = "сейчас на сайте";
            onlineCheckBox.Font = chatFont;
            onlineCheckBox.AutoSize = true;
            bottomLastControl =
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            onlineCheckBox.Location = new Point(padding, bottomLastControl + padding);
            rightPanel.Controls.Add(onlineCheckBox);

            BunifuSeparator bottomLine = new BunifuSeparator();
            bottomLine.LineColor = lineColor;
            bottomLastControl = 
                rightPanel.Controls[rightPanel.Controls.Count - 1].Bottom;
            bottomLine.Location = new Point(padding, bottomLastControl);
            bottomLine.Width = rightPanel.Width - 2 * padding;
            rightPanel.Controls.Add(bottomLine);

            ClientSocketHandler.SendToServer("Select 1");

        }

        public void ShowUsers(string[] people)
        {
            int topPadding = (int)(4.85 * millimetr);
            int beginIndex;

            for (int i = 0; i < people.Length / 6; i++)
            {
                beginIndex = i * 6;

                Human currentUser = Human.Parse(people, beginIndex);

                if ((I.UserId == currentUser.UserId) 
                       || (friend.ContainsKey(currentUser.UserId)))
                {
                    continue;
                }

                Panel usersPanel = new Panel();
                usersPanel.Size = new Size(leftPanel.Width, (int)(25.65 * millimetr));
                Control lastPanel = leftPanel.Controls[leftPanel.Controls.Count - 1];
                usersPanel.Location = new Point(0, lastPanel.Bottom + 1);             


                AvatarBox avatar = new AvatarBox();
                if (currentUser.Avatar == null)
                    avatar.Image = Properties.Resources.AvatarDefault;
                avatar.SizeMode = PictureBoxSizeMode.StretchImage;
                avatar.Size = new Size((int)(19 * millimetr), (int)(19 * millimetr));

                int topAvatar = GetLeftPosition(avatar.Height, usersPanel.Height);
                avatar.Location = new Point(topAvatar, topAvatar);

                usersPanel.Controls.Add(avatar);
                

                Label nameLabel = new Label();
                nameLabel.Text = currentUser.Name + " " + currentUser.Surname;
                nameLabel.Font = boldChatFont;
                nameLabel.ForeColor = Color.FromArgb(42, 88, 133);
                nameLabel.AutoSize = true;
                nameLabel.TextAlign = ContentAlignment.BottomCenter;

                int leftPadding = (int)(3 * millimetr);
                nameLabel.Location = new Point
                    (avatar.Right + leftPadding, (int)(topPadding * 1.25));

                usersPanel.Controls.Add(nameLabel);

                BunifuThinButton2 addButton = new BunifuThinButton2();
                addButton.ButtonText = "Добавить собеседника";
                addButton.Size = new Size
                    ((int)(56.22 * millimetr), (int)(13.5 * millimetr));
                addButton.Font = chatFont;

                addButton.BackColor = Color.White;

                addButton.ActiveForecolor = Color.White;
                addButton.ActiveFillColor = Color.FromArgb(104, 136, 173);
                addButton.ActiveLineColor = Color.White;
                addButton.ActiveCornerRadius = 7;

                addButton.IdleForecolor = Color.White;
                addButton.IdleFillColor = Color.FromArgb(94, 129, 167);
                addButton.IdleLineColor = Color.White;
                addButton.IdleCornerRadius = 7;

                addButton.Location = new Point
                    (usersPanel.Width - addButton.Width - topAvatar, topPadding);
                addButton.Click += AddButton_Click;
                addButton.Tag = currentUser;

                usersPanel.Controls.Add(addButton);

                BunifuSeparator separatorLine = new BunifuSeparator();
                separatorLine.LineColor = lineColor;
                separatorLine.Size = new Size(usersPanel.Width - 2 * topAvatar, 1);
                separatorLine.Location = 
                    new Point(topAvatar, usersPanel.Height - 1);

                usersPanel.Controls.Add(separatorLine);

                AddPanel AddFriendMethod = leftPanel.Controls.Add;
                leftPanel.Invoke(AddFriendMethod, usersPanel);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Human newFriend = (Human)control.Tag;

            Form1.AddFriend = newFriend;
            ClientSocketHandler.SendToServer("AddFriend " + newFriend.UserId);

            Close();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length == 0)
            {
                shadowLabel.Show();
                resetPicture.Hide();
            }
            else
            {
                shadowLabel.Hide();
                resetPicture.Show();
            }
        }

        private void ResetPicture_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
        }

        private void ResetPicture_MouseLeave(object sender, EventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            picture.Image = Properties.Resources.reset;
        }

        private void ResetPicture_MouseEnter(object sender, EventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            picture.Image = Properties.Resources.reset_Active;
        }
    }
}
