using System;
using UnityEngine;

namespace CustomAttributes
{
    public enum Relation
    {
        Equal,
        NotEqual,
        GTE,
        GT,
        LTE,
        LT,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public string source = "";
        public object value = null;
        public Relation evaluationMethod = Relation.Equal;

        public ShowIfAttribute(string source, Relation comparison, object value)
        {
            this.source = source;
            this.value = value;
            this.evaluationMethod = comparison;
        }
    }
}