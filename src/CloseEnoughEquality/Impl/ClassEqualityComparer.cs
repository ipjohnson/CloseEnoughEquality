using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloseEnoughEquality.Impl
{
    public class ClassEqualityComparer : IEqualityWrapper
    {
        private static MethodInfo _getGenericDictionaryValuesMethod;
        private static MethodInfo _getGenericReadOnlyDictionaryValues;

        static ClassEqualityComparer()
        {
            _getGenericDictionaryValuesMethod = typeof(ClassEqualityComparer).GetRuntimeMethods().First(m => m.Name == "GetGenericDictionaryValues");
            _getGenericReadOnlyDictionaryValues = typeof(ClassEqualityComparer).GetRuntimeMethods().First(m => m.Name == "GetGenericReadOnlyDictionaryValues");
        }

        private ICloseEnoughConfiguration _configuration;
        private List<object> _visitedObjects = new List<object>();

        public bool GeneratesDiscrepancy { get { return true; } }

        public ClassEqualityComparer(ICloseEnoughConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Applies(IPropertyInfo property)
        {
            return true;
        }

        public bool Equals(object left, object right, IPropertyInfo property)
        {
            if (left == null)
            {
                return right == null;
            }
            else if (right == null)
            {
                return false;
            }

            for(int i = 0; i < _visitedObjects.Count; i++)
            {
                if(ReferenceEquals( _visitedObjects[i], left))
                {
                    return true;
                }
            }

            _visitedObjects.Add(left);

            if (left.GetType() == right.GetType() && _configuration.UseCustomEquals(property))
            {
                MethodInfo equalsMethod = null;

                foreach (var method in left.GetType().GetRuntimeMethods())
                {
                    if (!method.IsStatic && method.IsPublic && method.Name == "Equals")
                    {
                        var parameters = method.GetParameters();

                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object))
                        {
                            equalsMethod = method;
                        }
                    }
                }

                if (equalsMethod != null &&
                    equalsMethod.DeclaringType != typeof(object) &&
                   !equalsMethod.DeclaringType.IsAnonymousType())
                {
                    return (bool)equalsMethod.Invoke(left, new object[] { right });
                }
            }

            if (IsDictionaryType(left.GetType()))
            {
                List<IPropertyInfo> leftList = GetDictionaryValues(left);
                List<IPropertyInfo> rightList = null;

                if (IsDictionaryType(right.GetType()))
                {
                    rightList = GetDictionaryValues(left);
                }
                else if (IsEnumerableType(right.GetType()))
                {
                    return false;
                }
                else
                {
                    rightList = GetProperties(right);
                }

                return ComparePropertyLists(leftList, rightList, property);
            }
            else if (IsDictionaryType(right.GetType()))
            {
                var leftList = GetProperties(left);
                var rightList = GetDictionaryValues(right);

                return ComparePropertyLists(leftList, rightList, property);
            }

            if (IsEnumerableType(left.GetType()))
            {
                if (!IsEnumerableType(right.GetType()))
                {
                    // don't compare object to list makes no sense
                    return false;
                }

                return CompareEnumerables(left, right, property);
            }
            else if (IsEnumerableType(right.GetType()))
            {
                // don't compare object to list makes no sense
                return false;
            }
            else
            {
                var leftPropertyList = GetProperties(left);
                var rightPropertyList = GetProperties(right);

                return ComparePropertyLists(leftPropertyList, rightPropertyList, property);
            }
        }

        private bool ComparePropertyLists(List<IPropertyInfo> leftPropertyList, List<IPropertyInfo> rightPropertyList, IPropertyInfo property)
        {
            bool returnValue = true;

            List<IPropertyInfo> processed = null;
            bool ignoreMissing = _configuration.IgnoreUnmatchedProperties(property);

            if (!ignoreMissing)
            {
                processed = new List<IPropertyInfo>();
            }

            foreach (var leftProperty in leftPropertyList)
            {
                _configuration.PushCurrentPath(leftProperty.Name);

                var rightProperty = rightPropertyList.FirstOrDefault(p => p.Name == leftProperty.Name);

                if (rightProperty == null)
                {
                    if (!ignoreMissing &&  _configuration.GenerateDiscrepancy)
                    {
                        _configuration.AddMissingPropertyDiscrepancy(_configuration.CurrentPath);
                    }
                }
                else
                {
                    if (processed != null)
                    {
                        processed.Add(rightProperty);
                    }

                    var leftValue = leftProperty.GetValue();
                    var rightValue = rightProperty.GetValue();

                    if (leftValue != null)
                    {
                        if (rightValue != null)
                        {
                            var wrapper = _configuration.GetEqualityWrapper(leftValue.GetType(), leftProperty);

                            var localValue = wrapper.Equals(leftValue, rightValue, leftProperty);

                            if (!localValue)
                            {
                                returnValue = false;

                                if (!wrapper.GeneratesDiscrepancy && _configuration.GenerateDiscrepancy)
                                {
                                    _configuration.AddDiscrepancy(leftValue, rightValue);
                                }
                            }
                        }
                        else if (_configuration.GenerateDiscrepancy)
                        {
                            _configuration.AddDiscrepancy(leftValue, rightValue);
                        }
                    }
                    else if (rightValue != null && _configuration.GenerateDiscrepancy)
                    {
                        _configuration.AddDiscrepancy(leftValue, rightValue);
                    }
                }

                _configuration.PopCurrentPath();
            }

            if (processed != null && processed.Count != leftPropertyList.Count)
            {
                var newRightList = new List<IPropertyInfo>(rightPropertyList);

                foreach (var processedItem in processed)
                {
                    newRightList.RemoveAll(p => p.Name == processedItem.Name);
                }

                foreach (var right in newRightList)
                {
                    _configuration.AddExtraPropertyDiscrepancy(_configuration.CurrentPath + "." + right.Name);
                }
            }

            return returnValue;
        }

        private bool CompareEnumerables(object left, object right, IPropertyInfo property)
        {
            IReadOnlyList<object> leftReadOnly = left as IReadOnlyList<object>;
            List<object> rightList = right as List<object>;

            if (leftReadOnly == null)
            {
                var newLeft = new List<object>();

                foreach (var objectL in (IEnumerable)left)
                {
                    newLeft.Add(objectL);
                }

                leftReadOnly = newLeft;
            }

            if (rightList == null)
            {
                rightList = new List<object>();

                foreach (var objectR in (IEnumerable)right)
                {
                    rightList.Add(objectR);
                }
            }

            bool ignoreUnmatched = _configuration.IgnoreUnmatchedProperties(property);

            if (leftReadOnly.Count != rightList.Count && !ignoreUnmatched)
            {
                if (_configuration.GenerateDiscrepancy)
                {
                    _configuration.AddDiscrepancy(
                        new ListCountDiscrepancy(_configuration.CurrentPath,
                                             leftReadOnly.Count,
                                             rightList.Count));
                }

                return false;
            }
            
            bool returnValue = true;

            for (int i = 0; i < leftReadOnly.Count; i++)
            {
                var leftValue = leftReadOnly[i];
                _configuration.GenerateDiscrepancy = false;

                _configuration.PushCurrentPath(i.ToString());

                var leftProperty = new ListPropertyInfo(left.GetType(), i.ToString(), leftValue);
                var wrapper = _configuration.GetEqualityWrapper(leftValue.GetType(), leftProperty);
                bool found = false;

                for(int j = 0; j < rightList.Count; j++)
                {
                    var rightValue = rightList[j];

                    if (leftValue != null)
                    {
                        if (rightValue != null)
                        {                            
                            var localValue = wrapper.Equals(leftValue, rightValue, leftProperty);

                            if(localValue)
                            {
                                rightList.RemoveAt(j);
                                found = true;
                                break;                                
                            }
                        }
                    }
                    else if(rightValue == null)
                    {
                        found = true;
                        break;
                    }                
                }

                _configuration.GenerateDiscrepancy = true;

                if (!found && !ignoreUnmatched)
                {
                    returnValue = false;
                    _configuration.AddMissingPropertyDiscrepancy(_configuration.CurrentPath);
                }

                _configuration.PopCurrentPath();
            }

            return returnValue;
        }

        private bool IsDictionaryType(Type type)
        {
            return type.GetTypeInfo().
                        ImplementedInterfaces.Any(t => t.IsConstructedGenericType &&
                                                       (t.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                                                        t.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)));

        }

        private bool IsEnumerableType(Type type)
        {
            if(type.IsArray)
            {
                return true;
            }

            return type.GetTypeInfo().ImplementedInterfaces.Any(t => t.IsConstructedGenericType &&
                                                                     t.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private List<IPropertyInfo> GetProperties(object propertiesObject)
        {
            var returnList = new List<IPropertyInfo>();

            foreach (var property in propertiesObject.GetType().GetRuntimeProperties())
            {
                if (!property.CanRead)
                {
                    continue;
                }

                var getMethod = property.GetMethod;

                if (!getMethod.IsPublic || getMethod.IsStatic || getMethod.GetParameters().Length > 0)
                {
                    continue;
                }

                var wrapper = new PropertyInfoWrapper(property, propertiesObject);

                if (!_configuration.ShouldSkipProperty(wrapper))
                {
                    returnList.Add(wrapper);
                }
            }

            foreach(var field in propertiesObject.GetType().GetRuntimeFields())
            {
                if(!field.IsPublic || field.IsStatic)
                {
                    continue;
                }

                var wrapper = new FieldInfoWrapper(field, propertiesObject);
                
                if (!_configuration.ShouldSkipProperty(wrapper))
                {
                    returnList.Add(wrapper);
                }
            }

            return returnList;
        }

        private List<IPropertyInfo> GetDictionaryValues(object propertiesObject)
        {
            foreach (var interfaceP in propertiesObject.GetType().GetTypeInfo().ImplementedInterfaces)
            {
                if (interfaceP.IsConstructedGenericType)
                {
                    var generic = interfaceP.GetTypeInfo().GetGenericTypeDefinition();

                    if (generic == typeof(IDictionary<,>))
                    {
                        var closedMethod = _getGenericDictionaryValuesMethod.MakeGenericMethod(interfaceP.GenericTypeArguments);

                        return (List<IPropertyInfo>)closedMethod.Invoke(this, new object[] { propertiesObject });
                    }
                    else if (generic == typeof(IReadOnlyDictionary<,>))
                    {
                        var closedMethod = _getGenericReadOnlyDictionaryValues.MakeGenericMethod(interfaceP.GenericTypeArguments);

                        return (List<IPropertyInfo>)closedMethod.Invoke(this, new object[] { propertiesObject });
                    }
                }
            }

            return new List<IPropertyInfo>();
        }

        private List<IPropertyInfo> GetGenericDictionaryValues<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            List<IPropertyInfo> returnValue = new List<IPropertyInfo>();

            foreach (var entry in dictionary)
            {
                returnValue.Add(new DictionaryPropertyInfo<TKey, TValue>(dictionary, entry.Key, entry.Value));
            }

            return returnValue;
        }

        private List<IPropertyInfo> GetGenericReadOnlyDictionaryValues<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            List<IPropertyInfo> returnValue = new List<IPropertyInfo>();

            foreach (var entry in dictionary)
            {
                returnValue.Add(new ReadOnlyDictionaryPropertyInfo<TKey, TValue>(dictionary, entry.Key, entry.Value));
            }

            return returnValue;
        }
    }
}
