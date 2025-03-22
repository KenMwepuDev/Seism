using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seism.Logique.Model;

public class AlertData
{
    public DateTime DateEncodage { get; set; }
    public double Value { get; set; }

    public AlertData()
    {
    }

    public AlertData(AlertData dataCopy)
    {
        DateEncodage = dataCopy.DateEncodage;
        Value = dataCopy.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is AlertData data)
        {
            return DateEncodage == data.DateEncodage && Value == data.Value;
        }
        return false;
    }
}