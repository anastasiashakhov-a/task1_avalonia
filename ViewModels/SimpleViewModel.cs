using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tmds.DBus.Protocol;
using WindowsFormsApp1;
using WorkListAvalonia.Models;

namespace BasicMvvmSample.ViewModels
{
    public class SimpleViewModel : INotifyPropertyChanged
    {
        private workList<int> myList = new workList<int>();
        private LogManager logManager = new LogManager(); 

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ---- Add some Properties ----

        private string? _ValueData; // This is our backing field for Name

        public string? ValueData
        {
            get
            {
                return _ValueData;
            }
            set
            {
                // We only want to update the UI if the Name actually changed, so we check if the value is actually new
                if (_ValueData != value)
                {
                    // 1. update our backing field
                    _ValueData = value;

                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Greeting));
                }
            }
        }

        // Greeting will change based on a Name.
        public string Greeting
        {
            get
            {
                return "Show List: " + myList.Print();
            }
        }

        // Команды для кнопок
        public ICommand AddButtonClickCommand { get; }
        public ICommand DelButtonClickCommand { get; }
        public ICommand SaveLogsCommand { get; } // Новая команда для сохранения логов

        public SimpleViewModel()
        {
            // Инициализация команд
            AddButtonClickCommand = new RelayCommand(AddOnButtonClick);
            DelButtonClickCommand = new RelayCommand(DelOnButtonClick);
            SaveLogsCommand = new RelayCommand(SaveLogs);
        }

        // Обработчик для кнопки "Add"
        private void AddOnButtonClick()
        {
            if (int.TryParse(_ValueData, out int x))
            {
                myList.Add(x);
                logManager.AddLog(LogType.Information, $"Added value: {x}"); // Логирование
                RaisePropertyChanged(nameof(Greeting));
            }
            else
            {
                logManager.AddLog(LogType.Error, $"Failed to add value: {_ValueData}"); // Логирование ошибки
            }
        }

        // Обработчик для кнопки "Delete"
        private void DelOnButtonClick()
        {
            if (int.TryParse(_ValueData, out int x))
            {
                if (myList.Remove(x))
                {
                    logManager.AddLog(LogType.Information, $"Removed value: {x}"); // Логирование
                }
                else
                {
                    logManager.AddLog(LogType.Warning, $"Value not found: {x}"); // Логирование предупреждения
                }
                RaisePropertyChanged(nameof(Greeting));
            }
            else
            {
                logManager.AddLog(LogType.Error, $"Failed to remove value: {_ValueData}"); // Логирование ошибки
            }
        }

        // Обработчик для кнопки "Save Logs"
        private void SaveLogs()
        {
            string filePath = "logs.txt";
            logManager.SaveLogsToFile(filePath); // Сохранение логов в файл
            logManager.AddLog(LogType.Information, $"Logs saved to {filePath}"); // Логирование
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Класс RelayCommand для реализации ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;
    }
}