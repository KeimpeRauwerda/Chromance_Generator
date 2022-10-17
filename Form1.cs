using Chromance_Generator.Generator;

namespace Chromance_Generator;

public partial class Form1 : Form
    {
    private int offsetX = 10;
    private int offsetY = 10;
    private int canvasWidth = 1200;
    private int canvasHeight;
    private int windowWidth;
    private int windowHeight;
    private float wallWidth = 180;
    private float wallHeight = 102.77f;
    private HexagonGrid hexagonGrid;
    public Form1()
    {
        this.windowWidth = this.canvasWidth + offsetX * 2;
        this.windowHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight) + offsetY * 2);

        InitializeComponent(windowWidth, windowHeight);
        this.hexagonGrid = new HexagonGrid(offsetX, offsetY, wallWidth, wallHeight);
    }

    private void Form1_Paint(object sender, PaintEventArgs pe)
    {
        Graphics GFX = pe.Graphics;
        hexagonGrid.GenerateGrid();
        hexagonGrid.Draw(GFX, this.canvasWidth);
    }

    private void ResizeCanvas(object sender, System.EventArgs e) {
        this.windowWidth = this.ClientSize.Width;
        this.windowHeight = this.ClientSize.Height;

        this.canvasWidth = this.windowWidth - offsetX * 2;
        this.canvasHeight = (int)((this.canvasWidth / this.wallWidth * this.wallHeight));

        if(windowHeight < this.canvasHeight + offsetY * 2)
            this.canvasWidth = (int)((this.windowHeight - offsetY * 2) / this.wallHeight * wallWidth);

        this.Refresh();
    }
}
