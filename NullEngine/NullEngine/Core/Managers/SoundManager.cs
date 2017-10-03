using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace NullEngine.Core.Managers
{
    class SoundManager
    {
        public static string getSoundDevices()
        {
            return Alc.GetString(IntPtr.Zero, AlcGetString.DefaultAllDevicesSpecifier);
        }

        public static string getDefaultSoundDevice()
        {
            return Alc.GetString(IntPtr.Zero, AlcGetString.DefaultDeviceSpecifier);
        }
    }
}
