using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Options
{

    /// <summary>
    /// full name class : Options In Online Managing Of Test Tensile Machine
    /// </summary>
    public class OptionsInOnlineManagingOfTTM
    {
        /// <summary>
        /// maksimalna dozvoljena vrednost promene napona 
        /// </summary>
        public static double Rmax = 10;
        /// <summary>
        /// minimalna dozvoljena vrednost promene napona 
        /// </summary>
        public static double Rmin = 1;
        /// <summary>
        ///  maksimalna dozvoljena vrednost promene izduzenja u rangu 2
        /// </summary>
        public static double eR2 = 10;
        /// <summary>
        /// maksimalna dozvoljena vrednost promene izduzenja u rangu 4
        /// </summary>
        public static double eR4 = 10;
    }
}
