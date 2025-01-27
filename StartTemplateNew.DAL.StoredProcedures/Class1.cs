using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;

namespace StartTemplateNew.DAL.StoredProcedures
{
    public readonly struct StoredProcedureParameter
    {
        public string Name { get; }
        public object Value { get; }

        public StoredProcedureParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public class DbContextStoredProcedures : ApplicationDbContext
    {
        public DbContextStoredProcedures(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public IQueryable<TModel> ExecuteStoredProcedure<TModel>(string storedProcedureName)
        {
            return Database.SqlQuery<TModel>($"EXEC {storedProcedureName}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "<Pending>")]
        public IQueryable<TModel> ExecuteStoredProcedureWithParameters<TModel>(string storedProcedureName, params StoredProcedureParameter[] parameters)
        {
            SqlParameter[] sqlParameters =
                parameters.Select(p => new SqlParameter(p.Name, p.Value)).ToArray();

            return Database.SqlQueryRaw<TModel>($"EXEC {storedProcedureName}", sqlParameters);
        }
    }
}
