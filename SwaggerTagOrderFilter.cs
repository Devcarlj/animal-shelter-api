using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AnimalShelterApi.Swagger;

public class SwaggerTagOrderFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var orderedTags = new List<string> { "Auth", "Animals", "Adopters", "Adoptions", "FosterPlacements", "Reports" };

        // Only order tags that are present in the document
        if (swaggerDoc.Tags != null && swaggerDoc.Tags.Count > 0)
        {
            swaggerDoc.Tags = swaggerDoc.Tags
                .OrderBy(t =>
                {
                    var index = orderedTags.IndexOf(t.Name);
                    return index == -1 ? int.MaxValue : index;
                })
                .ToList();
        }

        // Also order the paths by their tags for consistency
        if (swaggerDoc.Paths != null && swaggerDoc.Paths.Count > 0)
        {
            var orderedPaths = new Dictionary<string, OpenApiPathItem>();

            // Group paths by their first tag
            var pathsByTag = new Dictionary<string, List<KeyValuePair<string, OpenApiPathItem>>>();

            foreach (var path in swaggerDoc.Paths)
            {
                var tag = path.Value.Operations?.FirstOrDefault().Value.Tags?.FirstOrDefault()?.Name ?? "Untagged";

                if (!pathsByTag.ContainsKey(tag))
                {
                    pathsByTag[tag] = new List<KeyValuePair<string, OpenApiPathItem>>();
                }

                pathsByTag[tag].Add(path);
            }

            // Rebuild paths in tag order
            foreach (var tagName in orderedTags)
            {
                if (pathsByTag.ContainsKey(tagName))
                {
                    foreach (var path in pathsByTag[tagName].OrderBy(p => p.Key))
                    {
                        orderedPaths[path.Key] = path.Value;
                    }
                }
            }

            // Add any remaining paths that weren't in the ordered tags list
            foreach (var tag in pathsByTag.Keys.Where(t => !orderedTags.Contains(t)).OrderBy(t => t))
            {
                foreach (var path in pathsByTag[tag].OrderBy(p => p.Key))
                {
                    orderedPaths[path.Key] = path.Value;
                }
            }

            // Clear existing paths and add them back in order
            swaggerDoc.Paths.Clear();
            foreach (var orderedPath in orderedPaths)
            {
                swaggerDoc.Paths[orderedPath.Key] = orderedPath.Value;
            }
        }
    }
}