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
        protected delegate void AddPanel(Panel panel);

        static Dictionary<int, ChatScreen> openDialog =
            new Dictionary<int, ChatScreen>();

        public static Dictionary<int, ChatScreen> OpenDialog
        {
            get { return openDialog; }
            set { openDialog = value; }
        }

        public static Human AddFriend { get; set; }
        public static AddFriendScreen AddScreen { get; set; }

        protected static Human I { get; set; }
        protected static Dictionary<int, Human> friend = 
            new Dictionary<int, Human>();

        BunifuMetroTextbox loginTextBox;
        BunifuMetroTextbox passwordTextBox;

        protected Panel header { get; set; }

        protected Panel Body { get; set; }
        protected Label TitleLabel { get; set; }
        protected Font chatFont { get; set; }
        protected Font boldChatFont { get; set; }

        ScrollPanel scrollPanel;
        Panel bottomPanel;


        Color blueColor, blueActiveColor, bodyColor, grayColor;

        int bodyPadding = 50;
        int heighPadding = 5;

        Size textBoxSize = new Size(170, 32);
        int textBoxBorder = 1;
        Padding textBoxPadding = new Padding(5);

        public static Human GetFriendAtId(int id)
        {
            return friend[id];
        }

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

            if (!((this is ChatScreen) || (this is AddFriendScreen)))
                ShowLoginChat();           
        }

        protected int GetLeftPosition(int controlWidth, int parentWidth)
        {
            return parentWidth / 2 - controlWidth / 2;
        }

        void ClearBody()
        {
            while (Body.Controls.Count > 0)
            {
                Body.Controls.RemoveAt(0);
            }
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

            Padding = new Padding(0);
        }

       protected void DropDownComplete
            (ComboBox dropdown, string defaultValue, int minValue, int maxValue)
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

        #region Login
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

        private void RegestrationButton_Click(object sender, EventArgs e)
        {
            ClearBody();
            ShowRegestrationChat();
        }


        bool visibleErrorLogin = false;
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            ClientSocketHandler.SendToServer("Login " + login + " " + password);
            string[] answerFromServer;
            answerFromServer =
                ClientSocketHandler.WaitReciveFromServer().Split();


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
        #endregion

        #region Conversation

        Label shadowLabel;
        void BottomPanelShow()
        {
            double ratio = 128 / 14.0;

            TitleLabel.Text = "10 друзей онлайн";

            bottomPanel = new Panel();
            bottomPanel.BackColor = Color.White;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = (int)(Body.Height / ratio);

            Body.Controls.Add(bottomPanel);
            int avatarSize = (int)(bottomPanel.Height / 1.5);

            shadowLabel = new Label();
            shadowLabel.Text = "Начните вводить имя..";
            shadowLabel.ForeColor = Color.FromArgb(146, 158, 176);
            shadowLabel.Font = chatFont;
            shadowLabel.AutoSize = true;
            shadowLabel.Enabled = false;

            bottomPanel.Controls.Add(shadowLabel);

            TextBox textBox = new TextBox();
            textBox.Font = chatFont;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.BorderStyle = BorderStyle.None;


            int imageSize = (int)(bottomPanel.Height / 2.0);
            int imagePadding = (int)(imageSize / 2.0);

            PictureBox plusPicture = new PictureBox();
            plusPicture.Size = new Size(imageSize, imageSize);
            plusPicture.Location = new Point(imagePadding, imagePadding);
            plusPicture.Image = Properties.Resources.addFriend;
            plusPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            plusPicture.BringToFront();
            plusPicture.Click += PlusPicture_Click;

            bottomPanel.Controls.Add(plusPicture);

            int circleSize = (int)(imageSize / 2.6);

            PictureBox circlePicture = new PictureBox();
            circlePicture.Size = new Size(circleSize, circleSize);

            int topCircle = GetLeftPosition(circlePicture.Height, bottomPanel.Height);
            circlePicture.Location = new Point
                (bottomPanel.Width - imagePadding - circleSize, topCircle);
            circlePicture.BringToFront();

            circlePicture.Paint += CirclePicture_Paint;
            circlePicture.Click += CirclePicture_Click;

            bottomPanel.Controls.Add(circlePicture);
            bottomPanel.Controls.Add(textBox);

            Point textBoxLocation = new Point(imageSize + 2 * imagePadding,
                (int)(bottomPanel.Height / 3.0));
            textBox.Location = textBoxLocation;
            textBox.Size = new Size
                (bottomPanel.Width - 2 * imageSize - 4 * imagePadding,
                (int)(bottomPanel.Height / 3.0));

            shadowLabel.Location = new Point(textBoxLocation.X + 1,
                textBoxLocation.Y);
        }

        private static void PlusPicture_Click(object sender, EventArgs e)
        {
            AddScreen = new AddFriendScreen();
            AddScreen.ShowDialog();
        }

        private void CirclePicture_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            if (backColor == offlineColor)
            {
                backColor = onlineColor;
            }
            else
            {
                backColor = offlineColor;
            }

            control.Invalidate();
            control.Update();
        }

        Color offlineColor = Color.FromArgb(185, 218, 173);
        Color onlineColor = Color.FromArgb(138, 193, 118);
        Color backColor = Color.FromArgb(185, 218, 173);
        private void CirclePicture_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            
            SolidBrush brush = new SolidBrush(backColor);

            Graphics g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.FillEllipse(brush, e.ClipRectangle);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.TextLength == 0) shadowLabel.Show();
            else shadowLabel.Hide();
        }

        void ShowListOfConversation(int countConversation, string[] answer)
        {          
            int beginIndex = 2;
            BottomPanelShow();

            I = Human.Parse(answer, beginIndex);
            // if (answer.Length > 5) I.Avatar = answer[5];

            scrollPanel = new ScrollPanel();
            scrollPanel.Size = new Size(Width - scrollPanel.scrollPadding,
                Height - header.Height - bottomPanel.Height);
            scrollPanel.Location = new Point(0, header.Bottom + 1);

            Body.Controls.Add(scrollPanel);

            beginIndex = 8;
            for (int i = 0; i < countConversation; i++)
            {
                Human currentFriend = new Human();               
           
                currentFriend = Human.Parse(answer, beginIndex);
                int numberOfUnread = int.Parse(answer[beginIndex + 6]);

                ShowConversationPanel(currentFriend);
                beginIndex += 7;
            }
            scrollPanel.ContentAdded = false;
            
            ClientSocketHandler.BeginRecive();
        }

        public void ShowConversationPanel(Human currentFriend)
        {
            
            double ratio = 13 / 8.0;           

            friend[currentFriend.UserId] = currentFriend;

            int topPanel;
            if (scrollPanel.Controls.Count == 0) topPanel = 0;
            else topPanel = scrollPanel.Controls[scrollPanel.Controls.Count - 1].Bottom;

            Panel conversationPanel = new Panel();
            conversationPanel.Tag = currentFriend.UserId;
            conversationPanel.Size =
                new Size(scrollPanel.Width, (int)(header.Height * ratio));
            conversationPanel.Location =
                new Point(0, topPanel);
            conversationPanel.BackColor = Color.White;
    
        
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

            chatImage.Location = new Point(paddingLeft, topChatImage);

            conversationPanel.Controls.Add(chatImage);

            conversationPanel.MouseEnter += ConversationPanel_MouseEnter;
            conversationPanel.MouseLeave += ConversationPanel_MouseLeave;

            conversationPanel.Click += ConversationPanel_Click;
            completeName.Click += ConversationPanel_Click;


            AddPanel addConversation = scrollPanel.Controls.Add;
            Invoke(addConversation, conversationPanel);
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
             
            ClientSocketHandler.SendToServer("Read " + id1 + " " + id2 + " 0");         
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

        #endregion

        #region Regestration

        BunifuMetroTextbox lastnameTextBox;
        BunifuMetroTextbox firstnameTextBox;
        ComboBox dayDrop, monthDrop, yearDrop;
        RadioButton male;

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

            lastnameTextBox = new BunifuMetroTextbox();
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


            firstnameTextBox = new BunifuMetroTextbox();
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

            dayDrop = new ComboBox();
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


            monthDrop = new ComboBox();
            monthDrop.DropDownStyle = ComboBoxStyle.DropDown;
            monthDrop.Location = new Point
                (Body.Controls[Body.Controls.Count - 1].Right + (ratio),
                Body.Controls[Body.Controls.Count - 1].Top);
            DropDownComplete(monthDrop, "Месяц", 0, 11);

            Body.Controls.Add(monthDrop);
            monthDrop.Width = 8 * ratio;
            monthDrop.DropDownHeight = dropDownHeight;

            yearDrop = new ComboBox();
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

            male = new RadioButton();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            I = new Human();

            I.UserId = -1;
            I.Name = firstnameTextBox.Text;
            I.Surname = lastnameTextBox.Text;

            I.Brithday = new DateTime(
                (int)yearDrop.SelectedItem, 
                monthDrop.SelectedIndex, 
                dayDrop.SelectedIndex);

            I.Gender = (male.Checked) ? "male" : "female";

            ClearBody();
            ValidRegestrationShow();
        }

        BunifuMetroTextbox loginTextbox;
        BunifuMetroTextbox passwordTextbox;

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


            loginTextbox = new BunifuMetroTextbox();
            loginTextbox.Font = chatFont;
            loginTextbox.Padding = textBoxPadding;
            loginTextbox.BorderThickness = textBoxBorder;
            loginTextbox.BackColor = Color.White;
            loginTextbox.ForeColor = grayColor;
            loginTextbox.BorderColorFocused = blueColor;
            loginTextbox.BorderColorMouseHover = blueColor;
            loginTextbox.Size = new Size(widthControl, heightControl);
            loginTextbox.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom + heighPadding);

            whitePanel.Controls.Add(loginTextbox);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Пароль";
            passwordLabel.Font = chatFont;
            passwordLabel.AutoSize = true;

            whitePanel.Controls.Add(passwordLabel);
            passwordLabel.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 2].Bottom + heighPadding);

            passwordTextbox = new BunifuMetroTextbox();
            passwordTextbox.Font = chatFont;
            passwordTextbox.Padding = textBoxPadding;
            passwordTextbox.BorderThickness = textBoxBorder;
            passwordTextbox.BackColor = Color.White;
            passwordTextbox.ForeColor = grayColor;
            passwordTextbox.BorderColorFocused = blueColor;
            passwordTextbox.BorderColorMouseHover = blueColor;
            passwordTextbox.isPassword = true;
            passwordTextbox.Size = new Size(widthControl, heightControl);
            passwordTextbox.Location = new Point(
                leftPosition,
                whitePanel.Controls[whitePanel.Controls.Count - 1].Bottom + heighPadding);

            whitePanel.Controls.Add(passwordTextbox);


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
            string login = loginTextbox.Text;
            string password = passwordTextbox.Text;

            ClientSocketHandler.SendToServer
                ("Regestration " + login + " " + password + " " + I.ToString());
            string answer = ClientSocketHandler.WaitReciveFromServer();
            string[] parametr = answer.Split();

            if (parametr[0] == "Ok")
            {
                ClearBody();
                I.UserId = int.Parse(parametr[1]);
                Sizes();                
                Body.Padding = new Padding(0);
                Body.BackColor = bodyColor;
                BottomPanelShow();
            }
            else
            {
                loginTextbox.Text = "";
                passwordTextbox.Text = "";
            }
        }
        #endregion
    }
}
