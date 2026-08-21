using System.Text.Json;

namespace Prism.Application.Dtos;

public record UpdateAppProfileDataRequest(string Name, JsonElement Data); 
