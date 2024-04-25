using Swashbuckle.AspNetCore.SwaggerGen;

namespace AssemblyMaster.Utilities;

// Definição do Schema Filter
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(Microsoft.OpenApi.Models.OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            var enumType = context.Type;
            foreach (var name in Enum.GetNames(enumType))
            {
                schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name));
            }
            schema.Type = "string";
        }
    }
}