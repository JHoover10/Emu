using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu;

public class GameData
{
    public string Name { get; set; }
    public List<string> ConfigPaths { get; set; }
    public List<string> SaveGameDataPaths { get; set; }
}
