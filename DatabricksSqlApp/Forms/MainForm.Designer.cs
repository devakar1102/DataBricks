namespace DatabricksSqlApp.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private Label lblClient;
    private ComboBox cboClient;
    private GroupBox grpSetup;
    private Label lblHost;
    private TextBox txtHost;
    private Label lblHttpPath;
    private TextBox txtHttpPath;
    private Label lblPat;
    private TextBox txtPat;
    private Label lblSql;
    private TextBox txtSql;
    private Button btnQuery;
    private Label lblStatus;
    private DataGridView dgvResults;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        lblClient = new Label();
        cboClient = new ComboBox();
        grpSetup = new GroupBox();
        lblHost = new Label();
        txtHost = new TextBox();
        lblHttpPath = new Label();
        txtHttpPath = new TextBox();
        lblPat = new Label();
        txtPat = new TextBox();
        lblSql = new Label();
        txtSql = new TextBox();
        btnQuery = new Button();
        lblStatus = new Label();
        dgvResults = new DataGridView();

        SuspendLayout();
        grpSetup.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();

        // lblClient
        lblClient.AutoSize = true;
        lblClient.Location = new Point(16, 18);
        lblClient.Name = "lblClient";
        lblClient.Size = new Size(44, 15);
        lblClient.Text = "Client:";

        // cboClient
        cboClient.DropDownStyle = ComboBoxStyle.DropDownList;
        cboClient.FormattingEnabled = true;
        cboClient.Location = new Point(72, 14);
        cboClient.Name = "cboClient";
        cboClient.Size = new Size(360, 23);
        cboClient.TabIndex = 0;
        cboClient.SelectedIndexChanged += cboClient_SelectedIndexChanged;

        // grpSetup
        grpSetup.Controls.Add(lblHost);
        grpSetup.Controls.Add(txtHost);
        grpSetup.Controls.Add(lblHttpPath);
        grpSetup.Controls.Add(txtHttpPath);
        grpSetup.Controls.Add(lblPat);
        grpSetup.Controls.Add(txtPat);
        grpSetup.Location = new Point(16, 50);
        grpSetup.Name = "grpSetup";
        grpSetup.Size = new Size(848, 130);
        grpSetup.TabIndex = 1;
        grpSetup.TabStop = false;
        grpSetup.Text = "Application Setup";
        grpSetup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lblHost
        lblHost.AutoSize = true;
        lblHost.Location = new Point(12, 28);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(36, 15);
        lblHost.Text = "Host:";

        // txtHost
        txtHost.Location = new Point(110, 25);
        txtHost.Name = "txtHost";
        txtHost.ReadOnly = true;
        txtHost.Size = new Size(720, 23);
        txtHost.TabIndex = 0;
        txtHost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lblHttpPath
        lblHttpPath.AutoSize = true;
        lblHttpPath.Location = new Point(12, 58);
        lblHttpPath.Name = "lblHttpPath";
        lblHttpPath.Size = new Size(64, 15);
        lblHttpPath.Text = "HTTP Path:";

        // txtHttpPath
        txtHttpPath.Location = new Point(110, 55);
        txtHttpPath.Name = "txtHttpPath";
        txtHttpPath.ReadOnly = true;
        txtHttpPath.Size = new Size(720, 23);
        txtHttpPath.TabIndex = 1;
        txtHttpPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lblPat
        lblPat.AutoSize = true;
        lblPat.Location = new Point(12, 88);
        lblPat.Name = "lblPat";
        lblPat.Size = new Size(32, 15);
        lblPat.Text = "PAT:";

        // txtPat
        txtPat.Location = new Point(110, 85);
        txtPat.Name = "txtPat";
        txtPat.ReadOnly = true;
        txtPat.Size = new Size(720, 23);
        txtPat.TabIndex = 2;
        txtPat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lblSql
        lblSql.AutoSize = true;
        lblSql.Location = new Point(16, 195);
        lblSql.Name = "lblSql";
        lblSql.Size = new Size(33, 15);
        lblSql.Text = "SQL:";

        // txtSql
        txtSql.Location = new Point(16, 215);
        txtSql.Multiline = true;
        txtSql.Name = "txtSql";
        txtSql.ReadOnly = true;
        txtSql.ScrollBars = ScrollBars.Vertical;
        txtSql.Size = new Size(848, 70);
        txtSql.TabIndex = 2;
        txtSql.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // btnQuery
        btnQuery.Enabled = false;
        btnQuery.Location = new Point(16, 295);
        btnQuery.Name = "btnQuery";
        btnQuery.Size = new Size(120, 30);
        btnQuery.TabIndex = 3;
        btnQuery.Text = "Query";
        btnQuery.UseVisualStyleBackColor = true;
        btnQuery.Click += btnQuery_Click;

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(150, 302);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(120, 15);
        lblStatus.Text = "0 row(s), 0 column(s)";

        // dgvResults
        dgvResults.AllowUserToAddRows = false;
        dgvResults.AllowUserToDeleteRows = false;
        dgvResults.ReadOnly = true;
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvResults.Location = new Point(16, 335);
        dgvResults.Name = "dgvResults";
        dgvResults.Size = new Size(848, 240);
        dgvResults.TabIndex = 4;
        dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(880, 590);
        Controls.Add(dgvResults);
        Controls.Add(lblStatus);
        Controls.Add(btnQuery);
        Controls.Add(txtSql);
        Controls.Add(lblSql);
        Controls.Add(grpSetup);
        Controls.Add(cboClient);
        Controls.Add(lblClient);
        MinimumSize = new Size(700, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Databricks SQL Client";

        grpSetup.ResumeLayout(false);
        grpSetup.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
