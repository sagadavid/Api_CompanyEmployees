using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class LinkResponse//will help us with the response once we start with the HATEOAS implementation:
    {
        public bool HasLinks { get; set; }
        public List<Entity> ShapedEntities { get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }
        
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<Entity>();
            ShapedEntities = new List<Entity>();
            //With this class, we are going to know whether our response has links.
            //If it does, we are going to use the LinkedEntities property.
            //Otherwise, we are going to use the ShapedEntities property.
        }
    }
}
