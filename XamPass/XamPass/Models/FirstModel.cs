using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models
{
    public class FirstModel
    {
        public Institution Institution { get; set; }

        public List<SelectListItem> Institutions { get; set; }
    }
}
