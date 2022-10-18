namespace Chromance_Generator;

partial class Form1
{
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
        this.trackbar_wallWidth.Size     = new Size(this.offsetX - this.paddingX, 45);
        this.trackbar_wallHeight.Size    = new Size(this.offsetX - this.paddingX, 45);
        this.trackbar_profileLength.Size = new Size(this.offsetX - this.paddingX, 45);
        this.trackbar_hubInnerWidth.Size = new Size(this.offsetX - this.paddingX, 45);
        this.trackbar_hubOuterWidth.Size = new Size(this.offsetX - this.paddingX, 45);

        this.trackbar_wallWidth.Location     = new Point(this.paddingX, this.paddingY + 0);
        this.trackbar_wallHeight.Location    = new Point(this.paddingX, this.paddingY + 50);
        this.trackbar_profileLength.Location = new Point(this.paddingX, this.paddingY + 100);
        this.trackbar_hubInnerWidth.Location = new Point(this.paddingX, this.paddingY + 150);
        this.trackbar_hubOuterWidth.Location = new Point(this.paddingX, this.paddingY + 200);
        
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
        this.trackbar_profileLength.Scroll += new System.EventHandler(this.Trackbar_ProfileSize_Scroll);
        this.trackbar_hubInnerWidth.Scroll += new System.EventHandler(this.Trackbar_hubInnerSize_Scroll);
        this.trackbar_hubOuterWidth.Scroll += new System.EventHandler(this.Trackbar_hubOuterSize_Scroll);

        Controls.Add(this.trackbar_wallWidth);
        Controls.Add(this.trackbar_wallHeight);
        Controls.Add(this.trackbar_profileLength);
        Controls.Add(this.trackbar_hubInnerWidth);
        Controls.Add(this.trackbar_hubOuterWidth);

        this.button_generate.Location = new Point(this.paddingX, this.paddingY + 250);
        this.button_generate.Text = "Generate";
        this.button_generate.Click += new System.EventHandler(this.Button_generate_Click);
        Controls.Add(button_generate);

        this.label_generate.Location = new Point(this.paddingX + 5, this.paddingY + 275);
        this.label_generate.Size = new Size(this.offsetX - this.paddingX, 20);
        Controls.Add(label_generate);
    }

    #endregion
}
