using System.IO;
using System.Collections.Generic;
using System.Reflection;

//zad1

public static class PrzetwarzaniePlikow
{
    public static string Wczytaj(string path)
    {
        StreamReader sr = new StreamReader(path);
        return sr.ReadToEnd();
    }

    public static void Zapisz(string s, string path)
    {
        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(s);
    }

    public static void Duplikuj(string path)
    {
        string directory = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);

        int counter = 1;
        string newPath = Path.Combine(directory, $"{fileName}_1{extension}");

        while (File.Exists(newPath))
        {
            counter++;
            newPath = Path.Combine(directory, $"{fileName}_{counter}{extension}");
        }

        string content = Wczytaj(path);
        Zapisz(content, newPath);

        Console.WriteLine($"Utworzono duplikat pliku: {newPath}");

    }
}

//zad2

public static class ListHelper
{
    public static void Wyswietl(this List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i]);
        }
    }
}

public static class ObjectReflector
{
    public static List<string> GetFieldNames(object obj)
    {
        List<string> fieldNames = new List<string>();
        Type objectType = obj.GetType();
        FieldInfo[] fields = objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            fieldNames.Add(field.Name);
        }

        return fieldNames;
    }

    public static List<string> GetMethods(object obj)
    {
        List<string> methodNames = new List<string>();
        Type objectType = obj.GetType();
        MethodInfo[] methods = objectType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach (MethodInfo method in methods)
        {
            methodNames.Add(method.Name);
        }

        return methodNames;
    }

    public static List<string> GetPropertyNames(object obj)
    {
        List<string> propertyNames = new List<string>();
        Type objectType = obj.GetType();
        PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach (PropertyInfo property in properties)
        {
            propertyNames.Add(property.Name);
        }

        return propertyNames;
    }
}

public class Osoba
{
    public string Imie;
    public int Wiek;

    public string Nazwisko { get; set; }

    public Osoba(string imie,string nazwisko,int wiek)
    {
        Imie = imie;
        Wiek = wiek;
        Nazwisko = nazwisko;
    }

    public void UstawWiek(int wiek)
    {
        Wiek = wiek;
    }

    public string ZwrocPelneImie()
    {
        return $"{Imie} {Nazwisko}";
    }

    public int ZwrocWiek()
    {
        return Wiek;
    }
}

public class Samochod
{
    public string Marka;
    public int RokProdukcji;

    public string Model { get; set; }

    public Samochod(string marka, string model, int rokProdukcji)
    {
        Marka = marka;
        RokProdukcji = rokProdukcji;
        Model = model;
    }

    public void UstawRokProdukcji(int rokProdukcji)
    {
        RokProdukcji = rokProdukcji;
    }

    public string ZwrocMarkeModel()
    {
        return $"{Marka} {Model}";
    }

    public int ZwrocRokProdukcji()
    {
        return RokProdukcji;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        //zad1

/*        for (int i = 0; i < 5; i++)
        {

            PrzetwarzaniePlikow.Duplikuj("C:\\Users\\jczab\\Downloads\\lalka.txt");

        }*/

        //zad2

        Osoba o = new Osoba("Paweł", "Kowalski", 25);
        Console.WriteLine("Pola klasy Osoba:");
        ObjectReflector.GetFieldNames(o).Wyswietl();
        Console.WriteLine("Właściwości klasy Osoba:");
        ObjectReflector.GetPropertyNames(o).Wyswietl();
        Console.WriteLine("Metody klasy Osoba:");
        ObjectReflector.GetMethods(o).Wyswietl();

        Console.WriteLine();
        Samochod s = new Samochod("Izera", "Elektryczna", 2999);
        Console.WriteLine("Pola klasy Samochod:");
        ObjectReflector.GetFieldNames(s).Wyswietl();
        Console.WriteLine("Właściwości klasy Samochod:");
        ObjectReflector.GetPropertyNames(s).Wyswietl();
        Console.WriteLine("Metody klasy Samochod:");
        ObjectReflector.GetMethods(s).Wyswietl();
    }
}