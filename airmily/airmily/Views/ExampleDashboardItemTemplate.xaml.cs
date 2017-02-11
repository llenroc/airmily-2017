using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.ModelsExample;
using Prism.Events;
using Prism.Unity;
using Xamarin.Forms;

namespace airmily.Views
{
    public partial class ExampleDashboardItemTemplate : ContentView
    {
        public uint animationDuration = 250;
        public bool _processingTag = false;

        public static BindableProperty ShowBackgroundImageProperty =
            BindableProperty.Create("ShowBackgroundImage", typeof(bool),
                typeof(ExampleDashboardItemTemplate),
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
                typeof(ExampleDashboardItemTemplate),
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
                typeof(ExampleDashboardItemTemplate),
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
                typeof(ExampleDashboardItemTemplate),
                defaultValue: Color.White,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public ExampleDashboardItemTemplate()
        {
            InitializeComponent();
        }

        public async void OnWidgetTapped(object sender, EventArgs e)
        {
            if (_processingTag)
            {
                return;
            }

            _processingTag = true;

            try
            {
                await AnimateItem(this, animationDuration);

                // await SamplesListFromCategoryPage.NavigateToCategory((SampleCategory)BindingContext, Navigation);
            }
            finally
            {
                _processingTag = false;
            }
        }

        private async Task AnimateItem(View uiElement, uint duration)
        {
            var originalOpacity = uiElement.Opacity;

            await uiElement.FadeTo(.5, duration / 2, Easing.CubicIn);
            await uiElement.FadeTo(originalOpacity, duration / 2, Easing.CubicIn);
        }
    }
}
