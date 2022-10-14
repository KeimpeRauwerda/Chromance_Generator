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
            x + r * (float)Math.Cos(i * 60 * Math.PI / 180f), 
            y + r * (float)Math.Sin(i * 60 * Math.PI / 180f));
    }

    GFX.FillPolygon(this.brush, shape);    
  }
}

public class Line {

}