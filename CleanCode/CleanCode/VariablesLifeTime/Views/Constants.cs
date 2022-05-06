using System;

namespace CleanCode.VariablesLifeTime.Views
{
    public class Constants
    {
        public static readonly Guid GuidFamilyInstanceWidth = new Guid();
        public static readonly Guid GuidFamilyInstanceLength = new Guid();
        public static readonly Guid GuidFamilySymbolDiameter = new Guid();

        public const string ConcreteClassPattern = @"B?В?[0-9]{1,2}"; // B15 - B60 (B-rus, B-eng)
    }
}