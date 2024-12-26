using System.Collections.Concurrent;
using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Controllers;
using Synnovation.WebFramework.Core.Types;

namespace Synnovation.WebFramework.Demo.Controllers;

public class UsersApiController : ApiControllerBase
{
    private static readonly ConcurrentDictionary<int, User> _users = new();
    private static int _nextId = 1;

    // GET /api/users
    [HttpGet("/api/users")]
    public IActionResult GetAll()
    {
        return Ok(_users.Values);
    }

    // GET /api/users/{id}
    [HttpGet("/api/users/{id}")]
    public IActionResult GetById(int id)
    {
        return _users.TryGetValue(id, out var u)
            ? Ok(u)
            : NotFound(new { error = "User not found" });
    }

    // POST /api/users
    // JSON body => [FromBody] CreateUserDto body
    [HttpPost("/api/users")]
    public IActionResult Create([FromBody] CreateUserDto body)
    {
        if (string.IsNullOrWhiteSpace(body.Name))
        {
            return BadRequest(new { error = "Invalid user data" });
        }

        var newUser = new User { Id = _nextId++, Name = body.Name };
        _users[newUser.Id] = newUser;
        return Created(newUser);
    }

    // PUT /api/users/{id}
    [HttpPut("/api/users/{id}")]
    public IActionResult Update(int id)
    {
        var body = ParseJsonBody<CreateUserDto>();
        if (body == null || string.IsNullOrEmpty(body.Name))
        {
            return BadRequest(new { error = "Invalid user data" });
        }

        if (!_users.ContainsKey(id)) return NotFound(new { error = "User not found" });

        var updatedUser = new User { Id = id, Name = body.Name };
        _users[id] = updatedUser;
        return Ok(updatedUser);
    }

    // DELETE /api/users/{id}
    [HttpDelete("/api/users/{id}")]
    public IActionResult Delete(int id)
    {
        if (_users.TryRemove(id, out _))
        {
            return Ok(new { message = "User deleted" });
        }

        return NotFound(new { error = "User not found" });
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public record CreateUserDto(string Name);