using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Services
{
    public static class ServisFabrika
    {
        public static IObracunServis OdrediSmenu()
        {
            int sat = DateTime.Now.Hour;

            if (sat >= 8 && sat <= 12)
                return new PrepodneObracun();

            return new PoslepodneObracun();
        }
    }
}
