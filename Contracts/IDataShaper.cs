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

        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);

        ShapedEntity ShapeData(T entity, string fieldsString);
        //notice ... expandoObject-> Entity-> ShapedEntity
        //we have to modify the IDataShaper interface and the DataShaper class by replacing
        //all Entity usage with ShapedEntity .


    }
}
