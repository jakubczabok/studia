using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;


//zad01

[Serializable]
public class ProduktAGD
{
    decimal cena;
    DateTime dataProdukcji;
    string krajProdukcji;
    string marka;
    string model;
    string nazwa;
    string opis;

    public ProduktAGD(string nazwa, string marka, string model, decimal cena, DateTime dataProdukcji, string krajProdukcji, string opis)
    {
        this.nazwa = nazwa;
        this.marka = marka;
        this.model = model;
        this.cena = cena;
        this.dataProdukcji = dataProdukcji;
        this.krajProdukcji = krajProdukcji;
        this.opis = opis;
    }

    public bool Equals(object obj)
    {
        if (obj is ProduktAGD)
        {
            ProduktAGD objp = obj as ProduktAGD;
            return (this.nazwa == objp.nazwa && this.marka == objp.marka && this.model == objp.model && this.cena == objp.cena && this.dataProdukcji == objp.dataProdukcji && this.krajProdukcji == objp.krajProdukcji && this.opis == objp.opis);
        }
        return false;
    }
    public int GetHashCode()
    {
        return Tuple.Create(nazwa,marka, model ,cena,dataProdukcji,krajProdukcji,opis).GetHashCode();
    }

    public override string ToString()
    {
        return $"{nazwa} {marka} {model} {cena} {dataProdukcji} {krajProdukcji} {opis}";
    }
}

public class SklepAGD
{
    List<ProduktAGD> produkty = new List<ProduktAGD> { };
    public List<ProduktAGD> Produkty
    {
        get { return produkty; }
    }

    public SklepAGD()
    {

    }

    public SklepAGD(List<ProduktAGD> produkty)
    {
        this.produkty = produkty;
    }

    public bool Equals(object obj)
    {
        if (obj is SklepAGD)
        {
            SklepAGD s = obj as SklepAGD;
            if (s.produkty == null && produkty == null)
            {
                return true;
            }
            if (s.produkty != null && produkty != null && s.produkty.Count == produkty.Count)
            {
                for (int i = 0; i < produkty.Count; i++)
                {
                    if (!produkty[i].Equals(s.produkty[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        return false;
    }

    public int GetHashCode()
    {
        return Tuple.Create(produkty).GetHashCode();
    }

    public void DodajProdukt(ProduktAGD produkt)
    {
        produkty.Add(produkt);
    }

    public void UsunProdukt(ProduktAGD produkt)
    {
        produkty.Remove(produkt);
    }

    public void Serializuj(string nazwaPliku)
    {
        FileStream fs = new FileStream(nazwaPliku, FileMode.Create);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, produkty);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Serializacja nie powiodła się: " + e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    public List<ProduktAGD> Deserializuj(string nazwaPliku)
    {
        FileStream fs = new FileStream(nazwaPliku, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<ProduktAGD> noweProdukty = (List<ProduktAGD>)formatter.Deserialize(fs);
            return noweProdukty;
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Deserializacja nie powiodła się: " + e.Message);
            return null;
        }
        finally
        {
            fs.Close();
        }
    }

    public override string ToString()
    {

        StringBuilder sb = new StringBuilder(2500);
        if (produkty != null)
            for (int i = 0; i < produkty.Count; i++)
        {
            sb.AppendLine(produkty[i].ToString());
        }

        return sb.ToString();
    }
}

//zad02

public class DateOfBirthAttribute : ValidationAttribute
{
    public int MinimumAge;
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime)
        {
            DateTime date = (DateTime)value;
            if (date.AddYears(18) >= DateTime.Now)
            {
                return new ValidationResult("Tylko dla osób pełnoletnich!");
            }
        }

        return ValidationResult.Success;
    }
}

public class Klient
{
    [DateOfBirthAttribute(ErrorMessage = "Nieprawidłowy format adresu email")]
    public DateTime DataUrodzenia { get; set; }

    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    [Required(ErrorMessage = "Adres email jest wymagany")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane")]
    public string Imie { get; set; }
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string Nazwisko { get; set; }

    public override string ToString()
    {
        return $"{Imie} {Nazwisko} {Email} {DataUrodzenia}";
    }
}
internal class Program
{
    private static void Main(string[] args)
    {
        //zad01

        ProduktAGD produkt1 = new ProduktAGD(
    "Czajnik",
    "Philips",
    "HD9350/90",
    49.99m,
    new DateTime(2022, 1, 1),
    "Chiny",
    "Czajnik ze stali nierdzewnej z wymiennym filtrem"
);

        ProduktAGD produkt2 = new ProduktAGD(
            "Odkurzacz",
            "Dyson",
            "V11 Absolute",
            599.99m,
            new DateTime(2021, 6, 1),
            "Malezja",
            "Bezprzewodowy odkurzacz z cyfrowym silnikiem"
        );

        SklepAGD sklep = new SklepAGD();
        sklep.DodajProdukt(produkt1);
        sklep.DodajProdukt(produkt2);

        sklep.Serializuj("produkty.bin");
        SklepAGD sklep2 = new SklepAGD(sklep.Deserializuj("produkty.bin"));

        Console.WriteLine(sklep.ToString());
        Console.WriteLine(sklep2.ToString());

        Console.WriteLine(sklep.Equals(sklep2));
        Console.WriteLine(sklep == sklep2);

        //zad02

        var klienci = new List<Klient>()
{
    new Klient() { Nazwisko = "Nowak", Email = "jan_nowak@domena.pl", DataUrodzenia = new DateTime(2000, 1, 1)},
    new Klient() { Imie = "Anna", Email = "anna_kowalska@domena.pl", DataUrodzenia = new DateTime(2003, 5, 7)},
    new Klient() { Imie = "Adam", Nazwisko = "Mickiewicz", Email = "adam_mickiewicz", DataUrodzenia = new DateTime(1999, 12, 31)},
    new Klient() { Imie = "Barbara", Nazwisko = "Nowacka", Email = "barbara_nowacka@domena.pl", DataUrodzenia = new DateTime(2020, 1, 12) },
    new Klient() {},
    new Klient() {DataUrodzenia = new DateTime(2050, 1, 1)},
    new Klient() { Imie = "Andrzej", Nazwisko = "Dudu", Email = "andrzej_dudu@domena.pl", DataUrodzenia = new DateTime(2004, 2, 29) }
};

        foreach (var klient in klienci)
        {
            Console.WriteLine("Sprawdzam klienta: ");
            Console.WriteLine(klient);
            var validationContext = new ValidationContext(klient);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(klient, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    Console.WriteLine("\tBłąd: " + validationResult.ErrorMessage + "\n\n");
                }
            }
            else
            {
                Console.WriteLine("\tPoprawna walidacja!\n");
            }
        }
    }
}