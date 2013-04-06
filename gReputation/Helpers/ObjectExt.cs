using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace gReputation.Helpers
{
    public static class ObjectExt
    {
        /// <summary>
        /// This overcomes the problem with dynamic objects having all properties internal so they can't be passed to the MVC view
        ///     which is in a different assembly compiled at runtime
        /// </summary>
        /// <param name="anonymousObject"></param>
        /// <returns></returns>
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }

        public static T1 CopyFrom<T1, T2>(this T1 obj, T2 otherObject)
            where T1 : class
            where T2 : class
        {
            PropertyInfo[] srcFields = otherObject.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            PropertyInfo[] destFields = obj.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var property in srcFields) {
                //try {

                    var dest = destFields.FirstOrDefault(x => x.Name == property.Name && x.GetSetMethod() != null);
                    if (dest != null) {
                        if (dest.PropertyType == property.PropertyType)
                            dest.SetValue(obj, property.GetValue(otherObject, null), null);
                    }
                //} catch (Exception ex) {
                //    // TODO: logging
                //}
            }

            return obj;
        }
    }
}
