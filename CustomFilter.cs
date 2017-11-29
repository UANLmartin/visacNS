using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NationalInstruments.Examples.VisaicNS
{
    /// <summary>
    /// CustomFilterForm lets the user enter the custom filter string to
    /// find the available resources with. Public property CustomFilter 
    /// returns the custom filter string.
    /// </summary>
    public class CustomFilterForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label customFilterStringLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox customFilterTextBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CustomFilterForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomFilterForm));
            this.customFilterTextBox = new System.Windows.Forms.TextBox();
            this.customFilterStringLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // customFilterTextBox
            // 
            this.customFilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.customFilterTextBox.Location = new System.Drawing.Point(16, 24);
            this.customFilterTextBox.Name = "customFilterTextBox";
            this.customFilterTextBox.Size = new System.Drawing.Size(152, 20);
            this.customFilterTextBox.TabIndex = 0;
            this.customFilterTextBox.Text = "?*";
            // 
            // customFilterStringLabel
            // 
            this.customFilterStringLabel.Location = new System.Drawing.Point(16, 8);
            this.customFilterStringLabel.Name = "customFilterStringLabel";
            this.customFilterStringLabel.Size = new System.Drawing.Size(144, 16);
            this.customFilterStringLabel.TabIndex = 1;
            this.customFilterStringLabel.Text = "Enter Custom Filter String:";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKButton.Location = new System.Drawing.Point(56, 56);
            this.OKButton.Name = "OKButton";
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CustomFilterForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(184, 78);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.customFilterStringLabel);
            this.Controls.Add(this.customFilterTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(384, 112);
            this.MinimumSize = new System.Drawing.Size(192, 112);
            this.Name = "CustomFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Filter";
            this.ResumeLayout(false);

        }
        #endregion

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public string CustomFilter
        {
            get
            {
                return customFilterTextBox.Text;
            }
        }

    }
}
