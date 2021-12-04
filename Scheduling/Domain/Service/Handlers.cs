using System;
using Scheduling.Domain.DoctorDay;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Infrastructure.Commands;

namespace Scheduling.Domain.Service;

public class Handlers : CommandHandler
{
    private readonly IDayRepository _repository;

    public Handlers(IDayRepository repository)
    {
        _repository = repository;

        Register<ScheduleDay>(async (c, m) =>
        {
            var day = await _repository.Get(new DayId(new DoctorId(c.DoctorId), c.Date));
            day.Schedule(new DoctorId(c.DoctorId), c.Date, c.Slots, Guid.NewGuid);
            await _repository.Save(day, m);
        });

        Register<ScheduleSlot>(async (c, m) =>
        {
            var day = await _repository.Get(new DayId(new DoctorId(c.DoctorId), c.Date));
            day.ScheduleSlot(c.SlotId, c.StartTime, c.Duration);
            await _repository.Save(day, m);
        });

        Register<BookSlot>(async (c, m) =>
        {
            var day = await _repository.Get(new DayId(c.DayId));
            day.BookSlot(new SlotId(c.SlotId), new PatientId(c.PatientId));
            await _repository.Save(day, m);
        });

        Register<CancelSlotBooking>(async (c, m) =>
        {
            var day = await _repository.Get(new DayId(c.DayId));
            day.CancelSlotBooking(c.SlotId, c.Reason);
            await _repository.Save(day, m);
        });

        Register<CancelDaySchedule>(async (c, m) =>
        {
            var day = await _repository.Get(new DayId(c.DayId));
            day.Cancel();
            await _repository.Save(day, m);
        });
    }
}