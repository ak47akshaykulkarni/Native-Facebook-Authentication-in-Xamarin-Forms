using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace GigUpdates.Behaviors
{
    public class AnimateSizeBehavior : Behavior<View>
    {
        public static readonly BindableProperty EasingFunctionProperty = BindableProperty.Create<AnimateSizeBehavior, string>(
            p => p.EasingFunctionName,
            "SinIn",
            propertyChanged: OnEasingFunctionChanged);

        public static readonly BindableProperty ScaleProperty = BindableProperty.Create<AnimateSizeBehavior, double>(
            p => p.Scale,
            1.25);

        private Easing _easingFunction;

        public string EasingFunctionName
        {
            get { return (string)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        protected override void OnAttachedTo (View bindable)
        {
            base.OnAttachedTo (bindable);
            if(bindable.GetType() == typeof(Entry))
                bindable.Focused += OnItemFocused;
            if (bindable.GetType() == typeof(Button))
                ((Button) bindable).Clicked += OnItemFocused;
            // Perform setup
        }
        
        protected override void OnDetachingFrom (View bindable)
        {
            base.OnDetachingFrom (bindable);
            if (bindable.GetType() == typeof(Entry))
                bindable.Focused -= OnItemFocused;
            if (bindable.GetType() == typeof(Button))
                ((Button)bindable).Clicked -= OnItemFocused;
            // Perform clean up
        }

       
        private static Easing GetEasing(string easingName)
        {
            switch (easingName)
            {
                case "BounceIn": return Easing.BounceIn;
                case "BounceOut": return Easing.BounceOut;
                case "CubicInOut": return Easing.CubicInOut;
                case "CubicOut": return Easing.CubicOut;
                case "Linear": return Easing.Linear;
                case "SinIn": return Easing.SinIn;
                case "SinInOut": return Easing.SinInOut;
                case "SinOut": return Easing.SinOut;
                case "SpringIn": return Easing.SpringIn;
                case "SpringOut": return Easing.SpringOut;
                default: throw new ArgumentException(easingName + " is not valid");
            }
        }

        private static void OnEasingFunctionChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            (bindable as AnimateSizeBehavior).EasingFunctionName = newvalue;
            (bindable as AnimateSizeBehavior)._easingFunction = GetEasing(newvalue);
        }

        private async void OnItemFocused(object sender, EventArgs e)
        {
            await ((View)sender).ScaleTo(Scale, 250, _easingFunction);
            await ((View)sender).ScaleTo(1.00, 250, _easingFunction);
        }

        private async void OnClicked(object sender, EventArgs e)
        {
            await ((View)sender).ScaleTo(Scale, 250, _easingFunction);
            await ((View)sender).ScaleTo(1.00, 250, _easingFunction);
        }

        // Behavior implementation
    }
}
