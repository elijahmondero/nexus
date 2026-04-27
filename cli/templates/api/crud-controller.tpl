using Microsoft.AspNetCore.Mvc;
using {{Namespace}}.Models;
using {{Namespace}}.Repositories;

namespace {{Namespace}}.Controllers;

[ApiController]
[Route("api/[controller]")]
public class {{Name}}Controller : ControllerBase
{
    private readonly {{Name}}Repository _repository;

    public {{Name}}Controller({{Name}}Repository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item != null ? Ok(item) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create({{Name}} model)
    {
        model.Id = Guid.NewGuid();
        await _repository.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }
}
