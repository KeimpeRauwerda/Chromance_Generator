using Chromance_Generator.Generator;

namespace Chromance_Generator;

public partial class Form1 : Form
    {
    private int paddingX = 10;
    private int paddingY = 10;
    private int offsetX = 200;
    private int offsetY = 0;
    private int canvasWidth = 1200;
    private int canvasHeight;
    private int windowWidth;
    private int windowHeight;
    private float wallWidth = 180;
    private float wallHeight = 102.77f;
    private HexagonGrid hexagonGrid;
    public Form1()
    {
        this.windowWidth = this.canvasWidth + offsetX + paddingX * 2;
        this.windowHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight) + offsetY + paddingY * 2);

        InitializeComponent(windowWidth, windowHeight);
        this.hexagonGrid = new HexagonGrid(offsetX + paddingX, offsetY + paddingY, wallWidth, wallHeight);
    }

    private void Form1_Paint(object sender, PaintEventArgs pe)
    {
        Graphics GFX = pe.Graphics;
        // hexagonGrid.GenerateGrid(15, 25, 0, 40);
        hexagonGrid.Draw(GFX, this.canvasWidth);
    }

    private void ResizeCanvas(object sender, EventArgs e) {
        this.windowWidth = this.ClientSize.Width;
        this.windowHeight = this.ClientSize.Height;

        this.canvasWidth = this.windowWidth - offsetX - paddingX * 2;
        this.canvasHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight));

        if(windowHeight < this.canvasHeight + offsetY + paddingY * 2)
            this.canvasWidth = (int)((this.windowHeight - offsetY - paddingY * 2) / this.wallHeight * wallWidth);

        this.Refresh();
    }



    private async void generateButton_Click(object sender, EventArgs e) {
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
        hexagonGrid.GenerateGrid(15, 25, 0, 40); 
    }
}
