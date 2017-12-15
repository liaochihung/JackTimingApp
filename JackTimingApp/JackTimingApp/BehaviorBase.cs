using System;
using Xamarin.Forms;

namespace JackTimingApp
{
    public class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }

    // function not working
    //public class EventToCommand
    //{
    //    public static readonly BindableProperty ToCommandProperty =
    //        BindableProperty.CreateAttached<EventToCommand, ICommand>(
    //            bindable => EventToCommand.GetToCommand(bindable),
    //            null,
    //            BindingMode.OneWay,
    //            null,
    //            (bindable, oldValue, newValue) => EventToCommand.OnCommandChanged(bindable, oldValue, newValue),
    //            null,
    //            null);

    //    public static readonly BindableProperty FromEventProperty =
    //        BindableProperty.CreateAttached<EventToCommand, string>(
    //            bindable => EventToCommand.GetFromEvent(bindable),
    //            null,
    //            BindingMode.OneWay);

    //    public static ICommand GetToCommand(BindableObject obj)
    //    {
    //        return (ICommand)obj.GetValue(EventToCommand.ToCommandProperty);
    //    }

    //    public static void SetToCommand(BindableObject obj, ICommand value)
    //    {
    //        obj.SetValue(EventToCommand.ToCommandProperty, value);
    //    }

    //    public static string GetFromEvent(BindableObject obj)
    //    {
    //        return (string)obj.GetValue(EventToCommand.FromEventProperty);
    //    }

    //    public static void SetFromEvent(BindableObject obj, string value)
    //    {
    //        obj.SetValue(EventToCommand.FromEventProperty, value);
    //    }

    //    private static void OnCommandChanged(BindableObject obj, ICommand oldValue, ICommand newValue)
    //    {
    //        var eventName = GetFromEvent(obj);

    //        if (string.IsNullOrEmpty(eventName))
    //        {
    //            throw new InvalidOperationException("FromEvent property is null or empty");
    //        }

    //        Observable.FromEventPattern(obj, eventName).Subscribe(p =>
    //        {
    //            var command = GetToCommand(obj);

    //            if (command != null &&
    //                command.CanExecute(p.EventArgs))
    //            {
    //                command.Execute(p.EventArgs);
    //            }
    //        });
    //    }
    //}
}