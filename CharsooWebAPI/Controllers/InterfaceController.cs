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
            return Ok(JArray.FromObject(
                ControllerList
                    .Select(coltroller => new
                    {
                        // Controller
                        Name = coltroller.Name,

                        Prefix = coltroller.GetCustomAttribute<RoutePrefixAttribute>()?.Prefix,

                        Methods = GetControllerMethods(coltroller)
                            .Select(method => new
                            {
                                // Method
                                Name = method.Name,

                                Prefix = method.GetCustomAttribute<RouteAttribute>()?.Template,

                                ConnectionMethod = method.GetCustomAttribute<HttpPostAttribute>() == null ? 
                                    ServerConnectionMethod.Get : 
                                    ServerConnectionMethod.Post,

                                Info = method.GetCustomAttribute<FollowMachineAttribute>()?.Info,

                                Outputs = method.GetCustomAttribute<FollowMachineAttribute>()?
                                    .Outputs
                                    .Split(new []{','},StringSplitOptions.RemoveEmptyEntries),

                                Parameters = method.GetParameters()
                                    .Select(paramInfo => new
                                    {
                                        // Parameter
                                        paramInfo.Name,
                                        TypeName = paramInfo.ParameterType.Name,
                                        FormBody = paramInfo.GetCustomAttribute<FromBodyAttribute>() != null,
                                    }),
                            }),
                    })));
        }

        private List<Type> ControllerList
        {
            get
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
                return controllerList;
            }
        }

        private static MethodInfo[] GetControllerMethods(Type controller)
        {
            return controller.GetMethods(
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly |
                BindingFlags.Public
            );
        }

        public enum ServerConnectionMethod
        {
            Get, Post
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
