using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerStudio.ViewModels
{
    public class CustomComboBoxViewModel<T> : ObservableObject
    {
        private ObservableCollection<T> _itemsSource = new();
        private T _selectedItem;

        public ObservableCollection<T> ItemsSource
        {
            get => _itemsSource;
            set { SetProperty(ref _itemsSource, value); }
        }

        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                ItemSelectedCommand?.Execute(value);
            }
        }

        public bool IsVisible => ItemsSource.Count > 0;
        public AsyncRelayCommand<T> ItemSelectedCommand { get; set; }
        public void AddItems(IEnumerable<T> items)
        {
            ItemsSource.Clear();

            foreach (var item in items)
            {
                ItemsSource.Add(item);
            }
            OnPropertyChanged(nameof(IsVisible));
        }

        public void Clear()
        {
            _itemsSource.Clear();
            OnPropertyChanged(nameof(IsVisible));
        }
    }
}
