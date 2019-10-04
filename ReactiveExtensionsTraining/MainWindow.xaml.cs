namespace ReactiveExtensionsTraining
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEnumerable<int> _numbers;

        public MainWindow()
        {
            InitializeComponent();

            _numbers = GetNumbersFromSource();
        }

        private static IEnumerable<int> GetNumbersFromSource()
        {
            return Enumerable.Range(1, 10);
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
            ItemsListBox.Items.Add("*** Observing Data ***");

            // Convert Enumerable to Stream (Remember the Marble Diagrams)
            var numbersObs = _numbers.ToObservable();

            // Each Marble is OnNext(), at the end there's a OnCompleted()
            numbersObs.Subscribe(
                n => ItemsListBox.Items.Add(n),
                () => ItemsListBox.Items.Add("Completed!"));
        }
    }
}
