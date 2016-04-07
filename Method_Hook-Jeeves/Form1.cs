using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using info.lundin.math;

namespace Method_Hook_Jeeves
{
    public partial class MainForm : Form
    {
        double f; // функция f(x1,x2)
        double x01, x02; // начальная точка
        double vpx1, vpx2; // вектор приращения
        double step; // коэффициент уменьшения шага
        double acc; // точность

        public MainForm()
        {
            InitializeComponent();
        }

        private void calculation_Click(object sender, EventArgs e)
        {
            x01 = Convert.ToDouble(textBox2.Text);
            x02 = Convert.ToDouble(textBox3.Text);
            vpx1 = Convert.ToDouble(textBox5.Text);
            vpx2 = Convert.ToDouble(textBox4.Text);
            step = Convert.ToDouble(textBox6.Text);
            acc = Convert.ToDouble(textBox7.Text);
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", x01);
            parser.Values.Add("x2", x02);
            f = parser.Parse(textBox1.Text);
            label11.Text = Convert.ToString(f);
        }
        
    }
}
