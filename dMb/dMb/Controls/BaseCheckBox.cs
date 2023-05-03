using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace dMb.Controls
{
    public abstract partial class BaseCheckBox : Grid, INotifyPropertyChanged
    {
        protected static double _minSize = 25;

        #region Fields
        protected Path outlineBox = new Path
        {
            Fill = new SolidColorBrush(Color.Transparent),
            Stroke = new SolidColorBrush(
                Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.WhiteSmoke : Color.Black),
            StrokeThickness = 2,
            WidthRequest = _minSize,
            HeightRequest = _minSize,
            MinimumWidthRequest = _minSize,
            MinimumHeightRequest = _minSize,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Aspect = Stretch.Uniform,
            Scale = 1,
            Data = PredefinedShapes.RoundedSquare,
        };

        protected Path iconPath = new Path
        {
            Fill = new SolidColorBrush(
                Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.Green : Color.Blue),
            WidthRequest = _minSize,
            HeightRequest = _minSize,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Aspect = Stretch.Uniform,
            Scale = 0.6,
        };

        #endregion



        #region Properties
        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }


        public Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }


        public Color BoxColor
        {
            get => (Color)GetValue(BoxColorProperty);
            set => SetValue(BoxColorProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParemeter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        //public int Size
        //{
        //    get => (int)GetValue(SizeProperty);
        //    set => SetValue(SizeProperty, value);
        //}

        //public double CornerRadius
        //{
        //    get => (double)GetValue(CornerRadiusProperty);
        //    set => SetValue(CornerRadiusProperty, value);
        //}

        //public Stretch Aspect
        //{
        //    get => (Stretch)GetValue(AspectProperty);
        //    set => SetValue(AspectProperty, value);
        //}

        //public LayoutOptions Aligment
        //{
        //    get => (LayoutOptions)GetValue(AligmentProperty);
        //    set => SetValue(AligmentProperty, value);
        //}

        #endregion



        #region BindableProperties
        public static readonly BindableProperty IconGeometryProperty = BindableProperty.Create(
            nameof(IconGeometry), typeof(Geometry),
            typeof(BaseCheckBox),
            PredefinedShapes.Check,
            BindingMode.Default,
            propertyChanged: null
            );

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
            nameof(IconColor), typeof(Color),
            typeof(BaseCheckBox),
            Color.Green,
            BindingMode.Default,
            propertyChanged: null
            );


        public static readonly BindableProperty BoxColorProperty = BindableProperty.Create(
            nameof(BoxColor), typeof(Color),
            typeof(BaseCheckBox),
            (Application.Current.RequestedTheme == OSAppTheme.Dark) ? Color.WhiteSmoke : Color.Black,
            BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BaseCheckBox)bindable;
                control.outlineBox.Stroke = new SolidColorBrush((Color)newValue);
            }
            );

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand),
            typeof(BaseCheckBox),
            null,
            BindingMode.Default,
            propertyChanged: null
            );

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParemeter), typeof(object),
            typeof(BaseCheckBox),
            null,
            BindingMode.Default,
            propertyChanged: null
            );


        //public static readonly BindableProperty SizeProperty = BindableProperty.Create(
        //    nameof(Size), typeof(int),
        //    typeof(StateCheckBox),
        //    25,
        //    BindingMode.Default,
        //    propertyChanged: OnSizeChanged
        //    );

        //public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        //    nameof(CornerRadius), typeof(double),
        //    typeof(StateCheckBox),
        //    8.0,
        //    BindingMode.Default,
        //    propertyChanged: (bindable, oldValue, newValue) =>
        //    {
        //        var control = (StateCheckBox)bindable;

        //        //control.outlineBox.RadiusX = (double)newValue;
        //        //control.outlineBox.RadiusY = (double)newValue;
        //    }
        //    );

        //public static readonly BindableProperty AspectProperty = BindableProperty.Create(
        //    nameof(Aspect), typeof(Stretch),
        //    typeof(StateCheckBox),
        //    Stretch.Uniform,
        //    BindingMode.Default,
        //    propertyChanged: (bindable, oldValue, newValue) =>
        //    {
        //        var control = (StateCheckBox)bindable;
        //        control.iconPath.Aspect = (Stretch)newValue;
        //        control.outlineBox.Aspect = (Stretch)newValue;
        //    }
        //    );

        //public static readonly BindableProperty AligmentProperty = BindableProperty.Create(
        //    nameof(Aligment), typeof(LayoutOptions),
        //    typeof(StateCheckBox),
        //    LayoutOptions.Center,
        //    BindingMode.Default,
        //    propertyChanged: (bindable, oldValue, newValue) =>
        //    {
        //        var control = (StateCheckBox)bindable;
        //        control.iconPath.VerticalOptions = (LayoutOptions)newValue;
        //        control.iconPath.HorizontalOptions = (LayoutOptions)newValue;
        //        control.outlineBox.VerticalOptions = (LayoutOptions)newValue;
        //        control.outlineBox.HorizontalOptions = (LayoutOptions)newValue;
        //    }
        //    );

        //public static readonly BindableProperty Property = BindableProperty.Create(
        //    nameof(filed), typeof(type),
        //    typeof(StateCheckBox),
        //    default,
        //    BindingMode.Default,
        //    propertyChanged: null
        //    );

        #endregion



        #region Events

        #endregion



        #region Contructor
        #endregion



        #region Methods


        protected async Task<bool> BoxBounceOnPressAsync()
        {
            await outlineBox.ScaleTo(1.1, 100, Easing.BounceOut);
            return await outlineBox.ScaleTo(1, 100, Easing.BounceIn);
        }

        protected async Task<bool> IconBounceOnPressAsync()
        {
            iconPath.Scale = 0;
            return await iconPath.ScaleTo(0.8, 200, Easing.SinIn);
        }

        #endregion

    }
}
