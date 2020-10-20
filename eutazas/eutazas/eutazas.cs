using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace eutazas
{
	class eutazas
	{
		//Pascal-írást fogok használni a C# konvenciója szerint, ha a feladat nem kéri máshogy: Minden szó nagybetűvel, elválasztás nélkül

		static bool Kiertekelo(string[] Utas)
		{
			if (Utas[(int)Adatok.Tipus] == "JGY")
			{
				if (Convert.ToInt32(Utas[(int)Adatok.Ervenyes]) == 0)
				{
					return false;
				}
			}
			if (Convert.ToInt32(Utas[(int)Adatok.Ervenyes]) < 20190326)
			{
				return false;
			}
			return true;
		}
		static int NapokSzama(int e1, int h1, int n1, int e2, int h2, int n2)
		{
			h1 = (h1 + 9) % 12;
			e1 = e1 - h1 / 10;
			int d1 = 365 * e1 + e1 / 4 - e1 / 100 + e1 / 400 + (h1 * 306 + 5) / 10 + n1 - 1;
			h2 = (h2 + 9) % 12;
			e2 = e2 - h2 / 10;
			int d2 = 365 * e2 + e2 / 4 - e2 / 100 + e2 / 400 + (h2 * 306 + 5) / 10 + n2 - 1;
			return d2 - d1;
		}
		/// <summary>
		/// Szigorúan véve nem szükséges, de olvashatóbb a lekérés később a tömbben, még az indexelés is stimmelni is fog
		/// </summary>
		enum Adatok : int
		{
			Felszallo,
			Datum,
			Azon,
			Tipus,
			Ervenyes
		}
		static void Main(string[] args)
		{
			#region 1.feladat
			List<string[]> AdatLista = new List<string[]>(); // A List egy Dinamikus hosszúságú tömb, nem kell indexelni amikor írsz bele
			using (StreamReader sr = File.OpenText("utasadat.txt"))
			{
				string temp;
				while ((temp = sr.ReadLine()) != null) //itt a temp = sr... visszaad értéket, mint assignment expression, elég hasznos
				{
					AdatLista.Add(temp.Split(" "));
				}

			}
			#endregion
			#region 2.feladat
			Console.WriteLine("2. feladat:");
			Console.WriteLine("A buszra " + AdatLista.Count + " szeretett volna felszállni");
			#endregion
			#region 3.feladat
			int count = 0;
			foreach ( string[] Utas in AdatLista)
			{
				if (!Kiertekelo(Utas))
				{
					count++;
				}
			}
			Console.WriteLine("3.feladat");
			Console.WriteLine($"Ennyi utas nem szálhatott fel: {count}");
			#endregion
			#region 4.feladat
			string Tracker = AdatLista[0][(int)Adatok.Felszallo];
			int Counter = 0;
			int MaxCount = 0;
			string MaxTracker = "";
			foreach (string[] Utas in AdatLista)
			{
				if (Utas[(int)Adatok.Felszallo] == Tracker)
				{
					Counter++;
				}
				else
				{
					if (Counter > MaxCount)
					{
						MaxCount = Counter;
						MaxTracker = Tracker; 
					}
					Tracker = Utas[(int)Adatok.Felszallo]; Counter = 0;
				}
			}
			Console.WriteLine("4.feladat");
			Console.WriteLine($"A legtöbb utas ({MaxCount} fő) a {MaxTracker}. megállóban akart felszállni");
			#endregion
			#region 5.feladat

			int MaiDatum = 20190326; //2019-03-26
			int DisCount = 0; // Vicceske
			int FreeCount = 0;
			foreach (string[] Utas in AdatLista)
			{
				if (Convert.ToInt32(Utas[(int)Adatok.Datum].Substring(0,8)) >= MaiDatum)
				{
					if (Utas[(int)Adatok.Tipus] == "TAB" || Utas[(int)Adatok.Tipus] == "NYB")
					{
						DisCount++;
					}
					else if (Utas[(int)Adatok.Tipus] == "NYP" || Utas[(int)Adatok.Tipus] == "RVS" || Utas[(int)Adatok.Tipus] == "GYK")
					{
						FreeCount++;
					}
				}
			}
			Console.WriteLine("5.feladat"); 
			Console.WriteLine($"Az ingyenesen utazók száma: {FreeCount}");
			Console.WriteLine($"A kedvezményesen utazók száma: {DisCount}");
			#endregion
			#region 7.feladat
			int MaEv = 2019, MaHonap = 03, MaNap = 26;
			List<string> Figyelmeztetes = new List<string>();
			foreach (string[] Utas in AdatLista)
			{
				if (Utas[(int)Adatok.Ervenyes].Length > 2)
				{
					if (Kiertekelo(Utas) && 3 >= NapokSzama(MaEv, MaHonap, MaNap,
							Convert.ToInt32(Utas[(int)Adatok.Ervenyes].Substring(0, 4)), //iiiigen, pöttet ronda, ez van
							Convert.ToInt32(Utas[(int)Adatok.Ervenyes].Substring(4, 2)), // le kell bontani a paramétereket, mert a függvény ezt nem csinálja magától
							Convert.ToInt32(Utas[(int)Adatok.Ervenyes].Substring(6, 2))))
					{
						Figyelmeztetes.Add(Utas[(int)Adatok.Azon]+ " " + Utas[(int)Adatok.Datum]);
					}
				}
				
			}
			using (StreamWriter sw = new StreamWriter("figyelmeztetes.txt"))
			{
				foreach (string sor in Figyelmeztetes)
				{
					sw.WriteLine(sor);
				}
			}
			#endregion

		}

	}
}
