﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SelfCare.Models;

public partial class Gp
{
    public int Gpid { get; set; }

    public string Name { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string Address3 { get; set; }

    public string Town { get; set; }

    public string County { get; set; }

    public string PostCode { get; set; }

    public string Country { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}