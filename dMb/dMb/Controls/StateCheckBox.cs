using System;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;


namespace dMb.Controls
{
    public partial class StateCheckBox : BaseCheckBox
    {
        #region Properties
        public CheckBoxState State
        {
            get => (CheckBoxState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public Geometry SecondIconGeometry
        {
            get => (Geometry)GetValue(SecondIconGeometryProperty);
            set => SetValue(SecondIconGeometryProperty, value);
        }

        public Color SecondIconColor
        {
            get => (Color)GetValue(SecondIconColorProperty);
            set => SetValue(SecondIconColorProperty, value);
        }

        #endregion



        #region BindableProperties
        public static readonly BindableProperty StateProperty = BindableProperty.Create(
            nameof(State), typeof(CheckBoxState),
            typeof(StateCheckBox),
            CheckBoxState.Unchecked,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (StateCheckBox)bindable;
                control.UpdateVisuals();
            }
            );

        public static readonly BindableProperty SecondIconGeometryProperty = BindableProperty.Create(
            nameof(SecondIconGeometry), typeof(Geometry),
            typeof(StateCheckBox),
            PredefinedShapes.LineHorizontal,
            BindingMode.Default,
            propertyChanged: null
            );

        public static readonly BindableProperty SecondIconColorProperty = BindableProperty.Create(
            nameof(SecondIconColor), typeof(Color),
            typeof(StateCheckBox),
            Color.Red,
            BindingMode.Default,
            propertyChanged: null
            );

        #endregion



        #region Events
        public event EventHandler StateChanged;

        #endregion



        #region Contructor
        public StateCheckBox()
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
                    ChangeState();

                    // ClickEvent?.Invoke ...

                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParemeter))
                            Command.Execute(CommandParemeter);
                    }

                    StateChanged?.Invoke(this, new EventArgs());

                })
            });


        }

        #endregion



        #region Methods
        void UpdateVisuals()
        {
            switch (State)
            {
                case CheckBoxState.Unchecked:
                    outlineBox.Fill = new SolidColorBrush(Color.Transparent);
                    iconPath.Fill = new SolidColorBrush(Color.Transparent);
                    break;
                case CheckBoxState.Checked:
                    iconPath.Data = IconGeometry;
                    outlineBox.Fill = new SolidColorBrush(BoxColor);
                    iconPath.Fill = new SolidColorBrush(IconColor);
                    break;
                case CheckBoxState.Cross:
                    iconPath.Data = SecondIconGeometry;
                    outlineBox.Fill = new SolidColorBrush(BoxColor);
                    iconPath.Fill = new SolidColorBrush(SecondIconColor);
                    break;
            }
            IconBounceOnPressAsync();
        }

        void ChangeState()
        {
            switch (State)
            {
                case CheckBoxState.Unchecked:
                    State = CheckBoxState.Checked;
                    break;
                case CheckBoxState.Checked:
                    State = CheckBoxState.Cross;
                    break;
                case CheckBoxState.Cross:
                    State = CheckBoxState.Unchecked;
                    break;
            }
            UpdateVisuals();

        }

        //public static void OnStateChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (StateCheckBox)bindable;
        //    control.UpdateVisuals();

        //}

        //public static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (StateCheckBox)bindable;
        //    control.WidthRequest = (int)newValue;
        //    control.HeightRequest = (int)newValue;
        //    control.outlineBox.WidthRequest = (int)newValue;
        //    control.outlineBox.HeightRequest = (int)newValue;
        //    control.iconPath.WidthRequest = (int)newValue;
        //    control.iconPath.HeightRequest = (int)newValue;
        //}

        #endregion

        public enum CheckBoxState
        {
            Unchecked = 0,
            Checked = 1,
            Cross = 2
        }

    }

}
