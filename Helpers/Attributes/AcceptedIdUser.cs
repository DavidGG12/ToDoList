using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToDoList.Helpers.Interface;

namespace ToDoList.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AcceptedIdUser : Attribute, ICustomAttribute, IOperationFilter
    {
        public bool IsMandatory { get; }

        public AcceptedIdUser(bool isMandatory = false) 
        {
            IsMandatory = isMandatory;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            CustomAttribute acceptedIdUser = context.RequireAttribute<AcceptedIdUser>();

            if (!acceptedIdUser.ContainsAttribute)
                return;

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "idUser",
                In = ParameterLocation.Header,
                Required = acceptedIdUser.Mandatory,
                Schema = new OpenApiSchema() { Type = "string" }
            });
        }
    }
}
