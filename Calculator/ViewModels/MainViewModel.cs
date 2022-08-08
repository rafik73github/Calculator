using Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _screenVal;
        public ICommand AddNumberCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand GetResultCommand { get; set; }
        private List<string> _availableOperations = new List<string> { "+", "-", "/", "*" };
        private DataTable _dataTable = new DataTable();
        private bool _isLastSignAnOperation;

        public MainViewModel()
        {
            ScreenVal = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ScreenVal
        {
            get { return _screenVal; }
            set
            {
                _screenVal = value;
                OnPropertyChanged();
            }
        }

        private void AddNumber(object obj)
        {
            var number = obj as string;

            if (ScreenVal == "0" && number != ",")
                ScreenVal = string.Empty;
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
                number = "0,";

            ScreenVal += number;

            _isLastSignAnOperation = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            ScreenVal += operation;

            _isLastSignAnOperation = true;
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";

            _isLastSignAnOperation = false;
        }

        private void GetResult(object obj)
        {
            var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",", "."), "")), 2);

            ScreenVal = result.ToString();
        }

        private bool CanGetResult(object obj)
        {
            return !_isLastSignAnOperation;
        }

        private bool CanAddOperation(object obj)
        {
            return !_isLastSignAnOperation;
        }
    }
}
