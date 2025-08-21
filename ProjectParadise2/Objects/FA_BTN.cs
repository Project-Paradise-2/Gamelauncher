using FontAwesome.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectParadise2.Objects
{
    public class FA_BTN : RadioButton
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(FontAwesomeIcon), typeof(FA_BTN), new PropertyMetadata(FontAwesomeIcon.FontAwesome));

        public static readonly DependencyProperty IconDirectionProperty = DependencyProperty.Register(
            "IconDirection", typeof(HorizontalAlignment), typeof(FA_BTN), new PropertyMetadata(HorizontalAlignment.Right));

        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
            "IconMargin", typeof(Thickness), typeof(FA_BTN), new PropertyMetadata(new Thickness(0)));

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(
            "IconColor", typeof(Brush), typeof(FA_BTN), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            "TextMargin", typeof(Thickness), typeof(FA_BTN), new PropertyMetadata(new Thickness(0)));

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public FontAwesomeIcon Icon
        {
            get { return (FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public HorizontalAlignment IconDirection
        {
            get { return (HorizontalAlignment)GetValue(IconDirectionProperty); }
            set { SetValue(IconDirectionProperty, value); }
        }

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }
    }
}

