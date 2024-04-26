using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;


namespace PVZ {
public class ChartViewModel : INotifyPropertyChanged
{
    private SeriesCollection _orderSeries;
    public SeriesCollection OrderSeries
    {
        get { return _orderSeries; }
        set
        {
            _orderSeries = value;
            OnPropertyChanged(nameof(OrderSeries));
        }
    }

    private ObservableCollection<string> _monthLabels;
    public ObservableCollection<string> MonthLabels
    {
        get { return _monthLabels; }
        set
        {
            _monthLabels = value;
            OnPropertyChanged(nameof(MonthLabels));
        }
    }

    public ChartViewModel()
    {
        // Создаем коллекцию месяцев
        MonthLabels = new ObservableCollection<string>(
            new string[] { "January", "February", "March", "April", "May", "June",
                           "July", "August", "September", "October", "November", "December" });

        // Пример данных для графика (замените на ваши данные)
        OrderSeries = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Orders Received",
                Values = new ChartValues<int> { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65 }
            }
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
    }