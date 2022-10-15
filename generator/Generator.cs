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

    private List<Point> points;

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

        points = new List<Point>();
        GeneratePoints();
    }

    public void GeneratePoints() {
        this.gridWidth = CalculateGridWidth(maxWidth, profileLength, hubInnerWidth, hubOuterWidth, oddWidthOffset);
        this.gridHeight = CalculateGridHeight(maxHeight, profileLength, hubInnerWidth, hubOuterWidth);

        Vector2 position;

        DateTime now = DateTime.Now;
        for (int x = 0; x < this.gridWidth; x++) {
            for (int y = 0; y < this.gridHeight; y++) {
                float hexRadius = this.hubDiameter / 2;
                position = GridPointToWorldPoint(x, y, hexRadius);
                
                if (position.X + hubOuterWidth / 2 > maxWidth)
                    continue;
                if (position.Y + hubDiameter / 2 > maxHeight)
                    continue;
                
                points.Add(new Point(x, y, position, points));
            }
        }
        var time = (DateTime.Now - now).TotalMilliseconds;
    }

    public int CalculateGridWidth(float maxWidth, float profileLength, float hubInnerWidth, float hubOuterWidth, float oddWidthOffset) {
        double hexagonWidth = profileLength + hubInnerWidth;
        return 1 + (int)((maxWidth - hubOuterWidth) / hexagonWidth);
    }

    public int CalculateGridHeight(float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth) {
        double hexagonHeight = (profileLength + hubInnerWidth) / 2 * Math.Sqrt(3);
        return 1 + (int)((maxHeight - hubDiameter) / hexagonHeight);
    }

    private Border border = new Border();
    private Hexagon hexagon = new Hexagon();
    private Line line = new Line();

    public void Draw(float width) {
        border.Draw(GFX, this.offsetX, this.offsetY, width, width / this.maxWidth * this.maxHeight);
        float scale = width / maxWidth;

        foreach (var point in points)
            hexagon.Draw(GFX, point.position.X * scale + offsetX, point.position.Y * scale + offsetY, this.hubDiameter / 2 * scale);

        foreach (var point in points) {
            foreach (var adjacentPoint in point.adjacentPoints) {
                if (point.Connect(adjacentPoint)) {
                    line.Draw(GFX, 
                        point.position.X * scale + offsetX,
                        point.position.Y * scale + offsetX,
                        adjacentPoint.position.X * scale + offsetX,
                        adjacentPoint.position.Y * scale + offsetX,
                        this.profileWidth * scale
                    );
                }
            }
        }
    }

    public Vector2 GridPointToWorldPoint(int x, int y, float hexRadius) {
        Vector2 worldPoint = new Vector2();
        float oddOffset = 0;

        // float gridSpacingX = (this.profileLength + this.hubInnerWidth) * scale;
        // TODO: Figure out grid spacing and then transform them based on node size
        float gridSpacingX = (profileLength + hubInnerWidth);
        float gridSpacingY = (float)(gridSpacingX / 2 * Math.Sqrt(3));

        if (y % 2 == 0)
            oddOffset = gridSpacingX / 2;

        worldPoint.X = (x * gridSpacingX) + oddOffset + (this.hubOuterWidth / 2);
        worldPoint.Y = (float)(y * gridSpacingY) + (this.hubDiameter / 2);

        return worldPoint;
    }
}

public class Point {
    private Vector2 gridPosition;
    public Vector2 position;
    public List<Point> adjacentPoints;
    public List<Point> connectedPoints;
    
    public Point(int x, int y, Vector2 position, List<Point> points) {
        this.gridPosition = new Vector2(x, y);
        this.position = position;
        this.adjacentPoints = new List<Point>();
        this.connectedPoints = new List<Point>();

        FindAdjacened(points);
    }

    public void FindAdjacened(List<Point> points) {
        List<Point> adjacentPoints;
        int offset = 0;
        
        if (this.gridPosition.Y % 2 == 1) {
            offset = 1;
        }

        adjacentPoints = points.Where(p =>
            ((p.gridPosition.Y == this.gridPosition.Y) && (p.gridPosition.X == this.gridPosition.X - 1 || p.gridPosition.X == this.gridPosition.X + 1)) ||
            ((p.gridPosition.Y == this.gridPosition.Y + 1) && (p.gridPosition.X == this.gridPosition.X - offset || p.gridPosition.X == this.gridPosition.X + 1 - offset)) ||
            ((p.gridPosition.Y == this.gridPosition.Y - 1) && (p.gridPosition.X == this.gridPosition.X - offset || p.gridPosition.X == this.gridPosition.X + 1 - offset))
        ).ToList();
        
        foreach (var point in adjacentPoints) {
            AddAdjacent(point);
            point.AddAdjacent(this);
        }
    }

    public bool AddAdjacent(Point point) {
        if (this.adjacentPoints.Contains(point))
            return false;
        this.adjacentPoints.Add(point);
        point.adjacentPoints.Add(this);
        return true;
    }

    public bool RemoveAdjacent(Point point) {
        if (!this.adjacentPoints.Contains(point))
            return false;
        this.adjacentPoints.Remove(point);
        point.adjacentPoints.Remove(this);
        return true;
    }

    public bool Connect(Point point) {
        if (this.connectedPoints.Contains(point))
            return false;
        this.connectedPoints.Add(point);
        point.connectedPoints.Add(this);
        return true;
    }

    public bool Disconnect(Point point) {
        if (!this.connectedPoints.Contains(point))
            return false;
        this.connectedPoints.Remove(point);
        point.connectedPoints.Remove(this);
        return true;
    }
}