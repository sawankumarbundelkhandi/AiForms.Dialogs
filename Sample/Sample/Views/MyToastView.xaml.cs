﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AiForms.Extras.Abstractions;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class MyToastView : ToastView
    {
        public MyToastView()
        {
            InitializeComponent();
            ToastWidth = 0.8;
            ToastHeight = 0.5;
            OffsetY = 0;
            OffsetX = 0;
            Duration = 5000;
            LayoutAlignment = LayoutAlignment.Start;
            BackgroundColor = Color.FromRgb(0, 150, 0);
            Opacity = 1;
            CornerRadius = 20;

            image.Source = ImageSource.FromResource("Sample.Resources.ios7-paw-outline.png");


        }

        public override void RunPresentationAnimation()
        {
            Task.WhenAll(
                image.RotateTo(360, 250),
                image.ScaleTo(1.0, 250),
                image.TranslateTo(0, 0, 250)
            );
        }

    }
}