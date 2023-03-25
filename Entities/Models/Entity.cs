using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using Entities.LinkModels;
//xml serilization problem... explanations at the end
namespace Entities.Models
{
    public class Entity : DynamicObject, IXmlSerializable, IDictionary<string, object>
    {
        private readonly string _root = "Entity";
        private readonly IDictionary<string, object> _expando;
        /* just modify the IDataShaper interface and the DataShaper class by using the Entity type 
         * instead of the ExpandoObject type. Also, you have to do the same thing for the IEmployeeService
         * interface and the EmployeeService class. 
         * Again, you can check our implementation if you have any problems. */
        public Entity()
        {
            _expando = new ExpandoObject();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_expando.TryGetValue(binder.Name, out object value))
            {
                result = value;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _expando[binder.Name] = value;

            return true;
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(_root);

            while (!reader.Name.Equals(_root))
            {
                string typeContent;
                Type underlyingType;
                var name = reader.Name;

                reader.MoveToAttribute("type");
                typeContent = reader.ReadContentAsString();
                underlyingType = Type.GetType(typeContent);
                reader.MoveToContent();
                _expando[name] = reader.ReadElementContentAs(underlyingType, null);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in _expando.Keys)
            {
                var value = _expando[key];
                WriteLinksToXml(key, value, writer);
            }
        }

        public void Add(string key, object value)
        {
            _expando.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _expando.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _expando.Keys; }
        }

        public bool Remove(string key)
        {
            return _expando.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _expando.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _expando.Values; }
        }

        public object this[string key]
        {
            get
            {
                return _expando[key];
            }
            set
            {
                _expando[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _expando.Add(item);
        }

        public void Clear()
        {
            _expando.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _expando.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _expando.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _expando.Count; }
        }

        public bool IsReadOnly
        {
            get { return _expando.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _expando.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _expando.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Since our response will contain links too, we need to extend the XML serialization rules
        //so that our XML response returns the properly formatted links.
        private void WriteLinksToXml(string key, object value, XmlWriter writer)
        {
            writer.WriteStartElement(key);
            if (value.GetType() == typeof(List<Link>))
            {//So, we check if the type is List<Link> . If it is, we iterate through all the links and
             //call the method recursively for each of the properties: href, method, and rel.
                foreach (var val in value as List<Link>)
                {
                    writer.WriteStartElement(nameof(Link));
                    WriteLinksToXml(nameof(val.Href), val.Href, writer);
                    WriteLinksToXml(nameof(val.Method), val.Method, writer);
                    WriteLinksToXml(nameof(val.Rel), val.Rel, writer);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteString(value.ToString());
            }
            writer.WriteEndElement();
        }
    }
}

/* https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?pageNumber=1&pageSize=3&minAge=26&maxAge=34&searchTerm=ae&orderBy=name desc&fields=name,position 
 * Let’s send the same request one more time, but this time with the

different accept header (text/xml):
It works — but it looks pretty ugly and unreadable. But that’s how the

XmlDataContractSerializerOutputFormatter serializes our ExpandoObject by default.

We can fix that, but the logic is out of the scope of this book. Of course, we have implemented the solution in our source code. So, if you want, you can use it in your project.

All you have to do is to create the Entity class and copy the content from our Entity class that resides in the Entities/Models folder.

After that, just modify the IDataShaper interface and the DataShaper class by using the Entity type instead of the ExpandoObject type. Also, you have to do the same thing for the IEmployeeService interface and the EmployeeService class. Again, you can check our implementation if you have any problems.

After all those changes, once we send the same request, we are going to see a much better result:
If XML serialization is not important to you, you can keep using ExpandoObject — but if you want a nicely formatted XML response, this is the way to go.
 */