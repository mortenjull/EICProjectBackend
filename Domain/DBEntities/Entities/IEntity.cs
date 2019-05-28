using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime Created { get; set; }
    }
}
