using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting.Activation;

namespace JingleBellsGame
{
    public class ShapeParameters
    {
        public double X = 0;
        public double Y = 0;
        public SizeShapeParameters Size = SizeShapeParameters.Big;
        public TypeShapeParameters Type = TypeShapeParameters.Visible;
        public NameShapeParameters Name = NameShapeParameters.Welcome;
        public List<ShapeParameters> InvisibleBlocks = new List<ShapeParameters>();
    }
}