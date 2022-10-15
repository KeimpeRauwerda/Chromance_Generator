using Chromance_Generator.Generator;

namespace Chromance_Generator;

public partial class Form1 : Form
    {
    private int offsetX = 10;
    private int offsetY = 10;
    private int width = 1200;
    private float wallWidth = 200;
    private float wallHeight = 102.77f;
    public Form1()
    {
        int windowWidth = this.width + offsetX * 2;
        int windowHeight = (int)((this.width / this.wallWidth * this.wallHeight) + offsetY * 2);
        InitializeComponent(windowWidth, windowHeight);
    }

    private void Form1_Paint(object sender, PaintEventArgs pe)
    {
        Graphics GFX = pe.Graphics;
        HexagonGrid hexagonGrid = new HexagonGrid(GFX, offsetX, offsetY, wallWidth, wallHeight);
        hexagonGrid.Draw(this.width);
    }
}
