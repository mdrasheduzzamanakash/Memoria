using Memoria.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();

    }
}
