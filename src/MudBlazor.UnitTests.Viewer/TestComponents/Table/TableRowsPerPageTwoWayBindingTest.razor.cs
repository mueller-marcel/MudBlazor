﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace MudBlazor.UnitTests.TestComponents.Table
{
    public partial class TableRowsPerPageTwoWayBindingTest : ComponentBase
    {
        public static string __description__ = "Test Two-Way Binding of RowsPerPage Parameter.";

        private int _rowsPerPage = 3;
        private readonly ViewModel _viewModel = new();

        [Parameter]
        public int RowsPerPage
        {
            get => _rowsPerPage;
            set
            {
                if (_rowsPerPage == value)
                    return;
                _rowsPerPage = value;
                RowsPerPageChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<int> RowsPerPageChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _viewModel.PropertyChanged += (_, _) => InvokeAsync(StateHasChanged).ConfigureAwait(false);
            await _viewModel.LoadItemsAsync().ConfigureAwait(false);
            await base.OnInitializedAsync();
        }

        private sealed class Item
        {
            public string? Text { get; init; }
        }

        private sealed class ViewModel : INotifyPropertyChanged
        {
            private List<Item> _items = [];

            public List<Item> Items
            {
                get => _items;
                private set
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }

            public async Task LoadItemsAsync()
            {
                var list = new List<Item>();
                //delay to simulate web service call
                await Task.Delay(50).ConfigureAwait(false);
                for (var i = 0; i < 20; i++)
                {
                    list.Add(new Item { Text = i.ToString() });
                }
                Items = list;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
