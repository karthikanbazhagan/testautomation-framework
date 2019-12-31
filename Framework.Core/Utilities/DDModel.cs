namespace Framework.Core.Utilities
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
        
    public class DDModel : DynamicObject
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        /// <summary>
        /// Returns the number of parameters
        /// </summary>
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }
        
        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

        /// <summary>
        /// Adds a parameter to the class and assigns it the given value
        /// </summary>
        /// 
        /// <param name="name">
        /// Name of the parameter to be created
        /// </param>
        /// 
        /// <param name="value">
        /// Value to be assigned for the parameter
        /// </param>
        public void AddMember(string name, object value)
        {
            dictionary[name.ToLower()] = value;
        }

        /// <summary>
        /// Returns the value of the requested parameter
        /// </summary>
        /// 
        /// <param name="name">
        /// Parameter name
        /// </param>
        /// 
        /// <returns>
        /// Returns the value of the parameter
        /// </returns>
        public object GetMember(string name)
        {
            return dictionary[name.ToLower()];
        }

        /// <summary>
        /// Returns the collection parameters in the class
        /// </summary>
        public List<string> Parameters
        {
            get
            {
                return dictionary.Keys.ToList();
            }
        }

        /// <summary>
        /// Checks if the given parameter is present in the class
        /// </summary>
        /// 
        /// <param name="column">
        /// The name of the parameter to be checked for
        /// </param>
        /// 
        /// <returns>
        /// Returns true, if the parameter is present in the class; false, otherwise.
        /// </returns>
        public bool Contains(string column)
        {
            return dictionary.ContainsKey(column.ToLower());
        }

    }
}
