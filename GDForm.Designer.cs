namespace GenerativeDoom
{
    partial class GDForm
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
            this.BtnClose = new System.Windows.Forms.Button();
            this.btnDoMagic = new System.Windows.Forms.Button();
            this.btnAnalysis = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lbCategories = new System.Windows.Forms.ListBox();
            this.doorSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.doorHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.doorWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.roomNumber = new System.Windows.Forms.TextBox();
            this.RoomType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.roomID = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.roomWidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.roomHeight = new System.Windows.Forms.TextBox();
            this.doorCheck = new System.Windows.Forms.CheckedListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.R_x = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.R_y = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.L_y = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.L_x = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.T_y = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.T_x = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.B_y = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.B_x = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.CreateRoomInfo = new System.Windows.Forms.Button();
            this.prefabObjectPath = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.roomID)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnClose.Location = new System.Drawing.Point(267, 219);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(2);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(81, 29);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnDoMagic
            // 
            this.btnDoMagic.Location = new System.Drawing.Point(12, 53);
            this.btnDoMagic.Margin = new System.Windows.Forms.Padding(1);
            this.btnDoMagic.Name = "btnDoMagic";
            this.btnDoMagic.Size = new System.Drawing.Size(84, 27);
            this.btnDoMagic.TabIndex = 1;
            this.btnDoMagic.Text = "Do Some Magic";
            this.btnDoMagic.UseVisualStyleBackColor = true;
            this.btnDoMagic.Click += new System.EventHandler(this.btnDoMagic_Click);
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Location = new System.Drawing.Point(12, 12);
            this.btnAnalysis.Margin = new System.Windows.Forms.Padding(1);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(81, 33);
            this.btnAnalysis.TabIndex = 2;
            this.btnAnalysis.Text = "Analysis";
            this.btnAnalysis.UseVisualStyleBackColor = true;
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(185, 215);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(1);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(79, 33);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "Generation";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lbCategories
            // 
            this.lbCategories.FormattingEnabled = true;
            this.lbCategories.ItemHeight = 14;
            this.lbCategories.Location = new System.Drawing.Point(16, 132);
            this.lbCategories.Margin = new System.Windows.Forms.Padding(1);
            this.lbCategories.Name = "lbCategories";
            this.lbCategories.Size = new System.Drawing.Size(115, 116);
            this.lbCategories.TabIndex = 4;
            // 
            // doorSize
            // 
            this.doorSize.Location = new System.Drawing.Point(267, 13);
            this.doorSize.Name = "doorSize";
            this.doorSize.Size = new System.Drawing.Size(79, 19);
            this.doorSize.TabIndex = 5;
            this.doorSize.Text = "32";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(217, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "DoorSize";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "DoorHeight";
            // 
            // doorHeight
            // 
            this.doorHeight.Location = new System.Drawing.Point(267, 38);
            this.doorHeight.Name = "doorHeight";
            this.doorHeight.Size = new System.Drawing.Size(79, 19);
            this.doorHeight.TabIndex = 7;
            this.doorHeight.Text = "64";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "DoorWaySize";
            // 
            // doorWidth
            // 
            this.doorWidth.Location = new System.Drawing.Point(267, 63);
            this.doorWidth.Name = "doorWidth";
            this.doorWidth.Size = new System.Drawing.Size(79, 19);
            this.doorWidth.TabIndex = 9;
            this.doorWidth.Text = "128";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "RoomNumber";
            // 
            // roomNumber
            // 
            this.roomNumber.Location = new System.Drawing.Point(268, 88);
            this.roomNumber.Name = "roomNumber";
            this.roomNumber.Size = new System.Drawing.Size(79, 19);
            this.roomNumber.TabIndex = 11;
            this.roomNumber.Text = "5";
            // 
            // RoomType
            // 
            this.RoomType.FormattingEnabled = true;
            this.RoomType.Items.AddRange(new object[] {
            "Ammo",
            "Key",
            "Locked",
            "Weapon",
            "SuperWeapon",
            "Enemies",
            "Boss"});
            this.RoomType.Location = new System.Drawing.Point(475, 10);
            this.RoomType.Name = "RoomType";
            this.RoomType.Size = new System.Drawing.Size(234, 22);
            this.RoomType.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(412, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Room Type";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // roomID
            // 
            this.roomID.Location = new System.Drawing.Point(475, 39);
            this.roomID.Name = "roomID";
            this.roomID.Size = new System.Drawing.Size(44, 19);
            this.roomID.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(426, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Room ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(595, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 15);
            this.label7.TabIndex = 20;
            this.label7.Text = "Width";
            // 
            // roomWidth
            // 
            this.roomWidth.Location = new System.Drawing.Point(630, 66);
            this.roomWidth.Name = "roomWidth";
            this.roomWidth.Size = new System.Drawing.Size(79, 19);
            this.roomWidth.TabIndex = 19;
            this.roomWidth.Text = "2048";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(595, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 15);
            this.label8.TabIndex = 18;
            this.label8.Text = "Height";
            // 
            // roomHeight
            // 
            this.roomHeight.Location = new System.Drawing.Point(630, 41);
            this.roomHeight.Name = "roomHeight";
            this.roomHeight.Size = new System.Drawing.Size(79, 19);
            this.roomHeight.TabIndex = 17;
            this.roomHeight.Text = "1024";
            // 
            // doorCheck
            // 
            this.doorCheck.BackColor = System.Drawing.SystemColors.Info;
            this.doorCheck.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.doorCheck.FormattingEnabled = true;
            this.doorCheck.Items.AddRange(new object[] {
            "RIGHT",
            "LEFT",
            "TOP",
            "BOTTOM"});
            this.doorCheck.Location = new System.Drawing.Point(415, 90);
            this.doorCheck.Name = "doorCheck";
            this.doorCheck.Size = new System.Drawing.Size(72, 98);
            this.doorCheck.TabIndex = 21;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 261);
            this.splitter1.TabIndex = 22;
            this.splitter1.TabStop = false;
            // 
            // R_x
            // 
            this.R_x.Location = new System.Drawing.Point(569, 91);
            this.R_x.Name = "R_x";
            this.R_x.Size = new System.Drawing.Size(58, 19);
            this.R_x.TabIndex = 23;
            this.R_x.Text = "32";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(550, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 15);
            this.label9.TabIndex = 24;
            this.label9.Text = "X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(632, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 15);
            this.label10.TabIndex = 26;
            this.label10.Text = "Y";
            // 
            // R_y
            // 
            this.R_y.Location = new System.Drawing.Point(651, 90);
            this.R_y.Name = "R_y";
            this.R_y.Size = new System.Drawing.Size(58, 19);
            this.R_y.TabIndex = 25;
            this.R_y.Text = "32";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(632, 118);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 15);
            this.label11.TabIndex = 30;
            this.label11.Text = "Y";
            // 
            // L_y
            // 
            this.L_y.Location = new System.Drawing.Point(651, 115);
            this.L_y.Name = "L_y";
            this.L_y.Size = new System.Drawing.Size(58, 19);
            this.L_y.TabIndex = 29;
            this.L_y.Text = "32";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(550, 119);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 15);
            this.label12.TabIndex = 28;
            this.label12.Text = "X";
            // 
            // L_x
            // 
            this.L_x.Location = new System.Drawing.Point(569, 116);
            this.L_x.Name = "L_x";
            this.L_x.Size = new System.Drawing.Size(58, 19);
            this.L_x.TabIndex = 27;
            this.L_x.Text = "32";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(632, 143);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 15);
            this.label13.TabIndex = 34;
            this.label13.Text = "Y";
            // 
            // T_y
            // 
            this.T_y.Location = new System.Drawing.Point(651, 140);
            this.T_y.Name = "T_y";
            this.T_y.Size = new System.Drawing.Size(58, 19);
            this.T_y.TabIndex = 33;
            this.T_y.Text = "32";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(550, 144);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 15);
            this.label14.TabIndex = 32;
            this.label14.Text = "X";
            // 
            // T_x
            // 
            this.T_x.Location = new System.Drawing.Point(569, 141);
            this.T_x.Name = "T_x";
            this.T_x.Size = new System.Drawing.Size(58, 19);
            this.T_x.TabIndex = 31;
            this.T_x.Text = "32";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(632, 168);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 15);
            this.label15.TabIndex = 38;
            this.label15.Text = "Y";
            // 
            // B_y
            // 
            this.B_y.Location = new System.Drawing.Point(651, 165);
            this.B_y.Name = "B_y";
            this.B_y.Size = new System.Drawing.Size(58, 19);
            this.B_y.TabIndex = 37;
            this.B_y.Text = "32";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(550, 169);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(13, 15);
            this.label16.TabIndex = 36;
            this.label16.Text = "X";
            // 
            // B_x
            // 
            this.B_x.Location = new System.Drawing.Point(569, 166);
            this.B_x.Name = "B_x";
            this.B_x.Size = new System.Drawing.Size(58, 19);
            this.B_x.TabIndex = 35;
            this.B_x.Text = "32";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(493, 93);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 15);
            this.label17.TabIndex = 39;
            this.label17.Text = "RIGHT :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(493, 119);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 15);
            this.label18.TabIndex = 40;
            this.label18.Text = "LEFT :";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(493, 143);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 15);
            this.label19.TabIndex = 41;
            this.label19.Text = "TOP :";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(493, 168);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 15);
            this.label20.TabIndex = 42;
            this.label20.Text = "BOTTOM :";
            // 
            // CreateRoomInfo
            // 
            this.CreateRoomInfo.Location = new System.Drawing.Point(569, 215);
            this.CreateRoomInfo.Margin = new System.Windows.Forms.Padding(1);
            this.CreateRoomInfo.Name = "CreateRoomInfo";
            this.CreateRoomInfo.Size = new System.Drawing.Size(140, 33);
            this.CreateRoomInfo.TabIndex = 43;
            this.CreateRoomInfo.Text = "Create Room Info";
            this.CreateRoomInfo.UseVisualStyleBackColor = true;
            this.CreateRoomInfo.Click += new System.EventHandler(this.CreateRoomInfo_Click);
            // 
            // prefabObjectPath
            // 
            this.prefabObjectPath.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefabObjectPath.Location = new System.Drawing.Point(496, 191);
            this.prefabObjectPath.Name = "prefabObjectPath";
            this.prefabObjectPath.Size = new System.Drawing.Size(212, 20);
            this.prefabObjectPath.TabIndex = 44;
            this.prefabObjectPath.Text = "prefabs/newprefab.pfb";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(421, 194);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 15);
            this.label21.TabIndex = 45;
            this.label21.Text = "Path to prefab :";
            // 
            // GDForm
            // 
            this.AcceptButton = this.BtnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(721, 261);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.prefabObjectPath);
            this.Controls.Add(this.CreateRoomInfo);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.B_y);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.B_x);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.T_y);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.T_x);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.L_y);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.L_x);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.R_y);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.R_x);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.doorCheck);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.roomWidth);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.roomHeight);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.roomID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RoomType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.roomNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.doorWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.doorHeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.doorSize);
            this.Controls.Add(this.lbCategories);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnAnalysis);
            this.Controls.Add(this.btnDoMagic);
            this.Controls.Add(this.BtnClose);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GDForm";
            this.Text = "Generative Doom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GDForm_FormClosing);
            this.Load += new System.EventHandler(this.GDForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.roomID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button btnDoMagic;
        private System.Windows.Forms.Button btnAnalysis;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ListBox lbCategories;
        private System.Windows.Forms.TextBox doorSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox doorHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox doorWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox roomNumber;
        private System.Windows.Forms.ComboBox RoomType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown roomID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox roomWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox roomHeight;
        private System.Windows.Forms.CheckedListBox doorCheck;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox R_x;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox R_y;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox L_y;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox L_x;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox T_y;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox T_x;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox B_y;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox B_x;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button CreateRoomInfo;
        private System.Windows.Forms.TextBox prefabObjectPath;
        private System.Windows.Forms.Label label21;
    }
}