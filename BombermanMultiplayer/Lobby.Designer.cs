namespace BombermanMultiplayer
{
    partial class Lobby
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
                if (cts != null)
                {
                    cts.Dispose();

                }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lobby));
            this.lbConnected = new System.Windows.Forms.ListBox();
            this.btnLaunchServer = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.tslMenu = new System.Windows.Forms.ToolStrip();
            this.tsDropDownFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsbExit = new System.Windows.Forms.ToolStripButton();
            this.lbLstPlayers = new System.Windows.Forms.Label();
            this.PanelConnections = new System.Windows.Forms.Panel();
            this.panelPlayersOptions = new System.Windows.Forms.Panel();
            this.lbPlayerOptions = new System.Windows.Forms.Label();
            this.tbNamePlayer = new System.Windows.Forms.TextBox();
            this.lbNamePlayer = new System.Windows.Forms.Label();
            this.panelClient = new System.Windows.Forms.Panel();
            this.lbConnectToAGame = new System.Windows.Forms.Label();
            this.lbAdressConnect = new System.Windows.Forms.Label();
            this.tbAddressConnect = new System.Windows.Forms.TextBox();
            this.lbPortConnect = new System.Windows.Forms.Label();
            this.tbPortConnect = new System.Windows.Forms.TextBox();
            this.panelServer = new System.Windows.Forms.Panel();
            this.lbCreateServer = new System.Windows.Forms.Label();
            this.lbSaveGame = new System.Windows.Forms.Label();
            this.tbGameToLoad = new System.Windows.Forms.TextBox();
            this.btnLoadGame = new System.Windows.Forms.Button();
            this.lbPortServer = new System.Windows.Forms.Label();
            this.tbPortServer = new System.Windows.Forms.TextBox();
            this.lbServerOnline = new System.Windows.Forms.Label();
            this.ConnectionTimer = new System.Windows.Forms.Timer(this.components);
            this.refreshGraphics = new System.Windows.Forms.Timer(this.components);
            this.panelPlayerList = new System.Windows.Forms.Panel();
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.panelGame = new System.Windows.Forms.Panel();
            this.tslMenu.SuspendLayout();
            this.PanelConnections.SuspendLayout();
            this.panelPlayersOptions.SuspendLayout();
            this.panelClient.SuspendLayout();
            this.panelServer.SuspendLayout();
            this.panelPlayerList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            this.panelGame.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbConnected
            // 
            this.lbConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbConnected.FormattingEnabled = true;
            this.lbConnected.ItemHeight = 16;
            this.lbConnected.Location = new System.Drawing.Point(0, 17);
            this.lbConnected.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbConnected.Name = "lbConnected";
            this.lbConnected.Size = new System.Drawing.Size(1387, 141);
            this.lbConnected.TabIndex = 0;
            // 
            // btnLaunchServer
            // 
            this.btnLaunchServer.Location = new System.Drawing.Point(15, 107);
            this.btnLaunchServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLaunchServer.Name = "btnLaunchServer";
            this.btnLaunchServer.Size = new System.Drawing.Size(137, 46);
            this.btnLaunchServer.TabIndex = 1;
            this.btnLaunchServer.Text = "Launch ";
            this.btnLaunchServer.UseVisualStyleBackColor = true;
            this.btnLaunchServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnClient
            // 
            this.btnClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClient.Location = new System.Drawing.Point(363, 66);
            this.btnClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(219, 50);
            this.btnClient.TabIndex = 2;
            this.btnClient.Text = "Connect";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // tslMenu
            // 
            this.tslMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tslMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.tslMenu.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.tslMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tslMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDropDownFile,
            this.tlsbExit});
            this.tslMenu.Location = new System.Drawing.Point(12, 2);
            this.tslMenu.Name = "tslMenu";
            this.tslMenu.Size = new System.Drawing.Size(173, 53);
            this.tslMenu.Stretch = true;
            this.tslMenu.TabIndex = 2;
            this.tslMenu.Text = "toolStrip1";
            this.tslMenu.Visible = false;
            // 
            // tsDropDownFile
            // 
            this.tsDropDownFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsDropDownFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.tsDropDownFile.Image = ((System.Drawing.Image)(resources.GetObject("tsDropDownFile.Image")));
            this.tsDropDownFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDropDownFile.Name = "tsDropDownFile";
            this.tsDropDownFile.Size = new System.Drawing.Size(85, 50);
            this.tsDropDownFile.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(183, 50);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // tlsbExit
            // 
            this.tlsbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tlsbExit.Image")));
            this.tlsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbExit.Name = "tlsbExit";
            this.tlsbExit.Size = new System.Drawing.Size(77, 50);
            this.tlsbExit.Text = "Exit";
            this.tlsbExit.Click += new System.EventHandler(this.tlsbExit_Click);
            // 
            // lbLstPlayers
            // 
            this.lbLstPlayers.AutoSize = true;
            this.lbLstPlayers.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbLstPlayers.Location = new System.Drawing.Point(0, 0);
            this.lbLstPlayers.Name = "lbLstPlayers";
            this.lbLstPlayers.Size = new System.Drawing.Size(81, 17);
            this.lbLstPlayers.TabIndex = 1;
            this.lbLstPlayers.Text = "Players List";
            // 
            // PanelConnections
            // 
            this.PanelConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelConnections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelConnections.Controls.Add(this.panelPlayersOptions);
            this.PanelConnections.Controls.Add(this.panelClient);
            this.PanelConnections.Controls.Add(this.panelServer);
            this.PanelConnections.Location = new System.Drawing.Point(12, 225);
            this.PanelConnections.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelConnections.Name = "PanelConnections";
            this.PanelConnections.Size = new System.Drawing.Size(1392, 189);
            this.PanelConnections.TabIndex = 4;
            // 
            // panelPlayersOptions
            // 
            this.panelPlayersOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPlayersOptions.Controls.Add(this.lbPlayerOptions);
            this.panelPlayersOptions.Controls.Add(this.tbNamePlayer);
            this.panelPlayersOptions.Controls.Add(this.lbNamePlayer);
            this.panelPlayersOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayersOptions.Location = new System.Drawing.Point(631, 0);
            this.panelPlayersOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPlayersOptions.Name = "panelPlayersOptions";
            this.panelPlayersOptions.Size = new System.Drawing.Size(420, 185);
            this.panelPlayersOptions.TabIndex = 2;
            // 
            // lbPlayerOptions
            // 
            this.lbPlayerOptions.AutoSize = true;
            this.lbPlayerOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlayerOptions.Location = new System.Drawing.Point(5, 9);
            this.lbPlayerOptions.Name = "lbPlayerOptions";
            this.lbPlayerOptions.Size = new System.Drawing.Size(149, 25);
            this.lbPlayerOptions.TabIndex = 8;
            this.lbPlayerOptions.Text = "Player options";
            // 
            // tbNamePlayer
            // 
            this.tbNamePlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tbNamePlayer.Location = new System.Drawing.Point(158, 94);
            this.tbNamePlayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbNamePlayer.MaxLength = 10;
            this.tbNamePlayer.Name = "tbNamePlayer";
            this.tbNamePlayer.Size = new System.Drawing.Size(100, 22);
            this.tbNamePlayer.TabIndex = 4;
            this.tbNamePlayer.Text = "Player";
            // 
            // lbNamePlayer
            // 
            this.lbNamePlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbNamePlayer.AutoSize = true;
            this.lbNamePlayer.Location = new System.Drawing.Point(162, 73);
            this.lbNamePlayer.Name = "lbNamePlayer";
            this.lbNamePlayer.Size = new System.Drawing.Size(97, 17);
            this.lbNamePlayer.TabIndex = 5;
            this.lbNamePlayer.Text = "Player Name :";
            // 
            // panelClient
            // 
            this.panelClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelClient.Controls.Add(this.lbConnectToAGame);
            this.panelClient.Controls.Add(this.lbAdressConnect);
            this.panelClient.Controls.Add(this.tbAddressConnect);
            this.panelClient.Controls.Add(this.btnClient);
            this.panelClient.Controls.Add(this.lbPortConnect);
            this.panelClient.Controls.Add(this.tbPortConnect);
            this.panelClient.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelClient.Location = new System.Drawing.Point(0, 0);
            this.panelClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelClient.Name = "panelClient";
            this.panelClient.Size = new System.Drawing.Size(631, 185);
            this.panelClient.TabIndex = 1;
            // 
            // lbConnectToAGame
            // 
            this.lbConnectToAGame.AutoSize = true;
            this.lbConnectToAGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbConnectToAGame.Location = new System.Drawing.Point(9, 9);
            this.lbConnectToAGame.Name = "lbConnectToAGame";
            this.lbConnectToAGame.Size = new System.Drawing.Size(207, 25);
            this.lbConnectToAGame.TabIndex = 6;
            this.lbConnectToAGame.Text = "Connect to a server ";
            // 
            // lbAdressConnect
            // 
            this.lbAdressConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbAdressConnect.AutoSize = true;
            this.lbAdressConnect.Location = new System.Drawing.Point(3, 101);
            this.lbAdressConnect.Name = "lbAdressConnect";
            this.lbAdressConnect.Size = new System.Drawing.Size(20, 17);
            this.lbAdressConnect.TabIndex = 5;
            this.lbAdressConnect.Text = "IP";
            // 
            // tbAddressConnect
            // 
            this.tbAddressConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbAddressConnect.Location = new System.Drawing.Point(5, 126);
            this.tbAddressConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbAddressConnect.Name = "tbAddressConnect";
            this.tbAddressConnect.Size = new System.Drawing.Size(100, 22);
            this.tbAddressConnect.TabIndex = 4;
            this.tbAddressConnect.Text = "127.0.0.1";
            // 
            // lbPortConnect
            // 
            this.lbPortConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbPortConnect.AutoSize = true;
            this.lbPortConnect.Location = new System.Drawing.Point(3, 41);
            this.lbPortConnect.Name = "lbPortConnect";
            this.lbPortConnect.Size = new System.Drawing.Size(34, 17);
            this.lbPortConnect.TabIndex = 3;
            this.lbPortConnect.Text = "Port";
            // 
            // tbPortConnect
            // 
            this.tbPortConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPortConnect.Location = new System.Drawing.Point(5, 70);
            this.tbPortConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPortConnect.Name = "tbPortConnect";
            this.tbPortConnect.Size = new System.Drawing.Size(100, 22);
            this.tbPortConnect.TabIndex = 2;
            this.tbPortConnect.Text = "3000";
            // 
            // panelServer
            // 
            this.panelServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelServer.Controls.Add(this.lbCreateServer);
            this.panelServer.Controls.Add(this.lbSaveGame);
            this.panelServer.Controls.Add(this.tbGameToLoad);
            this.panelServer.Controls.Add(this.btnLoadGame);
            this.panelServer.Controls.Add(this.btnLaunchServer);
            this.panelServer.Controls.Add(this.lbPortServer);
            this.panelServer.Controls.Add(this.tbPortServer);
            this.panelServer.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelServer.Location = new System.Drawing.Point(1051, 0);
            this.panelServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelServer.Name = "panelServer";
            this.panelServer.Size = new System.Drawing.Size(337, 185);
            this.panelServer.TabIndex = 2;
            this.panelServer.Paint += new System.Windows.Forms.PaintEventHandler(this.panelServer_Paint);
            // 
            // lbCreateServer
            // 
            this.lbCreateServer.AutoSize = true;
            this.lbCreateServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCreateServer.Location = new System.Drawing.Point(5, 9);
            this.lbCreateServer.Name = "lbCreateServer";
            this.lbCreateServer.Size = new System.Drawing.Size(161, 25);
            this.lbCreateServer.TabIndex = 7;
            this.lbCreateServer.Text = "Create a server";
            // 
            // lbSaveGame
            // 
            this.lbSaveGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSaveGame.AutoSize = true;
            this.lbSaveGame.Location = new System.Drawing.Point(199, 36);
            this.lbSaveGame.Name = "lbSaveGame";
            this.lbSaveGame.Size = new System.Drawing.Size(101, 17);
            this.lbSaveGame.TabIndex = 6;
            this.lbSaveGame.Text = "Game to load :";
            // 
            // tbGameToLoad
            // 
            this.tbGameToLoad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGameToLoad.Enabled = false;
            this.tbGameToLoad.Location = new System.Drawing.Point(200, 68);
            this.tbGameToLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbGameToLoad.Name = "tbGameToLoad";
            this.tbGameToLoad.ReadOnly = true;
            this.tbGameToLoad.Size = new System.Drawing.Size(100, 22);
            this.tbGameToLoad.TabIndex = 5;
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadGame.Location = new System.Drawing.Point(200, 107);
            this.btnLoadGame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(120, 39);
            this.btnLoadGame.TabIndex = 4;
            this.btnLoadGame.Text = "Load Game";
            this.btnLoadGame.UseVisualStyleBackColor = true;
            this.btnLoadGame.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbPortServer
            // 
            this.lbPortServer.AutoSize = true;
            this.lbPortServer.Location = new System.Drawing.Point(12, 36);
            this.lbPortServer.Name = "lbPortServer";
            this.lbPortServer.Size = new System.Drawing.Size(34, 17);
            this.lbPortServer.TabIndex = 1;
            this.lbPortServer.Text = "Port";
            // 
            // tbPortServer
            // 
            this.tbPortServer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbPortServer.Location = new System.Drawing.Point(15, 65);
            this.tbPortServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPortServer.Name = "tbPortServer";
            this.tbPortServer.Size = new System.Drawing.Size(100, 22);
            this.tbPortServer.TabIndex = 0;
            this.tbPortServer.Text = "3000";
            // 
            // lbServerOnline
            // 
            this.lbServerOnline.AutoSize = true;
            this.lbServerOnline.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbServerOnline.ForeColor = System.Drawing.Color.Red;
            this.lbServerOnline.Location = new System.Drawing.Point(0, 401);
            this.lbServerOnline.Name = "lbServerOnline";
            this.lbServerOnline.Size = new System.Drawing.Size(92, 17);
            this.lbServerOnline.TabIndex = 5;
            this.lbServerOnline.Text = "Server online";
            this.lbServerOnline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbServerOnline.Visible = false;
            // 
            // ConnectionTimer
            // 
            this.ConnectionTimer.Interval = 120;
            this.ConnectionTimer.Tick += new System.EventHandler(this.ConnectionTimer_Tick);
            // 
            // refreshGraphics
            // 
            this.refreshGraphics.Tick += new System.EventHandler(this.refreshGraphics_Tick);
            // 
            // panelPlayerList
            // 
            this.panelPlayerList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelPlayerList.Controls.Add(this.lbConnected);
            this.panelPlayerList.Controls.Add(this.lbLstPlayers);
            this.panelPlayerList.Enabled = false;
            this.panelPlayerList.Location = new System.Drawing.Point(12, 58);
            this.panelPlayerList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPlayerList.Name = "panelPlayerList";
            this.panelPlayerList.Size = new System.Drawing.Size(1391, 162);
            this.panelPlayerList.TabIndex = 3;
            // 
            // pbGame
            // 
            this.pbGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbGame.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pbGame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbGame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbGame.Enabled = false;
            this.pbGame.Location = new System.Drawing.Point(0, 0);
            this.pbGame.Margin = new System.Windows.Forms.Padding(60);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(527, 377);
            this.pbGame.TabIndex = 6;
            this.pbGame.TabStop = false;
            this.pbGame.Visible = false;
            // 
            // panelGame
            // 
            this.panelGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGame.AutoSize = true;
            this.panelGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGame.Controls.Add(this.pbGame);
            this.panelGame.Location = new System.Drawing.Point(13, 420);
            this.panelGame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(1389, 657);
            this.panelGame.TabIndex = 7;
            this.panelGame.Visible = false;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1413, 418);
            this.Controls.Add(this.tslMenu);
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.PanelConnections);
            this.Controls.Add(this.panelPlayerList);
            this.Controls.Add(this.lbServerOnline);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Lobby";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Lobby_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lobby_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Lobby_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Lobby_PreviewKeyDown);
            this.tslMenu.ResumeLayout(false);
            this.tslMenu.PerformLayout();
            this.PanelConnections.ResumeLayout(false);
            this.panelPlayersOptions.ResumeLayout(false);
            this.panelPlayersOptions.PerformLayout();
            this.panelClient.ResumeLayout(false);
            this.panelClient.PerformLayout();
            this.panelServer.ResumeLayout(false);
            this.panelServer.PerformLayout();
            this.panelPlayerList.ResumeLayout(false);
            this.panelPlayerList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
            this.panelGame.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbConnected;
        private System.Windows.Forms.Button btnLaunchServer;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Panel PanelConnections;
        private System.Windows.Forms.Panel panelServer;
        private System.Windows.Forms.Label lbPortServer;
        private System.Windows.Forms.TextBox tbPortServer;
        private System.Windows.Forms.Panel panelClient;
        private System.Windows.Forms.Label lbAdressConnect;
        private System.Windows.Forms.TextBox tbAddressConnect;
        private System.Windows.Forms.Label lbPortConnect;
        private System.Windows.Forms.TextBox tbPortConnect;
        private System.Windows.Forms.Label lbServerOnline;
        private System.Windows.Forms.Label lbLstPlayers;
        private System.Windows.Forms.Timer ConnectionTimer;
        private System.Windows.Forms.PictureBox pbGame;
        private System.Windows.Forms.Timer refreshGraphics;
        private System.Windows.Forms.ToolStrip tslMenu;
        private System.Windows.Forms.ToolStripDropDownButton tsDropDownFile;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Panel panelPlayerList;
        private System.Windows.Forms.Label lbNamePlayer;
        private System.Windows.Forms.TextBox tbNamePlayer;
        private System.Windows.Forms.Label lbSaveGame;
        private System.Windows.Forms.TextBox tbGameToLoad;
        private System.Windows.Forms.Button btnLoadGame;
        private System.Windows.Forms.ToolStripButton tlsbExit;
        private System.Windows.Forms.Panel panelPlayersOptions;
        private System.Windows.Forms.Label lbPlayerOptions;
        private System.Windows.Forms.Label lbConnectToAGame;
        private System.Windows.Forms.Label lbCreateServer;
        private System.Windows.Forms.Panel panelGame;
    }
}