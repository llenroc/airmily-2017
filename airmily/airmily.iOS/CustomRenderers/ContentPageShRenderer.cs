using System;
using System.Collections.Generic;
using System.Text;
using airmily.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using airmily.Views.Controls;
using HockeyApp;
using HockeyApp.iOS;

[assembly:ExportRendererAttribute(typeof(ContentPageShView),typeof(ContentPageShRenderer))]

namespace airmily.iOS.CustomRenderers
{
    class ContentPageShRenderer : PageRenderer
    {
        public override bool CanBecomeFirstResponder {
            get { return true; } 
        }

        public override void ViewDidAppear(bool animated)
        {
            BecomeFirstResponder();
            base.ViewDidAppear(animated);
        }

        public override void MotionEnded(UIKit.UIEventSubtype motion, UIKit.UIEvent evt)
        {
            if (motion == UIEventSubtype.MotionShake)
            {
                BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackComposeViewWithGeneratedScreenshot();
            }
            base.MotionEnded(motion, evt);
        }
    }
}
