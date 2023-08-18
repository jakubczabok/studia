
//zad2

public class Matrix
{
    private double[] data;
    private int size;

    public int Size
    {
        get { return size; }
    }

    public Matrix(int size)
    {
        this.size = size;
        data = new double[size * size];
    }

    public void SetValue(int row, int column, double value)
    {
        if (row < 0 || row >= size || column < 0 || column >= size)
        {
            throw new IndexOutOfRangeException("Indeks w macierzy jest poza zakresem.");
        }

        data[row * size + column] = value;
    }

    public double GetValue(int row, int column)
    {
        if (row < 0 || row >= size || column < 0 || column >= size)
        {
            throw new IndexOutOfRangeException("Indeks w macierzy jest poza zakresem.");
        }

        return data[row * size + column];
    }

    public double CalculateSum()
    {
        double sum = 0;

        for (int i = 0; i < size * size; i++)
        {
            sum += data[i];
        }

        return sum;
    }
}



internal class Program
{
    private static void Main(string[] args)
    {

        //zad1

        unsafe
        {
            static void ZamienAdres(int** x2, int** y2)
            {
                int* temp = *x2;
                *x2 = *y2;
                *y2 = temp;
            }

            int x = 1;
            int y = 3;

            int* pX = &x;
            int* pY = &y;

            Console.WriteLine("Przed zamianą:");
            Console.WriteLine("Wartość x: {0}, Adres x: {1}", x, (int)pX);
            Console.WriteLine("Wartość y: {0}, Adres y: {1}", y, (int)pY);

            ZamienAdres(&pX, &pY);

            Console.WriteLine("Po zamianie:");
            Console.WriteLine("Wartość x: {0}, Adres x: {1}", x, (int)pX);
            Console.WriteLine("Wartość y: {0}, Adres y: {1}", y, (int)pY);

            decimal d = 150150150m;
            byte* pointer = (byte*)&d;

            Console.WriteLine("Bajty liczby d:");

            for (int i = 0; i < sizeof(decimal); i++)
            {
                Console.WriteLine("Bajt {0}: {1}", i, *(pointer + i));
            }
        }

        //zad2

        unsafe
        {
            Matrix matrix = new Matrix(3);

            matrix.SetValue(0, 0, 1.0);
            matrix.SetValue(0, 1, 2.0);
            matrix.SetValue(0, 2, 3.0);
            matrix.SetValue(1, 0, 4.0);
            matrix.SetValue(1, 1, 5.0);
            matrix.SetValue(1, 2, 6.0);
            matrix.SetValue(2, 0, 7.0);
            matrix.SetValue(2, 1, 8.0);
            matrix.SetValue(2, 2, 9.0);

            double value = matrix.GetValue(1, 1);
            Console.WriteLine("Wartość elementu (1,1): " + value);

            double sum = matrix.CalculateSum();
            Console.WriteLine("Suma wszystkich elementów macierzy: " + sum);
        }


        
    }
}