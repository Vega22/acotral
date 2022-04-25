using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.Extensions.Primitives;

#nullable disable

namespace Acotral.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public byte[] Images { get; set; }
        public DateTime? Dates { get; set; }
        [Required(ErrorMessage ="Por favor ingrese el Titulo.")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Por favor ingrese el cuerpo de la noticia.")]
        public string Body { get; set; }
        public bool Visible { get; set; }
    }
}
