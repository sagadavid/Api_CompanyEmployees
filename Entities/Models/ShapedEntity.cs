using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* HATEOAS strongly relies on having the ids available to construct the links for the response. 
 * Data shaping, on the other hand, enables us to return only the fields we want. 
 * So, if we want only the name and age fields, the id field won’t be added. 
 * To solve that, we have to apply some changes. 
 * With this class, we expose the Entity and the Id property as well.*/
namespace Entities.Models
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new Entity();
        }
        public Guid Id { get; set; }
        public Entity Entity { get; set; }
    }
}
