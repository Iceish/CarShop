using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels;

public class VehicleModelApiModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Brand { get; set; }

    public int MaintenanceFrequency { get; set; }
}
