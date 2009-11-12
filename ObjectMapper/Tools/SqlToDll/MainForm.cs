using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Puzzle.ObjectMapper.Tools.SqlToDll
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox connectionStringTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox sourceTypeComboBox;
		private System.Windows.Forms.ComboBox providerTypeComboBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox databaseGroupBox;
		private System.Windows.Forms.TextBox outputFolderTextBox;
		private System.Windows.Forms.GroupBox outputGroupBox;
		private System.Windows.Forms.Button browseFolderButton;
		private System.Windows.Forms.RadioButton fullSourceRadioButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox sourceCodeLanguageComboBox;
		private System.Windows.Forms.RadioButton assemblyRadioButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button testConnectionButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox rootNamespaceTextBox;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox assemblyNameTextBox;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.connectionStringTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.sourceTypeComboBox = new System.Windows.Forms.ComboBox();
			this.providerTypeComboBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.outputFolderTextBox = new System.Windows.Forms.TextBox();
			this.databaseGroupBox = new System.Windows.Forms.GroupBox();
			this.testConnectionButton = new System.Windows.Forms.Button();
			this.outputGroupBox = new System.Windows.Forms.GroupBox();
			this.assemblyNameTextBox = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.rootNamespaceTextBox = new System.Windows.Forms.TextBox();
			this.assemblyRadioButton = new System.Windows.Forms.RadioButton();
			this.sourceCodeLanguageComboBox = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.fullSourceRadioButton = new System.Windows.Forms.RadioButton();
			this.browseFolderButton = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.databaseGroupBox.SuspendLayout();
			this.outputGroupBox.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 144);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please enter the connection string to your database:";
			// 
			// connectionStringTextBox
			// 
			this.connectionStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStringTextBox.Location = new System.Drawing.Point(24, 160);
			this.connectionStringTextBox.Name = "connectionStringTextBox";
			this.connectionStringTextBox.Size = new System.Drawing.Size(480, 21);
			this.connectionStringTextBox.TabIndex = 1;
			this.connectionStringTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(224, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Please select the type of your database:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(472, 23);
			this.label3.TabIndex = 3;
			this.label3.Text = "Please select the type of provider you would like in order to communicate with th" +
				"e database:";
			// 
			// sourceTypeComboBox
			// 
			this.sourceTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.sourceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sourceTypeComboBox.Items.AddRange(new object[] {
																	"MS Sql Server",
																	"MS Access"});
			this.sourceTypeComboBox.Location = new System.Drawing.Point(24, 49);
			this.sourceTypeComboBox.Name = "sourceTypeComboBox";
			this.sourceTypeComboBox.Size = new System.Drawing.Size(520, 21);
			this.sourceTypeComboBox.TabIndex = 4;
			// 
			// providerTypeComboBox
			// 
			this.providerTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.providerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.providerTypeComboBox.Items.AddRange(new object[] {
																	  "SqlClient",
																	  "OleDbClient",
																	  "OdbcClient"});
			this.providerTypeComboBox.Location = new System.Drawing.Point(24, 105);
			this.providerTypeComboBox.Name = "providerTypeComboBox";
			this.providerTypeComboBox.Size = new System.Drawing.Size(520, 21);
			this.providerTypeComboBox.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 168);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(472, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Please enter the path to the output folder where you would like your generated fi" +
				"les to be placed:";
			// 
			// outputFolderTextBox
			// 
			this.outputFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.outputFolderTextBox.Location = new System.Drawing.Point(24, 185);
			this.outputFolderTextBox.Name = "outputFolderTextBox";
			this.outputFolderTextBox.Size = new System.Drawing.Size(496, 21);
			this.outputFolderTextBox.TabIndex = 7;
			this.outputFolderTextBox.Text = "";
			// 
			// databaseGroupBox
			// 
			this.databaseGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.databaseGroupBox.Controls.Add(this.testConnectionButton);
			this.databaseGroupBox.Controls.Add(this.providerTypeComboBox);
			this.databaseGroupBox.Controls.Add(this.sourceTypeComboBox);
			this.databaseGroupBox.Controls.Add(this.label2);
			this.databaseGroupBox.Controls.Add(this.connectionStringTextBox);
			this.databaseGroupBox.Controls.Add(this.label3);
			this.databaseGroupBox.Controls.Add(this.label1);
			this.databaseGroupBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.databaseGroupBox.Location = new System.Drawing.Point(208, 112);
			this.databaseGroupBox.Name = "databaseGroupBox";
			this.databaseGroupBox.Size = new System.Drawing.Size(568, 200);
			this.databaseGroupBox.TabIndex = 8;
			this.databaseGroupBox.TabStop = false;
			this.databaseGroupBox.Text = "Input (sql database)";
			// 
			// testConnectionButton
			// 
			this.testConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.testConnectionButton.Location = new System.Drawing.Point(504, 161);
			this.testConnectionButton.Name = "testConnectionButton";
			this.testConnectionButton.Size = new System.Drawing.Size(40, 20);
			this.testConnectionButton.TabIndex = 6;
			this.testConnectionButton.Text = "Test";
			this.testConnectionButton.Click += new System.EventHandler(this.testConnectionButton_Click);
			// 
			// outputGroupBox
			// 
			this.outputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.outputGroupBox.Controls.Add(this.assemblyNameTextBox);
			this.outputGroupBox.Controls.Add(this.label10);
			this.outputGroupBox.Controls.Add(this.rootNamespaceTextBox);
			this.outputGroupBox.Controls.Add(this.assemblyRadioButton);
			this.outputGroupBox.Controls.Add(this.sourceCodeLanguageComboBox);
			this.outputGroupBox.Controls.Add(this.label6);
			this.outputGroupBox.Controls.Add(this.fullSourceRadioButton);
			this.outputGroupBox.Controls.Add(this.browseFolderButton);
			this.outputGroupBox.Controls.Add(this.outputFolderTextBox);
			this.outputGroupBox.Controls.Add(this.label4);
			this.outputGroupBox.Controls.Add(this.label9);
			this.outputGroupBox.Controls.Add(this.label5);
			this.outputGroupBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.outputGroupBox.Location = new System.Drawing.Point(208, 328);
			this.outputGroupBox.Name = "outputGroupBox";
			this.outputGroupBox.Size = new System.Drawing.Size(568, 296);
			this.outputGroupBox.TabIndex = 9;
			this.outputGroupBox.TabStop = false;
			this.outputGroupBox.Text = "Output (source code or compiled assembly)";
			// 
			// assemblyNameTextBox
			// 
			this.assemblyNameTextBox.Location = new System.Drawing.Point(24, 104);
			this.assemblyNameTextBox.Name = "assemblyNameTextBox";
			this.assemblyNameTextBox.Size = new System.Drawing.Size(520, 21);
			this.assemblyNameTextBox.TabIndex = 17;
			this.assemblyNameTextBox.Text = "";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(24, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(472, 23);
			this.label10.TabIndex = 16;
			this.label10.Text = "Please enter a name for your assembly (or your project if you generate source cod" +
				"e):";
			// 
			// rootNamespaceTextBox
			// 
			this.rootNamespaceTextBox.Location = new System.Drawing.Point(24, 48);
			this.rootNamespaceTextBox.Name = "rootNamespaceTextBox";
			this.rootNamespaceTextBox.Size = new System.Drawing.Size(520, 21);
			this.rootNamespaceTextBox.TabIndex = 14;
			this.rootNamespaceTextBox.Text = "";
			// 
			// assemblyRadioButton
			// 
			this.assemblyRadioButton.Checked = true;
			this.assemblyRadioButton.Location = new System.Drawing.Point(24, 224);
			this.assemblyRadioButton.Name = "assemblyRadioButton";
			this.assemblyRadioButton.Size = new System.Drawing.Size(288, 24);
			this.assemblyRadioButton.TabIndex = 13;
			this.assemblyRadioButton.TabStop = true;
			this.assemblyRadioButton.Text = "Generate a compiled assembly for my classes";
			// 
			// sourceCodeLanguageComboBox
			// 
			this.sourceCodeLanguageComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.sourceCodeLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sourceCodeLanguageComboBox.Items.AddRange(new object[] {
																			"C#",
																			"Visual Basic.Net"});
			this.sourceCodeLanguageComboBox.Location = new System.Drawing.Point(312, 257);
			this.sourceCodeLanguageComboBox.Name = "sourceCodeLanguageComboBox";
			this.sourceCodeLanguageComboBox.Size = new System.Drawing.Size(232, 21);
			this.sourceCodeLanguageComboBox.TabIndex = 12;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(312, 240);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(152, 24);
			this.label6.TabIndex = 11;
			this.label6.Text = "Source code language:";
			// 
			// fullSourceRadioButton
			// 
			this.fullSourceRadioButton.Location = new System.Drawing.Point(24, 256);
			this.fullSourceRadioButton.Name = "fullSourceRadioButton";
			this.fullSourceRadioButton.Size = new System.Drawing.Size(240, 24);
			this.fullSourceRadioButton.TabIndex = 9;
			this.fullSourceRadioButton.Text = "Generate full source code for my classes";
			// 
			// browseFolderButton
			// 
			this.browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseFolderButton.Location = new System.Drawing.Point(520, 185);
			this.browseFolderButton.Name = "browseFolderButton";
			this.browseFolderButton.Size = new System.Drawing.Size(24, 20);
			this.browseFolderButton.TabIndex = 8;
			this.browseFolderButton.Text = "...";
			this.browseFolderButton.Click += new System.EventHandler(this.browseFolderButton_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(24, 32);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(296, 23);
			this.label9.TabIndex = 15;
			this.label9.Text = "Please enter a root namespace for your classes:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(528, 23);
			this.label5.TabIndex = 10;
			this.label5.Text = "Please note that any existing files with conflicting names in the chosen output f" +
				"older will be overwritten!";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.okButton.Location = new System.Drawing.Point(624, 640);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 10;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(704, 640);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(184, 670);
			this.panel1.TabIndex = 13;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Info;
			this.panel2.Controls.Add(this.label8);
			this.panel2.Controls.Add(this.label7);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(184, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(616, 96);
			this.panel2.TabIndex = 14;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(48, 40);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(544, 48);
			this.label8.TabIndex = 1;
			this.label8.Text = @"The Puzzle Sql2Dll application takes you in one single step from your existing sql database to a compiled assembly (or, if you want, the source code for your assembly) containing domain model classes matching the tables in your database. You also get an NPersist xml mapping file mapping your classes to your tables.";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(16, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(296, 23);
			this.label7.TabIndex = 0;
			this.label7.Text = "Welcome to Puzzle Sql2Dll";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(800, 670);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.outputGroupBox);
			this.Controls.Add(this.databaseGroupBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Puzzle Sql2Dll";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.databaseGroupBox.ResumeLayout(false);
			this.outputGroupBox.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
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

		private void TestConnection()
		{
			
		}

		private void Generate()
		{
			ISourceToTables sourceToTables = null;

			if (sourceTypeComboBox.SelectedIndex == 0)
			{
				sourceToTables = new SourceToTablesMSAccess() ;
			}
			
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			sourceTypeComboBox.SelectedIndex = 0;
			providerTypeComboBox.SelectedIndex = 0;
			sourceCodeLanguageComboBox.SelectedIndex = 0;
		}

		private void browseFolderButton_Click(object sender, System.EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() != DialogResult.Cancel)
			{
				outputFolderTextBox.Text = folderBrowserDialog1.SelectedPath ;
			}  
		}

		private void testConnectionButton_Click(object sender, System.EventArgs e)
		{
			TestConnection();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.Close() ;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			Generate();		
			this.Close() ;
		}


	}
}
