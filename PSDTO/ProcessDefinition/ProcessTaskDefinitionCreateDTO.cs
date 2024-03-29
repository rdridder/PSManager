﻿using System.ComponentModel.DataAnnotations;

namespace PSDTO.ProcessDefinition
{
    public class ProcessTaskDefinitionCreateDTO
    {
        public ProcessTaskDefinitionCreateDTO(string name, string description, string key, bool isEnabled, string processTaskType)
        {
            Name = name;
            Description = description;
            Key = key;
            IsEnabled = isEnabled;
            ProcessTaskType = processTaskType;
        }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(32)]
        public string Key { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public string ProcessTaskType { get; set; }
    }
}
