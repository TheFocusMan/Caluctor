using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaluctorSchoolProect
{
    static class MathHelper
    {
        [Description("π"), OutputFormat("{0}π")]
        public static double MultiplatPI(double num) => Math.PI * num;

        [Description("E"), OutputFormat("{0}e")]
        public static double MultiplatE(double num) => Math.E * num;

        [Description("%"), OutputFormat("{0}%")]
        public static double MultiplatPrecent(double num) => (1.0 / 100) * num;

        [Description("X^2"), OutputFormat("{0}^2")]
        public static double Pow2(double num) => Math.Pow(num,2);

        [Description("X^3"), OutputFormat("{0}^3")]
        public static double Pow3(double num) => Math.Pow(num, 3);

        [Description("n√X"), OutputFormat("{1}√{0}")]
        public static double RootByCustomNumer(double num, double n) => Math.Pow(num,1.0 / n);

        [Description("3√X"), OutputFormat("3√{0}")]
        public static double Root3(double num) => Math.Pow(num, 1.0 / 3);

        [Description("10^n"), OutputFormat("10 Pow {0}")]
        public static double TenPowerOF(double num) => Math.Pow(10, num);

        [Description("2^n"), OutputFormat("2 Pow {0}")]
        public static double TowPowerOF(double num) => Math.Pow(2, num);

        [Description("E^n"), OutputFormat("E Pow {0}")]
        public static double EPowerOF(double num) => Math.Pow(Math.E, num);

        [Description("ln"), OutputFormat("ln {0}")]
        public static double LnMath(double num) => Math.Log(num, Math.E);
    }
}
