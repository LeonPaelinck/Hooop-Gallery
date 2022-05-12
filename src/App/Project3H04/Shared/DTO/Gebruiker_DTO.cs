using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Shared.DTO
{
    public class Gebruiker_DTO
    {
        public int GebruikerId { get; set; }
        public string Gebruikersnaam { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public string Email { get; set; }
        public DateTime DatumCreatie { get; set; }
        public string Fotopad { get; set; }
        public string Details { get; set; }

        public class Validator : AbstractValidator<Gebruiker_DTO>
        {
            public Validator()
            {
                RuleFor(x => x.Gebruikersnaam).NotEmpty().OverridePropertyName("Username");
                RuleFor(x => x.GeboorteDatum).NotEmpty().OverridePropertyName("Birthdate");
                RuleFor(x => x.Email).NotEmpty().OverridePropertyName("Email");
                RuleFor(x => x.Details).NotEmpty();
                RuleFor(x => x.GeboorteDatum).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Cannot use a future date.");
                //RuleFor(x=>x.Fotopad).NotEmpty().OverridePropertyName("Images");
            }
        }
    }
}
