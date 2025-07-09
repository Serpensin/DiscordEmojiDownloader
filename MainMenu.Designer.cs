namespace DiscordEmojiDownloader
{
    partial class MainMenu
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

            ButtonDownload = new Guna.UI2.WinForms.Guna2Button();
            TextboxGuildID = new Guna.UI2.WinForms.Guna2TextBox();
            ProgressBarMainMenu = new Guna.UI2.WinForms.Guna2ProgressBar();
            ProgressIndicatorMainMenu = new Guna.UI2.WinForms.Guna2ProgressIndicator();
            CheckBoxCompress = new Guna.UI2.WinForms.Guna2CheckBox();
            SuspendLayout();
            // 
            // ButtonDownload
            // 
            ButtonDownload.CustomizableEdges = customizableEdges1;
            ButtonDownload.DisabledState.BorderColor = Color.DarkGray;
            ButtonDownload.DisabledState.CustomBorderColor = Color.DarkGray;
            ButtonDownload.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            ButtonDownload.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            ButtonDownload.FillColor = Color.FromArgb(88, 101, 242);
            ButtonDownload.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            ButtonDownload.ForeColor = Color.White;
            ButtonDownload.Location = new Point(208, 113);
            ButtonDownload.Name = "ButtonDownload";
            ButtonDownload.ShadowDecoration.CustomizableEdges = customizableEdges2;
            ButtonDownload.Size = new Size(160, 42);
            ButtonDownload.TabIndex = 1;
            ButtonDownload.Text = "Download";
            ButtonDownload.Click += ButtonDownload_Click;
            ButtonDownload.HoverState.FillColor = Color.FromArgb(100, 115, 250);
            ButtonDownload.HoverState.ForeColor = Color.White;
            ButtonDownload.HoverState.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            // 
            // TextboxGuildID
            // 
            TextboxGuildID.CustomizableEdges = customizableEdges3;
            TextboxGuildID.DefaultText = "";
            TextboxGuildID.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextboxGuildID.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextboxGuildID.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextboxGuildID.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextboxGuildID.FillColor = Color.FromArgb(54, 57, 63);
            TextboxGuildID.FocusedState.BorderColor = Color.FromArgb(114, 137, 218);
            TextboxGuildID.Font = new Font("Segoe UI", 9F);
            TextboxGuildID.ForeColor = Color.White;
            TextboxGuildID.HoverState.BorderColor = Color.FromArgb(114, 137, 218);
            TextboxGuildID.Location = new Point(20, 15);
            TextboxGuildID.Name = "TextboxGuildID";
            TextboxGuildID.PlaceholderText = "Server ID";
            TextboxGuildID.SelectedText = "";
            TextboxGuildID.ShadowDecoration.CustomizableEdges = customizableEdges4;
            TextboxGuildID.Size = new Size(348, 36);
            TextboxGuildID.TabIndex = 2;
            TextboxGuildID.TextChanged += TextboxGuildID_TextChanged;
            TextboxGuildID.KeyPress += TextboxGuildID_KeyPress;
            // 
            // ProgressBarMainMenu
            // 
            ProgressBarMainMenu.CustomizableEdges = customizableEdges5;
            ProgressBarMainMenu.FillColor = Color.FromArgb(40, 43, 48);
            ProgressBarMainMenu.Font = new Font("Segoe UI", 9F);
            ProgressBarMainMenu.ForeColor = Color.FromArgb(128, 255, 128);
            ProgressBarMainMenu.Location = new Point(20, 65);
            ProgressBarMainMenu.Name = "ProgressBarMainMenu";
            ProgressBarMainMenu.ProgressColor = Color.FromArgb(114, 137, 218);
            ProgressBarMainMenu.ProgressColor2 = Color.FromArgb(114, 137, 218);
            ProgressBarMainMenu.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ProgressBarMainMenu.ShowText = true;
            ProgressBarMainMenu.Size = new Size(348, 20);
            ProgressBarMainMenu.Style = ProgressBarStyle.Continuous;
            ProgressBarMainMenu.TabIndex = 5;
            ProgressBarMainMenu.TextMode = Guna.UI2.WinForms.Enums.ProgressBarTextMode.Custom;
            ProgressBarMainMenu.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // ProgressIndicatorMainMenu
            // 
            ProgressIndicatorMainMenu.AutoStart = true;
            ProgressIndicatorMainMenu.Location = new Point(157, 110);
            ProgressIndicatorMainMenu.Name = "ProgressIndicatorMainMenu";
            ProgressIndicatorMainMenu.ShadowDecoration.CustomizableEdges = customizableEdges7;
            ProgressIndicatorMainMenu.Size = new Size(45, 45);
            ProgressIndicatorMainMenu.TabIndex = 6;
            ProgressIndicatorMainMenu.Visible = false;
            // 
            // CheckBoxCompress
            // 
            CheckBoxCompress.AutoSize = true;
            CheckBoxCompress.CheckedState.BorderColor = Color.FromArgb(114, 137, 218);
            CheckBoxCompress.CheckedState.BorderRadius = 0;
            CheckBoxCompress.CheckedState.BorderThickness = 0;
            CheckBoxCompress.CheckedState.FillColor = Color.FromArgb(114, 137, 218);
            CheckBoxCompress.ForeColor = Color.White;
            CheckBoxCompress.Location = new Point(20, 122);
            CheckBoxCompress.Name = "CheckBoxCompress";
            CheckBoxCompress.Size = new Size(135, 19);
            CheckBoxCompress.TabIndex = 7;
            CheckBoxCompress.Text = "Compress on finish";
            CheckBoxCompress.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            CheckBoxCompress.UncheckedState.BorderRadius = 0;
            CheckBoxCompress.UncheckedState.BorderThickness = 0;
            CheckBoxCompress.UncheckedState.FillColor = Color.FromArgb(64, 68, 75);
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(35, 39, 42);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(CheckBoxCompress);
            Controls.Add(ProgressIndicatorMainMenu);
            Controls.Add(ProgressBarMainMenu);
            Controls.Add(TextboxGuildID);
            Controls.Add(ButtonDownload);
            Name = "MainMenu";
            Size = new Size(398, 168);
            Load += SelectDownloadFolderRoot;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button ButtonDownload;
        private Guna.UI2.WinForms.Guna2TextBox TextboxGuildID;
        private Guna.UI2.WinForms.Guna2ProgressBar ProgressBarMainMenu;
        private Guna.UI2.WinForms.Guna2ProgressIndicator ProgressIndicatorMainMenu;
        private Guna.UI2.WinForms.Guna2CheckBox CheckBoxCompress;
    }
}
