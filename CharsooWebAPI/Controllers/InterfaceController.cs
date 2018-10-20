using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Interface")]
    public class InterfaceController : ApiController
    {
        // GET: api/Interface
        [ResponseType(typeof(string)), HttpGet, Route("GetControllers")]
        public IHttpActionResult GetInterface()
        {
            var controllerList =
                GetType()
                .Assembly
                .GetTypes()
                    .ToList();

            controllerList = controllerList
                .Where(t =>
                    t.IsSubclassOf(typeof(ApiController)) &&
                    t != GetType())
                .ToList();

            var controllersJArray = new JArray();

            foreach (var controller in controllerList)
            {
                var methods = controller.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public 
                );

                var methodsJArray = new JArray();

                foreach (var method in methods)
                {
                    var fmAtrib = method.GetCustomAttribute<FollowMachineAttribute>();

                    var methodJObject = new JObject
                    {
                        ["Name"] = GetMethodName(method)
                    };
                    if (fmAtrib != null)
                    {
                        methodJObject["Info"] = fmAtrib.Info;
                        methodJObject["Outputs"] = fmAtrib.Outputs;
                    }
                    methodsJArray.Add(methodJObject);

                }

                controllersJArray.Add(new JObject()
                {
                    ["Name"]=GetControllerName(controller),
                    ["Methods"]=methodsJArray
                });
            }


            return Ok(controllersJArray);
        }

        private string GetMethodName(MethodInfo method)
        {
            var routeAttribute = method.GetCustomAttribute<RouteAttribute>();

            var name = "";

            if (routeAttribute == null)
                name = method.Name+"(";
            else
                name = routeAttribute.Template.Split('/').First() + "(";

            var parameterInfos = method.GetParameters();

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var param = parameterInfos[i];
                var fromBodyAttribute = param.GetCustomAttribute<FromBodyAttribute>();

                name +=
                    (i == 0 ? "" : " ,") +
                    (fromBodyAttribute==null ? "":"[FromBody] ")+
                    param.ParameterType.Name + " " + param.Name;

            }

            return name + ")";
        }

        private string GetControllerName(Type controller)
        {
            var prefixAttribute = (RoutePrefixAttribute)controller.GetCustomAttribute(typeof(RoutePrefixAttribute));
            if (prefixAttribute == null)
                return controller.Name;
            else
            {
                return prefixAttribute.Prefix.Split('/').Last();
            }
        }
    }

    public class FollowMachineAttribute : Attribute
    {
        public string Info { get; set; }

        public string Outputs { get; set; }

        public FollowMachineAttribute(string info, string outputs = "")
        {
            Info = info;
            Outputs = outputs;
        }


    }
}
