using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.IdealFinish.PFSWrapper
{
    public enum State
    {
        Valid,
        NoReading,
        OpenCircuit,
        Low,
        High,
        CJ,
    }
    public static class DoubleExtension
    {
        public static  PFSReading ToReading(this double value)
        {
            return new PFSReading(value);
        }
    }

    public class PFSReading : IEquatable<PFSReading>
    {             
        public double Value { get; private set; }        
        public State State { get; private set; }


            public PFSReading(double value)
            {
                Value = value;
                State = State.Valid;
                if (Value < -300)
                {
                    if (EqualsValue(-3001.0))
                    {
                        State = State.OpenCircuit;
                    }
                    else if (EqualsValue(-3002.0))
                    {
                        State = State.Low;
                    }
                    else if (EqualsValue(-3003.0))
                    {
                        State = State.High;
                    }
                    else if (EqualsValue(-3004.0))
                    {
                        State = State.CJ;
                    }
                    else if (EqualsValue(-3005.0))
                    {
                        State = State.NoReading;
                    }
                    else
                    {
                        State = State.NoReading;
                    }
                }
            }

            private bool EqualsValue(double p)
            {
                return Math.Abs(Value - p) < 0.1;
            }

            public PFSReading(PFSWrapper.State state)
            {                
                this.State = state;
            }
            public override string ToString()
            {
                switch (State)
                {
                    case State.Valid:
                        return Value.ToString();
                    case State.OpenCircuit:
                        return "*OC";
                    case State.NoReading:
                        return "XX";
                    case State.Low:
                        return "*LO";
                    case State.High:
                        return "*HI";
                    default:
                        return State.ToString();
                }
            }
            public override bool Equals(object obj)
            {
                var other = obj as PFSReading;
                if (other != null)
                {
                    return Equals(other);
                }
                return false;
            }

            public bool Equals(PFSReading other)
            {
                bool isEqual = false ;
                if (other.State == State)
                {
                    
                    if (State == State.Valid)
                    {
                        isEqual = EqualsValue(other.Value);
                    }
                    else
                    {
                        isEqual = true;
                    }
                    
                }
                return isEqual;
            }
        
    }
}
