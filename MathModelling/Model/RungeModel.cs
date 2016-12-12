namespace MathModelling.Model
{
    public delegate double MyDelegate(double x, double y);
    public delegate double answer(double x);

    public class RungeModel
    {
        public static DiscreteFunc RK4(MyDelegate del, double x_0, double y_0, double b, double h)
        {
            DiscreteFunc dicr = new DiscreteFunc();
            double[] arg = new double[4];
            int i = 0;
            double x, y;
            x = x_0;
            y = y_0;
            dicr[i] = new Point(x, y) ;
            i++;
            while (x <= b)
            {
                arg[0] = del(x, y);
                arg[1] = del(x + h / 2, y + h * arg[0] / 2);
                arg[2] = del(x + h / 2, y + h * arg[1] / 2);
                arg[3] = del(x + h, y + h * arg[2]);
                y += h * (arg[0] + 2 * arg[1] + 2 * arg[2] + arg[3]) / 6;
                x += h;
                dicr[i] = new Point(x, y);
                i++;
            }
            return dicr;
        }
    }
}
