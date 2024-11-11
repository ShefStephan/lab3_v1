using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace TestProject1;

public class TurtleControllerTests : IClassFixture<WebApplicationFactory<Lab1_v2.Program>>
{
    private readonly HttpClient client;

    public TurtleControllerTests(WebApplicationFactory<Lab1_v2.Program> factory)
    {
        client = factory.CreateClient();
    }
                
    [Fact]
    public async Task ExecuteCommandPenDown_PostValidCommand_ReturnsOk()
    {
        var commandRequest = new { Command = "pendown", Parameter = "" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(commandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("penDown", responseBody);
    }
    
    [Fact]
    public async Task ExecuteCommandHistory_PostValidCommand_ReturnsOk()
    {
        var commandRequest = new { Command = "penup", Parameter = "" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(commandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("penUp", responseBody);
    }
    
    [Fact]
    public async Task ExecuteCommandColor_PostValidCommand_ReturnsOk()
    {
        var commandRequest = new { Command = "color", Parameter = "pink" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(commandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("pink", responseBody);
    }
    
    [Fact]
    public async Task ExecuteCommandPenUp_PostValidCommand_ReturnsOk()
    {
        var commandRequest = new { Command = "move", Parameter = "555" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(commandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("555", responseBody);
    }
    
    [Fact]
    public async Task ExecuteCommand_WithInvalidCommand_ReturnsNotFound()
    {
        var invalidCommandRequest = new { Command = "invalidCommand", Parameter = "" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidCommandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("Invalid command, or command doesn`t exist", responseBody);
    }
    
    
    [Fact]
    public async Task ExecuteCommand_WithInvalidArgument_ReturnsBadRequest()
    {
        var invalidCommandRequest = new { Command = "move", Parameter = "gggg" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidCommandRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Turtle", content);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("Invalid argument, please try again or check command list", responseBody);
    }
    
}