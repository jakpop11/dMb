using System;

using Xamarin.Forms;


namespace dMb.Controls
{
    public partial class CheckBox : BaseCheckBox
    {
        #region Properties
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        #endregion



        #region BindableProperties
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked), typeof(bool),
            typeof(CheckBox),
            false,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CheckBox)bindable;
                control.UpdateVisuals();
            }
            );


        #endregion



        #region Events
        public event EventHandler CheckedChaned;

        #endregion



        #region Constructor
        public CheckBox()
        {
            // Grid setup
            Padding = new Thickness(10);
            WidthRequest = _minSize;
            HeightRequest = _minSize;
            MinimumWidthRequest = _minSize;
            MinimumHeightRequest = _minSize;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            Children.Clear();
            Children.Add(outlineBox);
            Children.Add(iconPath);


            // Update
            UpdateVisuals();


            // Functionality
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    BoxBounceOnPressAsync();
                    ChangeCheck();

                    // ClickEvent?.Invoke ...

                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParemeter))
                            Command.Execute(CommandParemeter);
                    }

                    CheckedChaned?.Invoke(this, new EventArgs());

                })
            });


        }
        #endregion



        #region Methods
        void UpdateVisuals()
        {
            if (!IsChecked)
            {
                outlineBox.Fill = new SolidColorBrush(Color.Transparent);
                iconPath.Fill = new SolidColorBrush(Color.Transparent);
            }
            else
            {
                iconPath.Data = IconGeometry;
                outlineBox.Fill = new SolidColorBrush(BoxColor);
                iconPath.Fill = new SolidColorBrush(IconColor);
            }
            IconBounceOnPressAsync();
        }

        void ChangeCheck()
        {
            if (IsChecked)
            {
                IsChecked = false;
            }
            else
            {
                IsChecked = true;
            }
            UpdateVisuals();

        }

        #endregion



    }
}
