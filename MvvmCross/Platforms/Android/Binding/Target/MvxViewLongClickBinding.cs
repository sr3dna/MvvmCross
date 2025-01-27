﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxViewLongClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _subscription;

        protected View View => (View)Target;

        public MvxViewLongClickBinding(View view)
            : base(view)
        {
            _subscription = view.WeakSubscribe<View, View.LongClickEventArgs>(nameof(view.LongClick), ViewOnLongClick);
        }

        private void ViewOnLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        protected override void SetValueImpl(object target, object value)
        {
            _command = value as ICommand;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;

                _command = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
