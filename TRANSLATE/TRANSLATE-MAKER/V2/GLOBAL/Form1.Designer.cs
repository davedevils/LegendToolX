namespace Ini_Mixer
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.security = new System.Windows.Forms.CheckBox();
            this.removeduplicate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(343, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 92);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(406, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Generate Translate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(325, 20);
            this.textBox1.TabIndex = 3;
            // 
            // security
            // 
            this.security.AutoSize = true;
            this.security.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.security.Location = new System.Drawing.Point(328, 48);
            this.security.Name = "security";
            this.security.Size = new System.Drawing.Size(90, 16);
            this.security.TabIndex = 6;
            this.security.Text = "Use Folder OUT";
            this.security.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.security.UseVisualStyleBackColor = true;
            this.security.CheckedChanged += new System.EventHandler(this.security_CheckedChanged);
            // 
            // removeduplicate
            // 
            this.removeduplicate.AutoSize = true;
            this.removeduplicate.Location = new System.Drawing.Point(208, 48);
            this.removeduplicate.Name = "removeduplicate";
            this.removeduplicate.Size = new System.Drawing.Size(114, 17);
            this.removeduplicate.TabIndex = 8;
            this.removeduplicate.Text = "Remove Duplicate";
            this.removeduplicate.UseVisualStyleBackColor = true;
            this.removeduplicate.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 138);
            this.Controls.Add(this.removeduplicate);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.security);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 177);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 177);
            this.Name = "Form1";
            this.Text = "Translator Extracter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox security;
        private System.Windows.Forms.CheckBox removeduplicate;
    }
}

