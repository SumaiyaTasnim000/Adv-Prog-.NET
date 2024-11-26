using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelevisionManagement.DTOs
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "Program Name is required.")]
        [MaxLength(150, ErrorMessage = "Program Name cannot exceed 150 characters.")]
        public string ProgramName { get; set; }

        [Required(ErrorMessage = "TRP Score is required.")]
        [Range(0.0, 10.0, ErrorMessage = "TRP Score must be between 0.0 and 10.0.")]
        public decimal TRPScore { get; set; }

        [Required(ErrorMessage = "Channel ID is required.")]
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "Air Time is required.")]
        [DataType(DataType.Time, ErrorMessage = "Air Time must be in HH:mm:ss format.")]
        public TimeSpan AirTime { get; set; }

    }
}