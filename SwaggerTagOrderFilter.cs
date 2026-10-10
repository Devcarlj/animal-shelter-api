using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AnimalShelterApi.Swagger;

public class SwaggerTagOrderFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {

        var orderedTags = new List<string> { "Auth", "Animals", "Adopters", "Adoptions", "FosterPlacements", "Reports" };

        swaggerDoc.Tags = swaggerDoc.Tags
            .OrderBy(t =>
            {
                var index = orderedTags.IndexOf(t.Name);
                return index == -1 ? int.MaxValue : index;
            })
            .ToList();
    }
}