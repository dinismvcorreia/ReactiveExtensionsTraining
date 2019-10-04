namespace ReactiveExtensionsTraining
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEnumerable<(int Number, int ThreadId)> _numbers;

        public MainWindow()
        {
            InitializeComponent();

            _numbers = GetNumbersFromSource();
        }

        private static IEnumerable<(int, int)> GetNumbersFromSource()
        {
            return Enumerable
                    .Range(1, 10)
                    .Select(n => (n, Thread.CurrentThread.ManagedThreadId));
        }

        private void PullButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsListBox.Items.Clear();
            ItemsListBox.Items.Add("*** Pulling Data ***");

            foreach (var item in _numbers)
            {
                ItemsListBox.Items.Add(item);
            }

            ItemsListBox.Items.Add("Completed!");
        }

        private void ObserveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsListBox.Items.Clear();
            ItemsListBox.Items.Add($"*** Observing Data on Thread {Thread.CurrentThread.ManagedThreadId} ***");

            var numbersObs = _numbers
                .ToObservable()
                .Select(tuple => $"{tuple.Number} on Thread {tuple.ThreadId}");

            var intervalObs = Observable.Interval(TimeSpan.FromMilliseconds(200));

            intervalObs.Zip(numbersObs, (l, r) => r)
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOnDispatcher()
                .Subscribe(
                    text => ItemsListBox.Items.Add(text),
                    () => ItemsListBox.Items.Add("Completed!"));
        }
    }
}
