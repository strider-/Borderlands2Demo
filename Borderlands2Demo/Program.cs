using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Borderlands2Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ammoReader = new Borderlands2AmmoReader();
            if(ammoReader.CanRead)
            {
                Console.WriteLine("Pistol:   " + ammoReader.GetAmmo(WeaponType.Pistol));
                Console.WriteLine("Grenades: " + ammoReader.GetAmmo(WeaponType.Grenade));
            }
            else
            {
                Console.WriteLine("Borderlands 2 isn't running!");
            }

            Console.ReadLine();

            /* Arduino specific code
            // wrapped up in a Task so as not to hog CPU priority
            Task t = new Task(() =>
            {
                Borderlands2AmmoReader ammoReader = new Borderlands2AmmoReader();
                using(var com = new SerialPort("COM12", 9600, Parity.None, 8, StopBits.One))
                {
                    com.Open();
                    com.Write("s");

                    float curVal = 0;
                    while(ammoReader.CanRead)
                    {
                        var ammo = Math.Min(ammoReader.GetAmmo(WeaponType.Pistol), 999f);
                        if(ammo != curVal)
                        {
                            curVal = ammo;
                            com.WriteLine(string.Format("n{0:000}", ammo));
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    com.Write("e");
                    com.Close();
                }
            });
            t.Start();

            Console.WriteLine("Ammo Reader running, press enter to quit.");
            Console.ReadLine();
           */
        }
    }
}
