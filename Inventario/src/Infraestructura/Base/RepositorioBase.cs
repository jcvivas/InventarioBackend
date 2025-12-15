using Infraestructura.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Infraestructura.Base
{
    public class RepositorioBase
    {
        protected AppDbContext _context = null!;
        protected IConfiguration _config = null!;

        public void SetRepoInit(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
    }
}
