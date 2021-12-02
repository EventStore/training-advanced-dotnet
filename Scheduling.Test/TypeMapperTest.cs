using System;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Scheduling.Test;

[Collection("TypeMapper collection")]
public class TypeMapperTest
{
    // [Fact]
    // public void check_slot_booking_cancelled_correctly_maps_with_default_value()
    // {
    //     EventMappings.MapEventTypes();
    //
    //     var mapper = TypeMapper.GetDataToType("doctorday-slot-booking-cancelled");
    //
    //     var slotId = Guid.NewGuid();
    //     var data = new JObject
    //     {
    //         ["dayId"] = "dayId",
    //         ["slotId"] = slotId.ToString(),
    //         ["reason"] = "reason",
    //
    //     };
    //
    //     var slotBookingCancelled = mapper(data) as SlotBookingCancelled;
    //
    //     Assert.NotNull(slotBookingCancelled);
    //     Assert.IsType<SlotBookingCancelled>(slotBookingCancelled);
    //     Assert.Equal(new SlotBookingCancelled("dayId", slotId, "reason", "unknown request"), slotBookingCancelled);
    // }
    //
    // [Fact]
    // public void check_slot_booking_cancelled_correctly_maps_with_value_present()
    // {
    //     EventMappings.MapEventTypes();
    //
    //     var mapper = TypeMapper.GetDataToType("doctorday-slot-booking-cancelled");
    //
    //     var slotId = Guid.NewGuid();
    //     var data = new JObject
    //     {
    //         ["dayId"] = "dayId",
    //         ["slotId"] = slotId.ToString(),
    //         ["reason"] = "reason",
    //         ["requestedBy"] = "doctor"
    //
    //     };
    //
    //     var slotBookingCancelled = mapper(data) as SlotBookingCancelled;
    //
    //     Assert.NotNull(slotBookingCancelled);
    //     Assert.IsType<SlotBookingCancelled>(slotBookingCancelled);
    //     Assert.Equal(new SlotBookingCancelled("dayId", slotId, "reason", "doctor"), slotBookingCancelled);
    // }
}