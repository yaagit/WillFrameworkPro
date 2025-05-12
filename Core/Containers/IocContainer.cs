using System;
using System.Collections.Generic;
using System.Text;
using WillFrameworkPro.Core.Attributes.Types;

namespace WillFrameworkPro.Core.Containers
{
    // todo 考虑放弃对 View 的注册
    public class IocContainer : IDisposable
    {
        private readonly Dictionary<TypeEnum, Dictionary<Type, List<object>>> _identityIoc;
        
        public Dictionary<TypeEnum, Dictionary<Type, List<object>>> IdentityIoc
        {
            get => _identityIoc;
        }
        
        public IocContainer()
        {
            _identityIoc = new();
        }

        public void Add(TypeEnum typeEnum, object instance)
        {
            Type instanceType = instance.GetType();
            if (_identityIoc.TryGetValue(typeEnum, out Dictionary<Type, List<object>> value))
            {
                if (value.TryGetValue(instanceType, out List<object> objectList))
                {
                    objectList.Add(instance);
                }
                else
                {
                    value.Add(instanceType, new List<object>() {instance});
                }
            }
            else
            {
                value = new Dictionary<Type, List<object>>();
                List<object> objectList = new List<object>(){instance};
                value.Add(instanceType, objectList);
                _identityIoc.Add(typeEnum, value);
            }
        }

        public void Remove(TypeEnum typeEnum, object instance)
        {
            if (_identityIoc.TryGetValue(typeEnum, out Dictionary<Type, List<object>> value))
            {
                Type instanceType = instance.GetType();
                if (value.TryGetValue(instanceType, out List<object> objectList))
                {
                    objectList.Remove(instance);
                    if (objectList.Count == 0)
                    {
                        value.Remove(instanceType);
                    }
                    if (value.Count == 0)
                    {
                        _identityIoc.Remove(typeEnum);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("-------------------------- IOC Container --------------------------\n");
            foreach (KeyValuePair<TypeEnum, Dictionary<Type, List<object>>> outerKv in _identityIoc)
            {
                result.Append($"{outerKv.Key}:").Append("\n\t");
                foreach (KeyValuePair<Type, List<object>> innerKv in outerKv.Value)
                {
                    result.Append($"{innerKv.Key.FullName}").Append(" : ").Append(nameof(Object)).Append("s(" + innerKv.Value.Count + ")").Append("\n\t");
                }
                result.Append("\n");
            }
            result.Append("-------------------------------------------------------------------");
            return result.ToString();
        }

        public void Dispose()
        {
            _identityIoc.Clear();
        }
    }
}