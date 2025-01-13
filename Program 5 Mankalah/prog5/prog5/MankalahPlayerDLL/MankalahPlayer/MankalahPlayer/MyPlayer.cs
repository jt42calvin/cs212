using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankalah
{
    // rename me
    public class MyPlayer : Player // class must be public
    {
        public MyPlayer(Position pos, int maxTimePerMove) // constructor must match this signature
            : base(pos, "MyPlayer", maxTimePerMove) // choose a string other than "MyPlayer"
        {
        }

        // adapt all code from your player class into this

    }
}