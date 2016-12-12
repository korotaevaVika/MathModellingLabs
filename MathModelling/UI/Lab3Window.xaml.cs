using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MathModelling.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace MathModelling.UI
{
    /// <summary>
    /// Логика взаимодействия для Lab3Window.xaml
    /// </summary>
    public partial class Lab3Window : MetroWindow
    {
        public string[] BasisFunctions;
        string ChartAreaName = "Default";
        string ChartSerieName = "Graphic";
        ApproximationModel model;
        List<double> lst_x;
        List<int> k_basis;
        f func;
        basis_func basis;
        public Lab3Window()
        {
            BasisFunctions = new string[] { "Pow(x, k)", "Sin(k*x)"  };
            func = (x) => (Math.Exp(Math.Sin(x)));
            basis = (x, k) => (Math.Pow(x, k));
            //basis = (x, k) => (Math.Sin(x*k));
            lst_x = new List<double>();
            for (int i = 0; i < 60; i++)
            {
                lst_x.Add(0.1 * i);
            }
            k_basis = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                k_basis.Add(i);
            }
            InitializeComponent();
            //combobox.itemssource = basisfunctions;

            slider.Minimum = 0;
            slider.Maximum = k_basis.ToArray()[k_basis.Count - 1];
            slider.Value = 1;
            model =
                new ApproximationModel(
                    int.Parse(slider.Value.ToString()),
                    lst_x.ToArray(),
                    basis,
                    func);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            chart.ChartAreas.Add(new ChartArea(ChartAreaName));

            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series(ChartSerieName));
            chart.Series[ChartSerieName].ChartArea = ChartAreaName;
            chart.Series[ChartSerieName].ChartType = SeriesChartType.Point;
            chart.Series[ChartSerieName].Color = System.Drawing.Color.Blue;

            chart.Series.Add(new Series("Original"));
            chart.Series["Original"].ChartArea = ChartAreaName;
            chart.Series["Original"].ChartType = SeriesChartType.Point;
            chart.Series["Original"].Color = System.Drawing.Color.OrangeRed;

            // добавим данные линии
            chart.Series[ChartSerieName].Points.DataBindXY(lst_x, model.getValues());
            chart.Series["Original"].Points.DataBindXY(lst_x, model.getOriginalValues());

        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                slider.Value = Math.Round(slider.Value);

                chart.Series[ChartSerieName].Points.Clear();

                model = new ApproximationModel(int.Parse(slider.Value.ToString()), lst_x.ToArray(), basis, func);
                
                try
                {
                    chart.Series[ChartSerieName].Points.DataBindXY(lst_x, model.getValues());
                    chart.Series["Original"].Points.DataBindXY(lst_x, model.getOriginalValues());
                }
                catch (Exception) { Debug.WriteLine(slider.Value); }

                txtBlock.Text = slider.Value.ToString();
            }
            catch(Exception ex) {
                Debug.WriteLine( "SLIDER CHANGED EXCEPTION\n\n" + ex.ToString());
            }
        }

        private async void btnChangeTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int kolvo = int.Parse(txtTime.Text);
                chart.Series[ChartSerieName].Points.Clear();
                model = new ApproximationModel(kolvo, lst_x.ToArray(), basis, func);
                chart.Series[ChartSerieName].Points.DataBindXY(lst_x, model.getValues());
                chart.Series["Original"].Points.Clear();

                chart.Series["Original"].Points.DataBindXY(lst_x, model.getOriginalValues());

                slider.Value = kolvo;
            }
            catch
            {
                form.Visibility = Visibility.Hidden;
                await this.ShowMessageAsync("Неверный формат", "Количество может быть только натуральным числом");
                form.Visibility = Visibility.Visible;

            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedValue.ToString() == BasisFunctions[1])
            {
                basis = (x, k) => (Math.Sin(x * k));
            }
            else basis = (x, k) => (Math.Pow(x, k));
            
            
            chart.Series[ChartSerieName].Points.Clear();

            model = new ApproximationModel(int.Parse(slider.Value.ToString()), lst_x.ToArray(), basis, func);

            try
            {
                chart.Series[ChartSerieName].Points.DataBindXY(lst_x, model.getValues());
                chart.Series["Original"].Points.DataBindXY(lst_x, model.getOriginalValues());
            }
            catch (Exception) {}
            
        }
    }
}
