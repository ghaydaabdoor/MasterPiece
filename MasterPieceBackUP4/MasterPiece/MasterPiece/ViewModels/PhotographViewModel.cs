using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterPiece.Models
{
    public class PhotographViewModel
    {
        public Photographer photographer { get; set; }
        public PhotographerDetail photographerDetail { get; set; }
    }
}