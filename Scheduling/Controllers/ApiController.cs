using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.Commands;

namespace Scheduling.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly Dispatcher _dispatcher;

    private readonly IAvailableSlotsRepository _availableSlotsRepository;

    private readonly IEventStore _eventStore;

    public ApiController(Dispatcher dispatcher,
        IAvailableSlotsRepository availableSlotsRepository,
        IEventStore eventStore)
    {
        _dispatcher = dispatcher;
        _availableSlotsRepository = availableSlotsRepository;
        _eventStore = eventStore;
    }

    [HttpGet]
    [Route("slots/today/available")]
    public async Task<List<AvailableSlotsResponse>> GetAvailableSlotsToday()
    {
        var availableSlots = await _availableSlotsRepository.GetAvailableSlotsOn(new DateTime(2020, 8, 1));
        return availableSlots
            .Select(a => new AvailableSlotsResponse(a.DayId, a.Id, a.Date, a.StartTime, a.Duration))
            .ToList();
    }

    [HttpGet]
    [Route("slots/{date}/available")]
    public async Task<List<AvailableSlotsResponse>> GetAvailableSlotsToday(string date)
    {
        var availableSlots = await _availableSlotsRepository.GetAvailableSlotsOn(DateTime.Parse(date));
        return availableSlots
            .Select(a => new AvailableSlotsResponse(a.DayId, a.Id, a.Date, a.StartTime, a.Duration))
            .ToList();;
    }

    [HttpPost]
    [Route("doctor/schedule")]
    public async Task<IActionResult> ScheduleDay([FromBody] ScheduleDayRequest scheduleDay)
    {
        var command = scheduleDay.ToCommand();
        var metadata = GetCommandMetadata();
        await _dispatcher.Dispatch(command, metadata);
        return Created($"/api/slots/{command.Date}/available", null);
    }

    [HttpPost]
    [Route("slots/{dayId}/cancel-booking")]
    public async Task<IActionResult> CancelBooking(string dayId,
        [FromBody] CancelSlotBookingRequest cancelSlotBooking)
    {
        var command = cancelSlotBooking.ToCommand(dayId);
        var metadata = GetCommandMetadata();
        await _dispatcher.Dispatch(command, metadata);
        return Ok();
    }

    [HttpPost]
    [Route("slots/{dayId}/book")]
    public async Task<IActionResult> BookSlot(string dayId, [FromBody] BookSlotRequest bookSlot)
    {
        var command = bookSlot.ToCommand(dayId);
        var metadata = GetCommandMetadata();

        await _dispatcher.Dispatch(command, metadata);

        return Ok();
    }

    [HttpPost]
    [Route("calendar/{date}/day-started")]
    public async Task<IActionResult> CalendarDayStarted(string date)
    {
        var commandMetadata = GetCommandMetadata();

        await _eventStore.AppendEvents("doctorday-time-events", commandMetadata, new CalendarDayStarted(DateTime.Parse(date)));

        return Ok();
    }

    private CommandMetadata GetCommandMetadata()
    {
        if (!Request.Headers.TryGetValue("X-CorrelationId", out var correlationId))
        {
            throw new ArgumentNullException(nameof(correlationId), "please provide an X-CorrelationId header");
        }

        if (!Request.Headers.TryGetValue("X-CausationId", out var causationId))
        {
            throw new ArgumentNullException(nameof(causationId), "please provide an X-CausationId header");
        }

        return new CommandMetadata(
            new CorrelationId(Guid.Parse(correlationId.First())),
            new CausationId(Guid.Parse(causationId.First())));
    }
}