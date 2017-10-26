using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu;
using Bunifu.Framework.UI;
using WindowsFormsControlLibrary1;

namespace DanilaChat
{
    public partial class Form1 : Form
    {
        Dictionary<int, ChatScreen> OpenDialog = 
            new Dictionary<int, ChatScreen>();

        Human I;

        ClientSocketHandler ClientSocket;

        BunifuMetroTextbox loginTextBox;
        BunifuMetroTextbox passwordTextBox;

        Panel header;

        protected Panel Body { get; set; }
        protected Label TitleLabel { get; set; }
        protected Font chatFont { get; set; }

        Font  boldChatFont;
        Color blueColor, blueActiveColor, bodyColor, grayColor;

        int bodyPadding = 50;
        int heighPadding = 5;

        Size textBoxSize = new Size(170, 32);
        int textBoxBorder = 1;
        Padding textBoxPadding = new Padding(5);

        public Form1()
        {
            InitializeComponent();

            CreateGraphics().CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            blueColor = Color.FromArgb(80, 114, 153);
            blueActiveColor = Color.FromArgb(104, 136, 173);
            bodyColor = Color.FromArgb(237, 238, 240);
            grayColor = Color.FromArgb(101, 101, 101);

            StartPosition = FormStartPosition.CenterScreen;
            Sizes();

            chatFont = new Font("Tahoma", 9);
            boldChatFont = new Font("Tahoma", 9, FontStyle.Bold);

            header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = (int)((double)Height / 12.0);
            header.BackColor = blueColor;
            header.MouseEnter += head_MouseEnter;
            Controls.Add(header);

            Body = new Panel();
            Body.Dock = DockStyle.Fill;
            Body.BackColor = bodyColor;
            Controls.Add(Body); 

            TitleLabel = new Label();
            TitleLabel.Anchor = AnchorStyles.Left;
            TitleLabel.Padding = new Padding(8);
            TitleLabel.Text = "Добро пожаловать в DanilaChat !";
            TitleLabel.Font = boldChatFont;
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.ForeColor = Color.White;
            TitleLabel.AutoSize = true;
            TitleLabel.MouseEnter += head_MouseEnter;

            header.Controls.Add(TitleLabel);

            BunifuImageButton closeButton = new BunifuImageButton();
            closeButton.Anchor = AnchorStyles.Right;
            closeButton.BackColor = Color.Transparent;
            closeButton.Size = new Size(header.Height-16, header.Height-16);
            closeButton.Location = new Point(header.Width - closeButton.Width-10,7);
            closeButton.Image = Properties.Resources.vk_close;
            closeButton.ImageActive = Properties.Resources.vk_close_active;
            closeButton.Zoom = 0;
            closeButton.Click += CloseButton_Click;

            header.Controls.Add(closeButton);

            if (!(this is ChatScreen)) ShowLoginChat();           
        }

        private void head_MouseEnter(object sender, EventArgs e)
        {
            BunifuDragControl drag = new BunifuDragControl();
            drag.TargetControl = (Control)sender;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        void Sizes()
        {
            int y = Screen.PrimaryScreen.WorkingArea.Height;
            int x = Screen.PrimaryScreen.WorkingArea.Width;

            Width = x / 5;
            Height = (int)((double)y / 1.8);
        }


        void ShowLoginChat()
        {
            Size buttonSize = new Size(240, 68);
            
            Label loginLabel = new Label();
            loginLabel.Text = "Телефон или e-mail";
            loginLabel.AutoSize = true;
            loginLabel.Font = boldChatFont;
            loginLabel.ForeColor = grayColor;
            loginLabel.Location = new Point(bodyPadding, bodyPadding);
            Body.Controls.Add(loginLabel);

            loginTextBox = new BunifuMetroTextbox();
            loginTextBox.Font = chatFont;
            loginTextBox.Padding = textBoxPadding;
            loginTextBox.BorderThickness = textBoxBorder;
            loginTextBox.BorderColorFocused = blueColor;
            loginTextBox.BorderColorMouseHover = blueColor;
            loginTextBox.Size = textBoxSize;
            loginTextBox.Location = new Point
                (bodyPadding, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(loginTextBox);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Пароль";
            passwordLabel.Font = boldChatFont;
            passwordLabel.ForeColor = grayColor;
            passwordLabel.Location = new Point
                (bodyPadding, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(passwordLabel);

            passwordTextBox = new BunifuMetroTextbox();
            passwordTextBox.Font = chatFont;
            passwordTextBox.Padding = textBoxPadding;
            passwordTextBox.BorderThickness = textBoxBorder;
            passwordTextBox.BorderColorFocused = blueColor;
            passwordTextBox.BorderColorMouseHover = blueColor;
            passwordTextBox.Size = textBoxSize;
            passwordTextBox.isPassword = true;
            passwordTextBox.Location = new Point
                (bodyPadding, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(passwordTextBox);

            BunifuThinButton2 loginButton = new BunifuThinButton2();
            loginButton.ButtonText = "Войти";
            loginButton.Size = buttonSize;
            loginButton.Font = boldChatFont;

            loginButton.BackColor = bodyColor;

            loginButton.ActiveForecolor = Color.White;
            loginButton.ActiveFillColor = blueActiveColor;
            loginButton.ActiveLineColor = bodyColor;
            loginButton.ActiveCornerRadius = 7;

            loginButton.IdleForecolor = Color.White;
            loginButton.IdleFillColor = blueColor;
            loginButton.IdleLineColor = bodyColor;
            loginButton.IdleCornerRadius = 7;

            loginButton.Location = new Point
                (bodyPadding, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(loginButton);

            loginButton.Click += LoginButton_Click;

            BunifuThinButton2 regestrationButton = new BunifuThinButton2();
            regestrationButton.ButtonText = "Регистрация";
            regestrationButton.Size = buttonSize;
            regestrationButton.Font = boldChatFont;

            regestrationButton.BackColor = bodyColor;

            regestrationButton.ActiveForecolor = Color.White;
            regestrationButton.ActiveFillColor = blueActiveColor;
            regestrationButton.ActiveLineColor = bodyColor;
            regestrationButton.ActiveCornerRadius = 7;

            regestrationButton.IdleForecolor = Color.White;
            regestrationButton.IdleFillColor = blueColor;
            regestrationButton.IdleLineColor = bodyColor;
            regestrationButton.IdleCornerRadius = 7;

            regestrationButton.Location = new Point
                (bodyPadding, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(regestrationButton);

            regestrationButton.Click += RegestrationButton_Click;

        }


        bool visibleErrorLogin = false;
        private void LoginButton_Click(object sender, EventArgs e)
        {
            ClientSocket = new ClientSocketHandler();
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            ClientSocket.SendToServer("Login " + login + " " + password);
            List<string> answerFromServer = new List<string>();
            answerFromServer.AddRange(ClientSocket.WaitReciveFromServer().Split());

            //MessageBox.Show(answerFromServer[0]);
            //MessageBox.Show(answerFromServer[1]);

            if (answerFromServer[0] == "Ok")
            {
                visibleErrorLogin = false;
                ClearBody();


                ShowListOfConversation(int.Parse(answerFromServer[1]), 
                  answerFromServer);
                return;
            }
            else
            if (visibleErrorLogin) return;

            visibleErrorLogin = true;
            ShowErrorLogin();            
        }

        void ShowErrorLogin()
        {
            Label MessageLabel = new Label();
            MessageLabel.AutoSize = true;
            MessageLabel.Text = "Не удается войти.\n";
            MessageLabel.Font = boldChatFont;
            MessageLabel.ForeColor = Color.Black;

            Label MessageLabel2 = new Label();
            MessageLabel2.AutoSize = true;
            MessageLabel2.Text =
                "Пожалуйста, проверьте правильность\nнаписания логина и пароля.";
            MessageLabel2.Font = chatFont;
            MessageLabel2.ForeColor = Color.Black;

            Panel BorderPanel = new Panel();
            BorderPanel.BackColor = Color.FromArgb(242, 171, 153);
            BorderPanel.Size = textBoxSize;
            BorderPanel.Width = (int)(BorderPanel.Width * 1.35);
            BorderPanel.Height = (int)(BorderPanel.Height * 2);
            int panelLeft = GetLeftPosition(BorderPanel.Width, Body.Width);

            BorderPanel.Location = new Point
                (panelLeft, Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(BorderPanel);

            Panel MessagePanel = new Panel();
            MessagePanel.BackColor = Color.FromArgb(255, 239, 233);
            MessagePanel.Dock = DockStyle.Fill;
            MessagePanel.Controls.Add(MessageLabel);
            MessagePanel.Controls.Add(MessageLabel2);
            MessageLabel2.Location = new Point(MessageLabel.Left, MessageLabel.Bottom + heighPadding);

            BorderPanel.Padding = new Padding(1);
            BorderPanel.Controls.Add(MessagePanel);
        }

        Dictionary<int, Human> friend = new Dictionary<int, Human>();

        void ShowListOfConversation(int countConversation, List<string> answer)
        {            
            TitleLabel.Text = "10 друзей онлайн";
            answer.RemoveAt(0);
            answer.RemoveAt(0);

            I = new Human();
            I.UserId = int.Parse(answer[0]);
            I.Name = answer[1];
            I.Surname = answer[2];
            I.Brithday = DateTime.Parse(answer[3] + " " + answer[4]);
            I.Gender = answer[5];
           // if (answer.Length > 5) I.Avatar = answer[5];

            double ratio = 13 / 8.0; 

            for (int i = 0; i < countConversation; i++)
            {
                int beginIndex = 6 + (i * 7);
                //answerFromServer = ClientSocket.WaitReciveFromServer();
                //answer = answerFromServer.Split();
                Human currentFriend = new Human();

                currentFriend.UserId = int.Parse(answer[beginIndex]);
                currentFriend.Name = answer[beginIndex + 1];
                currentFriend.Surname = answer[beginIndex + 2];
                currentFriend.Brithday = DateTime.Parse(answer[beginIndex + 3] 
                    + " " + answer[beginIndex + 4]);
                currentFriend.Gender = answer[beginIndex + 5];
                //if (answer.Length > 5) currentFriend.Avatar = answer[5];
                int numberOfUnread = int.Parse(answer[beginIndex + 6]);

                friend[currentFriend.UserId] = currentFriend;

                //body.BackColor = Color.White;

                int topPanel;
                if (Body.Controls.Count == 0) topPanel = header.Bottom;
                else topPanel = Body.Controls[Body.Controls.Count - 1].Bottom;

                Panel conversationPanel = new Panel();
                conversationPanel.Tag = currentFriend.UserId;
                conversationPanel.Size = 
                    new Size(Body.Width, (int)(header.Height * ratio));
                conversationPanel.Location =
                    new Point(0, topPanel);
                conversationPanel.BackColor = Color.White;                

                Body.Controls.Add(conversationPanel);


                AvatarBox avatarImage = new AvatarBox();
                avatarImage.Image = Properties.Resources.AvatarDefault;
                avatarImage.SizeMode = PictureBoxSizeMode.StretchImage;

                int length = (int)(conversationPanel.Height / ratio);
                avatarImage.Size = new Size(length, length);

                int padding = (int)(conversationPanel.Height - avatarImage.Height) / 2;
                avatarImage.Location = new Point(padding, padding);
                avatarImage.Enabled = false;

                conversationPanel.Controls.Add(avatarImage);

                Label completeName = new Label();
                completeName.Text = currentFriend.Name + " " + currentFriend.Surname;
                completeName.Font = boldChatFont;
                completeName.TextAlign = ContentAlignment.BottomCenter;
                completeName.AutoSize = true;
                completeName.ForeColor = Color.FromArgb(66, 100, 139);

                int top = 
                    GetLeftPosition(completeName.Height, conversationPanel.Height);
                top = (int)(top * 1.2);
                int left = avatarImage.Right + padding;
                completeName.Location = new Point(left, top);

                conversationPanel.Controls.Add(completeName);                           

                PictureBox chatImage = new PictureBox();
                chatImage.Image = Properties.Resources.chatImage;
                chatImage.SizeMode = PictureBoxSizeMode.StretchImage;
                chatImage.Visible = false;
                chatImage.Enabled = false;

                int sizeChatImage = (int)(3 * length / 4.0);
                chatImage.Size = new Size(sizeChatImage, sizeChatImage);

                int topChatImage =
                    GetLeftPosition(chatImage.Height, conversationPanel.Height);
                int paddingRight = sizeChatImage / 2;
                int paddingLeft = conversationPanel.Width 
                    - paddingRight - chatImage.Width;

                chatImage.Location = new Point(paddingLeft,topChatImage);

                conversationPanel.Controls.Add(chatImage);

                conversationPanel.MouseEnter += ConversationPanel_MouseEnter;
                conversationPanel.MouseLeave += ConversationPanel_MouseLeave;

                conversationPanel.Click += ConversationPanel_Click;
                completeName.Click += ConversationPanel_Click;
            }
        }

        private void ConversationPanel_Click(object sender, EventArgs e)
        {
            Control panel;

            if (sender is Label)
            {
                panel = ((Control)sender).Parent;
            }
            else
            {
                panel = (Control)sender;
            }

            OpenConversation((int)panel.Tag);
        }

        void OpenConversation(int number)
        {
            int id1 = I.UserId;
            int id2 = friend[number].UserId;

            if (id1 > id2)
            {
                int temp = id1;
                id1 = id2;
                id2 = temp;
            }
             
            ClientSocket.SendToServer("Read " + id1 + " " + id2);

            string answerFromServer = ClientSocket.WaitReciveFromServer();
            string[] answer = answerFromServer.Split();

            ChatScreen chatScreen = new ChatScreen(answer, I, friend[number]);
            chatScreen.Show();
            OpenDialog[number] = chatScreen;
        }

        SolidBrush brush = new SolidBrush(Color.White);
        private void ConversationPanel_MouseLeave(object sender, EventArgs e)
        {

            Control panel = (Control)sender;
            Point position = panel.PointToClient(MousePosition);
            if (!(position.X <= 2
                || position.Y <= 2
                || position.X >= panel.Width - 2
                || position.Y >= panel.Height - 2))
                return;

            panel.BackColor = Color.White;
            panel.Controls[2].Visible = false;

            brush.Color = Color.White;
            panel.Controls[0].Invalidate();
        }

        private void ConversationPanel_MouseEnter(object sender, EventArgs e)
        {
            /*
            Control panel = (Control)sender;
            Point position = panel.PointToClient(MousePosition);
            if (!(position.X <= 6
                || position.Y <= 6
                || position.X >= panel.Width - 6
                || position.Y >= panel.Height - 6))
                return;*/
            Control panel = (Control)sender;
            panel.BackColor = Color.FromArgb(240, 242, 245);
            panel.Controls[2].Visible = true;

            brush.Color = Color.FromArgb(240, 242, 245);
            panel.Controls[0].Invalidate();
        }

        private void RegestrationButton_Click(object sender, EventArgs e)
        {
            ClearBody();
            ShowRegestrationChat();
        }

        void ClearBody()
        {
            while (Body.Controls.Count > 0)
            {
                Body.Controls[0].Dispose();
                Body.Controls.RemoveAt(0);
            }
        }

        void ShowRegestrationChat()
        {
            Width *= 2;
            Body.BackColor = Color.FromArgb(247, 248, 250);
            TitleLabel.Text = "Моментальная регистрация";

            Label headerLabel = new Label();
            headerLabel.Text = "Пожалуйста, укажите Ваше имя и фамилию.";
            headerLabel.Dock = DockStyle.None;
            headerLabel.TextAlign = ContentAlignment.MiddleCenter;
            headerLabel.Font = boldChatFont;
            headerLabel.AutoSize = true;                

            Body.Controls.Add(headerLabel);
            int headerLeft = GetLeftPosition(headerLabel.Width, Body.Width);
            headerLabel.Location = new Point(headerLeft, bodyPadding);
        

            Label noteLabel = new Label();
            noteLabel.Text =
                "Чтобы облегчить поиск и общение друзей,у нас приняты настоящие \nимена и фамилии.";
            noteLabel.Dock = DockStyle.None;
            noteLabel.TextAlign = ContentAlignment.MiddleCenter;
            noteLabel.Font = chatFont;
            noteLabel.AutoSize = true;

            Body.Controls.Add(noteLabel);

            int noteLeft = GetLeftPosition(noteLabel.Width, Body.Width);
            noteLabel.Location = new Point(
                noteLeft,
                Body.Controls[Body.Controls.Count - 2].Bottom + heighPadding);

            Size sizeRegestrationTextBox = new Size(271, 32);

            BunifuMetroTextbox lastnameTextBox = new BunifuMetroTextbox();
            lastnameTextBox.Font = chatFont;
            lastnameTextBox.Padding = textBoxPadding;
            lastnameTextBox.BorderThickness = textBoxBorder;
            lastnameTextBox.BackColor = Color.White;
            lastnameTextBox.ForeColor = Color.Black;
            lastnameTextBox.BorderColorFocused = blueColor;
            lastnameTextBox.BorderColorMouseHover = blueColor;
            lastnameTextBox.Size = sizeRegestrationTextBox;
            int leftPosition = GetLeftPosition(lastnameTextBox.Width, Body.Width);
            lastnameTextBox.Location = new Point(
                leftPosition,
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);

            Body.Controls.Add(lastnameTextBox);


            BunifuMetroTextbox firstnameTextBox = new BunifuMetroTextbox();
            firstnameTextBox.Font = chatFont;
            firstnameTextBox.Padding = textBoxPadding;
            firstnameTextBox.BorderThickness = textBoxBorder;
            firstnameTextBox.BackColor = Color.White;
            firstnameTextBox.ForeColor = Color.Black;
            firstnameTextBox.BorderColorFocused = blueColor;
            firstnameTextBox.BorderColorMouseHover = blueColor;
            firstnameTextBox.Size = sizeRegestrationTextBox;
            firstnameTextBox.Location = new Point(
                leftPosition, 
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);

            Body.Controls.Add(firstnameTextBox);

            Label brithdayLabel = new Label();
            brithdayLabel.Text = "Дата рождения";
            brithdayLabel.Font = chatFont;
            brithdayLabel.ForeColor = grayColor;
            brithdayLabel.Location = new Point(
                leftPosition, 
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);

            Body.Controls.Add(brithdayLabel);

            int dropPadding = 20;

            int ratio = (int)((double)firstnameTextBox.Width / 22.0);

            ComboBox dayDrop = new ComboBox();
            dayDrop.DropDownStyle = ComboBoxStyle.DropDown;
            
            dayDrop.Location = new Point(
                leftPosition, 
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            DropDownComplete(dayDrop, "День", 1, 31);

            Body.Controls.Add(dayDrop);
            dayDrop.Width = 6 * ratio;

            double ratioDropDown =2.376;
            int dropDownHeight = (int)((double)Body.Height / ratioDropDown);
            dayDrop.DropDownHeight = dropDownHeight;


            ComboBox monthDrop = new ComboBox();
            monthDrop.DropDownStyle = ComboBoxStyle.DropDown;
            monthDrop.Location = new Point
                (Body.Controls[Body.Controls.Count - 1].Right + (ratio),
                Body.Controls[Body.Controls.Count - 1].Top);
            DropDownComplete(monthDrop, "Месяц", 0, 11);

            Body.Controls.Add(monthDrop);
            monthDrop.Width = 8 * ratio;
            monthDrop.DropDownHeight = dropDownHeight;

            ComboBox yearDrop = new ComboBox();
            yearDrop.DropDownStyle = ComboBoxStyle.DropDown;
            yearDrop.Location = new Point
                (Body.Controls[Body.Controls.Count - 1].Right + (ratio),
                Body.Controls[Body.Controls.Count - 1].Top);
            DropDownComplete(yearDrop, "Год", 1940, 2017);

            Body.Controls.Add(yearDrop);
            yearDrop.Width = 6 * ratio;
            yearDrop.DropDownHeight = dropDownHeight;

            Label genderLabel = new Label();
            genderLabel.Text = "Ваш пол";
            genderLabel.Font = chatFont;
            genderLabel.ForeColor = grayColor;
            genderLabel.Location = new Point(
                leftPosition, 
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);

            Body.Controls.Add(genderLabel);

            int radioPadding = 20;

            RadioButton male = new RadioButton();
            male.Font = boldChatFont;
            male.Text = "Мужской";
            male.Location = new Point(
                leftPosition,
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);

            Body.Controls.Add(male);

            RadioButton female = new RadioButton();
            female.Font = boldChatFont;
            female.Text = "Женский";
            female.Location = new Point
                (Body.Controls[Body.Controls.Count - 1].Right + radioPadding,
                Body.Controls[Body.Controls.Count - 1].Top);

            Body.Controls.Add(female);

            Color greenColor = Color.FromArgb(95, 176, 83);
            Color activeGreenColor = Color.FromArgb(119, 198, 103);

            BunifuThinButton2 continueButton = new BunifuThinButton2();
            continueButton.ButtonText = "Зарегистрироваться";
            continueButton.Font = boldChatFont;

            continueButton.BackColor = bodyColor;

            continueButton.ActiveForecolor = Color.White;
            continueButton.ActiveFillColor = activeGreenColor;
            continueButton.ActiveLineColor = bodyColor;
            continueButton.ActiveCornerRadius = 7;

            continueButton.IdleForecolor = Color.White;
            continueButton.IdleFillColor = greenColor;
            continueButton.IdleLineColor = bodyColor;
            continueButton.IdleCornerRadius = 7;

            continueButton.Location = new Point(
                leftPosition, 
                Body.Controls[Body.Controls.Count - 1].Bottom + heighPadding);
            Body.Controls.Add(continueButton);
            continueButton.Size = new Size(268, 45);

            continueButton.Click += ContinueButton_Click;
        }

        int GetLeftPosition(int controlWidth, int parentWidth)
        {
            return parentWidth / 2 - controlWidth / 2;
        }

        void DropDownComplete(ComboBox dropdown, string defaultValue, int minValue, int maxValue)
        {
            dropdown.Items.Add(defaultValue);
            dropdown.SelectedIndex = 0;

            for (int i = minValue; i <= maxValue; i++)
            {
                if (maxValue == 11)
                {
                    MonthName monthName = (MonthName)i;
                    dropdown.Items.Add(monthName);
                }
                else
                    dropdown.Items.Add(i);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            ClearBody();
            ValidRegestrationShow();
        }

        void ValidRegestrationShow()
        {
            Height = (int)(Height * 1.4);

            int padding = (int)((double)Body.Width / 18.2);

            Body.Padding = new Padding(padding, padding + header.Height, padding, padding); 
            Panel whitePanel = new Panel();
            whitePanel.Padding = new Padding(0);
            Body.Controls.Add(whitePanel);
            whitePanel.Dock = DockStyle.Fill;
            whitePanel.BackColor = Color.White;


            Label headerLabel = new Label();
            headerLabel.Text = "Потверждение регистрации";
            headerLabel.TextAlign = ContentAlignment.MiddleCenter;
            headerLabel.Font = new Font("Tahoma", 12, FontStyle.Bold);
            headerLabel.AutoSize = true;

            whitePanel.Controls.Add(headerLabel);
            int headerLeft = GetLeftPosition(headerLabel.Width, whitePanel.Width);
            headerLabel.Location = new Point(headerLeft, bodyPadding);

            Label noteLabel = new Label();
            noteLabel.Text =
                "Придумайте логин и пароль для входа в чат.";
            noteLabel.ForeColor = grayColor;
            noteLabel.TextAlign = ContentAlignment.MiddleCenter;
            noteLabel.Font = chatFont;
            noteLabel.AutoSize = true;

            whitePanel.Controls.Add(noteLabel);

            int noteLeft = GetLeftPosition(noteLabel.Width, whitePanel.Width);
            noteLabel.Location = new Point(
                noteLeft,
                whitePanel.Controls[whitePanel.Controls.Count - 2].Bottom + heighPadding);

            double ratioWidth = 3;
            int widthControl = (int)((double)Body.Width / ratioWidth);

            double ratioHeight = 16.8;
            int heightControl = (int)((double)Body.Height / ratioHeight);

            //double ratioHight = 

            int leftPosition = 
                GetLeftPosition(widthControl, whitePanel.Width);

            Label loginLabel = new Label();
            loginLabel.Text = "Логин";
            loginLabel.Font = chatFont;
            loginLabel.AutoSize = true;

            whitePanel.Controls.Add(loginLabel);
            loginLabel.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 2].Bottom + heighPadding);


            BunifuMetroTextbox loginTextBox = new BunifuMetroTextbox();
            loginTextBox.Font = chatFont;
            loginTextBox.Padding = textBoxPadding;
            loginTextBox.BorderThickness = textBoxBorder;
            loginTextBox.BackColor = Color.White;
            loginTextBox.ForeColor = grayColor;
            loginTextBox.BorderColorFocused = blueColor;
            loginTextBox.BorderColorMouseHover = blueColor;
            loginTextBox.Size = new Size(widthControl, heightControl);
            loginTextBox.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom + heighPadding);

            whitePanel.Controls.Add(loginTextBox);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Пароль";
            passwordLabel.Font = chatFont;
            passwordLabel.AutoSize = true;

            whitePanel.Controls.Add(passwordLabel);
            passwordLabel.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 2].Bottom + heighPadding);

            BunifuMetroTextbox passwordTextBox = new BunifuMetroTextbox();
            passwordTextBox.Font = chatFont;
            passwordTextBox.Padding = textBoxPadding;
            passwordTextBox.BorderThickness = textBoxBorder;
            passwordTextBox.BackColor = Color.White;
            passwordTextBox.ForeColor = grayColor;
            passwordTextBox.BorderColorFocused = blueColor;
            passwordTextBox.BorderColorMouseHover = blueColor;
            passwordTextBox.isPassword = true;
            passwordTextBox.Size = new Size(widthControl, heightControl);
            passwordTextBox.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom + heighPadding);

            whitePanel.Controls.Add(passwordTextBox);


            Label validLengthLabel = new Label();
            validLengthLabel.Text = "Не менее 6 символов в длину";
            validLengthLabel.Font = chatFont;
            validLengthLabel.ForeColor = grayColor;
            validLengthLabel.AutoSize = true;

            whitePanel.Controls.Add(validLengthLabel);
            validLengthLabel.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 2].Bottom + heighPadding);

            int buttonHeight = (int)((double)heightControl * 1.5);

            BunifuThinButton2 continueButton = new BunifuThinButton2();
            continueButton.ButtonText = "Войти в чат";
            continueButton.Font = boldChatFont;

            continueButton.BackColor = Color.White;

            continueButton.ActiveForecolor = Color.White;
            continueButton.ActiveFillColor = blueActiveColor;
            continueButton.ActiveLineColor = Color.White;
            continueButton.ActiveCornerRadius = 7;

            continueButton.IdleForecolor = Color.White;
            continueButton.IdleFillColor = blueColor;
            continueButton.IdleLineColor = Color.White;
            continueButton.IdleCornerRadius = 7;

            continueButton.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom + heighPadding);
            whitePanel.Controls.Add(continueButton);
            continueButton.Size = new Size(widthControl, buttonHeight);

            continueButton.Click += CompleteButton_Click;

            int pictureTop = 
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom 
                + heighPadding;

            int remainingHeight = 
                whitePanel.Bottom - Body.Padding.Top - pictureTop;
            int leftImage = GetLeftPosition(remainingHeight, whitePanel.Width);

            PictureBox picture = new PictureBox();
            picture.Size = new Size(remainingHeight, remainingHeight);
            picture.Image = Properties.Resources.image;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.Location = new Point(
                leftImage,
                pictureTop);
            whitePanel.Controls.Add(picture);
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            ClearBody();
        }
    }
}
