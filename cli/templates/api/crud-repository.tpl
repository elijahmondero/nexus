using System.Data;
using Dapper;
using Nexus.Api.Data;
using {{Namespace}}.Models;

namespace {{Namespace}}.Repositories;

public class {{Name}}Repository
{
    private readonly DbConnectionFactory _db;

    public {{Name}}Repository(DbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Models.{{Name}}>> GetAllAsync()
    {
        using var db = _db.CreateConnection();
        return await db.QueryAsync<Models.{{Name}}>("SELECT * FROM {{TableName}}");
    }

    public async Task<Models.{{Name}}?> GetByIdAsync(Guid id)
    {
        using var db = _db.CreateConnection();
        return await db.QueryFirstOrDefaultAsync<Models.{{Name}}>("SELECT * FROM {{TableName}} WHERE id = @id", new { id });
    }

    public async Task CreateAsync(Models.{{Name}} model)
    {
        using var db = _db.CreateConnection();
        await db.ExecuteAsync("INSERT INTO {{TableName}} (id, name) VALUES (@Id, @Name)", model);
    }
}
