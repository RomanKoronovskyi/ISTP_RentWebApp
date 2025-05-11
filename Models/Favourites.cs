using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace RentAPIWebApp.Models
{
    public class Favourites
    {
        public int Id { get; set; }

        public int UsrId { get; set; }
        public int FlId { get; set; }
        [JsonIgnore]
        [BindNever]
        public virtual Users ?Usr { get; set; }

        [JsonIgnore]
        [BindNever]
        public virtual Flats ?Fl { get; set; }
    }

}