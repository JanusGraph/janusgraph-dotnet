using Gremlin.Net.Process.Traversal;

namespace JanusGraph.Net
{
    internal class JanusGraphP : P
    {
        public JanusGraphP(string operatorName, object value, P other = null) : base(operatorName, value, other)
        {
        }
    }
}