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
    private bool generating;
    private bool allowSingleDegree;
    private bool generateSymmetrical;

    public int gridWidth;
    public int gridHeight;

    public List<Point> points;
    public List<Line> lines;

    public HexagonGrid(int offsetX, int offsetY, float maxWidth, float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth, bool allowSingleDegree, bool generateSymmetrical, float profileWidth = 1.7f)
    {
        this.offsetX = offsetX;
        this.offsetY = offsetY;

        this.oddWidthOffset = (profileLength + hubInnerWidth) / 2;
        this.generating = false;

        points = new List<Point>();
        lines = new List<Line>();

        this.UpdateGrid(maxWidth, maxHeight, profileLength, hubInnerWidth, hubOuterWidth, allowSingleDegree, generateSymmetrical, profileWidth);
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

    public void UpdateGrid(float maxWidth, float maxHeight, float profileLength, float hubInnerWidth, float hubOuterWidth, bool allowSingleDegree, bool generateSymmetrical, float profileWidth = 1.7f) {
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;
        this.profileLength = profileLength;
        this.profileWidth = profileWidth;
        this.hubInnerWidth = hubInnerWidth;
        this.hubOuterWidth = hubOuterWidth;
        this.hubDiameter = (float)(this.hubOuterWidth / Math.Sqrt(3) * 2);
        this.allowSingleDegree = allowSingleDegree;
        this.generateSymmetrical = generateSymmetrical;

        this.ClearGrid();
        this.points = GeneratePoints(generateSymmetrical);
    }
    
    public void GenerateGrid(int minPoints, int maxPoints, int minProfiles, int maxProfiles, bool generateSymmetrical, bool showSteps, Form1 form) {
        this.generating = true;
        form.SetLabel("Generating points");
        
        this.points = GeneratePoints(generateSymmetrical);
        
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
        List<Point> validPoints = new List<Point>();
        Point startPoint = points.OrderBy(n => random.NextDouble()).First();

        if (generateSymmetrical) {
            List<Point> potentialPoints = new List<Point>();
            if (this.gridWidth % 2 == 0) {
                potentialPoints = points.Where(p => p.gridPosition.Y % 2 == 0 && p.gridPosition.X == (this.gridWidth / 2) - 1).ToList();
            }
            else {
                potentialPoints = points.Where(p => p.gridPosition.Y % 2 == 1 && p.gridPosition.X == (this.gridWidth - 1) / 2).ToList();
            }
            startPoint = potentialPoints.OrderBy(n => random.NextDouble()).First();
        }

        validPoints.Add(startPoint);
        this.lines = new List<Line>();

        var success = false;
        for (int i = 0; i < 10 && !success; i++) {
            form.SetLabel("Generating lines (cycle " + i + " / " + 10 + ")");

            MakeValid(validPoints, this.lines, this.allowSingleDegree, startPoint);
            if (showSteps && i > 0 && !this.allowSingleDegree) {
                Thread.Sleep(500);
                form.ForceRedraw();
            }
            (success, this.lines) = GenerateLines(validPoints, minPoints, maxPoints, minProfiles, maxProfiles, generateSymmetrical);
            if (showSteps) {
                Thread.Sleep(500);
                form.ForceRedraw();
            }
        }
        this.generating = false;

        if (!success)
            ClearGrid();

        if (success && generateSymmetrical) {
            MirrorGrid(this.points, this.lines);
        }

        if (this.lines.Count == 0) {
            form.SetLabel("Failed to generate lines");
            return;
        }

        form.SetLabel("Removing empty points");
        
        foreach (var point in points.Where(p => p.connectedPoints.Count == 0).ToList())
            points.Remove(point);
        form.SetLabel("Successfully generated grid!");
    }

    public List<Point> GeneratePoints(bool generateSymmetrical) {
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
                
                if (generateSymmetrical) {
                    if (y % 2 == 0 && x == this.gridWidth - 1)
                        continue;
                }

                if (generateSymmetrical) {
                    if (y % 2 == 0 && this.gridWidth % 2 == 0 && x == this.gridWidth - 1)
                        continue;
                }
                
                points.Add(new Point(this, x, y, position, points));
            }
        }
        
        return points;
    }

    public (bool, List<Line>) GenerateLines(List<Point> validPoints, int minPoints, int maxPoints, int minProfiles, int maxProfiles, bool generateSymmetrical) {
        Random random = new Random();
        int iteration = 0;
        (bool done, bool success) = GenerateLinesRecursive(random, validPoints, lines, minPoints, maxPoints, minProfiles, maxProfiles, generateSymmetrical, ref iteration);
        
        return (success, lines);
    }

    public (bool, bool) GenerateLinesRecursive(Random random, List<Point> validPoints, List<Line> lines, int minPoints, int maxPoints, int minProfiles, int maxProfiles, bool generateSymmetrical, ref int iteration) {
        iteration++;

        if (iteration >= 100000)
            return (true, false);

        if (IsIllegal(validPoints, lines, maxPoints, maxProfiles, generateSymmetrical))
            return (false, false);

        if (IsValid(validPoints, lines, minPoints, minProfiles, generateSymmetrical))
            return (true, true);

        while (true) {
            Point r_validPoint = validPoints.OrderBy(n => random.NextDouble()).First();
            List<Point> r_adjacentPoints = r_validPoint.adjacentPoints.OrderBy(n => random.NextDouble()).ToList();
            if (r_adjacentPoints.Count == 0)
                return (false, false);
            Point r_adjacentPoint = r_adjacentPoints.First();
            
            if (generateSymmetrical) {
                bool foundValid = false;
                foreach (var adjacentPoint in r_adjacentPoints) {
                    if (adjacentPoint.gridPosition.Y % 2 == 0 && !(adjacentPoint.gridPosition.X + 1 > gridWidth / 2)) {
                        foundValid = true;
                        r_adjacentPoint = adjacentPoint;
                        break;
                    }
                    if (adjacentPoint.gridPosition.Y % 2 == 1 && !(r_adjacentPoint.gridPosition.X + 1 > (gridWidth + 1) / 2)) {
                        foundValid = true;
                        r_adjacentPoint = adjacentPoint;
                        break;
                    }
                }

                if (!foundValid)
                    return (false, false);
            }

            if (r_validPoint.Connect(r_adjacentPoint, lines)) {
                if (!validPoints.Contains(r_adjacentPoint))
                    validPoints.Add(r_adjacentPoint);

                (bool done, bool success) = GenerateLinesRecursive(random, validPoints, lines, minPoints, maxPoints, minProfiles, maxProfiles, generateSymmetrical, ref iteration);

                if(done) {
                    return (done, success);
                }
                else {
                    if(r_validPoint.Disconnect(r_adjacentPoint, lines)) {
                        if (validPoints.Contains(r_adjacentPoint) && r_adjacentPoint.connectedPoints.Count == 0)
                            validPoints.Remove(r_adjacentPoint);
                    }
                }
            }
        }
    }

    public bool IsIllegal(List<Point> validPoints, List<Line> lines, int maxPoints, int maxProfiles, bool generateSymmetrical) {
        if (!generateSymmetrical && lines.Count > maxProfiles)
            return true;
        
        if (!generateSymmetrical && validPoints.Count > maxPoints)
            return true;

        if (generateSymmetrical && lines.Count > maxProfiles / 2)
            return true;
            
        if (generateSymmetrical) {
            int totalPoints = validPoints.Count * 2;
            foreach(var point in validPoints.Where(p => (gridWidth % 2 == 0 && p.gridPosition.Y % 2 == 0 && p.gridPosition.X + 1 == gridWidth / 2) || (gridWidth % 2 == 1 && p.gridPosition.Y % 2 == 1 && p.gridPosition.X + 1 == (gridWidth + 1) / 2) )) {
                totalPoints--;
            }
            if (totalPoints > maxPoints)
                return true;
        }
        
        if (generateSymmetrical && !validPoints.Any(p => (gridWidth % 2 == 0 && p.gridPosition.Y % 2 == 0 && p.gridPosition.X + 1 == gridWidth / 2) || (gridWidth % 2 == 1 && p.gridPosition.Y % 2 == 1 && p.gridPosition.X + 1 == (gridWidth + 1) / 2) ))
            return true;

        return false;
    }


    public bool IsValid(List<Point> validPoints, List<Line> lines, int minPoints, int minProfiles, bool generateSymmetrical) {
        if (!generateSymmetrical && lines.Count < minProfiles)
            return false;
        
        if (!generateSymmetrical && validPoints.Count < minPoints)
            return false;

        if (generateSymmetrical && lines.Count < minProfiles / 2)
            return false;

        if (generateSymmetrical) {
            int totalPoints = validPoints.Count * 2;
            foreach(var point in validPoints.Where(p => (gridWidth % 2 == 0 && p.gridPosition.Y % 2 == 0 && p.gridPosition.X + 1 == gridWidth / 2) || (gridWidth % 2 == 1 && p.gridPosition.Y % 2 == 1 && p.gridPosition.X + 1 == (gridWidth + 1) / 2) )) {
                totalPoints--;
            }
            if (totalPoints <= minPoints)
                return false;
        }
        
        if (!this.allowSingleDegree && validPoints.Any(p => p.connectedPoints.Count == 1))
            return false;

        return true;
    }

    private void MakeValid(List<Point> validPoints, List<Line> lines, bool allowSingleDegree, Point startPoint) {
        if (allowSingleDegree)
            return;
        
        while (validPoints.Where(p => p.connectedPoints.Count == 1).ToList().Count != 0) {
            var point = validPoints.Where(p => p.connectedPoints.Count == 1).First();
            for (int i = point.connectedPoints.Count; i > 0; i--) {
                if (point.connectedPoints.ElementAt(i - 1) == startPoint)
                    continue;
                point.Disconnect(point.connectedPoints.ElementAt(i - 1), lines);
            }
            validPoints.Remove(point);
        }

        // Quick and dirty fix for resetting points whe. Can't be bothered to find the real problem.
        if (validPoints.Count == 1) {
            Point point = validPoints.First();
            for (int i = point.connectedPoints.Count; i > 0; i--) {
                if (point.connectedPoints.ElementAt(i - 1) == startPoint)
                    continue;
                point.Disconnect(point.connectedPoints.ElementAt(i - 1), lines);
            }
            validPoints.Clear();
            validPoints.Add(startPoint);
        }
    }
    public void ClearGrid() {
        foreach (var point in this.points) {
            for (int i = point.connectedPoints.Count; i > 0; i--) {
                point.Disconnect(point.connectedPoints.ElementAt(i - 1), this.lines);
            }
        }
    }
    
    public void MirrorGrid(List<Point> points, List<Line> lines) {
        List<Point> mirroredLinePoints = new List<Point>();
        for (int i = lines.Count; i > 0; i--) {
            List<Point> linePoints = lines[i - 1].points.ToList();
            mirroredLinePoints.Clear();
            foreach (var point in linePoints) {
                mirroredLinePoints.Add(GetMirroredPoint(point, points, this.gridWidth));
            }
            mirroredLinePoints[0].Connect(mirroredLinePoints[1], lines);
        }
    }

    private Point GetMirroredPoint(Point point, List<Point> points, int gridWidth) {
        if (point.gridPosition.Y % 2 == 0)
            return points.Where(p => p.gridPosition.X + 1 == gridWidth - (point.gridPosition.X + 1) && p.gridPosition.Y == point.gridPosition.Y).First();
        else
            return points.Where(p => p.gridPosition.X + 1 == gridWidth - point.gridPosition.X && p.gridPosition.Y == point.gridPosition.Y).First();
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
    public Vector2 gridPosition;
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