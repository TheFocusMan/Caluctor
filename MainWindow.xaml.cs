using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace CaluctorSchoolProect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double _addnum;
        double _orgnum;
        bool _res;
        public MainWindow()
        {
            InitializeComponent();
            foreach (Button g in BasicPanel.Children)
            {
                g.Click += BasicAction;
            }
            // add function to calculator
            var met = typeof(Math).GetMethods().ToList();
            met.AddRange(typeof(MathHelper).GetMethods());
            int i = 0;
            foreach (var gg in met)
            {
                bool valid = false;
                foreach (var ggbh in gg.GetParameters())
                {
                    valid = ggbh.ParameterType == typeof(double);
                }
                if (gg.ReturnType == typeof(double) && valid)
                {
                    var arrt = gg.GetCustomAttribute<DescriptionAttribute>();
                    Button button = new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = 120,
                        Height = 60,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        Content = arrt != null ? arrt.Description : gg.Name == "Log" && gg.GetParameters().Length == 2 ? "LogX" : gg.Name,
                        Tag = gg,
                        Background = new SolidColorBrush(Color.FromRgb(0, 220, 255))
                    };
                    button.Margin = new Thickness(10 + (button.Width * (i / 5)) + (i / 5 > 0 ? (5 * (i / 5)) : 0),
                        10 + (button.Height * (i % 5)) + (i % 5 > 0 ? (5 * (i % 5)) : 0), 0, 0);
                    HardMathPanel.Children.Add(button);
                    button.Click += BasicAction;
                    i++;
                }
            }
            CheckMemeryCapiticy();
        }

        private void BasicAction(object sender, RoutedEventArgs e)
        {
            var gg = sender as Button;
            SendCalucteCommand((string)gg.Content, gg);
            RefreshText();
        }

        private void SendCalucteCommand(string command, object parm = null)
        {
            switch (command)
            {
                case ".":
                    if (!OutputText.Text.Contains(".")) OutputText.Text += ".";
                    break;
                case "=":
                    RefreshResult();
                    AddMathLog(command, false);
                    _res = true;
                    break;
                case "C":
                    {
                        SendCalucteCommand("CE");
                        foreach (Button fg in BasicPanel.Children)
                        {
                            if (!double.TryParse((string)fg.Content, out double _))
                            {
                                fg.Background = new SolidColorBrush(Color.FromRgb(0, 220, 255));
                            }
                            MathBox.Text = "";
                        }
                    }
                    break;
                case "CE":
                    OutputText.Text = "0";
                    break;
                case "Del":
                    if (!string.IsNullOrEmpty(OutputText.Text))
                        OutputText.Text = OutputText.Text.Remove(OutputText.Text.Length - 1, 1);
                    break;
                case "±":
                    if (!OutputText.Text.Contains("-"))
                        OutputText.Text = OutputText.Text.Insert(0, "-");
                    else OutputText.Text = OutputText.Text.Remove(0, 1);
                    break;
                case "*":
                case "+":
                case "-":
                case "/":
                    RefreshResult();
                    if (!IsActionActive())
                        SendCalucteCommand("CE");
                    (((Button)parm).Background as SolidColorBrush).Color = Colors.Orange;
                    AddMathLog(command, false);
                    break;
                default:
                    if (_res )//|| _clickedonact)
                    {
                        SendCalucteCommand("CE");
                        if (MathBox.Text.EndsWith("=") || (!IsActionActive()))
                            MathBox.Text = "";
                        _res = false;
                    }
                    if (double.TryParse(command, out double _))
                    {
                        OutputText.Text += command;
                    }
                    else
                    {
                        var h = (parm as Button)?.Tag as MethodInfo;
                        if (h != null)
                        {
                            RefreshResult();
                            if (h.GetParameters().Length == 1)
                                OutputText.Text = ((double)((parm as Button).Tag as MethodInfo).Invoke(null, new object[] { _orgnum })).ToString();
                            else
                            {
                                if (!IsActionActive())
                                    SendCalucteCommand("CE");
                                (((Button)parm).Background as SolidColorBrush).Color = Colors.Orange;
                            }
                            AddMathLog(command, true,h.GetCustomAttribute<OutputFormatAttribute>());
                        }
                    }
                    break;
            }
        }

        private void AddMathLog(string command, bool itshimath,OutputFormatAttribute attribute=null)
        {
            if (!_res)
            {
                MathBox.Text += string.Format(attribute != null ? " "+attribute.Format : itshimath ? " {1} {0}" : " {0} {1}", _orgnum, command);
            }
        }

        private void RefreshText()
        {
            int zerozins = 0;
            bool zerozafter = false;
            foreach (var g in OutputText.Text)
            {
                if (g != '0')
                {
                    zerozafter = true;
                    break;
                }
                zerozins++;
            }
            if (zerozins > 1 || (zerozafter && !OutputText.Text.Contains(".")))
            {
                OutputText.Text = OutputText.Text.TrimStart('0');
            }
            if (string.IsNullOrEmpty(OutputText.Text)) OutputText.Text = "0";
        }

        private void RefreshResult()
        {
            if (double.TryParse(OutputText.Text, out double gg))
            {
                OutputText.Text = gg.ToString();
                var lst = BasicPanel.Children.OfType<UIElement>().ToList();
                lst.AddRange(HardMathPanel.Children.OfType<UIElement>());
                foreach (Button fg in lst)
                {
                    if ((fg.Background as SolidColorBrush).Color == Colors.Orange)
                    {
                        _addnum = gg;
                        switch (fg.Content)
                        {
                            case "+":
                                _orgnum += _addnum;
                                break;
                            case "/":
                                _orgnum /= _addnum;
                                break;
                            case "-":
                                _orgnum -= _addnum;
                                break;
                            case "*":
                                _orgnum *= _addnum;
                                break;
                            default:
                                _orgnum = (double)(fg.Tag as MethodInfo).Invoke(null, new object[] { _orgnum, _addnum });
                                break;
                        }
                        OutputText.Text = _orgnum.ToString();
                    }
                    if (!double.TryParse((string)fg.Content, out double _))
                    {
                        fg.Background = new SolidColorBrush(Color.FromRgb(0, 220, 255));
                    }
                }
                _orgnum = gg;
            }
            else
            {
                OutputText.Text = "Invalid Input";
                _res = true;
            }
        }

        private bool IsActionActive()
        {
            foreach (Button fg in BasicPanel.Children)
                if ((fg.Background as SolidColorBrush).Color == Colors.Orange) 
                    return true;
            return false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                case Key.Back:
                    SendCalucteCommand("Del");
                    break;
                case Key.Enter:
                    SendCalucteCommand("=");
                    break;
                case Key.L:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && CheckMemeryCapiticy())
                        ClearAllMemory(sender, e);
                    break;
                case Key.R:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && CheckMemeryCapiticy())
                        RecallMemory(sender, e);
                    break;
                case Key.M:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
                        SaveMemery(sender, e);
                    break;
                case Key.P:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
                        AddMemory(sender, e);
                    break;
                case Key.Q:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
                        SubtractMemory(sender, e);
                    break;
            }
            RefreshText();
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            SendCalucteCommand(e.Text, FindButton(e.Text));
            RefreshText();
        }

        private void SaveMemery(object sender, RoutedEventArgs e)
        {
            var hg = new MemeryNumberPanel() { NumberMemery = double.Parse(OutputText.Text) };
            hg.MouseUp += (s, f) => { 
                if (f.ChangedButton == MouseButton.Left)
                    RecallMemory(s, f);
            };
            hg.ClearMem.Click += ClearAllMemory;
            hg.AddMem.Click += AddMemory;
            hg.SubcartMem.Click += SubtractMemory;
            MemeryPanel.Children.Insert(0,hg);
            CheckMemeryCapiticy();
        }

        private void ClearAllMemory(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                MemeryPanel.Children.Remove(button.Tag as MemeryNumberPanel);
            }
            else MemeryPanel.Children.Clear();
            CheckMemeryCapiticy();
        }

        private bool CheckMemeryCapiticy()
        {
            return RecallMemBnt.IsEnabled = ClearAllbnt.IsEnabled = MemeryPanel.Children.Count != 0;
        }

        private void RecallMemory(object sender, RoutedEventArgs e)
        {
            var h = (sender as MemeryNumberPanel) != null ? sender as MemeryNumberPanel : (MemeryNumberPanel)MemeryPanel.Children.OfType<UIElement>().First();
            OutputText.Text = h.NumberMemery.ToString();
        }

        private void AddMemory(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
                (button.Tag as MemeryNumberPanel).NumberMemery += double.Parse(OutputText.Text);
            else
            {
                if (!CheckMemeryCapiticy()) SaveMemery(sender, e);
                else
                {
                    var h = (MemeryNumberPanel)MemeryPanel.Children.OfType<UIElement>().First();
                    h.NumberMemery +=  double.Parse(OutputText.Text);
                }
            }
        }

        private void SubtractMemory(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
                (button.Tag as MemeryNumberPanel).NumberMemery -= double.Parse(OutputText.Text);
            else
            {
                if (!CheckMemeryCapiticy()) SaveMemery(sender, e);
                else
                {
                    var h = (MemeryNumberPanel)MemeryPanel.Children.OfType<UIElement>().First();
                    h.NumberMemery -= double.Parse(OutputText.Text);
                }
            }
        }

        private Button FindButton(string text)
        {
            foreach (Button g in BasicPanel.Children)
            {
                if ((string)g.Content == text) return g;
            }
            return null;
        }
    }
}
