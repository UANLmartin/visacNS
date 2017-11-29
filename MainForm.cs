//================================================================================================== 
// 
// Title      : MainForm.cs 
// Purpose    : This program is much like VISAIC (VISA Interactive Control) that currently 
//				ships with NI-VISA, but it would use our .NET Interface to NI-VISA.  
//				This application takes an advantage of reflection to discover what is available on 
//				VISA.NET APIs. This program provides a simple UI to let the user of the example program 
//				interactively tweak properties and call methods on the API.  
// 
//================================================================================================== 

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NationalInstruments.VisaNS;

namespace NationalInstruments.Examples.VisaicNS
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private Session session;
		private System.Windows.Forms.TextBox getTextBox;
		private System.Windows.Forms.Button setButton;
		private System.Windows.Forms.Button getButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button registerButton;
		private System.Windows.Forms.Button unregisterButton;
		private System.Windows.Forms.Button invokeButton;

		struct ControlProperties
		{
			internal int pointX;
			internal int pointY;
			internal int sizeX;
			internal int sizeY;
			internal int tabIndex;
			internal string name;
		}
 
        private int prevLeftPanelWidth;
        private ControlProperties ctrlProperties;
		private System.Windows.Forms.Label returnTypeLabel;
		private System.Windows.Forms.TextBox returnTypeTextBox;
		private System.Windows.Forms.TextBox propertyTypeTextBox;
		private System.Windows.Forms.Button cleanStatusButton;
		private System.Windows.Forms.Button clearResponseButton;
		private System.Windows.Forms.TextBox eventFeedbackTextBox; // will be dynamically created

		// Used only for Public Properties
		private System.Windows.Forms.Control setControl;
		
		// Used only for Public Methods
		private ArrayList paramCtrlSet; // will be dynamically created
		private ArrayList paramLabelCtrlSet; // will be dynamically created
		private Control returnValueControl; // will be dynamically created
		private System.Windows.Forms.Button executeButton; // will be dynamically created
		private System.Windows.Forms.Label feedbackLabel;
		private const int SPACE_BETWEEN_CTRLS = 24;
		private const int HEIGHT_CTRL = 20;
		private const int LABEL_HEIGHT = 16;
		private const int BUTTON_WIDTH = 75;
		private const int BUTTON_HEIGHT = 25;
        private const int OFFSET = 16;

        // Regular expression that matches integers
        private static readonly string IntegerPattern = @"([+-]?\d+)";
        private static readonly Regex IntegerMatcher;

        // Stores the last IAsyncResult returned from a function
        private IAsyncResult lastAsyncResult = null;
				
		private GpibInterfaceControllerInChargeEventHandler GpibCICEventHandler;
		private VxiSessionSignalProcessorEventHandler SignalProcessorEventHandler;
		private GpibInterfaceTriggerEventHandler GITriggerEventHandler;
        private VxiBackplaneTriggerEventHandler VBTriggerEventHandler;
        private VxiSessionTriggerEventHandler VSTriggerEventHandler;
		private VisaEventHandler VisaEHandler;
        private GpibInterfaceEventHandler GIEventHandler;
        private VxiBackplaneEventHandler VBEventHandler;
        private MessageBasedSessionEventHandler MbsEventHandler;
        private SerialSessionEventHandler SSEventHandler;
        private VxiSessionEventHandler VSEventHandler;
        private PxiSessionEventHandler PSEventHandler;
        private VxiSessionVxiVmeInterruptEventHandler VxiVmeInterruptEventHandler;
        private UsbRawEventHandler UsbREventHandler;
        private UsbRawInterruptEventHandler UsbRIEventHandler;
        private UsbSessionEventHandler UsbSEventHandler;
        private UsbSessionInterruptEventHandler UsbSIEventHandler;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label propertyTypeLabel;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.Label responseLabel;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.TreeView sessionTreeView;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Splitter formSplitter;
        internal System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.Panel rightPanel;

        static MainForm()
        {
            IntegerMatcher = new Regex(IntegerPattern);
        }
        
        public MainForm()
		{
            //
            // Required for Windows Form Designer support
            //
			InitializeComponent();
		} 

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            // First, get the resource name of the session to open, and open it in ResourceListForm
            ResourceListForm rlf = new ResourceListForm();
            rlf.ShowDialog();
            session = rlf.OpenedSession;
            
            if(session != null)
            {
                PopulateSessionTreeView();
                prevLeftPanelWidth = leftPanel.Size.Width;
            }
            else
            {
                this.Close();
            }
        }
				
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				if (session != null)
				{
					session.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.propertyTypeTextBox = new System.Windows.Forms.TextBox();
            this.propertyTypeLabel = new System.Windows.Forms.Label();
            this.getTextBox = new System.Windows.Forms.TextBox();
            this.setButton = new System.Windows.Forms.Button();
            this.getButton = new System.Windows.Forms.Button();
            this.valueLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.returnTypeTextBox = new System.Windows.Forms.TextBox();
            this.returnTypeLabel = new System.Windows.Forms.Label();
            this.invokeButton = new System.Windows.Forms.Button();
            this.unregisterButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.eventFeedbackTextBox = new System.Windows.Forms.TextBox();
            this.cleanStatusButton = new System.Windows.Forms.Button();
            this.clearResponseButton = new System.Windows.Forms.Button();
            this.responseLabel = new System.Windows.Forms.Label();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.sessionTreeView = new System.Windows.Forms.TreeView();
            this.formSplitter = new System.Windows.Forms.Splitter();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.promptLabel = new System.Windows.Forms.Label();
            this.leftPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyTypeTextBox
            // 
            this.propertyTypeTextBox.Location = new System.Drawing.Point(96, 8);
            this.propertyTypeTextBox.Name = "propertyTypeTextBox";
            this.propertyTypeTextBox.ReadOnly = true;
            this.propertyTypeTextBox.Size = new System.Drawing.Size(144, 20);
            this.propertyTypeTextBox.TabIndex = 8;
            this.propertyTypeTextBox.Text = "";
            this.propertyTypeTextBox.Visible = false;
            // 
            // propertyTypeLabel
            // 
            this.propertyTypeLabel.Location = new System.Drawing.Point(8, 8);
            this.propertyTypeLabel.Name = "propertyTypeLabel";
            this.propertyTypeLabel.Size = new System.Drawing.Size(80, 16);
            this.propertyTypeLabel.TabIndex = 7;
            this.propertyTypeLabel.Text = "Property Type:";
            this.propertyTypeLabel.Visible = false;
            // 
            // getTextBox
            // 
            this.getTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.getTextBox.Location = new System.Drawing.Point(96, 56);
            this.getTextBox.Name = "getTextBox";
            this.getTextBox.ReadOnly = true;
            this.getTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.getTextBox.Size = new System.Drawing.Size(212, 20);
            this.getTextBox.TabIndex = 4;
            this.getTextBox.Text = "";
            this.getTextBox.Visible = false;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(8, 96);
            this.setButton.Name = "setButton";
            this.setButton.TabIndex = 3;
            this.setButton.Text = "Set";
            this.setButton.Visible = false;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // getButton
            // 
            this.getButton.Location = new System.Drawing.Point(8, 56);
            this.getButton.Name = "getButton";
            this.getButton.TabIndex = 2;
            this.getButton.Text = "Get";
            this.getButton.Visible = false;
            this.getButton.Click += new System.EventHandler(this.getButton_Click);
            // 
            // valueLabel
            // 
            this.valueLabel.Location = new System.Drawing.Point(48, 59);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(40, 16);
            this.valueLabel.TabIndex = 10;
            this.valueLabel.Text = "Value:";
            this.valueLabel.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.Location = new System.Drawing.Point(8, 392);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(48, 16);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Status:";
            // 
            // returnTypeTextBox
            // 
            this.returnTypeTextBox.Location = new System.Drawing.Point(96, 8);
            this.returnTypeTextBox.Name = "returnTypeTextBox";
            this.returnTypeTextBox.ReadOnly = true;
            this.returnTypeTextBox.Size = new System.Drawing.Size(212, 20);
            this.returnTypeTextBox.TabIndex = 10;
            this.returnTypeTextBox.Text = "";
            this.returnTypeTextBox.Visible = false;
            // 
            // returnTypeLabel
            // 
            this.returnTypeLabel.Location = new System.Drawing.Point(8, 8);
            this.returnTypeLabel.Name = "returnTypeLabel";
            this.returnTypeLabel.Size = new System.Drawing.Size(72, 21);
            this.returnTypeLabel.TabIndex = 9;
            this.returnTypeLabel.Text = "Return Type:";
            this.returnTypeLabel.Visible = false;
            // 
            // invokeButton
            // 
            this.invokeButton.Location = new System.Drawing.Point(8, 40);
            this.invokeButton.Name = "invokeButton";
            this.invokeButton.TabIndex = 8;
            this.invokeButton.Text = "Invoke";
            this.invokeButton.Visible = false;
            this.invokeButton.Click += new System.EventHandler(this.invokeButton_Click);
            // 
            // unregisterButton
            // 
            this.unregisterButton.Location = new System.Drawing.Point(96, 8);
            this.unregisterButton.Name = "unregisterButton";
            this.unregisterButton.TabIndex = 2;
            this.unregisterButton.Text = "Unregister";
            this.unregisterButton.Visible = false;
            this.unregisterButton.Click += new System.EventHandler(this.unregisterButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(8, 8);
            this.registerButton.Name = "registerButton";
            this.registerButton.TabIndex = 1;
            this.registerButton.Text = "Register";
            this.registerButton.Visible = false;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // eventFeedbackTextBox
            // 
            this.eventFeedbackTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.eventFeedbackTextBox.Location = new System.Drawing.Point(16, 496);
            this.eventFeedbackTextBox.Multiline = true;
            this.eventFeedbackTextBox.Name = "eventFeedbackTextBox";
            this.eventFeedbackTextBox.ReadOnly = true;
            this.eventFeedbackTextBox.Size = new System.Drawing.Size(176, 54);
            this.eventFeedbackTextBox.TabIndex = 3;
            this.eventFeedbackTextBox.Text = "";
            // 
            // cleanStatusButton
            // 
            this.cleanStatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cleanStatusButton.Location = new System.Drawing.Point(200, 424);
            this.cleanStatusButton.Name = "cleanStatusButton";
            this.cleanStatusButton.Size = new System.Drawing.Size(96, 23);
            this.cleanStatusButton.TabIndex = 11;
            this.cleanStatusButton.Text = "Clear Status";
            this.cleanStatusButton.Click += new System.EventHandler(this.cleanStatusButton_Click);
            // 
            // clearResponseButton
            // 
            this.clearResponseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearResponseButton.Location = new System.Drawing.Point(200, 504);
            this.clearResponseButton.Name = "clearResponseButton";
            this.clearResponseButton.Size = new System.Drawing.Size(96, 23);
            this.clearResponseButton.TabIndex = 12;
            this.clearResponseButton.Text = "Clear Response";
            this.clearResponseButton.Click += new System.EventHandler(this.clearResponseButton_Click);
            // 
            // responseLabel
            // 
            this.responseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.responseLabel.Location = new System.Drawing.Point(8, 472);
            this.responseLabel.Name = "responseLabel";
            this.responseLabel.Size = new System.Drawing.Size(160, 16);
            this.responseLabel.TabIndex = 9;
            this.responseLabel.Text = "Response from Event Handler:";
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(16, 408);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(176, 56);
            this.statusTextBox.TabIndex = 10;
            this.statusTextBox.Text = "";
            // 
            // sessionTreeView
            // 
            this.sessionTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionTreeView.ImageIndex = -1;
            this.sessionTreeView.Location = new System.Drawing.Point(16, 16);
            this.sessionTreeView.Name = "sessionTreeView";
            this.sessionTreeView.SelectedImageIndex = -1;
            this.sessionTreeView.Size = new System.Drawing.Size(296, 368);
            this.sessionTreeView.TabIndex = 13;
            this.sessionTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.sessionTree_AfterCollapse);
            this.sessionTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sessionTreeView_AfterSelect);
            // 
            // formSplitter
            // 
            this.formSplitter.Location = new System.Drawing.Point(312, 0);
            this.formSplitter.MinExtra = 50;
            this.formSplitter.MinSize = 50;
            this.formSplitter.Name = "formSplitter";
            this.formSplitter.Size = new System.Drawing.Size(4, 566);
            this.formSplitter.TabIndex = 14;
            this.formSplitter.TabStop = false;
            this.formSplitter.LocationChanged += new System.EventHandler(this.formSplitter_LocationChanged);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.eventFeedbackTextBox);
            this.leftPanel.Controls.Add(this.clearResponseButton);
            this.leftPanel.Controls.Add(this.cleanStatusButton);
            this.leftPanel.Controls.Add(this.statusTextBox);
            this.leftPanel.Controls.Add(this.statusLabel);
            this.leftPanel.Controls.Add(this.responseLabel);
            this.leftPanel.Controls.Add(this.sessionTreeView);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(312, 566);
            this.leftPanel.TabIndex = 15;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.unregisterButton);
            this.rightPanel.Controls.Add(this.propertyTypeLabel);
            this.rightPanel.Controls.Add(this.returnTypeLabel);
            this.rightPanel.Controls.Add(this.returnTypeTextBox);
            this.rightPanel.Controls.Add(this.setButton);
            this.rightPanel.Controls.Add(this.registerButton);
            this.rightPanel.Controls.Add(this.propertyTypeTextBox);
            this.rightPanel.Controls.Add(this.getTextBox);
            this.rightPanel.Controls.Add(this.invokeButton);
            this.rightPanel.Controls.Add(this.getButton);
            this.rightPanel.Controls.Add(this.valueLabel);
            this.rightPanel.Controls.Add(this.promptLabel);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(316, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(324, 566);
            this.rightPanel.TabIndex = 16;
            // 
            // promptLabel
            // 
            this.promptLabel.Location = new System.Drawing.Point(16, 192);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(296, 32);
            this.promptLabel.TabIndex = 17;
            this.promptLabel.Text = "Please select a property, method, or event you would like to access on the left s" +
                "ide.";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 566);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.formSplitter);
            this.Controls.Add(this.leftPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(648, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VISA Interactive Control for .NET";
            this.SizeChanged += new System.EventHandler(this.MainFormSizeChanges);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.leftPanel.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private string ReplaceCommonEscapeSequences(string s)
		{
			return s.Replace("\\n", "\n").Replace("\\r", "\r");
		}

		private string InsertCommonEscapeSequences(string s)
		{
			return s.Replace("\n", "\\n").Replace("\r", "\\r");
		}

        private byte[] ParseByteArray(string s)
        {
            MatchCollection matches = IntegerMatcher.Matches(s);
            byte[] parsedArray = new byte[matches.Count];
            for(int i=0; i<matches.Count; i++)
            {
                parsedArray[i] = Byte.Parse(matches[i].Value);
            }
            return parsedArray;
        }

        private string ByteArrayToString(byte[] byteArray)
        {
            if(byteArray == null || byteArray.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder byteArraySB = new StringBuilder();
            for(int i=0; i<byteArray.Length; i++)
            {
                if(i > 0)
                {
                    byteArraySB.Append(" ");
                }
                byteArraySB.Append(byteArray[i].ToString());
            }
            return byteArraySB.ToString();
        }

        private void PopulateSessionTreeView()
        {
            sessionTreeView.Nodes.Clear();

            TreeNode sessionNode = new TreeNode(session.ResourceName);
            sessionTreeView.Nodes.Add(sessionNode);

            TreeNode propertyNode = new TreeNode("Public Properties");
            PopulatePublicProperties(propertyNode);
            sessionNode.Nodes.Add(propertyNode);

            TreeNode methodNode = new TreeNode("Public Methods");
            PopulatePublicMethods(methodNode);
            sessionNode.Nodes.Add(methodNode);
            
            TreeNode eventNode = new TreeNode("Events");
            PopulateEvents(eventNode);
            sessionNode.Nodes.Add(eventNode);

            sessionNode.Expand();
        }
                       
        private int GetCtrlWidth()
        {
            return this.Size.Width-(sessionTreeView.Size.Width+OFFSET*3);
        }
        
        private void formSplitter_LocationChanged(object sender, System.EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(Size.Width+(leftPanel.Size.Width-prevLeftPanelWidth), this.MinimumSize.Height);    
            prevLeftPanelWidth = leftPanel.Size.Width;this.ResizeRedraw = true; this.Refresh();
        }
        
        private void sessionTree_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {
                if (sessionTreeView.SelectedNode.Parent.Text == "Public Properties")
                {
                    ShowPublicPropertiesCtrls(false);
                }
                else if (sessionTreeView.SelectedNode.Parent.Text == "Public Methods")
                {
                    ShowPublicMethodsCtrls(false);
                } 
                else if (sessionTreeView.SelectedNode.Parent.Text == "Events")
                {
                    ShowEventsCtrls(false);
                }
                else
                {
                    ShowPublicPropertiesCtrls(false);
                    ShowPublicMethodsCtrls(false);
                    ShowEventsCtrls(false);
                }
            }
            catch(Exception ex) // sesssion node was collapsed
            {
                GetInnerException(ex);
            }
        }

        private void sessionTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            statusTextBox.Text = "";
            promptLabel.Visible = false;
            try
            {
                if (sessionTreeView.SelectedNode.Parent == null)
                    return;
                
                if (sessionTreeView.SelectedNode.Parent.Text == "Public Properties")
                {
                    PublicPropertySelected();
                }
                else if (sessionTreeView.SelectedNode.Parent.Text == "Public Methods")
                {
                    PublicMethodSelected();
                }
                else if (sessionTreeView.SelectedNode.Parent.Text == "Events")
                {
                    EventSelected();
                } 
                else // propertyNode, methodNode, or eventNode is selected
                {
                    ShowPublicPropertiesCtrls(false);
                    ShowPublicMethodsCtrls(false);
                    ShowEventsCtrls(false);
                }

                statusTextBox.Text = session.LastStatus.ToString();
            }
            catch(Exception ex)
            {
                GetInnerException(ex);
            }
        }
     
        private void SetNewControlProperties(Control ctrl)
		{
			ctrl.Location = new System.Drawing.Point(ctrlProperties.pointX, ctrlProperties.pointY);
			ctrl.Size = new System.Drawing.Size(ctrlProperties.sizeX, ctrlProperties.sizeY);
			ctrl.TabIndex = ctrlProperties.tabIndex;
			ctrl.Name = ctrlProperties.name;
		}
		
		// convert the string in the control to the type specified
		private Object ConvertFromCtrlToType(Control ctrl, Type type)
		{
			Object newValue = new object();
            if (type.IsEnum)
            {
                newValue = Enum.Parse(type, ctrl.Text);
            }
            else if (type == typeof(string))
            {
                newValue = ReplaceCommonEscapeSequences(ctrl.Text);
            }
            else if (type == typeof(byte[]))
            {
                newValue = ParseByteArray(ctrl.Text);
            }
            else if (type == typeof(IAsyncResult))
            {
                newValue = lastAsyncResult;
            }
            else
            {
                Object[] parameter = new Object[1]; // parameter for Parse method
                parameter[0] = ctrl.Text;
                newValue = type.InvokeMember("Parse", BindingFlags.InvokeMethod, null, null, parameter);
            }
			return newValue;
		}

		private void CreateCtrl(Type type, out Control ctrl)
		{
			if (type.Namespace.StartsWith("NationalInstruments.VisaNS") // those are enums defined in NationalInstruments.VisaNS
				|| type == typeof(bool))
			{
				CreateComboBox(type, out ctrl);
			}
			else if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(byte) 
                || type == typeof(IntPtr))
			{
				CreateNumericUpDown(type, out ctrl);
			}			
			else // just create a text control
			{
				CreateTextControl(out ctrl);
			}
 		}
		
		private void CreateComboBox(Type type, out Control ctrl)
		{
			// create combo box dynamically
			ctrl = new System.Windows.Forms.ComboBox();
			SetNewControlProperties(ctrl);
	
			// populate it with enum consts
			if (type.IsEnum)
			{
				string[] enumNames = Enum.GetNames(type);
				foreach (string enumName in enumNames)
				{
					((ComboBox)ctrl).Items.Add(enumName);
				}
			}
			else if (type == typeof(bool))
			{
				((ComboBox)ctrl).Items.Add("False");
				((ComboBox)ctrl).Items.Add("True");
			}
			else
			{
				statusTextBox.Text = "The property type is not enum, boolean, or number. Unable to fill the combo box.";
				return;
			}
			((ComboBox)ctrl).SelectedIndex = 0;
			((ComboBox)ctrl).DropDownStyle = ComboBoxStyle.DropDownList;
		}

		private void CreateNumericUpDown(Type type, out Control ctrl)
		{
			// create NumericUpDown dynamically
			ctrl = new System.Windows.Forms.NumericUpDown();
			SetNewControlProperties(ctrl);
			if (type == typeof(IntPtr))
			{
				((NumericUpDown)ctrl).Minimum = 0;
				((NumericUpDown)ctrl).Maximum = IntPtr.Size;
			}
			else if(type == typeof(byte)) // cannot convert object to int if type is Byte
			{
				((NumericUpDown)ctrl).Minimum = Byte.MinValue;
				((NumericUpDown)ctrl).Maximum = Byte.MaxValue;
			}
			else // Int16, or Int 32
			{
				((NumericUpDown)ctrl).Minimum = (int)type.GetField("MinValue").GetValue(null);
				((NumericUpDown)ctrl).Maximum = (int)type.GetField("MaxValue").GetValue(null);
			}
		}

		private void CreateTextControl(out Control ctrl)
		{
			// create text control dynamically
			ctrl = new System.Windows.Forms.TextBox();
			SetNewControlProperties(ctrl);
			((TextBox)ctrl).ScrollBars = ScrollBars.Horizontal;
		}

		private void GetInnerException(Exception e)
		{
			Exception prev = e, current = e.InnerException;
			while (current != null)
			{
				prev = current;
				current = current.InnerException;
			}
			statusTextBox.Text = prev.Message;
		}

		private void cleanStatusButton_Click(object sender, System.EventArgs e)
		{
			statusTextBox.Text = "";
		}

		private void clearResponseButton_Click(object sender, System.EventArgs e)
		{
			eventFeedbackTextBox.Text = "";
		}

		//*************** Methods for Public Properties ***************//
		#region Methods for Public Properties

		// This class implements IComparer so that PropertyInfo can be sorted in an alphabetical order for display.
		class PropertyInfoComparer : IComparer
		{
			public int Compare(object ob1, object ob2)
			{
				PropertyInfo pi1 = ob1 as PropertyInfo;
				PropertyInfo pi2 = ob2 as PropertyInfo;
				if (pi1 != null && pi2 != null)
				{
					return pi1.Name.CompareTo(pi2.Name);
				}
				else
				{
					MessageBox.Show("Compare function on PropertyInfo failed!!");
					return 0;
				}
			}
		}

		private void MainFormSizeChanges(object sender, System.EventArgs e)
		{
			ctrlProperties.pointY = setButton.Location.Y;
			ctrlProperties.sizeX = getTextBox.Size.Width; // size of setControl
		}

        private void PopulatePublicProperties(TreeNode parent)
        {
            try
            {
                PropertyInfo[] prpInfoArray = session.GetType().GetProperties();
                Array.Sort(prpInfoArray, new PropertyInfoComparer());
                foreach (PropertyInfo pi in prpInfoArray)
                {
                    TreeNode tn = new TreeNode(pi.Name);
                    tn.Tag = pi;
                    parent.Nodes.Add(tn);
                }
                statusTextBox.Text = session.LastStatus.ToString();
            }
            catch (Exception ex)
            {
                GetInnerException(ex);
            }        
        }
        
        private void PublicPropertySelected() 
        {
            ShowPublicPropertiesCtrls(true);
            ShowPublicMethodsCtrls(false);
            ShowEventsCtrls(false);

            SetControlSetProperties();
            PropertyInfo propertyInfo = (PropertyInfo)sessionTreeView.SelectedNode.Tag;
            CreateControlForType(propertyInfo);
            propertyTypeTextBox.Text = propertyInfo.PropertyType.Name.ToString();
            getButton_Click(null, null);
        }

        private void SetControlSetProperties() 
        {
            ctrlProperties.pointX = getTextBox.Location.X; // position of setControl
            ctrlProperties.pointY = setButton.Location.Y;
            ctrlProperties.sizeX = getTextBox.Size.Width; // size of setControl
            ctrlProperties.sizeY = getTextBox.Size.Height;
            ctrlProperties.tabIndex = 5;
            ctrlProperties.name = "ctrlPublicProperties";
        }

        private void ShowPublicPropertiesCtrls(bool show) 
        {
            getButton.Visible = show;
            setButton.Visible = show;
            getTextBox.Visible = show;
            propertyTypeTextBox.Visible = show;
            propertyTypeLabel.Visible = show;
            valueLabel.Visible = show;
        
            if (setControl != null)
            {
                rightPanel.Controls.Remove(setControl);
                setControl = null;
            }
        }
        
        private void PublicMethodSelected() // move m
        {
            ShowPublicPropertiesCtrls(false);
            ShowPublicMethodsCtrls(true);
            ShowEventsCtrls(false);
        }
        
        private void getButton_Click(object sender, System.EventArgs e)
		{
			statusTextBox.Text = "";
			try
			{
				PropertyInfo propertyInfo = (PropertyInfo)sessionTreeView.SelectedNode.Tag;
				getTextBox.Text = propertyInfo.GetValue(session, null).ToString();
				statusTextBox.Text = session.LastStatus.ToString();
			}
			catch (Exception ex)
			{
				getTextBox.Text = "";
				GetInnerException(ex);
			}
            sessionTreeView.Focus();
		}

		private void setButton_Click(object sender, System.EventArgs e)
		{
			statusTextBox.Text = "";
			try
			{
                PropertyInfo propertyInfo = (PropertyInfo)sessionTreeView.SelectedNode.Tag;

				// dynamically get the type of property, and invoke Parse method on it.
				Type type = propertyInfo.GetValue(session, null).GetType();
				Object newVal = ConvertFromCtrlToType(setControl, type);
				propertyInfo.SetValue(session, newVal, null); 
				statusTextBox.Text = session.LastStatus.ToString();
			}
			catch (Exception ex)
			{
				GetInnerException(ex);
			}
            sessionTreeView.Focus();
		}

		private void CreateControlForType(PropertyInfo propertyInfo)
		{
			SuspendLayout();
			rightPanel.Controls.Remove(setControl);
            setControl = null;
			Type type = propertyInfo.GetValue(session, null).GetType(); // dynamically get the type of property

			if (!HideControls(propertyInfo)) // property is not readonly
			{
				CreateCtrl(type, out setControl);
				this.setControl.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right);
                rightPanel.Controls.Add(setControl);
            } 

            ResumeLayout(false);
		}

		bool HideControls(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.CanWrite) //  readonly
			{
				setButton.Hide();
				getButton.Hide();
				return true;
			}
			else
			{
				setButton.Show();
				getButton.Show();
				return false;
			}		
		}

		#endregion

		//*************** Methods for publicMethodsTabPage ***************//
		#region Methods for publicMethodsTabPage

		// This class implements IComparer so that MethodInfo can be sorted in an alphabetical order for display.
		class MethodInfoComparer : IComparer
		{
			public int Compare(object ob1, object ob2)
			{
				MethodInfo mi1 = ob1 as MethodInfo;
				MethodInfo mi2 = ob2 as MethodInfo;
				if (mi1 != null && mi2 != null)
				{
					return GetMethodSignature(mi1).CompareTo(GetMethodSignature(mi2));
				}
				else
				{
					MessageBox.Show("Compare function on MethodInfo failed!!");
					return 0;
				}
			}		
		}

        // This method returns a method signature, excluding the return type. Also, uses type name rather than a full name
        static public string GetMethodSignature(MethodInfo mi)
        {
            string rs = mi.Name + "(";
            ParameterInfo[] piArray = mi.GetParameters();
            int i = 0;
            foreach (ParameterInfo pi in piArray)
            {
                rs = rs + pi.ParameterType.Name.ToString() + " " + pi.Name;
                if (i++ != piArray.Length-1)
                {
                    rs += ", ";
                }
            }
            rs += ")";
            return rs;
        }
        
        private void PopulatePublicMethods(TreeNode parent)
        {
            try
            {
                MethodInfo[] mdInfoArray = session.GetType().GetMethods();
                Array.Sort(mdInfoArray, new MethodInfoComparer());
                foreach (MethodInfo mi in mdInfoArray)
                {
                    if (!HasUnderscoreInMethodName(mi))
                    {
                        TreeNode tn = new TreeNode(GetMethodSignature(mi));
                        tn.Tag = mi;
                        parent.Nodes.Add(tn);
                    }
                }
                statusTextBox.Text = session.LastStatus.ToString();
            }
            catch (Exception ex)
            {
                GetInnerException(ex);
            }		
        }
        
        private void ShowPublicMethodsCtrls(bool show) 
        {
            invokeButton.Visible = show;
            returnTypeLabel.Visible = show;
            returnTypeTextBox.Visible = show;
            RemoveMethodCtrls();

            if (show)
            {
                MethodInfo mi = (MethodInfo)sessionTreeView.SelectedNode.Tag;
                returnTypeTextBox.Text = mi.ReturnType.Name.ToString();			
                statusTextBox.Text = session.LastStatus.ToString();
            }
        }
        
        // There are methods that contain "_" and appear as public methods 
		// using reflection. However, they should not show up in intellisense  
		// or the object browser. This method removes those functions
		// from the listbox publicMethodsListBox.
		private bool HasUnderscoreInMethodName(MethodInfo mi)
		{
			if (mi.Name.IndexOf("_") != -1)
				return true;
			return false;
		}

		private void OnKeyUpInpublicMethodsListBox(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				invokeButton_Click(null, null);
			}		
		}
		
		private void invokeButton_Click(object sender, System.EventArgs e)
		{
			RemoveMethodCtrls();
            statusTextBox.Text = "";
			try
			{
				paramCtrlSet = new ArrayList();
				paramLabelCtrlSet = new ArrayList();

				// dynamically create function paramter controls and also for the return value if appropriate.
				int numCtrl = CreateParamCtrls();
				statusTextBox.Text = session.LastStatus.ToString();
 			}
			catch (Exception ex)
			{
				GetInnerException(ex);
			}		
            sessionTreeView.Focus();
        }

		private int GetPositionY(int counter) // returns the position y for parameter control
		{
			return invokeButton.Location.Y*5/4 + (counter + 1) * (SPACE_BETWEEN_CTRLS + HEIGHT_CTRL + LABEL_HEIGHT) - HEIGHT_CTRL; 
		}

			
		private void CreateLabelForParamControl(int counter, string name)
		{
			System.Windows.Forms.Label label = new System.Windows.Forms.Label();
			label.Text = name;
			label.Location = new System.Drawing.Point(invokeButton.Location.X, GetPositionY(counter) - LABEL_HEIGHT);
			label.Size = new System.Drawing.Size(GetCtrlWidth(), LABEL_HEIGHT);
			rightPanel.Controls.Add(label);
			paramLabelCtrlSet.Add(label);
		}

		private int CreateParamCtrls()
		{
            MethodInfo mi = (MethodInfo)sessionTreeView.SelectedNode.Tag;
            ParameterInfo[] piArray = mi.GetParameters();
 
			int counter = 0;
			foreach (ParameterInfo pi in piArray)
			{
				CreateParamLabelAndCtrl(pi, counter++);
			}

			Type rt = mi.ReturnType;
			if (rt != typeof(void)) // also add read-only control for return value
			{
				AddReturnValueCtrl(counter++);
				((Label)paramLabelCtrlSet[paramLabelCtrlSet.Count-1]).Text = "Return Value";
			}

			AddExecuteButton(counter++);
			AddFeedbackLabel(counter);

			return paramCtrlSet.Count;
		}

		private void AddFeedbackLabel(int counter)
		{
			feedbackLabel = new System.Windows.Forms.Label();
			feedbackLabel.Size = new System.Drawing.Size(GetCtrlWidth(), LABEL_HEIGHT * 2);
			feedbackLabel.TabIndex = counter;
			feedbackLabel.Name = "feedbackLabel";
			feedbackLabel.Location = new System.Drawing.Point(invokeButton.Location.X, GetPositionY(counter) - SPACE_BETWEEN_CTRLS - OFFSET);
			rightPanel.Controls.Add(feedbackLabel);
		}

		private void CreateParamLabelAndCtrl(ParameterInfo pi, int counter)
		{
			CreateLabelForParamControl(counter, pi.Name);

			ctrlProperties.pointX = invokeButton.Location.X; // position of control
			ctrlProperties.pointY = GetPositionY(counter);
			ctrlProperties.sizeX = GetCtrlWidth(); 
			ctrlProperties.sizeY = HEIGHT_CTRL;
			ctrlProperties.tabIndex = counter;
			ctrlProperties.name = "Parameter" + (counter+1).ToString();

            Type parameterType = pi.ParameterType;
			Control ctrl;
			CreateCtrl(parameterType, out ctrl);
			ctrl.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
            rightPanel.Controls.Add(ctrl);
			paramCtrlSet.Add(ctrl);

            if(parameterType.Name == "IAsyncResult")
            {
                ctrl.Text = "Last stored IAsyncResult will be used.";
                TextBox tb = (TextBox)ctrl;
                tb.ReadOnly = true;
            }
		}

		private void AddExecuteButton(int counter)
		{
			executeButton = new System.Windows.Forms.Button(); 
			executeButton.Size = new System.Drawing.Size(BUTTON_WIDTH, BUTTON_HEIGHT);
			executeButton.TabIndex = counter;
			int pointX, pointY;

			pointX = invokeButton.Location.X;
			pointY = GetPositionY(counter) - SPACE_BETWEEN_CTRLS/2;
			executeButton.Name = "executeButton";
			executeButton.Text = "Execute";
			executeButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			executeButton.Click += new System.EventHandler(this.executeButton_Click);
			
			executeButton.Location = new System.Drawing.Point(pointX, pointY);
			rightPanel.Controls.Add(executeButton);
		}

		private void AddReturnValueCtrl(int counter)
		{
			ctrlProperties.pointX = invokeButton.Location.X; // position of control
			ctrlProperties.pointY = GetPositionY(counter);
			ctrlProperties.tabIndex = counter;
			CreateLabelForParamControl(counter, "Return Value");

			returnValueControl = new Control();
			CreateCtrl(typeof(string), out returnValueControl);
			returnValueControl.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			((TextBox)returnValueControl).ReadOnly = true;
            rightPanel.Controls.Add(returnValueControl);
		}

		// Convert the string from the control to the type specified in piArray
		private Object GetParamObjects(Control ctrl, ParameterInfo[] piArray, int count)
		{
			Type type = piArray[count].ParameterType;
			return ConvertFromCtrlToType(ctrl, type);	
		}

		// When the Execute button that was created dynamically is pressed, it executes
		// the selected method with specified parameters.
		private void executeButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				MethodInfo mi = (MethodInfo)sessionTreeView.SelectedNode.Tag;
				ParameterInfo[] piArray = mi.GetParameters();
				Object[] parameters = new Object[paramCtrlSet.Count];
				int count = 0;
				foreach (Control ctrl in paramCtrlSet)
				{
					parameters[count] = GetParamObjects(ctrl, piArray, count);
					count++;
				}
				Object rv = mi.Invoke(session, parameters);
				if (mi.ReturnType != typeof(void)) // display returnval
				{
                    if(mi.ReturnType == typeof(byte[]))
                    {
                        ((TextBox)returnValueControl).Text = ByteArrayToString(rv as byte[]);
                    }
                    else if(rv is IAsyncResult)
                    {
                        lastAsyncResult = (IAsyncResult)rv;
                        ((TextBox)returnValueControl).Text = "IAsyncResult object stored for later use.";
                    }
                    else
                    {
                        ((TextBox)returnValueControl).Text = InsertCommonEscapeSequences(rv.ToString());
                    }
				}
				feedbackLabel.Text = "Method " + mi.Name + " executed successfully.";
           }
			catch (Exception ex)
			{
				GetInnerException(ex);
			}
            sessionTreeView.Focus();
        }
		
		// When the Close button that was created dynamically is pressed, it exits methodsForm
		private void RemoveMethodCtrls()
		{
            if (paramCtrlSet != null && paramCtrlSet.Count > 0)
            {
                foreach(Control ctrl in paramCtrlSet)
                {
                    rightPanel.Controls.Remove(ctrl);
                }
                paramCtrlSet.RemoveRange(0, paramCtrlSet.Count);
            }
            if (paramLabelCtrlSet != null && paramLabelCtrlSet.Count > 0)
            {
                foreach(Control ctrl in paramLabelCtrlSet)
                {
                    rightPanel.Controls.Remove(ctrl);
                }
                paramLabelCtrlSet.RemoveRange(0, paramLabelCtrlSet.Count);
            }
            rightPanel.Controls.Remove(returnValueControl);
            rightPanel.Controls.Remove(executeButton);
            rightPanel.Controls.Remove(feedbackLabel);
            returnValueControl = null;
            executeButton = null;
			feedbackLabel = null;
		}
		
		#endregion		

		//*************** Methods for eventTabPage ***************//
		#region Methods for eventTabPage

        private void PopulateEvents(TreeNode parent)
        {
            try
            {
                EventInfo[] evInfoArray = session.GetType().GetEvents();
                foreach (EventInfo ei in evInfoArray)
                {
                    TreeNode tn = new TreeNode(ei.Name);
                    tn.Tag = ei;
                    parent.Nodes.Add(tn);
                }
      
                InitializeDelegates();
                statusTextBox.Text = session.LastStatus.ToString();
            }
            catch (Exception ex)
            {
                GetInnerException(ex);
            }		
        }
        
        private void EventSelected() 
        {
            ShowPublicPropertiesCtrls(false);
            ShowPublicMethodsCtrls(false);
            ShowEventsCtrls(true);
        }

        private void ShowEventsCtrls(bool show) 
        {
            registerButton.Visible = show;
            unregisterButton.Visible = show;
        }
        
        private void InitializeDelegates()
		{
			GpibCICEventHandler = new GpibInterfaceControllerInChargeEventHandler(OnCICEventHandler);
			SignalProcessorEventHandler = new VxiSessionSignalProcessorEventHandler(OnVxiSignalProcessorEventHandler);
			GITriggerEventHandler = new GpibInterfaceTriggerEventHandler(OnGpibInterfaceTriggerEventHandler);
            VBTriggerEventHandler = new VxiBackplaneTriggerEventHandler(OnVxiBackplaneTriggerEventHandler);
            VSTriggerEventHandler = new VxiSessionTriggerEventHandler(OnVxiSessionTriggerEventHandler);
			VisaEHandler = new VisaEventHandler(OnVisaEventHandler);
            GIEventHandler = new GpibInterfaceEventHandler(OnGIEventHandler);
            VBEventHandler = new VxiBackplaneEventHandler(OnVBEventHandler);
            MbsEventHandler = new MessageBasedSessionEventHandler(OnMbsEventHandler);
            SSEventHandler = new SerialSessionEventHandler(OnSSEventHandler);
            VSEventHandler = new VxiSessionEventHandler(OnVSEventHandler);
            PSEventHandler = new PxiSessionEventHandler(OnPSEventHandler);
			VxiVmeInterruptEventHandler = new VxiSessionVxiVmeInterruptEventHandler(OnVxiVmeInterruptEventHandler);
            UsbREventHandler	= new UsbRawEventHandler(OnUsbREventHandler);
            UsbRIEventHandler	= new UsbRawInterruptEventHandler(OnUsbRIEventHandler);
            UsbSEventHandler	= new UsbSessionEventHandler(OnUsbSEventHandler);
            UsbSIEventHandler	= new UsbSessionInterruptEventHandler(OnUsbSIEventHandler);
        }
		
		private void registerButton_Click(object sender, System.EventArgs e)
		{
			statusTextBox.Text = "";
			try
			{
                EventInfo ef = (EventInfo)sessionTreeView.SelectedNode.Tag;

				Type handlerType = ef.EventHandlerType;
                if (handlerType == typeof(GpibInterfaceControllerInChargeEventHandler))
                {
                    ef.AddEventHandler(session, GpibCICEventHandler); 
                }
                else if (handlerType == typeof(VxiSessionSignalProcessorEventHandler))
                {
                    ef.AddEventHandler(session, SignalProcessorEventHandler); 
                }		
                else if (handlerType == typeof(GpibInterfaceTriggerEventHandler))
                {
                    ef.AddEventHandler(session, GITriggerEventHandler); 
                }
                else if (handlerType == typeof(VxiBackplaneTriggerEventHandler))
                {
                    ef.AddEventHandler(session, VBTriggerEventHandler); 
                }
                else if (handlerType == typeof(VxiSessionTriggerEventHandler))
                {
                    ef.AddEventHandler(session, VSTriggerEventHandler); 
                }
                else if (handlerType == typeof(VisaEventHandler))
                {
                    ef.AddEventHandler(session, VisaEHandler); 
                }
                else if (handlerType == typeof(GpibInterfaceEventHandler))
                {
                    ef.AddEventHandler(session, GIEventHandler);
                }
                else if (handlerType == typeof(VxiBackplaneEventHandler))
                {
                    ef.AddEventHandler(session, VBEventHandler);
                }
                else if (handlerType == typeof(MessageBasedSessionEventHandler))
                {
                    ef.AddEventHandler(session, MbsEventHandler);
                }
                else if (handlerType == typeof(SerialSessionEventHandler))
                {
                    ef.AddEventHandler(session, SSEventHandler);
                }
                else if (handlerType == typeof(VxiSessionEventHandler))
                {
                    ef.AddEventHandler(session, VSEventHandler);
                }
                else if (handlerType == typeof(PxiSessionEventHandler))
                {
                    ef.AddEventHandler(session, PSEventHandler);
                }
                else if (handlerType == typeof(VxiSessionVxiVmeInterruptEventHandler))
                {
                    ef.AddEventHandler(session, VxiVmeInterruptEventHandler); 
                }	
                else if (handlerType == typeof(UsbRawEventHandler))
                {
                    ef.AddEventHandler(session, UsbREventHandler); 
                }	
                else if (handlerType == typeof(UsbRawInterruptEventHandler))
                {
                    ef.AddEventHandler(session, UsbRIEventHandler); 
                }	
                else if (handlerType == typeof(UsbSessionEventHandler))
                {
                    ef.AddEventHandler(session, UsbSEventHandler ); 
                }	
                else if (handlerType == typeof(UsbSessionInterruptEventHandler))
                {
                    ef.AddEventHandler(session, UsbSIEventHandler ); 
                }	
                statusTextBox.Text = session.LastStatus.ToString();
			}
			catch (Exception ex)
			{
				GetInnerException(ex);
			}	
	        sessionTreeView.Focus();
		}

		private void unregisterButton_Click(object sender, System.EventArgs e)
		{
			statusTextBox.Text = "";
			eventFeedbackTextBox.Text = "";
			try
			{
				EventInfo ef = (EventInfo)sessionTreeView.SelectedNode.Tag;

				Type handlerType = ef.EventHandlerType;
				if (handlerType == typeof(GpibInterfaceControllerInChargeEventHandler))
				{
					ef.RemoveEventHandler(session, GpibCICEventHandler); 
				}
				else if (handlerType == typeof(VxiSessionSignalProcessorEventHandler))
				{
					ef.RemoveEventHandler(session, SignalProcessorEventHandler); 
				}		
				else if (handlerType == typeof(GpibInterfaceTriggerEventHandler))
				{
					ef.RemoveEventHandler(session, GITriggerEventHandler); 
				}	
                else if (handlerType == typeof(VxiBackplaneTriggerEventHandler))
                {
                    ef.RemoveEventHandler(session, VBTriggerEventHandler); 
                }	
                else if (handlerType == typeof(VxiSessionTriggerEventHandler))
                {
                    ef.RemoveEventHandler(session, VSTriggerEventHandler); 
                }	
				else if (handlerType == typeof(VisaEventHandler))
				{
					ef.RemoveEventHandler(session, VisaEHandler); 
				}		
                else if (handlerType == typeof(GpibInterfaceEventHandler))
                {
                    ef.RemoveEventHandler(session, GIEventHandler);
                }
                else if (handlerType == typeof(VxiBackplaneEventHandler))
                {
                    ef.RemoveEventHandler(session, VBEventHandler);
                }
                else if (handlerType == typeof(MessageBasedSessionEventHandler))
                {
                    ef.RemoveEventHandler(session, MbsEventHandler);
                }
                else if (handlerType == typeof(SerialSessionEventHandler))
                {
                    ef.RemoveEventHandler(session, SSEventHandler);
                }
                else if (handlerType == typeof(VxiSessionEventHandler))
                {
                    ef.RemoveEventHandler(session, VSEventHandler);
                }
                else if (handlerType == typeof(PxiSessionEventHandler))
                {
                    ef.RemoveEventHandler(session, PSEventHandler);
                }
				else if (handlerType == typeof(VxiSessionVxiVmeInterruptEventHandler))
				{
					ef.RemoveEventHandler(session, VxiVmeInterruptEventHandler); 
				}	
                else if (handlerType == typeof(UsbRawEventHandler))
                {
                    ef.RemoveEventHandler(session, UsbREventHandler); 
                }	
                else if (handlerType == typeof(UsbRawInterruptEventHandler))
                {
                    ef.RemoveEventHandler(session, UsbRIEventHandler); 
                }	
                else if (handlerType == typeof(UsbSessionEventHandler))
                {
                    ef.RemoveEventHandler(session, UsbSEventHandler ); 
                }	
                else if (handlerType == typeof(UsbSessionInterruptEventHandler))
                {
                    ef.RemoveEventHandler(session, UsbSIEventHandler ); 
                }	
                statusTextBox.Text = session.LastStatus.ToString();
			}
			catch (Exception ex)
			{
				GetInnerException(ex);
			}	
	        sessionTreeView.Focus();
		}
		
		public void  OnCICEventHandler(object sender, GpibInterfaceControllerInChargeEventArgs e)
		{
			eventFeedbackTextBox.Text = "ControllerInChargeEventHandler is caught. " + e.ToString();
		}

		public void  OnVxiSignalProcessorEventHandler(object sender, VxiSessionSignalProcessorEventArgs e)
		{
			eventFeedbackTextBox.Text = "VxiSignalProcessorEventHandler is caught. " + e.ToString();
		}

		public void  OnGpibInterfaceTriggerEventHandler(object sender, GpibInterfaceTriggerEventArgs e)
		{
			eventFeedbackTextBox.Text = "GpibInterfaceTriggerEventHandler is caught. " + e.ToString();
		}

        public void  OnVxiBackplaneTriggerEventHandler(object sender, VxiBackplaneTriggerEventArgs e)
        {
            eventFeedbackTextBox.Text = "VxiBackplaneTriggerEventHandler is caught. " + e.ToString();
        }

        public void  OnVxiSessionTriggerEventHandler(object sender, VxiSessionTriggerEventArgs e)
        {
            eventFeedbackTextBox.Text = "VxiSessionTriggerEventHandler is caught. " + e.ToString();
        }

		public void  OnVisaEventHandler(object sender, VisaEventArgs e)
		{
			eventFeedbackTextBox.Text = "VisaEventHandler is caught. " + e.ToString();
		}

        public void OnGIEventHandler(object sender, GpibInterfaceEventArgs e)
        {
            eventFeedbackTextBox.Text = "GpibInterfaceEventHandler is caught. " + e.ToString();
        }

        public void OnVBEventHandler(object sender, VxiBackplaneEventArgs e)
        {
            eventFeedbackTextBox.Text = "VxiBackplaneEventHandler is caught. " + e.ToString();
        }       

        public void OnMbsEventHandler(object sender, MessageBasedSessionEventArgs e)
        {
            eventFeedbackTextBox.Text = "MessageBasedSessionEventHandler is caught. " + e.ToString();
        }

        public void OnSSEventHandler(object sender, SerialSessionEventArgs e)
        {
            eventFeedbackTextBox.Text = "SerialSessionEventHandler is caught. " + e.ToString();
        }

        public void OnVSEventHandler(object sender, VxiSessionEventArgs e)
        {
            eventFeedbackTextBox.Text = "VxiSessionEventHandler is caught. " + e.ToString();
        }

        public void OnPSEventHandler(object sender, PxiSessionEventArgs e)
        {
            eventFeedbackTextBox.Text = "PxiSessionEventHandler is caught. " + e.ToString();
        }

		public void  OnVxiVmeInterruptEventHandler(object sender, VxiSessionVxiVmeInterruptEventArgs e)
		{
			eventFeedbackTextBox.Text = "VmeInterruptEventHandler is caught. " + e.ToString();
		}

        public void  OnUsbREventHandler (object sender, UsbRawEventArgs e)
        {
            eventFeedbackTextBox.Text = "UsbRawEventHandler is caught. " + e.ToString();
        }

        public void  OnUsbRIEventHandler (object sender, UsbRawInterruptEventArgs e)
        {
            eventFeedbackTextBox.Text = "UsbRawInterruptEventHandler is caught. " + e.ToString();
        }

        public void  OnUsbSEventHandler(object sender, UsbSessionEventArgs e)
        {
            eventFeedbackTextBox.Text = "UsbSessionEventHandler is caught. " + e.ToString();
        }

        public void  OnUsbSIEventHandler(object sender, UsbSessionInterruptEventArgs e)
        {
            eventFeedbackTextBox.Text = "UsbSessionInterruptEventHandler is caught. " + e.ToString();
        }

#endregion

 	}
}
