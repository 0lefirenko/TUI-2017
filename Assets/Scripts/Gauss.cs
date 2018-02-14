using System;
using System.Collections;



namespace Gauss {

    public struct ComplexDouble : System.IFormattable, System.IEquatable<ComplexDouble>
    {
        public const double Pi = 3.141592653589793238462643383279502884197169399311481966593000573842001111842089549379151d;
        public static readonly ComplexDouble I = new ComplexDouble(0d, 1d);
        public double Re { get; set; }
        public double Im { get; set; }
        public double Abs
        {
            get { return System.Math.Sqrt(this.SqrAbs); }
            set { this *= value / this.Abs; }
        }
        public double SqrAbs
        {
            get { return this.Re * this.Re + this.Im * this.Im; }
            set { this *= System.Math.Sqrt(value / this.SqrAbs); }
        }
        public double Arg
        {
            get { return System.Math.Atan2(this.Im, this.Re); }
            set
            {
                double abs = this.Abs;
                this.Re = abs * System.Math.Cos(value);
                this.Im = abs * System.Math.Sin(value);
            }
        }
        public ComplexDouble MultipleI { get { return new ComplexDouble(-this.Im, this.Re); } }
        public ComplexDouble DivideI { get { return new ComplexDouble(this.Im, -this.Re); } }
        public ComplexDouble(double x, double y) : this() { this.Re = x; this.Im = y; }

        public bool Equals(ComplexDouble other) { return this.Re.Equals(other.Re) && this.Im.Equals(other.Im); }
        public override bool Equals(object obj)
        {
            try { return this.Equals((ComplexDouble)obj); }
            catch { return false; }
        }
        public override int GetHashCode() { return this.Re.GetHashCode() + this.Im.GetHashCode(); }
        public override string ToString()
        {
            return this.Re.ToString() + (this.Im < 0 ? " " : " +") + this.Im.ToString() + 'i';
        }
        public string ToString(string format, System.IFormatProvider provider)
        {
            return this.Re.ToString(format, provider) + (this.Im < 0 ? " " : " +") + this.Im.ToString(format, provider) + 'i';
        }

        public static ComplexDouble Exp(ComplexDouble z)
        {
            return System.Math.Exp(z.Re) * (new ComplexDouble(System.Math.Cos(z.Im), System.Math.Sin(z.Im)));
        }
        public static ComplexDouble Log(ComplexDouble z) { return Log(z, 0); }
        public static ComplexDouble Log(ComplexDouble z, int k)
        {
            return new ComplexDouble(System.Math.Log(z.Abs), z.Arg + 2d * k * Pi);
        }
        public static ComplexDouble Log(ComplexDouble z, int kz, ComplexDouble a, int ka) { return Log(z, kz) / Log(a, ka); }
        public static ComplexDouble Pow(ComplexDouble z, double n)
        {
            return System.Math.Pow(z.Abs, n) * new ComplexDouble(System.Math.Cos(z.Arg * n), System.Math.Sin(z.Arg * n));
        }
        public static ComplexDouble Pow(ComplexDouble z, ComplexDouble n) { return Pow(z, n, 0); }
        public static ComplexDouble Pow(ComplexDouble z, ComplexDouble n, int k) { return Exp(n * Log(z, k)); }
        public static ComplexDouble Sqrt(ComplexDouble z, int k) { return Pow(z, new ComplexDouble(0.5d, 0d), k); }

        public static ComplexDouble Sin(ComplexDouble z) { return Sinh(z.MultipleI).DivideI; }
        public static ComplexDouble Cos(ComplexDouble z) { return Cosh(z.MultipleI); }
        public static ComplexDouble Tan(ComplexDouble z) { return Tanh(z.MultipleI).DivideI; }
        public static ComplexDouble Cot(ComplexDouble z) { return Coth(z.MultipleI).MultipleI; }
        public static ComplexDouble Sec(ComplexDouble z) { return Sech(z.MultipleI); }
        public static ComplexDouble Csc(ComplexDouble z) { return Csch(z.MultipleI).MultipleI; }
        public static ComplexDouble Sinh(ComplexDouble z) { return Sinh2(z) / 2d; }
        public static ComplexDouble Sinh2(ComplexDouble z) { return (Exp(z) - Exp(-z)); }
        public static ComplexDouble Cosh(ComplexDouble z) { return Cosh2(z) / 2d; }
        public static ComplexDouble Cosh2(ComplexDouble z) { return (Exp(z) + Exp(-z)); }
        public static ComplexDouble Tanh(ComplexDouble z) { return Sinh2(z) / Cosh2(z); }
        public static ComplexDouble Coth(ComplexDouble z) { return Cosh2(z) / Sinh2(z); }
        public static ComplexDouble Sech(ComplexDouble z) { return 2d / Cosh2(z); }
        public static ComplexDouble Csch(ComplexDouble z) { return 2d / Sinh2(z); }

        public static ComplexDouble Asin(ComplexDouble z, int kLog, int kSqrt) { return Asinh(z.MultipleI, kLog, kSqrt).DivideI; }
        public static ComplexDouble Acos(ComplexDouble z, int kLog, int kSqrt) { return Acosh(z, kLog, kSqrt).DivideI; }
        public static ComplexDouble Atan(ComplexDouble z, int kLog) { return Atanh(z.DivideI, kLog).MultipleI; }
        public static ComplexDouble Acot(ComplexDouble z, int kLog) { return Atan(1d / z, kLog); }
        public static ComplexDouble Asec(ComplexDouble z, int kLog, int kSqrt) { return Acos(1d / z, kLog, kSqrt); }
        public static ComplexDouble Acsc(ComplexDouble z, int kLog, int kSqrt) { return Asin(1d / z, kLog, kSqrt); }
        public static ComplexDouble Asinh(ComplexDouble z, int kLog, int kSqrt) { return Log(z + Sqrt(z * z + 1d, kSqrt), kLog); }
        public static ComplexDouble Acosh(ComplexDouble z, int kLog, int kSqrt) { return Log(z + Sqrt(z * z - 1d, kSqrt), kLog); }
        public static ComplexDouble Atanh(ComplexDouble z, int kLog) { return Log((1d + z) / (1d - z), kLog) / 2d; }
        public static ComplexDouble Acoth(ComplexDouble z, int kLog) { return Atanh(1d / z, kLog); }
        public static ComplexDouble Asech(ComplexDouble z, int kLog, int kSqrt) { return Acosh(1d / z, kLog, kSqrt); }
        public static ComplexDouble Acsch(ComplexDouble z, int kLog, int kSqrt) { return Asinh(1d / z, kLog, kSqrt); }

        public static ComplexDouble operator ~(ComplexDouble z) { return new ComplexDouble(z.Re, -z.Im); }
        public static ComplexDouble operator !(ComplexDouble z) { return ~z; }
        public static ComplexDouble operator ++(ComplexDouble z) { return new ComplexDouble(++z.Re, z.Im); }
        public static ComplexDouble operator --(ComplexDouble z) { return new ComplexDouble(--z.Re, z.Im); }
        public static ComplexDouble operator +(ComplexDouble z) { return z; }
        public static ComplexDouble operator -(ComplexDouble z) { return new ComplexDouble(-z.Re, -z.Im); }
        public static ComplexDouble operator +(ComplexDouble z1, ComplexDouble z2) { return new ComplexDouble(z1.Re + z2.Re, z1.Im + z2.Im); }
        public static ComplexDouble operator -(ComplexDouble z1, ComplexDouble z2) { return z1 + -z2; }
        public static ComplexDouble operator *(ComplexDouble z1, ComplexDouble z2) { return new ComplexDouble(z1.Re * z2.Re - z1.Im * z2.Im, z1.Re * z2.Im + z1.Im * z2.Re); }
        public static ComplexDouble operator /(ComplexDouble z1, ComplexDouble z2) { return z1 * ~z2 / z2.SqrAbs; }
        public static ComplexDouble operator %(ComplexDouble z1, ComplexDouble z2) { return new ComplexDouble(z1.Re % z2.Re, z1.Im % z2.Im); }
        public static ComplexDouble operator +(ComplexDouble z1, double d2) { return z1 + new ComplexDouble(d2, 0d); }
        public static ComplexDouble operator -(ComplexDouble z1, double d2) { return z1 + -d2; }
        public static ComplexDouble operator *(ComplexDouble z1, double d2) { return new ComplexDouble(z1.Re * d2, z1.Im * d2); }
        public static ComplexDouble operator %(ComplexDouble z1, double d2) { return z1 % new ComplexDouble(d2, 0d); }
        public static ComplexDouble operator /(ComplexDouble z1, double d2) { return z1 * (1d / d2); }
        public static ComplexDouble operator +(double d1, ComplexDouble z2) { return z2 + d1; }
        public static ComplexDouble operator -(double d1, ComplexDouble z2) { return -z2 + d1; }
        public static ComplexDouble operator *(double d1, ComplexDouble z2) { return z2 * d1; }
        public static ComplexDouble operator /(double d1, ComplexDouble z2) { return new ComplexDouble(d1, 0d) / z2; }
        public static ComplexDouble operator %(double d1, ComplexDouble z2) { return new ComplexDouble(d1, 0d) % z2; }
        public static bool operator ==(ComplexDouble z1, ComplexDouble z2) { return z1.Equals(z2); }
        public static bool operator !=(ComplexDouble z1, ComplexDouble z2) { return !z1.Equals(z2); }
        public static explicit operator ComplexDouble(double d) { return new ComplexDouble(d, 0d); }

    }



    public class LinearSystem {
        private ComplexDouble[,] initial_a_matrix;
        private ComplexDouble[,] a_matrix;  // матрица A
        private ComplexDouble[] x_vector;   // вектор неизвестных x
        private ComplexDouble[] initial_b_vector;
        private ComplexDouble[] b_vector;   // вектор b
        private ComplexDouble[] u_vector;   // вектор невязки U
        private double eps;          // порядок точности для сравнения вещественных чисел 
        private int size;            // размерность задачи


        public LinearSystem(ComplexDouble[,] a_matrix, ComplexDouble[] b_vector, double eps) {


            int b_length = SaveLoadScript.countColor;
            int a_length = SaveLoadScript.countColor;

            this.initial_a_matrix = a_matrix;  // запоминаем исходную матрицу
            this.a_matrix = (ComplexDouble[,])a_matrix.Clone(); // с её копией будем производить вычисления
            this.initial_b_vector = b_vector;  // запоминаем исходный вектор
            this.b_vector = (ComplexDouble[])b_vector.Clone();  // с его копией будем производить вычисления
            this.x_vector = new ComplexDouble[b_length];
            this.u_vector = new ComplexDouble[b_length];
            this.size = b_length;
            this.eps = eps;

            GaussSolve();
        }

        public ComplexDouble[] XVector {
            get {
                return x_vector;
            }
        }

        public ComplexDouble[] UVector {
            get {
                return (u_vector);
            }
        }

        // инициализация массива индексов столбцов
        private int[] InitIndex() {
            int[] index = new int[size];
            for (int i = 0; i < index.Length; ++i)
                index[i] = i;
            return index;
        }

        // поиск главного элемента в матрице
        private ComplexDouble FindR(int row, int[] index) {
            int max_index = row;
            ComplexDouble max = a_matrix[row, index[max_index]];
            ComplexDouble max_abs = max;
            for (int cur_index = row + 1; cur_index < size; ++cur_index) {
                ComplexDouble cur = a_matrix[row, index[cur_index]];
                ComplexDouble cur_abs = cur;
                if (cur_abs.Abs > max_abs.Abs) {
                    max_index = cur_index;
                    max = cur;
                    max_abs = cur_abs;
                }
            }
            int temp = index[row];
            index[row] = index[max_index];
            index[max_index] = temp;

            return max;
        }

        // Нахождение решения СЛУ методом Гаусса
        private void GaussSolve() {
            int[] index = InitIndex();
            GaussForwardStroke(index);
            GaussBackwardStroke(index);
            GaussDiscrepancy();
        }

        // Прямой ход метода Гаусса
        private void GaussForwardStroke(int[] index) {
            for (int i = 0; i < size; ++i) {
                ComplexDouble r = FindR(i, index);
                for (int j = 0; j < size; ++j)
                    a_matrix[i, j] /= r;
                b_vector[i] /= r;
                for (int k = i + 1; k < size; ++k) {
                    ComplexDouble p = a_matrix[k, index[i]];
                    for (int j = i; j < size; ++j)
                        a_matrix[k, index[j]] -= a_matrix[i, index[j]] * p;
                    b_vector[k] -= b_vector[i] * p;
                    a_matrix[k, index[i]] = (ComplexDouble)0.0;
                }
            }
        }

        // Обратный ход метода Гаусса
        private void GaussBackwardStroke(int[] index) {
            for (int i = size - 1; i >= 0; --i) {
                ComplexDouble x_i = b_vector[i];
                for (int j = i + 1; j < size; ++j)
                    x_i -= x_vector[index[j]] * a_matrix[i, index[j]];
                x_vector[index[i]] = x_i;
            }
        }

        // x - решение уравнения, полученное методом Гаусса
        private void GaussDiscrepancy() {
            for (int i = 0; i < size; ++i) {
                ComplexDouble actual_b_i = (ComplexDouble)0.0; 
                for (int j = 0; j < size; ++j)
                    actual_b_i += initial_a_matrix[i, j] * x_vector[j];
                u_vector[i] = initial_b_vector[i] - actual_b_i;
            }
        }
    }
}
