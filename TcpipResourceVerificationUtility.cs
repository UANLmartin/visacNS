//================================================================================================== 
// 
// Title      : TcpipResourceVerifiicationUtility.cs 
// Copyright  : National Instruments 2002. All Rights Reserved. 
// Author     : Mika Fukuchi 
// Purpose    : This application shows the user how to verify the existance of
//              TCP/IP resources. Property TcpipResourceNames is a string array that    
//              contains the verified TCP/IP resource names.
// 
//================================================================================================== 

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NationalInstruments.VisaNS;

namespace NationalInstruments.Examples.VisaicNS
{
    /// <summary>
    /// Summary description for TcpipResourceVerifiicationUtilityForm.
    /// </summary>
    public class TcpipResourceVerifiicationUtilityForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label hostAddrInstrumentLabel;
        private System.Windows.Forms.Label portNumberLabel;
        private System.Windows.Forms.Label hostAddrSocketLabel;
        private System.Windows.Forms.Label LANLabel;
        private System.Windows.Forms.GroupBox instrumentGroupBox;
        private System.Windows.Forms.GroupBox socketGroupBox;
        private System.Windows.Forms.Label TCPIPResourcesLabel;
        private System.Windows.Forms.ListBox resourcesVerifiedListBox;
        private System.Windows.Forms.TextBox hostAddressInstrTextBox;
        private System.Windows.Forms.TextBox deviceNameTextBox;
        private System.Windows.Forms.TextBox hostAddressSocketTextBox;
        private System.Windows.Forms.TextBox portNumberTextBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button verifyInstrButton;
        private System.Windows.Forms.Label hostBoardSocketLabel;
        private System.Windows.Forms.TextBox boardSocketTextBox;
        private System.Windows.Forms.Label boardInstrumentLabel;
        private System.Windows.Forms.Label optionalLabel;
        private System.Windows.Forms.TextBox boardInstrTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button verifySocketButton;

        public TcpipResourceVerifiicationUtilityForm()
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TcpipResourceVerifiicationUtilityForm));
            this.resourcesVerifiedListBox = new System.Windows.Forms.ListBox();
            this.hostAddressInstrTextBox = new System.Windows.Forms.TextBox();
            this.deviceNameTextBox = new System.Windows.Forms.TextBox();
            this.hostAddrInstrumentLabel = new System.Windows.Forms.Label();
            this.portNumberLabel = new System.Windows.Forms.Label();
            this.hostAddressSocketTextBox = new System.Windows.Forms.TextBox();
            this.portNumberTextBox = new System.Windows.Forms.TextBox();
            this.hostAddrSocketLabel = new System.Windows.Forms.Label();
            this.LANLabel = new System.Windows.Forms.Label();
            this.instrumentGroupBox = new System.Windows.Forms.GroupBox();
            this.optionalLabel = new System.Windows.Forms.Label();
            this.boardInstrTextBox = new System.Windows.Forms.TextBox();
            this.boardInstrumentLabel = new System.Windows.Forms.Label();
            this.verifyInstrButton = new System.Windows.Forms.Button();
            this.socketGroupBox = new System.Windows.Forms.GroupBox();
            this.boardSocketTextBox = new System.Windows.Forms.TextBox();
            this.hostBoardSocketLabel = new System.Windows.Forms.Label();
            this.verifySocketButton = new System.Windows.Forms.Button();
            this.TCPIPResourcesLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.instrumentGroupBox.SuspendLayout();
            this.socketGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourcesVerifiedListBox
            // 
            this.resourcesVerifiedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesVerifiedListBox.Location = new System.Drawing.Point(288, 48);
            this.resourcesVerifiedListBox.Name = "resourcesVerifiedListBox";
            this.resourcesVerifiedListBox.Size = new System.Drawing.Size(248, 251);
            this.resourcesVerifiedListBox.TabIndex = 0;
            // 
            // hostAddressInstrTextBox
            // 
            this.hostAddressInstrTextBox.Location = new System.Drawing.Point(112, 56);
            this.hostAddressInstrTextBox.Name = "hostAddressInstrTextBox";
            this.hostAddressInstrTextBox.Size = new System.Drawing.Size(128, 20);
            this.hostAddressInstrTextBox.TabIndex = 2;
            this.hostAddressInstrTextBox.Text = "";
            // 
            // deviceNameTextBox
            // 
            this.deviceNameTextBox.Location = new System.Drawing.Point(112, 88);
            this.deviceNameTextBox.Name = "deviceNameTextBox";
            this.deviceNameTextBox.Size = new System.Drawing.Size(128, 20);
            this.deviceNameTextBox.TabIndex = 3;
            this.deviceNameTextBox.Text = "";
            // 
            // hostAddrInstrumentLabel
            // 
            this.hostAddrInstrumentLabel.Location = new System.Drawing.Point(8, 56);
            this.hostAddrInstrumentLabel.Name = "hostAddrInstrumentLabel";
            this.hostAddrInstrumentLabel.Size = new System.Drawing.Size(80, 16);
            this.hostAddrInstrumentLabel.TabIndex = 4;
            this.hostAddrInstrumentLabel.Text = "Host Address:";
            // 
            // portNumberLabel
            // 
            this.portNumberLabel.Location = new System.Drawing.Point(8, 88);
            this.portNumberLabel.Name = "portNumberLabel";
            this.portNumberLabel.Size = new System.Drawing.Size(72, 16);
            this.portNumberLabel.TabIndex = 5;
            this.portNumberLabel.Text = "Port Number:";
            // 
            // hostAddressSocketTextBox
            // 
            this.hostAddressSocketTextBox.Location = new System.Drawing.Point(112, 56);
            this.hostAddressSocketTextBox.Name = "hostAddressSocketTextBox";
            this.hostAddressSocketTextBox.Size = new System.Drawing.Size(128, 20);
            this.hostAddressSocketTextBox.TabIndex = 6;
            this.hostAddressSocketTextBox.Text = "";
            // 
            // portNumberTextBox
            // 
            this.portNumberTextBox.Location = new System.Drawing.Point(112, 88);
            this.portNumberTextBox.Name = "portNumberTextBox";
            this.portNumberTextBox.Size = new System.Drawing.Size(128, 20);
            this.portNumberTextBox.TabIndex = 7;
            this.portNumberTextBox.Text = "";
            // 
            // hostAddrSocketLabel
            // 
            this.hostAddrSocketLabel.Location = new System.Drawing.Point(8, 56);
            this.hostAddrSocketLabel.Name = "hostAddrSocketLabel";
            this.hostAddrSocketLabel.Size = new System.Drawing.Size(88, 24);
            this.hostAddrSocketLabel.TabIndex = 8;
            this.hostAddrSocketLabel.Text = "Host Address:";
            // 
            // LANLabel
            // 
            this.LANLabel.Location = new System.Drawing.Point(8, 88);
            this.LANLabel.Name = "LANLabel";
            this.LANLabel.Size = new System.Drawing.Size(104, 16);
            this.LANLabel.TabIndex = 9;
            this.LANLabel.Text = "LAN Device Name";
            // 
            // instrumentGroupBox
            // 
            this.instrumentGroupBox.Controls.Add(this.optionalLabel);
            this.instrumentGroupBox.Controls.Add(this.boardInstrTextBox);
            this.instrumentGroupBox.Controls.Add(this.boardInstrumentLabel);
            this.instrumentGroupBox.Controls.Add(this.verifyInstrButton);
            this.instrumentGroupBox.Controls.Add(this.deviceNameTextBox);
            this.instrumentGroupBox.Controls.Add(this.LANLabel);
            this.instrumentGroupBox.Controls.Add(this.hostAddrInstrumentLabel);
            this.instrumentGroupBox.Controls.Add(this.hostAddressInstrTextBox);
            this.instrumentGroupBox.Location = new System.Drawing.Point(16, 16);
            this.instrumentGroupBox.Name = "instrumentGroupBox";
            this.instrumentGroupBox.Size = new System.Drawing.Size(256, 152);
            this.instrumentGroupBox.TabIndex = 10;
            this.instrumentGroupBox.TabStop = false;
            this.instrumentGroupBox.Text = "TCP/IP Instrument Resource";
            // 
            // optionalLabel
            // 
            this.optionalLabel.Location = new System.Drawing.Point(8, 104);
            this.optionalLabel.Name = "optionalLabel";
            this.optionalLabel.Size = new System.Drawing.Size(56, 16);
            this.optionalLabel.TabIndex = 17;
            this.optionalLabel.Text = "(optional):";
            // 
            // boardInstrTextBox
            // 
            this.boardInstrTextBox.Location = new System.Drawing.Point(112, 24);
            this.boardInstrTextBox.Name = "boardInstrTextBox";
            this.boardInstrTextBox.Size = new System.Drawing.Size(128, 20);
            this.boardInstrTextBox.TabIndex = 16;
            this.boardInstrTextBox.Text = "";
            // 
            // boardInstrumentLabel
            // 
            this.boardInstrumentLabel.Location = new System.Drawing.Point(8, 24);
            this.boardInstrumentLabel.Name = "boardInstrumentLabel";
            this.boardInstrumentLabel.Size = new System.Drawing.Size(88, 16);
            this.boardInstrumentLabel.TabIndex = 15;
            this.boardInstrumentLabel.Text = "Board (optional):";
            // 
            // verifyInstrButton
            // 
            this.verifyInstrButton.Location = new System.Drawing.Point(96, 120);
            this.verifyInstrButton.Name = "verifyInstrButton";
            this.verifyInstrButton.TabIndex = 13;
            this.verifyInstrButton.Text = "Verify";
            this.verifyInstrButton.Click += new System.EventHandler(this.verifyInstrButton_Click);
            // 
            // socketGroupBox
            // 
            this.socketGroupBox.Controls.Add(this.boardSocketTextBox);
            this.socketGroupBox.Controls.Add(this.hostBoardSocketLabel);
            this.socketGroupBox.Controls.Add(this.verifySocketButton);
            this.socketGroupBox.Controls.Add(this.hostAddressSocketTextBox);
            this.socketGroupBox.Controls.Add(this.portNumberTextBox);
            this.socketGroupBox.Controls.Add(this.portNumberLabel);
            this.socketGroupBox.Controls.Add(this.hostAddrSocketLabel);
            this.socketGroupBox.Location = new System.Drawing.Point(16, 184);
            this.socketGroupBox.Name = "socketGroupBox";
            this.socketGroupBox.Size = new System.Drawing.Size(256, 152);
            this.socketGroupBox.TabIndex = 11;
            this.socketGroupBox.TabStop = false;
            this.socketGroupBox.Text = "TCP/IP Socket Resource";
            // 
            // boardSocketTextBox
            // 
            this.boardSocketTextBox.Location = new System.Drawing.Point(112, 24);
            this.boardSocketTextBox.Name = "boardSocketTextBox";
            this.boardSocketTextBox.Size = new System.Drawing.Size(128, 20);
            this.boardSocketTextBox.TabIndex = 10;
            this.boardSocketTextBox.Text = "";
            // 
            // hostBoardSocketLabel
            // 
            this.hostBoardSocketLabel.Location = new System.Drawing.Point(8, 24);
            this.hostBoardSocketLabel.Name = "hostBoardSocketLabel";
            this.hostBoardSocketLabel.Size = new System.Drawing.Size(88, 16);
            this.hostBoardSocketLabel.TabIndex = 9;
            this.hostBoardSocketLabel.Text = "Board (optional):";
            // 
            // verifySocketButton
            // 
            this.verifySocketButton.Location = new System.Drawing.Point(96, 120);
            this.verifySocketButton.Name = "verifySocketButton";
            this.verifySocketButton.TabIndex = 0;
            this.verifySocketButton.Text = "Verify";
            this.verifySocketButton.Click += new System.EventHandler(this.verifySocketButton_Click);
            // 
            // TCPIPResourcesLabel
            // 
            this.TCPIPResourcesLabel.Location = new System.Drawing.Point(288, 24);
            this.TCPIPResourcesLabel.Name = "TCPIPResourcesLabel";
            this.TCPIPResourcesLabel.Size = new System.Drawing.Size(144, 16);
            this.TCPIPResourcesLabel.TabIndex = 12;
            this.TCPIPResourcesLabel.Text = "TCP/IP Resources Verified:";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(464, 312);
            this.OKButton.Name = "OKButton";
            this.OKButton.TabIndex = 13;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // TcpipResourceVerifiicationUtilityForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 357);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.TCPIPResourcesLabel);
            this.Controls.Add(this.resourcesVerifiedListBox);
            this.Controls.Add(this.instrumentGroupBox);
            this.Controls.Add(this.socketGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 328);
            this.Name = "TcpipResourceVerifiicationUtilityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TCP/IP Resource Verification Utility";
            this.instrumentGroupBox.ResumeLayout(false);
            this.socketGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void verifyInstrButton_Click(object sender, System.EventArgs e)
        {
            string resourceName;
            
            if (boardInstrTextBox.TextLength == 0)
            {
                resourceName = "TCPIP::" + hostAddressInstrTextBox.Text;
            }
            else
            {
                resourceName = "TCPIP" + boardInstrTextBox.Text + "::" + hostAddressInstrTextBox.Text;
            }
 
            if (deviceNameTextBox.TextLength == 0)
            {
                resourceName += "::INSTR";
            }
            else
            {
                resourceName += "::" + deviceNameTextBox.Text + "::INSTR";
            }

            VerifyAndUpdateResourcename(resourceName);
        }

        private void verifySocketButton_Click(object sender, System.EventArgs e)
        {
            string resourceName;
            
            if (boardSocketTextBox.TextLength == 0)
            {
                resourceName = "TCPIP::" + hostAddressSocketTextBox.Text;
            }
            else
            {
                resourceName = "TCPIP" + boardSocketTextBox.Text + "::" + hostAddressSocketTextBox.Text;
            }
    
            resourceName += "::" + portNumberTextBox.Text + "::SOCKET";

            VerifyAndUpdateResourcename(resourceName);
        }

        private void VerifyAndUpdateResourcename(string resourceName)
        {
            string resourceFullName = ValidResourceName(resourceName);
            if (resourceFullName != null)
            {
                if (!resourcesVerifiedListBox.Items.Contains(resourceFullName))
                {
                    resourcesVerifiedListBox.Items.Add(resourceFullName);
                }
            }
            else
            {
                MessageBox.Show("Invalid Resource Name");
            }
        }

        // Returns the full name of the resource if it is valid. If it's
        // an invalid resource name, it returns null.
        private string ValidResourceName(string resourceName)
        {
            Session session = null;
            string fullName = null;
            try
            {
                session = ResourceManager.GetLocalManager().Open(resourceName);
                fullName = session.ResourceName;
            }
            catch (VisaException)
            {
                // Don't do anything
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (session != null)
            {
                session.Dispose();
            }

            return fullName;
        }

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public string[] TcpipResourceNames
        {
            get
            {
                string[] resourceNames = new String[resourcesVerifiedListBox.Items.Count];
                resourcesVerifiedListBox.Items.CopyTo(resourceNames, 0);
                return resourceNames;
            }
        }
    }   
}
