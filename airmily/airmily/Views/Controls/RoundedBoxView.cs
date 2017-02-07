using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace airmily.Views.Controls
{
	public class RoundedBoxView : BoxView
	{
		public static readonly BindableProperty CornerRadiusProperty =  /*BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(RoundedBoxView), null);*/BindableProperty.Create<RoundedBoxView, float>(p => p.CornerRadius, 0);

		public float CornerRadius
		{
			get { return (float)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}


		public static readonly BindableProperty StrokeProperty = /*BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Color));*/				BindableProperty.Create<RoundedBoxView, Color>(p => p.Stroke, Color.Red);
		public Color Stroke
		{
			get { return (Color)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		public static readonly BindableProperty StrokeThicknessProperty = /*BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(double));*/BindableProperty.Create<RoundedBoxView, int>(p => p.StrokeThickness, 0);
		public int StrokeThickness
		{
			get { return (int)GetValue(StrokeThicknessProperty); }
			set { SetValue(StrokeThicknessProperty, value); }
		}
	}
}
