using System.Collections.Generic;

namespace CleanCode.CommentsClassification.Engineering
{
    public static class Constants
    {
        // 3
        // clarification
        // unit conversion coeff - 1 feet = 304.8 mm
        public const double FtToMm = 304.8;
        
        // 4
        // clarification
        // instance parameter - dimension along Y axis
        public const string InstanceParamDim1= "Dim1";
        
        // 5
        // clarification
        // instance parameter - dimension along X axis
        public const string InstanceParamDim2 = "Dim2";
        
        public const string InstanceParamB = "B";
        public const string InstanceParamC = "C";

        public static readonly List<double> TableOfCutting = new List<double>
        {
            1170 / FtToMm, 1300 / FtToMm, 1460 / FtToMm, 1670 / FtToMm, 1950 / FtToMm, 2340 / FtToMm, 2925 / FtToMm,
            3900 / FtToMm, 4875 / FtToMm, 5850 / FtToMm, 6825 / FtToMm, 7800 / FtToMm, 8775 / FtToMm, 9750 / FtToMm
        };

        public const double ArmatureStep = 200 / FtToMm;
    }
}