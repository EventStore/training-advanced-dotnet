using System;
using Scheduling.Domain.DoctorDay.Events;

namespace Scheduling.Test.Test;

public class TypeMapperFixture : IDisposable
{
    public TypeMapperFixture()
    {
        EventMappings.MapEventTypes();
    }

    public void Dispose()
    {
    }
}