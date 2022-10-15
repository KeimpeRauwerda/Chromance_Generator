namespace Chromance_Generator.Shapes;

public abstract class Shape {
    protected Brush brush = new SolidBrush(Color.Black);
}

public class Border : Shape {
    public void Draw(Graphics GFX, float x, float y, float width, float height) {
        GFX.DrawRectangle(Pens.Black, x, y, width, height);
    }
}
public class Hexagon : Shape {
  public void Draw(Graphics GFX, float x, float y, float r) {
    var shape = new PointF[6];

    for(int i=0; i < 6; i++)
    {
        shape[i] = new PointF(
            x + r * (float)Math.Cos(0.5 * Math.PI + i * 60 * Math.PI / 180f), 
            y + r * (float)Math.Sin(0.5 * Math.PI + i * 60 * Math.PI / 180f));
    }

    GFX.FillPolygon(this.brush, shape);    
  }
}

public class Line {
    public void Draw(Graphics GFX, int x1, int y1, int x2, int y2, float thickness) {
        GFX.DrawLine(new Pen(Color.Black, thickness), new Point(x1, y1), new Point(x2, y2));
    }
}