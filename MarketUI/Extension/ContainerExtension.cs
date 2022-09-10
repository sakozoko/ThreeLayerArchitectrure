using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace MarketUI.Extension
{
    public static class ContainerExtension
    {
        public static T[] ResolveAll<T>(this IContainer self)
        {
            return self.Resolve<IEnumerable<T>>().ToArray();
        }

        public static object[] ResolveAll(this IContainer self, Type type)
        {
            var enumerableOfType = typeof(IEnumerable<>).MakeGenericType(type);
            return (object[])self.ResolveService(new TypedService(enumerableOfType));
        }
    }
}