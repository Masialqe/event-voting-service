using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.VoterEvents;
using NSubstitute;
using Xunit;

namespace EVS.Tests.Domain.VoterEvent;

// public class VoterEventServiceTests
// {
//     private readonly IEventRepository _eventRepository;
//     private readonly IVoterEventRepository _voterEventRepository;
//     private readonly IVoterRepository _voterRepository;
//     private readonly VoterEventService _voterEventService;
//     
//     public VoterEventServiceTests()
//     {
//         _eventRepository = Substitute.For<IEventRepository>();
//         _voterEventRepository = Substitute.For<IVoterEventRepository>();
//         _voterRepository = Substitute.For<IVoterRepository>();
//         _voterEventService = new VoterEventService(_voterEventRepository, _voterRepository, _eventRepository);
//     }
// }