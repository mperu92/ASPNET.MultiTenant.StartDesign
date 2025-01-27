using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace StartTemplateNew.Shared.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNestedRouteAttribute : Attribute, IRouteTemplateProvider
    {
        private readonly Type _controllerType;

        public ControllerNestedRouteAttribute(string template, Type controllerType)
        {
            Name = template;
            _controllerType = controllerType;
        }

        public ControllerNestedRouteAttribute(string template, Type controllerType, int order)
        {
            Name = template;
            _controllerType = controllerType;
            Order = order;
        }

        public string Template
        {
            get
            {
                // Look up the route from the parent type. This only goes up one level, but if the parent class also has a `NestedRouteAttribute`, then it should work recursively.
                Type? baseType = _controllerType.BaseType;

                IRouteTemplateProvider? baseTypeRouteAttr = baseType?.GetCustomAttributes().FirstOrDefault(a => a is IRouteTemplateProvider) as IRouteTemplateProvider;
                string? baseTypeRouteAttrTemplate = baseTypeRouteAttr?.Template;

                if (!string.IsNullOrEmpty(baseTypeRouteAttrTemplate))
                {
                    if (baseTypeRouteAttrTemplate.EndsWith('/'))
                        return Path.Join(baseTypeRouteAttrTemplate, Name);
                    else
                        return Path.Join(baseTypeRouteAttrTemplate, "/", Name);
                }

                return Path.Join(null, Name);
            }
        }

        public int? Order { get; }

        public string Name { get; }
    }
}
