using LiveCharts.Wpf;
using LiveCharts;
using PVZ;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public class ChartViewModel
{
    public ObservableCollection<string> MonthLabels { get; set; }

    public ChartViewModel()
    {
        // Инициализация коллекции меток месяцев
        MonthLabels = new ObservableCollection<string>
        {
            "Январь (1)", "Февраль (2)", "Март (3)", "Апрель (4)",
            "Май (5)", "Июнь (6)", "Июль (7)", "Август (8)",
            "Сентябрь (9)", "Октябрь (10)", "Ноябрь (11)", "Декабрь (12)"
        };
    }
}
public class ChartViewModel2 : INotifyPropertyChanged
{
    private List<string> _monthLabels;
    public List<string> MonthLabels
    {
        get { return _monthLabels; }
        set { _monthLabels = value; NotifyPropertyChanged(nameof(MonthLabels)); }
    }

    private SeriesCollection _orderSeries;
    public SeriesCollection OrderSeries
    {
        get { return _orderSeries; }
        set { _orderSeries = value; NotifyPropertyChanged(nameof(OrderSeries)); }
    }

    public ChartViewModel2()
    {
        // Получение данных о количестве заказов для каждого месяца
        var orderDataAccess = new OrderDataAccess();
        var orderData = orderDataAccess.GetReceivedOrdersByMonth();

        // Заполнение меток месяцев
        MonthLabels = orderData.Select(o => o.Month).ToList();

        // Создание серии данных для графика
        var series = new ColumnSeries
        {
            Title = "Orders Received",
            Values = new ChartValues<int>(orderData.Select(o => o.OrdersReceived))
        };

        OrderSeries = new SeriesCollection { series };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}