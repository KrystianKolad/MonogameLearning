using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonogameLearning.Engine.Objects
{
    public interface IGameObjectWithDamage
    {
        int Damage { get; }
    }
}