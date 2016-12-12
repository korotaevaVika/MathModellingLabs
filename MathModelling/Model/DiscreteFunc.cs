using System;
using System.Collections.Generic;

namespace MathModelling.Model
{
    public class DiscreteFunc
    {
        private List<Point> Values;

        public DiscreteFunc()
        {
            Values = new List<Point>();
        }

        public DiscreteFunc(DiscreteFunc discreteFunc)
        {
            this.Values = discreteFunc.Values;
        }
        public int Length { get { return Values.Count; } }

        public Point this[int ind]
        {
            get { return Values.ToArray()[ind]; }
            set { Values.Add(value); }
        }

        public List<Point> RungePoints
        {
            get{ return Values; }
        }
    }
}
