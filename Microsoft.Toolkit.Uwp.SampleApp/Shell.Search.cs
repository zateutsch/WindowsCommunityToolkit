// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        internal void StartSearch(string startingText = null)
        {
            if (FocusManager.GetFocusedElement() == SearchBox.FindDescendant<TextBox>())
            {
                return;
            }

            SearchBox.Focus(FocusState.Keyboard);

            var currentSearchText = SearchBox.Text;

            SearchBox.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(startingText))
            {
                SearchBox.Text = startingText;
            }
            else
            {
                SearchBox.Text = currentSearchText;
            }
        }

        private async void UpdateSearchSuggestions()
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                HideSamplePicker();
                return;
            }

            var samples = (await Samples.FindSample(SearchBox.Text)).OrderBy(s => s.Name).ToArray();
            if (samples.Count() > 0)
            {
                ShowSamplePicker(samples);
            }
            else
            {
                HideSamplePicker();
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            UpdateSearchSuggestions();
        }

        private void SearchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down && SamplePickerGrid.Visibility == Visibility.Visible)
            {
                // If we try and navigate down out of the textbox (and there's search results), go to the search results.
                DispatcherQueue.ExecuteOnUIThreadAsync(() => SamplePickerGridView.Focus(FocusState.Keyboard));
            }
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateSearchSuggestions();
        }
    }
}
