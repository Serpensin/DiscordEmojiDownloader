namespace DiscordEmojiDownloader
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelTopbar = new Guna.UI2.WinForms.Guna2Panel();
            LabelTopBarTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            PictureTopBarIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            ButtonMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            ButtonClose = new Guna.UI2.WinForms.Guna2ControlBox();
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            mainMenu = new MainMenu();
            tokenMenu = new TokenMenu();
            guna2DragControl2 = new Guna.UI2.WinForms.Guna2DragControl(components);
            panelTopbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureTopBarIcon).BeginInit();
            SuspendLayout();
            // 
            // panelTopbar
            // 
            panelTopbar.BackColor = Color.FromArgb(44, 47, 53);
            panelTopbar.BorderThickness = 1;
            panelTopbar.Controls.Add(LabelTopBarTitle);
            panelTopbar.Controls.Add(PictureTopBarIcon);
            panelTopbar.Controls.Add(ButtonMinimize);
            panelTopbar.Controls.Add(ButtonClose);
            panelTopbar.CustomizableEdges = customizableEdges7;
            panelTopbar.Dock = DockStyle.Top;
            panelTopbar.Location = new Point(0, 0);
            panelTopbar.Name = "panelTopbar";
            panelTopbar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelTopbar.Size = new Size(400, 30);
            panelTopbar.TabIndex = 1;
            // 
            // LabelTopBarTitle
            // 
            LabelTopBarTitle.AutoSize = false;
            LabelTopBarTitle.BackColor = Color.Transparent;
            LabelTopBarTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabelTopBarTitle.ForeColor = Color.White;
            LabelTopBarTitle.IsSelectionEnabled = false;
            LabelTopBarTitle.Location = new Point(30, 0);
            LabelTopBarTitle.Name = "LabelTopBarTitle";
            LabelTopBarTitle.Size = new Size(149, 30);
            LabelTopBarTitle.TabIndex = 5;
            LabelTopBarTitle.Text = "Discord Emoji Downloader";
            LabelTopBarTitle.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // PictureTopBarIcon
            // 
            PictureTopBarIcon.CustomizableEdges = customizableEdges1;
            PictureTopBarIcon.Image = Properties.Resources.icon_png;
            PictureTopBarIcon.ImageRotate = 0F;
            PictureTopBarIcon.Location = new Point(2, 2);
            PictureTopBarIcon.Name = "PictureTopBarIcon";
            PictureTopBarIcon.ShadowDecoration.CustomizableEdges = customizableEdges2;
            PictureTopBarIcon.Size = new Size(26, 26);
            PictureTopBarIcon.SizeMode = PictureBoxSizeMode.Zoom;
            PictureTopBarIcon.TabIndex = 4;
            PictureTopBarIcon.TabStop = false;
            // 
            // ButtonMinimize
            // 
            ButtonMinimize.BackColor = Color.Transparent;
            ButtonMinimize.ControlBoxStyle = Guna.UI2.WinForms.Enums.ControlBoxStyle.Custom;
            ButtonMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            ButtonMinimize.Cursor = Cursors.Hand;
            ButtonMinimize.CustomizableEdges = customizableEdges3;
            ButtonMinimize.Dock = DockStyle.Right;
            ButtonMinimize.FillColor = Color.Transparent;
            ButtonMinimize.HoverState.BorderColor = Color.Lime;
            ButtonMinimize.IconColor = Color.White;
            ButtonMinimize.Location = new Point(310, 0);
            ButtonMinimize.Name = "ButtonMinimize";
            ButtonMinimize.ShadowDecoration.CustomizableEdges = customizableEdges4;
            ButtonMinimize.Size = new Size(45, 30);
            ButtonMinimize.TabIndex = 3;
            // 
            // ButtonClose
            // 
            ButtonClose.BackColor = Color.Transparent;
            ButtonClose.ControlBoxStyle = Guna.UI2.WinForms.Enums.ControlBoxStyle.Custom;
            ButtonClose.Cursor = Cursors.Hand;
            ButtonClose.CustomizableEdges = customizableEdges5;
            ButtonClose.Dock = DockStyle.Right;
            ButtonClose.FillColor = Color.Transparent;
            ButtonClose.HoverState.BorderColor = Color.Lime;
            ButtonClose.IconColor = Color.White;
            ButtonClose.Location = new Point(355, 0);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ButtonClose.Size = new Size(45, 30);
            ButtonClose.TabIndex = 2;
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.DragStartTransparencyValue = 0.69D;
            guna2DragControl1.TargetControl = panelTopbar;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // mainMenu
            // 
            mainMenu.BackColor = Color.Transparent;
            mainMenu.BorderStyle = BorderStyle.FixedSingle;
            mainMenu.Location = new Point(0, 30);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(400, 170);
            mainMenu.TabIndex = 2;
            mainMenu.Visible = false;
            // 
            // tokenMenu
            // 
            tokenMenu.BackColor = Color.Transparent;
            tokenMenu.Location = new Point(0, 30);
            tokenMenu.Name = "tokenMenu";
            tokenMenu.Size = new Size(400, 170);
            tokenMenu.TabIndex = 6;
            tokenMenu.Visible = false;
            // 
            // guna2DragControl2
            // 
            guna2DragControl2.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl2.DragStartTransparencyValue = 0.69D;
            guna2DragControl2.TargetControl = LabelTopBarTitle;
            guna2DragControl2.UseTransparentDrag = true;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(54, 57, 63);
            ClientSize = new Size(400, 200);
            ControlBox = false;
            Controls.Add(tokenMenu);
            Controls.Add(panelTopbar);
            Controls.Add(mainMenu);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            panelTopbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureTopBarIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel panelTopbar;
        private Guna.UI2.WinForms.Guna2ControlBox ButtonMinimize;
        private Guna.UI2.WinForms.Guna2ControlBox ButtonClose;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2PictureBox PictureTopBarIcon;
        private Guna.UI2.WinForms.Guna2HtmlLabel LabelTopBarTitle;
        private MainMenu mainMenu;
        private TokenMenu tokenMenu;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl2;
    }
}
