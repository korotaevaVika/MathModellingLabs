using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModelling.Model
{
    /// <summary>
    /// Свой класс для Point'a. Для реализации возможности изменения X, Y в отличие от System.Windows.Point
    /// </summary>
    public class Point
    {
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        private double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }


        /// <summary>
        /// Преобразует экземпляр класса Point из MathModelling.Model в экзмепляр System.Windows.Point
        /// </summary>
        /// <returns></returns>
        public System.Windows.Point ConvertToWinPoint()
        {
            return new System.Windows.Point(X, Y);
        }
    }

}
