using System;
using System.Text;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class ee
    {
        public static void a(SplashScreen f)
        {
            var r = new Random();
            f.lblCopyright.Text = Encoding.UTF8.GetString(
                Convert.FromBase64String(
                     new string[]{
                        "U2lnbiBtbyBuYSAndG8gPDM=", "U2FuYSBhbGwgbmFnLWVmZm9ydC4uLg==",
                        "WWllZWUsIHBhc2FkbyBuYSAneWFuIQ==", "U2VsZi1sb3ZlIG9ubHk="
                    }[r.Next(0, 100) % 4]
                )
            );
        }

        public static bool c()
        {
            var r = new Random();
            return r.Next(1, 101) <= 25;
        }
    }
}
