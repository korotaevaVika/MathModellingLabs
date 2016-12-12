using System;

namespace MathModelling.Model
{
    public delegate double basis_func(double x, int k);//может быть x^k или sin(kx) в будущем
    public delegate double f(double x);//функция, которую апроксимируем

    public class ApproximationModel
    {
        private int _k; // кол-во базисных функций
        private basis_func _basis;
        private f _func;

        private double[,] _matrix; //матрица скалярных произведений 
        private double[] _x; // массив аргументов
        double[] parametres;
        //параметры, которые находятся из слау методом гаусса, то бишь коэффициенты линейной комбинации б.ф.

        public ApproximationModel(int k, double[] x, basis_func basis, f func)
        {
            _k = k;
            _x = x;
            _basis = basis;
            _func = func;
            parametres = new double[_k + 1];
            countMatrix();
            findParameters();//решение слау с помощью метода Гаусса
            //getValues(_x);//получить значения функции, которой мы приблизили func
        }

        private void countMatrix()
        {
            _matrix = new double[_k + 1, _k + 2];
            
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = i; j < _matrix.GetLength(0); j++)
                {

                    double res = 0;
                    //for (int m = 0; m < _x.Length - 1; m++)
                    //{
                    //    double a, b, c;
                    //    a = _x[m];
                    //    b = _x[m+1];
                    //    c = (a + b) / 2;
                    //    res += ((b - a) / 6) * (
                    //        _basis(a, i) * _basis(a, j)
                    //        + 4 * _basis(c, i) * _basis(c, j) +
                    //        _basis(b, i) * _basis(b, j));
                    //}

                    foreach (var x_i in _x)
                    {
                        res += _basis(x_i, i) * _basis(x_i, j);
                    }
                    _matrix[i, j] = res;
                    _matrix[j, i] = res;
                }
                double d = 0;
                //for (int m = 0; m < _x.Length - 1; m++)
                //{
                //    double a, b, c;
                //    a = _x[m];
                //    b = _x[m + 1];
                //    c = (a + b) / 2;
                //    d = ((b - a) / 6) * (
                //            _basis(a, i) * _func(a)
                //            + 4 * _basis(c, i) * _func(c) +
                //            _basis(b, i) * _func(b));
                //}
                foreach (var x_i in _x)
                {
                    d += _basis(x_i, i) * _func(x_i);
                }
                _matrix[i, _k+1] = d;
            }
        }

        private void findParameters()
        {
            #region do_not_need_anymore
            //GausMethod Solution = new GausMethod((uint)_matrix.GetLength(0), (uint) _matrix.GetLength(0));
            ////double[] ReturnVal = new double[_matrix.GetLength(0)];
            ////заполняем правую часть
            //for (int i = 0; i < _matrix.GetLength(0); i++)
            //{
            //    Solution.RightPart[i] = _matrix[_matrix.GetLength(0)-1, i];
            //}
            ////Solution.RightPart[0] = 0;
            ////Solution.RightPart[1] = 126913;
            ////Solution.RightPart[2] = 211674;
            //////Solution.RightPart[3] = 0;

            ////заполняем матрицу
            //for (int i = 0; i < _matrix.GetLength(0); i++)
            //    for (int j = 0; j < _matrix.GetLength(0); j++)
            //        Solution.Matrix[i][j] = _matrix[i, j];

            ////решаем матрицу

            //Solution.SolveMatrix();
            //for (int i = 0; i < parametres.Length; i++)
            //{
            //    parametres[i] = Solution.Answer[i];
            //}
            //for (int i = 0; i < parametres.Length; i++)
            //{
            //    Debug.WriteLine("param " + i + "  " + parametres[i]);
            //}
            #endregion do_not_need_anymore
            #region Gauss
            System.Diagnostics.Debug.WriteLine("До метода Гаусса");

            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int h = 0; h < _matrix.GetLength(1); h++)
                {
                    System.Diagnostics.Debug.Write(_matrix[i, h] + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            for (int k = 0; k < _matrix.GetLength(0) - 1; k++)
            {
                for (int i = k + 1; i < _matrix.GetLength(0); i++)
                {
                    for (int j = k + 1; j <= _matrix.GetLength(0); j++)
                    {
                        _matrix[i, j] = _matrix[i, j] - (_matrix[i, k] / _matrix[k, k]) * _matrix[k, j];
                    }
                    _matrix[i, k] = 0;
                }
            }
            System.Diagnostics.Debug.WriteLine("После метода Гаусса");

            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int h = 0; h < _matrix.GetLength(1); h++)
                {
                    System.Diagnostics.Debug.Write(_matrix[i, h] + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            System.Diagnostics.Debug.WriteLine("Параметры");
            parametres = new double[_k + 1];
            double sum;
            for (int k = _matrix.GetLength(0) - 1; k >= 0; k--)
            {
                sum = 0;
                for (int j = k + 1; j < _matrix.GetLength(0); j++)
                {
                    sum += _matrix[k, j] * parametres[j];
                }
                parametres[k] = (_matrix[k, _matrix.GetLength(1) - 1] - sum) / _matrix[k, k];
            }
            for (int i = 0; i < parametres.Length; i++)
            {
                System.Diagnostics.Debug.Write(parametres[i] +"\t");
                System.Diagnostics.Debug.WriteLine("");
            }
            #endregion Gauss
        }

        public double[] getValues(double[] args)
        {
            double[] res = new double[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < _k+1; j++)
                {
                    res[i] += _basis(args[i], j) * parametres[j];
                }
            }
            return res;
        }
        public double[] getValues()
        {
            double[] res = new double[_x.Length];
            for (int i = 0; i < _x.Length; i++)
            {
                for (int j = 0; j < _k+1; j++)
                {
                    res[i] += Math.Pow(_x[i], j) * parametres[j];
                }
            }
            return res;
        }
        public double[] getOriginalValues()
        {
            double[] res = new double[_x.Length];
            for (int i = 0; i < _x.Length; i++)
            {
                res[i] = _func(_x[i]);
            }
            return res;
        }
    }
}
