// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Xunit;

namespace MahApps.Metro.Tests.TestHelpers
{
    public static class WindowHelpers
    {
        public static Task<T> CreateInvisibleWindowAsync<T>(Action<T> changeAddiotionalProperties = null)
            where T : Window, new()
        {
            var window = new T
                         {
                             Visibility = Visibility.Hidden,
                             ShowInTaskbar = false
                         };

            changeAddiotionalProperties?.Invoke(window);

            var completionSource = new TaskCompletionSource<T>();

            EventHandler handler = null;

            handler = (sender, args) =>
                {
                    window.Activated -= handler;
                    completionSource.SetResult(window);
                };

            window.Activated += handler;

            window.Show();

            return completionSource.Task;
        }

        public static void AssertWindowCommandsColor(this MetroWindow window, Color color)
        {
            foreach (var element in window.RightWindowCommands.Items.OfType<Button>())
            {
                Assert.Equal(color, ((SolidColorBrush)element.Foreground).Color);
            }

            Assert.Equal(color, ((SolidColorBrush)window.WindowButtonCommands.Foreground).Color);
        }
    }
}