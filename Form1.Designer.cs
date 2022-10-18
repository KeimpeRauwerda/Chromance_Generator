namespace Chromance_Generator;

partial class Form1
{
    delegate void SetTextCallbackDelegate(string text);
    public TrackBar trackbar_wallWidth = new TrackBar();
    public TrackBar trackbar_wallHeight = new TrackBar();
    public TrackBar trackbar_profileLength = new TrackBar();
    public TrackBar trackbar_hubInnerWidth = new TrackBar();
    public TrackBar trackbar_hubOuterWidth = new TrackBar();

    public Label label_wallWidth = new Label();
    public Label label_wallHeight = new Label();
    public Label label_profileLength = new Label();
    public Label label_hubInnerWidth = new Label();
    public Label label_hubOuterWidth = new Label();

    public TextBox textBox_wallWidth = new TextBox();
    public TextBox textBox_wallHeight = new TextBox();
    public TextBox textBox_profileLength = new TextBox();
    public TextBox textBox_hubInnerWidth = new TextBox();
    public TextBox textBox_hubOuterWidth = new TextBox();

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

        this.trackbar_wallWidth.Location     = new Point(this.paddingX, this.paddingY + 20);
        this.trackbar_wallHeight.Location    = new Point(this.paddingX, this.paddingY + 80);
        this.trackbar_profileLength.Location = new Point(this.paddingX, this.paddingY + 140);
        this.trackbar_hubInnerWidth.Location = new Point(this.paddingX, this.paddingY + 200);
        this.trackbar_hubOuterWidth.Location = new Point(this.paddingX, this.paddingY + 260);

        this.textBox_wallWidth.Location     = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 20);
        this.textBox_wallHeight.Location    = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 80);
        this.textBox_profileLength.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 140);
        this.textBox_hubInnerWidth.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 200);
        this.textBox_hubOuterWidth.Location = new Point(this.offsetX - this.paddingX - 30, this.paddingY + 260);

        this.label_wallWidth.Size     = new Size(this.offsetX - this.paddingX, 20);
        this.label_wallHeight.Size    = new Size(this.offsetX - this.paddingX, 20);
        this.label_profileLength.Size = new Size(this.offsetX - this.paddingX, 20);
        this.label_hubInnerWidth.Size = new Size(this.offsetX - this.paddingX, 20);
        this.label_hubOuterWidth.Size = new Size(this.offsetX - this.paddingX, 20);

        this.trackbar_wallWidth.Size     = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_wallHeight.Size    = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_profileLength.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_hubInnerWidth.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        this.trackbar_hubOuterWidth.Size = new Size(this.offsetX - this.paddingX - 40, 45);
        
        this.textBox_wallWidth.Size     = new Size(40, 20);
        this.textBox_wallHeight.Size    = new Size(40, 20);
        this.textBox_profileLength.Size = new Size(40, 20);
        this.textBox_hubInnerWidth.Size = new Size(40, 20);
        this.textBox_hubOuterWidth.Size = new Size(40, 20);
        
        this.textBox_wallWidth.Text     = this.wallWidth.ToString();
        this.textBox_wallHeight.Text    = this.wallHeight.ToString();
        this.textBox_profileLength.Text = this.profileLength.ToString();
        this.textBox_hubInnerWidth.Text = this.hubInnerWidth.ToString();
        this.textBox_hubOuterWidth.Text = this.hubOuterWidth.ToString();
        
        this.label_wallWidth.Text     = "Wall width in cm";
        this.label_wallHeight.Text    = "Wall height in cm";
        this.label_profileLength.Text = "Profile length in cm";
        this.label_hubInnerWidth.Text = "Hub inner width in cm";
        this.label_hubOuterWidth.Text = "Hub outer width in cm";
        
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
        
        this.trackbar_wallWidth.Scroll += new System.EventHandler(this.Trackbar_wallWidth_Scroll);
        this.trackbar_wallHeight.Scroll += new System.EventHandler(this.Trackbar_wallHeight_Scroll);
        this.trackbar_profileLength.Scroll += new System.EventHandler(this.Trackbar_ProfileLength_Scroll);
        this.trackbar_hubInnerWidth.Scroll += new System.EventHandler(this.Trackbar_hubInnerWidth_Scroll);
        this.trackbar_hubOuterWidth.Scroll += new System.EventHandler(this.Trackbar_hubOuterWidth_Scroll);

        Controls.Add(this.label_wallWidth);
        Controls.Add(this.label_wallHeight);
        Controls.Add(this.label_profileLength);
        Controls.Add(this.label_hubInnerWidth);
        Controls.Add(this.label_hubOuterWidth);

        Controls.Add(this.trackbar_wallWidth);
        Controls.Add(this.trackbar_wallHeight);
        Controls.Add(this.trackbar_profileLength);
        Controls.Add(this.trackbar_hubInnerWidth);
        Controls.Add(this.trackbar_hubOuterWidth);

        Controls.Add(this.textBox_wallWidth);
        Controls.Add(this.textBox_wallHeight);
        Controls.Add(this.textBox_profileLength);
        Controls.Add(this.textBox_hubInnerWidth);
        Controls.Add(this.textBox_hubOuterWidth);

        this.button_generate.Location = new Point(this.paddingX, this.paddingY + 350);
        this.button_generate.Text = "Generate";
        this.button_generate.Click += new System.EventHandler(this.Button_generate_Click);
        Controls.Add(button_generate);

        this.label_generate.Location = new Point(this.paddingX + 5, this.paddingY + 375);
        this.label_generate.Size = new Size(this.offsetX - this.paddingX, 20);
        Controls.Add(label_generate);
    }

    #endregion
}
