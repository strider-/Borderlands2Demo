using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Borderlands2Demo
{
    public enum WeaponType
    {
        AssaultRifle = 0x0188,
        Shotgun = 0x018c,
        Grenade = 0x0190,
        SMG = 0x0194,
        Pistol = 0x0198,
        RocketLauncher = 0x019c,
        SniperRifle = 0x01a0
    }

    public class Borderlands2AmmoReader
    {
        private const int BASE_OFFSET = 0x015EB138;

        private int _baseAddress;
        private readonly Process _bl2;
        private Dictionary<WeaponType, int> _finalLocations;

        public Borderlands2AmmoReader()
        {
            _bl2 = Process.GetProcessesByName("Borderlands2").FirstOrDefault();
            _finalLocations = new Dictionary<WeaponType, int>();

            if(_bl2 != null)
            {
                _baseAddress = (int)_bl2.MainModule.BaseAddress;
            }
        }

        public float GetAmmo(WeaponType offset)
        {
            // proc not running?
            if(CanRead && _baseAddress > 0)
            {
                return 0f;
            }

            var bytesRead = 0;

            // yo dawg I heard you like pointers so I'mma point to these pointers that
            // point to pointers, get the point(ers)?
            if(!_finalLocations.ContainsKey(offset))
            {
                byte[] p1a = _bl2.ReadMemory(_baseAddress + BASE_OFFSET, 4, out bytesRead);
                int p1v = BitConverter.ToInt32(p1a, 0);

                byte[] p2a = _bl2.ReadMemory(p1v + 0x2c, 4, out bytesRead);
                int p2v = BitConverter.ToInt32(p2a, 0);

                byte[] p3a = _bl2.ReadMemory(p2v + (int)offset, 4, out bytesRead);
                int p3v = BitConverter.ToInt32(p3a, 0);

                // lets cache the actual ammo location so we're not doing this every time we check for ammo
                _finalLocations[offset] = p3v;
            }

            byte[] ammo = _bl2.ReadMemory(_finalLocations[offset] + 0x6c, 4, out bytesRead);
            return BitConverter.ToSingle(ammo, 0);
        }

        // If we've got a valid process that hasn't stopped running, we can kill it..err read from it
        public bool CanRead
        {
            get
            {
                return _bl2 != null ? !_bl2.HasExited : false;
            }
        }
    }
}
