﻿using System.ComponentModel.DataAnnotations;
using EVS.App.Domain.Events;

namespace EVS.App.Components.Events;

public partial class CreateEventForm
{
    private sealed class InputModel
    {
        [Required] [MinLength(5)] public string Name { get; set; } = string.Empty;
        [Required] [MinLength(10)] public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Player limit must fit between 1-100.")]
        public int PlayerLimit { get; set; } = 0;
        [Required] public EventTypes Types { get; set; } = default;
    }
}