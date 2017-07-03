using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Models
{
	// Represents the access to DB itself 
	// Is used for interractions with DB by using LINQ query
    public class WorldContext : DbContext
    {
		// Config file
		private IConfigurationRoot _config;

		// Construction injection
		// Because of using OnConfiguring(), we have to add another object DbContextOptions options
		public WorldContext(IConfigurationRoot config, DbContextOptions options) : base(options)
	    {
		    _config = config;
	    }

		// Starting point for te queriable interface
	    public DbSet<Trip> Trips { get; set; }
	    public DbSet<Stop> Stops { get; set; }

		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	    {
		    base.OnConfiguring(optionsBuilder);

			// Use SQL Server package for SQL Engine with connection string
		    optionsBuilder.UseSqlServer(_config["ConnectionStrings:WorldContextConnection"]);
	    }
    }
}
