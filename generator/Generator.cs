using Chromance_Generator.Shapes;
using System.Numerics;

namespace Chromance_Generator.Generator;

public class HexagonGrid
{
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

    public List<Point> points;
    public List<Line> lines;

    public HexagonGrid(int offsetX, int offsetY, float maxWidth, float maxHeight, float profileLength = 25, float profileWidth = 1.7f)
    {
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
        lines = new List<Line>();
    }

    private Shapes.Border shape_border = new Shapes.Border();
    private Shapes.Hexagon shape_hexagon = new Shapes.Hexagon();
    private Shapes.Line shape_line = new Shapes.Line();

    public void Draw(Graphics GFX, float width) {
        shape_border.Draw(GFX, this.offsetX, this.offsetY, width, width / this.maxWidth * this.maxHeight);
        float scale = width / maxWidth;

        foreach (var point in points)
            shape_hexagon.Draw(GFX, point.position.X * scale + offsetX, point.position.Y * scale + offsetY, this.hubDiameter / 2 * scale);
        
        foreach (var line in lines) {
            shape_line.Draw(GFX,
                line.points[0].position.X * scale + offsetX,
                line.points[0].position.Y * scale + offsetY,
                line.points[1].position.X * scale + offsetX,
                line.points[1].position.Y * scale + offsetY,
                this.profileWidth * scale
            );
        }
    }
    
    public void GenerateGrid() {
        this.points = GeneratePoints();
        this.lines = GenerateLines(points);

        if (lines.Count == 0)
            return;
        
        foreach (var point in points.Where(p => p.connectedPoints.Count == 0).ToList())
            points.Remove(point);
    }

    public List<Point> GeneratePoints() {
        this.gridWidth = CalculateGridWidth(maxWidth, profileLength, hubInnerWidth, hubOuterWidth, oddWidthOffset);
        this.gridHeight = CalculateGridHeight(maxHeight, profileLength, hubInnerWidth, hubOuterWidth);

        var points = new List<Point>();
        Vector2 position;

        for (int x = 0; x < this.gridWidth; x++) {
            for (int y = 0; y < this.gridHeight; y++) {
                float hexRadius = this.hubDiameter / 2;
                position = GridPointToWorldPoint(x, y, hexRadius);
                
                if (position.X + hubOuterWidth / 2 > maxWidth)
                    continue;
                if (position.Y + hubDiameter / 2 > maxHeight)
                    continue;
                
                points.Add(new Point(this, x, y, position, points));
            }
        }
        return points;
    }

    public List<Line> GenerateLines(List<Point> points) {
        var lines = new List<Line>();
        Random random = new Random();
        
        // foreach (var point in points) {
        //     foreach (var adjacentPoint in point.adjacentPoints) {
        //         point.Connect(adjacentPoint);
        //     }
        // }
        
        Point startPoint = points.OrderBy(n => random.NextDouble()).First();
        int iteration = 0;
        (bool done, bool success) = GenerateLinesRecursive(random, startPoint, points, lines, ref iteration);
        
        if (!success)
            ClearGrid(points, lines);

        return lines;
    }

    public (bool, bool) GenerateLinesRecursive(Random random, Point point, List<Point> points, List<Line> lines, ref int iteration) {
        iteration++;

        if (isValid(points, lines))
            return (true, true);

        if (iteration >= 100000)
            return (true, false);

        List<Point> r_adjacentPoints = point.adjacentPoints.OrderBy(n => random.NextDouble()).ToList();
        
        foreach (var adjacentPoint in r_adjacentPoints) {
            if (point.Connect(adjacentPoint, lines)) {
                (bool done, bool success) = GenerateLinesRecursive(random, adjacentPoint, points, lines, ref iteration);
                if(done)
                    return (done, success);
                else
                    point.Disconnect(adjacentPoint, lines);
            }
        }

        return (false, false);
    }

    public bool isValid(List<Point> points, List<Line> lines) {
        if (lines.Count < 40)
            return false;
        
        if (points.Any(p => p.connectedPoints.Count == 1))
            return false;

        return true;
    }

    public void ClearGrid(List<Point> points, List<Line> lines) {
        foreach (var point in points) {
            for (int i = point.connectedPoints.Count; i > 0; i--) {
                point.Disconnect(point.connectedPoints.ElementAt(i - 1), lines);
            }
        }
    }

    public int CalculateGridWidth(float maxWidth, float profileLength, float hubInnerWidth, float hubOuterWidth, float oddWidthOffset) {
        double hexagonWidth = profileLength + hubInnerWidth;
        return 1 + (int)((maxWidth - hubOuterWidth) / hexagonWidth);
    }

    public int CalculateGridHeight(float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth) {
        double hexagonHeight = (profileLength + hubInnerWidth) / 2 * Math.Sqrt(3);
        return 1 + (int)((maxHeight - hubDiameter) / hexagonHeight);
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
    private HexagonGrid hexagonGrid;
    private Vector2 gridPosition;
    public Vector2 position;
    public List<Point> adjacentPoints;
    public List<Point> connectedPoints;
    
    public Point(HexagonGrid hexagonGrid, int x, int y, Vector2 position, List<Point> points) {
        this.hexagonGrid = hexagonGrid;
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

    public bool Connect(Point point, List<Line> lines) {
        if (this.connectedPoints.Contains(point))
            return false;
        this.connectedPoints.Add(point);
        point.connectedPoints.Add(this);
        lines.Add(new Line(this, point));
        return true;
    }

    public bool Disconnect(Point point, List<Line> lines) {
        if (!this.connectedPoints.Contains(point))
            return false;
        this.connectedPoints.Remove(point);
        point.connectedPoints.Remove(this);
        lines.Remove(lines.Where(l => l.points.Contains(this) && l.points.Contains(point)).First());
        return true;
    }
}

public class Line {
    public Point[] points;
    public Line(Point point1, Point point2) {
        points = new Point[2] {
            point1,
            point2
        };
    }
}