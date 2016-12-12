using MathModelling.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathModelling.UI.ViewModels
{
    public class RungeViewModel
    {
        private List<Point> points_orign;
        private List<Point> points_runge;
        private List<PointWith2Ys> points;
        private double a;
        private double b;
        private double h;

        public class PointWith2Ys {
            public double X { get; set; }
            public double YOR { get; set; }
            public double YRU { get; set; }
            public double YEI { get; set; }

        }
        public RungeViewModel()
        {
            points = new List<PointWith2Ys>();
            answer ans = new answer((x) => Math.Exp(Math.Cos(2 * x) - x / 2));
            MyDelegate f = new MyDelegate((x, y) => Math.Exp(Math.Cos(2 * x) - x / 2) * (-2 * Math.Sin(2 * x) - 0.5) +
                Math.Sin(5 * x) * Math.Exp(Math.Cos(2 * x) - x / 2) - Math.Sin(5 * x) * ans(x));
            double c;
            a = c = 2.3;
            b = 3.4;
            h = 0.1;
            points_runge =  RungeModel.RK4(f, a, ans(a), b, h).RungePoints;
            points_orign = new List<Point>();
            while (a <= b)
            {
                points_orign.Add(new Point(a, ans(a)));
                points.Add(new PointWith2Ys { X = a, YOR = ans(a), YRU = 0 });
                a += h;
            }
            
            points_runge.ForEach(x => { points.ForEach(y => { if (y.X == x.X) { y.YRU = x.Y; } }); });
            MethodEilera(c, b, h, f, ans(c)).ForEach(x => { points.ForEach(s => { if (s.X == x.X) { s.YEI = x.Y; } }); });

        }
        public List<Point> MethodEilera(double a, double b, double h, MyDelegate f, double y_0)
        {
            double prev_x, prev_y, cur_y;
            List<Point> results = new List<Point>();
            results.Add(new Point(a, y_0));
            a += h;
            while (a <= b)
            {
                prev_x = results.ToArray()[results.Count - 1].X;
                prev_y = results.ToArray()[results.Count - 1].Y;
                cur_y = prev_y + (a - prev_x) * f(prev_x, prev_y);
                results.Add(new Point (a, cur_y));
                a += h;
            }
            return results;
        }


        public List<PointWith2Ys> Points { get { return points; } }
        //public double[] X { get { return points_orign.Select(X => X.X).ToArray(); } }
        //public double[] YOR { get { return points_orign.Select(X => X.Y).ToArray(); } }
        //public double[] YRU { get { return points_runge.Select(X => X.Y).ToArray(); } }
        //public double[] YEI { get { return points.Select(X => X.Y).ToArray(); } }


    }

}
