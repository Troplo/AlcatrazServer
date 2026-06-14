using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Alcatraz.DTO.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Alcatraz.GameServices.Helpers
{
    public class VersionedModelTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public VersionedModelTypeInfoResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Modifiers.Add(ModifyTypeInfo);
        }

        private void ModifyTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            foreach (var property in jsonTypeInfo.Properties)
            {
                if (property.AttributeProvider is PropertyInfo propertyInfo)
                {
                    var versionAttr = propertyInfo.GetCustomAttribute<ApiVersionSinceAttribute>();
                    if (versionAttr != null)
                    {
                        var requiredVersion = versionAttr.Version;
                        var existingShouldSerialize = property.ShouldSerialize;

                        property.ShouldSerialize = (obj, val) =>
                        {
                            if (existingShouldSerialize != null && !existingShouldSerialize(obj, val))
                                return false;

                            var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
                            if (httpContextAccessor?.HttpContext?.Items.TryGetValue("ApiVersion", out var versionObj) == true && versionObj is int currentVersion)
                            {
                                return currentVersion >= requiredVersion;
                            }

                            return true;
                        };
                    }
                }
            }
        }
    }
}
