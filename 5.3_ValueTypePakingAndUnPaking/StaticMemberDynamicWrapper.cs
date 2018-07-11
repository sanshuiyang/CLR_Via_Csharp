using System;
using System.Collections.Generic;
using System.Text;
using System.Dynamic;
using System.Reflection;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace _5._3_ValueTypePakingAndUnPaking
{
    class StaticMemberDynamicWrapper : DynamicObject
    {
        private readonly TypeInfo m_type;
        public StaticMemberDynamicWrapper(Type type)
        {
            m_type = type.GetTypeInfo();
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return m_type.DeclaredMembers.Select(mi => mi.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var field = FindFiled(binder.Name);
            if (field != null) { result = field.GetValue(null);return true; }

            var prop = FindProperty(binder.Name, true);
            if (prop != null) { result = prop.GetValue(null, null);return true; }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var field = FindFiled(binder.Name);
            if (field != null) { field.SetValue(null, value);return true; }

            var prop = FindProperty(binder.Name, false);
            if (prop != null)
            {
                prop.SetValue(null, value, null);
                return true;
            }
            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            MethodInfo method = FindMethod(binder.Name, args.Select(c => c.GetType()).ToArray());
            if (method == null) { result = null;return false; }
            result = method.Invoke(null, args);
            return true;
        }

        private MethodInfo FindMethod(string name,Type[] parameTypes)
        {
            return m_type.DeclaredMethods.FirstOrDefault(mi => mi.IsPublic && mi.IsStatic
            && mi.Name == name && ParametersMatch(mi.GetParameters(), parameTypes));
        }

        private Boolean ParametersMatch(ParameterInfo[] parameters, Type[] parameTypers)
        {
            if (parameters.Length != parameTypers.Length) return false;
            for(Int32 i = 0; i < parameTypers.Length; i++)
            {
                if (parameters[i].ParameterType != parameTypers[i]) return false;
            }
            return true;
        }

        private FieldInfo FindFiled(string name)
        {
            return m_type.DeclaredFields.FirstOrDefault(fi => fi.IsPublic && fi.IsStatic && fi.Name == name);
        }

        private PropertyInfo FindProperty(string name, Boolean get_)
        {
            if (get_)
                return m_type.DeclaredProperties.FirstOrDefault(pi => pi.Name == name && pi.GetMethod != null &&
                pi.GetMethod.IsPublic && pi.GetMethod.IsStatic);

            return m_type.DeclaredProperties.FirstOrDefault(pi => pi.Name == name && pi.SetMethod != null &&
                pi.GetMethod.IsPublic && pi.GetMethod.IsStatic);
        }
    }

    class Program3
    {
        static void Main(string[] args)
        {
            dynamic stringType = new StaticMemberDynamicWrapper(typeof(String));
            var r = stringType.Concat("A", "B");
            Console.WriteLine(r);
            Console.ReadKey(true);
        }
    }
}
