using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
	public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext<IdentityUser>(options)
	{
	}
}