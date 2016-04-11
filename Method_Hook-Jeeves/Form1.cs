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
        double x1,x2, xt1, xt2; // точки
        int vpx1, vpx2; // вектор приращения
        double step; // коэффициент уменьшения шага
        double acc; // точность
        string func; // строка для сохранения функции
        Boolean flag; //флаг проверки
        double[] fun; //массив хранящий значения функций
        int count; //счетчик
        Boolean xflag; //переменная отражает успех или неудачу исследующего поиска x1,x2
        double f_old; // переменная нужна для сравнения новой функции
        Boolean iflag; // флаг отражает был ли весь поиск успешным или нет
        double[] mainx1,mainx2; //массивы хранящие базовые точки
        double kp1,kp2,kp1_old,kp2_old; // точки, построенные при движении по образцу

        public MainForm()
        {
            InitializeComponent();
        }

        private void calculation_Click(object sender, EventArgs e)
        {
            // параметры формы
            x1 = Convert.ToDouble(textBox2.Text);
            x2 = Convert.ToDouble(textBox3.Text);
            func = textBox1.Text;
            vpx1 = Convert.ToInt32(textBox5.Text);
            vpx2 = Convert.ToInt32(textBox4.Text);
            step = Convert.ToDouble(textBox6.Text);
            fun = new double[1000];
            mainx1 = new double[1000];
            mainx2 = new double[1000];
            flag = true;
            if (step <= 1)
            {
                MessageBox.Show("Шаг должен быть > 1", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                flag = false;
            }
            acc = Convert.ToDouble(textBox7.Text);
            if (acc >= 1 || acc < 0)
            {
                MessageBox.Show("Точность должна быть <1 и >0", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                flag = false;
                
            }
            if (flag == true) start();
        }

        public double regexpr(double x, double y)
        {
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", x);
            parser.Values.Add("x2", y);
            return parser.Parse(textBox1.Text);
        }
        
        public Boolean uspex(double a, double b)
        {
            if (a < b)
                return true;
            else
                return false;
        }

        public void ipoisk(double xodin, double xdva, int p)
        {
            f = regexpr(xodin, xdva);
            xflag = uspex(f, f_old);
            if (p == 0)
                textBox8.AppendText("Фиксируя переменную x" + (p+1) + "=" + xdva + ", дадим приращение x" + Environment.NewLine);
            else
                textBox8.AppendText("Фиксируя переменную x" + (p+1) + "=" + xodin + ", дадим приращение x" + Environment.NewLine);
            textBox8.AppendText("f(" + xodin + "," + xdva + ")= " + f + " < " + f_old + " ? " + xflag + Environment.NewLine);
            if (xflag == true)
            {
                if (p == 0)
                    x1 = xt1;
                else
                    x2 = xt2;
                f_old = f;
                iflag = true;
            }
        }


        public void start()
        {
            // Шаг 1. Инициализация
            count = 0;
            fun[count] = regexpr(x1,x2);
            textBox8.AppendText("Вычислим значение функции в т. (" + x1 + "," + x2 + "). f="+fun[count]+ Environment.NewLine);
            mainx1[0] = x1;
            mainx2[0] = x2;

            // Шаг 2. Исследующий поиск
            textBox8.AppendText("Исследующий поиск: " + Environment.NewLine);
            f_old = fun[count];
            iflag = false;
            // исследующий поиск x1
            xt1 = x1 + vpx1;
            ipoisk(xt1,x2,0);
            // исследующий поиск x2
            xt2 = x2 + vpx2;
            ipoisk(x1,xt2,1);
            if (iflag == false)
            {   // исследующий поиск x1 в противоположном направлении 
                xt1 = x1 - vpx1;
                ipoisk(xt1, x2, 0);
                // исследующий поиск x2 в противоположном направлении 
                xt2 = x2 - vpx2;
                ipoisk(x1, xt2, 1);
            }

            textBox8.AppendText("-----------------------------------------------------------------------------" + Environment.NewLine);
            // Шаг 3. Проверка успеха исследующего поиска
            if (iflag == true)
            {

                kp1_old = mainx1[count];
                kp2_old = mainx2[count];
                do
                {
                count = count + 1;
                mainx1[count] = x1;
                mainx2[count] = x2;
                fun[count] = regexpr(mainx1[count], mainx2[count]);
                textBox8.AppendText("x"+count+"=("+x1+","+x2+")="+fun[count]+Environment.NewLine);
                
                // поиск по образцу
                kp1 = mainx1[count] + (mainx1[count] - kp1_old);
                kp2 = mainx2[count] + (mainx2[count] - kp2_old);
                f = regexpr(kp1,kp2);
                if (f >= fun[count])
                {
                    kp1 = kp1_old;
                    kp2 = kp2_old;
                    f = regexpr(kp1,kp2);
                }
                
                textBox8.AppendText("Поиск по образцу:"+ Environment.NewLine);
                textBox8.AppendText("p(" + kp1 + "," + kp2 + ")=" + f + Environment.NewLine);

                // исследующий поиск
                textBox8.AppendText("Исследующий поиск: " + Environment.NewLine);
                f_old = f;
                iflag = false;
                x1 = kp1;
                x2 = kp2;
                // исследующий поиск x1
                xt1 = x1 + vpx1;
                ipoisk(xt1, x2, 0);
                // исследующий поиск x2
                xt2 = x2 + vpx2;
                ipoisk(x1, xt2, 1);
                if (iflag == false)
                {   // исследующий поиск x1 в противоположном направлении 
                    xt1 = x1 - vpx1;
                    ipoisk(xt1, x2, 0);
                    // исследующий поиск x2 в противоположном направлении 
                    xt2 = x2 - vpx2;
                    ipoisk(x1, xt2, 1);
                }

                } while (count<9);
            }
        }
        
    }
}
