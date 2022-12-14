namespace Chromance_Generator.Shapes;

public abstract class Shape {
    protected Brush blackBrush = new SolidBrush(Color.Black);
    protected Brush redBrush = new SolidBrush(Color.Red);
}

public class Border : Shape {
    public void Draw(Graphics GFX, float x, float y, float width, float height) {
        GFX.DrawRectangle(Pens.Black, x, y, width, height);
    }
}
public class Hexagon : Shape {
    PointF[] shape = new PointF[6];
  public void Draw(Graphics GFX, float x, float y, float r) {

    for(int i=0; i < 6; i++)
    {
        shape[i].X = x + r * (float)Math.Cos(0.5 * Math.PI + i * 60 * Math.PI / 180f);
        shape[i].Y = y + r * (float)Math.Sin(0.5 * Math.PI + i * 60 * Math.PI / 180f);
    }

    GFX.FillPolygon(this.blackBrush, shape);
  }
}

public class Line {
    public void Draw(Graphics GFX, float x1, float y1, float x2, float y2, float thickness) {
        GFX.DrawLine(new Pen(Color.Black, thickness), x1, y1, x2, y2);
    }
}