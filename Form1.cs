using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NCalc;

namespace RungeKuttaVisualStudio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        


        private void Form1_Load(object sender, EventArgs e)
        {
            // Define las columnas del DataGridView
            dataGridView1.Columns.Add("Columna1", "Xn");
            dataGridView1.Columns.Add("Columna2", "RK1");
            dataGridView1.Columns.Add("Columna3", "RK2");
            dataGridView1.Columns.Add("Columna4", "RK4");
        }

        private void btnCalculate_Click_Click(object sender, EventArgs e) {
            // Lee los valores de la interfaz de usuario
            string equation = Ecuacion.Text;
            double x0 = double.Parse(X0.Text);
            double y0 = double.Parse(Y0.Text);
            double xFinal = double.Parse(X.Text);
            double h = double.Parse(H.Text);
            int verificar = 0;

            List<double> tsExact = new List<double>();
            List<double> ysExact = new List<double>();
            double xExact = x0;
            if (equation == "x+2-y")
            {
                while (xExact <= xFinal)
                {
                    tsExact.Add(xExact);
                    ysExact.Add(SolveExactEquation(x0, y0, equation, xExact));
                    xExact += h;
                    // MessageBox.Show(CalculateExact(equation, xExact, y0).ToString(), "Mensaje");

                }
                verificar = 1;
            }
            else if (equation == "2*y-3")
            {
                while (xExact <= xFinal)
                {
                    tsExact.Add(xExact);
                    ysExact.Add(SolveExactEquation(x0, y0, equation, xExact));
                    xExact += h;
                    //                    MessageBox.Show( xExact.ToString(), "Mensaje");

                    //MessageBox.Show(SolveExactEquation(xExact, 0, equation, xExact).ToString(), "Mensaje");

                }
                verificar = 1;

            }
           


            string method = comboBox1.SelectedItem.ToString();
            List<double> ts = new List<double>();
            List<double> ysMethod = new List<double>();
            List<double> ysRK1 = new List<double>();
            List<double> ysRK2 = new List<double>();
            List<double> ysRK4 = new List<double>();

            double y = y0;
            double x = x0;
            double y1 = y0;
            double y2 = y0;
            double y4 = y0;
            // Realiza los cálculos para el método seleccionado
            int todos = 0;
            if (method == "TODOS")
            {
                todos = 1;
                while (x <= xFinal)
                {
                    ts.Add(x);
                    ysRK1.Add(y1);
                    ysRK2.Add(y2);
                    ysRK4.Add(y4);
                    y1 = RK1(equation, x, y1, h);
                    y2 = RK2(equation, x, y2, h);
                    y4 = RK4(equation, x, y4, h);
                    x += h;
                }
                PlotGraphAll(ts, ysRK1, ysRK2, ysRK4);
            }
            else
            {
                while (x <= xFinal)
                {
                    ts.Add(x);
                    ysMethod.Add(y);

                    if (method == "RK1")
                    {
                        y = RK1(equation, x, y, h);
                    }
                    else if (method == "RK2")
                    {
                        y = RK2(equation, x, y, h);
                    }
                    else if (method == "RK4")
                    {
                        y = RK4(equation, x, y, h);
                    }

                    x += h;
                }

                
            }
            if (verificar != 0)
            {
                // Llena la DataGridView con datos de errores
                dataGridView1.Rows.Clear();
                for (int i = 0; i < tsExact.Count; i++)
                {
                    double exactValue = ysExact[i];
                    double errorRK1 = exactValue - RK1(equation, tsExact[i], y0, h);
                    double errorRK2 = exactValue - RK2(equation, tsExact[i], y0, h);
                    double errorRK4 = exactValue - RK4(equation, tsExact[i], y0, h);

                    dataGridView1.Rows.Add(tsExact[i], errorRK1, errorRK2, errorRK4);
                }
                // Mostrar la gráfica del método seleccionado
                if (todos != 1)
                {
                    PlotGraph(ts, ysMethod, ysExact);

                }
                else
                {
                    PlotGraphAllyexacta(ts, ysRK1, ysRK2, ysRK4, ysExact);
                }

            }
            else if (todos != 1)
            {
                // Mostrar la gráfica del método seleccionado
                PlotGraphnografica(ts, ysMethod);
            }
            else {
                PlotGraphAll(ts, ysRK1, ysRK2, ysRK4);

            }


        }

        private double SolveExactEquation(double x0,double y0, string equation,double hpro)
        {
            if (equation == "x+2-y")
            {
                // Para la ecuación y' = x + 2 - y

                double C = (y0 - x0 - 1) / Math.Exp(-x0);
                return  hpro + 1 + C * Math.Exp(-(hpro));
            }
            else if (equation == "2*y-3")
            {
                // Para la ecuación y' = 2y - 3
                double C = (y0 - 1.5) / Math.Exp(2 * x0);
               // MessageBox.Show( C.ToString(), "Mensaje");

                return (1.5 + C * Math.Exp(2 * hpro));
            }
            else
            {
                // Si la ecuación no es reconocida, devolver 0 como un valor predeterminado
                return 0;
            }
        }

        private double RK1(string equation, double x, double y, double h)
        {
            Expression expression = new Expression(equation);
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y;
            double k1 = Convert.ToDouble(expression.Evaluate());
            return y + h * k1;
        }

        private double RK2(string equation, double x, double y, double h)
        {
            Expression expression = new Expression(equation);
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y;
            double k1 = Convert.ToDouble(expression.Evaluate());
            expression.Parameters["x"] = x + h;
            expression.Parameters["y"] = y + h * k1;
            double k2 = Convert.ToDouble(expression.Evaluate());
            return y + h * (k1 + k2) / 2;
        }

        private double RK4(string equation, double x, double y, double h)
        {
            Expression expression = new Expression(equation);
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y;
            double k1 = h * Convert.ToDouble(expression.Evaluate());

            expression.Parameters["x"] = x + h / 2;
            expression.Parameters["y"] = y + k1 / 2;
            double k2 = h * Convert.ToDouble(expression.Evaluate());

            expression.Parameters["x"] = x + h / 2;
            expression.Parameters["y"] = y + k2 / 2;
            double k3 = h * Convert.ToDouble(expression.Evaluate());

            expression.Parameters["x"] = x + h;
            expression.Parameters["y"] = y + k3;
            double k4 = h * Convert.ToDouble(expression.Evaluate());

            return y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
        }

        private void PlotGraph(List<double> ts, List<double> ysMethod, List<double> ysExact)
        {
            // Limpiar el gráfico antes de añadir nuevas series
            chart.Series.Clear();

            var chartArea = new ChartArea();
            chartArea.AxisX.Minimum = ts.Min(); // Establecer el valor mínimo del eje X
            chartArea.AxisX.Maximum = ts.Max(); // Establecer el valor máximo del eje X
            chartArea.AxisY.Minimum = Math.Min(ysMethod.Min(), ysExact.Min()); // Establecer el valor mínimo del eje Y
            chartArea.AxisY.Maximum = Math.Max(ysMethod.Max(), ysExact.Max()); // Establecer el valor máximo del eje Y

            var seriesExact = new Series("Exacta");
            var seriesMethod = new Series(comboBox1.SelectedItem.ToString());

            // Configurar estilo de las series
            seriesExact.Color = System.Drawing.Color.Blue;
            seriesExact.BorderWidth = 2;
            seriesExact.ChartType = SeriesChartType.Spline; // Gráfica con curvas suaves
            seriesMethod.Color = System.Drawing.Color.Red;
            seriesMethod.BorderWidth = 2;
            seriesMethod.ChartType = SeriesChartType.Point;

            for (int i = 0; i < ts.Count; i++)
            {
                seriesExact.Points.AddXY(ts[i], ysExact[i]);
                seriesMethod.Points.AddXY(ts[i], ysMethod[i]);
            }

            chart.Series.Add(seriesExact);
            chart.Series.Add(seriesMethod);
        }
        private void PlotGraphnografica(List<double> ts, List<double> ysMethod)
        {
            // Limpiar el gráfico antes de añadir nuevas series
            chart.Series.Clear();

            var chartArea = new ChartArea();
            chartArea.AxisX.Minimum = ts.Min(); // Establecer el valor mínimo del eje X
            chartArea.AxisX.Maximum = ts.Max(); // Establecer el valor máximo del eje X
            chartArea.AxisY.Minimum = ysMethod.Min(); // Establecer el valor mínimo del eje Y
            chartArea.AxisY.Maximum = ysMethod.Max(); // Establecer el valor máximo del eje Y

            var seriesMethod = new Series(comboBox1.SelectedItem.ToString());


            seriesMethod.Color = System.Drawing.Color.Red;
            seriesMethod.BorderWidth = 2;
            seriesMethod.ChartType = SeriesChartType.Spline; // Gráfica con curvas suaves

            for (int i = 0; i < ts.Count; i++)
            {
                seriesMethod.Points.AddXY(ts[i], ysMethod[i]);
            }

            chart.Series.Add(seriesMethod);
        }
        private void PlotGraphAll(List<double> ts, List<double> ysRK1, List<double> ysRK2, List<double> ysRK4)
        {
            // Limpiar el gráfico antes de añadir nuevas series
            chart.Series.Clear();

            var chartArea = new ChartArea();
            chartArea.AxisX.Minimum = ts.Min(); // Establecer el valor mínimo del eje X
            chartArea.AxisX.Maximum = ts.Max(); // Establecer el valor máximo del eje X

            // Determinar el valor mínimo y máximo del eje Y entre todas las listas de valores
            double minY =(ysRK1).Min();
            double maxY =(ysRK1).Max();

       

            var seriesRK1 = new Series("RK1");
            var seriesRK2 = new Series("RK2");
            var seriesRK4 = new Series("RK4");
            // Configurar estilo de las series
            seriesRK1.Color = System.Drawing.Color.Blue;
            seriesRK1.BorderWidth = 2;
            seriesRK1.ChartType = SeriesChartType.Point;
            seriesRK2.Color = System.Drawing.Color.Red;
            seriesRK2.BorderWidth = 2;
            seriesRK2.ChartType = SeriesChartType.Point;

            seriesRK4.Color = System.Drawing.Color.Green;
            seriesRK4.BorderWidth = 2;
            seriesRK4.ChartType = SeriesChartType.Point;

            // Añadir puntos de las series correspondientes
            for (int i = 0; i < ts.Count; i++)
            {
                seriesRK1.Points.AddXY(ts[i], ysRK1[i]);
                seriesRK2.Points.AddXY(ts[i], ysRK2[i]);
                seriesRK4.Points.AddXY(ts[i], ysRK4[i]);
            }

            chart.Series.Add(seriesRK1);
            chart.Series.Add(seriesRK2);
            chart.Series.Add(seriesRK4);
        }
        
               private void PlotGraphAllyexacta(List<double> ts, List<double> ysRK1, List<double> ysRK2, List<double> ysRK4, List<double> ysExact)
        {
            // Limpiar el gráfico antes de añadir nuevas series
            chart.Series.Clear();

            var chartArea = new ChartArea();
            chartArea.AxisX.Minimum = ts.Min(); // Establecer el valor mínimo del eje X
            chartArea.AxisX.Maximum = ts.Max(); // Establecer el valor máximo del eje X

            // Determinar el valor mínimo y máximo del eje Y entre todas las listas de valores
            double minY = (ysRK1).Min();
            double maxY = (ysRK1).Max();


            var seriesExact = new Series("Exacta");

            var seriesRK1 = new Series("RK1");
            var seriesRK2 = new Series("RK2");
            var seriesRK4 = new Series("RK4");

            // Configurar estilo de las series
            seriesRK1.Color = System.Drawing.Color.Blue;
            seriesRK1.BorderWidth = 2;
            seriesRK1.ChartType = SeriesChartType.Point;
            seriesRK2.Color = System.Drawing.Color.Red;
            seriesRK2.BorderWidth = 2;
            seriesRK2.ChartType = SeriesChartType.Point;

            seriesRK4.Color = System.Drawing.Color.Green;
            seriesRK4.BorderWidth = 2;
            seriesRK4.ChartType = SeriesChartType.Point;


            // Configurar estilo de las series
            seriesExact.Color = System.Drawing.Color.Blue;
            seriesExact.BorderWidth = 2;
            seriesExact.ChartType = SeriesChartType.Spline; // Gráfica con curvas suaves
            // Añadir puntos de las series correspondientes
            for (int i = 0; i < ts.Count; i++)
            {
                seriesRK1.Points.AddXY(ts[i], ysRK1[i]);
                seriesRK2.Points.AddXY(ts[i], ysRK2[i]);
                seriesRK4.Points.AddXY(ts[i], ysRK4[i]);
                seriesExact.Points.AddXY(ts[i], ysExact[i]);
            }
            chart.Series.Add(seriesExact);

            chart.Series.Add(seriesRK1);
            chart.Series.Add(seriesRK2);
            chart.Series.Add(seriesRK4);
        }

        private void chart_Click(object sender, EventArgs e)
        {

        }

        private void lblResultRK2_Click(object sender, EventArgs e)
        {

        }

        private void lblResultRK4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void X0_TextChanged(object sender, EventArgs e)
        {

        }

        private void Y0_TextChanged(object sender, EventArgs e)
        {

        }

        private void X_TextChanged(object sender, EventArgs e)
        {

        }

        private void H_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
