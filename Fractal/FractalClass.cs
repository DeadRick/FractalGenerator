using System;

namespace Fractal
{
    public abstract class GeneralFractal 
    {

        public virtual string Name { get; }

        public GeneralFractal(string name) 
        {
            Name = name;
        }
    }


}
