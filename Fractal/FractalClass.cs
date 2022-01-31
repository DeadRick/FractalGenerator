using System;

namespace Fractal
{
    public abstract class GeneralFractal 
    {
        // Название фрактала.
        public virtual string Name { get; }

        public GeneralFractal(string name) 
        {
            Name = name;
        }
    }


}
