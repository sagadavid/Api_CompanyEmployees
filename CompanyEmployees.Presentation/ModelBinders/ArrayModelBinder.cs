using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ModelBinders
{
    /*We are creating a model binder for the IEnumerable type. Therefore, we have to check if our parameter is the same type.
Next, we extract the value (a comma-separated string of GUIDs) with the ValueProvider.GetValue() expression. Because it is a type string, we just check whether it is null or empty. If it is, we return null as a result because we have a null check in our action in the controller. If it is not, we move on.
In the genericType variable, with the reflection help, we store the type the IEnumerable consists of. In our case, it is GUID. With the converter variable, we create a converter to a GUID type. As you can see, we didn’t just force the GUID type in this model binder; instead, we inspected what is the nested type of the IEnumerable parameter and
    then created a converter for that exact type, thus making this binder generic.
After that, we create an array of type object (objectArray ) that consist of all the GUID values we sent to the API and then create an array of GUID types (guidArray ), copy all the values from the objectArray to the guidArray , and assign it to the bindingContext .
    These are the required using directives:
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;
     */
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType) 
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            } 

            var providedValue = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ToString();
            if(string.IsNullOrEmpty(providedValue) ) 
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var genericType = bindingContext.ModelType
                .GetTypeInfo()
                .GenericTypeArguments[0];

            var converter = 
                TypeDescriptor
                .GetConverter(genericType);

            var objectArray = 
                providedValue.Split(new[] { "," },
                StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => converter.ConvertFromString(x.Trim()))
                    .ToArray();

            var guidArray = 
                Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(guidArray, 0);
            bindingContext.Model = guidArray;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;

            /*get collection depending on id array--after model binding above
             * postman get : https://localhost:7165/api/companies/collection/(3b2d4e75-7ecc-4630-6142-08db1a7c0f35,550057df-0631-439c-6143-08db1a7c0f35)
             response body: 
            [
    {
        "id": "3b2d4e75-7ecc-4630-6142-08db1a7c0f35",
        "name": "company collection 3",
        "fullAddress": "address 123 britania"
    },
    {
        "id": "550057df-0631-439c-6143-08db1a7c0f35",
        "name": "company collection 4",
        "fullAddress": "address 456 englandia"
    }
]
             */

        }
    }
}
