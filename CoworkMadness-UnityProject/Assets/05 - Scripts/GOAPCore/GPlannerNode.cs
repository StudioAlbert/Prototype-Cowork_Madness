using System.Collections.Generic;
using System.Linq;

namespace GOAPCore
{
    public class GPlannerNode
    {
        public GAction Action;
        public GPlannerNode ParentNode;
        public List<GState> Effects = new List<GState>();

        public float Cost => Action.Cost();
        
        private List<GPlannerNode> _childNodes;
        
        public GPlannerNode(GAction action, GPlannerNode parentNode, List<GState> effects)
        {
            Action = action;
            ParentNode = parentNode;
            Effects = new List<GState>(effects);

            _childNodes = new List<GPlannerNode>();
        }

        public void AddChild(GPlannerNode child)
        {
            _childNodes.Add(child);
        }

        public GPlannerNode GetCheapestChild()
        {
            return _childNodes.OrderBy(c => c.Cost).FirstOrDefault();
        }
        
    }
}
