using Chromance_Generator.Generator;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Chromance_Generator;

public partial class Form1 : Form
    {
    private int paddingX = 10;
    private int paddingY = 10;
    private int offsetX = 200;
    private int offsetY = 0;
    private int canvasWidth = 1200;
    private int canvasHeight = 600;
    private int windowWidth;
    private int windowHeight;

    private float wallWidth = 180;
    private float wallHeight = 103;
    private float profileLength = 25;
    private float hubInnerWidth = 3;
    private float hubOuterWidth = 5;

    private int minPoints = 3;
    private int maxPoints = 25;
    private int minProfiles = 35;
    private int maxProfiles = 40;
    private HexagonGrid hexagonGrid;
    public Form1()
    {
        ResizeWindow();
        InitializeComponent(windowWidth, windowHeight);
        this.hexagonGrid = new HexagonGrid(this.offsetX + this.paddingX, this.offsetY + this.paddingY, this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
    }

    public void ResizeWindow() {
        this.windowHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight) + offsetY + paddingY * 2);
        if(windowHeight > this.canvasHeight + offsetY + paddingY * 2) {
            this.windowHeight = this.canvasHeight + offsetY + paddingY * 2;
            this.canvasWidth = (int)(this.canvasHeight / this.wallHeight * this.wallWidth);
        }
        this.windowWidth = this.canvasWidth + offsetX + paddingX * 2;
    }

    private void Form1_Paint(object sender, PaintEventArgs pe)
    {
        Graphics GFX = pe.Graphics;
        // hexagonGrid.GenerateGrid(15, 25, 0, 40);
        this.hexagonGrid.Draw(GFX, this.canvasWidth);
    }
    private void ResizeCanvas(object sender, EventArgs e) {
        ResizeCanvas();
    }

    private void ResizeCanvas() {
        this.windowWidth = this.ClientSize.Width;
        this.windowHeight = this.ClientSize.Height;

        this.canvasWidth = this.windowWidth - offsetX - paddingX * 2;
        this.canvasHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight));

        if(windowHeight < this.canvasHeight + offsetY + paddingY * 2)
            this.canvasWidth = (int)((this.windowHeight - offsetY - paddingY * 2) / this.wallHeight * wallWidth);
        
        this.Refresh();
    }

    private async void Button_generate_Click(object sender, EventArgs e) {
        var button = (Button)sender;
        button.Text = "Generating";
        button.Enabled = false;
        await GenerateGridAsync();
        button.Text = "Generate";
        button.Enabled = true;
        this.Refresh();
    }

    private async Task GenerateGridAsync() {
        await Task.Run(() => GenerateGrid());
    }

    private void GenerateGrid() {
        this.hexagonGrid.ClearGrid();
        this.hexagonGrid.GenerateGrid(this.minPoints, this.maxPoints, this.minProfiles, this.maxProfiles, this);
    }

    public void SetLabel(string text) {
        this.SetLabelTextCallback(text);
    }

    private void SetLabelTextCallback(string text) {
        if (this.label_generate.InvokeRequired) {
            SetTextCallbackDelegate t = new SetTextCallbackDelegate(SetLabelTextCallback);
            this.Invoke(t, new object [] { text });
        }
        else {
            this.label_generate.Text = text;
        }
    }

    public void ForceRedraw() {
        this.ForceRedrawCallback();
    }

    private void ForceRedrawCallback() {
        if (this.InvokeRequired) {
            SetRedrawCallbackDelegate t = new SetRedrawCallbackDelegate(ForceRedrawCallback);
            this.Invoke(t, new object[] {});
        }
        else {
            this.Refresh();
        }
    }

    private void Trackbar_wallWidth_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallWidth = trackBar.Value;
        this.textBox_wallWidth.Text = trackBar.Value.ToString();
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
        this.ResizeCanvas();
    }
    private void Trackbar_wallHeight_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallHeight = trackBar.Value;
        this.textBox_wallHeight.Text = trackBar.Value.ToString();
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
        this.ResizeCanvas();
    }
    private void Trackbar_ProfileLength_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.profileLength = trackBar.Value;
        this.textBox_profileLength.Text = trackBar.Value.ToString();
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
        this.ResizeCanvas();
    }
    private void Trackbar_hubInnerWidth_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.hubInnerWidth = trackBar.Value;
        this.textBox_hubInnerWidth.Text = trackBar.Value.ToString();
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
        this.ResizeCanvas();
    }
    private void Trackbar_hubOuterWidth_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.hubOuterWidth = trackBar.Value;
        this.textBox_hubOuterWidth.Text = trackBar.Value.ToString();
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight, this.profileLength, this.hubInnerWidth, this.hubOuterWidth);
        this.ResizeCanvas();
    }

    private void Trackbar_minPoints_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.minPoints = trackBar.Value;
        this.textBox_minPoints.Text = trackBar.Value.ToString();
        if (this.maxPoints < this.minPoints) {
            this.maxPoints = this.minPoints;
            this.trackbar_maxPoints.Value = this.minPoints;
            this.textBox_maxPoints.Text = this.minPoints.ToString();
        }
    }

    private void Trackbar_maxPoints_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.maxPoints = trackBar.Value;
        this.textBox_maxPoints.Text = trackBar.Value.ToString();
        if (this.minPoints > this.maxPoints) {
            this.minPoints = this.maxPoints;
            this.trackbar_minPoints.Value = this.maxPoints;
            this.textBox_minPoints.Text = this.maxPoints.ToString();
        }
    }

    private void Trackbar_minProfiles_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.minProfiles = trackBar.Value;
        this.textBox_minProfiles.Text = trackBar.Value.ToString();
        if (this.maxProfiles < this.minProfiles) {
            this.maxProfiles = this.minProfiles;
            this.trackbar_maxProfiles.Value = this.minProfiles;
            this.textBox_maxProfiles.Text = this.minProfiles.ToString();
        }
    }

    private void Trackbar_maxProfiles_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.maxProfiles = trackBar.Value;
        this.textBox_maxProfiles.Text = trackBar.Value.ToString();
        if (this.minProfiles > this.maxProfiles) {
            this.minProfiles = this.maxProfiles;
            this.trackbar_minProfiles.Value = this.maxProfiles;
            this.textBox_minProfiles.Text = this.maxProfiles.ToString();
        }
    }

}
