using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToDoList.Helpers.Interface;

namespace ToDoList.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AcceptedIdTask : Attribute, ICustomAttribute, IOperationFilter
    {
        public bool IsMandatory { get; }

        public AcceptedIdTask(bool isMandatory = false) 
        { 
            IsMandatory = isMandatory;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            CustomAttribute acceptedIdTask = context.RequireAttribute<AcceptedIdTask>();

            if (!acceptedIdTask.ContainsAttribute)
                return;

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "idTask",
                In = ParameterLocation.Header,
                Required = acceptedIdTask.Mandatory,
                Schema = new OpenApiSchema() { Type = "string" }
            });
        }
    }
}
