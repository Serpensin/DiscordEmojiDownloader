namespace DiscordEmojiDownloader
{
    partial class TokenMenu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            ButtonGetToken = new Guna.UI2.WinForms.Guna2Button();
            ComboDetectedAccounts = new Guna.UI2.WinForms.Guna2ComboBox();
            TextboxManualToken = new Guna.UI2.WinForms.Guna2TextBox();
            labelInvalidToken = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ProgressIndicatorTokenMenu = new Guna.UI2.WinForms.Guna2ProgressIndicator();
            SuspendLayout();
            // 
            // ButtonGetToken
            // 
            ButtonGetToken.CustomizableEdges = customizableEdges1;
            ButtonGetToken.DisabledState.BorderColor = Color.DarkGray;
            ButtonGetToken.DisabledState.CustomBorderColor = Color.DarkGray;
            ButtonGetToken.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            ButtonGetToken.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            ButtonGetToken.FillColor = Color.FromArgb(88, 101, 242);
            ButtonGetToken.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            ButtonGetToken.ForeColor = Color.White;
            ButtonGetToken.Location = new Point(106, 100);
            ButtonGetToken.Name = "ButtonGetToken";
            ButtonGetToken.ShadowDecoration.CustomizableEdges = customizableEdges2;
            ButtonGetToken.Size = new Size(180, 45);
            ButtonGetToken.TabIndex = 0;
            ButtonGetToken.Text = "Get Token";
            ButtonGetToken.Click += ButtonToken_Click;
            ButtonGetToken.HoverState.FillColor = Color.FromArgb(100, 115, 250);
            ButtonGetToken.HoverState.ForeColor = Color.White;
            ButtonGetToken.HoverState.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            // 
            // ComboDetectedAccounts
            // 
            ComboDetectedAccounts.BackColor = Color.Transparent;
            ComboDetectedAccounts.CustomizableEdges = customizableEdges3;
            ComboDetectedAccounts.DrawMode = DrawMode.OwnerDrawFixed;
            ComboDetectedAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboDetectedAccounts.FillColor = Color.FromArgb(54, 57, 63);
            ComboDetectedAccounts.FocusedColor = Color.FromArgb(114, 137, 218);
            ComboDetectedAccounts.FocusedState.BorderColor = Color.FromArgb(114, 137, 218);
            ComboDetectedAccounts.Font = new Font("Segoe UI Symbol", 9.75F);
            ComboDetectedAccounts.ForeColor = Color.White;
            ComboDetectedAccounts.ItemHeight = 30;
            ComboDetectedAccounts.Location = new Point(50, 31);
            ComboDetectedAccounts.Name = "ComboDetectedAccounts";
            ComboDetectedAccounts.ShadowDecoration.CustomizableEdges = customizableEdges4;
            ComboDetectedAccounts.Size = new Size(292, 36);
            ComboDetectedAccounts.TabIndex = 1;
            ComboDetectedAccounts.Visible = false;
            // 
            // TextboxManualToken
            // 
            TextboxManualToken.BackColor = Color.FromArgb(41, 43, 47);
            TextboxManualToken.CustomizableEdges = customizableEdges5;
            TextboxManualToken.DefaultText = "";
            TextboxManualToken.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextboxManualToken.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextboxManualToken.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextboxManualToken.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextboxManualToken.FillColor = Color.FromArgb(54, 57, 63);
            TextboxManualToken.FocusedState.BorderColor = Color.FromArgb(114, 137, 218);
            TextboxManualToken.Font = new Font("Segoe UI", 9F);
            TextboxManualToken.HoverState.BorderColor = Color.FromArgb(114, 137, 218);
            TextboxManualToken.Location = new Point(95, 31);
            TextboxManualToken.Name = "TextboxManualToken";
            TextboxManualToken.PlaceholderForeColor = Color.White;
            TextboxManualToken.PlaceholderText = "Manual Token";
            TextboxManualToken.SelectedText = "";
            TextboxManualToken.ShadowDecoration.CustomizableEdges = customizableEdges6;
            TextboxManualToken.Size = new Size(200, 36);
            TextboxManualToken.TabIndex = 2;
            TextboxManualToken.Visible = false;
            // 
            // labelInvalidToken
            // 
            labelInvalidToken.AutoSize = false;
            labelInvalidToken.BackColor = Color.Transparent;
            labelInvalidToken.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelInvalidToken.ForeColor = Color.Red;
            labelInvalidToken.Location = new Point(-1, 78);
            labelInvalidToken.Name = "labelInvalidToken";
            labelInvalidToken.Size = new Size(402, 17);
            labelInvalidToken.TabIndex = 3;
            labelInvalidToken.Text = "!!! Invalid Token !!!";
            labelInvalidToken.TextAlignment = ContentAlignment.MiddleCenter;
            labelInvalidToken.Visible = false;
            // 
            // ProgressIndicatorTokenMenu
            // 
            ProgressIndicatorTokenMenu.AutoStart = true;
            ProgressIndicatorTokenMenu.Location = new Point(292, 100);
            ProgressIndicatorTokenMenu.Name = "ProgressIndicatorTokenMenu";
            ProgressIndicatorTokenMenu.ShadowDecoration.CustomizableEdges = customizableEdges7;
            ProgressIndicatorTokenMenu.Size = new Size(45, 45);
            ProgressIndicatorTokenMenu.TabIndex = 4;
            ProgressIndicatorTokenMenu.Visible = false;
            // 
            // TokenMenu
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(35, 39, 42);
            Controls.Add(ProgressIndicatorTokenMenu);
            Controls.Add(labelInvalidToken);
            Controls.Add(TextboxManualToken);
            Controls.Add(ComboDetectedAccounts);
            Controls.Add(ButtonGetToken);
            Name = "TokenMenu";
            Size = new Size(400, 170);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button ButtonGetToken;
        private Guna.UI2.WinForms.Guna2ComboBox ComboDetectedAccounts;
        private Guna.UI2.WinForms.Guna2TextBox TextboxManualToken;
        private Guna.UI2.WinForms.Guna2HtmlLabel labelInvalidToken;
        private Guna.UI2.WinForms.Guna2ProgressIndicator ProgressIndicatorTokenMenu;
    }
}
