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

    private float hubOuterWidth = 5;
    private float hubInnerWidth = 3;
    
    private float profileLength;
    private float oddWidthOffset;

    public int gridWidth;
    public int gridHeight;
    private int[,] grid;

    public HexagonGrid(Graphics GFX, int offsetX, int offsetY, float maxWidth, float maxHeight, float profileLength = 25)
    {
        this.GFX = GFX;
        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;

        this.profileLength = profileLength;

        this.oddWidthOffset = (profileLength + hubInnerWidth) / 2;

        this.gridWidth = CalculateGridWidth(maxWidth, profileLength, hubInnerWidth, hubOuterWidth, oddWidthOffset);
        this.gridHeight = CalculateGridHeight(maxHeight, profileLength, hubInnerWidth, hubOuterWidth);

        this.grid = new int[this.gridWidth, this.gridHeight];
    }

    public int CalculateGridWidth(float maxWidth, float profileLength, float hubInnerWidth, float hubOuterWidth, float oddWidthOffset) {
        double hexagonWidth = profileLength + hubInnerWidth;
        return (int)((maxWidth - hubOuterWidth - oddWidthOffset) / hexagonWidth);
    }

    public int CalculateGridHeight(float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth) {
        double hexagonHeight = (profileLength + hubInnerWidth) / 2 * Math.Sqrt(3);
        return (int)((maxHeight - hubOuterWidth) / hexagonHeight);
    }

    private Border border = new Border();
    private Hexagon hexagon = new Hexagon();

    public void Draw(float width) {
        Vector2 screenPosition;
        float hexRadius = 15;

        border.Draw(GFX, this.offsetX, this.offsetY, width, width / this.maxWidth * this.maxHeight);

        for (int i = 0; i < this.gridWidth; i++) {
            for (int j = 0; j < this.gridHeight; j++) {
                screenPosition = gridpointToHexagonalScreenpoint(i, j, width, hexRadius);
                hexagon.Draw(GFX, screenPosition.X, screenPosition.Y, hexRadius);
            }
        }
    }

    public Vector2 gridpointToHexagonalScreenpoint(int x, int y, float width, float hexRadius) {
        Vector2 screenPosition = new Vector2();
        float scale = width / this.maxWidth;
        float oddOffset = 0;

        // float gridSpacingX = (this.profileLength + this.hubInnerWidth) * scale;
        // TODO: Figure out grid spacing and then transform them based on node size
        float gridSpacingX = 39 * scale;
        float gridSpacingY = (float)(gridSpacingX / 2 * Math.Sqrt(3));

        if (y % 2 == 1)
            oddOffset = gridSpacingX / 2;

        screenPosition.X = (x * gridSpacingX) + offsetX + oddOffset;
        screenPosition.Y = (float)(y * gridSpacingY) + offsetX;

        return screenPosition;
    }
}