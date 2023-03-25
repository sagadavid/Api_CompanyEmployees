using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{//since our response needs to describe the root of the controller, we need a wrapper for our links
    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public List<T> Value { get; set; } = new List<T>();
        public LinkCollectionWrapper(){ }
        public LinkCollectionWrapper(List<T> value) => Value = value;
    }//For now, let's just assume we wrapped our links in another class for response representation purposes.
}
