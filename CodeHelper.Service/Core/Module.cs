using CodeHelper.Service.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeHelper.Service.Core
{
    class Module
    {

        public string Name { get; set; }
        public string FullName { get; set; }
        public Dictionary<string, string> Parameters { get; }
        public List<Function> Functions => GetType()
            .GetMethods()
            .Where(func => Attribute.GetCustomAttributes(func, typeof(VisibleAttribute)).Any())
            .Select(info => new Function()
            {
                Name = info.Name,
                Description = ((VisibleAttribute)Attribute.GetCustomAttribute(info, typeof(VisibleAttribute))).Description
            })
            .ToList();

        public Module() { }

        public Module(string name, string fullname)
        {
            Name = name;

            FullName = fullname;

            Parameters = new(
           File.ReadAllLines($"{FullName}/config.cfg")
          .Select(property =>
          {
             var pair = property.Split("=");
             return KeyValuePair.Create(pair[0], pair[1]);
          }));
        }


        public void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

    }
}
