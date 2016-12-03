using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathModelling.Model
{

    public delegate double c_del(double x, double t);
    public delegate double y_0_del(double v);//для начальных условий

    public class DifferentialEquation// : INotifyPropertyChanged
    {
        Dictionary<double, List<Point>> collection;//t, (x, y)
        c_del c;
        c_del f;

        //public event PropertyChangedEventHandler PropertyChanged;

        double x_step(int n)
        {
            return collection[collection.Keys.ElementAt(0)].ElementAt(n + 1).X - collection[collection.Keys.ElementAt(0)].ElementAt(n).X;
        }

        double t_step(int m)
        {
            return collection.Keys.ElementAt(m + 1) - collection.Keys.ElementAt(m);
        }

        double GetY(int n_x, int m_t)
        {
            return collection[GetT(m_t)].ElementAt(n_x).Y;
        }

        double GetT(int m)
        {
            return collection.Keys.ElementAt(m);
        }

        double GetX(int n_x, int m_t)
        {
            return collection[GetT(m_t)].ElementAt(n_x).X;
        }

        void SetY(int n_x, int m_t, double value)
        {
            collection[GetT(m_t)].ElementAt(n_x).Y = value;
        }

        double GetC(int n, int m)
        {
            return c(collection[GetT(m)].ElementAt(n).X, GetT(m));
        }

        bool Criteria(int n, int m)
        {
            return (GetC(n, m + 1) * t_step(m) <= x_step(n - 1));
        }

        public DifferentialEquation(
            List<double> x,
            List<double> t,
            c_del func_c,
            c_del func_f,
            y_0_del y_x0,
            y_0_del y_0t
            )
        {
            this.c = func_c;
            this.f = func_f;
            collection = new Dictionary<double, List<Point>>(t.Count);
            for (int i = 0; i < t.Count; i++)
            {
                List<Point> points = new List<Point>();
                collection.Add(t.ElementAt(i), points);
                for (int j = 0; j < x.Count; j++)
                {
                    collection[t.ElementAt(i)].Add(new Point(x.ToArray()[j], 0));
                }
            }

            for (int i = 0; i < x.Count; i++)
            {
                SetY(i, 0, y_x0(collection[0].ElementAt(i).X));
            }
            for (int i = 0; i < t.Count; i++)
            {
                SetY(0, i, y_0t(GetX(0, i)));
            }

            for (int i = 1; i < t.Count - 1; i++)
            {
                for (int j = 1; j < x.Count; j++)
                {
                    if (Criteria(j, i))
                    {
                        collection[t.ElementAt(i + 1)].ElementAt(j).Y =
                            t_step(i) *
                            (
                            f(GetX(j, i), GetT(i))
                            - GetC(j, i + 1) * (GetY(j, i) - GetY(j - 1, i)) / x_step(j - 1)
                            )
                            + GetY(j, i)
                            ;
                    }
                    else
                    {
                        collection[t.ElementAt(i + 1)].ElementAt(j).Y =
                            (
                            f(GetX(j, i), GetT(i))
                            + ((GetY(j, i) - GetY(j - 1, i + 1)) / t_step(j))
                            * x_step(j - 1) / GetC(j, i + 1)
                            )
                            + GetY(j - 1, i + 1);
                    }
                }
            }

        }

        public List<double> GetListY(double t_value)
        {
            List<double> lst = new List<double>();
            foreach (var item in collection[t_value])
            {
                //System.Diagnostics.Debug.WriteLine("t = {0}, round = {1}", t_value, Math.Round(t_value, 1));
                lst.Add(item.Y);
            }
            return lst;
        }

        c_del y = new c_del((x, t) => Math.Exp(Math.Sin(t) * x));
        public List<double> GetOriginValues(double tValue)
        {
            List<double> ys = new List<double>();
            foreach (var item in collection[tValue])
            {
                ys.Add(y(item.X, tValue));
            }
            return ys;
        }

        

    }
}

