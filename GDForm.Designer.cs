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
            this.components = new System.ComponentModel.Container();
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
            this.roomNb = new System.Windows.Forms.TextBox();
            this.RoomType_t = new System.Windows.Forms.ComboBox();
            this.roomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.roomID = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.CreateRoomInfo = new System.Windows.Forms.Button();
            this.prefabObjectPath = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lockedNb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.BossNb = new System.Windows.Forms.TextBox();
            this.roomBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.keysBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.keysBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnClose.Location = new System.Drawing.Point(509, 172);
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
            this.btnGenerate.Location = new System.Drawing.Point(267, 172);
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
            // roomNb
            // 
            this.roomNb.Location = new System.Drawing.Point(268, 88);
            this.roomNb.Name = "roomNb";
            this.roomNb.Size = new System.Drawing.Size(79, 19);
            this.roomNb.TabIndex = 11;
            this.roomNb.Text = "10";
            // 
            // RoomType_t
            // 
            this.RoomType_t.DataSource = System.Enum.GetValues(typeof(RoomType));
            this.RoomType_t.FormattingEnabled = true;
            this.RoomType_t.Location = new System.Drawing.Point(450, 8);
            this.RoomType_t.Name = "RoomType_t";
            this.RoomType_t.Size = new System.Drawing.Size(140, 22);
            this.RoomType_t.TabIndex = 13;
            this.RoomType_t.SelectedIndexChanged += new System.EventHandler(this.RoomType_t_SelectedIndexChanged);
            // 
            // roomBindingSource
            // 
            this.roomBindingSource.DataSource = typeof(Room);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(387, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Room Type";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // roomID
            // 
            this.roomID.Location = new System.Drawing.Point(450, 37);
            this.roomID.Name = "roomID";
            this.roomID.Size = new System.Drawing.Size(140, 19);
            this.roomID.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(401, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Room ID";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 261);
            this.splitter1.TabIndex = 22;
            this.splitter1.TabStop = false;
            // 
            // CreateRoomInfo
            // 
            this.CreateRoomInfo.Location = new System.Drawing.Point(450, 99);
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
            this.prefabObjectPath.Location = new System.Drawing.Point(450, 64);
            this.prefabObjectPath.Name = "prefabObjectPath";
            this.prefabObjectPath.Size = new System.Drawing.Size(140, 20);
            this.prefabObjectPath.TabIndex = 44;
            this.prefabObjectPath.Text = "prefabs/newprefab.pfb";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(375, 66);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 15);
            this.label21.TabIndex = 45;
            this.label21.Text = "Path to prefab :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(199, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 15);
            this.label7.TabIndex = 47;
            this.label7.Text = "LockedRoom";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // lockedNb
            // 
            this.lockedNb.Location = new System.Drawing.Point(268, 113);
            this.lockedNb.Name = "lockedNb";
            this.lockedNb.Size = new System.Drawing.Size(79, 19);
            this.lockedNb.TabIndex = 46;
            this.lockedNb.Text = "2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(198, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 15);
            this.label8.TabIndex = 49;
            this.label8.Text = "BossRoom";
            // 
            // BossNb
            // 
            this.BossNb.Location = new System.Drawing.Point(267, 138);
            this.BossNb.Name = "BossNb";
            this.BossNb.Size = new System.Drawing.Size(79, 19);
            this.BossNb.TabIndex = 48;
            this.BossNb.Text = "1";
            // 
            // roomBindingSource1
            // 
            this.roomBindingSource1.DataSource = typeof(Room);
            // 
            // keysBindingSource
            // 
            this.keysBindingSource.DataSource = typeof(GeneratorParameters.Keys);
            // 
            // GDForm
            // 
            this.AcceptButton = this.BtnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(600, 261);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.BossNb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lockedNb);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.prefabObjectPath);
            this.Controls.Add(this.CreateRoomInfo);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.roomID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RoomType_t);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.roomNb);
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
            ((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.keysBindingSource)).EndInit();
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
        private System.Windows.Forms.TextBox roomNb;
        private System.Windows.Forms.ComboBox RoomType_t;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown roomID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button CreateRoomInfo;
        private System.Windows.Forms.TextBox prefabObjectPath;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox lockedNb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox BossNb;
        private System.Windows.Forms.BindingSource roomBindingSource;
        private System.Windows.Forms.BindingSource roomBindingSource1;
        private System.Windows.Forms.BindingSource keysBindingSource;
    }
}