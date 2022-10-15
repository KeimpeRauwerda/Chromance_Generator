using Chromance_Generator.Shapes;
using System.Numerics;

namespace Chromance_Generator.Generator;

public class HexagonGrid
{
    private Graphics GFX;
    private int offsetX;
    private int offsetY;
    private float maxWidth;
    private float maxHeight;

    private float hubOuterWidth;
    private float hubInnerWidth;
    private float hubDiameter;
    
    private float profileLength;
    private float profileWidth;
    private float oddWidthOffset;

    public int gridWidth;
    public int gridHeight;
    private int[,] grid;

    public HexagonGrid(Graphics GFX, int offsetX, int offsetY, float maxWidth, float maxHeight, float profileLength = 25, float profileWidth = 1.7f)
    {
        this.GFX = GFX;
        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;

        this.hubOuterWidth = 5;
        this.hubInnerWidth = 3;
        this.hubDiameter = (float)(this.hubOuterWidth / Math.Sqrt(3) * 2);

        this.profileLength = profileLength;
        this.profileWidth = profileWidth;

        this.oddWidthOffset = (profileLength + hubInnerWidth) / 2;

        this.gridWidth = CalculateGridWidth(maxWidth, profileLength, hubInnerWidth, hubOuterWidth, oddWidthOffset);
        this.gridHeight = CalculateGridHeight(maxHeight, profileLength, hubInnerWidth, hubOuterWidth);

        this.grid = new int[this.gridWidth, this.gridHeight];
    }

    public int CalculateGridWidth(float maxWidth, float profileLength, float hubInnerWidth, float hubOuterWidth, float oddWidthOffset) {
        double hexagonWidth = profileLength + hubInnerWidth;
        return 1 + (int)((maxWidth - hubOuterWidth - oddWidthOffset) / hexagonWidth);
    }

    public int CalculateGridHeight(float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth) {
        double hexagonHeight = (profileLength + hubInnerWidth) / 2 * Math.Sqrt(3);
        return 1 + (int)((maxHeight - hubDiameter) / hexagonHeight);
    }

    private Border border = new Border();
    private Hexagon hexagon = new Hexagon();
    private Line line = new Line();

    public void Draw(float width) {
        Vector2 screenPosition;
        float scale = width / this.maxWidth;
        float hexRadius = this.hubDiameter / 2 * scale;

        border.Draw(GFX, this.offsetX, this.offsetY, width, width / this.maxWidth * this.maxHeight);

        for (int i = 0; i < this.gridWidth; i++) {
            for (int j = 0; j < this.gridHeight; j++) {
                screenPosition = gridpointToHexagonalScreenpoint(i, j, scale, hexRadius);
                hexagon.Draw(GFX, screenPosition.X, screenPosition.Y, hexRadius);
            }
        }

        // for (int i = 0; i < this.gridWidth; i++) {
        //     for (int j = 0; j < this.gridHeight; j++) {
        //         screenPosition = gridpointToHexagonalScreenpoint(i, j, scale, hexRadius);
        //         line.Draw(GFX, screenPosition.X, screenPosition.Y, hexRadius);
        //     }
        // }
    }

    public Vector2 gridpointToHexagonalScreenpoint(int x, int y, float scale, float hexRadius) {
        Vector2 screenPosition = new Vector2();
        float oddOffset = 0;

        // float gridSpacingX = (this.profileLength + this.hubInnerWidth) * scale;
        // TODO: Figure out grid spacing and then transform them based on node size
        float gridSpacingX = (profileLength + hubInnerWidth) * scale;
        float gridSpacingY = (float)(gridSpacingX / 2 * Math.Sqrt(3));

        if (y % 2 == 1)
            oddOffset = gridSpacingX / 2;

        screenPosition.X = (x * gridSpacingX) + offsetX + oddOffset + (this.hubOuterWidth / 2 * scale);
        screenPosition.Y = (float)(y * gridSpacingY) + offsetX + (this.hubDiameter / 2 * scale);

        return screenPosition;
    }
}

public class Point {
    private int[] gridPosition;
    public Vector2 position;
    public List<Point> connectedPoints;
    
    public Point(int x, int y) {
        this.gridPosition = new int[] {x, y};
        this.connectedPoints = new List<Point>();
    }
}