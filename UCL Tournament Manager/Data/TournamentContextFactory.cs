using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace UCL_Tournament_Manager.Data;

public class TournamentContextFactory : IDesignTimeDbContextFactory<TournamentContext>
{
    public TournamentContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TournamentContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UCL_Tournament;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;");

        return new TournamentContext(optionsBuilder.Options);
    }
}