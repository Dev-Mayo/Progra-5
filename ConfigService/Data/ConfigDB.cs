using System.Data;
using ConfigService.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConfigService.Data
{
    public class ConfigDb
    {
        private readonly string _cs;
        public ConfigDb(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("ConfigDb")
                ?? throw new InvalidOperationException("ConnectionStrings:ConfigDb no configurada.");
        }

        public async Task<IEnumerable<ParametroDto>> GetAllAsync()
        {
            const string sql = @"SELECT ParametroId, Valor, Descripcion, UpdatedAtUtc FROM dbo.Parametros ORDER BY ParametroId;";
            await using var cn = new SqlConnection(_cs);
            var rows = await cn.QueryAsync(sql);
            return rows.Select(r => new ParametroDto
            {
                ParametroId = (string)r.ParametroId,
                Valor = (string)r.Valor,
                Descripcion = r.Descripcion as string,
                UpdatedAtUtc = DateTime.SpecifyKind((DateTime)r.UpdatedAtUtc, DateTimeKind.Utc)
            });
        }

        public async Task<ParametroDto?> GetByIdAsync(string id)
        {
            const string sql = @"SELECT ParametroId, Valor, Descripcion, UpdatedAtUtc FROM dbo.Parametros WHERE ParametroId = @id;";
            await using var cn = new SqlConnection(_cs);
            var r = await cn.QueryFirstOrDefaultAsync(sql, new { id });
            if (r == null) return null;
            return new ParametroDto
            {
                ParametroId = (string)r.ParametroId,
                Valor = (string)r.Valor,
                Descripcion = r.Descripcion as string,
                UpdatedAtUtc = DateTime.SpecifyKind((DateTime)r.UpdatedAtUtc, DateTimeKind.Utc)
            };
        }

        public async Task<bool> ExistsAsync(string id)
        {
            const string sql = @"SELECT 1 FROM dbo.Parametros WHERE ParametroId = @id;";
            await using var cn = new SqlConnection(_cs);
            var v = await cn.ExecuteScalarAsync<int?>(sql, new { id });
            return v.HasValue;
        }

        public async Task CreateAsync(ParametroCreateDto dto)
        {
            const string sql = @"
INSERT INTO dbo.Parametros (ParametroId, Valor, Descripcion)
VALUES (@ParametroId, @Valor, @Descripcion);";
            await using var cn = new SqlConnection(_cs);
            await cn.ExecuteAsync(sql, new
            {
                ParametroId = dto.ParametroId,
                Valor = dto.Valor,
                Descripcion = (object?)dto.Descripcion ?? DBNull.Value
            });
        }

        public async Task<int> UpdateAsync(string id, ParametroUpdateDto dto)
        {
            const string sql = @"
UPDATE dbo.Parametros
SET Valor = @Valor,
    Descripcion = @Descripcion,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE ParametroId = @id;";
            await using var cn = new SqlConnection(_cs);
            return await cn.ExecuteAsync(sql, new
            {
                id,
                Valor = dto.Valor,
                Descripcion = (object?)dto.Descripcion ?? DBNull.Value
            });
        }

        public async Task<int> DeleteAsync(string id)
        {
            const string sql = @"DELETE FROM dbo.Parametros WHERE ParametroId = @id;";
            await using var cn = new SqlConnection(_cs);
            return await cn.ExecuteAsync(sql, new { id });
        }
    }
}