using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISettingsService
    {
        Settings Get();
        void Update(Settings settings);
    }
}
