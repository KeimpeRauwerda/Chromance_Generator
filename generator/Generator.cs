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
    private int minDegree;
    private bool generating;

    public int gridWidth;
    public int gridHeight;

    public List<Point> points;
    public List<Line> lines;

    public HexagonGrid(int offsetX, int offsetY, float maxWidth, float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth, float profileWidth = 1.7f)
    {
        this.offsetX = offsetX;
        this.offsetY = offsetY;

        this.oddWidthOffset = (profileLength + hubInnerWidth) / 2;

        this.minDegree = 2;
        this.generating = false;

        points = new List<Point>();
        lines = new List<Line>();

        this.UpdateGrid(maxWidth, maxHeight, profileLength, hubInnerWidth, hubOuterWidth, profileWidth);
    }

    private Shapes.Border shape_border = new Shapes.Border();
    private Shapes.Hexagon shape_hexagon = new Shapes.Hexagon();
    private Shapes.Line shape_line = new Shapes.Line();

    public void Draw(Graphics GFX, float width) {
        shape_border.Draw(GFX, this.offsetX, this.offsetY, width, width / this.maxWidth * this.maxHeight);
        float scale = width / maxWidth;
        
        // if (this.generating)
        //     return;

        foreach (var point in points)
            shape_hexagon.Draw(GFX, point.position.X * scale + offsetX, point.position.Y * scale + offsetY, this.hubDiameter / 2 * scale);
        
        try {
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
        catch {
            
        }
    }

    public void UpdateGrid(float maxWidth, float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth, float profileWidth = 1.7f) {
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;
        this.profileLength = profileLength;
        this.profileWidth = profileWidth;
        this.hubInnerWidth = hubInnerWidth;
        this.hubOuterWidth = hubOuterWidth;
        this.hubDiameter = (float)(this.hubOuterWidth / Math.Sqrt(3) * 2);

        this.ClearGrid();
        this.points = this.GeneratePoints();
    }
    
    public void GenerateGrid(int minPoints, int maxPoints, int minProfiles, int maxProfiles, Form1 form) {
        this.generating = true;
        form.SetLabel("Generating points");
        
        this.points = GeneratePoints();
        
        form.SetLabel("Validiating inputs");
        int maxPossibleProfiles = 0;
        foreach (var point in points) {
            foreach (var adjacentPoint in point.adjacentPoints) {
                maxPossibleProfiles++;
            }
        }
        maxPossibleProfiles = maxPossibleProfiles / 2;
        
        if (maxPoints < 3)
            maxPoints = 3;
        if (maxPoints > this.points.Count)
            maxPoints = this.points.Count;
        
        if (minPoints < 3)
            minPoints = 3;
        if (minPoints > maxPoints)
            minPoints = maxPoints;
        
        if (maxProfiles < 3)
            maxProfiles = 3;
        if (maxProfiles > maxPossibleProfiles)
            maxProfiles = maxPossibleProfiles;

        if (minProfiles < 3)
            minProfiles = 3;
        if (minProfiles > maxProfiles)
            minProfiles = maxProfiles;
        

        form.SetLabel("Generating lines");
        this.lines.Clear();
        
        Random random = new Random();
        Point startPoint = points.OrderBy(n => random.NextDouble()).First();
        List<Point> validPoints = new List<Point>() {startPoint};
        this.lines = new List<Line>();

        var success = false;
        for (int i = 0; i < 10 && !success; i++) {
            form.SetLabel("Generating lines (cycle " + i + " / " + 10 + ")");
            this.makeValid(validPoints, this.lines, this.minDegree);
            form.ForceRedraw();
            (success, this.lines) = GenerateLines(validPoints, minPoints, maxPoints, minProfiles, maxProfiles);
            form.ForceRedraw();
        }
        this.generating = false;

        if (!success)
            this.ClearGrid();

        if (lines.Count == 0) {
            form.SetLabel("Failed to generate lines");
            return;
        }

        form.SetLabel("Removing empty points");
        
        foreach (var point in points.Where(p => p.connectedPoints.Count == 0).ToList())
            points.Remove(point);
        form.SetLabel("Successfully generated grid!");
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

    public (bool, List<Line>) GenerateLines(List<Point> validPoints, int minPoints, int maxPoints, int minProfiles = 35, int maxProfiles = 40) {
        var DateTime = new DateTime(2001, 8, 7) - new DateTime(0);

        Random random = new Random();
        int iteration = 0;
        (bool done, bool success) = GenerateLinesRecursive(random, validPoints, lines, minPoints, maxPoints, minProfiles, maxProfiles, ref iteration);
        
        return (success, lines);
    }

    public (bool, bool) GenerateLinesRecursive(Random random, List<Point> validPoints, List<Line> lines, int minPoints, int maxPoints, int minProfiles, int maxProfiles, ref int iteration) {
        iteration++;

        if (iteration >= 100000)
            return (true, false);

        if (isIllegal(validPoints, lines, maxPoints, maxProfiles))
            return (false, false);

        if (isValid(validPoints, lines, minPoints, minProfiles))
            return (true, true);

        while (true) {
            Point r_validPoint = validPoints.OrderBy(n => random.NextDouble()).First();
            List<Point> r_adjacentPoints = r_validPoint.adjacentPoints.OrderBy(n => random.NextDouble()).ToList();
            Point adjacentPoint = r_adjacentPoints.First();

            if (r_validPoint.Connect(adjacentPoint, lines)) {
                if (!validPoints.Contains(adjacentPoint))
                    validPoints.Add(adjacentPoint);

                (bool done, bool success) = GenerateLinesRecursive(random, validPoints, lines, minPoints, maxPoints, minProfiles, maxProfiles, ref iteration);

                if(done) {
                    return (done, success);
                }
                else {
                    if(r_validPoint.Disconnect(adjacentPoint, lines)) {
                        if (validPoints.Contains(adjacentPoint) && adjacentPoint.connectedPoints.Count == 0)
                            validPoints.Remove(adjacentPoint);
                    }
                }
            }
        }
    }

    public bool isIllegal(List<Point> validPoints, List<Line> lines, int maxPoints, int maxProfiles) {
        if (lines.Count > maxProfiles)
            return true;
        
        if (validPoints.Count > maxPoints)
            return true;

        return false;
    }


    public bool isValid(List<Point> validPoints, List<Line> lines, int minPoints, int minProfiles) {
        if (lines.Count < minProfiles)
            return false;
        
        if (validPoints.Count < minPoints)
            return false;
        
        if (validPoints.Any(p => p.connectedPoints.Count == 1))
            return false;

        return true;
    }

    private void makeValid(List<Point> validPoints, List<Line> lines, int minDegree) {
        if (minDegree > 1) {
            while (validPoints.Where(p => p.connectedPoints.Count > 0 && p.connectedPoints.Count < minDegree).ToList().Count != 0) {
                var point = validPoints.Where(p => p.connectedPoints.Count > 0 && p.connectedPoints.Count < minDegree).First();
                for (int i = point.connectedPoints.Count; i > 0; i--) {
                    point.Disconnect(point.connectedPoints.ElementAt(i - 1), lines);
                }
                validPoints.Remove(point);
            }
        }
    }
    public void ClearGrid() {
        foreach (var point in this.points) {
            for (int i = point.connectedPoints.Count; i > 0; i--) {
                point.Disconnect(point.connectedPoints.ElementAt(i - 1), this.lines);
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

public class GeneratorOptions {
    
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