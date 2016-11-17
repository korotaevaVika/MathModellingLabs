using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using MathModelling.Model;

namespace MathModelling
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ChartAreaName = "Default";
        string ChartSerieName = "Graphic";
        DifferentialEquation diffur;
        List<double> lst_x;
        public List<double> lst_t;

        public MainWindow()
        {
            lst_t = new List<double>();
            for (int i = 0; i < 30; i++)
            {
                lst_t.Add(0.05 * i);
            }
            InitializeComponent();
            slider.Minimum = 0;

            slider.Maximum = lst_t.ToArray()[lst_t.Count - 1];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst_x = new List<double>();
            //lst_t = new List<double>();

            for (int i = 0; i < 10; i++)
            {
                lst_x.Add(0.2 * i);
            }
            //for (int i = 0; i < 30; i++)
            //{
            //    lst_t.Add(0.5 * i);
            //}
            c_del c_func = new c_del((x, t) => 2 * x * t);
            c_del f_func = new c_del((x, t) => (-2 * x * Math.Sin(t) + c_func(x, t) * 2 * Math.Cos(t)) * Math.Exp(2 * x * Math.Cos(t)) + Math.Pow(t, 3));
            y_0_del yX0 = new y_0_del((x) => Math.Exp(2 * x));
            y_0_del y0T = new y_0_del((t) => 1);

            diffur = new DifferentialEquation(lst_x, lst_t, c_func, f_func, yX0, y0T);

            chart.ChartAreas.Add(new ChartArea(ChartAreaName));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series(ChartSerieName));
            chart.Series[ChartSerieName].ChartArea = ChartAreaName;
            chart.Series[ChartSerieName].ChartType = SeriesChartType.Line;

            // добавим данные линии
            chart.Series[ChartSerieName].Points.DataBindXY(lst_x, diffur.GetListY(0));
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            chart.Series[ChartSerieName].Points.Clear();
            chart.Series[ChartSerieName].Points.DataBindXY(lst_x, diffur.GetListY(slider.Value));
        }

        private void btnChangeTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double time = double.Parse(txtTime.Text);
                chart.Series[ChartSerieName].Points.Clear();
                chart.Series[ChartSerieName].Points.DataBindXY(lst_x, diffur.GetListY(time));
                slider.Value = time;
            }
            catch
            {
                MessageBox.Show("неверное время");
            }
        }
    }
}
