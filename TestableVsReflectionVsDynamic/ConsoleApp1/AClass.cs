using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class AClass
    {
        protected IA A { get; set; }
        protected IB B { get; set; }
        protected IC C { get; set; }

        public int CallABC()
        {
            return A.A() + B.B() + C.C();
        }
        protected int CallAB()
        {
            return A.A() + B.B();
        }
        private int CallAC()
        {
            return A.A() + C.C();
        }
    }

    public interface IA
    {
        int A();
    }

    public class Ao : IA
    {
        public int A()
        {
            return 1;
        }
    }

    public interface IB
    {
        int B();
    }

    public class Bo : IB
    {
        public int B()
        {
            return 2;
        }
    }

    public interface IC
    {
        int C();
    }

    public class Co : IC
    {
        public int C()
        {
            return 3;
        }
    }
}
