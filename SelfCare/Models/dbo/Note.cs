﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SelfCare.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public int PatientId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public DateTime? DateUpdated { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Patient Patient { get; set; }
}