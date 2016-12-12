using MathModelling.UI.Commands;
using System;
using System.Windows.Input;

namespace MathModelling.UI.ViewModels
{
    public class MenuViewModel
    {
        #region Commands
        private ICommand _showFirstLab;
        private ICommand _showSecondLab;
        private ICommand _showThirdLab;
        #endregion Commands
        
        private string _userName;
        public MenuViewModel()
        {
            _showFirstLab = new RelayCommand(CanExecute, LaunchFirstLab);
            _showSecondLab = new RelayCommand(CanExecute, LaunchSecondLab);
            _showThirdLab = new RelayCommand(CanExecute, LaunchThirdLab);
            _userName = Environment.UserName;
        }

        private string _periodOfTime
        {
            get
            {
                if (DateTime.Now.Hour <= 6)
                {
                    return "Доброй ночи";
                }
                else if (DateTime.Now.Hour < 12)
                {
                    return "Доброе утро";
                }
                else if (DateTime.Now.Hour < 18)
                {
                    return "Доброго дня";
                }
                else return "Добрый вечер";
            }
        }

        public string UserHello
        {
            get { return _periodOfTime + ", " + _userName + "!"+"\nВыберите работу, которую хотите проверить"; }
        }

        public ICommand ShowFirstLab
        {
            get { return _showFirstLab; }
        }
        public ICommand ShowSecondLab
        {
            get { return _showSecondLab; }
        }
        public ICommand ShowThirdLab
        {
            get { return _showThirdLab; }
        }
        private bool CanExecute(object parameter)
        {
            return true;
        }
        private void LaunchFirstLab(object parameter)
        {
            Lab1Window l = new Lab1Window();
            l.Show();
        }
        private void LaunchSecondLab(object parameter)
        {
            MainWindow m = new MainWindow();
            m.Show();
        }
        private void LaunchThirdLab(object parameter)
        {
            Lab3Window n = new Lab3Window();
            n.Show();
        }
    }
}
