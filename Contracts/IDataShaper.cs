using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IDataShaper<T>
    //expandoObject's members can be dynamically added and removed at runtime.
    {
        //The IDataShaper defines two methods that should be implemented one for the single
        //entity and one for the collection of entities.Both are named ShapeData ,
        //but they have different signatures.

        IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString);

        Entity ShapeData(T entity, string fieldsString);



    }
}
