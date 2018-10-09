using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace UnitTestWrapperProject
{
    public class ObjectWrapper : DynamicObject
    {
        object _wrapped; //用于存储被包装的对象

        static BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Static | BindingFlags.Public; //查找所有实例或静态的类成员

        public ObjectWrapper(object o)
        {
            _wrapped = o;
        }

        //覆盖原有的调用成员方法，改用反射来查找成员方法
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var types = args.Select(a => a.GetType());

            var method = _wrapped.GetType().GetMethod(binder.Name, flags, null, types.ToArray(), null);

            if (method != null)
            {
                result = method.Invoke(_wrapped, args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        //覆盖默认的获取成员方法，改用反射来查找成员
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            //先查找属性
            var prop = _wrapped.GetType().GetProperty(binder.Name, flags);
            if (prop != null)
            {
                result = prop.GetValue(_wrapped);
                return true;
            }

            //如果通过查找属性的方式没有找到，则按照字段来查找
            var fld = _wrapped.GetType().GetField(binder.Name, flags);
            if (fld != null)
            {
                result = fld.GetValue(_wrapped);
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        //覆盖默认的获取成员方法，改用反射来查找成员
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            //先查找属性
            var prop = _wrapped.GetType().GetProperty(binder.Name, flags);
            if (prop != null)
            {
                prop.SetValue(_wrapped, value, null);
                return true;
            }

            //如果通过查找属性的方式没有找到，则按照字段来查找
            var fld = _wrapped.GetType().GetField(binder.Name, flags);
            if (fld != null)
            {
                fld.SetValue(_wrapped, value);
                return true;
            }

            return base.TrySetMember(binder, value);
        }
    }

}
