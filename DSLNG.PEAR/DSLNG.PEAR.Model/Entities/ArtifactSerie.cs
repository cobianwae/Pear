﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSLNG.PEAR.Data.Entities
{
    public class ArtifactSerie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<ArtifactStack> ArtifactStacks { get; set; }
        public string PeriodeType { get; set; }
        public string Aggregation { get; set; }
        public string Color { get; set; }
        public string RefTable { get; set; }
        public string RefId { get; set; }
        public bool IsActive { get; set; }
    }
}
