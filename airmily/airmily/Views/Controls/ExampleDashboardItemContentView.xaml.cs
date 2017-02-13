﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using airmily.Ext;
using Xamarin.Forms;

namespace airmily.Views
{
    public partial class ExampleDashboardItemContentView : ContentView, INavigationServiceExt, IEventAggregatorExt
    {
        public uint _animationDuration = 250;
        public bool _processingTag = false;

        public static BindableProperty ShowBackgroundImageProperty =
            BindableProperty.Create("ShowBackgroundImage", typeof(bool),
                typeof(ExampleDashboardItemContentView),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool ShowBackgroundImage
        {
            get { return (bool)GetValue(ShowBackgroundImageProperty); }
            set { SetValue(ShowBackgroundImageProperty, value); }
        }

        public static BindableProperty ShowBackgroundColorProperty =
            BindableProperty.Create("ShowBackgroundColor", typeof(bool),
                typeof(ExampleDashboardItemContentView),
                false,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool ShowBackgroundColor
        {
            get { return (bool)GetValue(ShowBackgroundColorProperty); }
            set { SetValue(ShowBackgroundColorProperty, value); }
        }

        public static BindableProperty ShowiconColoredCircleBackgroundProperty =
            BindableProperty.Create("ShowiconColoredCircleBackground", typeof(bool),
                typeof(ExampleDashboardItemContentView),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool ShowiconColoredCircleBackground
        {
            get { return (bool)GetValue(ShowiconColoredCircleBackgroundProperty); }
            set { SetValue(ShowiconColoredCircleBackgroundProperty, value); }
        }

        public static BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color),
                typeof(ExampleDashboardItemContentView),
                defaultValue: Color.White,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private async Task AnimateItem(View uiElement, uint duration)
        {
            var originalOpacity = uiElement.Opacity;

            await uiElement.FadeTo(.5, duration / 2, Easing.CubicIn);
            await uiElement.FadeTo(originalOpacity, duration / 2, Easing.CubicIn);
        }

        private async void OnItemTapped(object sender, EventArgs e)
        {
            if (_processingTag)
            {
                return;
            }

            _processingTag = true;

            try
            {
                await AnimateItem(this, _animationDuration);

                OnNavigating(e);
            }
            finally
            {
                _processingTag = false;
            }
        }

        public event EventHandler Navigating;

        private void OnNavigating(EventArgs e)
        {
            if (Navigating != null)
                Navigating(this, e);
        }

        public ExampleDashboardItemContentView()
        {
            InitializeComponent();
        }
    }
}