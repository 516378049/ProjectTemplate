using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JR.NewTenancy.Models.Common
{
    class ExcelMapping
    {
    }
    public class ExcelField
    {
        public int Index { get; set; }
        public string CellField { get; set; }
        public string DBField { get; set; }
        public string CellReference { get; set; }
        //public string fType { get; set; }
        public FieldDataType fType { get; set; }
    }
    public class MappingKey
    {
        //Excel Title
        public string Key { get; set; }
        //Database field name for downloading
        public string Value { get; set; }
        public FieldDataType fType { get; set; }
        //public string AllowNull { get; set; }
        public bool CanNotNull { get; set; }
        public string MatchValues { get; set; }
        public string MaximumLength { get; set; }
        //Database Field Name for uploading
        public string DBField { get; set; }
        public bool IsCheck { get; set; }
        public string CallReference { get; set; }
        //Hason 2015.8.11
        public bool AdvancedCheck { get; set; }
        public string DBMatchValues { get; set; }
        //2016.6.15
        public string RegularExpression { get; set; }
        public string ExceptValue { get; set; }
        public NumberFormat NumberFormat { get; set; }
        public bool NumRangeCheck { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

    }
    public enum NumberFormat
    {
        Normal,
        Percentage
    }
    public enum FieldDataType
    {
        String,
        Date,
        Float,
        Int,
        Time,
        DateAndString
    }
}
