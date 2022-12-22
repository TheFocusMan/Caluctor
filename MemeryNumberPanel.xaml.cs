using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaluctorSchoolProect
{
    /// <summary>
    /// Interaction logic for MemeryNumberPanel.xaml
    /// </summary>
    public partial class MemeryNumberPanel : Grid
    {
        public MemeryNumberPanel()
        {
            InitializeComponent();
            this.ClearMem.Tag = this.SubcartMem.Tag = this.AddMem.Tag = this;
        }

        public double NumberMemery 
        { 
            get => double.Parse(textBlock.Text); 
            set => textBlock.Text = value.ToString();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ControlsPanel.Visibility = Visibility.Visible;
            this.Background = new SolidColorBrush(Colors.LightGray) { Opacity = 0.5 };
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = null;
            ControlsPanel.Visibility = Visibility.Hidden;
        }
    }
}
