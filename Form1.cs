using Chromance_Generator.Generator;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Chromance_Generator;

public partial class Form1 : Form
    {
    delegate void SetTextCallbackDelegate(string text);
    public Button button_generate = new Button();
    public Label label_generate = new Label();
    public TrackBar trackbar_wallWidth = new TrackBar();
    public TrackBar trackbar_wallHeight = new TrackBar();
    public TrackBar trackbar_profileLength = new TrackBar();
    public TrackBar trackbar_hubInnerWidth = new TrackBar();
    public TrackBar trackbar_hubOuterWidth = new TrackBar();
    
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
    private HexagonGrid hexagonGrid;
    public Form1()
    {
        ResizeWindow();
        InitializeComponent(windowWidth, windowHeight);
        this.hexagonGrid = new HexagonGrid(offsetX + paddingX, offsetY + paddingY, wallWidth, wallHeight);
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
        this.hexagonGrid.GenerateGrid(15, 25, 0, 40, this);
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

    private void Trackbar_wallWidth_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallWidth = trackBar.Value;
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight);
        this.ResizeCanvas();
    }
    private void Trackbar_wallHeight_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallHeight = trackBar.Value;
        this.hexagonGrid.UpdateGrid(this.wallWidth, this.wallHeight);
        this.ResizeCanvas();
    }
    private void Trackbar_ProfileSize_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallWidth = trackBar.Value;
    }
    private void Trackbar_hubInnerSize_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallWidth = trackBar.Value;
    }
    private void Trackbar_hubOuterSize_Scroll(object sender, EventArgs e) {
        TrackBar trackBar = (TrackBar)sender;
        this.wallWidth = trackBar.Value;
    }
}
