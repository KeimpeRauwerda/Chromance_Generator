namespace Chromance_Generator;

partial class Form1
{
    delegate void SetTextCallbackDelegate(string text);
    delegate void SetRedrawCallbackDelegate();
    public TrackBar trackbar_wallWidth = new TrackBar();
    public TrackBar trackbar_wallHeight = new TrackBar();
    public TrackBar trackbar_profileLength = new TrackBar();
    public TrackBar trackbar_hubInnerWidth = new TrackBar();
    public TrackBar trackbar_hubOuterWidth = new TrackBar();

    public TrackBar trackbar_minPoints = new TrackBar();
    public TrackBar trackbar_maxPoints = new TrackBar();
    public TrackBar trackbar_minProfiles = new TrackBar();
    public TrackBar trackbar_maxProfiles = new TrackBar();

    public Label label_wallWidth = new Label();
    public Label label_wallHeight = new Label();
    public Label label_profileLength = new Label();
    public Label label_hubInnerWidth = new Label();
    public Label label_hubOuterWidth = new Label();

    public Label label_minPoints = new Label();
    public Label label_maxPoints = new Label();
    public Label label_minProfiles = new Label();
    public Label label_maxProfiles = new Label();

    public TextBox textBox_wallWidth = new TextBox();
    public TextBox textBox_wallHeight = new TextBox();
    public TextBox textBox_profileLength = new TextBox();
    public TextBox textBox_hubInnerWidth = new TextBox();
    public TextBox textBox_hubOuterWidth = new TextBox();

    public TextBox textBox_minPoints = new TextBox();
    public TextBox textBox_maxPoints = new TextBox();
    public TextBox textBox_minProfiles = new TextBox();
    public TextBox textBox_maxProfiles = new TextBox();

    public CheckBox checkBox_allowSingleDegree = new CheckBox();
    public CheckBox checkBox_generateSymmetrical = new CheckBox();
    public CheckBox checkBox_showSteps = new CheckBox();
    public Button button_generate = new Button();
    public Label label_generate = new Label();

    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent(int width, int height)
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(width, height);
        this.Text = "Chromance Generator";

        this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        this.Resize += new System.EventHandler(this.ResizeCanvas);
        
        InitializeUI();
    }

    private void InitializeUI() {
        this.label_wallWidth.Location     = new Point(this.paddingX + 5, this.paddingY + 0);
        this.label_wallHeight.Location    = new Point(this.paddingX + 5, this.paddingY + 60);
        this.label_profileLength.Location = new Point(this.paddingX + 5, this.paddingY + 120);
        this.label_hubInnerWidth.Location = new Point(this.paddingX + 5, this.paddingY + 180);
        this.label_hubOuterWidth.Location = new Point(this.paddingX + 5, this.paddingY + 240);
        this.label_minPoints.Location     = new Point(this.paddingX + 5, this.paddingY + 310);
        this.label_maxPoints.Location     = new Point(this.paddingX + 5, this.paddingY + 370);
        this.label_minProfiles.Location   = new Point(this.paddingX + 5, this.paddingY + 430);
        this.label_maxProfiles.Location   = new Point(this.paddingX + 5, this.paddingY + 490);

        this.trackbar_wallWidth.Location     = new Point(this.paddingX, this.paddingY + 20);
        this.trackbar_wallHeight.Location    = new Point(this.paddingX, this.paddingY + 80);
        this.trackbar_profileLength.Location = new Point(this.paddingX, this.paddingY + 140);
        this.trackbar_hubInnerWidth.Location = new Point(this.paddingX, this.paddingY + 200);
        this.trackbar_hubOuterWidth.Location = new Point(this.paddingX, this.paddingY + 260);
        this.trackbar_minPoints.Location     = new Point(this.paddingX, this.paddingY + 330);
        this.trackbar_maxPoints.Location     = new Point(this.paddingX, this.paddingY + 390);
        this.trackbar_minProfiles.Location   = new Point(this.paddingX, this.paddingY + 450);
        this.trackbar_maxProfiles.Location   = new Point(this.paddingX, this.paddingY + 510);

        this.textBox_wallWidth.Location     = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 20);
        this.textBox_wallHeight.Location    = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 80);
        this.textBox_profileLength.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 140);
        this.textBox_hubInnerWidth.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 200);
        this.textBox_hubOuterWidth.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 260);
        this.textBox_minPoints.Location     = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 330);
        this.textBox_maxPoints.Location     = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 390);
        this.textBox_minProfiles.Location   = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 450);
        this.textBox_maxProfiles.Location   = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 510);

        this.label_wallWidth.Size     = new Size(this.offsetX - this.paddingX, 20);
        this.label_wallHeight.Size    = new Size(this.offsetX - this.paddingX, 20);
        this.label_profileLength.Size = new Size(this.offsetX - this.paddingX, 20);
        this.label_hubInnerWidth.Size = new Size(this.offsetX - this.paddingX, 20);
        this.label_hubOuterWidth.Size = new Size(this.offsetX - this.paddingX, 20);
        this.label_minPoints.Size     = new Size(this.offsetX - this.paddingX, 20);
        this.label_maxPoints.Size     = new Size(this.offsetX - this.paddingX, 20);
        this.label_minProfiles.Size   = new Size(this.offsetX - this.paddingX, 20);
        this.label_maxProfiles.Size   = new Size(this.offsetX - this.paddingX, 20);

        this.trackbar_wallWidth.Size     = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_wallHeight.Size    = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_profileLength.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_hubInnerWidth.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_hubOuterWidth.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_minPoints.Size     = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_maxPoints.Size     = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_minProfiles.Size   = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_maxProfiles.Size   = new Size(this.offsetX - this.paddingX - 40, 45);
        
        this.textBox_wallWidth.Size     = new Size(40, 20);
        this.textBox_wallHeight.Size    = new Size(40, 20);
        this.textBox_profileLength.Size = new Size(40, 20);
        this.textBox_hubInnerWidth.Size = new Size(40, 20);
        this.textBox_hubOuterWidth.Size = new Size(40, 20);
        this.textBox_minPoints.Size     = new Size(40, 20);
        this.textBox_maxPoints.Size     = new Size(40, 20);
        this.textBox_minProfiles.Size   = new Size(40, 20);
        this.textBox_maxProfiles.Size   = new Size(40, 20);
        
        this.textBox_wallWidth.Text     = this.wallWidth.ToString();
        this.textBox_wallHeight.Text    = this.wallHeight.ToString();
        this.textBox_profileLength.Text = this.profileLength.ToString();
        this.textBox_hubInnerWidth.Text = this.hubInnerWidth.ToString();
        this.textBox_hubOuterWidth.Text = this.hubOuterWidth.ToString();
        this.textBox_minPoints.Text     = this.minPoints.ToString();
        this.textBox_maxPoints.Text     = this.maxPoints.ToString();
        this.textBox_minProfiles.Text   = this.minProfiles.ToString();
        this.textBox_maxProfiles.Text   = this.maxProfiles.ToString();
        
        this.label_wallWidth.Text     = "Wall width in cm";
        this.label_wallHeight.Text    = "Wall height in cm";
        this.label_profileLength.Text = "Profile length in cm";
        this.label_hubInnerWidth.Text = "Hub inner width in cm";
        this.label_hubOuterWidth.Text = "Hub outer width in cm";
        this.label_minPoints.Text     = "Minimum amount of points";
        this.label_maxPoints.Text     = "Maximum amount of points";
        this.label_minProfiles.Text   = "Minimum amount of LED profiles";
        this.label_maxProfiles.Text   = "Maximum amount of LED profiles";
        
        this.trackbar_wallWidth.Minimum = 10;
        this.trackbar_wallWidth.Maximum = 400;
        this.trackbar_wallWidth.SmallChange = 1;
        this.trackbar_wallWidth.Value = (int)this.wallWidth;

        this.trackbar_wallHeight.Minimum = 10;
        this.trackbar_wallHeight.Maximum = 400;
        this.trackbar_wallHeight.SmallChange = 1;
        this.trackbar_wallHeight.Value = (int)this.wallHeight;

        this.trackbar_profileLength.Minimum = 10;
        this.trackbar_profileLength.Maximum = 100;
        this.trackbar_profileLength.SmallChange = 1;
        this.trackbar_profileLength.Value = (int)this.profileLength;

        this.trackbar_hubInnerWidth.Minimum = 1;
        this.trackbar_hubInnerWidth.Maximum = 10;
        this.trackbar_hubInnerWidth.SmallChange = 1;
        this.trackbar_hubInnerWidth.Value = (int)this.hubInnerWidth;

        this.trackbar_hubOuterWidth.Minimum = 1;
        this.trackbar_hubOuterWidth.Maximum = 10;
        this.trackbar_hubOuterWidth.SmallChange = 1;
        this.trackbar_hubOuterWidth.Value = (int)this.hubOuterWidth;

        this.trackbar_minPoints.Minimum = 3;
        this.trackbar_minPoints.Maximum = 100;
        this.trackbar_minPoints.SmallChange = 1;
        this.trackbar_minPoints.Value = (int)this.minPoints;

        this.trackbar_maxPoints.Minimum = 3;
        this.trackbar_maxPoints.Maximum = 100;
        this.trackbar_maxPoints.SmallChange = 1;
        this.trackbar_maxPoints.Value = (int)this.maxPoints;

        this.trackbar_minProfiles.Minimum = 3;
        this.trackbar_minProfiles.Maximum = 100;
        this.trackbar_minProfiles.SmallChange = 1;
        this.trackbar_minProfiles.Value = (int)this.minProfiles;

        this.trackbar_maxProfiles.Minimum = 3;
        this.trackbar_maxProfiles.Maximum = 100;
        this.trackbar_maxProfiles.SmallChange = 1;
        this.trackbar_maxProfiles.Value = (int)this.maxProfiles;
        
        this.trackbar_wallWidth.Scroll += new System.EventHandler(this.Trackbar_wallWidth_Scroll);
        this.trackbar_wallHeight.Scroll += new System.EventHandler(this.Trackbar_wallHeight_Scroll);
        this.trackbar_profileLength.Scroll += new System.EventHandler(this.Trackbar_ProfileLength_Scroll);
        this.trackbar_hubInnerWidth.Scroll += new System.EventHandler(this.Trackbar_hubInnerWidth_Scroll);
        this.trackbar_hubOuterWidth.Scroll += new System.EventHandler(this.Trackbar_hubOuterWidth_Scroll);
        this.trackbar_minPoints.Scroll += new System.EventHandler(this.Trackbar_minPoints_Scroll);
        this.trackbar_maxPoints.Scroll += new System.EventHandler(this.Trackbar_maxPoints_Scroll);
        this.trackbar_minProfiles.Scroll += new System.EventHandler(this.Trackbar_minProfiles_Scroll);
        this.trackbar_maxProfiles.Scroll += new System.EventHandler(this.Trackbar_maxProfiles_Scroll);

        Controls.Add(this.label_wallWidth);
        Controls.Add(this.label_wallHeight);
        Controls.Add(this.label_profileLength);
        Controls.Add(this.label_hubInnerWidth);
        Controls.Add(this.label_hubOuterWidth);
        Controls.Add(this.label_minPoints);
        Controls.Add(this.label_maxPoints);
        Controls.Add(this.label_minProfiles);
        Controls.Add(this.label_maxProfiles);

        Controls.Add(this.trackbar_wallWidth);
        Controls.Add(this.trackbar_wallHeight);
        Controls.Add(this.trackbar_profileLength);
        Controls.Add(this.trackbar_hubInnerWidth);
        Controls.Add(this.trackbar_hubOuterWidth);
        Controls.Add(this.trackbar_minPoints);
        Controls.Add(this.trackbar_maxPoints);
        Controls.Add(this.trackbar_minProfiles);
        Controls.Add(this.trackbar_maxProfiles);

        Controls.Add(this.textBox_wallWidth);
        Controls.Add(this.textBox_wallHeight);
        Controls.Add(this.textBox_profileLength);
        Controls.Add(this.textBox_hubInnerWidth);
        Controls.Add(this.textBox_hubOuterWidth);
        Controls.Add(this.textBox_minPoints);
        Controls.Add(this.textBox_maxPoints);
        Controls.Add(this.textBox_minProfiles);
        Controls.Add(this.textBox_maxProfiles);

        this.checkBox_generateSymmetrical.Location = new Point(this.paddingX + 6, this.paddingY + 555);
        this.checkBox_generateSymmetrical.Size = new Size(this.offsetX - this.paddingX - 6, 20);
        this.checkBox_generateSymmetrical.Text = "Generate Symmetrical";
        this.checkBox_generateSymmetrical.CheckedChanged += new System.EventHandler(this.CheckBox_generateSymmetrical_Click);
        Controls.Add(this.checkBox_generateSymmetrical);

        this.checkBox_showSteps.Location = new Point(this.paddingX + 6, this.paddingY + 575);
        this.checkBox_showSteps.Size = new Size(this.offsetX - this.paddingX - 6, 20);
        this.checkBox_showSteps.Text = "Render individual steps";
        this.checkBox_showSteps.CheckedChanged += new System.EventHandler(this.CheckBox_showSteps_Click);
        Controls.Add(this.checkBox_showSteps);

        this.checkBox_allowSingleDegree.Location = new Point(this.paddingX + 6, this.paddingY + 595);
        this.checkBox_allowSingleDegree.Size = new Size(this.offsetX - this.paddingX - 6, 20);
        this.checkBox_allowSingleDegree.Text = "Allow points with single edge";
        this.checkBox_allowSingleDegree.CheckedChanged += new System.EventHandler(this.CheckBox_allowSingleDegree_Click);
        Controls.Add(this.checkBox_allowSingleDegree);

        this.button_generate.Location = new Point(this.paddingX + 5, this.paddingY + 640);
        this.button_generate.Text = "Generate";
        this.button_generate.Click += new System.EventHandler(this.Button_generate_Click);
        Controls.Add(this.button_generate);

        this.label_generate.Location = new Point(this.paddingX + 10, this.paddingY + 665);
        this.label_generate.Size = new Size(this.offsetX - this.paddingX - 10, 20);
        Controls.Add(this.label_generate);
    }

    #endregion
}
