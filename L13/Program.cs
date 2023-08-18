using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

[Flags]
public enum RodzajSilnikaFlag
{
    BENZYNOWY=1,
    DIESEL=2,
    ELEKTRYCZNY=4
}

public delegate bool Wybierz<T>(T t);

[Serializable]
public abstract class Samochod : ICloneable, IComparable<Samochod>
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public RodzajSilnikaFlag RodzajSilnika { get; set; }

    public int RokProdukcji { get; set; }

    public Samochod(string marka,string model,int rokProdukcji)
    {
        Marka = marka;
        Model = model;
        RokProdukcji = rokProdukcji;
    }
    public object Clone()
    {
        return MemberwiseClone();
    }

    public int CompareTo(Samochod other)
    {
        if (this is SamochodElektryczny && other is SamochodElektryczny)
        {
            SamochodElektryczny samochodElektryczny = other as SamochodElektryczny;
            SamochodElektryczny samochodElektryczny1 = this as SamochodElektryczny;
            return samochodElektryczny.PojemnoscBaterii.CompareTo(samochodElektryczny1.PojemnoscBaterii);
        }
        return this.RokProdukcji.CompareTo(other.RokProdukcji);
    }

    public virtual void WyswietlInformacje()
    {
        Console.Write($"\nMarka: {Marka}, Model: {Model}, Rok produkcji: {RokProdukcji}");
    }

    public class SamochodPoModeluComparer :IComparer<Samochod>
    {
        public int Compare(Samochod s1,Samochod s2)
        {
            return s1.Model.CompareTo(s2.Model);
        }
    }
}
[Serializable]
public class SamochodSpalinowy : Samochod
{
    public new object Clone()
    {
        return MemberwiseClone();
    }

    public SamochodSpalinowy(string marka, string model, int rokProdukcji, RodzajSilnikaFlag rodzajSilnika) : base(marka, model, rokProdukcji)
    {
            RodzajSilnika = rodzajSilnika;
        if (rodzajSilnika == RodzajSilnikaFlag.ELEKTRYCZNY)
            throw new ArgumentException("Samochody hybrydowe tworzymy jako hybrydowe, a nie spalinowe!");


    }
    

                

    

    public override void WyswietlInformacje()
    {
        Console.Write($"\nMarka: {Marka}, Model: {Model}, Rok produkcji: {RokProdukcji} Rodzaj silnika: {RodzajSilnika}");
    }
}
[Serializable]
public class SamochodElektryczny : Samochod
{
    public int PojemnoscBaterii { get; set; }
    public new object Clone()
    {
        return MemberwiseClone();
    }

    public SamochodElektryczny(string marka, string model, int rokProdukcji,int pojemnoscBaterii) : base(marka, model, rokProdukcji)
    {
        PojemnoscBaterii = pojemnoscBaterii;
    }

    public override void WyswietlInformacje()
    {
        Console.Write($"\nMarka: {Marka}, Model: {Model}, Rok produkcji: {RokProdukcji} Pojemność baterii: {PojemnoscBaterii} kWh");

    }
}
[Serializable]
public class SamochodHybrydowy: Samochod
{   
    public int PojemnoscBaterii { get; set; }
    public new object Clone()
    {
        return MemberwiseClone();
    }

    public SamochodHybrydowy(string marka, string model, int rokProdukcji, RodzajSilnikaFlag rodzajSilnika, int pojemnoscBaterii) : base(marka, model, rokProdukcji)
    {
        if (rodzajSilnika==RodzajSilnikaFlag.BENZYNOWY || rodzajSilnika==RodzajSilnikaFlag.DIESEL || rodzajSilnika==RodzajSilnikaFlag.ELEKTRYCZNY)
        {
            throw new ArgumentException("Samochód hybrydowy składa się z silnika elektrycznego oraz spalinowego!");
        }
        RodzajSilnika = rodzajSilnika;
        PojemnoscBaterii = pojemnoscBaterii;
    }

    public override void WyswietlInformacje()
    {
        Console.Write($"\nMarka: {Marka}, Model: {Model}, Rok produkcji: {RokProdukcji} Rodzaj silnika: {RodzajSilnika} Pojemność baterii: {PojemnoscBaterii} kWh");
    }
}

[Serializable]
public class SalonSamochodowy
{
    List<Samochod> samochody=new List<Samochod>(){ };

    public SalonSamochodowy() { }

    public void DodajSamochod(Samochod samochod)
    {
        samochody.Add(samochod);
    }

    public void UsunSamochod(Samochod samochod)
    {
        samochody.Remove(samochod);
    }

    public static SalonSamochodowy WczytajSalonZPliku(string nazwaPliku)
    {
        SalonSamochodowy nowySalon=new SalonSamochodowy();
        FileStream fs = new FileStream(nazwaPliku, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            nowySalon = (SalonSamochodowy)formatter.Deserialize(fs);
            return nowySalon;
        }
        catch (SerializationException e)
        {
            Console.WriteLine("\n\tDeserializacja nie powiodła się: " + e.Message);
            return null;
        }
        finally
        {
            Console.WriteLine("\n\tSalon samochodowy został wczytany z pliku.");
            fs.Close();
        }

    }
    
    public void ZapiszSalonDoPliku(string nazwaPliku)
    {
        FileStream fs = new FileStream(nazwaPliku, FileMode.Create);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, this);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("\n\tSerializacja nie powiodła się: " + e.Message);
        }
        finally
        {
            Console.WriteLine("\n\tSalon samochodowy został zapisany do pliku.");
            fs.Close();
        }
        
    }

    public void WyswietlPosortowane()
    {
        samochody.Sort();
        foreach (Samochod samochod in samochody)
        {
            samochod.WyswietlInformacje();
        }
    }

    public void WyswietlPosortowane(IComparer<Samochod> comparer)
    {
        samochody.Sort(comparer);
        foreach (Samochod samochod in samochody)
        {
            samochod.WyswietlInformacje();
        }
    }

    public void WyswietlSamochody()
    {
        if (samochody!=null)
        foreach (Samochod samochod in samochody)
        {
            samochod.WyswietlInformacje();
        }    
    }

    public void WyswietlSamochody(List<Samochod>lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            lista[i].WyswietlInformacje();
        }
    }

    public void WyswietlSamochody(Wybierz<Samochod> w)
    {
        foreach (Samochod samochod in samochody)
            {
            if (w(samochod))
                samochod.WyswietlInformacje();
            }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        FileStream filestream = new FileStream("Wynik.txt", FileMode.Create);
        StreamWriter streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);

        Console.WriteLine("\t\t\t\tBBBBB     M     M    W       W");
        Console.WriteLine("\t\t\t\tB     B    MM   MM    W       W");
        Console.WriteLine("\t\t\t\tB     B    M M M M    W   W   W");
        Console.WriteLine("\t\t\t\tBBBBB      M  M  M    W W W W");
        Console.WriteLine("\t\t\t\tB     B    M     M    WW   WW");
        Console.WriteLine("\t\t\t\tB     B    M     M    W     W");
        Console.WriteLine("\t\t\t\tBBBBB      M     M    W     W");
        Console.WriteLine("\t\t\t\t\tSalon sprzedaży");

        SalonSamochodowy salon = new SalonSamochodowy();

        SamochodSpalinowy spalinowy1 = new SamochodSpalinowy("BMW", "M3", 2022, RodzajSilnikaFlag.BENZYNOWY);
        // Tworzenie i dodawanie 50 instancji samochodów do salonu
        salon.DodajSamochod(spalinowy1);
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "M5", 2023, RodzajSilnikaFlag.DIESEL));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "i3", 2022, 60));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "i3", 2022, 70));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "i3", 2022, 93));
        salon.DodajSamochod(new SamochodHybrydowy("BMW", "330e", 2023, RodzajSilnikaFlag.BENZYNOWY | RodzajSilnikaFlag.ELEKTRYCZNY, 40));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "X5", 2022, RodzajSilnikaFlag.DIESEL));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "X7", 2023, RodzajSilnikaFlag.BENZYNOWY));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "i4", 2022, 70));
        salon.DodajSamochod(new SamochodHybrydowy("BMW", "530e", 2023, RodzajSilnikaFlag.BENZYNOWY | RodzajSilnikaFlag.ELEKTRYCZNY, 50));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "M2", 2022, RodzajSilnikaFlag.BENZYNOWY));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "M8", 2023, RodzajSilnikaFlag.DIESEL));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "iX3", 2022, 65));
        salon.DodajSamochod(new SamochodHybrydowy("BMW", "745e", 2023, RodzajSilnikaFlag.BENZYNOWY | RodzajSilnikaFlag.ELEKTRYCZNY, 45));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "X3", 2022, RodzajSilnikaFlag.DIESEL));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "X6", 2023, RodzajSilnikaFlag.BENZYNOWY));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "i8", 2022, 75));
        salon.DodajSamochod(new SamochodHybrydowy("BMW", "X1 xDrive25e", 2023, RodzajSilnikaFlag.BENZYNOWY | RodzajSilnikaFlag.ELEKTRYCZNY, 55));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "330i", 2022, RodzajSilnikaFlag.BENZYNOWY));
        salon.DodajSamochod(new SamochodSpalinowy("BMW", "430i", 2023, RodzajSilnikaFlag.DIESEL));
        salon.DodajSamochod(new SamochodElektryczny("BMW", "iX5", 2022, 80));
        salon.DodajSamochod(new SamochodHybrydowy("BMW", "X2 xDrive25e", 2023, RodzajSilnikaFlag.BENZYNOWY | RodzajSilnikaFlag.ELEKTRYCZNY, 60));

        Console.WriteLine("\tWszystkie samochody w salonie:");
        salon.WyswietlSamochody();

        // Testowanie serializacji i deserializacji
        string nazwaPliku = "salon.bin";
        salon.ZapiszSalonDoPliku(nazwaPliku);

        Console.WriteLine("\n\tUsuwanie samochodu z salonu...");
        salon.UsunSamochod(spalinowy1);

        Console.WriteLine("\n\tWszystkie samochody po usunięciu:");
        salon.WyswietlSamochody();

        SalonSamochodowy wczytanySalon = SalonSamochodowy.WczytajSalonZPliku(nazwaPliku);

        Console.WriteLine("\n\tWczytane samochody z pliku:");
        wczytanySalon.WyswietlSamochody();

        Console.WriteLine("\n\tWyświetlanie samochodów według poszukiwanego wzorca (delegat i funkcja anonimowa):");
        salon.WyswietlSamochody((s) => s.Model == "M3");
        salon.WyswietlSamochody((s) => s.Model == "M2");

        Console.WriteLine("\n\tWyświetlanie posortowanych po roku produkcji, a elektryczne następnie po pojemności baterii malejąco:");
        salon.WyswietlPosortowane();

        Console.WriteLine("\n\tWyświetlanie po modelu:");
        salon.WyswietlPosortowane(new Samochod.SamochodPoModeluComparer());

        Console.WriteLine("\n\tTest obsługi wyjątków:");
        ExceptionsTester(() => { new SamochodSpalinowy("BMW", "M5", 2023, RodzajSilnikaFlag.ELEKTRYCZNY); });
        ExceptionsTester(() => { new SamochodHybrydowy("BMW", "745e", 2023, RodzajSilnikaFlag.BENZYNOWY, 45); });
        ExceptionsTester(() => { new SamochodHybrydowy("BMW", "745e", 2023, RodzajSilnikaFlag.DIESEL, 45); });
        ExceptionsTester(() => { new SamochodHybrydowy("BMW", "745e", 2023, RodzajSilnikaFlag.ELEKTRYCZNY, 45); });

        Console.SetOut(System.IO.TextWriter.Null);
    }

    public delegate void TesterDelegate();

    public static void ExceptionsTester(TesterDelegate test)
    {
        try
        {
            test();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
