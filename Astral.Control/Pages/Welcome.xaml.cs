using Autofac;
using Autofac.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Astral.Control.Pages;

public partial class Welcome : UserControl, IPage
{
    public Welcome(
        WindowsAPI.OSCheck osCheck,
        Resources.ImageBackgrounds imageBackgrounds,
        IEnumerable<Models.IConfig> configurations)
    {
        InitializeComponent();

        foreach (var config in configurations)
            ConfigurationsList.Children.Add(
                new Configuration(osCheck, imageBackgrounds, config)
                {
                    Margin = new System.Windows.Thickness(0, 0, 25, 0)
                });
    }

    public event EventHandler<IPage> Replaced;
}