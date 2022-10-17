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
        
        InitializeGenerateButton();
    }

    private void InitializeGenerateButton() {
        Button generateButton = new Button();
        generateButton.Location = new Point(10, 10);
        generateButton.Text = "Generate";
        generateButton.Click += new System.EventHandler(this.generateButton_Click);
        Controls.Add(generateButton);
    }

    #endregion
}
