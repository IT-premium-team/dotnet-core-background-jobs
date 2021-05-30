using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundTest.Services.ManagerChangeService
{
    public interface IManagerChangeService
    {
        Task ApplyChangesAsync();
    }
}
