using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using MathModelling.Model;
using System.Diagnostics;
using System.Drawing;

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
                lst_t.Add(0.2 * i);
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
                lst_x.Add(i);
            }
            c_del c_func = new c_del((x, t) => 2 * x * t);
            c_del f_func = new c_del((x, t) => (Math.Cos(t) * x * Math.Sin(t) + c_func(x, t) * Math.Exp(Math.Sin(t))));
            y_0_del yX0 = new y_0_del((x) => 1);
            y_0_del y0T = new y_0_del((t) => 0);

            diffur = new DifferentialEquation(lst_x, lst_t, c_func, f_func, yX0, y0T);

            chart.ChartAreas.Add(new ChartArea(ChartAreaName));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series(ChartSerieName));
            chart.Series[ChartSerieName].ChartArea = ChartAreaName;
            chart.Series[ChartSerieName].ChartType = SeriesChartType.Point;
            chart.Series[ChartSerieName].Color = Color.Blue;


            chart.Series.Add(new Series("Original"));
            chart.Series["Original"].ChartArea = ChartAreaName;
            chart.Series["Original"].ChartType = SeriesChartType.Line;
            chart.Series["Original"].Color = Color.OrangeRed;


            // добавим данные линии
            chart.Series[ChartSerieName].Points.DataBindXY(lst_x, diffur.GetListY(0));
            chart.Series["Original"].Points.DataBindXY(lst_x, diffur.GetOriginValues(0));

        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.Value = Math.Round(slider.Value, 1);
            chart.Series[ChartSerieName].Points.Clear();
            try
            {
                chart.Series[ChartSerieName].Points.DataBindXY(lst_x, diffur.GetListY(slider.Value));
                chart.Series["Original"].Points.DataBindXY(lst_x, diffur.GetListY(slider.Value));
            }
            catch(Exception) { Debug.WriteLine(slider.Value); }

            txtBlock.Text = slider.Value.ToString();
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
