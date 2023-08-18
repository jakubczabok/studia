using System.Collections;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Transactions;

internal class Program
{

    //lab08 zad01

    public class Osoba
    {
        string imie;
        string nazwisko;
        int? wiek;

        public string this[int index]
        {
            get 
            { 
                if (index == 0)
                        return imie; 
                if (index==1)
                    return nazwisko;
                if (index == 2 && wiek != null)
                    return Convert.ToString(wiek);
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Osoba(string imie, string nazwisko, int? wiek)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.wiek = wiek;
        }

        public Osoba(string imie, string nazwisko):this(imie,nazwisko,null)
        {

        }

        public override string ToString()
        {
            return $"{imie} {nazwisko} {wiek}";
        }

        public static explicit operator string(Osoba osoba)
        {
            return osoba.ToString();
        }

        public static implicit operator Osoba(string str)
        {
            string[] wyrazy=str.Split(" ");
            if (wyrazy.Length==3)
            {
                Osoba nowaOsoba = new Osoba(wyrazy[0], wyrazy[1], Convert.ToInt16(wyrazy[2]));
                return nowaOsoba;
            }
            if (wyrazy.Length==2)
            {
                Osoba nowaOsoba = new Osoba(wyrazy[0], wyrazy[1]);
                return nowaOsoba;
            }
            else
            {
                throw new ArgumentException("Niepoprawny format imienia, nazwiska i wieku. Oczekiwany format: 'Imię Nazwisko Wiek");
            }
        }
    }

    public class Osoby : IEnumerable
    {
        ListaNaSterydach<Osoba> listaOsob = new ListaNaSterydach<Osoba>();

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

        public class OsobyEnumerator : IEnumerator
        {
            int currentIndex;
            List<Osoba> listaOsob=new List<Osoba> ();
            public object Current
            {
                get { return currentIndex; }
                
            }



            public bool MoveNext()
            {
                currentIndex++;
                return (currentIndex < listaOsob.Count());
            }

            public void Reset()
            {
                currentIndex = -1;
            }
            public OsobyEnumerator(List<Osoba> listaOsob)
            {
                this.listaOsob = listaOsob;
            }
        }
    }

    public class ListaNaSterydach<T> : List<T>
    {
        public override string ToString()
        {
            IEnumerator e = GetEnumerator();
            e.Reset();
            StringBuilder sb = new StringBuilder(2000);
            while (e.MoveNext())
            {
                sb.AppendLine(e.Current.ToString());
            }
            return sb.ToString();
        }
    }

    //lab08 zad02

    public class EventArgsZmianyTemperatury : EventArgs
    {
        int nowaTemperatura;
        public int NowaTemperatura
        {
            get { return NowaTemperatura; }
            set { nowaTemperatura = value; }
        }

        public EventArgsZmianyTemperatury(int nowaTemperatura)
        {
            this.nowaTemperatura = nowaTemperatura;
        }
    }

    public delegate void DelegatZmianyTemperatury(object sender,EventArgsZmianyTemperatury e);

    public class ObslugaZmianyTemperatury
    {
        string nazwaObslugi;
        public ObslugaZmianyTemperatury(string nazwaObslugi)
        {
            this.nazwaObslugi = nazwaObslugi;
        }

        public void ObsluzZmianeTemperatury(object sender, EventArgsZmianyTemperatury e)
        {
            SensorTemperatury sensor = sender as SensorTemperatury;
            Console.WriteLine($"{nazwaObslugi} odnotowała zmianę temperatury z czujnika {sensor.IdSensora}: {e.NowaTemperatura} stopni");
        }

       

    }

    public class SensorTemperatury
    {
        int aktualnaTemperatura;
        byte idSensora;

        public int AktualnaTemperatura
        {
            get { return AktualnaTemperatura; }
            set { AktualnaTemperatura = value;
                OnZmianaTemperatury(new EventArgsZmianyTemperatury(value)); }   
        }

        public byte IdSensora
        {
            get { return IdSensora; }
        }

        public void OnZmianaTemperatury(EventArgsZmianyTemperatury e)
        {
            if (ZmianaTemperatury!=null)
            {
                ZmianaTemperatury(this, e);
            }
        }

        public SensorTemperatury(byte idSensora)
        {
            this.idSensora = idSensora;
        }

        public event DelegatZmianyTemperatury ZmianaTemperatury;
    }

    private static void Main(string[] args)
    {
        SensorTemperatury sensorTemperatury = new SensorTemperatury(23);

        // Tworzenie obiektu obsługującego zdarzenie zmiany temperatury
        ObslugaZmianyTemperatury obslugaZmianyTemperatury1 = new ObslugaZmianyTemperatury("Obsługa 1");
        ObslugaZmianyTemperatury obslugaZmianyTemperatury2 = new ObslugaZmianyTemperatury("Obsługa 2");

        // Dodawanie metody obsługującej zdarzenie do delegata zdarzenia
        sensorTemperatury.ZmianaTemperatury += obslugaZmianyTemperatury1.ObsluzZmianeTemperatury;
        sensorTemperatury.ZmianaTemperatury += obslugaZmianyTemperatury2.ObsluzZmianeTemperatury;

        // Symulowanie zmiany temperatury
        sensorTemperatury.AktualnaTemperatura = 25;
        sensorTemperatury.AktualnaTemperatura = 30;
        sensorTemperatury.AktualnaTemperatura = 22;
    }
} 