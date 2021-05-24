namespace BombermanMultiplayer
{
    partial class Mode
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
            this.btnLocalGame = new System.Windows.Forms.Button();
            this.btnLanMode = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocalGame
            // 
            this.btnLocalGame.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnLocalGame.AutoSize = true;
            this.btnLocalGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLocalGame.Location = new System.Drawing.Point(345, 146);
            this.btnLocalGame.Name = "btnLocalGame";
            this.btnLocalGame.Size = new System.Drawing.Size(256, 48);
            this.btnLocalGame.TabIndex = 0;
            this.btnLocalGame.Text = "Local Mode";
            this.btnLocalGame.UseVisualStyleBackColor = true;
            this.btnLocalGame.Click += new System.EventHandler(this.btnLocalGame_Click);
            // 
            // btnLanMode
            // 
            this.btnLanMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnLanMode.AutoSize = true;
            this.btnLanMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLanMode.Location = new System.Drawing.Point(344, 241);
            this.btnLanMode.Name = "btnLanMode";
            this.btnLanMode.Size = new System.Drawing.Size(256, 48);
            this.btnLanMode.TabIndex = 1;
            this.btnLanMode.Text = "LAN Mode";
            this.btnLanMode.UseVisualStyleBackColor = true;
            this.btnLanMode.Click += new System.EventHandler(this.btnLanMode_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(336, 33);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(265, 48);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "Chọn chế độ";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTitle.Click += new System.EventHandler(this.lbTitle_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_exit.Location = new System.Drawing.Point(344, 350);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(256, 48);
            this.btn_exit.TabIndex = 3;
            this.btn_exit.Text = "Quay lại";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.button1_Click);
            // 
            // Mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 659);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnLanMode);
            this.Controls.Add(this.btnLocalGame);
            this.Name = "Mode";
            this.Text = "Bắt đầu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLocalGame;
        private System.Windows.Forms.Button btnLanMode;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btn_exit;
    }
}