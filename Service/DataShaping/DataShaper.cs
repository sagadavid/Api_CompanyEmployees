using Contracts;
using System.Dynamic;
using System.Reflection;

namespace Service.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class

    {

        public PropertyInfo[] Properties { get; set; } //access to property metada..an array of
                                                        //PropertyInfo’s that we’re going to pull out of the
                                                        //input type, whatever it is — Company or Employee in our case

        public DataShaper()

        {

            Properties = typeof(T).GetProperties
                (BindingFlags.Public | BindingFlags.Instance);

        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)

        {
            var requiredProperties = GetRequiredProperties//to parse the input string that contains the fields we want to fetch.
                (fieldsString);

            return FetchData(entities, requiredProperties);

        }

        public ExpandoObject ShapeData(T entity, string fieldsString)

        {

            var requiredProperties = GetRequiredProperties(fieldsString);

            return FetchDataForEntity(entity, requiredProperties);

        }
        //The GetRequiredProperties method does the magic. It parses the input string and returns just
        //the properties we need to return to the controller:
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)

        {

            var requiredProperties = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(fieldsString))

            {

                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)

                {

                    var property = Properties

                    .FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property == null)

                        continue;

                    requiredProperties.Add(property);

                }

            }

            else

            {

                requiredProperties = Properties.ToList();

            }

            return requiredProperties;

        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities,

        IEnumerable<PropertyInfo> requiredProperties)

        {

            var shapedData = new List<ExpandoObject>();

            foreach (var entity in entities)

            {

                var shapedObject = FetchDataForEntity(entity, requiredProperties);

                shapedData.Add(shapedObject);

            }

            return shapedData;

        }
        //FetchData and FetchDataForEntity are the private methods to extract the values from these required properties we’ve prepared.
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo>requiredProperties)

        {//ExpandoObject implements IDictionary<string,object> , so we can use the TryAdd method to add our
         //property using its name as a key and the value as a value for the dictionary.

            var shapedObject = new ExpandoObject();
            foreach (var property in requiredProperties)

            {

                var objectPropertyValue = property.GetValue(entity);

                shapedObject.TryAdd(property.Name, objectPropertyValue);

            }

            return shapedObject;

        }
        //let’s register the DataShaper class in the IServiceCollection in the Program class
    }
}
