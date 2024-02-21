﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SelfCare.Models;

public partial class Practitioner
{
    public int PractitionerId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int ProductId { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ProductKey Product { get; set; }

    public virtual User User { get; set; }
}